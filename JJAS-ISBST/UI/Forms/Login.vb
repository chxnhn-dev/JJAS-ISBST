Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class Login

    ' ✅ Shared class to store current logged-in user
    Public Class CurrentUser
        Public Shared UserID As Integer
        Public Shared Username As String
        Public Shared Role As String
        Public Shared FullName As String
    End Class

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Try
            Using conn = DataAccess.GetConnection()
                Using cmd As New SqlCommand("SELECT COUNT(1) FROM Admins", conn)
                    conn.Open()
                    Dim count = Convert.ToInt32(cmd.ExecuteScalar())
                    If count = 0 Then
                        Using signup As New AdminSignupForm()
                            Dim res = signup.ShowDialog(Me)
                            If res <> DialogResult.OK Then
                                ' Admin required — exit or close the login form
                                MessageBox.Show("An admin account is required. The application will now exit.", "No Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Application.Exit()
                                Return
                            End If
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Unable to check admin accounts: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try
    End Sub

    Private Sub cbxShowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowpassword.CheckedChanged
        txtPassword.PasswordChar = If(cbxShowpassword.Checked, ChrW(0), "*"c)
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin.PerformClick()
        End If
    End Sub

    ' 🔐 Hash password using SHA-256
    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLower()
        End Using
    End Function

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
    Dim username As String = txtUsername.Text.Trim()
    Dim password As String = txtPassword.Text.Trim()

    Dim auth As New AuthService()
    Dim res As AuthService.LoginResult = auth.Login(username, password)

    If Not res.Success Then
        MsgBox(res.ErrorMessage, MsgBoxStyle.Exclamation)
        Exit Sub
    End If

    ' ✅ Store current user info
    CurrentUser.UserID = res.UserId
    CurrentUser.Username = res.Username
    CurrentUser.FullName = res.FullName
    CurrentUser.Role = res.Role.ToLower()

    ' ✅ Create a DB-backed session (tbl_AppSession). IsActive remains account status.
    Dim principalType As String = If(CurrentUser.Role = "admin", "admin", "user")
    SessionService.CreateSession(principalType, CurrentUser.UserID, CurrentUser.Username, CurrentUser.FullName, CurrentUser.Role)


    ' ✅ audit trail
    Try
        LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User logged in.")
    Catch
        ' ignore logging errors
    End Try

    If CurrentUser.Role = "admin" Then
        MsgBox($"Welcome Admin {CurrentUser.FullName}!", MsgBoxStyle.Information)
    Else
        MsgBox($"Login Successful! Welcome {CurrentUser.FullName}", MsgBoxStyle.Information)
    End If

    ' ✅ Navigate
    Select Case CurrentUser.Role
        Case "admin", "cashier", "staff"
            Dim f As New Admin_Home()
            f.Show()
        Case Else
            MsgBox("Unknown role. Access denied.", MsgBoxStyle.Critical)
            Exit Sub
    End Select

    Me.Hide()
End Sub



    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
