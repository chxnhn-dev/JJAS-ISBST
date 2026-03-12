Public Class AuditTrailModuleForm
    Inherits ModuleListBaseForm

    Private _service As AuditTrailService

    Protected Overrides ReadOnly Property CurrentModuleTab As ModuleTab
        Get
            Return ModuleTab.AuditTrailTab
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchCaption As String
        Get
            Return "Search Audit Trail:"
        End Get
    End Property

    Protected Overrides ReadOnly Property SearchPlaceholder As String
        Get
            Return "Name / Username / Role"
        End Get
    End Property

    Protected Overrides ReadOnly Property SupportsPagination As Boolean
        Get
            Return True
        End Get
    End Property

    Private Function GetAuditTrailService() As AuditTrailService
        If _service Is Nothing Then
            _service = New AuditTrailService()
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

    Protected Overrides Sub LoadModuleData(searchText As String)
        Try
            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) GetAuditTrailService().GetAuditTrailPage(request))
            Dim dt As DataTable = page.Records

            DGVtable.DataSource = dt
            ApplyAuditGridFormatting()
            DGVtable.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading audit trail: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyAuditGridFormatting()
        If Not GridHelpers.IsGridReady(DGVtable) Then Return

        Dim hiddenCols() As String = {"AuditID", "UserID"}
        For Each colName As String In hiddenCols
            GridHelpers.ApplyColumnSetup(DGVtable, colName, Sub(col) col.Visible = False)
        Next

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"Name", Sub(col) col.HeaderText = "Full Name"}
        }
        Dim columnAliases As New Dictionary(Of String, String()) From {
            {"Name", New String() {"FullName"}}
        }
        GridHelpers.ApplyColumnSetup(DGVtable, columnActions, columnAliases)

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ApplyStandardGridLayout(DGVtable)
        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
    End Sub

    Private Sub DGVtable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        ApplyAuditGridFormatting()
    End Sub
End Class
