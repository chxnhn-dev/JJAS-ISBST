Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports JJAS_ISBST.Login

' Note: For better separation of concerns, consider moving database operations (e.g., duplicate checks and updates) to a dedicated data access layer, such as a SupplierRepository class.
' This would make the form code cleaner and easier to test. Example structure:
' Public Class SupplierRepository
'     Public Function CompanyDuplicateExists(name As String, currentId As Integer) As Boolean
'         ' Implementation here
'     End Function
'     ' Other methods...
' End Class

Public Class Edit_Supplier
    Public Property SupplierId As Integer
    Public Property CompanyName As String
    Public Property SupplierFullName As String
    Public Property ContactNumber As String
    Public Property Address As String

    Private Sub Edit_Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable copy-paste for text fields (designer names)
        Try
            BlockCopyPaste(txtCompany)
            BlockCopyPaste(txtAddress)
            BlockCopyPaste(txtContactNumber)
        Catch ex As Exception
            ' Log or handle the error instead of ignoring it
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Load existing data into fields
        txtCompany.Text = If(CompanyName, String.Empty)
        txtContactNumber.Text = If(ContactNumber, String.Empty)
        txtAddress.Text = If(Address, String.Empty)  ' Fixed: Was 'AddressValue', now uses the correct property 'Address'

        ' UI Polish: Add tooltips to clarify field requirements
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(txtCompany, "Enter the company name (required).")
        tooltip.SetToolTip(txtContactNumber, "Enter a valid 11-digit contact number (required).")
        tooltip.SetToolTip(txtAddress, "Enter the address (required, multi-line allowed).")

        ' UI Polish: Enable/disable the Edit button based on validation state
        'UpdateEditButtonState()
    End Sub

    'Private Sub txtCompany_TextChanged(sender As Object, e As EventArgs) Handles txtCompany.TextChanged
    '    UpdateEditButtonState()
    'End Sub

    'Private Sub txtVendorName_TextChanged(sender As Object, e As EventArgs) Handles txtFullname.TextChanged
    '    UpdateEditButtonState()
    'End Sub

    'Private Sub txtCContactNumber_TextChanged(sender As Object, e As EventArgs) Handles txtContactNumber.TextChanged
    '    UpdateEditButtonState()
    'End Sub

    'Private Sub txtCAddress_TextChanged(sender As Object, e As EventArgs) Handles txtAddress.TextChanged
    '    UpdateEditButtonState()
    'End Sub

    'Private Sub UpdateEditButtonState()
    '    ' UI Polish: Enable Edit button only if all required fields are filled and contact number is valid
    '    btnEdit.Enabled = Not String.IsNullOrWhiteSpace(txtCompany.Text.Trim()) AndAlso
    '                      Not String.IsNullOrWhiteSpace(txtFullname.Text.Trim()) AndAlso
    '                      Not String.IsNullOrWhiteSpace(txtAddress.Text.Trim()) AndAlso
    '                      IsValidContactNumber(txtContactNumber.Text.Trim())
    'End Sub
    Private Sub txtcontactnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Function IsValidContactNumber(num As String) As Boolean
        ' Enhanced Validation: Trim whitespace before checking
        ' Fixed: Changed to exactly 11 digits as per your request
        Dim trimmedNum As String = num.Trim()
        Return Not String.IsNullOrWhiteSpace(trimmedNum) AndAlso Regex.IsMatch(trimmedNum, "^\d{11}$")
    End Function

    ' Refactoring: Moved to a potential SupplierRepository class for better separation
    Private Function CompanyDuplicateExists(name As String, currentId As Integer) As Boolean
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand("SELECT COUNT(1) FROM tbl_supplier WHERE Company = @Company AND SupplierId <> @Id", conn)
                cmd.Parameters.AddWithValue("@Company", name.Trim())
                cmd.Parameters.AddWithValue("@Id", currentId)
                conn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' Enhanced Validation: Trim all inputs before validation and use consistent checks
        Dim company As String = txtCompany.Text.Trim()
        Dim contact As String = txtContactNumber.Text.Trim()
        Dim address As String = txtAddress.Text.Trim()

        If String.IsNullOrEmpty(company) OrElse
           String.IsNullOrEmpty(contact) OrElse
           String.IsNullOrEmpty(address) Then
            MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        If MessageBox.Show("Are you sure you want to update this supplier?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            ' Performance: Reuse a single connection for duplicate checks to avoid opening/closing multiple times
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()


                Try
                    ' Check company duplicate existence
                    Using cmd As New SqlCommand("SELECT COUNT(1) FROM tbl_supplier WHERE Company = @Company AND SupplierId <> @Id", conn)
                        cmd.Parameters.AddWithValue("@Company", company)
                        cmd.Parameters.AddWithValue("@Id", SupplierId)
                        If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                            MessageBox.Show("Company name already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Validation error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                ' Update supplier record (using the same connection)
                Try
                    Using cmd As New SqlCommand("UPDATE tbl_supplier SET Company = @Company, ContactNumber = @ContactNumber, Address = @Address WHERE SupplierId = @SupplierId", conn)
                        cmd.Parameters.AddWithValue("@Company", company)
                        cmd.Parameters.AddWithValue("@ContactNumber", contact)
                        cmd.Parameters.AddWithValue("@Address", address)
                        cmd.Parameters.AddWithValue("@SupplierId", SupplierId)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Security: Ensure CurrentUser details are securely retrieved (e.g., from a session or authenticated context, not hardcoded)
                    ' Assuming CurrentUser is properly set via login/session management
                    LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited supplier.")
                    MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Catch ex As Exception
                    Try
                        LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Error editing supplier: " & ex.Message)
                    Catch
                    End Try
                    MessageBox.Show("Error updating supplier: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    ' Fixed: Merged the two btnExit_Click handlers into one (removed the empty duplicate)
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
