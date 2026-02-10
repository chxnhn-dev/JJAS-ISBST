Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net.Mail
Imports JJAS_ISBST.Login

Public Class Add_User

    ' 🔒 Hash password before saving (SHA-256)
    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLower()
        End Using
    End Function

    Private Sub Add_User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable copy-paste for security
        For Each tb In {txtfirstname, txtlastname, txtcontactnumber, txtemail, txtaddress, txtusername, txtpassword, txtConfirmPass}
            BlockCopyPaste(tb)
        Next

        ' 🟢 Setup role ComboBox with a Select Option
        cbrole.Items.Clear()
        cbrole.Items.Add("Select Role")
        cbrole.Items.Add("Staff")
        cbrole.Items.Add("Cashier")
        cbrole.SelectedIndex = 0
    End Sub

    ' 🧩 Check if value already exists in DB
    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim exists As Boolean = False
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_User WHERE {fieldName} = @Value AND IsActive = 1"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", fieldValue)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                exists = (count > 0)
            End Using
        End Using

        Return exists
    End Function

    ' 🧩 Email validation
    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    ' 🧩 Digits-only helper
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

    ' 🧠 ADD USER BUTTON
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Try
            ' Trimmed inputs
            Dim firstname = txtfirstname.Text.Trim()
            Dim lastname = txtlastname.Text.Trim()
            Dim username = txtusername.Text.Trim()
            Dim password = txtpassword.Text.Trim()
            Dim confirmPass = txtConfirmPass.Text.Trim()
            Dim email = txtemail.Text.Trim()
            Dim contact = txtcontactnumber.Text.Trim()
            Dim address = txtaddress.Text.Trim()
            Dim role = cbrole.Text

            ' === 1. REQUIRED FIELD CHECK ===
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

            If {firstname, lastname, username, password, confirmPass, email, contact, address}.Any(Function(x) String.IsNullOrWhiteSpace(x)) _
                OrElse cbrole.SelectedIndex = 0 Then
                MessageBox.Show("Please fill in all fields and select a valid role.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' === 2. CONTACT NUMBER CHECK ===
            If contact.Length <> 11 Then
                MessageBox.Show("Contact number must be exactly 11 digits!", "Invalid Length", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtcontactnumber.Clear()
                txtcontactnumber.Focus()
                Exit Sub
            End If

            ' === 3. FORMAT VALIDATIONS ===
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

            If password <> confirmPass Then
                MessageBox.Show("Passwords do not match.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtConfirmPass.Clear()
                txtConfirmPass.Focus()
                Exit Sub
            End If

            ' === 4. DUPLICATE VALIDATIONS ===
            If IsDuplicate("Username", username) Then
                MessageBox.Show("Username already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicate("Email", email) Then
                MessageBox.Show("Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicate("ContactNumber", contact) Then
                MessageBox.Show("Contact Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' === 5. INSERT TO DATABASE ===
            Dim hashedPass = HashPassword(password)

            If MessageBox.Show("Are you sure you want to add this user?",
                       "Confirm Edit",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Using conn As SqlConnection = DataAccess.GetConnection()
                    Using cmd As New SqlCommand("
                    INSERT INTO tbl_User 
                    (Role, FirstName, LastName, ContactNumber, Email, Address, Username, Password, DateCreated, IsActive)
                    VALUES 
                    (@Role, @FirstName, @LastName, @ContactNumber, @Email, @Address, @Username, @Password, @DateCreated, @IsActive)", conn)

                        With cmd.Parameters
                            .AddWithValue("@Role", role)
                            .AddWithValue("@FirstName", firstname)
                            .AddWithValue("@LastName", lastname)
                            .AddWithValue("@ContactNumber", contact)
                            .AddWithValue("@Email", email)
                            .AddWithValue("@Address", address)
                            .AddWithValue("@Username", username)
                            .AddWithValue("@Password", hashedPass)
                            .AddWithValue("@DateCreated", DateTime.Now)
                            .AddWithValue("@IsActive", True)
                        End With

                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added User.")
                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while adding user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 🔒 Show/Hide Password
    Private Sub cbxShowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowpassword.CheckedChanged
        Dim visible As Boolean = cbxShowpassword.Checked
        txtpassword.PasswordChar = If(visible, "", "*"c)
        txtConfirmPass.PasswordChar = If(visible, "", "*"c)
    End Sub

    ' 🧩 Exit Button
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class
