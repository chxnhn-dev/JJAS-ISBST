Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections.Generic
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Public Class FrmProductEntry
    Public Property ProductID As Integer?
    Public Property LockSellingPrice As Boolean

    Private ReadOnly _productService As New ProductService()
    Private ReadOnly _restrictedFieldSnapshots As New Dictionary(Of Control, Object)()
    Private ReadOnly cbStatus As New Guna2ComboBox()
    Private _currentImagePath As String = String.Empty
    Private _loadedProductRow As DataRow
    Private _modernUiInitialized As Boolean
    Private _hasImagePreview As Boolean
    Private _productUsage As ProductUsageInfo
    Private _isStructuralEditLocked As Boolean
    Private _loadingProductData As Boolean
    Private _pnlImagePlaceholder As Panel
    Private _lnkRemoveImage As LinkLabel
    Private _lblSubtitle As Label
    Private _lastRestrictedFieldWarning As DateTime = DateTime.MinValue
    Private _pnlFields As Panel
    Private _pnlActions As Panel
    Private _suppressRestrictedFieldGuards As Boolean
    Private _validationPlaceholders As New List(Of Label)()
    Private _queuedEditorRefresh As Boolean
    Private _suppressCategorySizeReload As Boolean

    Public Sub New()
        InitializeComponent()
        InitializeModernUiIfNeeded()
    End Sub

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return ProductID.HasValue
        End Get
    End Property

    Private Sub Add_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        BlockCopyPaste(txtProductName)
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtBarcodeNumber)
        BlockCopyPaste(txtCostPrice)
        BlockCopyPaste(txtSellingPrice)

        InitializeStatusCombo()
        WireRestrictedFieldGuards()
        LoadLookupCombos()

        ConfigureMode()

        If IsEditMode Then
            LoadProductForEdit()
        Else
            RefreshProductEditRestrictions()
        End If

        UpdateImagePlaceholderVisibility()
        QueueEditorVisualRefresh()
    End Sub

    Private Sub FrmProductEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        QueueEditorVisualRefresh()
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub LoadLookupCombos()
        _suppressCategorySizeReload = True
        ResetSizeCombo()
        LoadCategories()
        LoadComboBox(cbBrand, "tbl_Brand", "Brand", "BrandID")
        LoadComboBox(cbColor, "tbl_Color", "Color", "ColorID")
        _suppressCategorySizeReload = False
        ResetSizeCombo()
    End Sub

    Private Sub InitializeStatusCombo()
        _suppressRestrictedFieldGuards = True

        Try
            cbStatus.Items.Clear()
            cbStatus.Items.Add("Active")
            cbStatus.Items.Add("Inactive")
            If cbStatus.SelectedIndex < 0 Then
                cbStatus.SelectedIndex = 0
            End If
        Finally
            _suppressRestrictedFieldGuards = False
        End Try
    End Sub

    Private Function GetSelectedProductActiveStatus() As Boolean
        Return cbStatus.SelectedIndex <> 1
    End Function

    Private Sub SetSelectedProductActiveStatus(isActive As Boolean)
        cbStatus.SelectedIndex = If(isActive, 0, 1)
    End Sub

    Private Sub RefreshProductEditRestrictions()
        If IsEditMode AndAlso ProductID.HasValue Then
            _productUsage = _productService.GetProductUsageInfo(ProductID.Value)
            _isStructuralEditLocked = _productUsage IsNot Nothing AndAlso _productUsage.HasReferences
        Else
            _productUsage = Nothing
            _isStructuralEditLocked = False
        End If

        ApplyProductFieldStates()
        CaptureRestrictedFieldSnapshots()
        QueueEditorVisualRefresh()
    End Sub

    Private Sub ApplyProductFieldStates()
        ApplyTextFieldState(txtBarcodeNumber, _isStructuralEditLocked)
        ApplyTextFieldState(txtCostPrice, _isStructuralEditLocked)
        ApplyTextFieldState(txtDescription, _isStructuralEditLocked)

        ApplyComboFieldState(cbCategory, _isStructuralEditLocked)
        ApplyComboFieldState(cbBrand, _isStructuralEditLocked)
        ApplyComboFieldState(cbSize, _isStructuralEditLocked)
        ApplyComboFieldState(cbColor, _isStructuralEditLocked)

        ApplyTextFieldState(txtProductName, False)
        ApplyTextFieldState(txtSellingPrice, LockSellingPrice)
        ApplyComboFieldState(cbStatus, False)

        btnClear.Text = If(IsEditMode AndAlso _isStructuralEditLocked, "Reset", "Clear")
    End Sub

    Private Sub ApplyTextFieldState(tb As Guna2TextBox, isReadOnly As Boolean)
        If tb Is Nothing Then
            Return
        End If

        tb.ReadOnly = isReadOnly
        tb.TabStop = Not isReadOnly
        tb.Cursor = If(isReadOnly, Cursors.No, Cursors.IBeam)
        tb.FillColor = If(isReadOnly, Color.FromArgb(55, 58, 66), Color.FromArgb(41, 44, 51))
        tb.BorderColor = If(isReadOnly, Color.FromArgb(88, 92, 100), Color.FromArgb(76, 80, 88))
        tb.FocusedState.BorderColor = If(isReadOnly, tb.BorderColor, Color.FromArgb(0, 122, 204))
        tb.HoverState.BorderColor = If(isReadOnly, tb.BorderColor, Color.FromArgb(108, 114, 124))
        tb.ForeColor = If(isReadOnly, Color.FromArgb(208, 212, 220), Color.FromArgb(238, 241, 245))
    End Sub

    Private Sub ApplyComboFieldState(cbo As Guna2ComboBox, isLocked As Boolean)
        If cbo Is Nothing Then
            Return
        End If

        cbo.TabStop = Not isLocked
        cbo.Cursor = If(isLocked, Cursors.No, Cursors.Hand)
        cbo.FillColor = If(isLocked, Color.FromArgb(55, 58, 66), Color.FromArgb(41, 44, 51))
        cbo.BorderColor = If(isLocked, Color.FromArgb(88, 92, 100), Color.FromArgb(76, 80, 88))
        cbo.FocusedColor = If(isLocked, cbo.BorderColor, Color.FromArgb(0, 122, 204))
        cbo.FocusedState.BorderColor = If(isLocked, cbo.BorderColor, Color.FromArgb(0, 122, 204))
        cbo.HoverState.BorderColor = If(isLocked, cbo.BorderColor, Color.FromArgb(108, 114, 124))
        cbo.ForeColor = If(isLocked, Color.FromArgb(208, 212, 220), Color.FromArgb(238, 241, 245))
    End Sub

    Private Sub CaptureRestrictedFieldSnapshots()
        _restrictedFieldSnapshots.Clear()
        _restrictedFieldSnapshots(txtBarcodeNumber) = txtBarcodeNumber.Text
        _restrictedFieldSnapshots(txtCostPrice) = txtCostPrice.Text
        _restrictedFieldSnapshots(txtDescription) = txtDescription.Text
        _restrictedFieldSnapshots(cbCategory) = If(_loadedProductRow Is Nothing, CType(0, Object), ReadRowInt(_loadedProductRow, "CategoryID"))
        _restrictedFieldSnapshots(cbBrand) = If(_loadedProductRow Is Nothing, CType(0, Object), ReadRowInt(_loadedProductRow, "BrandID"))
        _restrictedFieldSnapshots(cbSize) = If(_loadedProductRow Is Nothing, CType(0, Object), ReadRowInt(_loadedProductRow, "SizeID"))
        _restrictedFieldSnapshots(cbColor) = If(_loadedProductRow Is Nothing, CType(0, Object), ReadRowInt(_loadedProductRow, "ColorID"))
    End Sub

    Private Sub WireRestrictedFieldGuards()
        For Each tb As Guna2TextBox In {txtBarcodeNumber, txtCostPrice, txtDescription}
            RemoveHandler tb.MouseDown, AddressOf RestrictedTextField_MouseDown
            RemoveHandler tb.KeyDown, AddressOf RestrictedTextField_KeyDown
            AddHandler tb.MouseDown, AddressOf RestrictedTextField_MouseDown
            AddHandler tb.KeyDown, AddressOf RestrictedTextField_KeyDown
        Next

        For Each cbo As Guna2ComboBox In {cbCategory, cbBrand, cbSize, cbColor}
            RemoveHandler cbo.DropDown, AddressOf RestrictedCombo_DropDown
            RemoveHandler cbo.KeyDown, AddressOf RestrictedCombo_KeyDown
            RemoveHandler cbo.SelectionChangeCommitted, AddressOf RestrictedCombo_SelectionChangeCommitted
            AddHandler cbo.DropDown, AddressOf RestrictedCombo_DropDown
            AddHandler cbo.KeyDown, AddressOf RestrictedCombo_KeyDown
            AddHandler cbo.SelectionChangeCommitted, AddressOf RestrictedCombo_SelectionChangeCommitted
        Next
    End Sub

    Private Sub RestrictedTextField_MouseDown(sender As Object, e As MouseEventArgs)
        If Not _isStructuralEditLocked OrElse _suppressRestrictedFieldGuards Then
            Return
        End If

        ShowRestrictedFieldMessage()
    End Sub

    Private Sub RestrictedTextField_KeyDown(sender As Object, e As KeyEventArgs)
        If Not _isStructuralEditLocked OrElse _suppressRestrictedFieldGuards Then
            Return
        End If

        If e.KeyCode = Keys.Tab OrElse e.KeyCode = Keys.ShiftKey OrElse e.KeyCode = Keys.ControlKey OrElse e.KeyCode = Keys.Menu Then
            Return
        End If

        e.SuppressKeyPress = True
        ShowRestrictedFieldMessage()
    End Sub

    Private Sub RestrictedCombo_DropDown(sender As Object, e As EventArgs)
        If Not _isStructuralEditLocked OrElse _suppressRestrictedFieldGuards Then
            Return
        End If

        Dim cbo As Guna2ComboBox = TryCast(sender, Guna2ComboBox)
        If cbo IsNot Nothing Then
            _suppressRestrictedFieldGuards = True
            Try
                cbo.DroppedDown = False
                RestoreRestrictedControlValue(cbo)
            Finally
                _suppressRestrictedFieldGuards = False
            End Try
        End If

        ShowRestrictedFieldMessage()
    End Sub

    Private Sub RestrictedCombo_KeyDown(sender As Object, e As KeyEventArgs)
        If Not _isStructuralEditLocked OrElse _suppressRestrictedFieldGuards Then
            Return
        End If

        If e.KeyCode = Keys.Tab OrElse e.KeyCode = Keys.ShiftKey OrElse e.KeyCode = Keys.ControlKey OrElse e.KeyCode = Keys.Menu Then
            Return
        End If

        e.SuppressKeyPress = True
        RestoreRestrictedControlValue(TryCast(sender, Control))
        ShowRestrictedFieldMessage()
    End Sub

    Private Sub RestrictedCombo_SelectionChangeCommitted(sender As Object, e As EventArgs)
        If Not _isStructuralEditLocked OrElse _suppressRestrictedFieldGuards Then
            Return
        End If

        RestoreRestrictedControlValue(TryCast(sender, Control))
        ShowRestrictedFieldMessage()
    End Sub

    Private Sub RestoreRestrictedControlValue(control As Control)
        If control Is Nothing OrElse Not _restrictedFieldSnapshots.ContainsKey(control) Then
            Return
        End If

        _suppressRestrictedFieldGuards = True

        Try
            If TypeOf control Is Guna2TextBox Then
                DirectCast(control, Guna2TextBox).Text = Convert.ToString(_restrictedFieldSnapshots(control))
                Return
            End If

            Dim cbo As Guna2ComboBox = TryCast(control, Guna2ComboBox)
            If cbo Is Nothing Then
                Return
            End If

            Dim snapshot As Object = _restrictedFieldSnapshots(control)
            Dim selectedId As Integer = 0

            If snapshot IsNot Nothing Then
                Integer.TryParse(snapshot.ToString(), selectedId)
            End If

            If selectedId > 0 Then
                cbo.SelectedValue = selectedId
            ElseIf cbo.Items.Count > 0 Then
                cbo.SelectedIndex = 0
            End If
        Finally
            _suppressRestrictedFieldGuards = False
        End Try
    End Sub

    Private Sub ShowRestrictedFieldMessage()
        Dim now As DateTime = DateTime.UtcNow
        If (now - _lastRestrictedFieldWarning).TotalMilliseconds < 400 Then
            Return
        End If

        _lastRestrictedFieldWarning = now
        MessageBox.Show(StructuralFieldsLockedMessage, "Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Product"
            btnAdd.Text = "Update"
            btnClear.Text = "Clear"
            If lblTitle IsNot Nothing Then lblTitle.Text = "Edit Product"
        Else
            Me.Text = "Add Product"
            btnAdd.Text = "Save"
            btnClear.Text = "Clear"
            If lblTitle IsNot Nothing Then lblTitle.Text = "Add Product"
        End If
    End Sub

    Private Sub LoadProductForEdit()
        If Not ProductID.HasValue Then
            Return
        End If

        Dim row As DataRow = _productService.GetProductById(ProductID.Value)
        If row Is Nothing Then
            MessageBox.Show("Selected product record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        _loadedProductRow = row
        _loadingProductData = True
        _suppressCategorySizeReload = True
        _suppressRestrictedFieldGuards = True

        Try
            Dim categoryId As Integer = ReadRowInt(row, "CategoryID")
            Dim sizeId As Integer = ReadRowInt(row, "SizeID")

            txtBarcodeNumber.Text = ReadRowString(row, "BarcodeNumber")
            txtProductName.Text = ReadRowString(row, "Product")
            txtCostPrice.Text = ReadRowDecimal(row, "CostPrice").ToString("0.##")
            txtSellingPrice.Text = ReadRowDecimal(row, "SellingPrice").ToString("0.##")
            txtDescription.Text = ReadRowString(row, "Description")

            cbBrand.SelectedValue = ReadRowInt(row, "BrandID")
            cbCategory.SelectedValue = categoryId
            cbColor.SelectedValue = ReadRowInt(row, "ColorID")

            If categoryId > 0 Then
                _suppressCategorySizeReload = False
                LoadSizesByCategory(categoryId)
                _suppressCategorySizeReload = True
                cbSize.SelectedValue = sizeId
            Else
                ResetSizeCombo()
            End If

            SetSelectedProductActiveStatus(ReadRowBoolean(row, "IsActive", True))

            _currentImagePath = ReadRowString(row, "ImagePath")
            If PictureBox1.Image IsNot Nothing Then
                PictureBox1.Image.Dispose()
                PictureBox1.Image = Nothing
            End If
            If Not String.IsNullOrWhiteSpace(_currentImagePath) AndAlso IO.File.Exists(_currentImagePath) Then
                Using tempImg As Image = Image.FromFile(_currentImagePath)
                    PictureBox1.Image = CType(tempImg.Clone(), Image)
                End Using
                PictureBox1.Tag = _currentImagePath
                _hasImagePreview = True
            Else
                PictureBox1.Image = Nothing
                PictureBox1.Tag = Nothing
                _hasImagePreview = False
            End If
        Finally
            _suppressRestrictedFieldGuards = False
            _suppressCategorySizeReload = False
            _loadingProductData = False
        End Try

        RefreshProductEditRestrictions()
        UpdateImagePlaceholderVisibility()
        QueueEditorVisualRefresh()
    End Sub

    Private Sub LoadCategories()
        If cbCategory Is Nothing Then
            Return
        End If

        Dim dt As New DataTable()
        Dim sql As String = "
            SELECT CategoryID,
                   Category
            FROM dbo.tbl_Category
            WHERE IsActive = 1
            ORDER BY Category;"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                Using da As New SqlDataAdapter(command)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Dim newRow As DataRow = dt.NewRow()
        newRow("Category") = "-- Select Category --"
        newRow("CategoryID") = 0
        dt.Rows.InsertAt(newRow, 0)

        cbCategory.DataSource = dt
        cbCategory.DisplayMember = "Category"
        cbCategory.ValueMember = "CategoryID"
        cbCategory.SelectedIndex = 0
    End Sub

    Private Sub LoadSizesByCategory(categoryId As Integer)
        If cbSize Is Nothing Then
            Return
        End If

        cbSize.DataSource = Nothing
        cbSize.Items.Clear()

        Dim dt As New DataTable()
        Dim sql As String = "
            SELECT SizeID,
                   Size
            FROM dbo.tbl_Size
            WHERE IsActive = 1
              AND CategoryID = @CategoryID
            ORDER BY Size;"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId
                Using da As New SqlDataAdapter(command)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Dim newRow As DataRow = dt.NewRow()
        newRow("Size") = "-- Select Size --"
        newRow("SizeID") = 0
        dt.Rows.InsertAt(newRow, 0)

        cbSize.DataSource = dt
        cbSize.DisplayMember = "Size"
        cbSize.ValueMember = "SizeID"
        cbSize.SelectedIndex = 0
        cbSize.Enabled = True
        QueueEditorVisualRefresh()
    End Sub

    Private Sub ResetSizeCombo(Optional placeholderText As String = "-- Select Category First --")
        If cbSize Is Nothing Then
            Return
        End If

        Dim dt As New DataTable()
        dt.Columns.Add("SizeID", GetType(Integer))
        dt.Columns.Add("Size", GetType(String))
        dt.Rows.Add(0, placeholderText)

        cbSize.DataSource = dt
        cbSize.DisplayMember = "Size"
        cbSize.ValueMember = "SizeID"
        cbSize.SelectedIndex = 0
        cbSize.Enabled = False
        QueueEditorVisualRefresh()
    End Sub

    Public Sub LoadComboBox(cbo As ComboBox, tableName As String, displayMember As String, valueMember As String)
        Dim dt As New DataTable()
        Dim sql As String = $"SELECT {valueMember}, {displayMember} FROM {tableName} WHERE IsActive = 1"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using da As New SqlDataAdapter(sql, connection)
                da.Fill(dt)
            End Using
        End Using

        Dim newRow As DataRow = dt.NewRow()
        newRow(displayMember) = "-- Select Option --"
        newRow(valueMember) = 0
        dt.Rows.InsertAt(newRow, 0)

        cbo.DataSource = dt
        cbo.DisplayMember = displayMember
        cbo.ValueMember = valueMember
        cbo.SelectedIndex = 0
    End Sub

    Private Sub cbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCategory.SelectedIndexChanged
        If _suppressCategorySizeReload OrElse cbCategory Is Nothing Then
            Return
        End If

        If _isStructuralEditLocked AndAlso Not _loadingProductData Then
            Return
        End If

        Dim categoryId As Integer
        If Not TryGetComboIntValue(cbCategory, categoryId) OrElse categoryId <= 0 Then
            ResetSizeCombo()
            Return
        End If

        LoadSizesByCategory(categoryId)
    End Sub

    Private Function TryGetComboIntValue(cbo As ComboBox, ByRef value As Integer) As Boolean
        value = 0
        If cbo Is Nothing OrElse cbo.SelectedValue Is Nothing OrElse TypeOf cbo.SelectedValue Is DataRowView Then
            Return False
        End If

        Return Integer.TryParse(cbo.SelectedValue.ToString(), value)
    End Function

    Private Function ReadRowString(row As DataRow, columnName As String) As String
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return String.Empty
        End If

        Return Convert.ToString(row(columnName)).Trim()
    End Function

    Private Function ReadRowInt(row As DataRow, columnName As String) As Integer
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return 0
        End If

        Return Convert.ToInt32(row(columnName))
    End Function

    Private Function ReadRowDecimal(row As DataRow, columnName As String) As Decimal
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return 0D
        End If

        Return Convert.ToDecimal(row(columnName))
    End Function

    Private Function ReadRowBoolean(row As DataRow, columnName As String, Optional defaultValue As Boolean = False) As Boolean
        If row Is Nothing OrElse row.Table Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then
            Return defaultValue
        End If

        Return Convert.ToBoolean(row(columnName))
    End Function

    Private Function IsSizeInSelectedCategory(categoryId As Integer, sizeId As Integer) As Boolean
        Const sql As String = "
            SELECT COUNT(1)
            FROM dbo.tbl_Size
            WHERE SizeID = @SizeID
              AND CategoryID = @CategoryID;"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.Add("@SizeID", SqlDbType.Int).Value = sizeId
                command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId
                connection.Open()

                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_Products WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND ProductID <> @ProductID"
        End If

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@ProductID", ProductID.Value)
                End If

                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub txtSellingPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSellingPrice.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = txtSellingPrice.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtCostPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCostPrice.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = txtCostPrice.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtBarcodeNumber_keypress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtProductName.Text) OrElse String.IsNullOrWhiteSpace(txtSellingPrice.Text) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedBrandId As Integer
        Dim selectedCategoryId As Integer
        Dim selectedColorId As Integer
        Dim selectedSizeId As Integer
        Dim costPriceValue As Decimal
        Dim barcodeValue As String
        Dim descriptionValue As String

        If _isStructuralEditLocked Then
            If _loadedProductRow Is Nothing Then
                MessageBox.Show("Selected product record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            barcodeValue = ReadRowString(_loadedProductRow, "BarcodeNumber")
            descriptionValue = ReadRowString(_loadedProductRow, "Description")
            costPriceValue = ReadRowDecimal(_loadedProductRow, "CostPrice")
            selectedBrandId = ReadRowInt(_loadedProductRow, "BrandID")
            selectedCategoryId = ReadRowInt(_loadedProductRow, "CategoryID")
            selectedColorId = ReadRowInt(_loadedProductRow, "ColorID")
            selectedSizeId = ReadRowInt(_loadedProductRow, "SizeID")
        Else
            If cbBrand.SelectedValue Is Nothing OrElse
               cbSize.SelectedValue Is Nothing OrElse
               cbCategory.SelectedValue Is Nothing OrElse
               cbColor.SelectedValue Is Nothing Then

                MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not TryGetComboIntValue(cbBrand, selectedBrandId) OrElse selectedBrandId <= 0 Then
                MessageBox.Show("Please select a valid Brand.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not TryGetComboIntValue(cbCategory, selectedCategoryId) OrElse selectedCategoryId <= 0 Then
                MessageBox.Show("Please select a valid Category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not TryGetComboIntValue(cbColor, selectedColorId) OrElse selectedColorId <= 0 Then
                MessageBox.Show("Please select a valid Color.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not TryGetComboIntValue(cbSize, selectedSizeId) OrElse selectedSizeId <= 0 Then
                MessageBox.Show("Please select a valid Size.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim costPriceText As String = txtCostPrice.Text.Trim()
            If String.IsNullOrWhiteSpace(costPriceText) Then
                costPriceValue = 0D
            ElseIf Not Decimal.TryParse(costPriceText, costPriceValue) OrElse costPriceValue < 0D Then
                MessageBox.Show("Please enter a valid non-negative Cost Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not IsSizeInSelectedCategory(selectedCategoryId, selectedSizeId) Then
                MessageBox.Show("Selected size does not belong to the selected category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            barcodeValue = txtBarcodeNumber.Text.Trim()
            descriptionValue = txtDescription.Text.Trim()
        End If

        Dim sellingPriceValue As Decimal
        If Not Decimal.TryParse(txtSellingPrice.Text.Trim(), sellingPriceValue) OrElse sellingPriceValue < 0D Then
            MessageBox.Show("Please enter a valid non-negative Selling Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If costPriceValue > sellingPriceValue Then
            MessageBox.Show("Cost Price cannot be greater than Selling Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If _isStructuralEditLocked Then
                txtSellingPrice.Focus()
                txtSellingPrice.SelectAll()
            Else
                txtCostPrice.Focus()
                txtCostPrice.SelectAll()
            End If
            Exit Sub
        End If

        If costPriceValue = sellingPriceValue Then
            MessageBox.Show("Cost Price and Selling Price cannot be the same. Selling Price must be higher than Cost Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSellingPrice.Focus()
            txtSellingPrice.SelectAll()
            Exit Sub
        End If

        If IsDuplicate("Product", txtProductName.Text.Trim()) Then
            MessageBox.Show("Product already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not _isStructuralEditLocked AndAlso IsDuplicate("BarcodeNumber", barcodeValue) Then
            MessageBox.Show("Barcode Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim imagePath As String = _currentImagePath
        If PictureBox1.Tag IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(PictureBox1.Tag.ToString()) Then
            imagePath = PictureBox1.Tag.ToString()
        End If

        If String.IsNullOrWhiteSpace(imagePath) AndAlso Not IsEditMode Then
            Dim defaultImagePath As String = IO.Path.Combine(Application.StartupPath, "Resources\no_image_available.png")
            If IO.File.Exists(defaultImagePath) Then
                imagePath = defaultImagePath
            End If
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Dim request As New ProductSaveRequest With {
                .BarcodeNumber = barcodeValue,
                .ProductName = txtProductName.Text.Trim(),
                .CostPrice = costPriceValue,
                .SellingPrice = sellingPriceValue,
                .Description = descriptionValue,
                .BrandID = selectedBrandId,
                .CategoryID = selectedCategoryId,
                .ColorID = selectedColorId,
                .SizeID = selectedSizeId,
                .ImagePath = imagePath,
                .IsActive = GetSelectedProductActiveStatus(),
                .ChangedByUserID = CurrentUser.UserID,
                .ChangeReason = If(IsEditMode, "Adjusted via Product Entry", "Initial price set")
            }

            _productService.SaveProduct(
                If(IsEditMode, EntryFormMode.EditExisting, EntryFormMode.AddNew),
                If(ProductID.HasValue, ProductID.Value, 0),
                request)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Product.", "Added Product."))
            MessageBox.Show(If(IsEditMode, "Product updated successfully!", "Product added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As InvalidOperationException When String.Equals(ex.Message, StructuralFieldsLockedMessage, StringComparison.Ordinal)
            MessageBox.Show(ex.Message, "Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"

            If ofd.ShowDialog() = DialogResult.OK Then
                Dim imagePath As String = ofd.FileName

                If Not IO.File.Exists(imagePath) Then
                    MessageBox.Show("The selected image file does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                Dim fileInfo As New IO.FileInfo(imagePath)
                If fileInfo.Length > 5 * 1024 * 1024 Then
                    MessageBox.Show("Image file is too large. Please select an image smaller than 5 MB.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                If PictureBox1.Image IsNot Nothing Then
                    PictureBox1.Image.Dispose()
                    PictureBox1.Image = Nothing
                End If

                Try
                    Using tempImg As Image = Image.FromFile(imagePath)
                        PictureBox1.Image = CType(tempImg.Clone(), Image)
                    End Using
                    PictureBox1.Tag = imagePath
                    _currentImagePath = imagePath
                    _hasImagePreview = True
                    PictureBox1.Visible = True
                    PictureBox1.Refresh()
                    UpdateImagePlaceholderVisibility()
                Catch ex As OutOfMemoryException
                    _hasImagePreview = False
                    PictureBox1.Image = Nothing
                    PictureBox1.Visible = False
                    PictureBox1.Tag = Nothing
                    UpdateImagePlaceholderVisibility()
                    MessageBox.Show("The selected file is not a valid image or is corrupted.", "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    _hasImagePreview = False
                    PictureBox1.Image = Nothing
                    PictureBox1.Visible = False
                    PictureBox1.Tag = Nothing
                    UpdateImagePlaceholderVisibility()
                    MessageBox.Show("Error loading image: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        btnExit_Click(sender, e)
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If IsEditMode AndAlso _isStructuralEditLocked Then
            LoadProductForEdit()
            Return
        End If

        txtBarcodeNumber.Clear()
        txtProductName.Clear()
        txtCostPrice.Clear()
        txtSellingPrice.Clear()
        txtDescription.Clear()

        If cbCategory.Items.Count > 0 Then cbCategory.SelectedIndex = 0
        If cbBrand.Items.Count > 0 Then cbBrand.SelectedIndex = 0
        If cbColor.Items.Count > 0 Then cbColor.SelectedIndex = 0
        ResetSizeCombo()

        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        PictureBox1.Tag = Nothing
        _currentImagePath = String.Empty
        _hasImagePreview = False
        PictureBox1.Visible = False
        SetSelectedProductActiveStatus(If(IsEditMode AndAlso _loadedProductRow IsNot Nothing, ReadRowBoolean(_loadedProductRow, "IsActive", True), True))
        UpdateImagePlaceholderVisibility()
    End Sub

    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        BackColor = Color.FromArgb(21, 22, 25)
        ForeColor = Color.White

        pnlMainContainer.BackColor = Color.FromArgb(28, 29, 33)
        pnlMainContainer.Dock = DockStyle.Fill
        pnlMainContainer.Padding = New Padding(24)

        Dim pnlHeader As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(245, 247, 250)
        lblTitle.BackColor = pnlHeader.BackColor
        lblTitle.Location = New Point(0, 0)
        lblTitle.Text = "Add Product"

        _lblSubtitle = New Label With {
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Regular),
            .ForeColor = Color.FromArgb(170, 175, 184),
            .BackColor = pnlHeader.BackColor,
            .Location = New Point(4, 35),
            .Text = "Fill in the details below"
        }

        Dim pnlHeaderDivider As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 1,
            .BackColor = Color.FromArgb(56, 60, 68)
        }

        StyleHeaderCloseButton(btnExit)
        btnExit.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExit.TabIndex = 13
        btnExit.Location = New Point(pnlHeader.Width - 60, 12)

        AddHandler pnlHeader.Resize, Sub()
                                         btnExit.Location = New Point(pnlHeader.ClientSize.Width - 24 - btnExit.Width, 12)
                                     End Sub

        pnlHeader.Controls.Add(btnExit)
        pnlHeader.Controls.Add(_lblSubtitle)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(pnlHeaderDivider)

        Dim pnlBodyHost As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 20, 0, 0)
        }

        Dim tblContentLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 3,
            .RowCount = 1
        }
        tblContentLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 320.0F))
        tblContentLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 24.0F))
        tblContentLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tblContentLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        Dim pnlLeft As Guna2Panel = CreateCardPanel()
        pnlLeft.Dock = DockStyle.Fill
        BuildLeftColumn(pnlLeft)

        Dim pnlRight As Guna2Panel = CreateCardPanel()
        pnlRight.Dock = DockStyle.Fill
        pnlRight.Padding = New Padding(20)
        BuildRightColumn(pnlRight)

        tblContentLayout.Controls.Add(pnlLeft, 0, 0)
        tblContentLayout.Controls.Add(pnlRight, 2, 0)
        pnlBodyHost.Controls.Add(tblContentLayout)

        pnlMainContainer.Controls.Add(pnlBodyHost)
        pnlMainContainer.Controls.Add(pnlHeader)
        Controls.Add(pnlMainContainer)

        _modernUiInitialized = True
        ResumeLayout(True)
    End Sub

    Private Function CreateCardPanel() As Guna2Panel
        Dim pnl As New Guna2Panel()
        pnl.FillColor = Color.FromArgb(34, 36, 42)
        pnl.BorderColor = Color.FromArgb(58, 61, 68)
        pnl.BorderThickness = 1
        pnl.BorderRadius = 14
        pnl.Margin = Padding.Empty
        Return pnl
    End Function

    Private Sub BuildLeftColumn(parentPanel As Guna2Panel)
        Dim lblImageTitle As New Label With {
            .AutoSize = True,
            .BackColor = Color.FromArgb(34, 36, 42),
            .Font = New Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(240, 242, 245),
            .Location = New Point(20, 20),
            .Text = "Product Image"
        }

        Dim imageFrame As New Guna2Panel With {
            .BorderColor = Color.FromArgb(76, 80, 88),
            .BorderThickness = 1,
            .BorderRadius = 12,
            .FillColor = Color.FromArgb(41, 44, 51),
            .Location = New Point(35, 55),
            .Size = New Size(250, 250),
            .Padding = New Padding(4)
        }

        PictureBox1.Parent = imageFrame
        PictureBox1.Dock = DockStyle.Fill
        PictureBox1.BackColor = Color.FromArgb(41, 44, 51)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.Image = Nothing
        PictureBox1.InitialImage = Nothing
        PictureBox1.ErrorImage = Nothing
        PictureBox1.Tag = Nothing
        PictureBox1.Visible = False
        _hasImagePreview = False

        _pnlImagePlaceholder = New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(41, 44, 51)
        }

        Dim placeholderLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 4,
            .BackColor = Color.FromArgb(41, 44, 51)
        }
        placeholderLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0F))
        placeholderLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        placeholderLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        placeholderLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 55.0F))

        Dim lblPlaceholderMain As New Label With {
            .AutoSize = True,
            .Anchor = AnchorStyles.None,
            .Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(193, 197, 205),
            .Text = "Drop image here"
        }
        Dim lblPlaceholderSub As New Label With {
            .AutoSize = True,
            .Anchor = AnchorStyles.None,
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(150, 154, 164),
            .Text = "or click Browse"
        }
        placeholderLayout.Controls.Add(lblPlaceholderMain, 0, 1)
        placeholderLayout.Controls.Add(lblPlaceholderSub, 0, 2)
        _pnlImagePlaceholder.Controls.Add(placeholderLayout)

        imageFrame.Controls.Add(_pnlImagePlaceholder)
        imageFrame.Controls.Add(PictureBox1)
        _pnlImagePlaceholder.BringToFront()

        StyleSecondaryButton(btnBrowse)
        btnBrowse.Text = "Browse Image"
        btnBrowse.Size = New Size(280, 44)
        btnBrowse.Location = New Point(20, 319)
        btnBrowse.TabIndex = 9

        _lnkRemoveImage = New LinkLabel With {
            .AutoSize = True,
            .Location = New Point(20, 370),
            .Text = "Remove Image",
            .LinkColor = Color.FromArgb(204, 208, 216),
            .ActiveLinkColor = Color.FromArgb(255, 146, 146),
            .VisitedLinkColor = Color.FromArgb(204, 208, 216),
            .Visible = False,
            .TabStop = False
        }
        AddHandler _lnkRemoveImage.LinkClicked, AddressOf lnkRemoveImage_LinkClicked

        parentPanel.Controls.Add(_lnkRemoveImage)
        parentPanel.Controls.Add(btnBrowse)
        parentPanel.Controls.Add(imageFrame)
        parentPanel.Controls.Add(lblImageTitle)
    End Sub

    Private Sub BuildRightColumn(parentPanel As Guna2Panel)
        _validationPlaceholders.Clear()
        parentPanel.Padding = New Padding(20)

        StylePrimaryButton(btnAdd)
        btnAdd.Text = "Save"
        btnAdd.Size = New Size(176, 44)
        btnAdd.Margin = New Padding(0)
        btnAdd.TabIndex = 12

        StyleSecondaryButton(btnClear)
        btnClear.Text = "Clear"
        btnClear.Size = New Size(100, 40)
        btnClear.Margin = New Padding(0)
        btnClear.TabIndex = 11

        StyleSecondaryButton(btnCancel)
        btnCancel.Text = "Cancel"
        btnCancel.Size = New Size(100, 40)
        btnCancel.Margin = New Padding(0)
        btnCancel.TabIndex = 10

        _pnlFields = New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(0, 0, 0, 30),
            .BackColor = Color.FromArgb(34, 36, 42)
        }

        _pnlActions = New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 88,
            .BackColor = Color.FromArgb(34, 36, 42),
            .Padding = New Padding(16, 12, 16, 0)
        }

        Dim pnlActionsDivider As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 1,
            .BackColor = Color.FromArgb(52, 56, 64)
        }

        _pnlActions.Controls.Add(btnCancel)
        _pnlActions.Controls.Add(btnClear)
        _pnlActions.Controls.Add(btnAdd)
        _pnlActions.Controls.Add(pnlActionsDivider)

        Dim pnlRightBody As Panel = _pnlFields

        Dim contentWidth As Integer = GetFieldsContentWidth(pnlRightBody)
        Dim y As Integer = 16

        StyleTextBox(txtBarcodeNumber, "Enter barcode number")
        txtBarcodeNumber.MaxLength = 50
        txtBarcodeNumber.TabIndex = 0
        y = AddField(pnlRightBody, "Barcode Number *", txtBarcodeNumber, contentWidth, y, CreateValidationLabel())

        StyleTextBox(txtProductName, "Enter product name")
        txtProductName.MaxLength = 50
        txtProductName.TabIndex = 1
        y = AddField(pnlRightBody, "Product Name *", txtProductName, contentWidth, y, CreateValidationLabel())

        StyleComboBox(cbCategory)
        cbCategory.TabIndex = 2
        y = AddField(pnlRightBody, "Category *", cbCategory, contentWidth, y, CreateValidationLabel())

        StyleComboBox(cbBrand)
        cbBrand.TabIndex = 3
        y = AddField(pnlRightBody, "Brand *", cbBrand, contentWidth, y, CreateValidationLabel())

        StyleComboBox(cbSize)
        cbSize.Enabled = False
        cbSize.TabIndex = 4
        y = AddField(pnlRightBody, "Size *", cbSize, contentWidth, y, CreateValidationLabel())

        StyleComboBox(cbColor)
        cbColor.TabIndex = 5
        y = AddField(pnlRightBody, "Color *", cbColor, contentWidth, y, CreateValidationLabel())

        StyleTextBox(txtCostPrice, "0.00")
        txtCostPrice.MaxLength = 14
        txtCostPrice.TextAlign = HorizontalAlignment.Right
        txtCostPrice.TabIndex = 6
        y = AddField(pnlRightBody, "Cost Price", txtCostPrice, contentWidth, y, CreateValidationLabel())

        StyleTextBox(txtSellingPrice, "0.00")
        txtSellingPrice.MaxLength = 14
        txtSellingPrice.TextAlign = HorizontalAlignment.Right
        txtSellingPrice.TabIndex = 7
        y = AddField(pnlRightBody, "Selling Price *", txtSellingPrice, contentWidth, y, CreateValidationLabel())

        StyleComboBox(cbStatus)
        cbStatus.TabIndex = 8
        y = AddField(pnlRightBody, "Status *", cbStatus, contentWidth, y, CreateValidationLabel())

        StyleTextBox(txtDescription, "Enter product description (optional)", True)
        txtDescription.MaxLength = 200
        txtDescription.TabIndex = 9
        y = AddField(pnlRightBody, "Description (Optional)", txtDescription, contentWidth, y, CreateValidationLabel(), 116)

        Dim lblDescriptionHelper As New Label With {
            .AutoSize = True,
            .ForeColor = Color.FromArgb(155, 160, 170),
            .Location = New Point(0, y - 8),
            .Text = "Max 200 characters"
        }
        pnlRightBody.Controls.Add(lblDescriptionHelper)
        y += 18

        pnlRightBody.AutoScrollMinSize = New Size(0, y + pnlRightBody.Padding.Bottom)

        RemoveHandler pnlRightBody.ClientSizeChanged, AddressOf pnlFields_ClientSizeChanged
        AddHandler pnlRightBody.ClientSizeChanged, AddressOf pnlFields_ClientSizeChanged
        AddHandler _pnlActions.ClientSizeChanged, Sub() LayoutActionButtonsPanel()
        pnlFields_ClientSizeChanged(pnlRightBody, EventArgs.Empty)
        LayoutActionButtonsPanel()

        parentPanel.Controls.Add(_pnlFields)
        parentPanel.Controls.Add(_pnlActions)
    End Sub

    Private Function AddField(container As Panel,
                              labelText As String,
                              inputControl As Control,
                              width As Integer,
                              y As Integer,
                              validationLabel As Label,
                              Optional inputHeight As Integer = 44) As Integer
        Dim labelControl As New Label With {
            .AutoSize = True,
            .Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(240, 242, 245),
            .Location = New Point(0, y),
            .Text = labelText
        }

        inputControl.Location = New Point(0, y + 23)
        inputControl.Size = New Size(width, inputHeight)
        inputControl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        validationLabel.Location = New Point(4, y + 23 + inputHeight + 2)

        container.Controls.Add(validationLabel)
        container.Controls.Add(inputControl)
        container.Controls.Add(labelControl)
        inputControl.Tag = "FieldInput"
        Return y + 82 + Math.Max(0, inputHeight - 44)
    End Function

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then
            Return 720
        End If

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then
            clientWidth = fieldsPanel.Width
        End If

        If clientWidth <= 0 Then
            Return 720
        End If

        ' Reserve a few pixels for vertical scrollbar/focus borders to prevent clipping past the card border.
        Dim scrollbarAllowance As Integer = If(fieldsPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim width As Integer = clientWidth - scrollbarAllowance - 8
        Return Math.Max(240, width)
    End Function

    Private Sub pnlFields_ClientSizeChanged(sender As Object, e As EventArgs)
        Dim fieldsPanel As Panel = TryCast(sender, Panel)
        If fieldsPanel Is Nothing Then
            Return
        End If

        Dim targetWidth As Integer = GetFieldsContentWidth(fieldsPanel)
        Dim maxBottom As Integer = 0
        For Each ctrl As Control In fieldsPanel.Controls
            If TypeOf ctrl Is Guna2TextBox OrElse TypeOf ctrl Is Guna2ComboBox Then
                ctrl.Width = targetWidth
            End If
            maxBottom = Math.Max(maxBottom, ctrl.Bottom)
        Next

        fieldsPanel.AutoScrollMinSize = New Size(0, maxBottom + fieldsPanel.Padding.Bottom)
    End Sub

    Private Sub LayoutActionButtonsPanel()
        If _pnlActions Is Nothing OrElse _pnlActions.IsDisposed Then
            Return
        End If

        Dim innerRight As Integer = _pnlActions.ClientSize.Width - _pnlActions.Padding.Right
        Dim spacing As Integer = 12

        btnAdd.Size = New Size(176, 44)
        btnClear.Size = New Size(100, 40)
        btnCancel.Size = New Size(100, 40)
        btnAdd.Margin = Padding.Empty
        btnClear.Margin = Padding.Empty
        btnCancel.Margin = Padding.Empty

        btnAdd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClear.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnCancel.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        Dim innerTop As Integer = _pnlActions.Padding.Top
        Dim innerBottom As Integer = Math.Max(innerTop, _pnlActions.ClientSize.Height - _pnlActions.Padding.Bottom)
        Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
        Dim primaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnAdd.Height) \ 2)
        Dim secondaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnClear.Height) \ 2)

        btnAdd.Location = New Point(Math.Max(0, innerRight - btnAdd.Width), primaryY)
        btnClear.Location = New Point(Math.Max(0, btnAdd.Left - spacing - btnClear.Width), secondaryY)
        btnCancel.Location = New Point(Math.Max(0, btnClear.Left - spacing - btnCancel.Width), secondaryY)

        btnAdd.BringToFront()
        btnClear.BringToFront()
        btnCancel.BringToFront()
    End Sub

    Private Function CreateValidationLabel() As Label
        Dim lbl As New Label With {
            .AutoSize = True,
            .Font = New Font("Segoe UI", 8.25F, FontStyle.Regular),
            .ForeColor = Color.FromArgb(255, 122, 122),
            .Visible = False
        }
        _validationPlaceholders.Add(lbl)
        Return lbl
    End Function

    Private Sub StyleTextBox(tb As Guna2TextBox, placeholder As String, Optional multiline As Boolean = False)
        tb.Animated = False
        tb.BorderRadius = 12
        tb.BorderThickness = 1
        tb.BorderColor = Color.FromArgb(76, 80, 88)
        tb.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        tb.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
        tb.FillColor = Color.FromArgb(41, 44, 51)
        tb.ForeColor = Color.FromArgb(238, 241, 245)
        tb.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
        tb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        tb.PlaceholderText = placeholder
        tb.Multiline = multiline
        tb.ScrollBars = If(multiline, ScrollBars.Vertical, ScrollBars.None)
        tb.Margin = New Padding(0)
        tb.Cursor = Cursors.IBeam
    End Sub

    Private Sub QueueEditorVisualRefresh()
        If _queuedEditorRefresh OrElse IsDisposed Then
            Return
        End If

        If Not IsHandleCreated Then
            Return
        End If

        _queuedEditorRefresh = True

        BeginInvoke(New MethodInvoker(Sub()
                                          _queuedEditorRefresh = False
                                          RefreshEditorVisuals()
                                      End Sub))
    End Sub

    Private Sub RefreshEditorVisuals()
        If IsDisposed Then
            Return
        End If

        Dim textBoxes As Guna2TextBox() = {
            txtBarcodeNumber,
            txtProductName,
            txtCostPrice,
            txtSellingPrice,
            txtDescription
        }

        For Each tb As Guna2TextBox In textBoxes
            If tb Is Nothing Then Continue For

            tb.Invalidate()
            tb.Update()
            tb.Refresh()

            If tb.TextLength > 0 Then
                tb.SelectionStart = tb.TextLength
                tb.SelectionLength = 0
            End If
        Next

        Dim comboBoxes As Guna2ComboBox() = {cbCategory, cbBrand, cbSize, cbColor, cbStatus}
        For Each cbo As Guna2ComboBox In comboBoxes
            If cbo Is Nothing Then Continue For
            cbo.Invalidate()
            cbo.Update()
            cbo.Refresh()
        Next

        If _pnlFields IsNot Nothing Then
            _pnlFields.Invalidate()
            _pnlFields.Update()
        End If
    End Sub

    Private Sub StyleComboBox(cbo As Guna2ComboBox)
        cbo.BackColor = Color.Transparent
        cbo.BorderRadius = 12
        cbo.BorderThickness = 1
        cbo.BorderColor = Color.FromArgb(76, 80, 88)
        cbo.FocusedColor = Color.FromArgb(0, 122, 204)
        cbo.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        cbo.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
        cbo.FillColor = Color.FromArgb(41, 44, 51)
        cbo.ForeColor = Color.FromArgb(238, 241, 245)
        cbo.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        cbo.DrawMode = DrawMode.OwnerDrawFixed
        cbo.DropDownStyle = ComboBoxStyle.DropDownList
        cbo.IntegralHeight = False
        cbo.ItemHeight = 38
        cbo.Margin = New Padding(0)
        cbo.Cursor = Cursors.Hand
    End Sub

    Private Sub StylePrimaryButton(button As Guna2Button)
        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.BorderRadius = 12
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(0, 122, 204)
        button.FillColor = Color.FromArgb(0, 122, 204)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 10.5F, FontStyle.Bold)
        button.HoverState.FillColor = Color.FromArgb(20, 141, 224)
        button.HoverState.ForeColor = Color.White
        button.HoverState.BorderColor = Color.FromArgb(56, 171, 250)
        button.HoverState.CustomBorderColor = button.CustomBorderColor
        button.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
        button.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        button.Cursor = Cursors.Hand

        button.Image = Nothing
        button.ImageSize = New Size(18, 18)
    End Sub

    Private Sub StyleSecondaryButton(button As Guna2Button)
        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
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
        button.HoverState.CustomBorderColor = button.CustomBorderColor
        button.DisabledState.FillColor = Color.FromArgb(58, 62, 70)
        button.DisabledState.ForeColor = Color.FromArgb(170, 174, 181)
        button.Cursor = Cursors.Hand
    End Sub

    Private Sub StyleHeaderCloseButton(button As Guna2Button)
        StyleSecondaryButton(button)
        button.Size = New Size(36, 36)
        button.BorderRadius = 10
        button.Text = String.Empty
        button.FillColor = Color.FromArgb(62, 66, 75)
        button.BorderColor = Color.FromArgb(74, 79, 88)
        button.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        button.HoverState.BorderColor = Color.FromArgb(106, 112, 124)
        button.HoverState.ForeColor = Color.White

        ' Use the same close icon resource as FrmUserEntry so the icon design matches exactly.
        Dim rm As New System.ComponentModel.ComponentResourceManager(GetType(FrmUserEntry))
        Dim closeImage As Image = TryCast(rm.GetObject("btnExit.Image"), Image)
        button.Image = closeImage
        button.ImageSize = New Size(12, 12)

        If closeImage Is Nothing Then
            button.Text = "X"
            button.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        End If
    End Sub

    Private Sub UpdateImagePlaceholderVisibility()
        Dim hasImage As Boolean = _hasImagePreview AndAlso PictureBox1 IsNot Nothing AndAlso PictureBox1.Image IsNot Nothing

        If _pnlImagePlaceholder IsNot Nothing Then
            _pnlImagePlaceholder.Visible = Not hasImage
        End If

        If PictureBox1 IsNot Nothing Then
            PictureBox1.Visible = hasImage
        End If

        If _lnkRemoveImage IsNot Nothing Then
            _lnkRemoveImage.Visible = hasImage
            _lnkRemoveImage.TabStop = hasImage
        End If
    End Sub

    Private Sub lnkRemoveImage_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        PictureBox1.Tag = Nothing
        _currentImagePath = String.Empty
        _hasImagePreview = False
        PictureBox1.Visible = False
        UpdateImagePlaceholderVisibility()
    End Sub

End Class
