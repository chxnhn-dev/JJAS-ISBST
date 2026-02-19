Public Class Add_Discount
    Public Property DiscountID As Integer?

    Private Sub Add_Discount_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmDiscountEntry With {
            .Mode = If(DiscountID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(DiscountID.HasValue, DiscountID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
