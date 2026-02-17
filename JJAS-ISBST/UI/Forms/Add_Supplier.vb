Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports JJAS_ISBST.Login

Public Class Add_Supplier
    Public Property SupplierId As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return SupplierId.HasValue
        End Get
    End Property

    Private Sub Add_Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            BlockCopyPaste(txtCompany)
            BlockCopyPaste(txtContactNumber)
            BlockCopyPaste(txtAddress)
        Catch ex As Exception
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(txtCompany, "Enter the company name (required).")
        tooltip.SetToolTip(txtContactNumber, "Enter a valid 11-digit contact number (required).")
        tooltip.SetToolTip(txtAddress, "Enter the address (required, multi-line allowed).")

        ConfigureMode()
        If IsEditMode Then
            LoadSupplierForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Supplier"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Supplier"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadSupplierForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Company, ContactNumber, Address FROM tbl_supplier WHERE SupplierId = @SupplierId", connection)
                command.Parameters.AddWithValue("@SupplierId", SupplierId.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtCompany.Text = reader("Company").ToString()
                        txtContactNumber.Text = reader("ContactNumber").ToString()
                        txtAddress.Text = reader("Address").ToString()
                    Else
                        MessageBox.Show("Selected supplier record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsValidContactNumber(num As String) As Boolean
        Dim trimmedNum As String = num.Trim()
        Return Not String.IsNullOrWhiteSpace(trimmedNum) AndAlso Regex.IsMatch(trimmedNum, "^\d{11}$")
    End Function

    Private Function CompanyExists(companyName As String) As Boolean
        Dim sql As String = "SELECT COUNT(1) FROM tbl_supplier WHERE Company = @Company"
        If IsEditMode Then
            sql &= " AND SupplierId <> @SupplierId"
        End If

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Company", companyName.Trim())
                If IsEditMode Then
                    command.Parameters.AddWithValue("@SupplierId", SupplierId.Value)
                End If

                connection.Open()
                Return Convert.ToInt32(command.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Private Sub txtcontactnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim company As String = txtCompany.Text.Trim()
        Dim contact As String = txtContactNumber.Text.Trim()
        Dim address As String = txtAddress.Text.Trim()

        If String.IsNullOrEmpty(company) OrElse String.IsNullOrEmpty(contact) OrElse String.IsNullOrEmpty(address) Then
            MessageBox.Show("Please fill in all required fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
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
            Exit Sub
        End If

        If CompanyExists(company) Then
            MessageBox.Show("This company already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this supplier?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_supplier SET Company=@Company, ContactNumber=@ContactNumber, Address=@Address WHERE SupplierId=@SupplierId"
                Else
                    sql = "INSERT INTO tbl_supplier (Company, ContactNumber, Address, isactive, expiredate, dateCreated) VALUES (@Company, @ContactNumber, @Address, 1, null, GetDate())"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@Company", company)
                    command.Parameters.AddWithValue("@ContactNumber", contact)
                    command.Parameters.AddWithValue("@Address", address)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@SupplierId", SupplierId.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited supplier.", "Added supplier."))
            MessageBox.Show(If(IsEditMode, "Supplier updated successfully!", "Supplier added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error saving supplier: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
