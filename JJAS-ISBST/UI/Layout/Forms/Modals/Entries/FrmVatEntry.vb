Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms

Public Class FrmVATEntry
    Private _modernUiInitialized As Boolean
    Private _lblSubtitle As Label
    Private _pnlFields As Panel
    Private _pnlActions As Panel
    Private _btnCloseProxy As Guna2Button
    Private _btnCancelProxy As Guna2Button
    Private _btnClearProxy As Guna2Button
    Private _btnPrimaryProxy As Guna2Button
    Private _txtVatRateProxy As Guna2TextBox
    Private _isSyncingFields As Boolean

    Public Sub New()
        InitializeComponent()

        Dim fixedSize As New Size(820, 520)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize

        InitializeModernUiIfNeeded()
    End Sub

    Public Property VatID As Integer
    Public Property VatRate As Decimal

    Private Sub Update_Vat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
        ModalEntryAnimator.PrepareForOpenAnimation(Me)

        If _txtVatRateProxy IsNot Nothing Then BlockCopyPaste(_txtVatRateProxy)

        Me.Text = "Update Vat"
        Label12.Text = "Update Vat"
        btnUpdate.Text = "Update"

        txtVatrate.Text = VatRate.ToString()
        SyncLegacyToModernFields()
        SyncPrimaryButtonFromLegacy()
        RefreshVatVisuals()
    End Sub

    Private Sub FrmVATEntry_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ModalEntryAnimator.PlayOpenAnimation(Me)
    End Sub

    Private Sub CenterModalToOwnerOrScreen()
        StartPosition = If(Owner IsNot Nothing, FormStartPosition.CenterParent, FormStartPosition.CenterScreen)
    End Sub

    Private Sub txtVatrate_Keypress(sender As Object, e As KeyPressEventArgs) Handles txtVatrate.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If txtVatrate.Text.Contains(".") Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else
            MsgBox("Vat must be numbers only.", vbCritical, "Invalid Input")
            e.Handled = True
        End If
    End Sub

    Private Sub txtVatRateProxy_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If _txtVatRateProxy IsNot Nothing AndAlso _txtVatRateProxy.Text.Contains(".") Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else
            MsgBox("Vat must be numbers only.", vbCritical, "Invalid Input")
            e.Handled = True
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        SyncModernToLegacyFields()

        If String.IsNullOrWhiteSpace(txtVatrate.Text) Then
            MessageBox.Show("Please enter a VAT rate!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusVatRateField()
            Exit Sub
        End If

        Dim vatValue As Decimal
        If Not Decimal.TryParse(txtVatrate.Text, vatValue) Then
            MessageBox.Show("VAT rate must be a number!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusVatRateField()
            Exit Sub
        End If

        If vatValue < 0 OrElse vatValue > 100 Then
            MessageBox.Show("VAT rate must be between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            FocusVatRateField()
            Exit Sub
        End If

        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            cmd = New SqlCommand("UPDATE tbl_vat SET vat_rate=@vat_rate, Dateupdated=@DateUpdated WHERE vatid=@vatid", conn)
            cmd.Parameters.AddWithValue("@vat_rate", vatValue)
            cmd.Parameters.AddWithValue("@Dateupdated", DateTime.Now)
            cmd.Parameters.AddWithValue("@vatid", VatID)
            cmd.ExecuteNonQuery()

            MessageBox.Show("VAT updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            MessageBox.Show("Error while updating VAT: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
            Me.Close()
        End Try
    End Sub

    Private Sub FocusVatRateField()
        If _txtVatRateProxy IsNot Nothing AndAlso _txtVatRateProxy.Visible Then
            _txtVatRateProxy.Focus()
        Else
            txtVatrate.Focus()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class
