Imports System.Data.SqlClient
Imports System.Windows.Forms.DataVisualization.Charting
Imports JJAS_ISBST.FrmLogin

Public Class Admin_Home
    Dim formtoshow As Form
    Private _sessionTimer As Timer

    Private Sub Admin_Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            lblUserName.Text = "Welcome " & FrmLogin.CurrentUser.FullName

            Select Case FrmLogin.CurrentUser.Role.ToLower()
                Case "cashier"
                    btnFileMaintenance.Visible = False
                    btnDelivery.Visible = False
                    btnAuditTrail.Visible = False
                    btnAuditTrail.Image = Nothing
                    btnInventory.Visible = False

                Case "staff"
                    btnPos.Visible = False
                    btnAuditTrail.Visible = False
                    btnAuditTrail.Image = Nothing
                    btnTransaction.Visible = False
            End Select

            RearrangeButtons(PanelMenu)
            LoadSalesChart("Week")
            LoadCategoryChart("Week")
            LoadDashboardStats() ' 🧮 Load panels

        Catch ex As Exception
            MessageBox.Show("An error occurred while loading the dashboard." & vbCrLf &
                            ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub StartSessionHeartbeat()
        ' Heartbeat every 60 seconds to keep session fresh and detect expiry/kick.
        _sessionTimer = New Timer()
        _sessionTimer.Interval = 60000
        AddHandler _sessionTimer.Tick, AddressOf SessionHeartbeat_Tick
        _sessionTimer.Start()
    End Sub

    Private Sub SessionHeartbeat_Tick(sender As Object, e As EventArgs)
        If Not SessionService.Heartbeat() Then
            Try
                MessageBox.Show("Your session has expired. Please login again.", "Session Expired", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch
            End Try

            Try
                SessionService.EndCurrentSession("Expired")
            Catch
            End Try

            ' Clear current user info
            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""

            Me.Hide()
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub


    ' 🧮 DASHBOARD SUMMARY PANELS
    Private Sub LoadDashboardStats()
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()

                ' 1️⃣ Sales Today
                Dim sqlSalesToday As String = "
                SELECT ISNULL(SUM(TotalAmount), 0)
                FROM tbl_SalesHistory
                WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)"
                Using cmd As New SqlCommand(sqlSalesToday, conn)
                    lblSalesToday.Text = "₱" & Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2")
                    AdjustSmartLabel(lblSalesToday)
                End Using

                ' 2️⃣ Transactions Today
                Dim sqlTransactions As String = "
                SELECT COUNT(*)
                FROM tbl_SalesHistory
                WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)"
                Using cmd As New SqlCommand(sqlTransactions, conn)
                    lblTransactions.Text = cmd.ExecuteScalar().ToString()
                    AdjustSmartLabel(lblTransactions)
                End Using

                ' 3️⃣ Low Stock Items
                Dim sqlLowStock As String = "
                SELECT COUNT(*)
                FROM tbl_Delivery_Products
                WHERE status = 'Posted' AND Quantity < 5"
                Using cmd As New SqlCommand(sqlLowStock, conn)
                    lblLowStock.Text = cmd.ExecuteScalar().ToString()
                    AdjustSmartLabel(lblLowStock)
                End Using

                ' 4️⃣ Profit Today
                Dim sqlProfit As String = "
                SELECT ISNULL(SUM((SellingPrice - CostPrice) * Quantity), 0)
                FROM tbl_SalesHistory
                WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)"
                Using cmd As New SqlCommand(sqlProfit, conn)
                    lblProfitToday.Text = "₱" & Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2")
                    AdjustSmartLabel(lblProfitToday)
                End Using

                ' 5️⃣ Active Users
                Dim sqlActiveUsers As String = "
                SELECT COUNT(*)
                FROM tbl_User
                WHERE isActive = 1"
                Using cmd As New SqlCommand(sqlActiveUsers, conn)
                    lblActiveUSer.Text = cmd.ExecuteScalar().ToString()
                    AdjustSmartLabel(lblActiveUSer)
                End Using

                ' 6️⃣ Active Discounts
                Dim sqlDiscount As String = "
                SELECT COUNT(*)
                FROM tbl_Discount
                WHERE isActive = 1"
                Using cmd As New SqlCommand(sqlDiscount, conn)
                    lblDiscount.Text = cmd.ExecuteScalar().ToString()
                    AdjustSmartLabel(lblDiscount)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading dashboard stats: " & ex.Message,
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 🪄 Rearrange visible buttons on sidebar
    Private Sub RearrangeButtons(panel As Panel)
        Dim y As Integer = 10
        For Each ctrl As Control In panel.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Visible Then
                ctrl.Location = New Point(10, y)
                y += ctrl.Height + 10
            End If
        Next
    End Sub

    ' 🔁 Timer to switch forms smoothly
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub

    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        Dim role As String = FrmLogin.CurrentUser.Role.ToLower()

        If role = "staff" Then
            formtoshow = New admin_category()
        Else
            formtoshow = New Admin_User()
        End If

        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        formtoshow = New Admin_Pos()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                "Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' 🧾 Log audit trail

            Try
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    SessionService.EndCurrentSession("Logout")
                End Using
            Catch ex As Exception
                MsgBox("Error logging out: " & ex.Message)
            End Try


            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

            ' Clear current user info
            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show Login form again
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub

    ' ========== CHARTS AND DASHBOARD LOGIC ==========

    ' 🧭 Sales Chart by Week/Month/Year
    Private Sub LoadSalesChart(timeFrame As String)
        Try
            chartSales.Series.Clear()
            chartSales.Titles.Clear()
            chartSales.ChartAreas.Clear()
            chartSales.ChartAreas.Add("SalesArea")

            Dim series As New Series("Sales")
            series.ChartType = SeriesChartType.Column
            series.Color = Color.SkyBlue
            series.IsValueShownAsLabel = True

            Dim sql As String = ""
            Select Case timeFrame
                Case "Week"
                    sql = "SELECT FORMAT(SaleDate, 'ddd') AS Period, SUM(TotalAmount) AS SalesTotal
                           FROM tbl_SalesHistory
                           WHERE SaleDate >= DATEADD(day, -7, GETDATE())
                           GROUP BY FORMAT(SaleDate, 'ddd')"
                Case "Month"
                    sql = "SELECT FORMAT(SaleDate, 'dd') AS Period, SUM(TotalAmount) AS SalesTotal
                           FROM tbl_SalesHistory
                           WHERE MONTH(SaleDate) = MONTH(GETDATE())
                           GROUP BY FORMAT(SaleDate, 'dd')"
                Case "Year"
                    sql = "SELECT FORMAT(SaleDate, 'MMM') AS Period, SUM(TotalAmount) AS SalesTotal
                           FROM tbl_SalesHistory
                           WHERE YEAR(SaleDate) = YEAR(GETDATE())
                           GROUP BY FORMAT(SaleDate, 'MMM')"
            End Select

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                While rdr.Read()
                    series.Points.AddXY(rdr("Period").ToString(), Convert.ToDecimal(rdr("SalesTotal")))
                End While
            End Using

            chartSales.Series.Add(series)
            chartSales.Titles.Add("Sales by " & timeFrame)
            chartSales.Titles(0).Font = New Font("Segoe UI", 12, FontStyle.Bold)
            chartSales.Titles(0).ForeColor = Color.White
            chartSales.ChartAreas("SalesArea").BackColor = Color.Transparent

        Catch ex As Exception
            MessageBox.Show("Error loading Sales Chart: " & ex.Message,
                            "Chart Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 🥧 Category Pie Chart
    Private Sub LoadCategoryChart(timeFrame As String)
        Try
            chartCategory.Series.Clear()
            chartCategory.ChartAreas.Clear()
            chartCategory.Titles.Clear()
            chartCategory.Legends.Clear()

            Dim area As New ChartArea("CategoryArea")
            chartCategory.ChartAreas.Add(area)

            Dim legend As New Legend("Default")
            legend.Docking = Docking.Bottom
            legend.Alignment = StringAlignment.Center
            chartCategory.Legends.Add(legend)

            Dim series As New Series("CategorySales")
            series.ChartType = SeriesChartType.Pie
            series.Legend = "Default"
            series.IsValueShownAsLabel = True
            series.LabelFormat = "₱{0:N2}"
            series.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            series("PieLabelStyle") = "Outside"
            series("PieDrawingStyle") = "Concave"

            Dim query As String = ""
            Select Case timeFrame
                Case "Week"
                    query = "
                    SELECT Category, SUM(Quantity * SellingPrice) AS TotalSales
                    FROM tbl_SalesHistory
                    WHERE SaleDate >= DATEADD(day, -7, GETDATE())
                    GROUP BY Category
                    ORDER BY TotalSales DESC"
                Case "Month"
                    query = "
                    SELECT Category, SUM(Quantity * SellingPrice) AS TotalSales
                    FROM tbl_SalesHistory
                    WHERE MONTH(SaleDate) = MONTH(GETDATE())
                    GROUP BY Category
                    ORDER BY TotalSales DESC"
                Case "Year"
                    query = "
                    SELECT Category, SUM(Quantity * SellingPrice) AS TotalSales
                    FROM tbl_SalesHistory
                    WHERE YEAR(SaleDate) = YEAR(GETDATE())
                    GROUP BY Category
                    ORDER BY TotalSales DESC"
                Case Else
                    query = "
                    SELECT Category, SUM(Quantity * SellingPrice) AS TotalSales
                    FROM tbl_SalesHistory
                    GROUP BY Category
                    ORDER BY TotalSales DESC"
            End Select

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(query, conn)
                Dim rdr As SqlDataReader = cmd.ExecuteReader()

                While rdr.Read()
                    series.Points.AddXY(rdr("Category").ToString(), Convert.ToDecimal(rdr("TotalSales")))
                End While
            End Using

            If series.Points.Count > 0 Then
                series.Points(0)("Exploded") = "True"
            End If

            chartCategory.Series.Add(series)
            chartCategory.Titles.Add("Top Categories by Sales (" & timeFrame & ")")
            chartCategory.Titles(0).Font = New Font("Segoe UI", 12, FontStyle.Bold)
            chartCategory.Titles(0).ForeColor = Color.White

            chartCategory.Palette = ChartColorPalette.SeaGreen
            chartCategory.ChartAreas("CategoryArea").BackColor = Color.Transparent
            chartCategory.ChartAreas("CategoryArea").AxisX.LabelStyle.ForeColor = Color.White
            chartCategory.ChartAreas("CategoryArea").AxisY.LabelStyle.ForeColor = Color.White

        Catch ex As Exception
            MessageBox.Show("Error loading Category Chart: " & ex.Message,
                            "Chart Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 🧩 Smart label adjustment — shrinks font and adjusts width a little bit
    Private Sub AdjustSmartLabel(lbl As Label)
        Try
            Dim g As Graphics = lbl.CreateGraphics()
            Dim originalFont As Font = lbl.Font
            Dim fontSize As Single = originalFont.Size
            Dim maxWidth As Integer = lbl.Width

            Dim textWidth As Single = g.MeasureString(lbl.Text, lbl.Font).Width
            While textWidth > maxWidth AndAlso fontSize > 6
                fontSize -= 0.5
                textWidth = g.MeasureString(lbl.Text, New Font(originalFont.FontFamily, fontSize)).Width
            End While

            lbl.Width = CInt(Math.Max(textWidth + 10, maxWidth))
            lbl.Font = New Font(originalFont.FontFamily, fontSize)
            lbl.Left = (lbl.Parent.Width - lbl.Width) \ 2
            g.Dispose()

        Catch ex As Exception
            ' optional: just skip silently to avoid UI freeze
        End Try
    End Sub

    ' ========== FILTER BUTTONS ==========
    Private Sub btnWeekly_Click(sender As Object, e As EventArgs) Handles btnWeekly.Click
        LoadSalesChart("Week")
        LoadCategoryChart("Week")
    End Sub

    Private Sub btnMontly_Click(sender As Object, e As EventArgs) Handles btnMontly.Click
        LoadSalesChart("Month")
        LoadCategoryChart("Month")
    End Sub

    Private Sub btnyearly_Click(sender As Object, e As EventArgs) Handles btnyearly.Click
        LoadSalesChart("Year")
        LoadCategoryChart("Year")
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New Admin_AuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MsgBox("Error logging out: " & ex.Message)
        End Try


        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
