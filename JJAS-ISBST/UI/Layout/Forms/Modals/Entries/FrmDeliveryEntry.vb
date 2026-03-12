Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Public Class FrmDeliveryEntry
    Private ReadOnly _service As New DeliveriesService()
    Private Shared ReadOnly DeliverySurfaceColor As Color = Color.FromArgb(34, 36, 42)

    Private Const ColGridEdit As String = "colGridEdit"
    Private Const ColGridDelete As String = "colGridDelete"
    Private Const ColTempRowId As String = "TempRowID"

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return Mode = EntryFormMode.EditExisting AndAlso SelectedId > 0
        End Get
    End Property

    Private dtPending As DataTable
    Private _isLoading As Boolean = False
    Private _nextTempRowId As Integer = 1
    Private _modernUiInitialized As Boolean = False
    Private _btnCancelAction As Guna2Button
    Private _headerSubtitle As Label
    Private _mainCardPanelV2 As Panel
    Private _topFieldsPanelV2 As Panel
    Private _addItemPanelV2 As Panel
    Private _gridHostPanelV2 As Panel
    Private _bottomButtonsPanelV2 As Panel
    Private _fieldsTableV2 As TableLayoutPanel
    Private _contentTableV2 As TableLayoutPanel
    Private _btnCloseModernV2 As Guna2Button
    Private _isSuppressingSupplierSelectionChange As Boolean = False
    Private _lockedSupplierId As Integer = 0

    Public Sub New()
        InitializeComponent()

        Dim fixedSize As New Size(1200, 760)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        InitializeModernUiIfNeeded()
    End Sub

    Private Sub EnsureDtPending()
        If dtPending Is Nothing Then
            dtPending = New DataTable()
            dtPending.Columns.Add(ColTempRowId, GetType(Integer))
            dtPending.Columns.Add("DeliveryProductID", GetType(Integer))
            dtPending.Columns.Add("ProductID", GetType(Integer))
            dtPending.Columns.Add("BarcodeNumber", GetType(String))
            dtPending.Columns.Add("Product", GetType(String))
            dtPending.Columns.Add("Quantity", GetType(Integer))
            dtPending.Columns.Add("ReturnedQty", GetType(Integer))
            dtPending.Columns.Add("Status", GetType(String))
            dtPending.Columns.Add("ImagePath", GetType(String))
            dtPending.Columns.Add("ProductImage", GetType(Image))
        End If
    End Sub

    Private Function NormalizeDraftItemStatus(value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then Return "Pending"

        Dim statusText As String = value.ToString().Trim()
        If String.IsNullOrWhiteSpace(statusText) Then
            Return "Pending"
        End If

        Return statusText
    End Function

    Private Function IsPostedDraftStatus(statusText As String) As Boolean
        Return String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function IsPostedDraftRow(row As DataRow) As Boolean
        If row Is Nothing Then Return False
        If row.Table Is Nothing OrElse Not row.Table.Columns.Contains("Status") Then Return False
        Return IsPostedDraftStatus(NormalizeDraftItemStatus(row("Status")))
    End Function

    Private Function HasSelectedSupplier() As Boolean
        If cbCompany Is Nothing OrElse cbCompany.IsDisposed Then Return False
        If cbCompany.SelectedIndex <= 0 OrElse cbCompany.SelectedValue Is Nothing Then Return False

        Try
            Return Convert.ToInt32(cbCompany.SelectedValue) > 0
        Catch
            Return False
        End Try
    End Function

    Private Function HasPendingDraftItems() As Boolean
        Return dtPending IsNot Nothing AndAlso dtPending.Rows.Count > 0
    End Function

    Private Function IsSupplierSelectionLocked() As Boolean
        Return HasPendingDraftItems()
    End Function

    Private Sub ShowSupplierRequiredMessage()
        MessageBox.Show("Please select a supplier first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ShowSupplierLockedMessage()
        MessageBox.Show("Remove all items first before changing supplier.", "Supplier Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub RestoreLockedSupplierSelection()
        If _lockedSupplierId <= 0 Then Return
        If cbCompany Is Nothing OrElse cbCompany.IsDisposed Then Return

        _isSuppressingSupplierSelectionChange = True
        Try
            cbCompany.SelectedValue = _lockedSupplierId
        Catch
        Finally
            _isSuppressingSupplierSelectionChange = False
        End Try
    End Sub

    Private Sub ApplyAddButtonAvailabilityState(isSupplierSelected As Boolean)
        If btnadd Is Nothing OrElse btnadd.IsDisposed Then Return

        btnadd.Enabled = True ' Keep clickable so we can show validation messaging.
        btnadd.Cursor = If(isSupplierSelected, Cursors.Hand, Cursors.No)
        btnadd.BorderColor = If(isSupplierSelected, Color.FromArgb(0, 122, 204), Color.FromArgb(90, 95, 106))
        btnadd.FillColor = If(isSupplierSelected, Color.FromArgb(0, 122, 204), Color.FromArgb(72, 76, 84))
        btnadd.ForeColor = If(isSupplierSelected, Color.White, Color.FromArgb(190, 194, 201))
        btnadd.HoverState.BorderColor = If(isSupplierSelected, Color.FromArgb(56, 171, 250), Color.FromArgb(90, 95, 106))
        btnadd.HoverState.FillColor = If(isSupplierSelected, Color.FromArgb(20, 141, 224), Color.FromArgb(72, 76, 84))
        btnadd.HoverState.ForeColor = If(isSupplierSelected, Color.White, Color.FromArgb(190, 194, 201))
    End Sub

    Private Sub ApplySupplierLockVisualState(isLocked As Boolean)
        If cbCompany Is Nothing OrElse cbCompany.IsDisposed Then Return

        cbCompany.Cursor = If(isLocked, Cursors.No, Cursors.Hand)
        cbCompany.TabStop = Not isLocked
        cbCompany.BorderColor = If(isLocked, Color.FromArgb(90, 95, 106), Color.FromArgb(76, 80, 88))
        cbCompany.FillColor = If(isLocked, Color.FromArgb(54, 57, 64), Color.FromArgb(41, 44, 51))
        cbCompany.ForeColor = If(isLocked, Color.FromArgb(210, 214, 220), Color.FromArgb(238, 241, 245))
        cbCompany.HoverState.BorderColor = If(isLocked, Color.FromArgb(90, 95, 106), Color.FromArgb(108, 114, 124))
    End Sub

    Private Sub UpdateDraftInteractionState()
        Dim supplierSelected As Boolean = HasSelectedSupplier()
        Dim shouldLockSupplier As Boolean = IsSupplierSelectionLocked()

        If shouldLockSupplier Then
            If _lockedSupplierId <= 0 AndAlso supplierSelected Then
                Try
                    _lockedSupplierId = Convert.ToInt32(cbCompany.SelectedValue)
                Catch
                    _lockedSupplierId = 0
                End Try
            End If
        Else
            _lockedSupplierId = 0
        End If

        ApplyAddButtonAvailabilityState(supplierSelected)
        ApplySupplierLockVisualState(shouldLockSupplier)
    End Sub

    Private Function GetNextTempRowId() As Integer
        Dim currentId As Integer = _nextTempRowId
        _nextTempRowId += 1
        Return currentId
    End Function

    Private Function LoadImageFromPath(imagePath As String) As Image
        If String.IsNullOrWhiteSpace(imagePath) OrElse Not IO.File.Exists(imagePath) Then
            Return Nothing
        End If

        Using tempImg As Image = Image.FromFile(imagePath)
            Return New Bitmap(tempImg)
        End Using
    End Function

    Private Sub EnsureActionColumns()
        If Not DGVtable.Columns.Contains(ColGridEdit) Then
            Dim editCol As New DataGridViewButtonColumn() With {
                .Name = ColGridEdit,
                .HeaderText = "Edit",
                .Text = "Edit",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
            }
            DGVtable.Columns.Add(editCol)
        End If

        If Not DGVtable.Columns.Contains(ColGridDelete) Then
            Dim deleteCol As New DataGridViewButtonColumn() With {
                .Name = ColGridDelete,
                .HeaderText = "Delete",
                .Text = "Delete",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
            }
            DGVtable.Columns.Add(deleteCol)
        End If
    End Sub

    Private Function FindPendingRowByTempId(tempRowId As Integer) As DataRow
        EnsureDtPending()
        Dim foundRows() As DataRow = dtPending.Select(ColTempRowId & " = " & tempRowId.ToString())
        If foundRows.Length = 0 Then Return Nothing
        Return foundRows(0)
    End Function

    Private Function FindEditablePendingRowByProductId(productId As Integer) As DataRow
        If productId <= 0 Then Return Nothing
        EnsureDtPending()

        For Each row As DataRow In dtPending.Rows
            If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For
            If Convert.ToInt32(row("ProductID")) <> productId Then Continue For
            If IsPostedDraftRow(row) Then Continue For
            Return row
        Next

        Return Nothing
    End Function

    Private Sub EditPendingRowByTempId(tempRowId As Integer)
        Dim rowToEdit As DataRow = FindPendingRowByTempId(tempRowId)
        If rowToEdit Is Nothing Then
            MessageBox.Show("Unable to find product in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If IsPostedDraftRow(rowToEdit) Then
            MessageBox.Show("Posted items are read-only. Only pending items can be edited.", "Edit Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim productId As Integer = Convert.ToInt32(rowToEdit("ProductID"))
        Dim productName As String = Convert.ToString(rowToEdit("Product")).Trim()
        Dim currentQty As Integer = Math.Max(1, Convert.ToInt32(rowToEdit("Quantity")))
        Dim updatedQty As Integer? = PromptForPendingItemQuantity(productName, currentQty, "Edit Quantity")
        If Not updatedQty.HasValue Then Return

        rowToEdit("Quantity") = updatedQty.Value

        If MergeDuplicatePendingRowsForProduct(tempRowId, productId) Then
            MessageBox.Show("Duplicate product rows were merged and quantity increased.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        displayData()
        DGVtable.ClearSelection()
    End Sub

    Private Function PromptForPendingItemQuantity(productName As String, defaultQuantity As Integer, dialogTitle As String) As Integer?
        Dim seed As Integer = Math.Max(1, defaultQuantity)

        Do
            Dim input As String = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter quantity for this item:" & Environment.NewLine & productName,
                dialogTitle,
                seed.ToString())

            If String.IsNullOrWhiteSpace(input) Then Return Nothing

            Dim qty As Integer
            If Integer.TryParse(input.Trim(), qty) AndAlso qty > 0 Then
                Return qty
            End If

            MessageBox.Show("Quantity must be numeric and greater than 0.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Loop
    End Function

    Private Function MergeDuplicatePendingRowsForProduct(currentTempRowId As Integer, productId As Integer) As Boolean
        If productId <= 0 Then Return False

        Dim currentRow As DataRow = FindPendingRowByTempId(currentTempRowId)
        If currentRow Is Nothing Then Return False
        If IsPostedDraftRow(currentRow) Then Return False

        Dim duplicateRows As New List(Of DataRow)()
        For Each candidate As DataRow In dtPending.Rows
            If candidate Is Nothing OrElse candidate.RowState = DataRowState.Deleted Then Continue For
            If Convert.ToInt32(candidate("ProductID")) <> productId Then Continue For
            If Convert.ToInt32(candidate(ColTempRowId)) = currentTempRowId Then Continue For
            If IsPostedDraftRow(candidate) Then Continue For
            duplicateRows.Add(candidate)
        Next

        If duplicateRows.Count = 0 Then
            Return False
        End If

        Dim mergedQty As Integer = Math.Max(1, Convert.ToInt32(currentRow("Quantity")))
        For Each duplicateRow As DataRow In duplicateRows
            mergedQty += Math.Max(0, Convert.ToInt32(duplicateRow("Quantity")))
        Next

        currentRow("Quantity") = mergedQty

        For Each duplicateRow As DataRow In duplicateRows
            dtPending.Rows.Remove(duplicateRow)
        Next

        Return True
    End Function

    Private Sub DeletePendingRowByTempId(tempRowId As Integer)
        Dim rowToDelete As DataRow = FindPendingRowByTempId(tempRowId)
        If rowToDelete Is Nothing Then
            MessageBox.Show("Unable to find product in the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If IsPostedDraftRow(rowToDelete) Then
            MessageBox.Show("Posted items are read-only. Only pending items can be removed.", "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Are you sure you want to remove this product from the list?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        dtPending.Rows.Remove(rowToDelete)
        displayData()
        DGVtable.ClearSelection()
    End Sub

    Private Sub displayData()
        EnsureDtPending()

        For Each row As DataRow In dtPending.Rows
            Dim path As String = If(row("ImagePath") IsNot Nothing, row("ImagePath").ToString(), String.Empty)
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    row("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                row("ProductImage") = Nothing
            End If
        Next

        DGVtable.DataSource = dtPending
        EnsureActionColumns()

        GridHelpers.ApplyColumnSetup(DGVtable, ColTempRowId, Sub(col) col.Visible = False)
        GridHelpers.ApplyColumnSetup(DGVtable, "DeliveryProductID", Sub(col) col.Visible = False)
        GridHelpers.ApplyColumnSetup(DGVtable, "ProductID", Sub(col) col.Visible = False)
        GridHelpers.ApplyColumnSetup(DGVtable, "ImagePath", Sub(col) col.Visible = False)

        Dim productImageColumn As DataGridViewColumn = Nothing
        If GridHelpers.TryGetColumn(DGVtable, productImageColumn, "ProductImage") Then
            productImageColumn.DisplayIndex = 0
            Dim imageColumn As DataGridViewImageColumn = TryCast(productImageColumn, DataGridViewImageColumn)
            If imageColumn IsNot Nothing Then
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
                imageColumn.Width = 80
            End If
        End If

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGVtable.ScrollBars = ScrollBars.Vertical
        DGVtable.RowHeadersVisible = False
        DGVtable.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        DGVtable.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        DGVtable.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGVtable.MultiSelect = False
        For Each col As DataGridViewColumn In DGVtable.Columns
            If col.Name = "ProductImage" OrElse col.Name = ColGridEdit OrElse col.Name = ColGridDelete Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        GridHelpers.ApplyColumnSetup(DGVtable, "BarcodeNumber", Sub(col)
                                                                    col.HeaderText = "Barcode"
                                                                    col.FillWeight = 24
                                                                    col.MinimumWidth = 150
                                                                End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "Product", Sub(col)
                                                              col.HeaderText = "Product"
                                                              col.FillWeight = 44
                                                              col.MinimumWidth = 220
                                                          End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "Quantity", Sub(col)
                                                               col.HeaderText = "Qty"
                                                               col.FillWeight = 12
                                                               col.MinimumWidth = 70
                                                               col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                           End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "ReturnedQty", Sub(col)
                                                                  col.HeaderText = "Returned"
                                                                  col.FillWeight = 12
                                                                  col.MinimumWidth = 80
                                                                  col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                              End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, "Status", Sub(col)
                                                             col.HeaderText = "Status"
                                                             col.FillWeight = 14
                                                             col.MinimumWidth = 100
                                                             col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                         End Sub)
        ApplyStandardGridLayout(DGVtable)
        DGVtable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        DGVtable.AllowUserToResizeRows = False
        DGVtable.RowTemplate.Height = 40
        DGVtable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DGVtable.ColumnHeadersHeight = 40
        GridHelpers.ApplyColumnSetup(DGVtable, ColGridEdit, Sub(col)
                                                                col.Width = 84
                                                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColGridDelete, Sub(col)
                                                                  col.Width = 84
                                                                  col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                              End Sub)
        ApplyDeliveryGridThemeV2()
        DGVtable.ClearSelection()
        UpdateDraftInteractionState()
    End Sub

    Public Sub generateorderNumber()
        Dim rnd As New Random()
        Dim orderNumber As String = DateTime.Now.ToString("yyyyMMddHHmmss") & rnd.Next(10, 99).ToString()
        txtOrderNumber.Text = orderNumber
    End Sub

    Private Sub FrmDeliveryEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isLoading = True
        Try
            StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
            ModalEntryAnimator.PrepareForOpenAnimation(Me)

            txtDeliveryNumber.ReadOnly = False
            txtDeliveryNumber.PlaceholderText = "DLV-00001"

            LoadSuppliers()

            dtpShipDate.MaxDate = DateTime.Today

            dtPending = Nothing
            _nextTempRowId = 1
            EnsureDtPending()

            If IsEditMode Then
                Me.Text = "Deliveries Catalog"
                LoadDeliveryDetails(SelectedId)
            Else
                Me.Text = "Deliveries Catalog"
                txtDeliveryNumber.Clear()
                generateorderNumber()
            End If

            displayData()

            Dim tooltip As New ToolTip()
            tooltip.SetToolTip(cbCompany, "Select a supplier (company).")
            tooltip.SetToolTip(txtDeliveryNumber, "Enter the supplier delivery number (example: DLV-00021).")
            tooltip.SetToolTip(dtpShipDate, "Select the delivery date.")
            tooltip.SetToolTip(btnadd, "Select a supplier first, then add a product to the delivery.")
            tooltip.SetToolTip(btnSave, "Save the delivery.")

        Finally
            _isLoading = False
        End Try
    End Sub

    Private Sub FrmDeliveryEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub LoadSuppliers()
        Dim dt As DataTable = _service.GetSupplierLookup()

        Dim row As DataRow = dt.NewRow()
        row("SupplierID") = 0
        row("Company") = "-- Select Supplier --"
        dt.Rows.InsertAt(row, 0)

        cbCompany.DataSource = dt
        cbCompany.DisplayMember = "Company"
        cbCompany.ValueMember = "SupplierID"
        cbCompany.SelectedIndex = 0
    End Sub

    Public Sub LoadDeliveryDetails(deliveryId As Integer)
        EnsureDtPending()
        dtPending.Clear()
        _nextTempRowId = 1

        Dim header As DataRow = _service.GetDeliveryHeaderById(deliveryId)
        If header Is Nothing Then
            MessageBox.Show("Selected delivery record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If header.Table.Columns.Contains("DeliveryNumber") AndAlso Not IsDBNull(header("DeliveryNumber")) Then
            txtDeliveryNumber.Text = header("DeliveryNumber").ToString().Trim()
        Else
            txtDeliveryNumber.Clear()
        End If

        If Not IsDBNull(header("OrderNumber")) Then txtOrderNumber.Text = header("OrderNumber").ToString()

        If Not IsDBNull(header("DeliveryDate")) Then
            Dim deliveryDate As DateTime = Convert.ToDateTime(header("DeliveryDate"))
            If deliveryDate < dtpShipDate.MinDate Then
                dtpShipDate.Value = dtpShipDate.MinDate
            ElseIf deliveryDate > dtpShipDate.MaxDate Then
                dtpShipDate.Value = dtpShipDate.MaxDate
            Else
                dtpShipDate.Value = deliveryDate
            End If
        End If

        If Not IsDBNull(header("SupplierID")) Then
            Dim supplierId As Integer = Convert.ToInt32(header("SupplierID"))
            Try
                If supplierId > 0 Then
                    _isLoading = True
                    cbCompany.SelectedValue = supplierId
                End If
            Catch
            Finally
                _isLoading = False
            End Try
        End If

        Dim products As DataTable = _service.GetDeliveryProductsByDeliveryId(deliveryId)
        dtPending.Clear()
        For Each r As DataRow In products.Rows
            Dim nr As DataRow = dtPending.NewRow()
            nr(ColTempRowId) = GetNextTempRowId()
            nr("DeliveryProductID") = r("DeliveryProductID")
            nr("ProductID") = r("ProductID")
            nr("BarcodeNumber") = r("BarcodeNumber")
            nr("Product") = r("Product")
            nr("Quantity") = r("Quantity")
            nr("ReturnedQty") = If(r.Table.Columns.Contains("ReturnedQty"), Convert.ToInt32(r("ReturnedQty")), 0)
            nr("Status") = NormalizeDraftItemStatus(If(r.Table.Columns.Contains("Status"), r("Status"), Nothing))
            nr("ImagePath") = r("ImagePath")
            nr("ProductImage") = Nothing
            dtPending.Rows.Add(nr)
        Next

        For Each r As DataRow In dtPending.Rows
            Dim path As String = If(r("ImagePath") IsNot Nothing, r("ImagePath").ToString(), String.Empty)
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    r("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                r("ProductImage") = Nothing
            End If
        Next

        displayData()
    End Sub

    Private Sub cbCompany_DropDown(sender As Object, e As EventArgs) Handles cbCompany.DropDown
        If _isLoading OrElse _isSuppressingSupplierSelectionChange Then Return

        If IsSupplierSelectionLocked() Then
            RestoreLockedSupplierSelection()
            ShowSupplierLockedMessage()
            cbCompany.DroppedDown = False
            Return
        End If

        If cbCompany.Items.Count <= 1 Then
            _isLoading = True
            Try
                LoadSuppliers()
            Finally
                _isLoading = False
            End Try
        End If
    End Sub

    Private Sub cbCompany_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbCompany.SelectionChangeCommitted
        If _isLoading OrElse _isSuppressingSupplierSelectionChange Then Return

        If IsSupplierSelectionLocked() Then
            RestoreLockedSupplierSelection()
            ShowSupplierLockedMessage()
            Return
        End If

        UpdateDraftInteractionState()
    End Sub

    Private Sub cbCompany_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbCompany.SelectedValueChanged
        If _isLoading OrElse _isSuppressingSupplierSelectionChange Then Return

        If IsSupplierSelectionLocked() AndAlso HasSelectedSupplier() Then
            Dim currentSupplierId As Integer = 0
            Try
                currentSupplierId = Convert.ToInt32(cbCompany.SelectedValue)
            Catch
                currentSupplierId = 0
            End Try

            If _lockedSupplierId > 0 AndAlso currentSupplierId <> _lockedSupplierId Then
                RestoreLockedSupplierSelection()
                Return
            End If
        End If

        UpdateDraftInteractionState()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        EnsureDtPending()

        If Not HasSelectedSupplier() Then
            ShowSupplierRequiredMessage()
            Return
        End If

        Dim f As New FrmDeliveryProductEntry
        If f.ShowDialog(Me) = DialogResult.OK Then
            AddProductSelectionToPendingGrid(f)
        End If
    End Sub

    Private Sub AddProductSelectionToPendingGrid(selection As FrmDeliveryProductEntry)
        If selection Is Nothing Then Return
        EnsureDtPending()

        Dim wasMerged As Boolean = AddOrMergePendingProduct(
            selection.selectedID,
            selection.SelectedBarcode,
            selection.SelectedProductName,
            selection.SelectedQuantity,
            selection.SelectedImagePath)

        MessageBox.Show(
            If(wasMerged, "Product already exists; quantity updated.", "Product added to delivery list!"),
            If(wasMerged, "Updated", "Success"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        displayData()
        DGVtable.ClearSelection()
    End Sub

    Private Function AddOrMergePendingProduct(productId As Integer,
                                              barcode As String,
                                              productName As String,
                                              quantity As Integer,
                                              imagePath As String) As Boolean
        If productId <= 0 Then
            Throw New InvalidOperationException("Invalid product selection.")
        End If

        EnsureDtPending()

        Dim existingEditableRow As DataRow = FindEditablePendingRowByProductId(productId)
        If existingEditableRow IsNot Nothing Then
            existingEditableRow("Quantity") = Convert.ToInt32(existingEditableRow("Quantity")) + Math.Max(1, quantity)
            existingEditableRow("BarcodeNumber") = If(barcode, String.Empty)
            existingEditableRow("Product") = If(productName, String.Empty)
            existingEditableRow("Status") = "Pending"
            existingEditableRow("ImagePath") = If(imagePath, String.Empty)
            existingEditableRow("ProductImage") = LoadImageFromPath(imagePath)
            Return True
        End If

        dtPending.Rows.Add(GetNextTempRowId(),
                           DBNull.Value,
                           productId,
                           If(barcode, String.Empty),
                           If(productName, String.Empty),
                           Math.Max(1, quantity),
                           0,
                           "Pending",
                           If(imagePath, String.Empty),
                           LoadImageFromPath(imagePath))

        Return False
    End Function

    Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        If e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
        If colName <> ColGridEdit AndAlso colName <> ColGridDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim tempIdColumn As DataGridViewColumn = Nothing
        If Not GridHelpers.TryGetColumn(DGVtable, tempIdColumn, ColTempRowId) OrElse row.Cells(tempIdColumn.Name).Value Is Nothing Then
            MessageBox.Show("Unable to identify selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim tempRowId As Integer = Convert.ToInt32(row.Cells(tempIdColumn.Name).Value)
        If colName = ColGridEdit Then
            EditPendingRowByTempId(tempRowId)
        ElseIf colName = ColGridDelete Then
            DeletePendingRowByTempId(tempRowId)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim deliveryNumber As String = If(txtDeliveryNumber.Text, String.Empty).Trim()
        txtDeliveryNumber.Text = deliveryNumber

        If String.IsNullOrWhiteSpace(deliveryNumber) Then
            MessageBox.Show("Delivery Number cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDeliveryNumber.Focus()
            Return
        End If

        If _service.DeliveryNumberExists(deliveryNumber, If(IsEditMode, CType(SelectedId, Integer?), Nothing)) Then
            MessageBox.Show("Delivery Number already exists.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDeliveryNumber.Focus()
            Return
        End If

        If cbCompany.SelectedIndex <= 0 OrElse cbCompany.SelectedValue Is Nothing OrElse Convert.ToInt32(cbCompany.SelectedValue) = 0 Then
            MessageBox.Show("Please select a supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        EnsureDtPending()
        If dtPending.Rows.Count = 0 Then
            MessageBox.Show("Please add at least one product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim actionText As String = If(IsEditMode, "update", "save")
        If MessageBox.Show($"Are you sure you want to {actionText} this delivery?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            _service.SaveDelivery(Mode, SelectedId, Convert.ToInt32(cbCompany.SelectedValue), deliveryNumber, txtOrderNumber.Text.Trim(), dtpShipDate.Value.Date, dtPending)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Deliveries.", "Added Deliveries."))
            MessageBox.Show(If(IsEditMode, "Delivery updated successfully!", "Delivery saved successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            If String.Equals(ex.Message, "Delivery Number already exists.", StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(ex.Message, "Delivery Number cannot be empty.", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDeliveryNumber.Focus()
                Return
            End If

            MessageBox.Show("Error saving delivery: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        _modernUiInitialized = True
        AutoScaleMode = AutoScaleMode.None
        AutoSize = False

        BuildDeliveryModalLayoutV2()
        ApplyDeliveryGridThemeV2()
        Return

        Dim bodyPanel As New Panel With {
            .Name = "pnlDeliveryBodyHost",
            .BackColor = DeliverySurfaceColor
        }

        Dim pnlTopSection As New Panel With {
            .Name = "pnlDeliveryTopSection",
            .Dock = DockStyle.Top,
            .Height = 132,
            .Padding = New Padding(0, 0, 0, 10),
            .BackColor = DeliverySurfaceColor
        }

        Dim fieldsTable As New TableLayoutPanel With {
            .Name = "tblDeliveryFields",
            .Dock = DockStyle.Top,
            .Height = 78,
            .ColumnCount = 3,
            .RowCount = 2,
            .BackColor = DeliverySurfaceColor,
            .Margin = Padding.Empty
        }
        fieldsTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        fieldsTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 24.0F))
        fieldsTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 50.0F))
        fieldsTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        fieldsTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        fieldsTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))

        For Each ctl As Control In New Control() {s, Label2, Label3}
            StyleDeliveryFieldLabel(ctl)
        Next

        ConfigureDeliveryFieldControl(cbCompany)
        ConfigureDeliveryFieldControl(txtOrderNumber)
        ConfigureDeliveryFieldControl(dtpShipDate)

        s.Dock = DockStyle.Fill
        Label2.Dock = DockStyle.Fill
        Label3.Dock = DockStyle.Fill
        cbCompany.Dock = DockStyle.Fill
        txtOrderNumber.Dock = DockStyle.Fill
        dtpShipDate.Dock = DockStyle.Fill

        s.Margin = New Padding(2, 0, 12, 4)
        Label2.Margin = New Padding(2, 0, 12, 4)
        Label3.Margin = New Padding(2, 0, 2, 4)
        cbCompany.Margin = New Padding(0, 0, 12, 0)
        txtOrderNumber.Margin = New Padding(0, 0, 12, 0)
        dtpShipDate.Margin = New Padding(0)

        fieldsTable.Controls.Add(s, 0, 0)
        fieldsTable.Controls.Add(Label2, 1, 0)
        fieldsTable.Controls.Add(Label3, 2, 0)
        fieldsTable.Controls.Add(cbCompany, 0, 1)
        fieldsTable.Controls.Add(txtOrderNumber, 1, 1)
        fieldsTable.Controls.Add(dtpShipDate, 2, 1)

        Dim pnlTopActions As New Panel With {
            .Name = "pnlDeliveryTopActions",
            .Dock = DockStyle.Top,
            .Height = 44,
            .BackColor = DeliverySurfaceColor
        }

        btnadd.Text = " Add Item"
        btnadd.DefaultAutoSize = False
        btnadd.AutoRoundedCorners = False
        btnadd.BorderRadius = 12
        btnadd.Size = New Size(132, 42)
        btnadd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnadd.Margin = Padding.Empty

        pnlTopActions.Controls.Add(btnadd)
        Dim positionAddButton As Action =
            Sub()
                btnadd.Location = New Point(Math.Max(0, pnlTopActions.ClientSize.Width - btnadd.Width), 0)
            End Sub
        AddHandler pnlTopActions.Resize, Sub() positionAddButton()
        positionAddButton()

        pnlTopSection.Controls.Add(pnlTopActions)
        pnlTopSection.Controls.Add(fieldsTable)

        DGVtable.Dock = DockStyle.Fill
        DGVtable.Margin = Padding.Empty
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGVtable.RowHeadersVisible = False
        DGVtable.ScrollBars = ScrollBars.Vertical
        DGVtable.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom

        bodyPanel.Controls.Add(DGVtable)
        bodyPanel.Controls.Add(pnlTopSection)
        bodyPanel.Controls.Add(btnSave)

        EntryFormUiStyler.ApplyStandardSingleColumnEntryLayout(Me, Label12, btnExit, bodyPanel, btnSave)
        FinalizeModernDeliveryLayout()
    End Sub

    Private Sub StyleDeliveryFieldLabel(lbl As Control)
        If lbl Is Nothing Then Return
        lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(238, 241, 245)
        lbl.BackColor = DeliverySurfaceColor
        lbl.AutoSize = False
        lbl.Height = 24

        If TypeOf lbl Is Label Then
            DirectCast(lbl, Label).TextAlign = ContentAlignment.BottomLeft
        End If
    End Sub

    Private Sub ConfigureDeliveryFieldControl(ctrl As Control)
        If ctrl Is Nothing Then Return

        ctrl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        If TypeOf ctrl Is Guna.UI2.WinForms.Guna2TextBox Then
            Dim txt = DirectCast(ctrl, Guna.UI2.WinForms.Guna2TextBox)
            txt.Animated = True
            txt.AutoRoundedCorners = False
            txt.BorderRadius = 12
            txt.Height = 44
        ElseIf TypeOf ctrl Is Guna.UI2.WinForms.Guna2ComboBox Then
            Dim cbo = DirectCast(ctrl, Guna.UI2.WinForms.Guna2ComboBox)
            cbo.AutoRoundedCorners = False
            cbo.BorderRadius = 12
            cbo.ItemHeight = 38
            cbo.Height = 44
        ElseIf TypeOf ctrl Is Guna.UI2.WinForms.Guna2DateTimePicker Then
            Dim dtp = DirectCast(ctrl, Guna.UI2.WinForms.Guna2DateTimePicker)
            dtp.AutoRoundedCorners = False
            dtp.BorderRadius = 12
            dtp.Height = 44
            dtp.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub FinalizeModernDeliveryLayout()
        Dim fieldsHost As Panel = TryCast(FindControlRecursive(Me, "pnlFieldsHost"), Panel)
        If fieldsHost IsNot Nothing Then
            fieldsHost.SuspendLayout()
            Try
                fieldsHost.AutoScroll = False
                fieldsHost.AutoScrollMinSize = Size.Empty
                fieldsHost.Padding = Padding.Empty
                fieldsHost.BackColor = DeliverySurfaceColor

                Dim topSection As Panel = TryCast(FindControlRecursive(fieldsHost, "pnlDeliveryTopSection"), Panel)
                If topSection IsNot Nothing Then
                    topSection.Dock = DockStyle.Top
                    topSection.Height = 86
                    topSection.Padding = Padding.Empty
                    topSection.Margin = Padding.Empty
                    topSection.BackColor = DeliverySurfaceColor
                End If

                Dim topActions As Panel = TryCast(FindControlRecursive(fieldsHost, "pnlDeliveryTopActions"), Panel)
                If topActions IsNot Nothing Then
                    topActions.Dock = DockStyle.Top
                    topActions.Height = 50
                    topActions.Margin = Padding.Empty
                    topActions.BackColor = DeliverySurfaceColor
                End If

                Dim fieldTable As TableLayoutPanel = TryCast(FindControlRecursive(fieldsHost, "tblDeliveryFields"), TableLayoutPanel)
                If fieldTable IsNot Nothing Then
                    fieldTable.Dock = DockStyle.Top
                    fieldTable.Height = 78
                    fieldTable.Margin = Padding.Empty
                    fieldTable.BackColor = DeliverySurfaceColor
                End If

                If btnadd IsNot Nothing AndAlso Not btnadd.IsDisposed Then
                    btnadd.Visible = True
                    btnadd.BringToFront()
                    btnadd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
                    btnadd.Size = New Size(132, 42)
                End If

                Dim itemsSection As Panel = TryCast(FindControlRecursive(fieldsHost, "pnlDeliveryItemsSection"), Panel)
                If itemsSection Is Nothing Then
                    itemsSection = New Panel With {
                        .Name = "pnlDeliveryItemsSection",
                        .Dock = DockStyle.Fill,
                        .Margin = Padding.Empty,
                        .Padding = Padding.Empty,
                        .BackColor = DeliverySurfaceColor
                    }
                Else
                    itemsSection.Dock = DockStyle.Fill
                    itemsSection.Margin = Padding.Empty
                    itemsSection.Padding = Padding.Empty
                    itemsSection.BackColor = DeliverySurfaceColor
                End If
                itemsSection.AutoScroll = False
                itemsSection.Visible = True

                Dim gridHost As Panel = TryCast(FindControlRecursive(fieldsHost, "pnlDeliveryGridHost"), Panel)
                If gridHost Is Nothing Then
                    gridHost = New Panel With {
                        .Name = "pnlDeliveryGridHost",
                        .Dock = DockStyle.Fill,
                        .Margin = Padding.Empty,
                        .Padding = Padding.Empty,
                        .BackColor = DeliverySurfaceColor
                    }
                Else
                    gridHost.Dock = DockStyle.Fill
                    gridHost.Margin = Padding.Empty
                    gridHost.Padding = Padding.Empty
                    gridHost.BackColor = DeliverySurfaceColor
                End If
                gridHost.AutoScroll = False
                gridHost.Visible = True

                If DGVtable IsNot Nothing AndAlso Not DGVtable.IsDisposed Then
                    If DGVtable.Parent IsNot Nothing Then
                        DGVtable.Parent.Controls.Remove(DGVtable)
                    End If

                    DGVtable.Dock = DockStyle.Fill
                    DGVtable.Anchor = AnchorStyles.None
                    DGVtable.Margin = Padding.Empty
                    DGVtable.ScrollBars = ScrollBars.Vertical
                    DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    DGVtable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                    DGVtable.AllowUserToResizeRows = False
                    DGVtable.RowTemplate.Height = 40
                    DGVtable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    DGVtable.ColumnHeadersHeight = 40
                    DGVtable.Visible = True

                    gridHost.Controls.Clear()
                    gridHost.Controls.Add(DGVtable)
                    DGVtable.BringToFront()
                End If

                If topSection IsNot Nothing AndAlso topSection.Parent IsNot Nothing Then
                    topSection.Parent.Controls.Remove(topSection)
                End If
                If topActions IsNot Nothing AndAlso topActions.Parent IsNot Nothing Then
                    topActions.Parent.Controls.Remove(topActions)
                End If

                If fieldTable IsNot Nothing AndAlso fieldTable.Parent IsNot topSection AndAlso topSection IsNot Nothing Then
                    If fieldTable.Parent IsNot Nothing Then
                        fieldTable.Parent.Controls.Remove(fieldTable)
                    End If
                    topSection.Controls.Add(fieldTable)
                End If

                If topSection IsNot Nothing Then
                    topSection.Controls.Clear()
                    If fieldTable IsNot Nothing Then
                        topSection.Controls.Add(fieldTable)
                    End If
                End If

                If topActions IsNot Nothing Then
                    topActions.Controls.Clear()
                    topActions.Dock = DockStyle.Top
                    topActions.Height = 50
                    topActions.Padding = Padding.Empty
                    topActions.Margin = Padding.Empty
                    topActions.BackColor = DeliverySurfaceColor

                    If btnadd IsNot Nothing AndAlso Not btnadd.IsDisposed Then
                        If btnadd.Parent IsNot Nothing Then
                            btnadd.Parent.Controls.Remove(btnadd)
                        End If

                        topActions.Controls.Add(btnadd)
                        Dim positionAddButton As Action =
                            Sub()
                                btnadd.Location = New Point(Math.Max(0, topActions.ClientSize.Width - btnadd.Width), 4)
                            End Sub
                        AddHandler topActions.Resize, Sub() positionAddButton()
                        positionAddButton()
                    End If
                End If

                itemsSection.Controls.Clear()
                If topActions IsNot Nothing Then
                    itemsSection.Controls.Add(topActions)
                End If
                itemsSection.Controls.Add(gridHost)
                If topActions IsNot Nothing Then topActions.BringToFront()

                Dim contentTable As New TableLayoutPanel With {
                    .Name = "tblDeliveryContentRows",
                    .Dock = DockStyle.Fill,
                    .Margin = Padding.Empty,
                    .Padding = Padding.Empty,
                    .ColumnCount = 1,
                    .RowCount = 2,
                    .BackColor = DeliverySurfaceColor
                }
                contentTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
                contentTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 86.0F))
                contentTable.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

                If topSection IsNot Nothing Then
                    topSection.Dock = DockStyle.Fill
                    contentTable.Controls.Add(topSection, 0, 0)
                End If

                itemsSection.Dock = DockStyle.Fill
                contentTable.Controls.Add(itemsSection, 0, 1)

                fieldsHost.Controls.Clear()
                fieldsHost.Controls.Add(contentTable)
            Finally
                fieldsHost.ResumeLayout(True)
            End Try
        End If

        Dim cardPanel As Panel = TryCast(FindControlRecursive(Me, "pnlEntryCard"), Panel)
        If cardPanel IsNot Nothing Then
            cardPanel.AutoScroll = False
        End If

        Dim bodyPanel As Panel = TryCast(FindControlRecursive(Me, "pnlBodyHost"), Panel)
        If bodyPanel IsNot Nothing Then
            bodyPanel.AutoScroll = False
        End If

        Dim actionsHost As Panel = TryCast(FindControlRecursive(Me, "pnlActionsHost"), Panel)
        If actionsHost IsNot Nothing Then
            actionsHost.AutoScroll = False
            actionsHost.Height = 88
            actionsHost.Padding = New Padding(16, 12, 16, 8)
            actionsHost.BringToFront()
        End If

        If btnSave IsNot Nothing AndAlso Not btnSave.IsDisposed Then
            btnSave.Visible = True
            btnSave.Enabled = True
        End If
    End Sub

    Private Function FindControlRecursive(root As Control, controlName As String) As Control
        If root Is Nothing OrElse String.IsNullOrWhiteSpace(controlName) Then
            Return Nothing
        End If

        For Each child As Control In root.Controls
            If String.Equals(child.Name, controlName, StringComparison.OrdinalIgnoreCase) Then
                Return child
            End If

            Dim nested As Control = FindControlRecursive(child, controlName)
            If nested IsNot Nothing Then
                Return nested
            End If
        Next

        Return Nothing
    End Function

    Private Sub BuildDeliveryModalLayoutV2()
        SuspendLayout()
        Try
            Padding = Padding.Empty
            BackColor = Color.FromArgb(28, 29, 33)

            EnsureHeaderControlsV2()
            EnsureActionButtonsV2()
            ConfigureFieldLabelsV2()
            ConfigureInputControlsV2()
            ConfigureGridBaseV2()

            _mainCardPanelV2 = New Panel With {
                .Name = "pnlDeliveryCardV2",
                .Dock = DockStyle.Fill,
                .Padding = New Padding(20),
                .BackColor = Color.Transparent
            }
            _mainCardPanelV2.AutoScroll = False
            AddHandler _mainCardPanelV2.Paint, AddressOf MainCardPanelV2_Paint
            AddHandler _mainCardPanelV2.Resize, AddressOf MainCardPanelV2_Resize

            Dim headerPanel As New Panel With {
                .Name = "pnlHeaderV2",
                .Dock = DockStyle.Top,
                .Height = 60,
                .BackColor = Color.Transparent,
                .Padding = Padding.Empty
            }

            Dim headerTextPanel As New Panel With {
                .Name = "pnlHeaderTextV2",
                .Dock = DockStyle.Fill,
                .BackColor = Color.Transparent
            }

            Dim headerCloseHost As New Panel With {
                .Name = "pnlHeaderCloseHostV2",
                .Dock = DockStyle.Right,
                .Width = 60,
                .BackColor = Color.Transparent,
                .Padding = Padding.Empty
            }

            Label12.AutoSize = False
            Label12.Text = "Deliveries Catalog"
            Label12.Font = New Font("Segoe UI Semibold", 15.0F, FontStyle.Bold)
            Label12.ForeColor = Color.White
            Label12.BackColor = Color.Transparent
            Label12.Dock = DockStyle.Top
            Label12.Height = 30
            Label12.TextAlign = ContentAlignment.MiddleLeft
            Label12.Margin = Padding.Empty

            _headerSubtitle.AutoSize = False
            _headerSubtitle.Text = "Fill in the details below"
            _headerSubtitle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
            _headerSubtitle.ForeColor = Color.FromArgb(160, 170, 190)
            _headerSubtitle.BackColor = Color.Transparent
            _headerSubtitle.Dock = DockStyle.Top
            _headerSubtitle.Height = 20
            _headerSubtitle.TextAlign = ContentAlignment.MiddleLeft
            _headerSubtitle.Margin = Padding.Empty

            _btnCloseModernV2.Dock = DockStyle.None
            _btnCloseModernV2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            _btnCloseModernV2.Margin = Padding.Empty

            headerTextPanel.Controls.Add(_headerSubtitle)
            headerTextPanel.Controls.Add(Label12)
            headerCloseHost.Controls.Add(_btnCloseModernV2)
            AddHandler headerCloseHost.Resize, Sub() PositionHeaderCloseButtonV2(headerCloseHost)
            PositionHeaderCloseButtonV2(headerCloseHost)
            headerPanel.Controls.Add(headerTextPanel)
            headerPanel.Controls.Add(headerCloseHost)

            _contentTableV2 = New TableLayoutPanel With {
                .Name = "tblDeliveryLayoutV2",
                .Dock = DockStyle.Fill,
                .Margin = Padding.Empty,
                .Padding = Padding.Empty,
                .ColumnCount = 1,
                .RowCount = 3,
                .BackColor = Color.Transparent
            }
            _contentTableV2.ColumnStyles.Clear()
            _contentTableV2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
            _contentTableV2.RowStyles.Clear()
            _contentTableV2.RowStyles.Add(New RowStyle(SizeType.Absolute, 90.0F))
            _contentTableV2.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            _contentTableV2.RowStyles.Add(New RowStyle(SizeType.Absolute, 70.0F))

            BuildTopFieldsPanelV2()
            BuildItemsSectionV2()
            BuildBottomButtonsPanelV2()

            _contentTableV2.Controls.Add(_topFieldsPanelV2, 0, 0)

            Dim itemsRowPanel As New Panel With {
                .Name = "pnlItemsRowV2",
                .Dock = DockStyle.Fill,
                .Padding = Padding.Empty,
                .Margin = Padding.Empty,
                .BackColor = Color.Transparent
            }
            itemsRowPanel.AutoScroll = False
            itemsRowPanel.Controls.Add(_gridHostPanelV2)
            itemsRowPanel.Controls.Add(_addItemPanelV2)

            _contentTableV2.Controls.Add(itemsRowPanel, 0, 1)
            _contentTableV2.Controls.Add(_bottomButtonsPanelV2, 0, 2)

            _mainCardPanelV2.Controls.Add(_contentTableV2)
            _mainCardPanelV2.Controls.Add(headerPanel)

            Controls.Add(_mainCardPanelV2)
            _mainCardPanelV2.BringToFront()
        Finally
            ResumeLayout(True)
            PerformLayout()
        End Try
    End Sub

    Private Sub EnsureHeaderControlsV2()
        If _headerSubtitle Is Nothing OrElse _headerSubtitle.IsDisposed Then
            _headerSubtitle = New Label()
        End If

        If _btnCloseModernV2 Is Nothing OrElse _btnCloseModernV2.IsDisposed Then
            _btnCloseModernV2 = New Guna2Button With {
                .Name = "btnCloseModernV2",
                .Text = String.Empty,
                .Size = New Size(36, 36),
                .Animated = True,
                .DefaultAutoSize = False,
                .AutoRoundedCorners = False,
                .BorderRadius = 10,
                .BorderThickness = 1,
                .BorderColor = Color.FromArgb(76, 80, 88),
                .FillColor = Color.FromArgb(62, 66, 75),
                .ForeColor = Color.White,
                .Cursor = Cursors.Hand
            }
            _btnCloseModernV2.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
            _btnCloseModernV2.HoverState.FillColor = Color.FromArgb(78, 83, 94)
            _btnCloseModernV2.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
            _btnCloseModernV2.Image = btnExit.Image
            _btnCloseModernV2.ImageSize = New Size(12, 12)
            AddHandler _btnCloseModernV2.Click,
                Sub()
                    Me.DialogResult = DialogResult.Cancel
                    Me.Close()
                End Sub
        End If
    End Sub

    Private Sub EnsureActionButtonsV2()
        If _btnCancelAction Is Nothing OrElse _btnCancelAction.IsDisposed Then
            _btnCancelAction = New Guna2Button With {
                .Name = "btnCancelAction",
                .Text = "Cancel",
                .Animated = True,
                .DefaultAutoSize = False,
                .Size = New Size(96, 42)
            }
            AddHandler _btnCancelAction.Click,
                Sub()
                    Me.DialogResult = DialogResult.Cancel
                    Me.Close()
                End Sub
        End If

        StyleSecondaryButtonV2(_btnCancelAction)
        StylePrimaryButtonV2(btnSave, " Save")
        StylePrimaryButtonV2(btnadd, " Add Item")
        btnSave.Size = New Size(132, 42)
        btnadd.Size = New Size(132, 42)
    End Sub

    Private Sub BuildTopFieldsPanelV2()
        _topFieldsPanelV2 = New Panel With {
            .Name = "pnlTopFieldsV2",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(2, 4, 2, 0),
            .Margin = Padding.Empty,
            .BackColor = Color.Transparent
        }
        _topFieldsPanelV2.AutoScroll = False

        _fieldsTableV2 = New TableLayoutPanel With {
            .Name = "tblFieldsV2",
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 2,
            .Margin = Padding.Empty,
            .Padding = Padding.Empty,
            .BackColor = Color.Transparent
        }
        _fieldsTableV2.ColumnStyles.Clear()
        _fieldsTableV2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 28.0F))
        _fieldsTableV2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 24.0F))
        _fieldsTableV2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 24.0F))
        _fieldsTableV2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 24.0F))
        _fieldsTableV2.RowStyles.Clear()
        _fieldsTableV2.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        _fieldsTableV2.RowStyles.Add(New RowStyle(SizeType.Absolute, 44.0F))

        s.Text = "Company:"
        lblDeliveryNumber.Text = "Delivery Number:"
        Label2.Text = "Order Number:"
        Label3.Text = "Delivery Date:"

        AddFieldLabelToTableV2(s, 0)
        AddFieldLabelToTableV2(lblDeliveryNumber, 1)
        AddFieldLabelToTableV2(Label2, 2)
        AddFieldLabelToTableV2(Label3, 3)
        AddInputToTableV2(cbCompany, 0)
        AddInputToTableV2(txtDeliveryNumber, 1)
        AddInputToTableV2(txtOrderNumber, 2)
        AddInputToTableV2(dtpShipDate, 3)

        _topFieldsPanelV2.Controls.Add(_fieldsTableV2)
    End Sub

    Private Sub BuildItemsSectionV2()
        _addItemPanelV2 = New Panel With {
            .Name = "pnlAddItemV2",
            .Dock = DockStyle.Top,
            .Height = 58,
            .Padding = New Padding(0, 4, 0, 12),
            .Margin = Padding.Empty,
            .BackColor = Color.Transparent
        }
        _addItemPanelV2.AutoScroll = False
        btnadd.Dock = DockStyle.Right
        btnadd.Margin = Padding.Empty
        _addItemPanelV2.Controls.Add(btnadd)

        _gridHostPanelV2 = New Panel With {
            .Name = "pnlGridHostV2",
            .Dock = DockStyle.Fill,
            .Padding = Padding.Empty,
            .Margin = Padding.Empty,
            .BackColor = Color.Transparent
        }
        _gridHostPanelV2.AutoScroll = False

        DGVtable.Dock = DockStyle.Fill
        DGVtable.Margin = Padding.Empty
        DGVtable.Visible = True
        _gridHostPanelV2.Controls.Add(DGVtable)
        DGVtable.BringToFront()
    End Sub

    Private Sub BuildBottomButtonsPanelV2()
        _bottomButtonsPanelV2 = New Panel With {
            .Name = "pnlBottomButtonsV2",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 12, 0, 8),
            .Margin = Padding.Empty,
            .BackColor = Color.Transparent
        }
        _bottomButtonsPanelV2.AutoScroll = False
        btnSave.Dock = DockStyle.None
        btnSave.Margin = New Padding(10, 0, 0, 0)
        _btnCancelAction.Margin = Padding.Empty

        Dim actionsFlow As New FlowLayoutPanel With {
            .Name = "flpBottomActionsV2",
            .Dock = DockStyle.Right,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .FlowDirection = FlowDirection.RightToLeft,
            .WrapContents = False,
            .BackColor = Color.Transparent,
            .Padding = Padding.Empty,
            .Margin = Padding.Empty
        }

        actionsFlow.Controls.Add(btnSave)
        actionsFlow.Controls.Add(_btnCancelAction)
        _bottomButtonsPanelV2.Controls.Add(actionsFlow)
    End Sub

    Private Sub ConfigureFieldLabelsV2()
        For Each lbl As Label In New Label() {s, lblDeliveryNumber, Label2, Label3}
            If lbl Is Nothing Then Continue For
            lbl.AutoSize = False
            lbl.Dock = DockStyle.Fill
            lbl.Margin = Padding.Empty
            lbl.BackColor = Color.Transparent
            lbl.ForeColor = Color.FromArgb(180, 190, 210)
            lbl.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
            lbl.TextAlign = ContentAlignment.MiddleLeft
        Next
    End Sub

    Private Sub ConfigureInputControlsV2()
        StyleInputControlV2(cbCompany, True)
        StyleInputControlV2(txtDeliveryNumber, True)
        StyleInputControlV2(txtOrderNumber, True)
        StyleInputControlV2(dtpShipDate, False)

        If txtDeliveryNumber IsNot Nothing Then
            txtDeliveryNumber.PlaceholderText = "DLV-00001"
            txtDeliveryNumber.ReadOnly = False
        End If
    End Sub

    Private Sub StyleInputControlV2(ctrl As Control, hasRightSpacing As Boolean)
        If ctrl Is Nothing Then Return
        ctrl.AutoSize = False
        ctrl.Dock = DockStyle.Fill
        ctrl.Margin = If(hasRightSpacing, New Padding(0, 0, 10, 0), New Padding(0))
        ctrl.Padding = Padding.Empty

        If TypeOf ctrl Is Guna2ComboBox Then
            Dim cbo = DirectCast(ctrl, Guna2ComboBox)
            cbo.AutoRoundedCorners = False
            cbo.BorderRadius = 12
            cbo.BorderThickness = 1
            cbo.BorderColor = Color.FromArgb(76, 80, 88)
            cbo.FillColor = Color.FromArgb(41, 44, 51)
            cbo.FocusedColor = Color.FromArgb(0, 122, 204)
            cbo.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
            cbo.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
            cbo.ForeColor = Color.FromArgb(238, 241, 245)
            cbo.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
            cbo.BackColor = Color.Transparent
            cbo.DrawMode = DrawMode.OwnerDrawFixed
            cbo.DropDownStyle = ComboBoxStyle.DropDownList
            cbo.IntegralHeight = False
            cbo.ItemHeight = 38
            cbo.MinimumSize = New Size(120, 44)
            cbo.Height = 44
        ElseIf TypeOf ctrl Is Guna2TextBox Then
            Dim txt = DirectCast(ctrl, Guna2TextBox)
            txt.Animated = False
            txt.AutoRoundedCorners = False
            txt.BorderRadius = 12
            txt.BorderThickness = 1
            txt.BorderColor = Color.FromArgb(76, 80, 88)
            txt.FillColor = Color.FromArgb(41, 44, 51)
            txt.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
            txt.FocusedState.Parent = txt
            txt.FocusedState.FillColor = Color.FromArgb(41, 44, 51)
            txt.FocusedState.ForeColor = Color.FromArgb(238, 241, 245)
            txt.FocusedState.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
            txt.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
            txt.HoverState.Parent = txt
            txt.HoverState.FillColor = Color.FromArgb(41, 44, 51)
            txt.HoverState.ForeColor = Color.FromArgb(238, 241, 245)
            txt.HoverState.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
            txt.ForeColor = Color.FromArgb(238, 241, 245)
            txt.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
            txt.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
            txt.TextOffset = Point.Empty
            txt.MinimumSize = New Size(120, 44)
            txt.MaximumSize = New Size(0, 44)
            txt.Height = 44
        ElseIf TypeOf ctrl Is Guna2DateTimePicker Then
            Dim dtp = DirectCast(ctrl, Guna2DateTimePicker)
            dtp.Animated = False
            dtp.AutoRoundedCorners = False
            dtp.BorderRadius = 12
            dtp.BorderThickness = 1
            dtp.BorderColor = Color.FromArgb(76, 80, 88)
            dtp.FillColor = Color.FromArgb(41, 44, 51)
            dtp.ForeColor = Color.FromArgb(238, 241, 245)
            dtp.FocusedColor = Color.FromArgb(0, 122, 204)
            dtp.CheckedState.Parent = dtp
            dtp.CheckedState.BorderColor = Color.FromArgb(76, 80, 88)
            dtp.CheckedState.FillColor = Color.FromArgb(41, 44, 51)
            dtp.CheckedState.ForeColor = Color.FromArgb(238, 241, 245)
            dtp.HoverState.Parent = dtp
            dtp.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
            dtp.HoverState.FillColor = Color.FromArgb(41, 44, 51)
            dtp.HoverState.ForeColor = Color.FromArgb(238, 241, 245)
            dtp.UseTransparentBackground = False
            dtp.IndicateFocus = False
            dtp.BackColor = Color.FromArgb(34, 36, 42)
            dtp.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
            dtp.Format = DateTimePickerFormat.Long
            dtp.TextAlign = HorizontalAlignment.Left
            dtp.TextOffset = Point.Empty
            dtp.MinimumSize = New Size(120, 44)
            dtp.MaximumSize = New Size(0, 44)
            dtp.Height = 44
        End If
    End Sub

    Private Sub ConfigureGridBaseV2()
        DGVtable.Dock = DockStyle.Fill
        DGVtable.Visible = True
        DGVtable.ReadOnly = True
        DGVtable.AllowUserToAddRows = False
        DGVtable.AllowUserToDeleteRows = False
        DGVtable.AllowUserToResizeRows = False
        DGVtable.RowHeadersVisible = False
        DGVtable.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGVtable.MultiSelect = False
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGVtable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        DGVtable.RowTemplate.Height = 40
        DGVtable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DGVtable.ColumnHeadersHeight = 40
        DGVtable.ScrollBars = ScrollBars.Vertical
        DGVtable.Margin = Padding.Empty
    End Sub

    Private Sub ApplyDeliveryGridThemeV2()
        If DGVtable Is Nothing OrElse DGVtable.IsDisposed Then Return

        Dim whiteColor As Color = Color.White
        Dim altRowColor As Color = Color.Gainsboro
        Dim headerColor As Color = Color.LightGray
        Dim gridLineColor As Color = Color.Silver
        Dim selectionColor As Color = Color.FromArgb(219, 232, 250)

        DGVtable.BackgroundColor = whiteColor
        DGVtable.BorderStyle = BorderStyle.None
        DGVtable.GridColor = gridLineColor
        DGVtable.ForeColor = Color.Black
        DGVtable.EnableHeadersVisualStyles = False
        DGVtable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        DGVtable.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        DGVtable.ColumnHeadersDefaultCellStyle.BackColor = headerColor
        DGVtable.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        DGVtable.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerColor
        DGVtable.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black

        DGVtable.DefaultCellStyle.BackColor = whiteColor
        DGVtable.DefaultCellStyle.ForeColor = Color.Black
        DGVtable.DefaultCellStyle.SelectionBackColor = selectionColor
        DGVtable.DefaultCellStyle.SelectionForeColor = Color.Black
        DGVtable.DefaultCellStyle.WrapMode = DataGridViewTriState.False

        DGVtable.RowsDefaultCellStyle.BackColor = whiteColor
        DGVtable.RowsDefaultCellStyle.ForeColor = Color.Black
        DGVtable.RowsDefaultCellStyle.SelectionBackColor = selectionColor
        DGVtable.RowsDefaultCellStyle.SelectionForeColor = Color.Black

        DGVtable.AlternatingRowsDefaultCellStyle.BackColor = altRowColor
        DGVtable.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black
        DGVtable.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectionColor
        DGVtable.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black

        DGVtable.ThemeStyle.BackColor = whiteColor
        DGVtable.ThemeStyle.GridColor = gridLineColor
        DGVtable.ThemeStyle.HeaderStyle.BackColor = headerColor
        DGVtable.ThemeStyle.HeaderStyle.ForeColor = Color.Black
        DGVtable.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        DGVtable.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DGVtable.ThemeStyle.HeaderStyle.Height = 40
        DGVtable.ThemeStyle.RowsStyle.BackColor = whiteColor
        DGVtable.ThemeStyle.RowsStyle.ForeColor = Color.Black
        DGVtable.ThemeStyle.RowsStyle.SelectionBackColor = selectionColor
        DGVtable.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black
        DGVtable.ThemeStyle.RowsStyle.Height = 40
        DGVtable.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DGVtable.ThemeStyle.ReadOnly = True
        DGVtable.ThemeStyle.AlternatingRowsStyle.BackColor = altRowColor
        DGVtable.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Black
        DGVtable.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = selectionColor
        DGVtable.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black

        For Each col As DataGridViewColumn In DGVtable.Columns
            If TypeOf col Is DataGridViewButtonColumn Then
                col.DefaultCellStyle.BackColor = whiteColor
                col.DefaultCellStyle.ForeColor = Color.Black
                col.DefaultCellStyle.SelectionBackColor = selectionColor
                col.DefaultCellStyle.SelectionForeColor = Color.Black
            End If
        Next
    End Sub

    Private Sub AddFieldLabelToTableV2(lbl As Label, columnIndex As Integer)
        If lbl Is Nothing Then Return
        lbl.Dock = DockStyle.Fill
        Dim leftPad As Integer = If(columnIndex = 0, 0, 12)
        Dim rightPad As Integer = If(columnIndex = _fieldsTableV2.ColumnCount - 1, 0, 12)
        lbl.Margin = New Padding(leftPad, 0, rightPad, 0)
        _fieldsTableV2.Controls.Add(lbl, columnIndex, 0)
    End Sub

    Private Sub AddInputToTableV2(ctrl As Control, columnIndex As Integer)
        If ctrl Is Nothing Then Return
        ctrl.Dock = DockStyle.Fill
        Dim leftPad As Integer = If(columnIndex = 0, 0, 12)
        Dim rightPad As Integer = If(columnIndex = _fieldsTableV2.ColumnCount - 1, 0, 12)
        ctrl.Margin = New Padding(leftPad, 0, rightPad, 0)
        _fieldsTableV2.Controls.Add(ctrl, columnIndex, 1)
    End Sub

    Private Sub LayoutBottomButtonsV2()
        If _bottomButtonsPanelV2 Is Nothing OrElse _bottomButtonsPanelV2.IsDisposed Then Return
        If btnSave Is Nothing OrElse btnSave.IsDisposed Then Return
        If _btnCancelAction Is Nothing OrElse _btnCancelAction.IsDisposed Then Return

        Dim spacing As Integer = 10
        Dim topY As Integer = 10
        Dim rightEdge As Integer = _bottomButtonsPanelV2.ClientSize.Width

        btnSave.Location = New Point(Math.Max(0, rightEdge - btnSave.Width), topY)
        _btnCancelAction.Location = New Point(Math.Max(0, btnSave.Left - spacing - _btnCancelAction.Width), topY)
        btnSave.BringToFront()
        _btnCancelAction.BringToFront()
    End Sub

    Private Sub PositionTopRightButtonV2(host As Panel, button As Control, offsetRight As Integer, offsetTop As Integer)
        If host Is Nothing OrElse button Is Nothing Then Return
        button.Location = New Point(Math.Max(0, host.ClientSize.Width - button.Width - offsetRight), Math.Max(0, offsetTop))
        button.BringToFront()
    End Sub

    Private Sub PositionHeaderCloseButtonV2(headerPanel As Panel)
        If headerPanel Is Nothing OrElse _btnCloseModernV2 Is Nothing Then Return
        _btnCloseModernV2.Location = New Point(Math.Max(0, headerPanel.ClientSize.Width - _btnCloseModernV2.Width - 24), 12)
        _btnCloseModernV2.BringToFront()
    End Sub

    Private Sub StylePrimaryButtonV2(button As Guna2Button, textValue As String)
        If button Is Nothing Then Return
        button.Text = textValue
        button.Animated = True
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 12
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(0, 122, 204)
        button.FillColor = Color.FromArgb(0, 122, 204)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.White
        button.HoverState.FillColor = Color.FromArgb(20, 141, 224)
        button.HoverState.BorderColor = Color.FromArgb(56, 171, 250)
        button.HoverState.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 10.5F, FontStyle.Bold)
        button.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
        button.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        button.Cursor = Cursors.Hand
        button.UseTransparentBackground = False
        button.Margin = Padding.Empty
    End Sub

    Private Sub StyleSecondaryButtonV2(button As Guna2Button)
        If button Is Nothing Then Return
        button.Animated = True
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 10
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(74, 79, 88)
        button.FillColor = Color.FromArgb(62, 66, 75)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.FromArgb(242, 244, 247)
        button.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        button.HoverState.ForeColor = Color.White
        button.HoverState.BorderColor = Color.FromArgb(106, 112, 124)
        button.DisabledState.FillColor = Color.FromArgb(58, 62, 70)
        button.DisabledState.ForeColor = Color.FromArgb(170, 174, 181)
        button.Cursor = Cursors.Hand
        button.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        button.UseTransparentBackground = False
        button.Margin = Padding.Empty
    End Sub

    Private Sub MainCardPanelV2_Paint(sender As Object, e As PaintEventArgs)
        Dim pnl As Panel = TryCast(sender, Panel)
        If pnl Is Nothing Then Return

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        Dim rect As New Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1)
        If rect.Width <= 0 OrElse rect.Height <= 0 Then Return

        Using path As GraphicsPath = CreateRoundedRectanglePathV2(rect, 14)
            Using brush As New SolidBrush(Color.FromArgb(34, 36, 42))
                e.Graphics.FillPath(brush, path)
            End Using
            Using pen As New Pen(Color.FromArgb(58, 61, 68), 1)
                e.Graphics.DrawPath(pen, path)
            End Using
        End Using
    End Sub

    Private Sub MainCardPanelV2_Resize(sender As Object, e As EventArgs)
        Dim pnl As Panel = TryCast(sender, Panel)
        If pnl Is Nothing OrElse pnl.Width <= 0 OrElse pnl.Height <= 0 Then Return

        Using path As GraphicsPath = CreateRoundedRectanglePathV2(New Rectangle(0, 0, pnl.Width, pnl.Height), 14)
            Dim oldRegion As Region = pnl.Region
            pnl.Region = New Region(path)
            If oldRegion IsNot Nothing Then oldRegion.Dispose()
        End Using
        pnl.Invalidate()
    End Sub

    Private Function CreateRoundedRectanglePathV2(bounds As Rectangle, radius As Integer) As GraphicsPath
        Dim r As Integer = Math.Max(1, radius)
        Dim d As Integer = r * 2
        Dim path As New GraphicsPath()

        Dim arc As New Rectangle(bounds.X, bounds.Y, d, d)
        path.AddArc(arc, 180, 90)
        arc.X = bounds.Right - d
        path.AddArc(arc, 270, 90)
        arc.Y = bounds.Bottom - d
        path.AddArc(arc, 0, 90)
        arc.X = bounds.X
        path.AddArc(arc, 90, 90)
        path.CloseFigure()
        Return path
    End Function
End Class
