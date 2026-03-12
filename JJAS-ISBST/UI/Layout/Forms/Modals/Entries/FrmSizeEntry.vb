Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Public Class FrmSizeEntry
    Private ReadOnly _service As New SizeService()

    Private _modernUiInitialized As Boolean
    Private _lblSubtitle As Label
    Private _lblCategory As Guna2HtmlLabel
    Private _pnlFields As Panel
    Private _pnlActions As Panel
    Private _btnCloseProxy As Guna2Button
    Private _btnCancelProxy As Guna2Button
    Private _btnClearProxy As Guna2Button
    Private cmbCategory As Guna2ComboBox

    Public Sub New()
        InitializeComponent()

        Dim fixedSize As New Size(820, 520)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        InitializeModernUiIfNeeded()
    End Sub

    Public Property Mode As EntryFormMode = EntryFormMode.AddNew
    Public Property SelectedId As Integer = -1

    Public Property SizeID As Integer?
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

    Private Sub FrmSizeEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        BlockCopyPaste(txtSize)
        BlockCopyPaste(txtDescription)

        ConfigureMode()
        LoadCategories()

        If IsEditMode Then
            LoadSizeForEdit()
        End If

        RefreshSizeEntryVisuals()
    End Sub

    Private Sub FrmSizeEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub CenterModalToOwnerOrScreen()
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
    End Sub

    Private Sub ConfigureMode()
        Dim actionLabel As String = If(IsEditMode, "Edit", "Add")

        Me.Text = $"{actionLabel} Size"
        Label12.Text = $"{actionLabel} Size"
        s.Text = "Size:"
        Label1.Text = "Description (Optional):"
        txtSize.PlaceholderText = "Small"
        txtDescription.PlaceholderText = "Small description"

        btnAdd.Text = "Save"
        btnEdit.Text = "Update"
        btnAdd.Visible = Not IsEditMode
        btnEdit.Visible = IsEditMode

        If _lblSubtitle IsNot Nothing Then
            _lblSubtitle.Text = "Fill in the details below"
        End If

        If IsEditMode Then
            btnEdit.BringToFront()
        Else
            txtSize.Clear()
            txtDescription.Clear()
            btnAdd.BringToFront()
        End If

        LayoutFieldControls()
        LayoutActionButtons()
        RefreshSizeEntryVisuals()
    End Sub

    Private Sub LoadSizeForEdit()
        Dim row As DataRow = _service.GetSizeById(SelectedId)
        If row Is Nothing Then
            MessageBox.Show("Selected size record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
            Return
        End If

        If cmbCategory IsNot Nothing AndAlso row.Table.Columns.Contains("CategoryID") AndAlso Not IsDBNull(row("CategoryID")) Then
            cmbCategory.SelectedValue = Convert.ToInt32(row("CategoryID"))
        End If
        txtSize.Text = row("Size").ToString()
        txtDescription.Text = row("Description").ToString()
        ForceImmediateFieldPaint()
    End Sub

    Private Function IsDuplicate(categoryId As Integer, sizeName As String) As Boolean
        Return _service.IsDuplicateName(categoryId, sizeName, If(IsEditMode, CType(SelectedId, Integer?), Nothing))
    End Function

    Private Function ValidateEntry(categoryId As Integer, sizeValue As String) As Boolean
        If cmbCategory Is Nothing OrElse cmbCategory.SelectedIndex < 0 Then
            MessageBox.Show("Please select a category.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If categoryId <= 0 Then
            MessageBox.Show("Please select a category.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If String.IsNullOrWhiteSpace(sizeValue) Then
            MessageBox.Show("Size is required.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If IsDuplicate(categoryId, sizeValue) Then
            MessageBox.Show("This size already exists for the selected category.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    Private Sub SaveEntry(categoryId As Integer, sizeValue As String, description As String)
        _service.SaveSize(Mode, SelectedId, categoryId, sizeValue, description)
    End Sub

    Private Sub SubmitEntry()
        Dim categoryId As Integer = GetSelectedCategoryId()
        Dim sizeValue As String = txtSize.Text.Trim()
        Dim description As String = txtDescription.Text.Trim()

        If Not ValidateEntry(categoryId, sizeValue) Then
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this size?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            SaveEntry(categoryId, sizeValue, description)

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Size.", "Added Size."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As SqlException When ex.Number = 2601 OrElse ex.Number = 2627
            MessageBox.Show("This size already exists for the selected category.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving size: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        SubmitEntry()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        SubmitEntry()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
