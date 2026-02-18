Imports System.Windows.Forms

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

    Public Sub ApplyStandardGridLayout(dgv As DataGridView, Optional rowHeight As Integer = 38, Optional buttonWidth As Integer = 100)
        If dgv Is Nothing Then Exit Sub

        RemoveHandler dgv.ColumnAdded, AddressOf HandleGridColumnAdded
        AddHandler dgv.ColumnAdded, AddressOf HandleGridColumnAdded
        RemoveHandler dgv.CellMouseMove, AddressOf HandleGridCellMouseMove
        AddHandler dgv.CellMouseMove, AddressOf HandleGridCellMouseMove
        RemoveHandler dgv.MouseLeave, AddressOf HandleGridMouseLeave
        AddHandler dgv.MouseLeave, AddressOf HandleGridMouseLeave
        RemoveHandler dgv.CellToolTipTextNeeded, AddressOf HandleGridCellToolTipTextNeeded
        AddHandler dgv.CellToolTipTextNeeded, AddressOf HandleGridCellToolTipTextNeeded

        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgv.RowTemplate.Height = rowHeight

        For Each row As DataGridViewRow In dgv.Rows
            If Not row.IsNewRow Then
                row.Height = rowHeight
            End If
        Next

        dgv.DefaultCellStyle.Alignment = NormalizeVerticalAlignment(dgv.DefaultCellStyle.Alignment)
        dgv.RowsDefaultCellStyle.Alignment = NormalizeVerticalAlignment(dgv.RowsDefaultCellStyle.Alignment)

        For Each col As DataGridViewColumn In dgv.Columns
            col.DefaultCellStyle.Alignment = NormalizeVerticalAlignment(col.DefaultCellStyle.Alignment)
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        Dim buttonColumns As New System.Collections.Generic.List(Of DataGridViewButtonColumn)()
        For Each col As DataGridViewColumn In dgv.Columns
            Dim btnCol As DataGridViewButtonColumn = TryCast(col, DataGridViewButtonColumn)
            If btnCol IsNot Nothing AndAlso btnCol.Visible Then
                btnCol.ReadOnly = False
                btnCol.Width = buttonWidth
                btnCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                btnCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                btnCol.FlatStyle = FlatStyle.Popup
                buttonColumns.Add(btnCol)
            End If
        Next

        If buttonColumns.Count > 0 Then
            buttonColumns.Sort(Function(a, b) a.DisplayIndex.CompareTo(b.DisplayIndex))
            Dim startIndex As Integer = dgv.Columns.Count - buttonColumns.Count
            For i As Integer = 0 To buttonColumns.Count - 1
                buttonColumns(i).DisplayIndex = startIndex + i
            Next
        End If
    End Sub

    Private Sub HandleGridColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        If e Is Nothing OrElse e.Column Is Nothing Then Exit Sub

        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable

        Dim btnCol As DataGridViewButtonColumn = TryCast(e.Column, DataGridViewButtonColumn)
        If btnCol IsNot Nothing Then
            btnCol.ReadOnly = False
            btnCol.FlatStyle = FlatStyle.Popup
        End If
    End Sub

    Private Sub HandleGridCellMouseMove(sender As Object, e As DataGridViewCellMouseEventArgs)
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If dgv Is Nothing Then Exit Sub

        If IsButtonCell(dgv, e.RowIndex, e.ColumnIndex) Then
            dgv.Cursor = Cursors.Hand
        Else
            dgv.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub HandleGridMouseLeave(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If dgv Is Nothing Then Exit Sub
        dgv.Cursor = Cursors.Default
    End Sub

    Private Sub HandleGridCellToolTipTextNeeded(sender As Object, e As DataGridViewCellToolTipTextNeededEventArgs)
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If dgv Is Nothing Then Exit Sub

        If Not IsButtonCell(dgv, e.RowIndex, e.ColumnIndex) Then Exit Sub

        Dim actionText As String = String.Empty
        Dim currentValue As Object = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        If currentValue IsNot Nothing Then
            actionText = currentValue.ToString().Trim()
        End If

        If String.IsNullOrWhiteSpace(actionText) Then
            actionText = dgv.Columns(e.ColumnIndex).HeaderText
        End If

        If String.IsNullOrWhiteSpace(actionText) Then
            actionText = "Open"
        End If

        e.ToolTipText = actionText & " (click)"
    End Sub

    Private Function IsButtonCell(dgv As DataGridView, rowIndex As Integer, columnIndex As Integer) As Boolean
        If dgv Is Nothing Then Return False
        If rowIndex < 0 OrElse columnIndex < 0 Then Return False
        If rowIndex >= dgv.Rows.Count OrElse columnIndex >= dgv.Columns.Count Then Return False

        Return TypeOf dgv.Columns(columnIndex) Is DataGridViewButtonColumn
    End Function

    Private Function NormalizeVerticalAlignment(alignment As DataGridViewContentAlignment) As DataGridViewContentAlignment
        Select Case alignment
            Case DataGridViewContentAlignment.TopLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.BottomLeft
                Return DataGridViewContentAlignment.MiddleLeft
            Case DataGridViewContentAlignment.TopCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.BottomCenter
                Return DataGridViewContentAlignment.MiddleCenter
            Case DataGridViewContentAlignment.TopRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.BottomRight
                Return DataGridViewContentAlignment.MiddleRight
            Case Else
                Return DataGridViewContentAlignment.MiddleLeft
        End Select
    End Function
End Module

