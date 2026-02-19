Public Class Add_Color
    Public Property ColorID As Integer?

    Private Sub Add_Color_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmColorEntry With {
            .Mode = If(ColorID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(ColorID.HasValue, ColorID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
