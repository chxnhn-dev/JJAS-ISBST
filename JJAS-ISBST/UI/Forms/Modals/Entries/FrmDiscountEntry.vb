Imports JJAS_ISBST.FrmLogin

Public Class FrmDiscountEntry
    Private ReadOnly _service As New DiscountService()

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property DiscountID As Integer?
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

    Private Sub FrmDiscountEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigureMode()
        BlockCopyPaste(txtDiscountName)
        BlockCopyPaste(txtDiscountValue)
        BlockCopyPaste(txtDescription)

        If IsEditMode Then
            LoadDiscountForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Discount"
            btnAdd.Text = "  Update"
        Else
            Me.Text = "Add Discount"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadDiscountForEdit()
        Dim row As DataRow = _service.GetDiscountById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected discount record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        txtDiscountName.Text = row("DiscountName").ToString()
        txtDiscountValue.Text = Convert.ToDecimal(row("DiscountValue")).ToString("0.##")
        txtDescription.Text = row("Description").ToString()
    End Sub

    Private Sub txtDiscountValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountValue.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = txtDiscountValue.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtDiscountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "-"c OrElse e.KeyChar = ControlChars.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Function IsDuplicateDiscount(name As String) As Boolean
        Return _service.IsDuplicateName(name, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim discountName As String = txtDiscountName.Text.Trim()
        Dim description As String = txtDescription.Text.Trim()

        If String.IsNullOrWhiteSpace(discountName) Then
            MessageBox.Show("Please enter a discount name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        If discountName.Length < 2 Then
            MessageBox.Show("Discount name must be at least 2 characters long.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If discountName.Contains("  ") Then
            MessageBox.Show("Discount name cannot contain multiple consecutive spaces.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicateDiscount(discountName) Then
            MessageBox.Show("This discount name already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountName.Focus()
            Exit Sub
        End If

        Dim discountValue As Decimal
        If String.IsNullOrWhiteSpace(txtDiscountValue.Text) OrElse Not Decimal.TryParse(txtDiscountValue.Text, discountValue) Then
            MessageBox.Show("Please enter a valid discount value.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        If discountValue < 0D OrElse discountValue > 100D Then
            MessageBox.Show("Percentage must be between 0 and 100.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountValue.Focus()
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this discount?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            _service.SaveDiscount(Mode, SelectedId, discountName, discountValue, description)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Discount.", "Added discount."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error while saving discount: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
