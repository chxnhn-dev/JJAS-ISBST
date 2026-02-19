Public Class Add_Category
    Public Property CategoryID As Integer?

    Private Sub Add_Category_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmCategoryEntry With {
            .Mode = If(CategoryID.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(CategoryID.HasValue, CategoryID.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
