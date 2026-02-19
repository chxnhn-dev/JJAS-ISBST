Public Class Add_Brand
    Public Property BrandID As Integer?

    Private Sub Add_Brand_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmBrandEntry With {
            .Mode = If(BrandID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(BrandID.HasValue, BrandID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
