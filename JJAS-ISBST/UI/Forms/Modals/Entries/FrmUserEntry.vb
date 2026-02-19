Imports System.Net.Mail
Imports JJAS_ISBST.FrmLogin

Public Class FrmUserEntry
    Private ReadOnly _service As New UserService()

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property UserID As Integer?
        Get
            If SelectedId > 0 Then Return SelectedId
            Return Nothing
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                SelectedId = value.Value
                Mode = EntryFormMode.EditExisting
            Else
                SelectedId = -1
                Mode = EntryFormMode.AddNew
            End If
        End Set
    End Property

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return Mode = EntryFormMode.EditExisting AndAlso SelectedId > 0
        End Get
    End Property

    Private Sub FrmUserEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            Me.Text = "Add User"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadUserForEdit()
        Dim row As DataRow = _service.GetUserById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected user record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        cbrole.Text = row("Role").ToString()
        txtfirstname.Text = row("FirstName").ToString()
        txtlastname.Text = row("LastName").ToString()
        txtcontactnumber.Text = row("ContactNumber").ToString()
        txtemail.Text = row("Email").ToString()
        txtaddress.Text = row("Address").ToString()
        txtusername.Text = row("Username").ToString()
        txtpassword.Clear()
        txtConfirmPass.Clear()
    End Sub

    Private Function IsDuplicateUsername(value As String) As Boolean
        Return _service.UsernameExists(value, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Function IsDuplicateEmail(value As String) As Boolean
        Return _service.EmailExists(value, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Function IsDuplicateContact(value As String) As Boolean
        Return _service.ContactExists(value, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
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

            If IsDuplicateUsername(username) Then
                MessageBox.Show("Username already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicateEmail(email) Then
                MessageBox.Show("Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If IsDuplicateContact(contact) Then
                MessageBox.Show("Contact Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim passwordToSave As String = Nothing

            If IsEditMode Then
                Dim updatePassword As Boolean = Not String.IsNullOrWhiteSpace(password) OrElse Not String.IsNullOrWhiteSpace(confirmPass)
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
                    passwordToSave = password
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

                passwordToSave = password
            End If

            Dim actionText As String = If(IsEditMode, "update", "add")
            If MessageBox.Show($"Are you sure you want to {actionText} this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Exit Sub
            End If

            _service.SaveUser(Mode, SelectedId, role, firstname, lastname, contact, email, address, username, passwordToSave)

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
