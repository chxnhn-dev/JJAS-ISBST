Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Admin_User
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

        DGVuser.DataSource = dt

        ' Hide columns
        Dim hiddenCols() As String = {"UserID", "isActive"}
        For Each colName In hiddenCols
            If DGVuser.Columns.Contains(colName) Then
                DGVuser.Columns(colName).Visible = False
            End If
        Next

        ' ✅ Highlight Active/Inactive rows
        For Each rowa As DataGridViewRow In DGVuser.Rows
            If Not rowa.IsNewRow Then
                Dim isActiveVal As Boolean = True
                Boolean.TryParse(rowa.Cells("isActive").Value.ToString(), isActiveVal)

                If isActiveVal Then
                    rowa.DefaultCellStyle.BackColor = Color.Honeydew     ' Active = Light green
                Else
                    rowa.DefaultCellStyle.BackColor = Color.LightSalmon   ' Inactive = Salmon
                End If
            End If
        Next

        DGVuser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DGVuser.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading users: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================
    ' ADD USER
    ' ================================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim f As New Add_User
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
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If userid = -1 OrElse DGVuser.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If


        If IsUserLoggedIn(userid) Then
            MessageBox.Show("This user is currently logged in. You cannot Edit this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVuser.SelectedRows(0)

        ' Create Edit_User form and fill the basic info
        Dim f As New Edit_User With {
        .UserID = Convert.ToInt32(row.Cells("UserID").Value),
        .Role = row.Cells("Role").Value.ToString(),
        .FirstName = row.Cells("FirstName").Value.ToString(),
        .LastName = row.Cells("LastName").Value.ToString(),
        .ContactNumber = row.Cells("ContactNumber").Value.ToString(),
        .Email = row.Cells("Email").Value.ToString(),
        .Address = row.Cells("Address").Value.ToString(),
        .Username = row.Cells("Username").Value.ToString()
    }

        ' 🟢 Fetch and attach the user's hashed password before showing the Edit_User form
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT Password FROM tbl_User WHERE UserID = @UserID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@UserID", f.UserID)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        f.Tag = result.ToString() ' store the hash temporarily in the Tag property
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading user hash: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Show the Edit_User form
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub
    Private Function IsUserLoggedIn(userId As Integer) As Boolean
        Dim isLoggedIn As Boolean = False

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using cmd As New SqlCommand("SELECT IsLoggedIn FROM tbl_User WHERE UserID=@UserID", conn)
                    cmd.Parameters.AddWithValue("@UserID", userId)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                        isLoggedIn = Convert.ToBoolean(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking login status: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return isLoggedIn
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
    ' DEACTIVATE USER
    ' ================================
    Private Sub btnDeactivate_Click(sender As Object, e As EventArgs) Handles btnDeactivate.Click
        If DGVuser.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = DGVuser.SelectedRows(0)
        Dim isActiveValue As Boolean = True
        Boolean.TryParse(selectedRow.Cells("isActive").Value.ToString(), isActiveValue)

        If Not isActiveValue Then
            MessageBox.Show("User is already deactivated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If


        Dim selectedUserID As Integer = Convert.ToInt32(selectedRow.Cells("UserID").Value)

        If IsUserLoggedIn(selectedUserID) Then
            MessageBox.Show("This user is currently logged in. You cannot deactivate this account.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to deactivate this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "UPDATE tbl_User SET IsActive = 0 WHERE UserID = @UserID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deactivated a user.")
            MessageBox.Show("User deactivated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            displayData("")
        End If
    End Sub

    ' ================================
    ' ACTIVATE USER
    ' ================================
    Private Sub btnActivate_Click(sender As Object, e As EventArgs) Handles btnActivate.Click
        If DGVuser.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to activate.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Get the selected row FIRST
        Dim selectedRow As DataGridViewRow = DGVuser.SelectedRows(0)

        ' Safely handle Boolean or DBNull values
        Dim isActiveValue As Boolean = False
        If Not IsDBNull(selectedRow.Cells("isActive").Value) Then
            Boolean.TryParse(selectedRow.Cells("isActive").Value.ToString(), isActiveValue)
        End If

        ' Check if user is already active
        If isActiveValue Then
            MessageBox.Show("User is already activated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Continue with activation
        Dim selectedUserID As Integer = Convert.ToInt32(selectedRow.Cells("UserID").Value)

        If MessageBox.Show("Are you sure you want to activate this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "UPDATE tbl_User SET IsActive = 1 WHERE UserID = @UserID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Activated a user.")
            MessageBox.Show("User activated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            displayData("")
        End If
    End Sub


    ' ================================
    ' DELETE USER
    ' ================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVuser.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = DGVuser.SelectedRows(0)
        Dim selectedUserID As Integer = Convert.ToInt32(selectedRow.Cells("UserID").Value)


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
        formtoshow = New Admin_Brand()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        formtoshow = New Admin_Color()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New admin_category()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        formtoshow = New Admin_Product()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnSupplier_Click(sender As Object, e As EventArgs) Handles btnSupplier.Click
        formtoshow = New Admin_Supplier()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        formtoshow = New Admin_Discount()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
        formtoshow = New Admin_Vat()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        formtoshow = New Admin_Pos()
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
            Login.CurrentUser.UserID = 0
            Login.CurrentUser.Username = ""
            Login.CurrentUser.Role = ""
            Login.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show Login form again
            Dim f As New Login()
            f.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        formtoshow = New Admin_Home()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New Admin_AuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs) Handles btnMeasurement.Click
        formtoshow = New Admin_Size()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub DGVuser_CellClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DGVuser.CellClick
        If e.RowIndex >= 0 Then
            userid = Convert.ToInt32(DGVuser.Rows(e.RowIndex).Cells("UserID").Value)
        End If
    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        displayData("")
    End Sub
End Class
