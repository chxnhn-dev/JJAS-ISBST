Namespace FileMaintenance
    Public Class User
        Inherits FileMaintenanceBaseForm

        Private ReadOnly _service As New UserService()

        Private Const ColViewEdit As String = "colViewEdit"
        Private Const ColDelete As String = "colDelete"
        Private Const ColToggleStatus As String = "colToggleStatus"
        Private Const ColId As String = "UserID"

        Private selectedId As Integer = -1

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.UserTab
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchCaption As String
            Get
                Return "Search User:"
            End Get
        End Property

        Protected Overrides ReadOnly Property SearchPlaceholder As String
            Get
                Return "Juan Dela Cruz"
            End Get
        End Property

        Protected Overrides Sub HandlePrimaryAction()
            Dim entryForm As New FrmUserEntry()
            If entryForm.ShowDialog() = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            DGVtable.DataSource = _service.GetUsers(searchText)

            Dim hiddenCols() As String = {"UserID", "isActive"}
            For Each colName In hiddenCols
                If DGVtable.Columns.Contains(colName) Then
                    DGVtable.Columns(colName).Visible = False
                End If
            Next

            EnsureActionColumns()
            UpdateToggleButtonText()
            ApplyStatusRowColors()

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

            If Not DGVtable.Columns.Contains(ColToggleStatus) Then
                Dim toggleCol As New DataGridViewButtonColumn() With {
                    .Name = ColToggleStatus,
                    .HeaderText = "Status",
                    .UseColumnTextForButtonValue = False,
                    .Width = 100,
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
                }
                DGVtable.Columns.Add(toggleCol)
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

        Private Function GetIsActiveFromValue(rawValue As Object) As Boolean
            If rawValue Is Nothing OrElse IsDBNull(rawValue) Then Return False
            If TypeOf rawValue Is Boolean Then Return CBool(rawValue)

            Dim activeInt As Integer
            If Integer.TryParse(rawValue.ToString(), activeInt) Then
                Return activeInt = 1
            End If

            Dim activeBool As Boolean
            If Boolean.TryParse(rawValue.ToString(), activeBool) Then
                Return activeBool
            End If

            Return False
        End Function

        Private Sub UpdateToggleButtonText()
            If Not DGVtable.Columns.Contains(ColToggleStatus) Then Exit Sub

            For Each row As DataGridViewRow In DGVtable.Rows
                If row.IsNewRow Then Continue For

                Dim isActiveVal As Boolean = GetIsActiveFromValue(row.Cells("isActive").Value)
                row.Cells(ColToggleStatus).Value = If(isActiveVal, "Deactivate", "Activate")
            Next
        End Sub

        Private Sub ApplyStatusRowColors()
            For Each row As DataGridViewRow In DGVtable.Rows
                If row.IsNewRow Then Continue For

                Dim isActiveVal As Boolean = GetIsActiveFromValue(row.Cells("isActive").Value)
                If isActiveVal Then
                    row.DefaultCellStyle.BackColor = Drawing.Color.Honeydew
                Else
                    row.DefaultCellStyle.BackColor = Drawing.Color.LightSalmon
                End If
            Next
        End Sub

        Private Function IsUserLoggedIn(userId As Integer) As Boolean
            Return SessionService.IsUserLoggedIn(userId)
        End Function

        Private Sub ToggleUserActiveStatus(userIdValue As Integer, isCurrentlyActive As Boolean)
            If isCurrentlyActive AndAlso IsUserLoggedIn(userIdValue) Then
                MessageBox.Show("This user is currently logged in. You cannot deactivate this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim nextIsActive As Boolean = Not isCurrentlyActive
            Dim actionText As String = If(isCurrentlyActive, "deactivate", "activate")

            If MessageBox.Show("Are you sure you want to " & actionText & " this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Exit Sub
            End If

            _service.SetUserActiveStatus(userIdValue, nextIsActive)
            LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, If(isCurrentlyActive, "Deactivated a user.", "Activated a user."))
            ReloadData()
        End Sub

        Private Sub OpenEditModalById(userIdValue As Integer)
            If IsUserLoggedIn(userIdValue) Then
                MessageBox.Show("This user is currently logged in. You cannot edit this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim entryForm As New FrmUserEntry With {
                .Mode = EntryFormMode.EditExisting,
                .SelectedId = userIdValue
            }

            If entryForm.ShowDialog() = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Private Sub DeleteById(userIdValue As Integer)
            If IsUserLoggedIn(userIdValue) Then
                MessageBox.Show("This user is currently logged in. You cannot delete this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If MessageBox.Show("Are you sure you want to permanently delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
                Exit Sub
            End If

            _service.DeleteUser(userIdValue)
            LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "Deleted a user.")
            ReloadData()
        End Sub

        Private Function TryGetId(row As DataGridViewRow, ByRef userIdValue As Integer) As Boolean
            userIdValue = -1
            If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColId) Then Return False

            Dim raw As Object = row.Cells(ColId).Value
            If raw Is Nothing OrElse IsDBNull(raw) Then Return False

            Return Integer.TryParse(raw.ToString(), userIdValue)
        End Function

        Private Sub DGVtable_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
            If colName = ColViewEdit OrElse colName = ColToggleStatus OrElse colName = ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            If Not TryGetId(row, selectedId) Then
                selectedId = -1
            End If
        End Sub

        Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
            If colName <> ColViewEdit AndAlso colName <> ColToggleStatus AndAlso colName <> ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            Dim userIdValue As Integer
            If Not TryGetId(row, userIdValue) Then Exit Sub

            selectedId = userIdValue

            If colName = ColViewEdit Then
                OpenEditModalById(userIdValue)
            ElseIf colName = ColToggleStatus Then
                Dim isActive As Boolean = GetIsActiveFromValue(row.Cells("isActive").Value)
                ToggleUserActiveStatus(userIdValue, isActive)
            ElseIf colName = ColDelete Then
                DeleteById(userIdValue)
            End If
        End Sub
    End Class
End Namespace
