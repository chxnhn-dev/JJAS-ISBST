Public Class Add_Supplier
    Public Property SupplierId As Integer?

    Private Sub Add_Supplier_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Using modal As New FrmSupplierEntry With {
            .Mode = If(SupplierId.HasValue, EntryFormMode.EditExisting, EntryFormMode.AddNew),
            .SelectedId = If(SupplierId.HasValue, SupplierId.Value, -1)
        }
            Me.DialogResult = modal.ShowDialog(Me)
        End Using

        Me.Close()
    End Sub
End Class
