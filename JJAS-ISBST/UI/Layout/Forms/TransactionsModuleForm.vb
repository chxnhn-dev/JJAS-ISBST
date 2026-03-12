Imports System.Drawing.Printing

Public Class TransactionsModuleForm
    Inherits ModuleListBaseForm

    Private ReadOnly _printDocument As New PrintDocument()
    Private _service As TransactionService
    Private _printData As DataTable
    Private _currentPrintRow As Integer
    Private _pageNumber As Integer = 1
    Private _grandTotal As Decimal

    Public Sub New()
        MyBase.New()
        AddHandler _printDocument.PrintPage, AddressOf PrintDocument_PrintPage
    End Sub

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.TransactionsTab
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Transactions:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Transaction / Product / Barcode"
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

    Private Function GetTransactionService() As TransactionService
        If _service Is Nothing Then
            _service = New TransactionService()
        End If

        Return _service
    End Function

    Protected Overrides Sub ConfigureModulePermissions()
        If IsCashierUser() Then
            ApplyCashierSidebarRestrictions()
        End If
    End Sub

    Protected Overrides Sub LoadModuleData(searchText As String)
        Try
            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) GetTransactionService().GetSalesHistoryPage(request))
            Dim dt As DataTable = page.Records

            DGVtable.DataSource = dt
            ApplyTransactionsGridFormatting()
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading transactions: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub HandlePrintAction()
        Dim source As DataTable = TryCast(DGVtable.DataSource, DataTable)
        If source Is Nothing OrElse source.Rows.Count = 0 Then
            MessageBox.Show("No sales history to print.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        _printData = source.Copy()
        _currentPrintRow = 0
        _pageNumber = 1
        _grandTotal = 0D

        For Each row As DataRow In _printData.Rows
            Dim rowTotal As Decimal
            If Decimal.TryParse(Convert.ToString(row("TotalAmount")), rowTotal) Then
                _grandTotal += rowTotal
            End If
        Next

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
                        "Printed Transaction.")

            Using preview As New PrintPreviewDialog()
                preview.Document = _printDocument
                preview.ShowDialog()
            End Using
        End Using
    End Sub

    Private Sub ApplyTransactionsGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return

        Dim hiddenCols() As String = {"SaleID", "VatRate", "VatAmount", "Vatable", "Discount"}
        For Each colName As String In hiddenCols
            GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
        Next

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"Cashier", Sub(col) col.HeaderText = "Cashier"},
            {"ProductName", Sub(col) col.HeaderText = "Product"},
            {"BarcodeNumber", Sub(col) col.HeaderText = "Barcode"},
            {"Quantity", Sub(col) col.HeaderText = "Qty"},
            {"SellingPrice", Sub(col)
                                 col.HeaderText = "Price"
                                 col.DefaultCellStyle.Format = "N2"
                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                             End Sub},
            {"TotalAmount", Sub(col)
                                col.HeaderText = "Total"
                                col.DefaultCellStyle.Format = "N2"
                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            End Sub},
            {"TransactionNo", Sub(col) col.HeaderText = "Trans. No"},
            {"SaleDate", Sub(col) col.HeaderText = "Date"}
        }
        Dim columnAliases As New Dictionary(Of String, String()) From {
            {"Cashier", New String() {"Name"}},
            {"ProductName", New String() {"Product"}}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions, columnAliases)

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)
        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
    End Sub

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplyTransactionsGridFormatting()
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

        Dim title As String = "Sales History Report"
        Dim titleSize As SizeF = e.Graphics.MeasureString(title, fontHeader)
        e.Graphics.DrawString(title, fontHeader, Brushes.Black, left + (e.MarginBounds.Width - titleSize.Width) \ 2, y)
        y += 40

        e.Graphics.DrawString("Printed by: " & FrmLogin.CurrentUser.FullName, fontSmall, Brushes.Black, left, y)
        y += 20
        e.Graphics.DrawString("Date Printed: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontSmall, Brushes.Black, left, y)
        y += 30

        Dim headers() As String = {"Trans#", "Date", "Product", "Qty", "Price", "Total", "Cashier"}
        Dim ratios() As Single = {0.15F, 0.12F, 0.25F, 0.07F, 0.1F, 0.1F, 0.21F}
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

        Dim linesPerPage As Integer = Math.Floor((e.MarginBounds.Bottom - y - 120) / rowHeight)
        Dim linesPrinted As Integer = 0

        While _currentPrintRow < _printData.Rows.Count AndAlso linesPrinted < linesPerPage
            Dim row As DataRow = _printData.Rows(_currentPrintRow)
            Dim saleDateText As String = String.Empty
            Dim parsedDate As DateTime
            If DateTime.TryParse(Convert.ToString(row("SaleDate")), parsedDate) Then
                saleDateText = parsedDate.ToString("MM/dd/yyyy")
            End If

            Dim priceValue As Decimal
            Dim totalValue As Decimal
            Decimal.TryParse(Convert.ToString(row("SellingPrice")), priceValue)
            Decimal.TryParse(Convert.ToString(row("TotalAmount")), totalValue)

            Dim values() As String = {
                Convert.ToString(row("TransactionNo")),
                saleDateText,
                Convert.ToString(row("ProductName")),
                Convert.ToString(row("Quantity")),
                priceValue.ToString("N2"),
                totalValue.ToString("N2"),
                Convert.ToString(row("Cashier"))
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
                If i >= 3 AndAlso i <= 5 Then
                    sf.Alignment = StringAlignment.Far
                Else
                    sf.Alignment = StringAlignment.Near
                End If

                e.Graphics.DrawString(values(i), fontBody, Brushes.Black, innerRect, sf)
                x += widths(i)
            Next

            y += maxHeight
            _currentPrintRow += 1
            linesPrinted += 1

            If y > e.MarginBounds.Bottom - 120 Then
                e.HasMorePages = True
                _pageNumber += 1
                Return
            End If
        End While

        If _currentPrintRow >= _printData.Rows.Count Then
            y += 10
            e.Graphics.DrawString("Grand Total: " & _grandTotal.ToString("N2"), fontHeader, Brushes.Black, left, y)
        End If

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
