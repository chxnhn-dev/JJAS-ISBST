Public Class Add_User
    Public Property UserID As Integer?

    Private Sub Add_User_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmUserEntry With {
            .Mode = If(UserID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(UserID.HasValue, UserID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
