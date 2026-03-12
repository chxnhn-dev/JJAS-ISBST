Namespace FileMaintenance
    Public Class Brand
        Inherits FileMaintenanceBaseForm

        Private _service As BrandService

        Private Const ColViewEdit As String = "colViewEdit"
        Private Const ColDelete As String = "colDelete"
        Private Const ColId As String = "BrandID"

        Private selectedId As Integer = -1

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.BrandTab
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchCaption As String
            Get
                Return "Search Brand:"
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchPlaceholder As String
            Get
                Return "Nike"
            End Get
        End Property

        Protected Overrides Sub InitializeServices()
            If _service Is Nothing Then
                _service = New BrandService()
            End If
        End Sub

        Protected Overrides Sub HandlePrimaryAction()
            Dim entryForm As New FrmBrandEntry With {
                .Mode = EntryFormMode.AddNew,
                .SelectedId = -1
            }
            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            If DGVtable Is Nothing OrElse _service Is Nothing Then Exit Sub

            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) _service.GetBrandsPage(request))
            DGVtable.DataSource = page.Records
            GridHelpers.ApplyColumnSetup(DGVtable, ColId, Sub(col) col.Visible = False)

            EnsureActionColumns()
            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyDefaultGridLayout()
        End Sub

        Private Sub EnsureActionColumns()
            If Not DGVtable.Columns.Contains(ColViewEdit) Then
                Dim viewCol As New DataGridViewButtonColumn() With {
                    .Name = ColViewEdit,
                    .HeaderText = "Action",
                    .Text = "View/Edit",
                    .UseColumnTextForButtonValue = True,
                    .Width = 100,
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
                }
                DGVtable.Columns.Add(viewCol)
            End If

            If Not DGVtable.Columns.Contains(ColDelete) Then
                Dim deleteCol As New DataGridViewButtonColumn() With {
                    .Name = ColDelete,
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

        Private Sub OpenEditModalById(brandId As Integer)
            Dim entryForm As New FrmBrandEntry With {
                .Mode = EntryFormMode.EditExisting,
                .SelectedId = brandId
            }

            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Private Sub DeleteById(brandId As Integer)
            If Not _service.CanDelete(brandId) Then
                MessageBox.Show("Cannot delete. Brand is still used in Product.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If MessageBox.Show("Are you sure you want to delete this brand?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            _service.DeleteBrand(brandId)
            LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "Deleted Brand.")
            ReloadData()
        End Sub

        Private Function TryGetId(row As DataGridViewRow, ByRef brandId As Integer) As Boolean
            brandId = -1
            If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColId) Then Return False

            Dim raw As Object = row.Cells(ColId).Value
            If raw Is Nothing OrElse IsDBNull(raw) Then Return False

            Return Integer.TryParse(raw.ToString(), brandId)
        End Function

        Private Sub DGVtable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
            If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            If Not TryGetId(row, selectedId) Then
                selectedId = -1
            End If
        End Sub

        Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
            If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            Dim brandId As Integer
            If Not TryGetId(row, brandId) Then Exit Sub

            selectedId = brandId

            If colName = ColViewEdit Then
                OpenEditModalById(brandId)
            ElseIf colName = ColDelete Then
                DeleteById(brandId)
            End If
        End Sub
    End Class
End Namespace
