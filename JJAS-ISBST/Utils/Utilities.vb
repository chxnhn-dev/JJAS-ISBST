Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Data

Public Class PagedQueryRequest
    Public Property PageSize As Integer
    Public Property PageIndex As Integer
    Public Property SearchText As String
    Public Property DateFrom As DateTime?
    Public Property DateTo As DateTime?
    Public Property Filters As IDictionary(Of String, Object)

    Public Sub New()
        PageSize = 10
        PageIndex = 1
        SearchText = String.Empty
        Filters = New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
    End Sub

    Public ReadOnly Property Offset As Integer
        Get
            Dim safePageSize As Integer = Math.Max(1, PageSize)
            Dim safePageIndex As Integer = Math.Max(1, PageIndex)
            Return (safePageIndex - 1) * safePageSize
        End Get
    End Property

    Public Function Clone() As PagedQueryRequest
        Dim clonedRequest As New PagedQueryRequest() With {
            .PageSize = Math.Max(1, PageSize),
            .PageIndex = Math.Max(1, PageIndex),
            .SearchText = If(SearchText, String.Empty),
            .DateFrom = DateFrom,
            .DateTo = DateTo
        }

        If Filters IsNot Nothing Then
            For Each pair As KeyValuePair(Of String, Object) In Filters
                clonedRequest.Filters(pair.Key) = pair.Value
            Next
        End If

        Return clonedRequest
    End Function
End Class

Public Class PagedQueryResult
    Public Property Records As DataTable
    Public Property PageSize As Integer
    Public Property PageIndex As Integer
    Public Property TotalRecords As Integer
    Public Property SearchText As String

    Public ReadOnly Property TotalPages As Integer
        Get
            Dim safePageSize As Integer = Math.Max(1, PageSize)
            Dim safeTotalRecords As Integer = Math.Max(0, TotalRecords)
            Return Math.Max(1, CInt(Math.Ceiling(safeTotalRecords / CDbl(safePageSize))))
        End Get
    End Property

    Public Shared Function Create(records As DataTable, totalRecords As Integer, request As PagedQueryRequest) As PagedQueryResult
        Dim safeRequest As PagedQueryRequest = If(request, New PagedQueryRequest())
        Return New PagedQueryResult() With {
            .Records = If(records, New DataTable()),
            .PageSize = Math.Max(1, safeRequest.PageSize),
            .PageIndex = Math.Max(1, safeRequest.PageIndex),
            .TotalRecords = Math.Max(0, totalRecords),
            .SearchText = If(safeRequest.SearchText, String.Empty)
        }
    End Function

    Public Shared Function Empty(request As PagedQueryRequest) As PagedQueryResult
        Return Create(New DataTable(), 0, request)
    End Function
End Class

Public Class PaginationState
    Private _pageSize As Integer
    Private _pageIndex As Integer
    Private _totalRecords As Integer

    Public Sub New(defaultPageSize As Integer)
        _pageSize = Math.Max(1, defaultPageSize)
        _pageIndex = 1
        _totalRecords = 0
    End Sub

    Public ReadOnly Property PageSize As Integer
        Get
            Return _pageSize
        End Get
    End Property

    Public ReadOnly Property PageIndex As Integer
        Get
            Return _pageIndex
        End Get
    End Property

    Public ReadOnly Property TotalRecords As Integer
        Get
            Return _totalRecords
        End Get
    End Property

    Public ReadOnly Property TotalPages As Integer
        Get
            Return Math.Max(1, CInt(Math.Ceiling(Math.Max(0, _totalRecords) / CDbl(Math.Max(1, _pageSize)))))
        End Get
    End Property

    Public ReadOnly Property CanGoPrevious As Boolean
        Get
            Return _pageIndex > 1
        End Get
    End Property

    Public ReadOnly Property CanGoNext As Boolean
        Get
            Return _pageIndex < TotalPages
        End Get
    End Property

    Public Sub ResetToFirstPage()
        _pageIndex = 1
    End Sub

    Public Sub SetPageIndex(value As Integer)
        _pageIndex = Math.Max(1, value)
        ClampPageIndex()
    End Sub

    Public Function TryMovePrevious() As Boolean
        If Not CanGoPrevious Then Return False
        _pageIndex -= 1
        Return True
    End Function

    Public Function TryMoveNext() As Boolean
        If Not CanGoNext Then Return False
        _pageIndex += 1
        Return True
    End Function

    Public Sub ApplyResult(result As PagedQueryResult)
        If result Is Nothing Then
            _totalRecords = 0
            ClampPageIndex()
            Return
        End If

        If result.PageSize > 0 Then
            _pageSize = result.PageSize
        End If

        _totalRecords = Math.Max(0, result.TotalRecords)
        _pageIndex = Math.Max(1, result.PageIndex)
        ClampPageIndex()
    End Sub

    Public Function BuildRequest(searchText As String,
                                 Optional dateFrom As DateTime? = Nothing,
                                 Optional dateTo As DateTime? = Nothing,
                                 Optional filters As IDictionary(Of String, Object) = Nothing) As PagedQueryRequest
        Dim request As New PagedQueryRequest() With {
            .PageSize = Math.Max(1, _pageSize),
            .PageIndex = Math.Max(1, _pageIndex),
            .SearchText = If(searchText, String.Empty),
            .DateFrom = dateFrom,
            .DateTo = dateTo
        }

        If filters IsNot Nothing Then
            For Each pair As KeyValuePair(Of String, Object) In filters
                request.Filters(pair.Key) = pair.Value
            Next
        End If

        Return request
    End Function

    Public Function GetPageLabel() As String
        Return $"Page {_pageIndex} of {TotalPages}"
    End Function

    Private Sub ClampPageIndex()
        If _pageIndex < 1 Then
            _pageIndex = 1
            Return
        End If

        Dim maxPage As Integer = TotalPages
        If _pageIndex > maxPage Then
            _pageIndex = maxPage
        End If
    End Sub
End Class

Module Utilities
    Private ReadOnly HiddenGridColumns As String() = {
        "AuditID",
        "BrandID",
        "CategoryID",
        "ColorID",
        "DeliveryID",
        "DeliveryProductID",
        "DiscountID",
        "ImagePath",
        "ProductID",
        "SaleID",
        "SizeID",
        "SupplierID",
        "TempRowID",
        "TempRowId",
        "UserID",
        "VatID",
        "VendorID"
    }

    Public Function ResolveSidebarFirstName() As String
        Dim firstName As String = GetFirstWord(FrmLogin.CurrentUser.FullName)
        If firstName.Length = 0 Then
            firstName = GetFirstWord(SessionContext.FullName)
        End If

        If firstName.Length = 0 Then
            firstName = GetFirstWord(FrmLogin.CurrentUser.Username)
        End If

        If firstName.Length = 0 Then
            firstName = GetFirstWord(SessionContext.Username)
        End If

        If firstName.Length = 0 Then
            firstName = "User"
        End If

        Return firstName
    End Function

    Public Function ResolveSidebarRoleDisplay() As String
        Dim roleValue As String = If(FrmLogin.CurrentUser.Role, String.Empty).Trim()
        If roleValue.Length = 0 Then
            roleValue = If(SessionContext.Role, String.Empty).Trim()
        End If

        Select Case roleValue.ToLowerInvariant()
            Case "admin", "administrator"
                Return "Administrator"
            Case "staff"
                Return "Staff"
            Case "cashier"
                Return "Cashier"
            Case Else
                If roleValue.Length = 0 Then
                    Return "User"
                End If

                Return Char.ToUpperInvariant(roleValue(0)) & roleValue.Substring(1).ToLowerInvariant()
        End Select
    End Function

    Private Function GetFirstWord(value As String) As String
        Dim normalized As String = If(value, String.Empty).Trim()
        If normalized.Length = 0 Then Return String.Empty

        Dim parts As String() = normalized.Split(New Char() {" "c, ControlChars.Tab}, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length = 0 Then Return String.Empty

        Return parts(0).Trim()
    End Function

    Public Sub ApplyCashierSidebarNavigationOrder(sidebarContainer As Control,
                                                  btnHome As Control,
                                                  btnPos As Control,
                                                  btnInventory As Control,
                                                  btnDiscount As Control,
                                                  btnTransaction As Control,
                                                  btnReports As Control,
                                                  btnLogout As Control)
        If sidebarContainer Is Nothing Then Exit Sub

        Dim orderedButtons As Control() = {
            btnHome,
            btnPos,
            btnInventory,
            btnDiscount,
            btnTransaction,
            btnReports,
            btnLogout
        }

        For Each button As Control In orderedButtons
            If button Is Nothing Then Continue For
            If Not sidebarContainer.Controls.Contains(button) Then Continue For

            button.Dock = DockStyle.Top
            button.BringToFront()
        Next
    End Sub

    Public Sub ApplyStaffSidebarNavigationOrder(sidebarContainer As Control,
                                                btnHome As Control,
                                                btnFileMaintenance As Control,
                                                btnDelivery As Control,
                                                btnReturns As Control,
                                                btnInventory As Control,
                                                btnTransaction As Control,
                                                btnReports As Control,
                                                btnLogout As Control)
        If sidebarContainer Is Nothing Then Exit Sub

        Dim orderedButtons As Control() = {
            btnHome,
            btnFileMaintenance,
            btnDelivery,
            btnReturns,
            btnInventory,
            btnTransaction,
            btnReports,
            btnLogout
        }

        For Each button As Control In orderedButtons
            If button Is Nothing Then Continue For
            If Not sidebarContainer.Controls.Contains(button) Then Continue For

            button.Dock = DockStyle.Top
            button.BringToFront()
        Next
    End Sub

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

    Public Sub BlockCopyPaste(ctrl As Guna.UI2.WinForms.Guna2TextBox)
        AddHandler ctrl.KeyDown, Sub(s, e)
                                     If e.Control AndAlso (e.KeyCode = Keys.C OrElse e.KeyCode = Keys.V OrElse e.KeyCode = Keys.X) Then
                                         e.SuppressKeyPress = True
                                         e.Handled = True
                                     End If
                                 End Sub

        ctrl.ContextMenuStrip = New ContextMenuStrip()
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

        HideGridColumnsByName(dgv)
        EnsureGridHeadersVisible(dgv)

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

        Dim visibleColumns As List(Of DataGridViewColumn) = dgv.Columns.Cast(Of DataGridViewColumn)().
            Where(Function(col) col.Visible).
            OrderBy(Function(col) col.DisplayIndex).
            ToList()

        Dim buttonColumns As List(Of DataGridViewButtonColumn) = visibleColumns.
            OfType(Of DataGridViewButtonColumn)().
            ToList()
        For Each btnCol As DataGridViewButtonColumn In buttonColumns
            ConfigureButtonColumn(btnCol, buttonWidth)
        Next

        If buttonColumns.Count > 0 Then
            Dim nonButtonColumns As List(Of DataGridViewColumn) = visibleColumns.
                Where(Function(col) Not TypeOf col Is DataGridViewButtonColumn).
                ToList()

            Dim orderedColumns As New List(Of DataGridViewColumn)(nonButtonColumns.Count + buttonColumns.Count)
            orderedColumns.AddRange(nonButtonColumns)
            orderedColumns.AddRange(buttonColumns.Cast(Of DataGridViewColumn)())

            For i As Integer = 0 To orderedColumns.Count - 1
                If orderedColumns(i).DisplayIndex <> i Then
                    orderedColumns(i).DisplayIndex = i
                End If
            Next
        End If
    End Sub

    Private Sub HandleGridColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        If e Is Nothing OrElse e.Column Is Nothing Then Exit Sub

        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable

        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        HideGridColumnsByName(dgv)

        Dim btnCol As DataGridViewButtonColumn = TryCast(e.Column, DataGridViewButtonColumn)
        If btnCol IsNot Nothing Then
            ConfigureButtonColumn(btnCol, 100)
        End If
    End Sub

    Private Sub HideGridColumnsByName(dgv As DataGridView)
        If dgv Is Nothing OrElse dgv.Columns Is Nothing Then Exit Sub

        For Each columnName As String In HiddenGridColumns
            If dgv.Columns.Contains(columnName) Then
                dgv.Columns(columnName).Visible = False
            End If
        Next
    End Sub

    Private Sub EnsureGridHeadersVisible(dgv As DataGridView)
        If dgv Is Nothing Then Exit Sub

        dgv.ColumnHeadersVisible = True
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        If dgv.ColumnHeadersHeight < 28 Then
            dgv.ColumnHeadersHeight = 34
        End If

        dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

        Dim gunaGrid As Guna.UI2.WinForms.Guna2DataGridView = TryCast(dgv, Guna.UI2.WinForms.Guna2DataGridView)
        If gunaGrid IsNot Nothing Then
            gunaGrid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            gunaGrid.ThemeStyle.HeaderStyle.Height = dgv.ColumnHeadersHeight
            gunaGrid.ThemeStyle.HeaderStyle.BackColor = System.Drawing.SystemColors.Control
            gunaGrid.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black
        End If
    End Sub

    Private Sub ConfigureButtonColumn(btnCol As DataGridViewButtonColumn, buttonWidth As Integer)
        If btnCol Is Nothing Then Exit Sub

        btnCol.ReadOnly = False
        btnCol.Width = If(btnCol.Width > 0, btnCol.Width, buttonWidth)
        btnCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        btnCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        ' Keep per-row button values when a column intentionally uses dynamic text.
        If btnCol.UseColumnTextForButtonValue AndAlso String.IsNullOrWhiteSpace(btnCol.Text) Then
            btnCol.Text = If(String.IsNullOrWhiteSpace(btnCol.HeaderText), "Action", btnCol.HeaderText)
        End If
        btnCol.FlatStyle = FlatStyle.Popup
        btnCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        btnCol.DefaultCellStyle.Padding = New Padding(6, 4, 6, 4)
        btnCol.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
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

Module GridHelpers
    Public Function IsGridReady(dgv As DataGridView) As Boolean
        Return dgv IsNot Nothing AndAlso dgv.Columns IsNot Nothing AndAlso dgv.ColumnCount > 0
    End Function

    Public Function TryGetColumn(dgv As DataGridView,
                                 ByRef column As DataGridViewColumn,
                                 columnName As String,
                                 ParamArray alternateColumnNames() As String) As Boolean
        column = Nothing
        If Not IsGridReady(dgv) Then Return False

        Dim namesToCheck As New List(Of String)()
        AddUniqueColumnName(namesToCheck, columnName)

        If alternateColumnNames IsNot Nothing Then
            For Each name As String In alternateColumnNames
                AddUniqueColumnName(namesToCheck, name)
            Next
        End If

        For Each name As String In namesToCheck
            If dgv.Columns.Contains(name) Then
                column = dgv.Columns(name)
                Return column IsNot Nothing
            End If
        Next

        Return False
    End Function

    Public Sub ApplyColumnSetup(dgv As DataGridView,
                                columnName As String,
                                action As Action(Of DataGridViewColumn),
                                ParamArray alternateColumnNames() As String)
        If action Is Nothing Then Exit Sub

        Dim column As DataGridViewColumn = Nothing
        If TryGetColumn(dgv, column, columnName, alternateColumnNames) Then
            action(column)
        End If
    End Sub

    Public Sub ApplyColumnSetup(dgv As DataGridView,
                                actions As IDictionary(Of String, Action(Of DataGridViewColumn)),
                                Optional aliasMap As IDictionary(Of String, String()) = Nothing)
        If actions Is Nothing OrElse actions.Count = 0 Then Exit Sub
        If Not IsGridReady(dgv) Then Exit Sub

        For Each entry As KeyValuePair(Of String, Action(Of DataGridViewColumn)) In actions
            Dim aliases As String() = Nothing
            If aliasMap IsNot Nothing AndAlso aliasMap.ContainsKey(entry.Key) Then
                aliases = aliasMap(entry.Key)
            End If

            If aliases Is Nothing Then
                ApplyColumnSetup(dgv, entry.Key, entry.Value)
            Else
                ApplyColumnSetup(dgv, entry.Key, entry.Value, aliases)
            End If
        Next
    End Sub

    Public Function GetColumnNameByIndex(dgv As DataGridView, columnIndex As Integer) As String
        If dgv Is Nothing OrElse dgv.Columns Is Nothing Then Return String.Empty
        If columnIndex < 0 OrElse columnIndex >= dgv.ColumnCount Then Return String.Empty

        Dim column As DataGridViewColumn = dgv.Columns(columnIndex)
        If column Is Nothing Then Return String.Empty

        Return column.Name
    End Function

    Private Sub AddUniqueColumnName(target As IList(Of String), columnName As String)
        If target Is Nothing Then Exit Sub
        If String.IsNullOrWhiteSpace(columnName) Then Exit Sub
        If target.Contains(columnName) Then Exit Sub
        target.Add(columnName)
    End Sub
End Module
