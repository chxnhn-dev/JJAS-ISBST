Namespace FileMaintenance
    Public Class User
        Inherits FileMaintenanceBaseForm

        Private _service As UserService

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

        Protected Overrides Sub InitializeServices()
            If _service Is Nothing Then
                _service = New UserService()
            End If
        End Sub

        Protected Overrides Sub HandlePrimaryAction()
            Dim entryForm As New FrmUserEntry()
            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            If DGVtable Is Nothing OrElse _service Is Nothing Then Exit Sub

            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) _service.GetUsersPage(request))
            Dim usersTable As DataTable = page.Records
            DGVtable.DataSource = usersTable
            If usersTable Is Nothing Then Exit Sub

            GridHelpers.ApplyColumnSetup(DGVtable, "UserID", Sub(col) col.Visible = False)
            GridHelpers.ApplyColumnSetup(DGVtable, "isActive", Sub(col) col.Visible = False, "IsActive")

            EnsureActionColumns()
            UpdateToggleButtonText()
            ApplyStatusRowColors()

            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyDefaultGridLayout()
            UpdateToggleButtonText()
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

            If Not DGVtable.Columns.Contains(ColToggleStatus) Then
                Dim toggleCol As New DataGridViewButtonColumn() With {
                    .Name = ColToggleStatus,
                    .HeaderText = "Status",
                    .UseColumnTextForButtonValue = False,
                    .Width = 100,
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
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
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
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
            Dim isActiveColumn As DataGridViewColumn = Nothing
            If Not GridHelpers.TryGetColumn(DGVtable, isActiveColumn, "isActive", "IsActive") Then Exit Sub

            For Each row As DataGridViewRow In DGVtable.Rows
                If row.IsNewRow Then Continue For

                Dim isActiveVal As Boolean = GetIsActiveFromValue(row.Cells(isActiveColumn.Name).Value)
                row.Cells(ColToggleStatus).Value = If(isActiveVal, "Deactivate", "Activate")
            Next
        End Sub

        Private Sub ApplyStatusRowColors()
            Dim isActiveColumn As DataGridViewColumn = Nothing
            If Not GridHelpers.TryGetColumn(DGVtable, isActiveColumn, "isActive", "IsActive") Then Exit Sub

            For Each row As DataGridViewRow In DGVtable.Rows
                If row.IsNewRow Then Continue For

                Dim isActiveVal As Boolean = GetIsActiveFromValue(row.Cells(isActiveColumn.Name).Value)
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
            If _service Is Nothing Then Exit Sub

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
            If userIdValue <= 0 Then
                MessageBox.Show("Unable to open the selected user record.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsUserLoggedIn(userIdValue) Then
                MessageBox.Show("This user is currently logged in. You cannot edit this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim entryForm As New FrmUserEntry With {
                .UserID = userIdValue
            }

            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                ReloadData()
            End If
        End Sub

        Private Sub DeleteById(userIdValue As Integer)
            If _service Is Nothing Then Exit Sub

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

            Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
            If colName = ColViewEdit OrElse colName = ColToggleStatus OrElse colName = ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            If Not TryGetId(row, selectedId) Then
                selectedId = -1
            End If
        End Sub

        Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
            If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

            Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
            If colName <> ColViewEdit AndAlso colName <> ColToggleStatus AndAlso colName <> ColDelete Then Exit Sub

            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            If row Is Nothing Then Exit Sub
            Dim userIdValue As Integer
            If Not TryGetId(row, userIdValue) Then Exit Sub

            selectedId = userIdValue

            If colName = ColViewEdit Then
                OpenEditModalById(userIdValue)
            ElseIf colName = ColToggleStatus Then
                Dim isActiveColumn As DataGridViewColumn = Nothing
                If Not GridHelpers.TryGetColumn(DGVtable, isActiveColumn, "isActive", "IsActive") Then Exit Sub
                Dim isActive As Boolean = GetIsActiveFromValue(row.Cells(isActiveColumn.Name).Value)
                ToggleUserActiveStatus(userIdValue, isActive)
            ElseIf colName = ColDelete Then
                DeleteById(userIdValue)
            End If
        End Sub
    End Class
End Namespace
