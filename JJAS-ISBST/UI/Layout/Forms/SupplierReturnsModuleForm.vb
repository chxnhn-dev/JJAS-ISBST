Public Class SupplierReturnsModuleForm
    Inherits ModuleListBaseForm

    Private _service As DeliveriesService

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.SupplierReturnsTab
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Supplier Returns:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Return # / Delivery # / Order # / Supplier / Product / Barcode"
        End Get
    End Property

    Protected Overrides ReadOnly Property SupportsPagination As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property ShowAddButton As Boolean
        Get
            Return True
        End Get
    End Property

    Private Function GetDeliveriesService() As DeliveriesService
        If _service Is Nothing Then
            _service = New DeliveriesService()
        End If

        Return _service
    End Function

    Protected Overrides Sub ConfigureModulePermissions()
        If IsStaffUser() Then
            ApplyStaffSidebarRestrictions()
        ElseIf IsCashierUser() Then
            ApplyCashierSidebarRestrictions()
        End If
    End Sub

    Protected Overrides Sub HandleAddAction()
        Using entry As New FrmSupplierReturnEntry()
            entry.StartPosition = FormStartPosition.CenterParent

            If entry.ShowDialog(Me) <> DialogResult.OK Then
                Return
            End If

            ReloadData()
            LogActivity(FrmLogin.CurrentUser.UserID,
                        FrmLogin.CurrentUser.FullName,
                        FrmLogin.CurrentUser.Username,
                        FrmLogin.CurrentUser.Role,
                        String.Format("Created supplier return {0}.", entry.ReturnNumber))

            MessageBox.Show(String.Format("Supplier return {0} saved successfully.", entry.ReturnNumber),
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
        End Using
    End Sub

    Protected Overrides Sub LoadModuleData(searchText As String)
        Try
            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) GetDeliveriesService().GetSupplierReturnsPage(request))
            DGVtable.DataSource = page.Records
            ApplySupplierReturnsGridFormatting()
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading supplier returns: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplySupplierReturnsGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return

        DGVtable.ReadOnly = True

        Dim hiddenCols() As String = {"ReturnID"}
        For Each colName As String In hiddenCols
            GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
        Next

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"ReturnNumber", Sub(col) col.HeaderText = "Return #"},
            {"SupplierName", Sub(col) col.HeaderText = "Supplier"},
            {"DeliveryNumber", Sub(col) col.HeaderText = "Delivery #"},
            {"OrderNumber", Sub(col) col.HeaderText = "Order #"},
            {"DeliveryDate", Sub(col)
                                 col.HeaderText = "Delivery Date"
                                 col.DefaultCellStyle.Format = "dd/MM/yyyy"
                             End Sub},
            {"ReturnDate", Sub(col)
                               col.HeaderText = "Return Date"
                               col.DefaultCellStyle.Format = "dd/MM/yyyy"
                           End Sub},
            {"ReturnType", Sub(col) col.HeaderText = "Type"},
            {"Resolution", Sub(col) col.HeaderText = "Resolution"},
            {"Status", Sub(col) col.HeaderText = "Status"},
            {"ItemCount", Sub(col)
                              col.HeaderText = "Items"
                              col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                          End Sub},
            {"TotalReturnedQty", Sub(col)
                                     col.HeaderText = "Returned Qty"
                                     col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                                 End Sub},
            {"Notes", Sub(col) col.HeaderText = "Notes"}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions)

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)
        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
    End Sub

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplySupplierReturnsGridFormatting()
    End Sub
End Class
