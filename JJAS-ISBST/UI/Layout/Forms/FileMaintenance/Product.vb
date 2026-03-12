Imports System.Data.SqlClient

Namespace FileMaintenance
    Public Class Product
        Inherits FileMaintenanceBaseForm

        Private _service As ProductService

        Private Const ColViewEdit As String = "colViewEdit"
        Private Const ColDelete As String = "colDelete"
        Private Const ColId As String = "ProductID"

        Private selectedId As Integer = -1

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.ProductTab
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchCaption As String
            Get
                Return "Search Product:"
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchPlaceholder As String
            Get
                Return "Nike Air"
            End Get
        End Property

        Protected Overrides Sub InitializeServices()
            If _service Is Nothing Then
                _service = New ProductService()
            End If
        End Sub

        Protected Overrides Sub HandlePrimaryAction()
            Dim entryForm As New FrmProductEntry()
            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            If DGVtable Is Nothing OrElse _service Is Nothing Then Exit Sub

            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) _service.GetActiveProductsPage(request))
            Dim dt As DataTable = page.Records

            ImageLoader.AddAndLoadImages(dt, "ImagePath", "ProductImage")
            DGVtable.DataSource = dt

            Dim productImageColumn As DataGridViewColumn = Nothing
            If GridHelpers.TryGetColumn(DGVtable, productImageColumn, "ProductImage") Then
                productImageColumn.DisplayIndex = 0
                Dim imgCol As DataGridViewImageColumn = TryCast(productImageColumn, DataGridViewImageColumn)
                If imgCol IsNot Nothing Then
                    imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgCol.Width = 120
                    imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                End If
            End If

            Dim hiddenCols() As String = {"ImagePath", "ProductID", "ColorID", "CategoryID", "BrandID", "SizeID"}
            For Each colName In hiddenCols
                GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
            Next

            EnsureActionColumns()

            For Each priceColumnName As String In {"CostPrice", "SellingPrice"}
                GridHelpers.ApplyColumnSetup(DGVtable, priceColumnName, Sub(col)
                                                                            col.DefaultCellStyle.Format = "C2"
                                                                            col.DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
                                                                        End Sub)
            Next

            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            For Each col As DataGridViewColumn In DGVtable.Columns
                If col.Name = "ProductImage" OrElse col.Name = ColViewEdit OrElse col.Name = ColDelete Then
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                Else
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End If
            Next

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

        Private Function DeleteValidation(id As Integer) As Boolean
            Dim sql As String = "SELECT COUNT(*) FROM tbl_Delivery_Products WHERE ProductID = @ProductID"
            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@ProductID", id)
                    conn.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count = 0
                End Using
            End Using
        End Function

        Private Sub OpenEditModalById(productId As Integer)
            Dim entryForm As New FrmProductEntry With {
                .ProductID = productId
            }

            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Private Sub DeleteById(productId As Integer)
            If Not DeleteValidation(productId) Then
                MessageBox.Show("Cannot delete. Product is still used.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "DELETE tbl_Products WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "Deleted product.")
            ReloadData()
        End Sub

        Private Function TryGetId(row As DataGridViewRow, ByRef productId As Integer) As Boolean
            productId = -1
            If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColId) Then Return False

            Dim raw As Object = row.Cells(ColId).Value
            If raw Is Nothing OrElse IsDBNull(raw) Then Return False

            Return Integer.TryParse(raw.ToString(), productId)
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
            Dim productId As Integer
            If Not TryGetId(row, productId) Then Exit Sub

            selectedId = productId

            If colName = ColViewEdit Then
                OpenEditModalById(productId)
            ElseIf colName = ColDelete Then
                DeleteById(productId)
            End If
        End Sub
    End Class
End Namespace
