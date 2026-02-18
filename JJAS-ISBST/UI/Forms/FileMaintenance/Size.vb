Imports System.Data.SqlClient

Namespace FileMaintenance
    Public Class Size
        Inherits FileMaintenanceBaseForm

        Private Const ColViewEdit As String = "colViewEdit"
        Private Const ColDelete As String = "colDelete"
        Private Const ColId As String = "SizeID"

        Private selectedId As Integer = -1

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.SizeTab
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchCaption As String
            Get
                Return "Search Size:"
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchPlaceholder As String
            Get
                Return "Small"
            End Get
        End Property

        Protected Overrides Sub HandlePrimaryAction()
            Dim entryForm As New FrmSizeEntry()
            If entryForm.ShowDialog() = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            Dim dt As New DataTable()
            Dim sql As String = "
                SELECT SizeID,
                       Size,
                       Description
                FROM tbl_Size
                WHERE IsActive = 1
                  AND (@search = '' OR Size LIKE @search)
                ORDER BY Size"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            DGVtable.DataSource = dt
            If DGVtable.Columns.Contains(ColId) Then
                DGVtable.Columns(ColId).Visible = False
            End If

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
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
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
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
                }
                DGVtable.Columns.Add(deleteCol)
            End If
        End Sub

        Private Function DeleteValidation(id As Integer) As Boolean
            Dim sql As String = "SELECT COUNT(*) FROM tbl_Products WHERE SizeID = @id"
            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    conn.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count = 0
                End Using
            End Using
        End Function

        Private Sub OpenEditModalById(sizeId As Integer)
            Dim entryForm As New FrmSizeEntry With {
                .SizeID = sizeId
            }

            If entryForm.ShowDialog() = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Private Sub DeleteById(sizeId As Integer)
            If Not DeleteValidation(sizeId) Then
                MessageBox.Show("Cannot delete. Size is still used in Product.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If MessageBox.Show("Are you sure you want to delete this size?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "DELETE tbl_Size WHERE SizeID = @SizeID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@SizeID", sizeId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "Deleted Size.")
            ReloadData()
        End Sub

        Private Function TryGetId(row As DataGridViewRow, ByRef sizeId As Integer) As Boolean
            sizeId = -1
            If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColId) Then Return False

            Dim raw As Object = row.Cells(ColId).Value
            If raw Is Nothing OrElse IsDBNull(raw) Then Return False

            Return Integer.TryParse(raw.ToString(), sizeId)
        End Function

        Private Sub DGVtable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
            If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            If Not TryGetId(row, selectedId) Then
                selectedId = -1
            End If
        End Sub

        Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
            If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            Dim sizeId As Integer
            If Not TryGetId(row, sizeId) Then Exit Sub

            selectedId = sizeId

            If colName = ColViewEdit Then
                OpenEditModalById(sizeId)
            ElseIf colName = ColDelete Then
                DeleteById(sizeId)
            End If
        End Sub
    End Class
End Namespace
