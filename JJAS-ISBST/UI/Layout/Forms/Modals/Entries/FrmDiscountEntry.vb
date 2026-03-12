Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Public Class FrmDiscountEntry
    Private ReadOnly _service As New DiscountService()
    Private Shared ReadOnly ModalSize As New Size(820, 580)

    Private _modernUiInitialized As Boolean
    Private _lblSubtitle As Label
    Private _pnlFields As Panel
    Private _pnlActions As Panel
    Private _btnCloseProxy As Guna2Button
    Private _btnCancelProxy As Guna2Button
    Private _btnClearProxy As Guna2Button
    Private _btnPrimaryProxy As Guna2Button
    Private _txtDiscountNameProxy As Guna2TextBox
    Private _txtDiscountValueProxy As Guna2TextBox
    Private _txtDescriptionProxy As Guna2TextBox
    Private _isSyncingFields As Boolean

    Public Sub New()
        InitializeComponent()

        ClientSize = ModalSize
        MinimumSize = ModalSize
        MaximumSize = ModalSize

        InitializeModernUiIfNeeded()
    End Sub

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
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        If _txtDiscountNameProxy IsNot Nothing Then BlockCopyPaste(_txtDiscountNameProxy)
        If _txtDiscountValueProxy IsNot Nothing Then BlockCopyPaste(_txtDiscountValueProxy)
        If _txtDescriptionProxy IsNot Nothing Then BlockCopyPaste(_txtDescriptionProxy)

        ConfigureMode()

        If IsEditMode Then
            LoadDiscountForEdit()
        Else
            txtDiscountName.Clear()
            txtDiscountValue.Clear()
            txtDescription.Clear()
            SyncLegacyToModernFields()
        End If

        RefreshDiscountVisuals()
    End Sub

    Private Sub FrmDiscountEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub CenterModalToOwnerOrScreen()
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Discount"
            Label12.Text = "Edit Discount"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Discount"
            Label12.Text = "Add Discount"
            btnAdd.Text = "Save"
        End If

        If _lblSubtitle IsNot Nothing Then
            _lblSubtitle.Text = "Fill in the details below"
        End If

        SyncPrimaryButtonFromLegacy()
        LayoutFieldControls()
        LayoutActionButtons()
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

        SyncLegacyToModernFields()
        ForceImmediateFieldPaint()
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

    Private Sub txtDiscountValueProxy_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = _txtDiscountValueProxy IsNot Nothing AndAlso _txtDiscountValueProxy.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtDiscountNameProxy_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "-"c OrElse e.KeyChar = ControlChars.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Function IsDuplicateDiscount(name As String) As Boolean
        Return _service.IsDuplicateName(name, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        SyncModernToLegacyFields()

        Dim discountName As String = txtDiscountName.Text.Trim()
        Dim description As String = txtDescription.Text.Trim()

        If String.IsNullOrWhiteSpace(discountName) Then
            MessageBox.Show("Please enter a discount name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusDiscountNameField()
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
            FocusDiscountNameField()
            Exit Sub
        End If

        Dim discountValue As Decimal
        If String.IsNullOrWhiteSpace(txtDiscountValue.Text) OrElse Not Decimal.TryParse(txtDiscountValue.Text, discountValue) Then
            MessageBox.Show("Please enter a valid discount value.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusDiscountValueField()
            Exit Sub
        End If

        If discountValue < 0D OrElse discountValue > 100D Then
            MessageBox.Show("Percentage must be between 0 and 100.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusDiscountValueField()
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

    Private Sub FocusDiscountNameField()
        If _txtDiscountNameProxy IsNot Nothing AndAlso _txtDiscountNameProxy.Visible Then
            _txtDiscountNameProxy.Focus()
        Else
            txtDiscountName.Focus()
        End If
    End Sub

    Private Sub FocusDiscountValueField()
        If _txtDiscountValueProxy IsNot Nothing AndAlso _txtDiscountValueProxy.Visible Then
            _txtDiscountValueProxy.Focus()
        Else
            txtDiscountValue.Focus()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
