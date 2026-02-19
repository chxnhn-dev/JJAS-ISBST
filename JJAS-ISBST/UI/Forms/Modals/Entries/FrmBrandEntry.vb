Imports JJAS_ISBST.FrmLogin

Public Class FrmBrandEntry
    Private ReadOnly _service As New BrandService()

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property BrandID As Integer?
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

    Private Sub FrmBrandEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtbrand)
        ConfigureMode()

        If IsEditMode Then
            LoadBrandForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Brand"
            btnAdd.Text = "  Update"
        Else
            Me.Text = "Add Brand"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadBrandForEdit()
        Dim row As DataRow = _service.GetBrandById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected brand record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        txtbrand.Text = row("Brand").ToString()
    End Sub

    Private Function IsDuplicate(brandName As String) As Boolean
        Return _service.IsDuplicateName(brandName, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Sub txtbrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbrand.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = False
            Exit Sub
        End If

        Dim allowedSymbols As String = "&'+-"
        If allowedSymbols.Contains(e.KeyChar) Then
            e.Handled = (txtbrand.Text.IndexOfAny(allowedSymbols.ToCharArray()) <> -1)
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim brandName As String = txtbrand.Text.Trim()

        If String.IsNullOrWhiteSpace(brandName) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If brandName.Length < 2 Then
            MessageBox.Show("Brand name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If brandName.Contains("  ") Then
            MessageBox.Show("Brand name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate(brandName) Then
            MessageBox.Show("Brand already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this brand?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            _service.SaveBrand(Mode, SelectedId, brandName)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Brand.", "Added Brand."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
