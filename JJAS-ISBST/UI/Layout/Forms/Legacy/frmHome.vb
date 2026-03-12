Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports Guna.UI2.WinForms

Public Class frmHome
    Private Const OuterPadding As Integer = 12
    Private Const SectionGap As Integer = 10
    Private Const HeaderPanelHeight As Integer = 52
    Private Const MinCardHeight As Integer = 165
    Private Const MinSalesInfoHeight As Integer = 150
    Private Const MinBottomHeight As Integer = 220

    Private ReadOnly _activeSidebarFillColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _inactiveSidebarFillColor As Color = Color.Black

    Private _dashboardStyled As Boolean
    Private _sessionTimer As Timer
    Private _currentRange As DashboardTimeRange = DashboardTimeRange.Today

    Private Enum DashboardTimeRange
        Today
        Week
        Month
        Year
    End Enum

    Private Sub frmHome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ApplyDashboardTheme()
            ApplySidebarUserPanel()
            ConfigureSidebarPermissions()
            SetActiveSidebarButton(btnHome)
            LayoutDashboard()

            lblUserName.Text = "Name: " & If(FrmLogin.CurrentUser.FullName, String.Empty)

            LoadDashboardStats()
            LoadLowStockItems()
            ApplyTimeRange(DashboardTimeRange.Today)
            StartSessionHeartbeat()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading the dashboard." & vbCrLf & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplySidebarUserPanel()
        lblFirstname.Text = ResolveSidebarFirstName()
        lblUserLevel.Text = ResolveSidebarRoleDisplay()
    End Sub

    Private Sub frmHome_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Not IsHandleCreated Then Return
        LayoutDashboard()
    End Sub

    Private Sub frmHome_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        StopSessionHeartbeat()
    End Sub

    Private Sub ApplyDashboardTheme()
        If _dashboardStyled Then Return

        BackColor = Color.FromArgb(45, 47, 45)

        StyleSurfacePanel(Guna2Panel1)
        StyleSurfacePanel(Guna2Panel5)
        StyleSurfacePanel(Guna2Panel6)
        StyleSurfacePanel(Guna2Panel10)
        StyleSurfacePanel(Guna2Panel11)

        Guna2Panel3.BorderRadius = 15
        Guna2Panel3.FillColor = Color.FromArgb(92, 184, 92)
        Guna2Panel4.BorderRadius = 15
        Guna2Panel4.FillColor = Color.FromArgb(240, 173, 78)

        lblUserName.ForeColor = Color.White
        lblUserName.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)

        ConfigureSummaryCardLabels(lblTransactionsValue, lblTransactions, "Transactions Today")
        ConfigureSummaryCardLabels(lblActiveUserValue, lblActiveUser, "Active User")

        lblSalesToday.Text = "Sales Today:"
        lblProfittoday.Text = "Profit Today:"
        ConfigureStatLabel(lblSalesToday)
        ConfigureStatLabel(lblProfittoday)
        ConfigureStatValueLabel(lblSalesTodayValue)
        ConfigureStatValueLabel(lblProfitTodayValue)

        ConfigureFilterButton(btnToday, "Today")
        ConfigureFilterButton(btnWeekly, "Weekly")
        ConfigureFilterButton(btnMontly, "Monthly")
        ConfigureFilterButton(btnYearly, "Yearly")

        ConfigurePanelHeaderLabel(Guna2HtmlLabel1, "Low stock item")
        ConfigurePanelHeaderLabel(Guna2HtmlLabel2, "Top selling product")

        ConfigureActionButton(btnAdd)
        ConfigureGrid(dgvTable)
        ConfigureGrid(dtgTopSellingProduct)
        ConfigureSalesChart()

        _dashboardStyled = True
    End Sub

    Private Sub LayoutDashboard()
        If ClientSize.Width <= 0 OrElse ClientSize.Height <= 0 Then Return

        Dim contentLeft As Integer = panelMenu.Right + OuterPadding
        Dim contentTop As Integer = OuterPadding
        Dim contentWidth As Integer = ClientSize.Width - contentLeft - OuterPadding
        Dim contentHeight As Integer = ClientSize.Height - (OuterPadding * 2)

        If contentWidth <= 0 OrElse contentHeight <= 0 Then Return

        SuspendLayout()
        Try
            SetControlBounds(Guna2Panel1, contentLeft, contentTop, contentWidth, HeaderPanelHeight)
            lblUserName.Location = New Point(20, 14)

            Dim bodyHeight As Integer = contentHeight - HeaderPanelHeight - SectionGap
            Dim leftWidth As Integer = (contentWidth - SectionGap) \ 2
            Dim rightWidth As Integer = contentWidth - leftWidth - SectionGap

            Dim upperAreaHeight As Integer = Math.Max(0, bodyHeight - (SectionGap * 2))
            Dim cardHeight As Integer = Math.Max(MinCardHeight, CInt(upperAreaHeight * 0.3R))
            Dim salesInfoHeight As Integer = Math.Max(MinSalesInfoHeight, CInt(upperAreaHeight * 0.3R))
            Dim bottomHeight As Integer = upperAreaHeight - cardHeight - salesInfoHeight

            If bottomHeight < MinBottomHeight Then
                Dim deficit As Integer = MinBottomHeight - bottomHeight
                Dim cardReduction As Integer = Math.Min(Math.Max(0, cardHeight - MinCardHeight), (deficit + 1) \ 2)
                cardHeight -= cardReduction
                deficit -= cardReduction

                Dim infoReduction As Integer = Math.Min(Math.Max(0, salesInfoHeight - MinSalesInfoHeight), deficit)
                salesInfoHeight -= infoReduction

                bottomHeight = upperAreaHeight - cardHeight - salesInfoHeight
            End If

            If bottomHeight < MinBottomHeight Then
                bottomHeight = MinBottomHeight
            End If

            Dim rowTop As Integer = Guna2Panel1.Bottom + SectionGap
            Dim leftX As Integer = contentLeft
            Dim rightX As Integer = leftX + leftWidth + SectionGap

            Dim summaryCardGap As Integer = 10
            Dim firstCardWidth As Integer = (leftWidth - summaryCardGap) \ 2
            Dim secondCardWidth As Integer = leftWidth - firstCardWidth - summaryCardGap

            SetControlBounds(Guna2Panel3, leftX, rowTop, firstCardWidth, cardHeight)
            SetControlBounds(Guna2Panel4, leftX + firstCardWidth + summaryCardGap, rowTop, secondCardWidth, cardHeight)
            SetControlBounds(Guna2Panel6, leftX, Guna2Panel3.Bottom + SectionGap, leftWidth, salesInfoHeight)

            Dim lowStockHeight As Integer = cardHeight + SectionGap + salesInfoHeight
            SetControlBounds(Guna2Panel5, rightX, rowTop, rightWidth, lowStockHeight)

            Dim bottomTop As Integer = Guna2Panel6.Bottom + SectionGap
            SetControlBounds(Guna2Panel10, leftX, bottomTop, leftWidth, bottomHeight)
            SetControlBounds(Guna2Panel11, rightX, bottomTop, rightWidth, bottomHeight)

            LayoutSummaryCard(Guna2Panel3, lblTransactionsValue, lblTransactions)
            LayoutSummaryCard(Guna2Panel4, lblActiveUserValue, lblActiveUser)
            LayoutSalesAndProfitPanel()
            LayoutSalesChartPanel()
            LayoutLowStockPanel()
            LayoutTopSellingPanel()
        Finally
            ResumeLayout()
        End Try
    End Sub

    Private Shared Sub StyleSurfacePanel(targetPanel As Guna2Panel)
        targetPanel.BorderRadius = 15
        targetPanel.FillColor = Color.FromArgb(30, 31, 30)
    End Sub

    Private Shared Sub ConfigureSummaryCardLabels(valueLabel As Guna2HtmlLabel, captionLabel As Guna2HtmlLabel, captionText As String)
        valueLabel.AutoSize = False
        valueLabel.BackColor = Color.Transparent
        valueLabel.Font = New Font("Segoe UI", 44.0!, FontStyle.Regular, GraphicsUnit.Point)
        valueLabel.ForeColor = Color.White
        valueLabel.TextAlignment = ContentAlignment.BottomCenter

        captionLabel.AutoSize = False
        captionLabel.BackColor = Color.Transparent
        captionLabel.Font = New Font("Segoe UI", 18.0!, FontStyle.Bold, GraphicsUnit.Point)
        captionLabel.ForeColor = Color.White
        captionLabel.Text = captionText
        captionLabel.TextAlignment = ContentAlignment.TopCenter
    End Sub

    Private Shared Sub ConfigureStatLabel(label As Guna2HtmlLabel)
        label.AutoSize = False
        label.BackColor = Color.Transparent
        label.Font = New Font("Segoe UI", 22.0!, FontStyle.Bold, GraphicsUnit.Point)
        label.ForeColor = Color.White
        label.TextAlignment = ContentAlignment.MiddleLeft
    End Sub

    Private Shared Sub ConfigureStatValueLabel(valueLabel As Guna2HtmlLabel)
        valueLabel.AutoSize = False
        valueLabel.BackColor = Color.Transparent
        valueLabel.Font = New Font("Segoe UI", 20.0!, FontStyle.Bold, GraphicsUnit.Point)
        valueLabel.ForeColor = Color.White
        valueLabel.TextAlignment = ContentAlignment.MiddleCenter
    End Sub

    Private Shared Sub ConfigureFilterButton(filterButton As Guna2Button, text As String)
        filterButton.Text = text
        filterButton.Animated = True
        filterButton.AutoRoundedCorners = True
        filterButton.DefaultAutoSize = False
        filterButton.AutoSize = False
        filterButton.BorderRadius = 20
        filterButton.BorderThickness = 1
        filterButton.BorderColor = Color.FromArgb(68, 70, 68)
        filterButton.FillColor = Color.FromArgb(42, 44, 42)
        filterButton.HoverState.FillColor = Color.FromArgb(54, 56, 54)
        filterButton.HoverState.BorderColor = Color.FromArgb(92, 184, 92)
        filterButton.Font = New Font("Segoe UI", 10.25!, FontStyle.Bold)
        filterButton.ForeColor = Color.White
        filterButton.Padding = New Padding(12, 0, 12, 0)
        filterButton.Cursor = Cursors.Hand
    End Sub

    Private Shared Sub ConfigurePanelHeaderLabel(headerLabel As Guna2HtmlLabel, text As String)
        headerLabel.AutoSize = False
        headerLabel.BackColor = Color.Transparent
        headerLabel.Font = New Font("Segoe UI", 18.0!, FontStyle.Bold)
        headerLabel.ForeColor = Color.White
        headerLabel.Text = text
        headerLabel.TextAlignment = ContentAlignment.MiddleLeft
    End Sub

    Private Shared Sub ConfigureActionButton(actionButton As Guna2Button)
        actionButton.AutoRoundedCorners = True
        actionButton.BorderRadius = 24
        actionButton.FillColor = Color.FromArgb(50, 52, 50)
        actionButton.HoverState.FillColor = Color.FromArgb(75, 78, 75)
        actionButton.Font = New Font("Segoe UI", 14.25!, FontStyle.Bold)
        actionButton.ForeColor = Color.White
        actionButton.Text = " Add"
    End Sub

    Private Shared Sub ConfigureGrid(grid As Guna2DataGridView)
        grid.BackgroundColor = Color.White
        grid.BorderStyle = BorderStyle.FixedSingle
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersHeight = 36
        grid.RowHeadersVisible = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.ReadOnly = True
        grid.MultiSelect = False

        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 47, 45)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 47, 45)
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White

        grid.DefaultCellStyle.BackColor = Color.White
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(45, 47, 45)
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 228, 232)
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 31, 30)

        grid.ThemeStyle.BackColor = Color.White
        grid.ThemeStyle.GridColor = Color.FromArgb(215, 215, 215)
        grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(45, 47, 45)
        grid.ThemeStyle.HeaderStyle.ForeColor = Color.White
        grid.ThemeStyle.HeaderStyle.Height = 36
        grid.ThemeStyle.RowsStyle.BackColor = Color.White
        grid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(45, 47, 45)
        grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(225, 228, 232)
        grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(30, 31, 30)
        grid.ThemeStyle.RowsStyle.Height = 28
    End Sub

    Private Sub ConfigureSalesChart()
        chartSales.BackColor = Color.FromArgb(30, 31, 30)
        chartSales.BackGradientStyle = GradientStyle.None
        chartSales.Palette = ChartColorPalette.None
        chartSales.Titles.Clear()

        If chartSales.ChartAreas.Count = 0 Then
            chartSales.ChartAreas.Add(New ChartArea("ChartArea1"))
        End If

        Dim area As ChartArea = chartSales.ChartAreas(0)
        area.BackColor = Color.FromArgb(30, 31, 30)
        area.AxisX.LineColor = Color.FromArgb(95, 95, 95)
        area.AxisY.LineColor = Color.FromArgb(95, 95, 95)
        area.AxisX.LabelStyle.ForeColor = Color.White
        area.AxisY.LabelStyle.ForeColor = Color.White
        area.AxisX.MajorGrid.LineColor = Color.FromArgb(65, 65, 65)
        area.AxisY.MajorGrid.LineColor = Color.FromArgb(65, 65, 65)

        If chartSales.Legends.Count = 0 Then
            chartSales.Legends.Add(New Legend("Legend1"))
        End If

        Dim legend As Legend = chartSales.Legends(0)
        legend.BackColor = Color.Transparent
        legend.ForeColor = Color.White
        legend.Docking = Docking.Top
    End Sub

    Private Sub LayoutSummaryCard(card As Guna2Panel, valueLabel As Guna2HtmlLabel, captionLabel As Guna2HtmlLabel)
        Dim innerPadding As Integer = 8
        Dim valueHeight As Integer = Math.Max(88, CInt(card.Height * 0.52R))
        Dim fullWidth As Integer = Math.Max(0, card.Width - (innerPadding * 2))
        Dim captionHeight As Integer = Math.Max(0, card.Height - valueHeight - innerPadding)

        SetControlBounds(valueLabel, innerPadding, innerPadding, fullWidth, valueHeight)
        SetControlBounds(captionLabel, innerPadding, Math.Max(innerPadding, valueLabel.Bottom - 8), fullWidth, captionHeight)
    End Sub

    Private Sub LayoutSalesAndProfitPanel()
        Dim panelPadding As Integer = 18
        Dim innerWidth As Integer = Guna2Panel6.Width - (panelPadding * 2)
        Dim halfHeight As Integer = (Guna2Panel6.Height - (panelPadding * 2)) \ 2

        SetControlBounds(lblSalesToday, panelPadding, panelPadding - 2, innerWidth, 42)
        SetControlBounds(lblSalesTodayValue, panelPadding, panelPadding + 40, innerWidth, Math.Max(52, halfHeight - 30))

        SetControlBounds(lblProfittoday, panelPadding, panelPadding + halfHeight - 2, innerWidth, 42)
        SetControlBounds(lblProfitTodayValue, panelPadding, panelPadding + halfHeight + 40, innerWidth, Math.Max(52, halfHeight - 30))
    End Sub

    Private Sub LayoutSalesChartPanel()
        Dim panelPadding As Integer = 10
        Dim filterHeight As Integer = 44
        Dim buttonGap As Integer = 8
        Dim minButtonWidth As Integer = 86
        Dim maxButtonWidth As Integer = 118
        Dim availableWidth As Integer = Math.Max(0, Guna2Panel10.Width - (panelPadding * 2))
        Dim maxWidthPerButton As Integer = Math.Max(0, (availableWidth - (buttonGap * 3)) \ 4)
        Dim buttonWidth As Integer = Math.Max(minButtonWidth, Math.Min(maxButtonWidth, maxWidthPerButton))

        If (buttonWidth * 4) + (buttonGap * 3) > availableWidth Then
            buttonWidth = Math.Max(66, maxWidthPerButton)
        End If

        Dim totalButtonsWidth As Integer = (buttonWidth * 4) + (buttonGap * 3)
        Dim startX As Integer = Math.Max(panelPadding, Guna2Panel10.Width - panelPadding - totalButtonsWidth)

        SetControlBounds(btnToday, startX, panelPadding, buttonWidth, filterHeight)
        SetControlBounds(btnWeekly, btnToday.Right + buttonGap, panelPadding, buttonWidth, filterHeight)
        SetControlBounds(btnMontly, btnWeekly.Right + buttonGap, panelPadding, buttonWidth, filterHeight)
        SetControlBounds(btnYearly, btnMontly.Right + buttonGap, panelPadding, buttonWidth, filterHeight)

        Dim chartTop As Integer = btnToday.Bottom + 10
        SetControlBounds(chartSales, panelPadding + 6, chartTop, Guna2Panel10.Width - ((panelPadding + 6) * 2), Guna2Panel10.Height - chartTop - panelPadding)

        chartSales.SendToBack()
        btnToday.BringToFront()
        btnWeekly.BringToFront()
        btnMontly.BringToFront()
        btnYearly.BringToFront()
    End Sub

    Private Sub LayoutLowStockPanel()
        Dim panelPadding As Integer = 15
        Dim headerHeight As Integer = 72

        SetControlBounds(Guna2PictureBox1, panelPadding, 24, 24, 24)
        SetControlBounds(Guna2HtmlLabel1, Guna2PictureBox1.Right + 8, 12, 290, 52)

        btnAdd.Location = New Point(Math.Max(panelPadding, Guna2Panel5.Width - btnAdd.Width - panelPadding), 12)

        SetControlBounds(dgvTable, panelPadding, headerHeight, Guna2Panel5.Width - (panelPadding * 2), Guna2Panel5.Height - headerHeight - panelPadding)
    End Sub

    Private Sub LayoutTopSellingPanel()
        Dim panelPadding As Integer = 15
        Dim headerHeight As Integer = 72
        Dim headerTextWidth As Integer = Guna2Panel11.Width - (panelPadding * 2) - 32

        SetControlBounds(Guna2PictureBox2, panelPadding, 24, 24, 24)
        SetControlBounds(Guna2HtmlLabel2, Guna2PictureBox2.Right + 8, 12, headerTextWidth, 52)

        SetControlBounds(dtgTopSellingProduct, panelPadding, headerHeight, Guna2Panel11.Width - (panelPadding * 2), Guna2Panel11.Height - headerHeight - panelPadding)
    End Sub

    Private Shared Sub SetControlBounds(target As Control, x As Integer, y As Integer, width As Integer, height As Integer)
        target.SetBounds(Math.Max(0, x), Math.Max(0, y), Math.Max(0, width), Math.Max(0, height))
    End Sub

    Private Sub ConfigureSidebarPermissions()
        btnFileMaintenance.Visible = True
        btnDelivery.Visible = True
        btnInventory.Visible = True
        btnPos.Visible = True
        btnTransaction.Visible = True
        btnReturn.Visible = True
        btnAuditTrail.Visible = True

        Dim role As String = If(FrmLogin.CurrentUser.Role, String.Empty).ToLowerInvariant()
        Select Case role
            Case "cashier"
                btnFileMaintenance.Visible = False
                btnDelivery.Visible = False
                btnInventory.Visible = False
                btnReturn.Visible = False
                btnAuditTrail.Visible = False

            Case "staff"
                btnPos.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False
        End Select
    End Sub

    Private Sub SetActiveSidebarButton(activeButton As Guna2Button)
        For Each button As Guna2Button In GetSidebarNavigationButtons()
            If button Is Nothing Then Continue For

            Dim isActive As Boolean = button Is activeButton
            button.FillColor = If(isActive, _activeSidebarFillColor, _inactiveSidebarFillColor)
            button.HoverState.FillColor = _activeSidebarFillColor
        Next
    End Sub

    Private Iterator Function GetSidebarNavigationButtons() As IEnumerable(Of Guna2Button)
        Yield btnHome
        Yield btnFileMaintenance
        Yield btnDelivery
        Yield btnInventory
        Yield btnPos
        Yield btnTransaction
        Yield btnReturn
        Yield btnReports
        Yield btnAuditTrail
    End Function

    Private Sub OpenModule(nextForm As Form)
        If nextForm Is Nothing Then Return
        nextForm.Show()
        Me.Hide()
    End Sub

    Private Sub ApplyTimeRange(range As DashboardTimeRange)
        _currentRange = range
        SetActiveTimeButton(GetTimeFilterButton(range))
        LoadSalesChart(range)
        UpdateTopSellingHeader(range)
        LoadTopSellingProducts(range)
    End Sub

    Private Function GetTimeFilterButton(range As DashboardTimeRange) As Guna2Button
        Select Case range
            Case DashboardTimeRange.Today
                Return btnToday
            Case DashboardTimeRange.Week
                Return btnWeekly
            Case DashboardTimeRange.Month
                Return btnMontly
            Case DashboardTimeRange.Year
                Return btnYearly
            Case Else
                Return btnWeekly
        End Select
    End Function

    Private Sub SetActiveTimeButton(activeButton As Guna2Button)
        For Each filterButton As Guna2Button In New Guna2Button() {btnToday, btnWeekly, btnMontly, btnYearly}
            If filterButton Is Nothing Then Continue For

            Dim isActive As Boolean = filterButton Is activeButton
            filterButton.FillColor = If(isActive, Color.FromArgb(58, 60, 58), Color.FromArgb(42, 44, 42))
            filterButton.BorderColor = If(isActive, Color.FromArgb(92, 184, 92), Color.FromArgb(68, 70, 68))
            filterButton.BorderThickness = If(isActive, 2, 1)
            filterButton.HoverState.FillColor = If(isActive, Color.FromArgb(66, 68, 66), Color.FromArgb(54, 56, 54))
            filterButton.ForeColor = Color.White
        Next
    End Sub

    Private Sub UpdateTopSellingHeader(range As DashboardTimeRange)
        Guna2HtmlLabel2.Text = $"Top selling product ({GetTimeRangeText(range)})"
    End Sub

    Private Sub LoadDashboardStats()
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()

                Dim salesToday As Decimal = ExecuteScalarDecimal(conn,
                    "SELECT ISNULL(SUM(TotalAmount), 0) " &
                    "FROM tbl_SalesTransaction " &
                    "WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE);")

                Dim transactionsToday As Integer = ExecuteScalarInt(conn,
                    "SELECT COUNT(1) " &
                    "FROM tbl_SalesTransaction " &
                    "WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE);")

                Dim profitToday As Decimal = ExecuteScalarDecimal(conn,
                    "SELECT ISNULL(SUM((ISNULL(SellingPrice, 0) - ISNULL(CostPrice, 0)) * ISNULL(Quantity, 0)), 0) " &
                    "FROM tbl_SalesHistory " &
                    "WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE);")

                Dim activeUsers As Integer = ExecuteScalarInt(conn,
                    "SELECT COUNT(*) FROM tbl_User WHERE isActive = 1;")

                Dim peso As String = ChrW(&H20B1)
                lblSalesTodayValue.Text = $"{peso}{salesToday:N2}"
                lblTransactionsValue.Text = transactionsToday.ToString("N0")
                lblProfitTodayValue.Text = $"{peso}{profitToday:N2}"
                lblActiveUserValue.Text = activeUsers.ToString("N0")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading dashboard summary: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadLowStockItems()
        Const query As String =
            "SELECT TOP 100 " &
            "       p.ImagePath AS [ImagePath], " &
            "       p.Product AS [Product], " &
            "       p.BarcodeNumber AS [Barcode], " &
            "       dp.Quantity AS [Stock], " &
            "       ISNULL(d.OrderNumber, '') AS [Delivery #], " &
            "       dp.DateUpdated AS [Updated] " &
            "FROM tbl_Delivery_Products dp " &
            "INNER JOIN tbl_Products p ON p.ProductID = dp.ProductID " &
            "LEFT JOIN tbl_Deliveries d ON d.DeliveryID = dp.DeliveryID " &
            "WHERE dp.Status = 'Posted' AND dp.Quantity < 5 " &
            "ORDER BY dp.Quantity ASC, p.Product ASC;"

        Try
            Dim data As New DataTable()
            Using conn As SqlConnection = DataAccess.GetConnection()
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(data)
                End Using
            End Using

            ImageLoader.AddAndLoadImages(data, "ImagePath", "ProductImage")
            dgvTable.DataSource = data

            If dgvTable.Columns.Contains("Updated") Then
                dgvTable.Columns("Updated").DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
            End If

            ApplyProductImageColumnStyle(dgvTable)
            AlignGridCellsLeft(dgvTable)
            dgvTable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("Error loading low stock items: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadTopSellingProducts(range As DashboardTimeRange)
        Dim whereClause As String = BuildDateFilterClause(range, "sh.SaleDate")
        Dim query As String =
            "SELECT TOP 1 " &
            "       MAX(ISNULL(p.ImagePath, '')) AS [ImagePath], " &
            "       sh.ProductName AS [Product], " &
            "       SUM(ISNULL(sh.Quantity, 0)) AS [Qty Sold], " &
            "       SUM(ISNULL(sh.TotalAmount, 0)) AS [Sales], " &
            "       SUM((ISNULL(sh.SellingPrice, 0) - ISNULL(sh.CostPrice, 0)) * ISNULL(sh.Quantity, 0)) AS [Profit] " &
            "FROM tbl_SalesHistory sh " &
            "LEFT JOIN tbl_Products p ON p.ProductID = sh.ProductID OR ((sh.ProductID IS NULL OR sh.ProductID = 0) AND p.BarcodeNumber = sh.BarcodeNumber) " &
            "WHERE " & whereClause & " " &
            "GROUP BY sh.ProductName " &
            "ORDER BY SUM(ISNULL(sh.Quantity, 0)) DESC, SUM(ISNULL(sh.TotalAmount, 0)) DESC;"

        Try
            Dim data As New DataTable()
            Using conn As SqlConnection = DataAccess.GetConnection()
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(data)
                End Using
            End Using

            ImageLoader.AddAndLoadImages(data, "ImagePath", "ProductImage")
            dtgTopSellingProduct.DataSource = data

            If dtgTopSellingProduct.Columns.Contains("Sales") Then
                dtgTopSellingProduct.Columns("Sales").DefaultCellStyle.Format = "N2"
            End If

            If dtgTopSellingProduct.Columns.Contains("Profit") Then
                dtgTopSellingProduct.Columns("Profit").DefaultCellStyle.Format = "N2"
            End If

            ApplyProductImageColumnStyle(dtgTopSellingProduct)
            AlignGridCellsLeft(dtgTopSellingProduct)
            dtgTopSellingProduct.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("Error loading top selling products: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Shared Sub ApplyProductImageColumnStyle(grid As DataGridView)
        If grid Is Nothing Then Return

        If grid.Columns.Contains("ImagePath") Then
            grid.Columns("ImagePath").Visible = False
        End If

        Dim productImageColumn As DataGridViewColumn = Nothing
        If Not GridHelpers.TryGetColumn(grid, productImageColumn, "ProductImage") Then Return

        productImageColumn.HeaderText = "Image"
        productImageColumn.DisplayIndex = 0
        productImageColumn.Width = 120
        productImageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        productImageColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        Dim imgCol As DataGridViewImageColumn = TryCast(productImageColumn, DataGridViewImageColumn)
        If imgCol IsNot Nothing Then
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            imgCol.DefaultCellStyle.NullValue = GetMissingProductImagePlaceholder()
        End If

        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In grid.Columns
            If Not col.Visible Then Continue For

            If col.Name = "ProductImage" Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next
    End Sub

    Private Shared Sub AlignGridCellsLeft(grid As DataGridView)
        If grid Is Nothing Then Return

        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        For Each col As DataGridViewColumn In grid.Columns
            If Not col.Visible Then Continue For
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Next
    End Sub

    Private Sub LoadSalesChart(range As DashboardTimeRange)
        Dim query As String = BuildSalesChartQuery(range)
        Dim series As New Series("Sales") With {
            .ChartType = SeriesChartType.Column,
            .Color = Color.FromArgb(92, 184, 92),
            .BorderWidth = 2,
            .IsValueShownAsLabel = True,
            .LabelForeColor = Color.White
        }
        series("PointWidth") = "0.65"

        Try
            chartSales.Series.Clear()
            chartSales.Titles.Clear()

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim period As String = Convert.ToString(reader("Period"))
                            Dim amount As Decimal = SafeToDecimal(reader("SalesTotal"))
                            series.Points.AddXY(period, amount)
                        End While
                    End Using
                End Using
            End Using

            chartSales.Series.Add(series)

            Dim title As Title = chartSales.Titles.Add("Sales - " & GetTimeRangeText(range))
            title.Font = New Font("Segoe UI", 11.0!, FontStyle.Bold)
            title.ForeColor = Color.White
        Catch ex As Exception
            MessageBox.Show("Error loading sales chart: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function BuildSalesChartQuery(range As DashboardTimeRange) As String
        Dim weekStartExpr As String = GetCurrentWeekStartSqlDateExpression()

        Select Case range
            Case DashboardTimeRange.Today
                Return "SELECT FORMAT(SaleDate, 'HH') + ':00' AS Period, " &
                       "       SUM(ISNULL(TotalAmount, 0)) AS SalesTotal, " &
                       "       MIN(SaleDate) AS SortValue " &
                       "FROM tbl_SalesTransaction " &
                       "WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE) " &
                       "GROUP BY FORMAT(SaleDate, 'HH') " &
                       "ORDER BY SortValue;"

            Case DashboardTimeRange.Week
                Return "SELECT FORMAT(CAST(SaleDate AS DATE), 'ddd') AS Period, " &
                       "       SUM(ISNULL(TotalAmount, 0)) AS SalesTotal, " &
                       "       MIN(CAST(SaleDate AS DATE)) AS SortValue " &
                       "FROM tbl_SalesTransaction " &
                       "WHERE SaleDate >= " & weekStartExpr & " " &
                       "  AND SaleDate < DATEADD(DAY, 7, " & weekStartExpr & ") " &
                       "GROUP BY FORMAT(CAST(SaleDate AS DATE), 'ddd') " &
                       "ORDER BY SortValue;"

            Case DashboardTimeRange.Month
                Return "SELECT FORMAT(SaleDate, 'dd') AS Period, " &
                       "       SUM(ISNULL(TotalAmount, 0)) AS SalesTotal, " &
                       "       DAY(SaleDate) AS SortValue " &
                       "FROM tbl_SalesTransaction " &
                       "WHERE MONTH(SaleDate) = MONTH(GETDATE()) " &
                       "  AND YEAR(SaleDate) = YEAR(GETDATE()) " &
                       "GROUP BY FORMAT(SaleDate, 'dd'), DAY(SaleDate) " &
                       "ORDER BY SortValue;"

            Case DashboardTimeRange.Year
                Return "SELECT FORMAT(SaleDate, 'MMM') AS Period, " &
                       "       SUM(ISNULL(TotalAmount, 0)) AS SalesTotal, " &
                       "       MONTH(SaleDate) AS SortValue " &
                       "FROM tbl_SalesTransaction " &
                       "WHERE YEAR(SaleDate) = YEAR(GETDATE()) " &
                       "GROUP BY FORMAT(SaleDate, 'MMM'), MONTH(SaleDate) " &
                       "ORDER BY SortValue;"

            Case Else
                Return "SELECT 'N/A' AS Period, CAST(0 AS DECIMAL(18,2)) AS SalesTotal, 0 AS SortValue;"
        End Select
    End Function

    Private Function BuildDateFilterClause(range As DashboardTimeRange, columnName As String) As String
        Dim weekStartExpr As String = GetCurrentWeekStartSqlDateExpression()

        Select Case range
            Case DashboardTimeRange.Today
                Return $"CAST({columnName} AS DATE) = CAST(GETDATE() AS DATE)"
            Case DashboardTimeRange.Week
                Return $"{columnName} >= {weekStartExpr} AND {columnName} < DATEADD(DAY, 7, {weekStartExpr})"
            Case DashboardTimeRange.Month
                Return $"MONTH({columnName}) = MONTH(GETDATE()) AND YEAR({columnName}) = YEAR(GETDATE())"
            Case DashboardTimeRange.Year
                Return $"YEAR({columnName}) = YEAR(GETDATE())"
            Case Else
                Return "1 = 1"
        End Select
    End Function

    Private Shared Function GetCurrentWeekStartSqlDateExpression() As String
        Return "DATEADD(DAY, -(DATEDIFF(DAY, 0, CAST(GETDATE() AS DATE)) % 7), CAST(GETDATE() AS DATE))"
    End Function

    Private Shared Function GetTimeRangeText(range As DashboardTimeRange) As String
        Select Case range
            Case DashboardTimeRange.Today
                Return "Today"
            Case DashboardTimeRange.Week
                Return "Weekly"
            Case DashboardTimeRange.Month
                Return "Monthly"
            Case DashboardTimeRange.Year
                Return "Yearly"
            Case Else
                Return "Weekly"
        End Select
    End Function

    Private Shared Function ExecuteScalarDecimal(conn As SqlConnection, query As String) As Decimal
        Using cmd As New SqlCommand(query, conn)
            Return SafeToDecimal(cmd.ExecuteScalar())
        End Using
    End Function

    Private Shared Function ExecuteScalarInt(conn As SqlConnection, query As String) As Integer
        Using cmd As New SqlCommand(query, conn)
            Dim result As Object = cmd.ExecuteScalar()
            If result Is Nothing OrElse result Is DBNull.Value Then Return 0

            Dim parsed As Integer
            If Integer.TryParse(result.ToString(), parsed) Then
                Return parsed
            End If
            Return 0
        End Using
    End Function

    Private Shared Function SafeToDecimal(value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D

        Dim parsed As Decimal
        If Decimal.TryParse(value.ToString(), parsed) Then
            Return parsed
        End If

        Return 0D
    End Function

    Private Sub StartSessionHeartbeat()
        If _sessionTimer IsNot Nothing Then
            RemoveHandler _sessionTimer.Tick, AddressOf SessionHeartbeat_Tick
            _sessionTimer.Stop()
            _sessionTimer.Dispose()
        End If

        _sessionTimer = New Timer()
        _sessionTimer.Interval = 60000
        AddHandler _sessionTimer.Tick, AddressOf SessionHeartbeat_Tick
        _sessionTimer.Start()
    End Sub

    Private Sub StopSessionHeartbeat()
        If _sessionTimer Is Nothing Then Return

        RemoveHandler _sessionTimer.Tick, AddressOf SessionHeartbeat_Tick
        _sessionTimer.Stop()
        _sessionTimer.Dispose()
        _sessionTimer = Nothing
    End Sub

    Private Sub SessionHeartbeat_Tick(sender As Object, e As EventArgs)
        If SessionService.Heartbeat() Then Return

        StopSessionHeartbeat()

        Try
            MessageBox.Show("Your session has expired. Please login again.",
                            "Session Expired",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
        Catch
        End Try

        Try
            SessionService.EndCurrentSession("Expired")
        Catch
        End Try

        FrmLogin.CurrentUser.UserID = 0
        FrmLogin.CurrentUser.Username = ""
        FrmLogin.CurrentUser.Role = ""
        FrmLogin.CurrentUser.FullName = ""

        Me.Hide()
        Dim loginForm As New FrmLogin()
        loginForm.Show()
    End Sub

    Private Sub btnToday_Click(sender As Object, e As EventArgs) Handles btnToday.Click
        ApplyTimeRange(DashboardTimeRange.Today)
    End Sub

    Private Sub btnWeekly_Click(sender As Object, e As EventArgs) Handles btnWeekly.Click
        ApplyTimeRange(DashboardTimeRange.Week)
    End Sub

    Private Sub btnMontly_Click(sender As Object, e As EventArgs) Handles btnMontly.Click
        ApplyTimeRange(DashboardTimeRange.Month)
    End Sub

    Private Sub btnYearly_Click(sender As Object, e As EventArgs) Handles btnYearly.Click
        ApplyTimeRange(DashboardTimeRange.Year)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        SetActiveSidebarButton(btnDelivery)
        OpenModule(New DeliveriesModuleForm())
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        SetActiveSidebarButton(btnHome)
        LoadDashboardStats()
        LoadLowStockItems()
        ApplyTimeRange(_currentRange)
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        SetActiveSidebarButton(btnFileMaintenance)
        If String.Equals(FrmLogin.CurrentUser.Role, "staff", StringComparison.OrdinalIgnoreCase) Then
            OpenModule(New FileMaintenance.Category())
        Else
            OpenModule(New FileMaintenance.User())
        End If
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        SetActiveSidebarButton(btnDelivery)
        OpenModule(New DeliveriesModuleForm())
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        SetActiveSidebarButton(btnInventory)
        OpenModule(New InventoryModuleForm())
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        SetActiveSidebarButton(btnPos)
        OpenModule(New FrmPOS())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        SetActiveSidebarButton(btnTransaction)
        OpenModule(New TransactionsModuleForm())
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        SetActiveSidebarButton(btnReturn)
        OpenModule(New SupplierReturnsModuleForm())
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SetActiveSidebarButton(btnReports)
        OpenModule(New FrmReports())
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        SetActiveSidebarButton(btnAuditTrail)
        OpenModule(New AuditTrailModuleForm())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                      "Logout",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        StopSessionHeartbeat()

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging out: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try

        LogActivity(FrmLogin.CurrentUser.UserID,
                    FrmLogin.CurrentUser.FullName,
                    FrmLogin.CurrentUser.Username,
                    FrmLogin.CurrentUser.Role,
                    "User Logged Out.")

        FrmLogin.CurrentUser.UserID = 0
        FrmLogin.CurrentUser.Username = ""
        FrmLogin.CurrentUser.Role = ""
        FrmLogin.CurrentUser.FullName = ""

        Me.Hide()
        Dim loginForm As New FrmLogin()
        loginForm.Show()
    End Sub
End Class
