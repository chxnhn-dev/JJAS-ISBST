Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports JJAS_ISBST.Login

Public Class Edit_User

    ' Properties from Admin_User
    Public Property UserID As Integer
    Public Property Role As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property ContactNumber As String
    Public Property Email As String
    Public Property Address As String
    Public Property Username As String

    ' 🔒 Hash password before saving (SHA-256)
    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLower()
        End Using
    End Function

    Private Sub Edit_User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 🟢 Setup role ComboBox with a "Select Option"
        cbrole.Items.Clear()
        cbrole.Items.Add("Select Option")
        cbrole.Items.Add("Staff")
        cbrole.Items.Add("Cashier")
        cbrole.SelectedIndex = 0

        ' 🔹 Load user info
        cbrole.Text = Role
        txtfirstname.Text = FirstName
        txtlastname.Text = LastName
        txtcontactnumber.Text = ContactNumber
        txtemail.Text = Email
        txtaddress.Text = Address
        txtusername.Text = Username

        ' 🔹 Leave password boxes empty
        txtpassword.Clear()
        txtConfirmPass.Clear()

        ' Block copy/paste for sensitive fields
        For Each tb In {txtfirstname, txtlastname, txtcontactnumber, txtemail, txtaddress, txtusername, txtpassword, txtConfirmPass}
            BlockCopyPaste(tb)
        Next
        ' 🟢 Display hashed password if available (coming from Admin_User.Tag)
        If Me.Tag IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.Tag.ToString()) Then
            txtpassword.Text = Me.Tag.ToString()
            txtConfirmPass.Text = Me.Tag.ToString()
            txtpassword.BackColor = SystemColors.Control
            txtConfirmPass.BackColor = SystemColors.Control
        End If

    End Sub

    ' ===============================
    ' 🔎 Helper Validations
    ' ===============================
    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function
    Private Function IsDuplicate(fieldName As String, value As String, currentColorId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_User WHERE " & fieldName & " = @Value AND UserID <> @UserID)
           SELECT 1 ELSE SELECT 0"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@UserID", currentColorId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub txtAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtaddress.KeyPress
        ' Allow letters, numbers, space, comma, period, and backspace
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso
       Not Char.IsControl(e.KeyChar) AndAlso
       Not e.KeyChar = " "c AndAlso
       Not e.KeyChar = ","c AndAlso
       Not e.KeyChar = "."c Then

            ' Block other special characters
            e.Handled = True

        End If
    End Sub
    Private Function IsDigitsOnly(str As String) As Boolean
        Return str.All(AddressOf Char.IsDigit)
    End Function
    Private Sub txtcontactnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontactnumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtusername_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtusername.KeyPress
        ' Allow only letters, digits, and control keys (like Backspace)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub OnlyLetters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtfirstname.KeyPress, txtlastname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        Try
            ' Trimmed inputs
            Dim firstname = txtfirstname.Text.Trim()
            Dim lastname = txtlastname.Text.Trim()
            Dim username = txtUsername.Text.Trim()
            Dim email = txtemail.Text.Trim()
            Dim contact = txtcontactnumber.Text.Trim()
            Dim address = txtaddress.Text.Trim()
            Dim role = cbrole.Text
            Dim newPassword As String = txtPassword.Text.Trim()
            Dim confirmPass As String = txtConfirmPass.Text.Trim()

            Dim updatePassword As Boolean = False

            ' 🟢 Compare with original hash (from Tag)
            Dim originalHash As String = ""
            If Me.Tag IsNot Nothing Then originalHash = Me.Tag.ToString()

            ' === 1. REQUIRED FIELD CHECK (exclude password unless changing) ===
            If {firstname, lastname, username, email, contact, address}.Any(Function(x) String.IsNullOrWhiteSpace(x)) _
            OrElse cbrole.SelectedIndex = 0 Then
                MessageBox.Show("Please fill in all fields and select a valid role.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not email.EndsWith("@gmail.com") AndAlso Not email.EndsWith("@outlook.com") Then
                MessageBox.Show("Please use a valid email provider like Gmail / Outlook.", "Invalid Email Domain", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If firstname.Length < 2 OrElse lastname.Length < 2 Then
                MessageBox.Show("First and last name must be at least 2 characters long.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If address.Length < 5 Then
                MessageBox.Show("Please enter a more detailed address.", "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not contact.StartsWith("09") Then
                MessageBox.Show("Contact number must start with '09'.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' === 2. CONTACT NUMBER CHECK ===
            If contact.Length <> 11 Then
                MessageBox.Show("Contact number must be exactly 11 digits!", "Invalid Length", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtcontactnumber.Clear()
                txtcontactnumber.Focus()
                Exit Sub
            End If

            ' === 3. DUPLICATION VALIDATIONS ===
            If IsDuplicate("Username", txtUsername.Text, UserID) Then
                MessageBox.Show("Username already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicate("Email", txtemail.Text, UserID) Then
                MessageBox.Show("Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicate("ContactNumber", txtcontactnumber.Text, UserID) Then
                MessageBox.Show("Contact number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' === 4. FORMAT VALIDATIONS ===
            If Not IsDigitsOnly(contact) Then
                MessageBox.Show("Contact number must contain only digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtcontactnumber.Focus()
                Exit Sub
            End If

            If Not IsValidEmail(email) Then
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtemail.Focus()
                Exit Sub
            End If

            ' === 5. PASSWORD VALIDATION ===
            ' Only update if changed (i.e. not the same as original hash)
            If Not String.IsNullOrEmpty(originalHash) Then
                If newPassword <> originalHash Then
                    updatePassword = True
                End If
            Else
                If newPassword <> "" Or confirmPass <> "" Then
                    updatePassword = True
                End If
            End If

            If updatePassword Then
                If newPassword = "" Or confirmPass = "" Then
                    MessageBox.Show("Please enter and confirm the new password.", "Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
                If newPassword <> confirmPass Then
                    MessageBox.Show("Passwords do not match.", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If MessageBox.Show("Are you sure you want to update this user?",
                       "Confirm Edit",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ' === 6. UPDATE USER INFO ===
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()

                    ' Update basic fields
                    Dim sql As String = "
                UPDATE tbl_User SET 
                    Role=@Role, 
                    FirstName=@FirstName, 
                    LastName=@LastName, 
                    ContactNumber=@ContactNumber, 
                    Email=@Email, 
                    Address=@Address, 
                    Username=@Username,
                    DateCreated=@DateCreated
                WHERE UserID=@UserID"

                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Role", cbrole.Text.Trim)
                        cmd.Parameters.AddWithValue("@FirstName", firstname)
                        cmd.Parameters.AddWithValue("@LastName", lastname)
                        cmd.Parameters.AddWithValue("@ContactNumber", contact)
                        cmd.Parameters.AddWithValue("@Email", email)
                        cmd.Parameters.AddWithValue("@Address", address)
                        cmd.Parameters.AddWithValue("@Username", username)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@UserID", UserID)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' 🔐 Update password only if it was changed
                    If updatePassword Then
                        Dim hashedPass = HashPassword(newPassword)
                        Dim passSql As String = "UPDATE tbl_User SET Password=@Password WHERE UserID=@UserID"
                        Using cmdPass As New SqlCommand(passSql, conn)
                            cmdPass.Parameters.AddWithValue("@Password", hashedPass)
                            cmdPass.Parameters.AddWithValue("@UserID", UserID)
                            cmdPass.ExecuteNonQuery()
                        End Using
                    End If
                End Using

                ' Log the edit
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited User.")
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while editing user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 🔒 Show / Hide Password
    Private Sub cbxShowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowpassword.CheckedChanged
        Dim visible As Boolean = cbxShowpassword.Checked
        txtpassword.PasswordChar = If(visible, "", "*"c)
        txtConfirmPass.PasswordChar = If(visible, "", "*"c)
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Panel2_Paint_1(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class
