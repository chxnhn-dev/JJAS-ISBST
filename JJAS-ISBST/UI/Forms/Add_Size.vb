Public Class Add_Measurement
    Public Property SizeID As Integer?

    Private Sub Add_Size_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmSizeEntry With {
            .Mode = If(SizeID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(SizeID.HasValue, SizeID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
