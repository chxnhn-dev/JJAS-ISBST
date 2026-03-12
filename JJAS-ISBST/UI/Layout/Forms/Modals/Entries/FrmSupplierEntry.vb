Imports System.Data
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports JJAS_ISBST.FrmLogin

Public Class FrmSupplierEntry
    Private ReadOnly _service As New SupplierService()

    Public Sub New()
        InitializeComponent()
        InitializeModernUiIfNeeded()
    End Sub

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property SupplierId As Integer?
        Get
            If SelectedId > 0 Then
                Return SelectedId
            End If

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
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        BlockCopyPaste(txtCompany)
        BlockCopyPaste(txtContactNumber)
        BlockCopyPaste(txtAddress)

        ConfigureMode()

        If IsEditMode Then
            LoadSupplierForEdit()
        Else
            ResetForm()
        End If

        RefreshSupplierVisuals()
    End Sub

    Private Sub FrmSupplierEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        RefreshSupplierVisuals()
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Text = "Edit Supplier"
            lblTitle.Text = "Edit Supplier"
            lblSubtitle.Text = "Update supplier details below"
            btnAdd.Text = "Update"
        Else
            Text = "Add Supplier"
            lblTitle.Text = "Add Supplier"
            lblSubtitle.Text = "Fill in the details below"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub ResetForm()
        txtCompany.Clear()
        txtContactNumber.Clear()
        txtAddress.Clear()
        chkAcceptsReturnRefund.Checked = False
        nudReturnWindowDays.Value = 7D
        UpdateReturnWindowState(True)
    End Sub

    Private Sub LoadSupplierForEdit()
        Dim row As DataRow = _service.GetSupplierById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected supplier record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Close()
            Return
        End If

        txtCompany.Text = row("Company").ToString()
        txtContactNumber.Text = row("ContactNumber").ToString()
        txtAddress.Text = row("Address").ToString()

        Dim acceptsReturnRefund As Boolean = False
        If row.Table.Columns.Contains("AcceptsReturnRefund") AndAlso Not IsDBNull(row("AcceptsReturnRefund")) Then
            acceptsReturnRefund = Convert.ToBoolean(row("AcceptsReturnRefund"))
        End If

        chkAcceptsReturnRefund.Checked = acceptsReturnRefund

        If acceptsReturnRefund AndAlso row.Table.Columns.Contains("ReturnWindowDays") AndAlso Not IsDBNull(row("ReturnWindowDays")) Then
            Dim storedValue As Integer = Convert.ToInt32(row("ReturnWindowDays"))
            Dim normalizedValue As Decimal = CDec(Math.Max(1, Math.Min(365, storedValue)))
            nudReturnWindowDays.Value = normalizedValue
        Else
            nudReturnWindowDays.Value = 7D
        End If

        UpdateReturnWindowState(True)
        RefreshSupplierVisuals()
    End Sub

    Private Function IsValidContactNumber(num As String) As Boolean
        Dim trimmedNum As String = num.Trim()
        Return Not String.IsNullOrWhiteSpace(trimmedNum) AndAlso Regex.IsMatch(trimmedNum, "^\d{11}$")
    End Function

    Private Function CompanyExists(companyName As String) As Boolean
        Return _service.CompanyExists(companyName, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Sub UpdateReturnWindowState(clearValueWhenDisabled As Boolean)
        Dim isEnabled As Boolean = chkAcceptsReturnRefund.Checked

        nudReturnWindowDays.Enabled = isEnabled
        nudReturnWindowDays.Visible = True
        nudReturnWindowDays.TabStop = isEnabled
        nudReturnWindowDays.Cursor = If(isEnabled, Cursors.IBeam, Cursors.Default)
        lblReturnWindow.ForeColor = If(isEnabled, Color.FromArgb(240, 242, 245), Color.FromArgb(132, 137, 146))
        lblReturnWindow.Visible = True

        If pnlFields IsNot Nothing Then
            pnlFields.Enabled = True
        End If

        If Not isEnabled AndAlso clearValueWhenDisabled Then
            nudReturnWindowDays.Value = 7D
        End If

        LayoutFieldControls()

        nudReturnWindowDays.BringToFront()
    End Sub

    Private Sub chkAcceptsReturnRefund_CheckedChanged(sender As Object, e As EventArgs) Handles chkAcceptsReturnRefund.CheckedChanged
        UpdateReturnWindowState(True)

        If chkAcceptsReturnRefund.Checked Then
            nudReturnWindowDays.Focus()
            nudReturnWindowDays.Select()
            RefreshSupplierVisuals()
        End If
    End Sub

    Private Sub txtContactNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim supplierName As String = txtCompany.Text.Trim()
        Dim contact As String = txtContactNumber.Text.Trim()
        Dim address As String = txtAddress.Text.Trim()
        Dim acceptsReturnRefund As Boolean = chkAcceptsReturnRefund.Checked
        Dim returnWindowDays As Integer? = Nothing

        If String.IsNullOrWhiteSpace(supplierName) Then
            MessageBox.Show("Please enter a supplier name.", "Supplier Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCompany.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(contact) Then
            MessageBox.Show("Please enter a contact number.", "Contact Number Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNumber.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(address) Then
            MessageBox.Show("Please enter an address.", "Address Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAddress.Focus()
            Exit Sub
        End If

        If supplierName.Length < 2 Then
            MessageBox.Show("Supplier name must be at least 2 characters long.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCompany.Focus()
            txtCompany.SelectAll()
            Exit Sub
        End If

        If Not contact.StartsWith("09") Then
            MessageBox.Show("Contact number must start with '09'.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNumber.Focus()
            txtContactNumber.SelectAll()
            Exit Sub
        End If

        If Not IsValidContactNumber(contact) Then
            MessageBox.Show("Contact number must be exactly 11 digits long.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNumber.Focus()
            txtContactNumber.SelectAll()
            Exit Sub
        End If

        If CompanyExists(supplierName) Then
            MessageBox.Show("This supplier already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCompany.Focus()
            txtCompany.SelectAll()
            Exit Sub
        End If

        If acceptsReturnRefund Then
            Dim parsedReturnWindow As Integer = Decimal.ToInt32(nudReturnWindowDays.Value)

            If parsedReturnWindow < 1 OrElse parsedReturnWindow > 365 Then
                MessageBox.Show("Return window days must be between 1 and 365.", "Invalid Return Window", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                nudReturnWindowDays.Focus()
                nudReturnWindowDays.Select()
                Exit Sub
            End If

            returnWindowDays = parsedReturnWindow
        End If

        txtCompany.Text = supplierName
        txtContactNumber.Text = contact
        txtAddress.Text = address

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this supplier?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            _service.SaveSupplier(Mode, SelectedId, supplierName, contact, address, acceptsReturnRefund, returnWindowDays)

            LogActivity(CurrentUser.UserID,
                        CurrentUser.FullName,
                        CurrentUser.Username,
                        CurrentUser.Role,
                        If(IsEditMode, "Edited supplier.", "Added supplier."))

            MessageBox.Show(If(IsEditMode, "Supplier updated successfully!", "Supplier added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Error saving supplier: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class
