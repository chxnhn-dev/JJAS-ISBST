Imports System.Text.RegularExpressions
Imports JJAS_ISBST.FrmLogin

Public Class FrmSupplierEntry
    Private ReadOnly _service As New SupplierService()

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property SupplierId As Integer?
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

    Private Sub FrmSupplierEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            btnAdd.Text = "  Update"
        Else
            Me.Text = "Add Supplier"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadSupplierForEdit()
        Dim row As DataRow = _service.GetSupplierById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected supplier record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        txtCompany.Text = row("Company").ToString()
        txtContactNumber.Text = row("ContactNumber").ToString()
        txtAddress.Text = row("Address").ToString()
    End Sub

    Private Function IsValidContactNumber(num As String) As Boolean
        Dim trimmedNum As String = num.Trim()
        Return Not String.IsNullOrWhiteSpace(trimmedNum) AndAlso Regex.IsMatch(trimmedNum, "^\d{11}$")
    End Function

    Private Function CompanyExists(companyName As String) As Boolean
        Return _service.CompanyExists(companyName, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
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
            _service.SaveSupplier(Mode, SelectedId, company, contact, address)

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

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
    End Sub
End Class
