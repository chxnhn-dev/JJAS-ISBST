Module Utilities
    Public Sub BlockCopyPaste(tb As TextBox)
        ' Block keyboard shortcuts
        AddHandler tb.KeyDown, Sub(s, e)
                                   If e.Control AndAlso (e.KeyCode = Keys.C OrElse e.KeyCode = Keys.V OrElse e.KeyCode = Keys.X) Then
                                       e.SuppressKeyPress = True
                                       e.Handled = True
                                   End If
                               End Sub

        ' Block right-click menu
        tb.ContextMenu = New ContextMenu()
    End Sub
    Public Sub BlockCopyPaste(ctrl As TextBoxBase)
        ' Block keyboard shortcuts
        AddHandler ctrl.KeyDown, Sub(s, e)
                                     If e.Control AndAlso (e.KeyCode = Keys.C OrElse e.KeyCode = Keys.V OrElse e.KeyCode = Keys.X) Then
                                         e.SuppressKeyPress = True
                                         e.Handled = True
                                     End If
                                 End Sub

        ' Block right-click menu
        ctrl.ContextMenu = New ContextMenu()
    End Sub
End Module
