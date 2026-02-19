Public Class Add_Deliveries
    Public Property deliveriesid As Integer = -1

    Private Sub Add_Deliveries_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmDeliveryEntry With {
            .Mode = If(deliveriesid > 0, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = deliveriesid
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
