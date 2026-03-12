Imports System.Collections.Generic

Public Class DeliveriesModuleForm
    Inherits ModuleListBaseForm

    Private Const ColDeliveryId As String = "DeliveryID"
    Private Const ColDeliveryNumber As String = "DeliveryNumber"
    Private Const ColOrderNumber As String = "OrderNumber"
    Private Const ColDeliveryDate As String = "DeliveryDate"
    Private Const ColCompanyName As String = "CompanyName"
    Private Const ColItemCount As String = "ItemCount"
    Private Const ColTotalQty As String = "TotalQty"
    Private Const ColStatus As String = "Status"
    Private Const ColPendingItemCount As String = "PendingItemCount"
    Private Const ColPostedItemCount As String = "PostedItemCount"
    Private Const ColImagePath As String = "ImagePath"

    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColPost As String = "colPost"
    Private Const ColDelete As String = "colDelete"
    Private Const PageSize As Integer = 10

    Private _deliveriesService As DeliveriesService
    Private _currentPage As Integer = 1
    Private _totalPages As Integer = 1
    Private _lastSearchText As String = String.Empty
    Private _deliveryGridColumnsConfigured As Boolean = False

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.DeliveriesTab
        End Get
    End Property

    Private Function GetDeliveriesService() As DeliveriesService
        If _deliveriesService Is Nothing Then
            _deliveriesService = New DeliveriesService()
        End If

        Return _deliveriesService
    End Function

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Deliveries:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Delivery # / Order # / Supplier / Product / Barcode"
        End Get
    End Property

    Protected Overrides ReadOnly Property ShowAddButton As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides Sub ConfigureModulePermissions()
        If IsStaffUser() Then
            ApplyStaffSidebarRestrictions()
        ElseIf IsCashierUser() Then
            ApplyCashierSidebarRestrictions()
        End If
    End Sub

    Protected Overrides Sub HandleAddAction()
        Using entryForm As New FrmDeliveryEntry With {
            .Mode = EntryFormMode.AddNew,
            .SelectedId = -1
        }
            If entryForm.ShowDialog(Me) = DialogResult.OK Then
                ReloadData()
            End If
        End Using
    End Sub

    Protected Overrides Sub LoadModuleData(searchText As String)
        Dim normalizedSearch As String = If(searchText, String.Empty).Trim()
        If Not String.Equals(normalizedSearch, _lastSearchText, StringComparison.OrdinalIgnoreCase) Then
            _currentPage = 1
            _lastSearchText = normalizedSearch
        End If

        Try
            Dim pageResult As DeliveryPagedResult = GetDeliveriesService().GetPendingDeliveryProducts(normalizedSearch, _currentPage, PageSize)
            Dim totalRecords As Integer = pageResult.TotalRecords
            _totalPages = Math.Max(1, CInt(Math.Ceiling(totalRecords / CDbl(PageSize))))

            If _currentPage > _totalPages Then
                _currentPage = _totalPages
                pageResult = GetDeliveriesService().GetPendingDeliveryProducts(normalizedSearch, _currentPage, PageSize)
            End If

            Dim dt As DataTable = If(pageResult.Items, New DataTable())

            EnsureDeliveryGridColumns()

            DGVtable.DataSource = dt
            ApplyGridFormatting()
            DGVtable.ClearSelection()
            UpdatePaginationUi()
        Catch ex As Exception
            _totalPages = 1
            UpdatePaginationUi()

            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        $"Error loading deliveries: {ex.Message}")
            MessageBox.Show("An error occurred while loading deliveries. Check logs for details.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub EnsureDeliveryGridColumns()
        If DGVtable Is Nothing OrElse DGVtable.IsDisposed Then Return

        DGVtable.AutoGenerateColumns = False

        Dim needsRebuild As Boolean = Not _deliveryGridColumnsConfigured
        If Not needsRebuild Then
            Dim requiredColumns As String() = {
                ColDeliveryNumber, ColOrderNumber, ColDeliveryDate, ColCompanyName,
                ColItemCount, ColTotalQty, ColStatus, ColDeliveryId,
                ColPendingItemCount, ColPostedItemCount, ColViewEdit, ColDelete
            }

            For Each colName As String In requiredColumns
                If Not DGVtable.Columns.Contains(colName) Then
                    needsRebuild = True
                    Exit For
                End If
            Next

            If IsAdminUser() AndAlso Not DGVtable.Columns.Contains(ColPost) Then
                needsRebuild = True
            End If
            If Not IsAdminUser() AndAlso DGVtable.Columns.Contains(ColPost) Then
                needsRebuild = True
            End If
        End If

        If Not needsRebuild Then Return

        DGVtable.Columns.Clear()

        DGVtable.Columns.Add(CreateTextBoundColumn(ColDeliveryNumber, ColDeliveryNumber, "Delivery #", 170, DataGridViewContentAlignment.MiddleLeft))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColOrderNumber, ColOrderNumber, "Order #", 160, DataGridViewContentAlignment.MiddleLeft))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColDeliveryDate, ColDeliveryDate, "Date", 110, DataGridViewContentAlignment.MiddleLeft))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColCompanyName, ColCompanyName, "Company", 170, DataGridViewContentAlignment.MiddleLeft))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColItemCount, ColItemCount, "Item Count", 95, DataGridViewContentAlignment.MiddleCenter))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColTotalQty, ColTotalQty, "Total Qty", 95, DataGridViewContentAlignment.MiddleCenter))
        DGVtable.Columns.Add(CreateTextBoundColumn(ColStatus, ColStatus, "Status", 120, DataGridViewContentAlignment.MiddleLeft))

        ' Hidden metadata columns used by actions/state logic.
        DGVtable.Columns.Add(CreateHiddenBoundColumn(ColDeliveryId, ColDeliveryId))
        DGVtable.Columns.Add(CreateHiddenBoundColumn(ColPendingItemCount, ColPendingItemCount))
        DGVtable.Columns.Add(CreateHiddenBoundColumn(ColPostedItemCount, ColPostedItemCount))
        DGVtable.Columns.Add(CreateHiddenBoundColumn(ColImagePath, ColImagePath))

        DGVtable.Columns.Add(CreateActionButtonColumn(ColViewEdit, "Action", "View/Edit", 100))

        If IsAdminUser() Then
            DGVtable.Columns.Add(CreateActionButtonColumn(ColPost, "Post", "Post", 100))
        End If

        DGVtable.Columns.Add(CreateActionButtonColumn(ColDelete, "Delete", "Delete", 100))

        _deliveryGridColumnsConfigured = True
    End Sub

    Private Function CreateTextBoundColumn(name As String,
                                           dataPropertyName As String,
                                           headerText As String,
                                           fillWeight As Integer,
                                           alignment As DataGridViewContentAlignment) As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn() With {
            .Name = name,
            .DataPropertyName = dataPropertyName,
            .HeaderText = headerText,
            .ReadOnly = True,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = fillWeight,
            .MinimumWidth = 70
        }
        col.DefaultCellStyle.Alignment = alignment
        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        Return col
    End Function

    Private Function CreateHiddenBoundColumn(name As String, dataPropertyName As String) As DataGridViewTextBoxColumn
        Dim col As New DataGridViewTextBoxColumn() With {
            .Name = name,
            .DataPropertyName = dataPropertyName,
            .HeaderText = name,
            .ReadOnly = True,
            .Visible = False,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .Width = 2,
            .MinimumWidth = 2
        }
        Return col
    End Function

    Private Function CreateActionButtonColumn(name As String,
                                              headerText As String,
                                              buttonText As String,
                                              width As Integer) As DataGridViewButtonColumn
        Dim col As New DataGridViewButtonColumn() With {
            .Name = name,
            .HeaderText = headerText,
            .Text = buttonText,
            .UseColumnTextForButtonValue = True,
            .ReadOnly = False,
            .Width = width,
            .MinimumWidth = width,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        }
        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        col.FlatStyle = FlatStyle.Popup
        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        Return col
    End Function

    Private Sub ApplyDeliveryGridDisplayOrder()
        If DGVtable Is Nothing OrElse DGVtable.IsDisposed Then Return

        Dim nextDisplayIndex As Integer = 0
        Dim orderedVisibleColumns As New List(Of String) From {
            ColDeliveryNumber,
            ColOrderNumber,
            ColDeliveryDate,
            ColCompanyName,
            ColItemCount,
            ColTotalQty,
            ColStatus,
            ColViewEdit
        }

        If DGVtable.Columns.Contains(ColPost) Then
            orderedVisibleColumns.Add(ColPost)
        End If

        orderedVisibleColumns.Add(ColDelete)

        For Each colName As String In orderedVisibleColumns
            If Not DGVtable.Columns.Contains(colName) Then Continue For
            DGVtable.Columns(colName).DisplayIndex = nextDisplayIndex
            nextDisplayIndex += 1
        Next
    End Sub

    Private Sub ApplyGridFormatting()
        EnsureDeliveryGridColumns()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return

        Dim hiddenCols() As String = {ColImagePath, ColDeliveryId, ColPendingItemCount, ColPostedItemCount}
        For Each colName As String In hiddenCols
            GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
        Next

        UpdatePostButtonStates()

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {ColDeliveryNumber, Sub(col)
                                    col.HeaderText = "Delivery #"
                                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                End Sub},
            {ColOrderNumber, Sub(col)
                                 col.HeaderText = "Order #"
                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                 col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                             End Sub},
            {ColDeliveryDate, Sub(col)
                                  col.HeaderText = "Date"
                                  col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                  col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                  col.DefaultCellStyle.Format = "dd/MM/yyyy"
                              End Sub},
            {ColCompanyName, Sub(col)
                                 col.HeaderText = "Company"
                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                                 col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                             End Sub},
            {ColItemCount, Sub(col)
                               col.HeaderText = "Item Count"
                               col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                               col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                           End Sub},
            {ColTotalQty, Sub(col)
                              col.HeaderText = "Total Qty"
                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                              col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                          End Sub},
            {ColStatus, Sub(col)
                            col.HeaderText = "Status"
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        End Sub}
        }
        Dim columnAliases As New Dictionary(Of String, String()) From {
            {ColCompanyName, New String() {"Company"}}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions, columnAliases)

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVtable.Columns
            If Not col.Visible Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            ElseIf col.Name = ColViewEdit OrElse col.Name = ColPost OrElse col.Name = ColDelete Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        ApplyDeliveryColumnSizing()

        ApplyStandardGridLayout(DGVtable)
        ApplyDeliveryGridDisplayOrder()
        ApplyDeliveryButtonAlignment()
        DGVtable.DefaultCellStyle.Padding = Padding.Empty
        DGVtable.RowsDefaultCellStyle.Padding = Padding.Empty
        DGVtable.ColumnHeadersDefaultCellStyle.Padding = Padding.Empty
        DGVtable.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DGVtable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        DGVtable.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single
        DGVtable.AdvancedColumnHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None
        DGVtable.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single
        DGVtable.GridColor = Color.Gainsboro
        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        DGVtable.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        DGVtable.DefaultCellStyle.WrapMode = DataGridViewTriState.False
    End Sub

    Private Sub ApplyDeliveryButtonAlignment()
        Dim actionColumns As String() = {ColViewEdit, ColPost, ColDelete}

        For Each colName As String In actionColumns
            If Not DGVtable.Columns.Contains(colName) Then Continue For

            Dim col As DataGridViewButtonColumn = TryCast(DGVtable.Columns(colName), DataGridViewButtonColumn)
            If col Is Nothing Then Continue For

            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            col.DefaultCellStyle.Padding = New Padding(6, 4, 6, 4)
            col.FlatStyle = FlatStyle.Popup
        Next
    End Sub

    Private Sub ApplyDeliveryColumnSizing()
        If DGVtable Is Nothing OrElse DGVtable.IsDisposed Then Return

        GridHelpers.ApplyColumnSetup(DGVtable, ColDeliveryNumber, Sub(col)
                                                                      col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                      col.Width = 180
                                                                  End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColOrderNumber, Sub(col)
                                                                   col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                   col.Width = 260
                                                               End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColDeliveryDate, Sub(col)
                                                                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                    col.Width = 140
                                                                End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColCompanyName, Sub(col)
                                                                   col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                                                                   col.FillWeight = 170
                                                                   col.MinimumWidth = 180
                                                               End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColItemCount, Sub(col)
                                                                 col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                 col.Width = 110
                                                             End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColTotalQty, Sub(col)
                                                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                col.Width = 110
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColStatus, Sub(col)
                                                              col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                              col.Width = 140
                                                          End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColViewEdit, Sub(col)
                                                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                col.Width = 100
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColPost, Sub(col)
                                                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                            col.Width = 100
                                                        End Sub)
        GridHelpers.ApplyColumnSetup(DGVtable, ColDelete, Sub(col)
                                                              col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                              col.Width = 100
                                                          End Sub)
    End Sub

    Private Sub UpdatePostButtonStates()
        If Not DGVtable.Columns.Contains(ColPost) OrElse Not DGVtable.Columns.Contains(ColStatus) Then Return

        For Each row As DataGridViewRow In DGVtable.Rows
            If row.IsNewRow Then Continue For

            Dim statusText As String = Convert.ToString(row.Cells(ColStatus).Value).Trim()
            Dim pendingItemCount As Integer = 0
            TryGetInt(row, ColPendingItemCount, pendingItemCount)

            Dim isPosted As Boolean = String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase) OrElse pendingItemCount <= 0
            Dim postCell As DataGridViewButtonCell = TryCast(row.Cells(ColPost), DataGridViewButtonCell)
            If postCell Is Nothing Then Continue For

            If isPosted Then
                postCell.Value = "Posted"
            ElseIf String.Equals(statusText, "Partially Posted", StringComparison.OrdinalIgnoreCase) Then
                postCell.Value = "Post Remaining"
            Else
                postCell.Value = "Post"
            End If
            postCell.ReadOnly = isPosted
            postCell.Style.ForeColor = If(isPosted, Color.DimGray, Color.Black)
            postCell.Style.BackColor = If(isPosted, Color.Gainsboro, Color.White)
        Next
    End Sub

    Private Sub OpenEditModalById(deliveryId As Integer)
        Using entryForm As New FrmDeliveryEntry With {
            .Mode = EntryFormMode.EditExisting,
            .SelectedId = deliveryId
        }
            If entryForm.ShowDialog(Me) = DialogResult.OK Then
                ReloadData()
            End If
        End Using
    End Sub

    Private Function HasPostedItems(row As DataGridViewRow) As Boolean
        Dim postedItemCount As Integer = 0
        Return TryGetInt(row, ColPostedItemCount, postedItemCount) AndAlso postedItemCount > 0
    End Function

    Private Sub DeleteDeliveryBatchById(deliveryId As Integer)
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this delivery batch?",
                                                     "Confirm Cancel",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question)
        If result <> DialogResult.Yes Then Return

        Try
            GetDeliveriesService().DeleteDelivery(deliveryId)

            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        "Cancelled Deliveries.")
            MessageBox.Show("Delivery batch has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ReloadData()
        Catch ex As Exception
            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        $"Error deleting delivery batch: {ex.Message}")
            MessageBox.Show("Failed to delete delivery batch. See log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PostDeliveryBatchById(deliveryId As Integer)
        If Not IsAdminUser() Then
            MessageBox.Show("Only Admin users can post deliveries to inventory.",
                            "Access Denied",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm As DialogResult = MessageBox.Show("Do you want to post all pending items in this delivery batch to inventory?",
                                                      "Confirm Post",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        Try
            Dim postedCount As Integer = GetDeliveriesService().PostDeliveryBatch(deliveryId)
            If postedCount <= 0 Then
                MessageBox.Show("This delivery batch has no pending items left to post.",
                                "Already Posted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Return
            End If

            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        $"Posted delivery batch. Items posted: {postedCount}.")
            MessageBox.Show($"Delivery batch posted successfully. {postedCount} item(s) were posted to inventory.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            ReloadData()
        Catch ex As Exception
            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        $"Error posting delivery batch: {ex.Message}")
            MessageBox.Show("Failed to post delivery batch. See log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdatePaginationUi()
        If _currentPage < 1 Then _currentPage = 1
        If _totalPages < 1 Then _totalPages = 1
        If _currentPage > _totalPages Then _currentPage = _totalPages

        SetPageDisplay(_currentPage, _totalPages)
        btnPreviousPage.Enabled = (_currentPage > 1)
        btnNextPage.Enabled = (_currentPage < _totalPages)
    End Sub

    Private Function TryGetInt(row As DataGridViewRow, columnName As String, ByRef value As Integer) As Boolean
        value = -1
        If row Is Nothing OrElse row.DataGridView Is Nothing OrElse Not row.DataGridView.Columns.Contains(columnName) Then Return False

        Dim raw As Object = row.Cells(columnName).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), value)
    End Function

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplyGridFormatting()
    End Sub

    Private Sub BtnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If _currentPage <= 1 Then
            UpdatePaginationUi()
            Return
        End If

        _currentPage -= 1
        ReloadData()
    End Sub

    Private Sub BtnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If _currentPage >= _totalPages Then
            UpdatePaginationUi()
            Return
        End If

        _currentPage += 1
        ReloadData()
    End Sub

    Private Sub DGVtable_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = GridHelpers.GetColumnNameByIndex(DGVtable, e.ColumnIndex)
        If colName <> ColViewEdit AndAlso colName <> ColPost AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)

        Dim deliveryId As Integer
        If Not TryGetInt(row, ColDeliveryId, deliveryId) Then Exit Sub

        If colName = ColViewEdit Then
            Dim pendingItemCount As Integer = 0
            TryGetInt(row, ColPendingItemCount, pendingItemCount)
            If pendingItemCount <= 0 Then
                MessageBox.Show("This delivery batch is fully posted and can no longer be edited.", "Edit Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            OpenEditModalById(deliveryId)
        ElseIf colName = ColPost Then
            Dim statusText As String = Convert.ToString(row.Cells(ColStatus).Value).Trim()
            Dim pendingItemCount As Integer = 0
            TryGetInt(row, ColPendingItemCount, pendingItemCount)

            If String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase) OrElse pendingItemCount <= 0 Then
                MessageBox.Show("This delivery batch is already posted.", "Already Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            PostDeliveryBatchById(deliveryId)
        ElseIf colName = ColDelete Then
            If HasPostedItems(row) Then
                MessageBox.Show("Cannot delete a delivery batch that already has posted items.", "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            DeleteDeliveryBatchById(deliveryId)
        End If
    End Sub
End Class
