Imports System.Windows.Forms

Namespace FileMaintenance
    Friend NotInheritable Class EntryModalHost
        Private Sub New()
        End Sub

        Public Shared Function ShowLikeUserProduct(owner As Form, entryForm As Form) As DialogResult
            If entryForm Is Nothing OrElse entryForm.IsDisposed Then
                Return DialogResult.Cancel
            End If

            If owner Is Nothing OrElse owner.IsDisposed OrElse Not owner.Visible Then
                entryForm.StartPosition = FormStartPosition.CenterScreen
                Return entryForm.ShowDialog()
            End If

            entryForm.StartPosition = FormStartPosition.CenterParent
            entryForm.Owner = owner
            Return entryForm.ShowDialog(owner)
        End Function
    End Class
End Namespace
