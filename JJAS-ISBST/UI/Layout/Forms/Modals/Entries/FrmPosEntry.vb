Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms
Imports Microsoft.VisualBasic

Public Class FrmPosEntry

    ' Runtime layout settings for the POS picker modal.
    ' Note: the WinForms designer view will look different because this form rebuilds its UI at runtime.
    Private Const UiFormWidth As Integer = 860
    Private Const UiFormHeight As Integer = 500
    Private Const UiOuterPadding As Integer = 20
    Private Const UiHeaderHeight As Integer = 48
    Private Const UiBodyTopGap As Integer = 16
    Private Const UiCardPadding As Integer = 16
    Private Const UiTopRowHeight As Integer = 56
    Private Const UiTopRowBottomGap As Integer = 12
    Private Const UiBottomRowHeight As Integer = 52
    Private Const UiBottomRowTopGap As Integer = 12
    Private Const UiSearchBoxHeight As Integer = 44
    Private Const UiFooterButtonWidth As Integer = 100
    Private Const UiHeaderCloseRightInset As Integer = 24
    Private Const UiHeaderCloseTopInset As Integer = 6
    Private Const UiTitleTopInset As Integer = 4

    Private Const ColAddAction As String = "colAddAction"
    Private Const ColProductImagePreview As String = "ProductImagePreview"
    Private Const ResultGridRowHeight As Integer = 64
    Private Const ResultGridImageColumnWidth As Integer = 72

    Private _uiInitialized As Boolean
    Private _btnCloseProxy As Guna2Button
    Private _txtSearchQuery As Guna2TextBox
    Private _gridResults As Guna2DataGridView
    Private _btnClose As Guna2Button
    Private Shared _productImagePlaceholderCache As Image

    Public IsEditMode As Boolean = False

    Public selectedID As Integer = -1 ' DeliveryProductID
    Public ProductID As Integer = -1
    Public SelectedProductName As String = String.Empty
    Public SelectedBarcode As String = String.Empty
    Public SelectedQuantity As Integer = 0
    Public SelectedSellingPrice As Decimal = 0D
    Public SelectedImagePath As String = String.Empty
    Public selectedCostPrice As Decimal = 0D
    Public SelectedCategory As String = String.Empty

    Public Sub New()
        InitializeComponent()

        Dim fixedSize As New Size(UiFormWidth, UiFormHeight)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        InitializeCompactUiIfNeeded()
    End Sub

    Private Sub FrmPosEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigureMode()

        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        If IsEditMode Then
            PrefillSearchForEdit()
        Else
            LoadSearchResults(String.Empty)
        End If
    End Sub

    Private Sub FrmPosEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
        If _txtSearchQuery IsNot Nothing AndAlso Not _txtSearchQuery.IsDisposed Then
            _txtSearchQuery.Focus()
        End If
    End Sub

    Private Sub InitializeCompactUiIfNeeded()
        If _uiInitialized Then Return

        SuspendLayout()
        Try
            AutoScaleMode = AutoScaleMode.None
            AutoSize = False
            AutoScroll = False
            FormBorderStyle = FormBorderStyle.None
            BackColor = Color.FromArgb(28, 29, 33)
            ForeColor = Color.FromArgb(238, 241, 245)

            HideLegacyDesignerControls()

            Dim pnlMain As New Panel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(UiOuterPadding),
                .BackColor = Color.FromArgb(28, 29, 33),
                .AutoScroll = False
            }

            Dim pnlHeader As New Panel With {
                .Dock = DockStyle.Top,
                .Height = UiHeaderHeight,
                .BackColor = Color.FromArgb(28, 29, 33),
                .AutoScroll = False
            }

            Label12.AutoSize = True
            Label12.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
            Label12.ForeColor = Color.FromArgb(245, 247, 250)
            Label12.BackColor = pnlHeader.BackColor
            Label12.Location = New Point(0, UiTitleTopInset)
            Label12.Margin = Padding.Empty

            _btnCloseProxy = New Guna2Button()
            StyleCloseButton(_btnCloseProxy)
            _btnCloseProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            _btnCloseProxy.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - UiHeaderCloseRightInset - _btnCloseProxy.Width), UiHeaderCloseTopInset)
            AddHandler _btnCloseProxy.Click, Sub() CancelAndClose()
            AddHandler pnlHeader.Resize,
                Sub()
                    If _btnCloseProxy Is Nothing OrElse _btnCloseProxy.IsDisposed Then Return
                    _btnCloseProxy.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - UiHeaderCloseRightInset - _btnCloseProxy.Width), UiHeaderCloseTopInset)
                End Sub

            Dim pnlHeaderDivider As New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = 1,
                .BackColor = Color.FromArgb(56, 60, 68)
            }

            pnlHeader.Controls.Add(_btnCloseProxy)
            pnlHeader.Controls.Add(Label12)
            pnlHeader.Controls.Add(pnlHeaderDivider)

            Dim pnlBodyHost As New Panel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(0, UiBodyTopGap, 0, 0),
                .BackColor = Color.FromArgb(28, 29, 33),
                .AutoScroll = False
            }

            Dim pnlCard As New Guna2Panel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(UiCardPadding),
                .FillColor = Color.FromArgb(34, 36, 42),
                .BorderColor = Color.FromArgb(58, 61, 68),
                .BorderThickness = 1,
                .BorderRadius = 14,
                .Margin = Padding.Empty
            }
            pnlCard.AutoScroll = False

            Dim pnlTop As New Panel With {
                .Dock = DockStyle.Top,
                .Height = UiTopRowHeight,
                .Padding = New Padding(0, 0, 0, UiTopRowBottomGap),
                .BackColor = Color.Transparent,
                .AutoScroll = False
            }

            _txtSearchQuery = New Guna2TextBox With {
                .Dock = DockStyle.Fill,
                .Height = UiSearchBoxHeight,
                .PlaceholderText = "Type product name or barcode"
            }
            StyleSearchTextBox(_txtSearchQuery)
            AddHandler _txtSearchQuery.TextChanged, AddressOf SearchQuery_TextChanged
            Try
                BlockCopyPaste(_txtSearchQuery)
            Catch
            End Try

            pnlTop.Controls.Add(_txtSearchQuery)

            Dim pnlBottom As New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = UiBottomRowHeight,
                .Padding = New Padding(0, UiBottomRowTopGap, 0, 0),
                .BackColor = Color.Transparent,
                .AutoScroll = False
            }

            _btnClose = New Guna2Button With {
                .Dock = DockStyle.Right,
                .Width = UiFooterButtonWidth,
                .Text = "Close"
            }
            StyleSecondaryButton(_btnClose)
            AddHandler _btnClose.Click, Sub() CancelAndClose()

            pnlBottom.Controls.Add(_btnClose)

            _gridResults = New Guna2DataGridView With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False,
                .AllowUserToResizeRows = False,
                .AllowUserToResizeColumns = False,
                .RowHeadersVisible = False,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                .ColumnHeadersHeight = 38,
                .ScrollBars = ScrollBars.Vertical,
                .Margin = Padding.Empty
            }
            _gridResults.RowTemplate.Height = ResultGridRowHeight
            AddHandler _gridResults.CellContentClick, AddressOf ResultsGrid_CellContentClick
            ApplyGridTheme(_gridResults)

            pnlCard.Controls.Add(_gridResults)
            pnlCard.Controls.Add(pnlBottom)
            pnlCard.Controls.Add(pnlTop)

            pnlBodyHost.Controls.Add(pnlCard)
            pnlMain.Controls.Add(pnlBodyHost)
            pnlMain.Controls.Add(pnlHeader)
            Controls.Add(pnlMain)
        Finally
            ResumeLayout(True)
            PerformLayout()
        End Try

        _uiInitialized = True
    End Sub

    Private Sub HideLegacyDesignerControls()
        For Each ctl As Control In New Control() {
            Panel8, Button3, Label1, txtProductName, lblPlaceholder, txtSearch, Panel2,
            txtQuantity, Label5, btnAdd, btnEdit, btnExit
        }
            If ctl Is Nothing Then Continue For
            ctl.Visible = False
            ctl.Location = New Point(-2000, -2000)
            ctl.Size = New Size(1, 1)
        Next
    End Sub

    Private Sub ConfigureMode()
        Text = If(IsEditMode, "Edit Product", "Add Product")
        Label12.Text = If(IsEditMode, "Edit Product", "Add Product")
    End Sub

    Private Sub SearchQuery_TextChanged(sender As Object, e As EventArgs)
        If _txtSearchQuery Is Nothing OrElse _txtSearchQuery.IsDisposed Then Return
        LoadSearchResults(_txtSearchQuery.Text)
    End Sub

    Private Sub PrefillSearchForEdit()
        Dim seed As String = SelectedBarcode
        If String.IsNullOrWhiteSpace(seed) Then
            seed = SelectedProductName
        End If

        If _txtSearchQuery Is Nothing OrElse _txtSearchQuery.IsDisposed Then Return
        If String.IsNullOrWhiteSpace(seed) Then
            LoadSearchResults(String.Empty)
            Return
        End If

        _txtSearchQuery.Text = seed.Trim()
        _txtSearchQuery.SelectionStart = _txtSearchQuery.TextLength
        _txtSearchQuery.SelectionLength = 0
    End Sub

    Private Sub LoadSearchResults(searchText As String)
        Dim normalized As String = If(searchText, String.Empty).Trim()
        If String.IsNullOrWhiteSpace(normalized) Then
            ClearSearchResultsGrid()
            Return
        End If

        Dim records As DataTable = SearchPostedInventory(normalized)
        BindSearchResults(records)
    End Sub

    Private Function SearchPostedInventory(searchText As String) As DataTable
        Dim dt As New DataTable()
        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   p.ProductID,
                   p.BarcodeNumber,
                   p.Product,
                   ISNULL(c.Category, '') AS Category,
                   ISNULL(s.Size, '') AS Size,
                   ISNULL(b.Brand, '') AS Brand,
                   ISNULL(col.Color, '') AS Color,
                   dp.Quantity AS AvailableQty,
                   ISNULL(p.CostPrice, 0) AS CostPrice,
                   ISNULL(p.SellingPrice, 0) AS SellingPrice,
                   ISNULL(p.ImagePath, '') AS ImagePath,
                   d.DateCreated
            FROM tbl_delivery_products dp
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            LEFT JOIN tbl_Category c ON p.CategoryID = c.CategoryID
            LEFT JOIN tbl_Size s ON p.SizeID = s.SizeID
            LEFT JOIN tbl_Brand b ON p.BrandID = b.BrandID
            LEFT JOIN tbl_Color col ON p.ColorID = col.ColorID
            WHERE dp.Status = 'Posted'
              AND dp.Quantity > 0
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
            ORDER BY d.DateCreated DESC, dp.DeliveryProductID DESC;"

        Dim searchValue As String = If(String.IsNullOrWhiteSpace(searchText), String.Empty, "%" & searchText.Trim() & "%")

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@search", searchValue)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Private Sub BindSearchResults(records As DataTable)
        Dim boundTable As DataTable = If(records, New DataTable())
        EnsureProductImagePreviewData(boundTable)

        _gridResults.DataSource = Nothing
        _gridResults.Columns.Clear()
        _gridResults.DataSource = boundTable

        EnsureAddButtonColumn()
        ApplyStandardGridLayout(_gridResults, ResultGridRowHeight, 70)
        ConfigureGridColumns()
        ApplyGridTheme(_gridResults)
        _gridResults.ScrollBars = ScrollBars.Vertical
        _gridResults.ClearSelection()
    End Sub

    Private Sub ClearSearchResultsGrid()
        If _gridResults Is Nothing OrElse _gridResults.IsDisposed Then Return

        _gridResults.DataSource = Nothing
        _gridResults.Columns.Clear()
        ApplyGridTheme(_gridResults)
        _gridResults.ClearSelection()
    End Sub

    Private Sub EnsureProductImagePreviewData(dt As DataTable)
        If dt Is Nothing Then Return

        If Not dt.Columns.Contains(ColProductImagePreview) Then
            dt.Columns.Add(ColProductImagePreview, GetType(Image))
        End If

        For Each row As DataRow In dt.Rows
            If row.RowState = DataRowState.Deleted Then Continue For
            If dt.Columns.Contains(ColProductImagePreview) AndAlso
               Not row.IsNull(ColProductImagePreview) AndAlso
               TypeOf row(ColProductImagePreview) Is Image Then
                Continue For
            End If

            row(ColProductImagePreview) = ResolveProductPreviewImage(row)
        Next
    End Sub

    Private Function ResolveProductPreviewImage(row As DataRow) As Image
        If row Is Nothing OrElse row.Table Is Nothing Then
            Return GetProductImagePlaceholder()
        End If

        Dim imageSourceColumns() As String = {"ProductImage", "ProductImageBytes", "ImageBytes", "ImageBlob", "Image"}
        For Each columnName As String In imageSourceColumns
            If Not row.Table.Columns.Contains(columnName) Then Continue For

            Dim resolved As Image = CreateImageFromUnknownSource(row(columnName))
            If resolved IsNot Nothing Then
                Return resolved
            End If
        Next

        If row.Table.Columns.Contains("ImagePath") Then
            Dim fromPath As Image = CreateImageFromUnknownSource(row("ImagePath"))
            If fromPath IsNot Nothing Then
                Return fromPath
            End If
        End If

        Return GetProductImagePlaceholder()
    End Function

    Private Function CreateImageFromUnknownSource(value As Object) As Image
        If value Is Nothing OrElse value Is DBNull.Value Then Return Nothing

        If TypeOf value Is Image Then
            Try
                Return New Bitmap(DirectCast(value, Image))
            Catch
                Return Nothing
            End Try
        End If

        If TypeOf value Is Byte() Then
            Dim bytes As Byte() = DirectCast(value, Byte())
            If bytes.Length = 0 Then Return Nothing

            Try
                Using ms As New IO.MemoryStream(bytes)
                    Using tempImage As Image = Image.FromStream(ms)
                        Return New Bitmap(tempImage)
                    End Using
                End Using
            Catch
                Return Nothing
            End Try
        End If

        Dim path As String = TryCast(value, String)
        If String.IsNullOrWhiteSpace(path) Then
            path = Convert.ToString(value)
        End If

        Return LoadImageFromPath(path)
    End Function

    Private Function LoadImageFromPath(path As String) As Image
        If String.IsNullOrWhiteSpace(path) Then Return Nothing

        Dim normalizedPath As String = path.Trim()
        Dim candidatePaths As New System.Collections.Generic.List(Of String) From {normalizedPath}

        Dim appRelativePath As String = IO.Path.Combine(Application.StartupPath, normalizedPath.TrimStart("\"c, "/"c))
        If Not candidatePaths.Contains(appRelativePath) Then
            candidatePaths.Add(appRelativePath)
        End If

        For Each candidate As String In candidatePaths
            If String.IsNullOrWhiteSpace(candidate) OrElse Not IO.File.Exists(candidate) Then Continue For

            Try
                Using tempImage As Image = Image.FromFile(candidate)
                    Return New Bitmap(tempImage)
                End Using
            Catch
            End Try
        Next

        Return Nothing
    End Function

    Private Shared Function GetProductImagePlaceholder() As Image
        If _productImagePlaceholderCache IsNot Nothing Then
            Return _productImagePlaceholderCache
        End If

        Dim placeholderCandidates As New System.Collections.Generic.List(Of String) From {
            IO.Path.Combine(Application.StartupPath, "Resources", "no_image_available.png"),
            IO.Path.Combine(Application.StartupPath, "no_image_available.png"),
            IO.Path.GetFullPath(IO.Path.Combine(Application.StartupPath, "..", "..", "Resources", "no_image_available.png")),
            IO.Path.GetFullPath(IO.Path.Combine(Application.StartupPath, "..", "..", "..", "Resources", "no_image_available.png"))
        }

        For Each candidate As String In placeholderCandidates
            Try
                If String.IsNullOrWhiteSpace(candidate) OrElse Not IO.File.Exists(candidate) Then Continue For
                Using tempImage As Image = Image.FromFile(candidate)
                    _productImagePlaceholderCache = New Bitmap(tempImage)
                    Exit For
                End Using
            Catch
            End Try
        Next

        If _productImagePlaceholderCache Is Nothing Then
            ' Fallback custom placeholder that matches the small boxed red-X image style.
            Dim bmp As New Bitmap(18, 18)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.Clear(Color.White)
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
                Using borderPen As New Pen(Color.FromArgb(150, 150, 150), 1.0F),
                      xPen As New Pen(Color.FromArgb(230, 0, 0), 2.0F)
                    g.DrawRectangle(borderPen, 0, 0, bmp.Width - 1, bmp.Height - 1)
                    g.DrawLine(xPen, 4, 4, bmp.Width - 5, bmp.Height - 5)
                    g.DrawLine(xPen, bmp.Width - 5, 4, 4, bmp.Height - 5)
                End Using
            End Using
            _productImagePlaceholderCache = bmp
        End If

        Return _productImagePlaceholderCache
    End Function

    Private Sub EnsureAddButtonColumn()
        If _gridResults.Columns.Contains(ColAddAction) Then Return

        Dim col As New DataGridViewButtonColumn With {
            .Name = ColAddAction,
            .HeaderText = String.Empty,
            .Text = "Add",
            .UseColumnTextForButtonValue = True,
            .Width = 70,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .ReadOnly = False
        }
        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        _gridResults.Columns.Add(col)
    End Sub

    Private Sub ConfigureGridColumns()
        For Each hidden As String In {"DateCreated", "ImagePath", "CostPrice", "ProductID", "DeliveryProductID", "Size", "Brand", "Color"}
            GridHelpers.ApplyColumnSetup(_gridResults, hidden, Sub(c) c.Visible = False)
        Next

        Dim previewColumn As DataGridViewColumn = Nothing
        If GridHelpers.TryGetColumn(_gridResults, previewColumn, ColProductImagePreview) Then
            previewColumn.HeaderText = "Product Image"
            previewColumn.DisplayIndex = 0
            previewColumn.Width = ResultGridImageColumnWidth
            previewColumn.MinimumWidth = ResultGridImageColumnWidth
            previewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None

            Dim imageColumn As DataGridViewImageColumn = TryCast(previewColumn, DataGridViewImageColumn)
            If imageColumn IsNot Nothing Then
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
                imageColumn.DefaultCellStyle.NullValue = GetProductImagePlaceholder()
            End If
        End If

        GridHelpers.ApplyColumnSetup(_gridResults, "BarcodeNumber", Sub(c)
                                                                        c.HeaderText = "Barcode"
                                                                        c.FillWeight = 18
                                                                        c.MinimumWidth = 95
                                                                    End Sub)
        GridHelpers.ApplyColumnSetup(_gridResults, "Product", Sub(c)
                                                                  c.HeaderText = "Product Name"
                                                                  c.FillWeight = 30
                                                                  c.MinimumWidth = 160
                                                              End Sub)
        GridHelpers.ApplyColumnSetup(_gridResults, "Category", Sub(c)
                                                                   c.FillWeight = 14
                                                                   c.MinimumWidth = 85
                                                               End Sub)
        GridHelpers.ApplyColumnSetup(_gridResults, "AvailableQty", Sub(c)
                                                                        c.HeaderText = "Stock"
                                                                        c.FillWeight = 10
                                                                        c.MinimumWidth = 70
                                                                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                                    End Sub)
        GridHelpers.ApplyColumnSetup(_gridResults, "SellingPrice", Sub(c)
                                                                        c.HeaderText = "Price"
                                                                        c.FillWeight = 12
                                                                        c.MinimumWidth = 80
                                                                        c.DefaultCellStyle.Format = "N2"
                                                                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                                    End Sub)
        GridHelpers.ApplyColumnSetup(_gridResults, ColAddAction, Sub(c)
                                                                     c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                     c.Width = 70
                                                                     c.MinimumWidth = 70
                                                                     c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                                 End Sub)
    End Sub

    Private Sub ResultsGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If GridHelpers.GetColumnNameByIndex(_gridResults, e.ColumnIndex) <> ColAddAction Then Return

        Dim item As PosInventoryResult = MapGridRow(e.RowIndex)
        If item.DeliveryProductID <= 0 OrElse item.ProductID <= 0 Then
            MessageBox.Show("Unable to read the selected product.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim qty As Integer? = PromptForQuantity(item)
        If Not qty.HasValue Then Return

        selectedID = item.DeliveryProductID
        ProductID = item.ProductID
        SelectedProductName = item.ProductName
        SelectedBarcode = item.Barcode
        SelectedQuantity = qty.Value
        SelectedSellingPrice = item.SellingPrice
        SelectedImagePath = item.ImagePath
        selectedCostPrice = item.CostPrice
        SelectedCategory = item.Category

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Function PromptForQuantity(item As PosInventoryResult) As Integer?
        Dim availableQty As Integer = Math.Max(0, item.AvailableQty)
        If availableQty <= 0 Then
            MessageBox.Show("This item is out of stock.", "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        End If

        Dim seed As Integer = 1
        If IsEditMode AndAlso selectedID > 0 AndAlso selectedID = item.DeliveryProductID AndAlso SelectedQuantity > 0 Then
            seed = SelectedQuantity
        End If
        seed = Math.Max(1, Math.Min(seed, availableQty))

        Do
            Dim input As String = Interaction.InputBox(
                "Enter quantity for this item:" & Environment.NewLine &
                item.ProductName & Environment.NewLine &
                "Available stock: " & availableQty.ToString(),
                If(IsEditMode, "Edit Quantity", "Add Quantity"),
                seed.ToString())

            If String.IsNullOrWhiteSpace(input) Then Return Nothing

            Dim qty As Integer
            If Not Integer.TryParse(input.Trim(), qty) OrElse qty <= 0 Then
                MessageBox.Show("Quantity must be numeric and greater than 0.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Continue Do
            End If

            If qty > availableQty Then
                MessageBox.Show($"Quantity cannot be higher than available stock ({availableQty}).", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Continue Do
            End If

            Return qty
        Loop
    End Function

    Private Function MapGridRow(rowIndex As Integer) As PosInventoryResult
        Dim result As New PosInventoryResult()
        If rowIndex < 0 OrElse rowIndex >= _gridResults.Rows.Count Then Return result

        Dim row As DataGridViewRow = _gridResults.Rows(rowIndex)
        result.DeliveryProductID = ReadInt(row, "DeliveryProductID")
        result.ProductID = ReadInt(row, "ProductID")
        result.Barcode = ReadText(row, "BarcodeNumber")
        result.ProductName = ReadText(row, "Product")
        result.Category = ReadText(row, "Category")
        result.AvailableQty = ReadInt(row, "AvailableQty")
        result.SellingPrice = ReadDecimal(row, "SellingPrice")
        result.CostPrice = ReadDecimal(row, "CostPrice")
        result.ImagePath = ReadText(row, "ImagePath")
        Return result
    End Function

    Private Shared Function ReadText(row As DataGridViewRow, columnName As String) As String
        If row Is Nothing OrElse row.DataGridView Is Nothing Then Return String.Empty
        If Not row.DataGridView.Columns.Contains(columnName) Then Return String.Empty
        Dim value As Object = row.Cells(columnName).Value
        If value Is Nothing OrElse value Is DBNull.Value Then Return String.Empty
        Return value.ToString().Trim()
    End Function

    Private Shared Function ReadInt(row As DataGridViewRow, columnName As String) As Integer
        Dim txt As String = ReadText(row, columnName)
        Dim parsed As Integer
        If Integer.TryParse(txt, parsed) Then Return parsed
        Return 0
    End Function

    Private Shared Function ReadDecimal(row As DataGridViewRow, columnName As String) As Decimal
        If row Is Nothing OrElse row.DataGridView Is Nothing Then Return 0D
        If Not row.DataGridView.Columns.Contains(columnName) Then Return 0D

        Dim value As Object = row.Cells(columnName).Value
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D

        Try
            Return Convert.ToDecimal(value)
        Catch
            Dim parsed As Decimal
            If Decimal.TryParse(Convert.ToString(value), parsed) Then Return parsed
            Return 0D
        End Try
    End Function

    Private Sub CancelAndClose()
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        CancelAndClose()
    End Sub

    Private Shared Sub StyleSearchTextBox(tb As Guna2TextBox)
        tb.Animated = False
        tb.AutoRoundedCorners = False
        tb.BorderRadius = 12
        tb.BorderThickness = 1
        tb.BorderColor = Color.FromArgb(76, 80, 88)
        tb.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        tb.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
        tb.FillColor = Color.FromArgb(41, 44, 51)
        tb.ForeColor = Color.FromArgb(238, 241, 245)
        tb.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
        tb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        tb.Margin = Padding.Empty
    End Sub

    Private Shared Sub StyleSecondaryButton(button As Guna2Button)
        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 10
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(74, 79, 88)
        button.FillColor = Color.FromArgb(62, 66, 75)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.FromArgb(242, 244, 247)
        button.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        button.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        button.HoverState.ForeColor = Color.White
        button.HoverState.BorderColor = Color.FromArgb(106, 112, 124)
        button.Cursor = Cursors.Hand
    End Sub

    Private Shared Sub StyleCloseButton(button As Guna2Button)
        StyleSecondaryButton(button)
        button.Size = New Size(36, 36)
        button.BorderRadius = 10
        button.Text = String.Empty

        Dim rm As New System.ComponentModel.ComponentResourceManager(GetType(FrmUserEntry))
        Dim closeImage As Image = TryCast(rm.GetObject("btnExit.Image"), Image)
        button.Image = closeImage
        button.ImageSize = New Size(12, 12)

        If closeImage Is Nothing Then
            button.Text = "X"
            button.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        End If
    End Sub

    Private Shared Sub ApplyGridTheme(grid As Guna2DataGridView)
        Dim whiteColor As Color = Color.White
        Dim altRowColor As Color = Color.Gainsboro
        Dim headerColor As Color = Color.LightGray
        Dim gridLineColor As Color = Color.Silver
        Dim selectionColor As Color = Color.FromArgb(219, 232, 250)

        grid.BackgroundColor = whiteColor
        grid.BorderStyle = BorderStyle.None
        grid.GridColor = gridLineColor
        grid.ForeColor = Color.Black
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        grid.ColumnHeadersDefaultCellStyle.BackColor = headerColor
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerColor
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black

        grid.DefaultCellStyle.BackColor = whiteColor
        grid.DefaultCellStyle.ForeColor = Color.Black
        grid.DefaultCellStyle.SelectionBackColor = selectionColor
        grid.DefaultCellStyle.SelectionForeColor = Color.Black
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False

        grid.RowsDefaultCellStyle.BackColor = whiteColor
        grid.RowsDefaultCellStyle.ForeColor = Color.Black
        grid.RowsDefaultCellStyle.SelectionBackColor = selectionColor
        grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black

        grid.AlternatingRowsDefaultCellStyle.BackColor = altRowColor
        grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectionColor
        grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black

        grid.ThemeStyle.BackColor = whiteColor
        grid.ThemeStyle.GridColor = grid.GridColor
        grid.ThemeStyle.HeaderStyle.BackColor = grid.ColumnHeadersDefaultCellStyle.BackColor
        grid.ThemeStyle.HeaderStyle.ForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor
        grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        grid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.ThemeStyle.HeaderStyle.Height = 38
        grid.ThemeStyle.RowsStyle.BackColor = grid.DefaultCellStyle.BackColor
        grid.ThemeStyle.RowsStyle.ForeColor = grid.DefaultCellStyle.ForeColor
        grid.ThemeStyle.RowsStyle.SelectionBackColor = grid.DefaultCellStyle.SelectionBackColor
        grid.ThemeStyle.RowsStyle.SelectionForeColor = grid.DefaultCellStyle.SelectionForeColor
        grid.ThemeStyle.RowsStyle.Height = ResultGridRowHeight
        grid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        grid.ThemeStyle.AlternatingRowsStyle.BackColor = grid.AlternatingRowsDefaultCellStyle.BackColor
        grid.ThemeStyle.AlternatingRowsStyle.ForeColor = grid.AlternatingRowsDefaultCellStyle.ForeColor
        grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = grid.AlternatingRowsDefaultCellStyle.SelectionBackColor
        grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = grid.AlternatingRowsDefaultCellStyle.SelectionForeColor
        grid.ThemeStyle.ReadOnly = True

        For Each col As DataGridViewColumn In grid.Columns
            If TypeOf col Is DataGridViewButtonColumn Then
                col.DefaultCellStyle.BackColor = whiteColor
                col.DefaultCellStyle.ForeColor = Color.Black
                col.DefaultCellStyle.SelectionBackColor = selectionColor
                col.DefaultCellStyle.SelectionForeColor = Color.Black
            End If
        Next
    End Sub

    Private Structure PosInventoryResult
        Public DeliveryProductID As Integer
        Public ProductID As Integer
        Public Barcode As String
        Public ProductName As String
        Public Category As String
        Public AvailableQty As Integer
        Public SellingPrice As Decimal
        Public CostPrice As Decimal
        Public ImagePath As String
    End Structure
End Class
