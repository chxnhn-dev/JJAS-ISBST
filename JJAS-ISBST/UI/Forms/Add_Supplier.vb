Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports JJAS_ISBST.Login

' Note: For better separation of concerns, consider moving database operations (e.g., existence checks and inserts) to a dedicated data access layer, such as a SupplierRepository class.
' This would make the form code cleaner and easier to test. Example structure:
' Public Class SupplierRepository
'     Public Function CompanyExists(companyName As String) As Boolean
'         ' Implementation here
'     End Function
'     ' Other methods...
' End Class

Public Class Add_Supplier
    Private Sub Add_Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable copy-paste for text fields (designer names)
        Try
            BlockCopyPaste(txtCompany)
            BlockCopyPaste(txtContactNumber)
            BlockCopyPaste(txtAddress)
        Catch ex As Exception
            ' Log or handle the error instead of ignoring it
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' UI Polish: Add tooltips to clarify field requirements
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(txtCompany, "Enter the company name (required).")
        tooltip.SetToolTip(txtContactNumber, "Enter a valid 11-digit contact number (required).")
        tooltip.SetToolTip(txtAddress, "Enter the address (required, multi-line allowed).")

        ' UI Polish: Enable/disable the Add button based on validation state
        'UpdateAddButtonState()
    End Sub

    'Private Sub txtCompany_TextChanged(sender As Object, e As EventArgs) Handles txtCompany.TextChanged
    '    UpdateAddButtonState()
    'End Sub

    'Private Sub txtFullname_TextChanged(sender As Object, e As EventArgs) Handles txtFullname.TextChanged
    '    UpdateAddButtonState()
    'End Sub

    'Private Sub txtContactNumber_TextChanged(sender As Object, e As EventArgs) Handles txtContactNumber.TextChanged
    '    UpdateAddButtonState()
    'End Sub

    'Private Sub txtAddress_TextChanged(sender As Object, e As EventArgs) Handles txtAddress.TextChanged
    '    UpdateAddButtonState()
    'End Sub

    'Private Sub UpdateAddButtonState()
    '    ' ✅ Enable Add button only if all required fields are filled and contact number is valid
    '    btnAdd.Enabled = Not String.IsNullOrWhiteSpace(txtCompany.Text.Trim()) AndAlso
    '                 Not String.IsNullOrWhiteSpace(txtFullname.Text.Trim()) AndAlso
    '                 Not String.IsNullOrWhiteSpace(txtAddress.Text.Trim()) AndAlso
    '                 IsValidContactNumber(txtContactNumber.Text.Trim())
    'End Sub
    Private Function IsValidContactNumber(num As String) As Boolean
        ' Enhanced Validation: Trim whitespace before checking
        Dim trimmedNum As String = num.Trim()
        Return Not String.IsNullOrWhiteSpace(trimmedNum) AndAlso Regex.IsMatch(trimmedNum, "^\d{11}$")
    End Function

    ' Refactoring: Moved to a potential SupplierRepository class for better separation
    Private Function CompanyExists(companyName As String) As Boolean
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand("SELECT COUNT(1) FROM tbl_supplier WHERE Company = @Company", conn)
                cmd.Parameters.AddWithValue("@Company", companyName.Trim())
                conn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function
    Private Sub txtcontactnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Enhanced Validation: Trim all inputs before validation and use consistent checks
        Dim company As String = txtCompany.Text.Trim()
        Dim contact As String = txtContactNumber.Text.Trim()
        Dim address As String = txtAddress.Text.Trim()

        If String.IsNullOrEmpty(company) OrElse
           String.IsNullOrEmpty(contact) OrElse
           String.IsNullOrEmpty(address) Then
            MessageBox.Show("Please fill in all required fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return

        End If

        If company.Length < 2 Then
            MessageBox.Show("Company must be at least 2 characters long.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not contact.StartsWith("09") Then
            MessageBox.Show("Contact number must start with '09'.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not IsValidContactNumber(contact) Then
            MessageBox.Show("Contact number must be exactly 11 digits long.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Performance: Reuse a single connection for duplicate checks to avoid opening/closing multiple times
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()
            Try
                ' Check company existence
                Using cmd As New SqlCommand("SELECT COUNT(1) FROM tbl_supplier WHERE Company = @Company", conn)
                    cmd.Parameters.AddWithValue("@Company", company)
                    If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                        MessageBox.Show("This company already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Validation error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
            ' Insert supplier record (using the same connection)

            If MessageBox.Show("Are you sure you want to add this supplier?",
               "Confirm Edit",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                Try
                    Using cmd As New SqlCommand(
                        "INSERT INTO tbl_supplier (Company, ContactNumber, Address, isactive, expiredate, dateCreated) " &
                        "VALUES (@Company, @ContactNumber, @Address, 1, null, Getdate())", conn)
                        cmd.Parameters.AddWithValue("@Company", company)
                        cmd.Parameters.AddWithValue("@ContactNumber", contact)
                        cmd.Parameters.AddWithValue("@Address", address)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Security: Ensure CurrentUser details are securely retrieved (e.g., from a session or authenticated context, not hardcoded)
                    ' Assuming CurrentUser is properly set via login/session management
                    LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added supplier.")
                    MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Catch ex As Exception
                    Try
                        LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Error adding supplier: " & ex.Message)
                    Catch
                    End Try
                    MessageBox.Show("Error saving supplier: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class