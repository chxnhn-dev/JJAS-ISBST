Imports System.Drawing
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Public Class FrmBrandEntry
    Private ReadOnly _brandService As New BrandService()
    Private ReadOnly _categoryService As New CategoryService()
    Private ReadOnly _colorService As New ColorService()

    Private _modernUiInitialized As Boolean
    Private _lblSubtitle As Label
    Private _pnlFields As Panel
    Private _pnlActions As Panel
    Private _btnCloseProxy As Guna2Button
    Private _btnCancelProxy As Guna2Button
    Private _btnClearProxy As Guna2Button

    Public Sub New()
        InitializeComponent()

        Dim fixedSize As New Size(760, 400)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        InitializeModernUiIfNeeded()
    End Sub

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1
    Private Property EntryType As SharedEntryType = SharedEntryType.Brand

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return Mode = EntryFormMode.EditExisting AndAlso SelectedId > 0
        End Get
    End Property

    Private ReadOnly Property EntityDisplayName As String
        Get
            Select Case EntryType
                Case SharedEntryType.Brand
                    Return "Brand"
                Case SharedEntryType.Category
                    Return "Category"
                Case Else
                    Return "Color"
            End Select
        End Get
    End Property

    Private ReadOnly Property EntityColumnName As String
        Get
            Select Case EntryType
                Case SharedEntryType.Brand
                    Return "Brand"
                Case SharedEntryType.Category
                    Return "Category"
                Case Else
                    Return "Color"
            End Select
        End Get
    End Property

    Private ReadOnly Property PlaceholderValue As String
        Get
            Select Case EntryType
                Case SharedEntryType.Brand
                    Return "Nike"
                Case SharedEntryType.Category
                    Return "Shoes"
                Case Else
                    Return "Black"
            End Select
        End Get
    End Property

    Private Sub FrmBrandEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        BlockCopyPaste(txtValue)
        ConfigureMode()

        If IsEditMode Then
            LoadEntryForEdit()
        End If

        RefreshSharedEntryVisuals()
    End Sub

    Private Sub FrmBrandEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub CenterModalToOwnerOrScreen()
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
    End Sub

    Private Sub ConfigureMode()
        Dim actionLabel As String = If(IsEditMode, "Edit", "Add")
        Dim entityName As String = EntityDisplayName

        Me.Text = $"{actionLabel} {entityName}"
        Label12.Text = $"{actionLabel} {entityName}"
        EentryName.Text = $"{entityName}:"
        txtValue.PlaceholderText = PlaceholderValue

        btnadd.Text = "Save"
        btnedit.Text = "Update"
        btnadd.Visible = Not IsEditMode
        btnedit.Visible = IsEditMode

        If _lblSubtitle IsNot Nothing Then
            _lblSubtitle.Text = "Fill in the details below"
        End If

        If IsEditMode Then
            btnedit.BringToFront()
        Else
            txtValue.Clear()
            btnadd.BringToFront()
        End If

        LayoutFieldControls()
        LayoutActionButtons()
        RefreshSharedEntryVisuals()
    End Sub

    Private Function GetRowById(entryId As Integer) As DataRow
        Select Case EntryType
            Case SharedEntryType.Brand
                Return _brandService.GetBrandById(entryId)
            Case SharedEntryType.Category
                Return _categoryService.GetCategoryById(entryId)
            Case Else
                Return _colorService.GetColorById(entryId)
        End Select
    End Function

    Private Sub LoadEntryForEdit()
        Dim row As DataRow = GetRowById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show($"Selected {EntityDisplayName.ToLowerInvariant()} record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        txtValue.Text = Convert.ToString(row(EntityColumnName))
        ForceImmediateFieldPaint()
    End Sub

    Private Function IsDuplicate(entryValue As String) As Boolean
        Select Case EntryType
            Case SharedEntryType.Brand
                Return _brandService.IsDuplicateName(entryValue, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
            Case SharedEntryType.Category
                Return _categoryService.IsDuplicateName(entryValue, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
            Case Else
                Return _colorService.IsDuplicateName(entryValue, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
        End Select
    End Function

    Private Shared Sub HandleBrandKeyPress(currentText As String, e As KeyPressEventArgs)
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = False
            Exit Sub
        End If

        Dim allowedSymbols As String = "&'+-"
        If allowedSymbols.Contains(e.KeyChar) Then
            e.Handled = (currentText.IndexOfAny(allowedSymbols.ToCharArray()) <> -1)
        Else
            e.Handled = True
        End If
    End Sub

    Private Shared Sub HandleCategoryOrColorKeyPress(currentText As String, selectionStart As Integer, e As KeyPressEventArgs)
        If selectionStart = 0 Then
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        Else
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                If e.KeyChar = "-"c Then
                    If currentText.Contains("-") Then
                        e.Handled = True
                    End If
                Else
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub txtValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValue.KeyPress
        Dim currentText As String = txtValue.Text
        Dim selectionStart As Integer = txtValue.SelectionStart

        Select Case EntryType
            Case SharedEntryType.Brand
                HandleBrandKeyPress(currentText, e)
            Case Else
                HandleCategoryOrColorKeyPress(currentText, selectionStart, e)
        End Select
    End Sub

    Private Sub SaveEntry(entryValue As String)
        Select Case EntryType
            Case SharedEntryType.Brand
                _brandService.SaveBrand(Mode, SelectedId, entryValue)
            Case SharedEntryType.Category
                _categoryService.SaveCategory(Mode, SelectedId, entryValue)
            Case Else
                _colorService.SaveColor(Mode, SelectedId, entryValue)
        End Select
    End Sub

    Private Sub SubmitEntry()
        Dim entryValue As String = txtValue.Text.Trim()
        Dim entityNameLower As String = EntityDisplayName.ToLowerInvariant()

        If String.IsNullOrWhiteSpace(entryValue) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If entryValue.Length < 2 Then
            MessageBox.Show($"{EntityDisplayName} name must be at least 2 characters long.", $"Invalid {EntityDisplayName}", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If entryValue.Contains("  ") Then
            MessageBox.Show($"{EntityDisplayName} name cannot contain multiple consecutive spaces.", $"Invalid {EntityDisplayName}", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate(entryValue) Then
            MessageBox.Show($"{EntityDisplayName} already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this {entityNameLower}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            SaveEntry(entryValue)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, $"Edited {EntityDisplayName}.", $"Added {EntityDisplayName}."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"An error occurred while saving {entityNameLower}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        SubmitEntry()
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        SubmitEntry()
    End Sub

    Private Sub btnexit_Click(sender As Object, e As EventArgs) Handles btnexit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
