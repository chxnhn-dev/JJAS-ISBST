Imports System.Data.SqlClient

Public Class FrmVATEntry
    Public Property VatID As Integer
    Public Property VatRate As Decimal
    Private Sub Update_Vat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtVatrate)


        txtVatrate.Text = VatRate
    End Sub
    Private Sub txtVatrate_Keypress(sender As Object, e As KeyPressEventArgs) Handles txtVatrate.KeyPress
        ' Allow digits, backspace, and dot
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then

            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If txtVatrate.Text.Contains(".") Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else
            ' ❌ Block everything else
            MsgBox("Vat must be numbers only.", vbCritical, "Invalid Input")
            e.Handled = True
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ' ✅ 1. Check if empty
        If String.IsNullOrWhiteSpace(txtVatrate.Text) Then
            MessageBox.Show("Please enter a VAT rate!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtVatrate.Focus()
            Exit Sub
        End If

        ' ✅ 2. Check if numeric
        Dim vatValue As Decimal
        If Not Decimal.TryParse(txtVatrate.Text, vatValue) Then
            MessageBox.Show("VAT rate must be a number!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtVatrate.Focus()
            Exit Sub
        End If

        ' ✅ 3. Check if within a reasonable range (example: 0–100%)
        If vatValue < 0 OrElse vatValue > 100 Then
            MessageBox.Show("VAT rate must be between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtVatrate.Focus()
            Exit Sub
        End If

        ' ✅ If validation passes, update record
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


    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class