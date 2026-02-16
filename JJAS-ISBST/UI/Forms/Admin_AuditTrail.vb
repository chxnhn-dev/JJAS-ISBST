Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Admin_AuditTrail

    Dim formtoshow As Form
    Private Sub Admin_AuditTrail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displaydata("")
    End Sub

    Private Sub displaydata(Searchtext As String)
        Try
            Dim dt As New DataTable()
            Dim sql As String = "
            SELECT 
                AuditID,
                UserID,
                Name,
                Username,
                Role,
                Action,
                DatetimeStamp
            FROM tbl_AuditTrail
            WHERE (@search = '' OR Name LIKE @search OR Username LIKE @search OR Role LIKE @search OR datetimestamp LIKE @search)
            ORDER BY DateTimeStamp DESC;"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & Searchtext & "%")
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using


            DGVdeliveries.DataSource = dt

            Dim hiddenCols() As String = {"AuditID", "UserID"}
            For Each colName In hiddenCols
                If DGVdeliveries.Columns.Contains(colName) Then
                    DGVdeliveries.Columns(colName).Visible = False
                End If
            Next
            ' Column headers
            DGVdeliveries.Columns("Name").HeaderText = "Full Name"
            DGVdeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DGVdeliveries.ClearSelection()

        Catch ex As Exception
            MessageBox.Show("An error occurred while loading audti trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================== SEARCH BAR ==================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displaydata(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then
            lblPlaceholder.Visible = True
        End If
    End Sub
    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        ' Prevent scanner's Enter from triggering anything
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub
    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnpos_Click(sender As Object, e As EventArgs) Handles btnpos.Click
        formtoshow = New Admin_Pos()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        formtoshow = New Admin_User()
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

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        formtoshow = New Admin_Home()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub
End Class