Imports JJAS_ISBST.FrmLogin

Public Class FrmCategoryEntry
    Private ReadOnly _service As New CategoryService()

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property CategoryID As Integer?
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

    Private Sub FrmCategoryEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtBrand)
        ConfigureMode()

        If IsEditMode Then
            LoadCategoryForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Category"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Category"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadCategoryForEdit()
        Dim row As DataRow = _service.GetCategoryById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected category record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        txtBrand.Text = row("Category").ToString()
    End Sub

    Private Function IsDuplicate(categoryName As String) As Boolean
        Return _service.IsDuplicateName(categoryName, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBrand.KeyPress
        Dim tb As TextBox = CType(sender, TextBox)

        If tb.SelectionStart = 0 Then
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        Else
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                If e.KeyChar = "-"c Then
                    If tb.Text.Contains("-") Then
                        e.Handled = True
                    End If
                Else
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim categoryName As String = txtBrand.Text.Trim()

        If String.IsNullOrWhiteSpace(categoryName) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If categoryName.Length < 2 Then
            MessageBox.Show("Category name must be at least 2 characters long.", "Invalid Category", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If categoryName.Contains("  ") Then
            MessageBox.Show("Category name cannot contain multiple consecutive spaces.", "Invalid Category", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate(categoryName) Then
            MessageBox.Show("Category already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this category?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            _service.SaveCategory(Mode, SelectedId, categoryName)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Category.", "Added Category."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
