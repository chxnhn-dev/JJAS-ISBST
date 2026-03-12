Imports System.Data.SqlClient
Imports System.Drawing.Printing

Public Class InventoryModuleForm
    Inherits ModuleListBaseForm

    Private ReadOnly _printDocument As New PrintDocument()
    Private _service As InventoryService
    Private _printData As DataTable
    Private _currentPrintRow As Integer
    Private _pageNumber As Integer = 1
    Private _cleanupExecuted As Boolean

    Public Sub New()
        MyBase.New()
        AddHandler _printDocument.PrintPage, AddressOf PrintDocument_PrintPage
    End Sub

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.InventoryTab
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Inventory:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Barcode / Product"
        End Get
    End Property

    Protected Overrides ReadOnly Property ShowPrintButton As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property SupportsPagination As Boolean
        Get
            Return True
        End Get
    End Property

    Private Function GetInventoryService() As InventoryService
        If _service Is Nothing Then
            _service = New InventoryService()
        End If

        Return _service
    End Function

    Protected Overrides Sub ConfigureModulePermissions()
        If IsStaffUser() Then
            ApplyStaffSidebarRestrictions()
        ElseIf IsCashierUser() Then
            ApplyCashierSidebarRestrictions()
        End If
    End Sub

    Protected Overrides Sub LoadModuleData(searchText As String)
        Try
            RunOneTimeInventoryCleanup()

            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) GetInventoryService().GetPostedInventoryPage(request))
            Dim dt As DataTable = page.Records

            If Not dt.Columns.Contains("StockLevelStatus") Then
                dt.Columns.Add("StockLevelStatus", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                Dim qty As Integer = Convert.ToInt32(row("Quantity"))
                row("StockLevelStatus") = GetStockStatus(qty)
            Next

            If Not dt.Columns.Contains("ProductImage") Then
                dt.Columns.Add("ProductImage", GetType(Image))
            End If

            For Each row As DataRow In dt.Rows
                Dim path As String = Convert.ToString(row("ImagePath"))
                If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                    Using tempImage As Image = Image.FromFile(path)
                        row("ProductImage") = New Bitmap(tempImage)
                    End Using
                Else
                    row("ProductImage") = Nothing
                End If
            Next

            DGVtable.DataSource = dt
            ApplyInventoryGridFormatting()
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading inventory: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub HandlePrintAction()
        Dim source As DataTable = TryCast(DGVtable.DataSource, DataTable)
        If source Is Nothing OrElse source.Rows.Count = 0 Then
            MessageBox.Show("No posted products to print.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        _printData = source.Copy()
        _currentPrintRow = 0
        _pageNumber = 1

        Using printDlg As New PrintDialog()
            printDlg.Document = _printDocument
            If printDlg.ShowDialog() <> DialogResult.OK Then
                MessageBox.Show("Printing cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            _printDocument.PrinterSettings = printDlg.PrinterSettings
            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        "Printed inventory.")

            Using preview As New PrintPreviewDialog()
                preview.Document = _printDocument
                preview.ShowDialog()
            End Using
        End Using
    End Sub

    Private Sub RunOneTimeInventoryCleanup()
        If _cleanupExecuted Then Return

        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Using cmd As New SqlCommand("DELETE FROM tbl_delivery_products WHERE Quantity <= 0 AND ISNULL(Status, 'Pending') = 'Pending' AND ISNULL(ReturnedQty, 0) = 0", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        _cleanupExecuted = True
    End Sub

    Private Function GetStockStatus(quantity As Integer) As String
        If quantity <= 0 Then
            Return "Out of Stock"
        ElseIf quantity <= 5 Then
            Return "Critical Level"
        ElseIf quantity <= 10 Then
            Return "Low Stock"
        End If

        Return "In Stock"
    End Function

    Private Sub ApplyInventoryGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return

        Dim hiddenCols() As String = {"ImagePath", "DeliveryProductID", "DeliveryID"}
        For Each colName As String In hiddenCols
            GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
        Next

        GridHelpers.ApplyColumnSetup(DGVtable, "StockLevelStatus", Sub(col)
                                                                       col.DisplayIndex = 0
                                                                       col.HeaderText = "Stock Status"
                                                                   End Sub)

        Dim imageColumn As DataGridViewColumn = Nothing
        If GridHelpers.TryGetColumn(DGVtable, imageColumn, "ProductImage") Then
            Dim imageCol As DataGridViewImageColumn = TryCast(imageColumn, DataGridViewImageColumn)
            If imageCol IsNot Nothing Then
                imageCol.DisplayIndex = 1
                imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                imageCol.Width = 120
                imageCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"OrderNumber", Sub(col) col.HeaderText = "Order #"},
            {"ProductName", Sub(col) col.HeaderText = "Product"},
            {"Quantity", Sub(col) col.HeaderText = "Qty"},
            {"BarcodeNumber", Sub(col) col.HeaderText = "Barcode"}
        }
        Dim columnAliases As New Dictionary(Of String, String()) From {
            {"ProductName", New String() {"Product"}}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions, columnAliases)

        Dim statusColumn As DataGridViewColumn = Nothing
        If GridHelpers.TryGetColumn(DGVtable, statusColumn, "StockLevelStatus") Then
            Dim statusColumnName As String = statusColumn.Name
            For Each row As DataGridViewRow In DGVtable.Rows
                If row.IsNewRow Then Continue For

                Dim status As String = Convert.ToString(row.Cells(statusColumnName).Value)
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
        End If

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVtable.Columns
            If col.Name = "ProductImage" Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        ApplyStandardGridLayout(DGVtable)
        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
    End Sub

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplyInventoryGridFormatting()
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        If _printData Is Nothing Then
            e.HasMorePages = False
            Return
        End If

        Dim fontHeader As New Font("Segoe UI", 14, FontStyle.Bold)
        Dim fontBody As New Font("Segoe UI", 10, FontStyle.Regular)
        Dim fontSmall As New Font("Segoe UI", 9, FontStyle.Italic)
        Dim fontPage As New Font("Segoe UI", 9, FontStyle.Regular)

        Dim y As Integer = e.MarginBounds.Top
        Dim left As Integer = e.MarginBounds.Left
        Dim rowHeight As Integer = 28

        Dim title As String = "Inventory Report"
        Dim titleSize As SizeF = e.Graphics.MeasureString(title, fontHeader)
        e.Graphics.DrawString(title, fontHeader, Brushes.Black, left + (e.MarginBounds.Width - titleSize.Width) \ 2, y)
        y += 40

        e.Graphics.DrawString("Printed by: " & FrmLogin.CurrentUser.FullName, fontSmall, Brushes.Black, left, y)
        y += 20
        e.Graphics.DrawString("Date Printed: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontSmall, Brushes.Black, left, y)
        y += 30

        Dim headers() As String = {"Order #", "Barcode", "Product", "Qty", "Stock Status"}
        Dim ratios() As Single = {0.15F, 0.2F, 0.3F, 0.1F, 0.25F}
        Dim totalWidth As Integer = e.MarginBounds.Width
        Dim widths(headers.Length - 1) As Integer
        For i As Integer = 0 To headers.Length - 1
            widths(i) = CInt(totalWidth * ratios(i))
        Next

        Dim x As Integer = left
        For i As Integer = 0 To headers.Length - 1
            e.Graphics.FillRectangle(Brushes.LightGray, x, y, widths(i), rowHeight)
            e.Graphics.DrawRectangle(Pens.Black, x, y, widths(i), rowHeight)
            Dim sfHeader As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            e.Graphics.DrawString(headers(i), fontBody, Brushes.Black, New RectangleF(x, y, widths(i), rowHeight), sfHeader)
            x += widths(i)
        Next
        y += rowHeight

        Dim linesPerPage As Integer = Math.Floor((e.MarginBounds.Bottom - y - 100) / rowHeight)
        Dim linesPrinted As Integer = 0

        While _currentPrintRow < _printData.Rows.Count AndAlso linesPrinted < linesPerPage
            Dim row As DataRow = _printData.Rows(_currentPrintRow)
            Dim values() As String = {
                Convert.ToString(row("OrderNumber")),
                Convert.ToString(row("BarcodeNumber")),
                Convert.ToString(row("ProductName")),
                Convert.ToString(row("Quantity")),
                Convert.ToString(row("StockLevelStatus"))
            }

            Dim maxHeight As Integer = rowHeight
            For i As Integer = 0 To values.Length - 1
                Dim textSize As SizeF = e.Graphics.MeasureString(values(i), fontBody, widths(i) - 8)
                Dim neededHeight As Integer = CInt(Math.Ceiling(textSize.Height)) + 10
                If neededHeight > maxHeight Then
                    maxHeight = neededHeight
                End If
            Next

            x = left
            For i As Integer = 0 To values.Length - 1
                Dim rect As New RectangleF(x, y, widths(i), maxHeight)
                e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rect))

                Dim innerRect As RectangleF = rect
                innerRect.Inflate(-4.0F, -2.0F)

                Dim sf As New StringFormat With {.LineAlignment = StringAlignment.Center}
                sf.Alignment = If(i = 3, StringAlignment.Far, StringAlignment.Near)
                e.Graphics.DrawString(values(i), fontBody, Brushes.Black, innerRect, sf)
                x += widths(i)
            Next

            y += maxHeight
            _currentPrintRow += 1
            linesPrinted += 1

            If y > e.MarginBounds.Bottom - 100 Then
                e.HasMorePages = True
                _pageNumber += 1
                Return
            End If
        End While

        Dim pageText As String = $"Page {_pageNumber}"
        Dim pageSize As SizeF = e.Graphics.MeasureString(pageText, fontPage)
        e.Graphics.DrawString(pageText,
                              fontPage,
                              Brushes.Black,
                              e.MarginBounds.Left + (e.MarginBounds.Width - pageSize.Width) \ 2,
                              e.MarginBounds.Bottom + 40)

        If _currentPrintRow >= _printData.Rows.Count Then
            e.HasMorePages = False
            _pageNumber = 1
        Else
            e.HasMorePages = True
            _pageNumber += 1
        End If
    End Sub
End Class
