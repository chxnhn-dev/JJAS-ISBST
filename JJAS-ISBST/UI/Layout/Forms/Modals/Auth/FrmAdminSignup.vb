Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class FrmAdminSignup
    ' Expected controls:
    ' txtFullName, txtUsername, txtPassword, txtConfirm (TextBoxes)
    ' btnCreate (Button)
    ' lblStatus (Label)  ← optional, to show validation messages

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim fullname = txtFullName.Text.Trim()
        Dim username = txtUsername.Text.Trim()
        Dim pw = txtPassword.Text
        Dim confirm = txtConfirm.Text

        ' Disable button to prevent double click
        btnCreate.Enabled = False

        ' ✅ BASIC VALIDATIONS
        If String.IsNullOrEmpty(fullname) OrElse String.IsNullOrEmpty(username) OrElse
           String.IsNullOrEmpty(pw) OrElse String.IsNullOrEmpty(confirm) Then
            MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnCreate.Enabled = True
            Return
        End If

        ' ✅ Full name validation: letters and spaces only
        If Not Regex.IsMatch(fullname, "^[a-zA-Z\s]+$") Then
            MessageBox.Show("Full name can only contain letters and spaces.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnCreate.Enabled = True
            Return
        End If

        ' ✅ Username validation: only allow letters, numbers, underscore
        If Not Regex.IsMatch(username, "^[a-zA-Z0-9_]+$") Then
            MessageBox.Show("Username can only contain letters, numbers, and underscores.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnCreate.Enabled = True
            Return
        End If

        ' ✅ Password confirmation
        If pw <> confirm Then
            MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnCreate.Enabled = True
            Return
        End If

        ' ✅ Password strength check
        Dim strongPass As Boolean =
            pw.Length >= 8 AndAlso
            Regex.IsMatch(pw, "[A-Z]") AndAlso
            Regex.IsMatch(pw, "[a-z]") AndAlso
            Regex.IsMatch(pw, "[0-9]")

        If Not strongPass Then
            MessageBox.Show("Password must have at least 8 characters, one uppercase letter, one lowercase letter, and one digit.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnCreate.Enabled = True
            Return
        End If

        Try
            Using conn = DataAccess.GetConnection()
                conn.Open()

                ' ✅ Ensure only one admin can exist
                Using checkCmd As New SqlCommand("SELECT COUNT(1) FROM Admins", conn)
                    Dim existing = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If existing > 0 Then
                        MessageBox.Show("An admin already exists. Please use the login screen.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                        Return
                    End If
                End Using

                ' ✅ Ensure username not already taken (safety check)
                Using userCheck As New SqlCommand("SELECT COUNT(1) FROM Admins WHERE Username = @Username", conn)
                    userCheck.Parameters.Add(New SqlParameter("@Username", SqlDbType.NVarChar, 50) With {.Value = username})
                    Dim ucount = Convert.ToInt32(userCheck.ExecuteScalar())
                    If ucount > 0 Then
                        MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        btnCreate.Enabled = True
                        Return
                    End If
                End Using

                ' ✅ Hash password securely (PBKDF2)
                Dim created = AuthUtils.CreatePasswordHash(pw)
                Dim hash = created.hash
                Dim salt = created.salt
                Dim iterations = created.iterationsUsed

                ' ✅ Updated SQL with FullName included
                Dim insertSql As String =
                    "INSERT INTO Admins (FullName, Username, PasswordHash, PasswordSalt, Iterations, FailedAttempts, LockoutEnd, CreatedAt) " &
                    "VALUES (@FullName, @Username, @PasswordHash, @PasswordSalt, @Iterations, 0, NULL, @CreatedAt)"

                Using ins As New SqlCommand(insertSql, conn)
                    ins.Parameters.Add(New SqlParameter("@FullName", SqlDbType.NVarChar, 50) With {.Value = fullname})
                    ins.Parameters.Add(New SqlParameter("@Username", SqlDbType.NVarChar, 50) With {.Value = username})
                    ins.Parameters.Add(New SqlParameter("@PasswordHash", SqlDbType.VarBinary, 32) With {.Value = hash})
                    ins.Parameters.Add(New SqlParameter("@PasswordSalt", SqlDbType.VarBinary, 32) With {.Value = salt})
                    ins.Parameters.Add(New SqlParameter("@Iterations", SqlDbType.Int) With {.Value = iterations})
                    ins.Parameters.Add(New SqlParameter("@CreatedAt", SqlDbType.DateTime2) With {.Value = DateTime.UtcNow})

                    ins.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Admin created successfully! Please log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As SqlException When ex.Number = 2627 ' Unique constraint violation
            MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            btnCreate.Enabled = True
        End Try
    End Sub
    Private Sub cbxShowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowpassword.CheckedChanged
        Dim visible As Boolean = cbxShowpassword.Checked
        txtPassword.PasswordChar = If(visible, "", "*"c)
        txtConfirm.PasswordChar = If(visible, "", "*"c)
    End Sub


    Private Sub AdminSignupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtfullname.Select() ' Focus Full Name on load
    End Sub
End Class
