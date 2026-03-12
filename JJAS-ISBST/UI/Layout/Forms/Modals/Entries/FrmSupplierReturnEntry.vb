Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms

Public Class FrmSupplierReturnEntry
    Inherits Form

    Private NotInheritable Class PendingDeliveryOption
        Public Property DeliveryID As Integer
        Public Property SupplierName As String
        Public Property DeliveryNumber As String
        Public Property OrderNumber As String
        Public Property DeliveryDate As DateTime?
        Public Property DisplayText As String

        Public Overrides Function ToString() As String
            Return If(DisplayText, String.Empty)
        End Function
    End Class

    Private Const ColDeliveryProductID As String = "DeliveryProductID"
    Private Const ColProductID As String = "ProductID"
    Private Const ColProduct As String = "Product"
    Private Const ColBarcode As String = "BarcodeNumber"
    Private Const ColDeliveredQty As String = "DeliveredQty"
    Private Const ColReturnedQty As String = "ReturnedQty"
    Private Const ColMaxReturnable As String = "MaxReturnable"
    Private Const ColStatus As String = "Status"
    Private Const ColReturnQty As String = "ReturnQty"
    Private Const FieldLabelHeight As Integer = 24
    Private Const FieldControlHeight As Integer = 44
    Private Const FieldRightSpacing As Integer = 12
    Private Const HeaderCloseButtonSize As Integer = 40

    Private ReadOnly _service As New DeliveriesService()

    Private _context As DataRow
    Private _itemsTable As DataTable
    Private _deliveryOptions As List(Of PendingDeliveryOption)
    Private _uiInitialized As Boolean
    Private _isLoadingDeliverySelection As Boolean

    Private _btnClose As Guna2Button
    Private _btnCancel As Guna2Button
    Private _btnSave As Guna2Button
    Private _cboDelivery As Guna2ComboBox
    Private _txtSupplier As Guna2TextBox
    Private _txtDeliveryNumber As Guna2TextBox
    Private _txtOrderNumber As Guna2TextBox
    Private _txtDeliveryDate As Guna2TextBox
    Private _cboReturnType As Guna2ComboBox
    Private _cboResolution As Guna2ComboBox
    Private _dtpReturnDate As Guna2DateTimePicker
    Private _txtNotes As Guna2TextBox
    Private _grid As Guna2DataGridView
    Private WithEvents Guna2BorderlessForm1 As Guna2BorderlessForm

    Public Property DeliveryId As Integer = -1
    Public Property ReturnNumber As String = String.Empty
    Public Property AppliedItems As IList(Of SupplierReturnAppliedItem)

    Public Sub New()
        InitializeComponent()

        AppliedItems = New List(Of SupplierReturnAppliedItem)()

        AutoScaleMode = AutoScaleMode.None
        FormBorderStyle = FormBorderStyle.None
        BackColor = Color.FromArgb(25, 27, 33)
        ForeColor = Color.FromArgb(238, 241, 245)
        Text = "Return To Supplier"

        Dim fixedSize As New Size(1120, 820)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        If components Is Nothing Then
            components = New System.ComponentModel.Container()
        End If

        Guna2BorderlessForm1 = New Guna2BorderlessForm(components) With {
            .ContainerControl = Me,
            .BorderRadius = 16,
            .DockIndicatorTransparencyValue = 0.6R,
            .TransparentWhileDrag = True,
            .AnimateWindow = True
        }

        BuildUiIfNeeded()
    End Sub

    Private Sub FrmSupplierReturnEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.UseGunaOpenAnimation(Me)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        LoadPendingDeliveries()
    End Sub

    Private Sub FrmSupplierReturnEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
        If _cboDelivery IsNot Nothing AndAlso Not _cboDelivery.IsDisposed Then
            _cboDelivery.Focus()
        End If
    End Sub

    Private Sub BuildUiIfNeeded()
        If _uiInitialized Then Return

        SuspendLayout()
        Try
            Dim rootPanel As New Panel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(24),
                .BackColor = BackColor
            }

            Dim card As New Guna2Panel With {
                .Dock = DockStyle.Fill,
                .FillColor = Color.FromArgb(32, 35, 42),
                .BorderColor = Color.FromArgb(58, 62, 72),
                .BorderThickness = 1,
                .BorderRadius = 16,
                .Padding = New Padding(20, 18, 20, 18)
            }

            Dim headerPanel As New Panel With {
                .Dock = DockStyle.Top,
                .Height = 56,
                .BackColor = Color.Transparent
            }

            Dim lblTitle As New Label With {
                .Dock = DockStyle.Left,
                .Width = 360,
                .Text = "Return To Supplier",
                .AutoSize = False,
                .TextAlign = ContentAlignment.MiddleLeft,
                .ForeColor = Color.White,
                .Font = New Font("Segoe UI Semibold", 17.0F, FontStyle.Bold),
                .BackColor = Color.Transparent
            }

            _btnClose = New Guna2Button With {
                .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
                .Size = New Size(HeaderCloseButtonSize, HeaderCloseButtonSize),
                .Text = "X"
            }
            StyleHeaderCloseButton(_btnClose)
            _btnClose.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
            AddHandler _btnClose.Click, Sub() CloseWithCancel()
            AddHandler headerPanel.Resize, Sub() PositionHeaderCloseButton(headerPanel, _btnClose)
            PositionHeaderCloseButton(headerPanel, _btnClose)

            headerPanel.Controls.Add(_btnClose)
            headerPanel.Controls.Add(lblTitle)

            Dim bodyLayout As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .ColumnCount = 1,
                .RowCount = 5,
                .BackColor = Color.Transparent,
                .Margin = Padding.Empty,
                .Padding = New Padding(0, 8, 0, 0)
            }
            bodyLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 78.0F))
            bodyLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 86.0F))
            bodyLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 170.0F))
            bodyLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            bodyLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 62.0F))

            bodyLayout.Controls.Add(BuildSelectorPanel(), 0, 0)
            bodyLayout.Controls.Add(BuildHeaderFieldsPanel(), 0, 1)
            bodyLayout.Controls.Add(BuildReturnInputsPanel(), 0, 2)
            bodyLayout.Controls.Add(BuildGridPanel(), 0, 3)
            bodyLayout.Controls.Add(BuildButtonsPanel(), 0, 4)

            card.Controls.Add(bodyLayout)
            card.Controls.Add(headerPanel)

            rootPanel.Controls.Add(card)
            Controls.Add(rootPanel)
        Finally
            ResumeLayout(True)
            PerformLayout()
        End Try

        _uiInitialized = True
    End Sub

    Private Function BuildSelectorPanel() As Control
        Dim table As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .BackColor = Color.Transparent,
            .Margin = Padding.Empty
        }
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldLabelHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldControlHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        _cboDelivery = CreateComboBox()
        _cboDelivery.Name = "cboPendingDelivery"
        AddHandler _cboDelivery.SelectedIndexChanged, AddressOf CboDelivery_SelectedIndexChanged
        AddHandler _cboDelivery.SelectionChangeCommitted, AddressOf CboDelivery_SelectionChangeCommitted

        table.Controls.Add(CreateFieldLabel("Pending Delivery / Order"), 0, 0)
        table.Controls.Add(_cboDelivery, 0, 1)

        Return table
    End Function

    Private Function BuildHeaderFieldsPanel() As Control
        Dim table As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 3,
            .BackColor = Color.Transparent,
            .Margin = Padding.Empty
        }
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldLabelHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldControlHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        _txtSupplier = CreateDisplayTextBox()
        _txtDeliveryNumber = CreateDisplayTextBox()
        _txtOrderNumber = CreateDisplayTextBox()
        _txtDeliveryDate = CreateDisplayTextBox()

        AddLabeledControl(table, 0, "Supplier", _txtSupplier)
        AddLabeledControl(table, 1, "Delivery Number", _txtDeliveryNumber)
        AddLabeledControl(table, 2, "Order Number", _txtOrderNumber)
        AddLabeledControl(table, 3, "Delivery Date", _txtDeliveryDate, False)

        Return table
    End Function

    Private Function BuildReturnInputsPanel() As Control
        Dim table As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 3,
            .RowCount = 4,
            .BackColor = Color.Transparent,
            .Margin = Padding.Empty
        }
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.34F))
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
        table.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldLabelHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldControlHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Absolute, CSng(FieldLabelHeight)))
        table.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        _cboReturnType = CreateComboBox()
        _cboReturnType.Items.AddRange(New Object() {"Damaged", "Wrong", "Missing"})
        _cboReturnType.SelectedIndex = -1

        _cboResolution = CreateComboBox()
        _cboResolution.Items.AddRange(New Object() {"Replacement", "CreditMemo", "Refund"})
        _cboResolution.SelectedIndex = -1

        _dtpReturnDate = CreateDatePicker()
        _dtpReturnDate.Value = Date.Today
        _dtpReturnDate.MaxDate = Date.Today

        _txtNotes = CreateMultilineTextBox()

        AddLabeledControl(table, 0, "Return Type", _cboReturnType)
        AddLabeledControl(table, 1, "Resolution", _cboResolution)
        AddLabeledControl(table, 2, "Return Date", _dtpReturnDate, False)

        Dim notesLabel As Label = CreateFieldLabel("Notes")
        notesLabel.Margin = Padding.Empty
        table.Controls.Add(notesLabel, 0, 2)
        table.SetColumnSpan(notesLabel, 3)

        _txtNotes.Margin = Padding.Empty
        table.Controls.Add(_txtNotes, 0, 3)
        table.SetColumnSpan(_txtNotes, 3)

        Return table
    End Function

    Private Function BuildGridPanel() As Control
        _grid = New Guna2DataGridView With {
            .Dock = DockStyle.Fill,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AllowUserToResizeRows = False,
            .RowHeadersVisible = False,
            .SelectionMode = DataGridViewSelectionMode.CellSelect,
            .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            .ColumnHeadersHeight = 38,
            .EditMode = DataGridViewEditMode.EditOnEnter,
            .Margin = Padding.Empty,
            .BackgroundColor = Color.FromArgb(32, 35, 42),
            .GridColor = Color.FromArgb(228, 231, 237),
            .BorderStyle = BorderStyle.None
        }
        ApplyGridTheme(_grid)
        AddHandler _grid.DataError, AddressOf Grid_DataError
        AddHandler _grid.CellValidating, AddressOf Grid_CellValidating

        Dim host As New Guna2Panel With {
            .Dock = DockStyle.Fill,
            .FillColor = Color.FromArgb(27, 29, 36),
            .BorderColor = Color.FromArgb(58, 62, 72),
            .BorderThickness = 1,
            .BorderRadius = 14,
            .Padding = New Padding(1)
        }
        host.Controls.Add(_grid)
        Return host
    End Function

    Private Function BuildButtonsPanel() As Control
        Dim panel As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.Transparent,
            .Padding = New Padding(0, 10, 0, 0)
        }

        _btnCancel = New Guna2Button With {
            .Text = "Cancel",
            .Size = New Size(110, 42)
        }
        StyleSecondaryButton(_btnCancel)
        AddHandler _btnCancel.Click, Sub() CloseWithCancel()

        _btnSave = New Guna2Button With {
            .Text = "Save Return",
            .Size = New Size(140, 42)
        }
        StylePrimaryButton(_btnSave)
        AddHandler _btnSave.Click, AddressOf BtnSave_Click

        Dim buttons As New FlowLayoutPanel With {
            .Dock = DockStyle.Right,
            .FlowDirection = FlowDirection.RightToLeft,
            .WrapContents = False,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = Color.Transparent
        }
        buttons.Controls.Add(_btnSave)
        buttons.Controls.Add(_btnCancel)

        panel.Controls.Add(buttons)
        Return panel
    End Function

    Private Sub LoadPendingDeliveries()
        Dim lookup As DataTable = _service.GetPendingSupplierReturnDeliveryLookup()
        _deliveryOptions = New List(Of PendingDeliveryOption) From {
            New PendingDeliveryOption With {
                .DeliveryID = 0,
                .DisplayText = "-- Select Pending Delivery / Order --"
            }
        }

        If lookup IsNot Nothing Then
            For Each row As DataRow In lookup.Rows
                If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For

                Dim optionItem As New PendingDeliveryOption With {
                    .DeliveryID = SafeToInt(row("DeliveryID")),
                    .SupplierName = Convert.ToString(row("SupplierName")).Trim(),
                    .DeliveryNumber = Convert.ToString(row("DeliveryNumber")).Trim(),
                    .OrderNumber = Convert.ToString(row("OrderNumber")).Trim(),
                    .DisplayText = Convert.ToString(row("DisplayText")).Trim()
                }

                If Not IsDBNull(row("DeliveryDate")) Then
                    optionItem.DeliveryDate = Convert.ToDateTime(row("DeliveryDate"))
                End If

                _deliveryOptions.Add(optionItem)
            Next
        End If

        _isLoadingDeliverySelection = True
        Try
            _cboDelivery.Items.Clear()
            For Each optionItem As PendingDeliveryOption In _deliveryOptions
                _cboDelivery.Items.Add(optionItem)
            Next

            Dim selectedIndex As Integer = 0
            If DeliveryId > 0 Then
                Dim matchedIndex As Integer = _deliveryOptions.FindIndex(Function(item) item IsNot Nothing AndAlso item.DeliveryID = DeliveryId)
                If matchedIndex >= 0 Then
                    selectedIndex = matchedIndex
                Else
                    DeliveryId = -1
                End If
            Else
                DeliveryId = -1
            End If

            _cboDelivery.SelectedIndex = selectedIndex
        Finally
            _isLoadingDeliverySelection = False
        End Try

        If DeliveryId > 0 Then
            LoadSelectedDelivery(DeliveryId)
        Else
            ClearSelectedDeliveryContext()
            If _deliveryOptions.Count <= 1 Then
                MessageBox.Show("No pending deliveries are available for return/refund.", "No Pending Deliveries", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub CboDelivery_SelectedIndexChanged(sender As Object, e As EventArgs)
        If _isLoadingDeliverySelection Then Return
        SyncSelectedDeliveryContext()
    End Sub

    Private Sub CboDelivery_SelectionChangeCommitted(sender As Object, e As EventArgs)
        If _isLoadingDeliverySelection Then Return
        SyncSelectedDeliveryContext()
    End Sub

    Private Function GetSelectedDeliveryOption() As PendingDeliveryOption
        If _cboDelivery Is Nothing OrElse _cboDelivery.IsDisposed Then Return Nothing

        Dim selectedOption As PendingDeliveryOption = TryCast(_cboDelivery.SelectedItem, PendingDeliveryOption)
        If selectedOption IsNot Nothing Then
            Return selectedOption
        End If

        Dim selectedIndex As Integer = _cboDelivery.SelectedIndex
        If selectedIndex >= 0 AndAlso selectedIndex < _cboDelivery.Items.Count Then
            selectedOption = TryCast(_cboDelivery.Items(selectedIndex), PendingDeliveryOption)
            If selectedOption IsNot Nothing Then
                Return selectedOption
            End If
        End If

        Dim comboText As String = _cboDelivery.Text.Trim()
        If comboText.Length = 0 OrElse _deliveryOptions Is Nothing Then
            Return Nothing
        End If

        For Each optionItem As PendingDeliveryOption In _deliveryOptions
            If optionItem Is Nothing OrElse optionItem.DeliveryID <= 0 Then Continue For
            If String.Equals(optionItem.DisplayText, comboText, StringComparison.OrdinalIgnoreCase) Then
                Return optionItem
            End If
        Next

        Return TryResolveDeliveryOptionFromVisibleFields()
    End Function

    Private Function GetSelectedDeliveryId() As Integer
        Dim selectedOption As PendingDeliveryOption = GetSelectedDeliveryOption()
        If selectedOption Is Nothing Then Return 0

        Return selectedOption.DeliveryID
    End Function

    Private Sub SyncSelectedDeliveryContext()
        Dim selectedOption As PendingDeliveryOption = GetSelectedDeliveryOption()
        If selectedOption Is Nothing OrElse selectedOption.DeliveryID <= 0 Then
            If DeliveryId > 0 AndAlso _context IsNot Nothing Then
                Return
            End If

            DeliveryId = -1
            ClearSelectedDeliveryContext()
            Return
        End If

        If DeliveryId = selectedOption.DeliveryID AndAlso _context IsNot Nothing Then
            Return
        End If

        LoadSelectedDelivery(selectedOption.DeliveryID)
    End Sub

    Private Function TryResolveDeliveryOptionFromVisibleFields() As PendingDeliveryOption
        If _deliveryOptions Is Nothing OrElse _deliveryOptions.Count = 0 Then
            Return Nothing
        End If

        Dim deliveryNumber As String = If(_txtDeliveryNumber Is Nothing OrElse _txtDeliveryNumber.IsDisposed, String.Empty, _txtDeliveryNumber.Text.Trim())
        Dim orderNumber As String = If(_txtOrderNumber Is Nothing OrElse _txtOrderNumber.IsDisposed, String.Empty, _txtOrderNumber.Text.Trim())
        Dim supplierName As String = If(_txtSupplier Is Nothing OrElse _txtSupplier.IsDisposed, String.Empty, _txtSupplier.Text.Trim())

        If deliveryNumber.Length = 0 AndAlso orderNumber.Length = 0 AndAlso supplierName.Length = 0 Then
            Return Nothing
        End If

        For Each optionItem As PendingDeliveryOption In _deliveryOptions
            If optionItem Is Nothing OrElse optionItem.DeliveryID <= 0 Then Continue For

            If deliveryNumber.Length > 0 AndAlso
               String.Equals(optionItem.DeliveryNumber, deliveryNumber, StringComparison.OrdinalIgnoreCase) Then
                Return optionItem
            End If
        Next

        For Each optionItem As PendingDeliveryOption In _deliveryOptions
            If optionItem Is Nothing OrElse optionItem.DeliveryID <= 0 Then Continue For

            If orderNumber.Length > 0 AndAlso
               String.Equals(optionItem.OrderNumber, orderNumber, StringComparison.OrdinalIgnoreCase) AndAlso
               (supplierName.Length = 0 OrElse String.Equals(optionItem.SupplierName, supplierName, StringComparison.OrdinalIgnoreCase)) Then
                Return optionItem
            End If
        Next

        Return Nothing
    End Function

    Private Function EnsureCurrentDeliveryContext() As Boolean
        If DeliveryId > 0 AndAlso _context IsNot Nothing Then
            Return True
        End If

        If _context IsNot Nothing AndAlso _context.Table IsNot Nothing AndAlso _context.Table.Columns.Contains("DeliveryID") Then
            Dim contextDeliveryId As Integer = SafeToInt(_context("DeliveryID"))
            If contextDeliveryId > 0 Then
                DeliveryId = contextDeliveryId
                Return True
            End If
        End If

        Dim selectedOption As PendingDeliveryOption = GetSelectedDeliveryOption()
        If selectedOption Is Nothing Then
            selectedOption = TryResolveDeliveryOptionFromVisibleFields()
        End If

        If selectedOption Is Nothing OrElse selectedOption.DeliveryID <= 0 Then
            Return False
        End If

        LoadSelectedDelivery(selectedOption.DeliveryID)
        Return DeliveryId > 0 AndAlso _context IsNot Nothing
    End Function

    Private Sub LoadSelectedDelivery(deliveryId As Integer)
        If deliveryId <= 0 Then
            DeliveryId = -1
            ClearSelectedDeliveryContext()
            Return
        End If

        Dim contextRow As DataRow = _service.GetSupplierReturnContextByDeliveryId(deliveryId)
        If contextRow Is Nothing Then
            MessageBox.Show("Selected delivery record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ResetDeliverySelection()
            Return
        End If

        Dim deliveryStatus As String = Convert.ToString(contextRow("DeliveryStatus")).Trim()
        Dim pendingItemCount As Integer = SafeToInt(contextRow("PendingItemCount"))

        If Not String.Equals(deliveryStatus, "Pending", StringComparison.OrdinalIgnoreCase) OrElse pendingItemCount <= 0 Then
            MessageBox.Show("Cannot return/refund Posted deliveries.", "Return Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ResetDeliverySelection()
            Return
        End If

        DeliveryId = deliveryId
        _context = contextRow
        _itemsTable = _service.GetSupplierReturnItemsByDeliveryId(deliveryId)
        NormalizeItemsTable()
        PopulateHeaderFields()
        BindReturnItemsGrid()
    End Sub

    Private Sub PopulateHeaderFields()
        If _context Is Nothing Then
            _txtSupplier.Clear()
            _txtDeliveryNumber.Clear()
            _txtOrderNumber.Clear()
            _txtDeliveryDate.Clear()
            Return
        End If

        _txtSupplier.Text = Convert.ToString(_context("SupplierName")).Trim()
        _txtDeliveryNumber.Text = Convert.ToString(_context("DeliveryNumber")).Trim()
        _txtOrderNumber.Text = Convert.ToString(_context("OrderNumber")).Trim()

        If Not IsDBNull(_context("DeliveryDate")) Then
            _txtDeliveryDate.Text = Convert.ToDateTime(_context("DeliveryDate")).ToString("MMMM d, yyyy")
        Else
            _txtDeliveryDate.Clear()
        End If
    End Sub

    Private Sub ResetDeliverySelection()
        DeliveryId = -1
        _isLoadingDeliverySelection = True
        Try
            If _cboDelivery IsNot Nothing AndAlso Not _cboDelivery.IsDisposed AndAlso _cboDelivery.Items.Count > 0 Then
                _cboDelivery.SelectedIndex = 0
            End If
        Finally
            _isLoadingDeliverySelection = False
        End Try

        ClearSelectedDeliveryContext()
    End Sub

    Private Sub ClearSelectedDeliveryContext()
        _context = Nothing
        _itemsTable = CreateEmptyItemsTable()
        PopulateHeaderFields()
        BindReturnItemsGrid()
    End Sub

    Private Function CreateEmptyItemsTable() As DataTable
        Dim table As New DataTable()
        table.Columns.Add(ColDeliveryProductID, GetType(Integer))
        table.Columns.Add(ColProductID, GetType(Integer))
        table.Columns.Add(ColProduct, GetType(String))
        table.Columns.Add(ColBarcode, GetType(String))
        table.Columns.Add(ColDeliveredQty, GetType(Integer))
        table.Columns.Add(ColReturnedQty, GetType(Integer))
        table.Columns.Add(ColMaxReturnable, GetType(Integer))
        table.Columns.Add(ColStatus, GetType(String))
        table.Columns.Add(ColReturnQty, GetType(Integer))
        Return table
    End Function

    Private Sub NormalizeItemsTable()
        If _itemsTable Is Nothing Then
            _itemsTable = CreateEmptyItemsTable()
            Return
        End If

        EnsureItemsTableColumn(ColDeliveryProductID, GetType(Integer))
        EnsureItemsTableColumn(ColProductID, GetType(Integer))
        EnsureItemsTableColumn(ColProduct, GetType(String))
        EnsureItemsTableColumn(ColBarcode, GetType(String))
        EnsureItemsTableColumn(ColDeliveredQty, GetType(Integer))
        EnsureItemsTableColumn(ColReturnedQty, GetType(Integer))
        EnsureItemsTableColumn(ColMaxReturnable, GetType(Integer))
        EnsureItemsTableColumn(ColStatus, GetType(String))
        EnsureItemsTableColumn(ColReturnQty, GetType(Integer))

        For Each row As DataRow In _itemsTable.Rows
            If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For

            Dim deliveredQty As Integer = SafeToInt(row(ColDeliveredQty))
            Dim returnedQty As Integer = SafeToInt(row(ColReturnedQty))
            If deliveredQty < returnedQty Then
                deliveredQty = returnedQty
            End If

            row(ColDeliveredQty) = deliveredQty
            row(ColReturnedQty) = Math.Max(0, returnedQty)
            row(ColMaxReturnable) = Math.Max(0, deliveredQty - returnedQty)

            Dim enteredQty As Integer = SafeToInt(row(ColReturnQty))
            If enteredQty < 0 Then enteredQty = 0
            row(ColReturnQty) = Math.Min(enteredQty, SafeToInt(row(ColMaxReturnable)))
        Next
    End Sub

    Private Sub EnsureItemsTableColumn(columnName As String, columnType As Type)
        If _itemsTable.Columns.Contains(columnName) Then Return
        _itemsTable.Columns.Add(columnName, columnType)
    End Sub

    Private Sub BindReturnItemsGrid()
        If _itemsTable Is Nothing Then
            _itemsTable = CreateEmptyItemsTable()
        End If

        _grid.AutoGenerateColumns = False
        _grid.DataSource = Nothing
        _grid.Columns.Clear()

        _grid.Columns.Add(CreateHiddenTextColumn(ColDeliveryProductID, ColDeliveryProductID))
        _grid.Columns.Add(CreateHiddenTextColumn(ColProductID, ColProductID))
        _grid.Columns.Add(CreateHiddenTextColumn(ColStatus, ColStatus))
        _grid.Columns.Add(CreateTextColumn(ColProduct, ColProduct, "Product", 36, True, DataGridViewContentAlignment.MiddleLeft))
        _grid.Columns.Add(CreateTextColumn(ColBarcode, ColBarcode, "Barcode", 20, True, DataGridViewContentAlignment.MiddleLeft))
        _grid.Columns.Add(CreateTextColumn(ColDeliveredQty, ColDeliveredQty, "Delivered Qty", 14, True, DataGridViewContentAlignment.MiddleCenter))
        _grid.Columns.Add(CreateTextColumn(ColReturnedQty, ColReturnedQty, "Returned Qty", 14, True, DataGridViewContentAlignment.MiddleCenter))
        _grid.Columns.Add(CreateTextColumn(ColMaxReturnable, ColMaxReturnable, "Max Returnable", 16, True, DataGridViewContentAlignment.MiddleCenter))
        _grid.Columns.Add(CreateTextColumn(ColReturnQty, ColReturnQty, "Return Qty", 14, False, DataGridViewContentAlignment.MiddleCenter))

        _grid.DataSource = _itemsTable
        _grid.ReadOnly = False

        For Each columnName As String In New String() {ColDeliveredQty, ColReturnedQty, ColMaxReturnable, ColReturnQty}
            If _grid.Columns.Contains(columnName) Then
                _grid.Columns(columnName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
        Next

        For Each col As DataGridViewColumn In _grid.Columns
            col.ReadOnly = (col.Name <> ColReturnQty)
        Next

        _grid.ClearSelection()

        If _grid.Rows.Count > 0 Then
            BeginInvoke(New MethodInvoker(Sub() FocusFirstReturnQtyCell(True)))
        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs)
        If _grid IsNot Nothing Then
            _grid.EndEdit()
        End If

        SyncSelectedDeliveryContext()

        Dim validationMessage As String = ValidateForm()
        If validationMessage.Length > 0 Then
            FocusControlForValidation(validationMessage)
            MessageBox.Show(validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim returnItems As List(Of SupplierReturnLineItem) = BuildReturnItems()
        If returnItems.Count = 0 Then
            FocusFirstReturnQtyCell(True)
            MessageBox.Show("Enter a Return Qty greater than 0 for at least one item.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Are you sure you want to save this supplier return?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Dim result As SupplierReturnSaveResult = _service.SaveSupplierReturn(
                DeliveryId,
                _dtpReturnDate.Value,
                Convert.ToString(_cboReturnType.SelectedItem).Trim(),
                Convert.ToString(_cboResolution.SelectedItem).Trim(),
                _txtNotes.Text.Trim(),
                returnItems)

            ReturnNumber = If(result.ReturnNumber, String.Empty)
            AppliedItems = If(result.AppliedItems, New List(Of SupplierReturnAppliedItem)())
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateForm() As String
        If Not EnsureCurrentDeliveryContext() Then
            Return "Select a pending delivery/order."
        End If

        Dim deliveryStatus As String = Convert.ToString(_context("DeliveryStatus")).Trim()
        Dim pendingItemCount As Integer = SafeToInt(_context("PendingItemCount"))
        If Not String.Equals(deliveryStatus, "Pending", StringComparison.OrdinalIgnoreCase) OrElse pendingItemCount <= 0 Then
            Return "Cannot return/refund Posted deliveries."
        End If

        Dim acceptsReturnRefund As Boolean = False
        If _context.Table.Columns.Contains("AcceptsReturnRefund") AndAlso Not IsDBNull(_context("AcceptsReturnRefund")) Then
            acceptsReturnRefund = Convert.ToBoolean(_context("AcceptsReturnRefund"))
        End If
        If Not acceptsReturnRefund Then
            Return "This supplier does not accept return/refund requests."
        End If

        If _context.Table.Columns.Contains("ReturnWindowDays") AndAlso Not IsDBNull(_context("ReturnWindowDays")) Then
            Dim deliveryDate As DateTime = Convert.ToDateTime(_context("DeliveryDate")).Date
            Dim expiryDate As DateTime = deliveryDate.AddDays(Convert.ToInt32(_context("ReturnWindowDays")))
            If Date.Today > expiryDate.Date Then
                Return String.Format("The supplier return window expired on {0:MMMM d, yyyy}.", expiryDate)
            End If
        End If

        If _cboReturnType.SelectedIndex < 0 Then
            Return "Return Type is required."
        End If

        If _cboResolution.SelectedIndex < 0 Then
            Return "Resolution is required."
        End If

        If _itemsTable Is Nothing OrElse _itemsTable.Rows.Count = 0 Then
            Return "The selected delivery has no pending products available for return."
        End If

        Dim hasPositiveQty As Boolean = False

        For Each row As DataRow In _itemsTable.Rows
            If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For

            Dim returnQty As Integer
            If Not Integer.TryParse(Convert.ToString(row(ColReturnQty)), returnQty) Then
                Return String.Format("Return quantity for {0} must be numeric.", Convert.ToString(row(ColProduct)).Trim())
            End If

            If returnQty < 0 Then
                Return String.Format("Return quantity for {0} cannot be negative.", Convert.ToString(row(ColProduct)).Trim())
            End If

            If returnQty > 0 Then
                hasPositiveQty = True

                Dim lineStatus As String = Convert.ToString(row(ColStatus)).Trim()
                If Not String.Equals(lineStatus, "Pending", StringComparison.OrdinalIgnoreCase) Then
                    Return "Cannot return/refund Posted deliveries."
                End If

                Dim deliveredQty As Integer = SafeToInt(row(ColDeliveredQty))
                Dim maxReturnable As Integer = SafeToInt(row(ColMaxReturnable))
                If returnQty > deliveredQty Then
                    Return String.Format("Return quantity for {0} cannot exceed delivered quantity ({1}).", Convert.ToString(row(ColProduct)).Trim(), deliveredQty)
                End If

                If returnQty > maxReturnable Then
                    Return String.Format("Return quantity for {0} exceeds the max returnable quantity ({1}).", Convert.ToString(row(ColProduct)).Trim(), maxReturnable)
                End If
            End If
        Next

        If Not hasPositiveQty Then
            Return "Enter a Return Qty greater than 0 for at least one item."
        End If

        Return String.Empty
    End Function

    Private Sub FocusControlForValidation(validationMessage As String)
        Select Case validationMessage
            Case "Select a pending delivery/order."
                If _cboDelivery IsNot Nothing AndAlso Not _cboDelivery.IsDisposed Then
                    _cboDelivery.Focus()
                End If
            Case "Return Type is required."
                If _cboReturnType IsNot Nothing AndAlso Not _cboReturnType.IsDisposed Then
                    _cboReturnType.Focus()
                End If
            Case "Resolution is required."
                If _cboResolution IsNot Nothing AndAlso Not _cboResolution.IsDisposed Then
                    _cboResolution.Focus()
                End If
            Case "Enter a Return Qty greater than 0 for at least one item.",
                 "The selected delivery has no pending products available for return."
                FocusFirstReturnQtyCell(True)
        End Select
    End Sub

    Private Sub FocusFirstReturnQtyCell(startEditing As Boolean)
        If _grid Is Nothing OrElse _grid.IsDisposed Then Return
        If _grid.Rows.Count = 0 OrElse Not _grid.Columns.Contains(ColReturnQty) Then Return

        Dim targetCell As DataGridViewCell = _grid.Rows(0).Cells(ColReturnQty)
        _grid.ClearSelection()
        _grid.CurrentCell = targetCell
        targetCell.Selected = True
        _grid.Focus()

        If startEditing Then
            _grid.BeginEdit(True)
        End If
    End Sub

    Private Function BuildReturnItems() As List(Of SupplierReturnLineItem)
        Dim items As New List(Of SupplierReturnLineItem)()
        If _itemsTable Is Nothing Then Return items

        For Each row As DataRow In _itemsTable.Rows
            If row Is Nothing OrElse row.RowState = DataRowState.Deleted Then Continue For

            Dim returnQty As Integer
            If Not Integer.TryParse(Convert.ToString(row(ColReturnQty)), returnQty) Then Continue For
            If returnQty <= 0 Then Continue For

            items.Add(New SupplierReturnLineItem With {
                .DeliveryProductID = SafeToInt(row(ColDeliveryProductID)),
                .ProductID = SafeToInt(row(ColProductID)),
                .Quantity = returnQty
            })
        Next

        Return items
    End Function

    Private Sub Grid_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If _grid.Columns(e.ColumnIndex).Name <> ColReturnQty Then Return

        Dim input As String = If(e.FormattedValue, String.Empty).ToString().Trim()
        If input.Length = 0 Then
            _grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            Return
        End If

        Dim parsed As Integer
        If Not Integer.TryParse(input, parsed) OrElse parsed < 0 Then
            MessageBox.Show("Return Qty must be a whole number greater than or equal to 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
            Return
        End If

        Dim deliveredQty As Integer = SafeToInt(_grid.Rows(e.RowIndex).Cells(ColDeliveredQty).Value)
        Dim maxReturnable As Integer = SafeToInt(_grid.Rows(e.RowIndex).Cells(ColMaxReturnable).Value)

        If parsed > deliveredQty Then
            MessageBox.Show(String.Format("Return Qty cannot exceed delivered quantity ({0}).", deliveredQty), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
            Return
        End If

        If parsed > maxReturnable Then
            MessageBox.Show(String.Format("Return Qty cannot exceed {0}.", maxReturnable), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End If
    End Sub

    Private Sub Grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        MessageBox.Show("Return Qty must be numeric.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.ThrowException = False
    End Sub

    Private Sub CloseWithCancel()
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Shared Function CreateHiddenTextColumn(name As String, dataPropertyName As String) As DataGridViewTextBoxColumn
        Return New DataGridViewTextBoxColumn With {
            .Name = name,
            .DataPropertyName = dataPropertyName,
            .Visible = False
        }
    End Function

    Private Shared Function CreateTextColumn(name As String,
                                             dataPropertyName As String,
                                             headerText As String,
                                             fillWeight As Integer,
                                             isReadOnly As Boolean,
                                             alignment As DataGridViewContentAlignment) As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn With {
            .Name = name,
            .DataPropertyName = dataPropertyName,
            .HeaderText = headerText,
            .ReadOnly = isReadOnly,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = fillWeight,
            .MinimumWidth = 80,
            .SortMode = DataGridViewColumnSortMode.NotSortable
        }
        col.DefaultCellStyle.Alignment = alignment
        Return col
    End Function

    Private Sub AddLabeledControl(table As TableLayoutPanel, columnIndex As Integer, labelText As String, control As Control, Optional hasRightMargin As Boolean = True)
        Dim label As Label = CreateFieldLabel(labelText)
        label.Margin = If(hasRightMargin, New Padding(0, 0, FieldRightSpacing, 0), Padding.Empty)
        control.Margin = If(hasRightMargin, New Padding(0, 0, FieldRightSpacing, 0), Padding.Empty)
        table.Controls.Add(label, columnIndex, 0)
        table.Controls.Add(control, columnIndex, 1)
    End Sub

    Private Shared Function CreateFieldLabel(textValue As String) As Label
        Return New Label With {
            .Text = textValue,
            .Dock = DockStyle.Fill,
            .AutoSize = False,
            .BackColor = Color.Transparent,
            .ForeColor = Color.FromArgb(182, 191, 208),
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Margin = Padding.Empty
        }
    End Function

    Private Shared Function CreateDisplayTextBox() As Guna2TextBox
        Dim tb As Guna2TextBox = CreateTextBoxBase()
        tb.ReadOnly = True
        tb.TabStop = False
        tb.Cursor = Cursors.Default
        tb.FillColor = Color.FromArgb(55, 58, 66)
        tb.ForeColor = Color.FromArgb(220, 224, 230)
        Return tb
    End Function

    Private Shared Function CreateTextBoxBase() As Guna2TextBox
        Dim tb As New Guna2TextBox With {
            .Dock = DockStyle.Fill,
            .Height = FieldControlHeight,
            .Animated = False,
            .AutoRoundedCorners = False,
            .BorderRadius = 12,
            .BorderThickness = 1,
            .BorderColor = Color.FromArgb(74, 79, 90),
            .FillColor = Color.FromArgb(41, 44, 52),
            .ForeColor = Color.FromArgb(238, 241, 245),
            .PlaceholderForeColor = Color.FromArgb(146, 151, 162),
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular),
            .Margin = Padding.Empty
        }
        tb.MinimumSize = New Size(120, FieldControlHeight)
        tb.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        tb.HoverState.BorderColor = Color.FromArgb(112, 118, 130)
        Return tb
    End Function

    Private Shared Function CreateMultilineTextBox() As Guna2TextBox
        Dim tb As Guna2TextBox = CreateTextBoxBase()
        tb.Multiline = True
        tb.Height = 100
        tb.ScrollBars = ScrollBars.Vertical
        tb.MaxLength = 250
        Return tb
    End Function

    Private Shared Function CreateComboBox() As Guna2ComboBox
        Dim cbo As New Guna2ComboBox With {
            .Dock = DockStyle.Fill,
            .Height = FieldControlHeight,
            .Animated = False,
            .AutoRoundedCorners = False,
            .BorderRadius = 12,
            .BorderThickness = 1,
            .BorderColor = Color.FromArgb(74, 79, 90),
            .FillColor = Color.FromArgb(41, 44, 52),
            .FocusedColor = Color.FromArgb(0, 122, 204),
            .ForeColor = Color.FromArgb(238, 241, 245),
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular),
            .BackColor = Color.Transparent,
            .DrawMode = DrawMode.OwnerDrawFixed,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .IntegralHeight = False,
            .ItemHeight = 38,
            .Margin = Padding.Empty
        }
        cbo.MinimumSize = New Size(120, FieldControlHeight)
        cbo.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        cbo.HoverState.BorderColor = Color.FromArgb(112, 118, 130)
        Return cbo
    End Function

    Private Shared Function CreateDatePicker() As Guna2DateTimePicker
        Dim picker As New Guna2DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Height = FieldControlHeight,
            .Animated = False,
            .AutoRoundedCorners = False,
            .BorderRadius = 12,
            .BorderThickness = 1,
            .BorderColor = Color.FromArgb(74, 79, 90),
            .FillColor = Color.FromArgb(55, 58, 66),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular),
            .Format = DateTimePickerFormat.Long,
            .Margin = Padding.Empty
        }
        picker.MinimumSize = New Size(120, FieldControlHeight)
        picker.CheckedState.FillColor = picker.FillColor
        picker.FocusedColor = Color.FromArgb(0, 122, 204)
        picker.HoverState.BorderColor = Color.FromArgb(112, 118, 130)
        Return picker
    End Function

    Private Shared Sub StylePrimaryButton(button As Guna2Button)
        button.Animated = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 12
        button.BorderThickness = 0
        button.FillColor = Color.FromArgb(0, 122, 204)
        button.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        button.Cursor = Cursors.Hand
        button.HoverState.FillColor = Color.FromArgb(27, 142, 224)
        button.PressedColor = Color.FromArgb(0, 102, 184)
        button.UseTransparentBackground = False
        button.Margin = New Padding(10, 0, 0, 0)
    End Sub

    Private Shared Sub StyleSecondaryButton(button As Guna2Button)
        button.Animated = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 12
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(78, 83, 94)
        button.FillColor = Color.FromArgb(60, 65, 76)
        button.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        button.Cursor = Cursors.Hand
        button.HoverState.FillColor = Color.FromArgb(74, 79, 90)
        button.Margin = New Padding(10, 0, 0, 0)
    End Sub

    Private Shared Sub StyleHeaderCloseButton(button As Guna2Button)
        StyleSecondaryButton(button)
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
        button.BorderRadius = 12
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(174, 74, 74)
        button.FillColor = Color.FromArgb(196, 66, 66)
        button.ForeColor = Color.White
        button.HoverState.FillColor = Color.FromArgb(214, 82, 82)
        button.HoverState.BorderColor = Color.FromArgb(228, 104, 104)
        button.HoverState.ForeColor = Color.White
        button.PressedColor = Color.FromArgb(176, 52, 52)
        button.Margin = Padding.Empty
    End Sub

    Private Shared Sub PositionHeaderCloseButton(headerPanel As Panel, button As Guna2Button)
        If headerPanel Is Nothing OrElse button Is Nothing OrElse button.IsDisposed Then Return
        Dim x As Integer = Math.Max(0, headerPanel.ClientSize.Width - button.Width)
        Dim y As Integer = Math.Max(0, (headerPanel.ClientSize.Height - button.Height) \ 2)
        button.Location = New Point(x, y)
    End Sub

    Private Shared Sub ApplyGridTheme(grid As Guna2DataGridView)
        grid.EnableHeadersVisualStyles = False
        grid.BorderStyle = BorderStyle.None
        grid.RowTemplate.Height = 34
        grid.DefaultCellStyle.BackColor = Color.White
        grid.DefaultCellStyle.ForeColor = Color.Black
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(213, 232, 255)
        grid.DefaultCellStyle.SelectionForeColor = Color.Black
        grid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 226, 226)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 226, 226)
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 249, 249)
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(213, 232, 255)
        grid.GridColor = Color.FromArgb(230, 230, 230)

        grid.ThemeStyle.BackColor = Color.White
        grid.ThemeStyle.GridColor = Color.FromArgb(230, 230, 230)
        grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(226, 226, 226)
        grid.ThemeStyle.HeaderStyle.ForeColor = Color.Black
        grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        grid.ThemeStyle.HeaderStyle.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        grid.ThemeStyle.HeaderStyle.Height = 38
        grid.ThemeStyle.RowsStyle.BackColor = Color.White
        grid.ThemeStyle.RowsStyle.ForeColor = Color.Black
        grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(213, 232, 255)
        grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black
        grid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        grid.ThemeStyle.ReadOnly = False
    End Sub

    Private Shared Function SafeToInt(value As Object) As Integer
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0

        Dim parsed As Integer
        If Integer.TryParse(Convert.ToString(value), parsed) Then
            Return parsed
        End If

        Return 0
    End Function

End Class
