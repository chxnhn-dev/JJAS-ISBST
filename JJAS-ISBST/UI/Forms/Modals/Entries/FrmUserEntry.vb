Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net.Mail
Imports JJAS_ISBST.FrmLogin

Public Class FrmUserEntry
    Public Property UserID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return UserID.HasValue
        End Get
    End Property

    Private Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return BitConverter.ToString(bytes).Replace("-", "").ToLower()
        End Using
    End Function

    Private Sub Add_User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each tb In {txtfirstname, txtlastname, txtcontactnumber, txtemail, txtaddress, txtusername, txtpassword, txtConfirmPass}
            BlockCopyPaste(tb)
        Next

        cbrole.Items.Clear()
        cbrole.Items.Add("Select Role")
        cbrole.Items.Add("Staff")
        cbrole.Items.Add("Cashier")
        cbrole.SelectedIndex = 0

        ConfigureMode()
        If IsEditMode Then
            LoadUserForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit User"
            btnAdd.Text = "  Update"
        Else
            Me.Text = "FrmUserEntry"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadUserForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Role, FirstName, LastName, ContactNumber, Email, Address, Username FROM tbl_User WHERE UserID = @UserID", connection)
                command.Parameters.AddWithValue("@UserID", UserID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        cbrole.Text = reader("Role").ToString()
                        txtfirstname.Text = reader("FirstName").ToString()
                        txtlastname.Text = reader("LastName").ToString()
                        txtcontactnumber.Text = reader("ContactNumber").ToString()
                        txtemail.Text = reader("Email").ToString()
                        txtaddress.Text = reader("Address").ToString()
                        txtusername.Text = reader("Username").ToString()
                        txtpassword.Clear()
                        txtConfirmPass.Clear()
                    Else
                        MessageBox.Show("Selected user record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_User WHERE {fieldName} = @Value AND IsActive = 1"

        If IsEditMode Then
            sql &= " AND UserID <> @UserID"
        End If

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@UserID", UserID.Value)
                End If

                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Function IsDigitsOnly(str As String) As Boolean
        Return str.All(AddressOf Char.IsDigit)
    End Function

    Private Sub txtcontactnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcontactnumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtusername_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtusername.KeyPress
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
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso
           Not Char.IsControl(e.KeyChar) AndAlso
           Not e.KeyChar = " "c AndAlso
           Not e.KeyChar = ","c AndAlso
           Not e.KeyChar = "."c Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim firstname = txtfirstname.Text.Trim()
            Dim lastname = txtlastname.Text.Trim()
            Dim username = txtusername.Text.Trim()
            Dim password = txtpassword.Text.Trim()
            Dim confirmPass = txtConfirmPass.Text.Trim()
            Dim email = txtemail.Text.Trim()
            Dim contact = txtcontactnumber.Text.Trim()
            Dim address = txtaddress.Text.Trim()
            Dim role = cbrole.Text

            If {firstname, lastname, username, email, contact, address}.Any(Function(x) String.IsNullOrWhiteSpace(x)) OrElse cbrole.SelectedIndex = 0 Then
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

            If contact.Length <> 11 Then
                MessageBox.Show("Contact number must be exactly 11 digits!", "Invalid Length", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtcontactnumber.Clear()
                txtcontactnumber.Focus()
                Exit Sub
            End If

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

            Dim updatePassword As Boolean

            If IsEditMode Then
                updatePassword = Not String.IsNullOrWhiteSpace(password) OrElse Not String.IsNullOrWhiteSpace(confirmPass)
                If updatePassword Then
                    If String.IsNullOrWhiteSpace(password) OrElse String.IsNullOrWhiteSpace(confirmPass) Then
                        MessageBox.Show("Please enter and confirm the new password.", "Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                    If password <> confirmPass Then
                        MessageBox.Show("Passwords do not match.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtConfirmPass.Clear()
                        txtConfirmPass.Focus()
                        Exit Sub
                    End If
                End If
            Else
                If String.IsNullOrWhiteSpace(password) OrElse String.IsNullOrWhiteSpace(confirmPass) Then
                    MessageBox.Show("Password and Confirm Password are required.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                If password <> confirmPass Then
                    MessageBox.Show("Passwords do not match.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtConfirmPass.Clear()
                    txtConfirmPass.Focus()
                    Exit Sub
                End If

                updatePassword = True
            End If

            Dim actionText As String = If(IsEditMode, "update", "add")
            If MessageBox.Show($"Are you sure you want to {actionText} this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Exit Sub
            End If

            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                If IsEditMode Then
                    Dim updateSql As String = "UPDATE tbl_User SET Role=@Role, FirstName=@FirstName, LastName=@LastName, ContactNumber=@ContactNumber, Email=@Email, Address=@Address, Username=@Username, DateCreated=@DateCreated WHERE UserID=@UserID"

                    Using command As New SqlCommand(updateSql, connection)
                        command.Parameters.AddWithValue("@Role", role)
                        command.Parameters.AddWithValue("@FirstName", firstname)
                        command.Parameters.AddWithValue("@LastName", lastname)
                        command.Parameters.AddWithValue("@ContactNumber", contact)
                        command.Parameters.AddWithValue("@Email", email)
                        command.Parameters.AddWithValue("@Address", address)
                        command.Parameters.AddWithValue("@Username", username)
                        command.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        command.Parameters.AddWithValue("@UserID", UserID.Value)
                        command.ExecuteNonQuery()
                    End Using

                    If updatePassword Then
                        Dim hashedPass = HashPassword(password)
                        Using commandPass As New SqlCommand("UPDATE tbl_User SET Password=@Password WHERE UserID=@UserID", connection)
                            commandPass.Parameters.AddWithValue("@Password", hashedPass)
                            commandPass.Parameters.AddWithValue("@UserID", UserID.Value)
                            commandPass.ExecuteNonQuery()
                        End Using
                    End If
                Else
                    Dim hashedPass = HashPassword(password)
                    Dim insertSql As String = "INSERT INTO tbl_User (Role, FirstName, LastName, ContactNumber, Email, Address, Username, Password, DateCreated, IsActive) VALUES (@Role, @FirstName, @LastName, @ContactNumber, @Email, @Address, @Username, @Password, @DateCreated, @IsActive)"

                    Using command As New SqlCommand(insertSql, connection)
                        command.Parameters.AddWithValue("@Role", role)
                        command.Parameters.AddWithValue("@FirstName", firstname)
                        command.Parameters.AddWithValue("@LastName", lastname)
                        command.Parameters.AddWithValue("@ContactNumber", contact)
                        command.Parameters.AddWithValue("@Email", email)
                        command.Parameters.AddWithValue("@Address", address)
                        command.Parameters.AddWithValue("@Username", username)
                        command.Parameters.AddWithValue("@Password", hashedPass)
                        command.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        command.Parameters.AddWithValue("@IsActive", True)
                        command.ExecuteNonQuery()
                    End Using
                End If
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited User.", "Added User."))
            MessageBox.Show(If(IsEditMode, "User updated successfully!", "User added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbxShowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowpassword.CheckedChanged
        Dim visible As Boolean = cbxShowpassword.Checked
        txtpassword.PasswordChar = If(visible, ControlChars.NullChar, "*"c)
        txtConfirmPass.PasswordChar = If(visible, ControlChars.NullChar, "*"c)
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class
