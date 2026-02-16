Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports JJAS_ISBST.Login

Public Class Admin_transaction
    Dim formtoshow As Form
    Private dtSales As DataTable
    Private currentRow As Integer = 0
    ' --- Global variables for printing ---

    Private pageNumber As Integer = 1
    Private totalPages As Integer = 1
    Public ReportStartDate As Date? = Nothing
    Public ReportEndDate As Date? = Nothing

    Private Sub Admin_SalesHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")

        ' Hide admin-only buttons for cashier role
        Select Case Login.CurrentUser.Role.ToLower()
            Case "cashier"
                btnInventory.Visible = False
                btnDelivery.Visible = False
                btnFileM.Visible = False
                btnAuditTrail.Visible = False

                btnAuditTrail.Image = Nothing
                btnInventory.Image = Nothing
                btnDelivery.Image = Nothing
                btnFileM.Image = Nothing
        End Select

        RearrangeButtons(Panelmenu)
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

    ' ================== DISPLAY SALES HISTORY ==================
    Private Sub displayData(searchText As String)
        Try
            Dim dt As New DataTable()
        Dim sql As String = "
            SELECT 
                SaleID,
                Name AS Cashier,
                ProductName,
                BarcodeNumber,
                Quantity,
                SellingPrice,
                TotalAmount,
                Discount,
                VatRate,
                VatAmount,
                Vatable,
                TransactionNo,
                SaleDate
            FROM tbl_SalesHistory
            WHERE (@search = '' OR ProductName LIKE @search OR BarcodeNumber LIKE @search OR TransactionNo LIKE @search OR Name LIKE @search)
            ORDER BY SaleDate DESC;"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        DGVTransacopn.DataSource = dt

        Dim hiddenCols() As String = {"SaleID", "VatRate", "VatAmount", "Vatable", "Discount"}
        For Each colName In hiddenCols
            If DGVTransacopn.Columns.Contains(colName) Then
                DGVTransacopn.Columns(colName).Visible = False
            End If
        Next
        ' Column headers
        With DGVTransacopn
            .Columns("SaleID").Visible = False
            .Columns("Cashier").HeaderText = "Cashier"
            .Columns("ProductName").HeaderText = "Product"
            .Columns("BarcodeNumber").HeaderText = "Barcode"
            .Columns("Quantity").HeaderText = "Qty"
            .Columns("SellingPrice").HeaderText = "Price"
            .Columns("TotalAmount").HeaderText = "Total"
            .Columns("TransactionNo").HeaderText = "Trans. No"
            .Columns("SaleDate").HeaderText = "Date"
        End With

        DGVTransacopn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DGVTransacopn.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading transaction: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================== SEARCH BAR ==================
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
    ' ================== PRINT SALES HISTORY ==================

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            ' --- Get DataTable safely (support both DataTable and BindingSource) ---
            If TypeOf DGVTransacopn.DataSource Is DataTable Then
                dtSales = CType(DGVTransacopn.DataSource, DataTable)
            ElseIf TypeOf DGVTransacopn.DataSource Is BindingSource Then
                Dim bs = CType(DGVTransacopn.DataSource, BindingSource)
                If TypeOf bs.DataSource Is DataTable Then
                    dtSales = CType(bs.DataSource, DataTable)
                End If
            Else
                dtSales = Nothing
            End If

            currentRow = 0
            pageNumber = 1

            ' --- Validate data before printing ---
            If dtSales Is Nothing OrElse dtSales.Rows.Count = 0 Then
                MessageBox.Show("No sales history to print.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' 🖨 Allow user to select printer (instead of default thermal)
            Using printDlg As New PrintDialog()
                printDlg.Document = PrintDocument1
                If printDlg.ShowDialog() = DialogResult.OK Then
                    PrintDocument1.PrinterSettings = printDlg.PrinterSettings

                    ' 🧾 Log action after user confirms print
                    LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Printed Transaction.")

                    ' ✅ Show preview window using selected printer
                    PrintPreviewDialog1.Document = PrintDocument1
                    PrintPreviewDialog1.ShowDialog()
                Else
                    MessageBox.Show("Printing cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error printing transaction history: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontHeader As New Font("Segoe UI", 14, FontStyle.Bold)
        Dim fontBody As New Font("Segoe UI", 10, FontStyle.Regular)
        Dim fontSmall As New Font("Segoe UI", 9, FontStyle.Italic)
        Dim fontPage As New Font("Segoe UI", 9, FontStyle.Regular)

        Dim y As Integer = e.MarginBounds.Top
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim baseRowHeight As Integer = 28

        ' --- HEADER ---
        Dim title As String = "Sales History Report"
        Dim titleSize As SizeF = e.Graphics.MeasureString(title, fontHeader)
        e.Graphics.DrawString(title, fontHeader, Brushes.Black,
                          leftMargin + (e.MarginBounds.Width - titleSize.Width) \ 2, y)
        y += 40

        ' Optional date range
        If ReportStartDate.HasValue AndAlso ReportEndDate.HasValue Then
            e.Graphics.DrawString($"Date Range: {ReportStartDate.Value:MM/dd/yyyy} - {ReportEndDate.Value:MM/dd/yyyy}", fontSmall, Brushes.Black, leftMargin, y)
            y += 20
        End If

        e.Graphics.DrawString("Printed by: " & Login.CurrentUser.FullName, fontSmall, Brushes.Black, leftMargin, y)
        y += 20
        e.Graphics.DrawString("Date Printed: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontSmall, Brushes.Black, leftMargin, y)
        y += 30

        ' --- COLUMN HEADERS ---
        Dim colHeaders() As String = {"Trans#", "Date", "Product", "Qty", "Price", "Total", "Cashier"}
        Dim colRatios() As Single = {0.15F, 0.12F, 0.25F, 0.07F, 0.1F, 0.1F, 0.21F}
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
        Dim grandTotal As Decimal = 0D
        Dim linesPerPage As Integer = Math.Floor((e.MarginBounds.Bottom - y - 100) / baseRowHeight)
        Dim linesPrinted As Integer = 0

        While currentRow < dtSales.Rows.Count AndAlso linesPrinted < linesPerPage
            Dim r = dtSales.Rows(currentRow)

            Dim values() As String = {
            r("TransactionNo").ToString(),
            CDate(r("SaleDate")).ToString("MM/dd/yyyy"),
            r("ProductName").ToString(),
            r("Quantity").ToString(),
            Convert.ToDecimal(r("SellingPrice")).ToString("N2"),
            Convert.ToDecimal(r("TotalAmount")).ToString("N2"),
            r("Cashier").ToString()
        }

            ' --- Determine max row height based on wrapped text ---
            Dim maxHeight As Integer = baseRowHeight
            For i As Integer = 0 To values.Length - 1
                Dim textSize As SizeF = e.Graphics.MeasureString(values(i), fontBody, colWidths(i) - 8)
                Dim neededHeight As Integer = CInt(Math.Ceiling(textSize.Height)) + 10
                If neededHeight > maxHeight Then
                    maxHeight = neededHeight
                End If
            Next

            ' --- Draw cells ---
            x = leftMargin
            For i As Integer = 0 To values.Length - 1
                Dim rectF As New RectangleF(x, y, colWidths(i), maxHeight)
                e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectF))

                Dim innerRect As RectangleF = rectF
                innerRect.Inflate(-4.0F, -2.0F)

                Dim sf As New StringFormat With {.LineAlignment = StringAlignment.Center}
                If i >= 3 AndAlso i <= 5 Then
                    sf.Alignment = StringAlignment.Far
                    e.Graphics.DrawString(values(i), fontBody, Brushes.Black, innerRect, sf)
                Else
                    sf.Alignment = StringAlignment.Near
                    e.Graphics.DrawString(values(i), fontBody, Brushes.Black, innerRect, sf)
                End If

                x += colWidths(i)
            Next

            ' --- Update totals and vertical position ---
            grandTotal += Convert.ToDecimal(r("TotalAmount"))
            y += maxHeight
            currentRow += 1
            linesPrinted += 1

            ' --- Page break check ---
            If y > e.MarginBounds.Bottom - 100 Then
                e.HasMorePages = True
                pageNumber += 1
                Return
            End If
        End While

        ' --- FOOTER ---
        If currentRow >= dtSales.Rows.Count Then
            e.HasMorePages = False
            y += 10
            e.Graphics.DrawString("Grand Total: ₱" & grandTotal.ToString("N2"), fontHeader, Brushes.Black, leftMargin, y)
        End If

        ' --- PAGE NUMBER ---
        Dim pageText As String = $"Page {pageNumber}"
        Dim pageSize As SizeF = e.Graphics.MeasureString(pageText, fontPage)
        e.Graphics.DrawString(pageText, fontPage, Brushes.Black,
                          e.MarginBounds.Left + (e.MarginBounds.Width - pageSize.Width) \ 2,
                          e.MarginBounds.Bottom + 40)
    End Sub



    ' ================== NAVIGATION ==================
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub

    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnpos_Click(sender As Object, e As EventArgs) Handles btnpos.Click
        formtoshow = New Admin_Pos()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnFileM_Click(sender As Object, e As EventArgs) Handles btnFileM.Click
        formtoshow = New Admin_User()
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

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New Admin_AuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click

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
