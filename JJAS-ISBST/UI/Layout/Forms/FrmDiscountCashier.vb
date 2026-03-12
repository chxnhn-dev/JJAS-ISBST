Imports System.Data

Public Class FrmDiscountCashier
    Inherits ModuleListBaseForm

    Private Const ColId As String = "DiscountID"
    Private Const ColName As String = "DiscountName"
    Private Const ColValue As String = "DiscountValue"
    Private Const ColDescription As String = "Description"
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"

    Private _service As DiscountService
    Private _selectedId As Integer = -1

    Public Sub New()
        MyBase.New()
        Text = "Cashier Discounts"
    End Sub

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.DiscountTab
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Discount:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Senior / PWD"
        End Get
    End Property

    Protected Overrides ReadOnly Property SupportsPagination As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property ShowAddButton As Boolean
        Get
            Return True
        End Get
    End Property

    Private Function GetDiscountService() As DiscountService
        If _service Is Nothing Then
            _service = New DiscountService()
        End If

        Return _service
    End Function

    Protected Overrides Sub ConfigureModulePermissions()
        If IsCashierUser() Then
            ApplyCashierSidebarRestrictions()
        ElseIf IsStaffUser() Then
            ApplyStaffSidebarRestrictions()
        End If
    End Sub

    Protected Overrides Function CanNavigateToModule(targetTab As ModuleTab) As Boolean
        If Not IsCashierUser() Then
            Return MyBase.CanNavigateToModule(targetTab)
        End If

        Return targetTab = ModuleTab.DiscountTab OrElse
               targetTab = ModuleTab.InventoryTab OrElse
               targetTab = ModuleTab.TransactionsTab
    End Function

    Protected Overrides Sub LoadModuleData(searchText As String)
        Try
            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) GetDiscountService().GetDiscountsPage(request))

            EnsureDiscountGridColumns()
            DGVtable.DataSource = page.Records
            ApplyDiscountGridFormatting()
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading discounts: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub HandleAddAction()
        Dim entryForm As New FrmDiscountEntry()
        If ShowDiscountEntryModal(entryForm) = DialogResult.OK Then
            ReloadData()
        End If
    End Sub

    Private Function ShowDiscountEntryModal(entryForm As Form) As DialogResult
        Return FileMaintenance.EntryModalHost.ShowLikeUserProduct(Me, entryForm)
    End Function

    Private Sub EnsureDiscountGridColumns()
        If DGVtable.Columns.Count > 0 AndAlso DGVtable.Columns.Contains(ColDelete) Then Return

        DGVtable.AutoGenerateColumns = False
        DGVtable.Columns.Clear()

        DGVtable.Columns.Add(New DataGridViewTextBoxColumn() With {
            .Name = ColId,
            .DataPropertyName = ColId,
            .Visible = False
        })

        DGVtable.Columns.Add(New DataGridViewTextBoxColumn() With {
            .Name = ColName,
            .DataPropertyName = ColName,
            .HeaderText = "Discount Type",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 34.0F,
            .MinimumWidth = 220
        })

        DGVtable.Columns.Add(New DataGridViewTextBoxColumn() With {
            .Name = ColValue,
            .DataPropertyName = ColValue,
            .HeaderText = "Value",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .Width = 120,
            .MinimumWidth = 120,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleRight,
                .Format = "0.##'%'"
            }
        })

        DGVtable.Columns.Add(New DataGridViewTextBoxColumn() With {
            .Name = ColDescription,
            .DataPropertyName = ColDescription,
            .HeaderText = "Description",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 46.0F,
            .MinimumWidth = 360,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .NullValue = "-"
            }
        })

        DGVtable.Columns.Add(New DataGridViewButtonColumn() With {
            .Name = ColViewEdit,
            .HeaderText = "Action",
            .Text = "View/Edit",
            .UseColumnTextForButtonValue = True,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .Width = 120,
            .MinimumWidth = 120,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        })

        DGVtable.Columns.Add(New DataGridViewButtonColumn() With {
            .Name = ColDelete,
            .HeaderText = "Delete",
            .Text = "Delete",
            .UseColumnTextForButtonValue = True,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .Width = 100,
            .MinimumWidth = 100,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        })
    End Sub

    Private Sub ApplyDiscountGridFormatting()
        If DGVtable Is Nothing OrElse DGVtable.Columns.Count = 0 Then Return

        DGVtable.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGVtable.MultiSelect = False
        DGVtable.ReadOnly = True
        ApplyStandardGridLayout(DGVtable)
        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        ApplyDiscountColumnOrder()
    End Sub

    Private Sub ApplyDiscountColumnOrder()
        If DGVtable Is Nothing Then Return

        Dim requiredColumns As String() = {ColName, ColValue, ColDescription, ColViewEdit, ColDelete}
        For Each columnName As String In requiredColumns
            If Not DGVtable.Columns.Contains(columnName) Then Return
            If Not DGVtable.Columns(columnName).Visible Then Return
        Next

        Dim visibleColumnCount As Integer = DGVtable.Columns.GetColumnCount(DataGridViewElementStates.Visible)
        If visibleColumnCount < requiredColumns.Length Then Return

        DGVtable.Columns(ColName).DisplayIndex = 0
        DGVtable.Columns(ColValue).DisplayIndex = 1
        DGVtable.Columns(ColDescription).DisplayIndex = 2
        DGVtable.Columns(ColViewEdit).DisplayIndex = visibleColumnCount - 2
        DGVtable.Columns(ColDelete).DisplayIndex = visibleColumnCount - 1
    End Sub

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplyDiscountColumnOrder()
    End Sub

    Private Sub OpenEditModalById(discountId As Integer)
        Dim entryForm As New FrmDiscountEntry With {
            .Mode = EntryFormMode.EditExisting,
            .SelectedId = discountId
        }

        If ShowDiscountEntryModal(entryForm) = DialogResult.OK Then
            ReloadData()
        End If
    End Sub

    Private Sub DeleteById(discountId As Integer)
        If MessageBox.Show("Are you sure you want to delete this discount?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        GetDiscountService().DeleteDiscount(discountId)
        LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "Deleted Discount.")
        ReloadData()
    End Sub

    Private Function TryGetId(row As DataGridViewRow, ByRef discountId As Integer) As Boolean
        discountId = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColId) Then Return False

        Dim raw As Object = row.Cells(ColId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), discountId)
    End Function

    Private Sub DGVtable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        If Not TryGetId(row, _selectedId) Then
            _selectedId = -1
        End If
    End Sub

    Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim discountId As Integer
        If Not TryGetId(row, discountId) Then Exit Sub

        _selectedId = discountId

        If colName = ColViewEdit Then
            OpenEditModalById(discountId)
        ElseIf colName = ColDelete Then
            DeleteById(discountId)
        End If
    End Sub
End Class
