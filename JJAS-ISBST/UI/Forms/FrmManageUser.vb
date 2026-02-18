Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin

Public Class FrmManageUser
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"
    Private Const ColToggleStatus As String = "colToggleStatus"
    Private Const ColUserId As String = "UserID"
    Dim userid As Integer = -1
    Dim formtoshow As Form

    Private Sub Admin_User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")
    End Sub

    ' ================================
    ' DISPLAY USERS
    ' ================================
    Private Sub displayData(searchText As String)
        Try
            Dim dt As New DataTable()

            Dim sql As String = "
        SELECT 
    UserID,
    Role,
    FirstName,
    LastName,
    Username,
    ContactNumber,
    Address,
    Email,
    DateCreated AS DateUpdated,
    isActive
FROM tbl_User
WHERE
    (FirstName LIKE @search
    OR LastName LIKE @search
    OR Username LIKE @search
    OR CONCAT(FirstName, ' ', LastName) LIKE @search
    OR (@search = 'active' AND isActive = 1)
    OR (@search = 'inactive' AND isActive = 0))
ORDER BY 
    isActive ASC,       -- 🧠 Inactive (0) first, Active (1) next
    DateCreated DESC;   -- 🕒 Then sort newest first within each group"


            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    Dim searchValue As String = "%" & searchText & "%"
                    cmd.Parameters.AddWithValue("@search", searchValue)

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            DGVtable.DataSource = dt

            ' Hide columns
            Dim hiddenCols() As String = {"UserID", "isActive"}
            For Each colName In hiddenCols
                If DGVtable.Columns.Contains(colName) Then
                    DGVtable.Columns(colName).Visible = False
                End If
            Next

            EnsureActionColumns()
            UpdateToggleButtonText()

            ' ✅ Highlight Active/Inactive rows
            For Each rowa As DataGridViewRow In DGVtable.Rows
                If Not rowa.IsNewRow Then
                    Dim isActiveVal As Boolean = GetIsActiveFromValue(rowa.Cells("isActive").Value)

                    If isActiveVal Then
                        rowa.DefaultCellStyle.BackColor = Color.Honeydew     ' Active = Light green
                    Else
                        rowa.DefaultCellStyle.BackColor = Color.LightSalmon   ' Inactive = Salmon
                    End If
                End If
            Next

            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyStandardGridLayout(DGVtable)
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading users: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

        For Each rowa As DataGridViewRow In DGVtable.Rows
            If rowa.IsNewRow Then Continue For

            Dim isActiveVal As Boolean = GetIsActiveFromValue(rowa.Cells("isActive").Value)
            rowa.Cells(ColToggleStatus).Value = If(isActiveVal, "Deactivate", "Activate")
        Next
    End Sub

    Private Sub ToggleUserActiveStatus(userIdValue As Integer, isCurrentlyActive As Boolean)
        If isCurrentlyActive AndAlso IsUserLoggedIn(userIdValue) Then
            MessageBox.Show("This user is currently logged in. You cannot deactivate this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim nextIsActive As Integer = If(isCurrentlyActive, 0, 1)
        Dim actionText As String = If(isCurrentlyActive, "deactivate", "activate")

        If MessageBox.Show("Are you sure you want to " & actionText & " this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "UPDATE tbl_User SET IsActive = @IsActive WHERE UserID = @UserID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@IsActive", nextIsActive)
                    cmd.Parameters.AddWithValue("@UserID", userIdValue)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected = 0 Then
                        MessageBox.Show("User record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(isCurrentlyActive, "Deactivated a user.", "Activated a user."))
            MessageBox.Show("User " & If(isCurrentlyActive, "deactivated", "activated") & " successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            displayData(txtSearch.Text.Trim())
        Catch ex As Exception
            MessageBox.Show("An error occurred while updating user status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================
    ' ADD USER
    ' ================================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim f As New FrmUserEntry()
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub

    ' ================================
    ' SELECT USER
    ' ================================

    ' ================================
    ' EDIT USER
    ' ================================
    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If userid = -1 OrElse DGVtable.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtable.SelectedRows(0)
        Dim selectedUserId As Integer
        If Not TryGetUserId(row, selectedUserId) Then
            MessageBox.Show("Invalid User ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        OpenEditModalById(selectedUserId)
    End Sub
    Private Function IsUserLoggedIn(userId As Integer) As Boolean
        ' Uses dbo.tbl_AppSession (see /DB/Create_Session_Table.sql)
        Return SessionService.IsUserLoggedIn(userId)
    End Function


    ' ================================
    ' SEARCH BAR
    ' ================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        lblPlaceholder.Visible = (txtSearch.Text.Trim() = "")
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    ' ================================
    ' DELETE USER
    ' ================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If DGVtable.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = DGVtable.SelectedRows(0)
        Dim selectedUserID As Integer
        If Not TryGetUserId(selectedRow, selectedUserID) Then
            MessageBox.Show("Invalid User ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        DeleteUserById(selectedUserID)
    End Sub

    Private Sub OpenEditModalById(userIdValue As Integer)
        If IsUserLoggedIn(userIdValue) Then
            MessageBox.Show("This user is currently logged in. You cannot Edit this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim f As New FrmUserEntry With {
            .UserID = userIdValue
        }

        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub

    Private Sub DeleteUserById(selectedUserID As Integer)
        If IsUserLoggedIn(selectedUserID) Then
            MessageBox.Show("This user is currently logged in. You cannot Delete this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try

            If MessageBox.Show("Are you sure you want to permanently delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Dim sql As String = "DELETE FROM tbl_User WHERE UserID = @UserID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted a user.")
                MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                displayData("")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while deleting user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DGVuser_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        If e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
        If colName <> ColViewEdit AndAlso colName <> ColToggleStatus AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim selectedUserId As Integer
        If Not TryGetUserId(row, selectedUserId) Then Exit Sub
        userid = selectedUserId

        If colName = ColViewEdit Then
            OpenEditModalById(selectedUserId)
        ElseIf colName = ColToggleStatus Then
            Dim isCurrentlyActive As Boolean = GetIsActiveFromValue(row.Cells("isActive").Value)
            ToggleUserActiveStatus(selectedUserId, isCurrentlyActive)
        ElseIf colName = ColDelete Then
            DeleteUserById(selectedUserId)
        End If
    End Sub
    ' ================================
    ' NAVIGATION BUTTONS
    ' ================================
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub

    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnBrand_Click(sender As Object, e As EventArgs) Handles btnBrand.Click
        formtoshow = New FrmManageBrand()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        formtoshow = New FrmManageColor()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New FrmManageCategory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        formtoshow = New FrmManageProduct()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnSupplier_Click(sender As Object, e As EventArgs) Handles btnSupplier.Click
        formtoshow = New FrmManageSupplier()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        formtoshow = New FrmManageDiscount()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
        formtoshow = New FrmVAT()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New FrmManageDeliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New FrmInventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        formtoshow = New FrmPOS()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                "Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' 🧾 Log audit trail

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

            ' Clear current user info
            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show FrmLogin form again
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        formtoshow = New FrmDashboard()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New FrmTransactions()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New FrmAuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs) Handles btnMeasurement.Click
        formtoshow = New FrmManageSize()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub DGVuser_CellClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit OrElse colName = ColToggleStatus OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim selectedUserId As Integer
        If TryGetUserId(row, selectedUserId) Then
            userid = selectedUserId
        Else
            userid = -1
        End If
    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs)
        displayData("")
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs)
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MsgBox("Error logging out: " & ex.Message)
        End Try


        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Function TryGetUserId(row As DataGridViewRow, ByRef userIdValue As Integer) As Boolean
        userIdValue = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColUserId) Then Return False

        Dim raw As Object = row.Cells(ColUserId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), userIdValue)
    End Function
End Class


