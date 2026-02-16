Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports JJAS_ISBST.Login

Public Class Admin_Inventory
    Private DeliveryProductID As Integer = -1
    Dim formtoshow As Form

    ' For printing
    Private dtPosted As DataTable
    Private currentRow As Integer = 0

    Private Sub Admin_inventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Dim deleteSql As String = "DELETE FROM tbl_delivery_products WHERE Quantity <= 0"
            Using cmd As New SqlCommand(deleteSql, conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        BlockCopyPaste(txtSearch)
        displayData("")
        Select Case Login.CurrentUser.Role.ToLower()

            Case "staff"

                btnpos.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False

                btnAuditTrail.Image = Nothing
                btnTransaction.Image = Nothing
                btnpos.Image = Nothing
        End Select

        RearrangeButtons(Panelmenu) ' replace PanelMenu with your actual panel name

    End Sub
    Private Sub RearrangeButtons(panel As Panel)
        Dim y As Integer = 10
        For Each ctrl As Control In panel.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Visible Then
                ctrl.Location = New Point(10, y)
                y += ctrl.Height + 10
            End If
        Next
    End Sub
    Private Function GetStockStatus(quantity As Integer) As String
        If quantity <= 0 Then
            Return "Out of Stock"
        ElseIf quantity <= 5 Then
            Return "Critical Level"
        ElseIf quantity <= 10 Then
            Return "Low Stock"
        Else
            Return "In Stock"
        End If
    End Function

    Private Sub displayData(searchText As String)
        Try
            Dim dt As New DataTable()
        Dim sql As String = "
        SELECT dp.DeliveryProductID,
               d.DeliveryID,
               d.OrderNumber,
               p.barcodeNumber,
               p.Product AS ProductName,
               dp.Quantity,
               p.ImagePath,
               dp.DateUpdated
        FROM tbl_delivery_products dp
        INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
        INNER JOIN tbl_Supplier s ON d.supplierid = s.supplierid
        INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
        WHERE dp.Status = 'Posted' AND Quantity >= 0
          AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
        ORDER BY d.DateCreated DESC;"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                Dim searchValue As String = "%" & searchText & "%"
                cmd.Parameters.AddWithValue("@search", searchValue)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        ' --- Add StockLevelStatus column if missing ---
        If Not dt.Columns.Contains("StockLevelStatus") Then
            dt.Columns.Add("StockLevelStatus", GetType(String))
        End If

        ' --- Compute stock status for each row ---
        For Each r As DataRow In dt.Rows
            Dim qty As Integer = Convert.ToInt32(r("Quantity"))
            r("StockLevelStatus") = GetStockStatus(qty)
        Next

        ' --- Add ProductImage column if missing ---
        If Not dt.Columns.Contains("ProductImage") Then
            dt.Columns.Add("ProductImage", GetType(Image))
        End If

        ' --- Load product images ---
        For Each r As DataRow In dt.Rows
            Dim path As String = r("ImagePath").ToString()
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    r("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                r("ProductImage") = Nothing
            End If
        Next

        ' --- Bind to grid ---
        DGVdeliveries.DataSource = dt

        ' --- Hide unnecessary columns ---
        Dim hiddenCols() As String = {"ImagePath", "DeliveryProductID", "DeliveryID"}
        For Each colName In hiddenCols
            If DGVdeliveries.Columns.Contains(colName) Then
                DGVdeliveries.Columns(colName).Visible = False
            End If
        Next

        ' --- Move StockLevelStatus as first column ---
        If DGVdeliveries.Columns.Contains("StockLevelStatus") Then
            DGVdeliveries.Columns("StockLevelStatus").DisplayIndex = 0
            DGVdeliveries.Columns("StockLevelStatus").HeaderText = "Stock Status"
        End If

        ' --- Image column settings ---
        If DGVdeliveries.Columns.Contains("ProductImage") Then
            DGVdeliveries.Columns("ProductImage").DisplayIndex = 1
            Dim imgCol As DataGridViewImageColumn = DirectCast(DGVdeliveries.Columns("ProductImage"), DataGridViewImageColumn)
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            imgCol.Width = 120
            imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If

        ' --- Headers ---
        With DGVdeliveries
            .Columns("OrderNumber").HeaderText = "Order #"
            .Columns("ProductName").HeaderText = "Product"
            .Columns("Quantity").HeaderText = "Qty"
        End With

        ' --- Color code rows based on status ---
        For Each row As DataGridViewRow In DGVdeliveries.Rows
            Dim status As String = row.Cells("StockLevelStatus").Value.ToString()
            Select Case status
                Case "Out of Stock"
                    row.DefaultCellStyle.BackColor = Color.LightCoral
                Case "Critical Level"
                    row.DefaultCellStyle.BackColor = Color.LightSalmon
                Case "Low Stock"
                    row.DefaultCellStyle.BackColor = Color.Khaki
                Case Else
                    row.DefaultCellStyle.BackColor = Color.LightGreen
            End Select
        Next

        ' --- Auto sizing ---
        DGVdeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVdeliveries.Columns
            If col.Name = "ProductImage" Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

            DGVdeliveries.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading inventory: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Make grid easier to read
    Private Sub DGVsize_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        DGVdeliveries.RowTemplate.Height = 50
        For Each row As DataGridViewRow In DGVdeliveries.Rows
            row.Height = 50
        Next

        DGVdeliveries.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVdeliveries.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVdeliveries.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    ' Search
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then
            lblPlaceholder.Visible = True
        End If
    End Sub
    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        ' Prevent scanner's Enter from triggering anything
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    ' Switch timer
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub

    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        Dim role As String = Login.CurrentUser.Role.ToLower()

        If role = "staff" Then
            formtoshow = New admin_category()
        Else
            formtoshow = New Admin_User()
        End If

        formtoshow.Show()
        StartSwitchTimer()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        dtPosted = CType(DGVdeliveries.DataSource, DataTable)
        currentRow = 0

        If dtPosted Is Nothing OrElse dtPosted.Rows.Count = 0 Then
            MessageBox.Show("No posted products to print.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' 🖨 Allow user to choose printer
        Using printDlg As New PrintDialog()
            printDlg.Document = PrintDocument1
            If printDlg.ShowDialog() = DialogResult.OK Then
                PrintDocument1.PrinterSettings = printDlg.PrinterSettings

                ' ✅ Log and show preview only if printer selected
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Printed inventory.")
                PrintPreviewDialog1.Document = PrintDocument1
                PrintPreviewDialog1.ShowDialog()
            Else
                MessageBox.Show("Printing cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontHeader As New Font("Segoe UI", 14, FontStyle.Bold)
        Dim fontBody As New Font("Segoe UI", 10, FontStyle.Regular)
        Dim fontSmall As New Font("Segoe UI", 9, FontStyle.Italic)
        Dim fontPage As New Font("Segoe UI", 9, FontStyle.Regular)

        Dim y As Integer = e.MarginBounds.Top
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim baseRowHeight As Integer = 28
        Static pageNumber As Integer = 1

        ' --- HEADER ---
        Dim title As String = "Inventory Report"
        Dim titleSize As SizeF = e.Graphics.MeasureString(title, fontHeader)
        e.Graphics.DrawString(title, fontHeader, Brushes.Black,
                          leftMargin + (e.MarginBounds.Width - titleSize.Width) \ 2, y)
        y += 40

        e.Graphics.DrawString("Printed by: " & Login.CurrentUser.FullName, fontSmall, Brushes.Black, leftMargin, y)
        y += 20
        e.Graphics.DrawString("Date Printed: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontSmall, Brushes.Black, leftMargin, y)
        y += 30

        ' --- COLUMN HEADERS ---
        Dim colHeaders() As String = {"Order #", "Barcode", "Product", "Qty", "Stock Status"}
        Dim colRatios() As Single = {0.15F, 0.2F, 0.3F, 0.1F, 0.25F} ' total = 1.0
        Dim totalWidth As Integer = e.MarginBounds.Width
        Dim colWidths(colHeaders.Length - 1) As Integer
        For i As Integer = 0 To colHeaders.Length - 1
            colWidths(i) = CInt(totalWidth * colRatios(i))
        Next

        Dim x As Integer = leftMargin
        For i As Integer = 0 To colHeaders.Length - 1
            e.Graphics.FillRectangle(Brushes.LightGray, x, y, colWidths(i), baseRowHeight)
            e.Graphics.DrawRectangle(Pens.Black, x, y, colWidths(i), baseRowHeight)
            Dim sfHeader As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            e.Graphics.DrawString(colHeaders(i), fontBody, Brushes.Black, New RectangleF(x, y, colWidths(i), baseRowHeight), sfHeader)
            x += colWidths(i)
        Next
        y += baseRowHeight

        ' --- DATA ROWS ---
        Dim linesPerPage As Integer = Math.Floor((e.MarginBounds.Bottom - y - 100) / baseRowHeight)
        Dim linesPrinted As Integer = 0

        While currentRow < dtPosted.Rows.Count AndAlso linesPrinted < linesPerPage
            Dim r = dtPosted.Rows(currentRow)
            Dim values() As String = {
            r("OrderNumber").ToString(),
            r("BarcodeNumber").ToString(),
            r("ProductName").ToString(),
            r("Quantity").ToString(),
            r("StockLevelStatus").ToString()
        }

            ' --- Auto-adjust height for wrapped text ---
            Dim maxHeight As Integer = baseRowHeight
            For i As Integer = 0 To values.Length - 1
                Dim textSize As SizeF = e.Graphics.MeasureString(values(i), fontBody, colWidths(i) - 8)
                Dim neededHeight As Integer = CInt(Math.Ceiling(textSize.Height)) + 10
                If neededHeight > maxHeight Then
                    maxHeight = neededHeight
                End If
            Next

            ' --- Draw row cells ---
            x = leftMargin
            For i As Integer = 0 To values.Length - 1
                Dim rectF As New RectangleF(x, y, colWidths(i), maxHeight)
                e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectF))

                Dim innerRect As RectangleF = rectF
                innerRect.Inflate(-4.0F, -2.0F)

                Dim sf As New StringFormat With {.LineAlignment = StringAlignment.Center}
                If i = 3 Then ' Qty column → right align
                    sf.Alignment = StringAlignment.Far
                Else
                    sf.Alignment = StringAlignment.Near
                End If

                e.Graphics.DrawString(values(i), fontBody, Brushes.Black, innerRect, sf)
                x += colWidths(i)
            Next

            y += maxHeight
            currentRow += 1
            linesPrinted += 1

            ' --- PAGE BREAK ---
            If y > e.MarginBounds.Bottom - 100 Then
                e.HasMorePages = True
                pageNumber += 1
                Return
            End If
        End While

        ' --- END OF REPORT ---
        If currentRow >= dtPosted.Rows.Count Then
            e.HasMorePages = False
            pageNumber = 1 ' Reset when done
        End If

        ' --- PAGE NUMBER ---
        Dim pageText As String = $"Page {pageNumber}"
        Dim pageSize As SizeF = e.Graphics.MeasureString(pageText, fontPage)
        e.Graphics.DrawString(pageText, fontPage, Brushes.Black,
                          e.MarginBounds.Left + (e.MarginBounds.Width - pageSize.Width) \ 2,
                          e.MarginBounds.Bottom + 40)
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnpos.Click
        formtoshow = New Admin_Pos()
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
            Login.CurrentUser.UserID = 0
            Login.CurrentUser.Username = ""
            Login.CurrentUser.Role = ""
            Login.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show Login form again
            Dim f As New Login()
            f.Show()
        End If
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        formtoshow = New Admin_Home()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
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
