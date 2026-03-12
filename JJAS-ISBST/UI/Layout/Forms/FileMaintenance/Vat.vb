Namespace FileMaintenance
    Public Class Vat
        Inherits FileMaintenanceBaseForm

        Private _service As VatService
        Private WithEvents _hideSearchTimer As New Windows.Forms.Timer() With {.Interval = 18}
        Private _hideSearchStep As Integer
        Private _searchOriginalWidth As Integer = -1
        Private _labelOriginalColor As Drawing.Color
        Private _searchOriginalColor As Drawing.Color
        Private Const HideAnimationSteps As Integer = 12

        Protected Overrides ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
            Get
                Return MaintenanceTab.VatTab
            End Get
        End Property

        Protected Overrides Sub InitializeServices()
            If _service Is Nothing Then
                _service = New VatService()
            End If
        End Sub

        Protected Overrides ReadOnly Property UseEditPrimaryAction As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Sub HandlePrimaryAction()
            UpdateVat()
        End Sub

        Private Sub Vat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If _searchOriginalWidth < 0 Then
                _searchOriginalWidth = txtSearch.Width
                _labelOriginalColor = lblSearch.ForeColor
                _searchOriginalColor = txtSearch.ForeColor
            End If
        End Sub

        Private Sub Vat_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            BeginInvoke(New Action(AddressOf StartSearchHideAnimation))
        End Sub

        Private Sub StartSearchHideAnimation()
            If txtSearch Is Nothing OrElse lblSearch Is Nothing Then Return
            If Not txtSearch.Visible AndAlso Not lblSearch.Visible Then Return

            _hideSearchStep = 0
            txtSearch.Width = _searchOriginalWidth
            txtSearch.ForeColor = _searchOriginalColor
            lblSearch.ForeColor = _labelOriginalColor
            txtSearch.TabStop = True

            _hideSearchTimer.Stop()
            _hideSearchTimer.Start()
        End Sub

        Private Sub HideSearchTimer_Tick(sender As Object, e As EventArgs) Handles _hideSearchTimer.Tick
            _hideSearchStep += 1

            Dim progress As Double = Math.Min(1.0, CDbl(_hideSearchStep) / HideAnimationSteps)
            Dim remaining As Double = 1.0 - progress
            Dim alpha As Integer = Math.Max(0, CInt(255 * remaining))

            txtSearch.Width = Math.Max(1, CInt(_searchOriginalWidth * remaining))
            lblSearch.ForeColor = Drawing.Color.FromArgb(alpha, _labelOriginalColor.R, _labelOriginalColor.G, _labelOriginalColor.B)
            txtSearch.ForeColor = Drawing.Color.FromArgb(alpha, _searchOriginalColor.R, _searchOriginalColor.G, _searchOriginalColor.B)

            If progress >= 1.0 Then
                _hideSearchTimer.Stop()
                lblSearch.Visible = False
                txtSearch.Visible = False
                txtSearch.TabStop = False
            End If
        End Sub

        Protected Overrides Sub LoadTableData(searchText As String)
            If DGVtable Is Nothing OrElse _service Is Nothing Then Exit Sub

            Dim page As PagedQueryResult = LoadPagedData(searchText, Function(request) _service.GetVatPage(request))
            Dim dt As DataTable = page.Records

            DGVtable.DataSource = dt
            GridHelpers.ApplyColumnSetup(DGVtable, "VatID", Sub(col) col.Visible = False)

            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyDefaultGridLayout()
        End Sub

        Private Sub UpdateVat()
            If DGVtable.Rows.Count = 0 Then
                MessageBox.Show("No VAT record available to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim vatIdColumn As DataGridViewColumn = Nothing
            Dim vatRateColumn As DataGridViewColumn = Nothing
            If Not GridHelpers.TryGetColumn(DGVtable, vatIdColumn, "VatID") OrElse
               Not GridHelpers.TryGetColumn(DGVtable, vatRateColumn, "Vat_Rate", "VatRate") Then
                MessageBox.Show("VAT columns are not available in the grid.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim targetRow As DataGridViewRow = Nothing
            If DGVtable.CurrentRow IsNot Nothing AndAlso Not DGVtable.CurrentRow.IsNewRow Then
                targetRow = DGVtable.CurrentRow
            Else
                targetRow = DGVtable.Rows(0)
            End If

            Dim vatId As Integer = Convert.ToInt32(targetRow.Cells(vatIdColumn.Name).Value)
            Dim vatRate As Decimal = Convert.ToDecimal(targetRow.Cells(vatRateColumn.Name).Value)

            Dim entryForm As New FrmVATEntry With {
                .VatID = vatId,
                .VatRate = vatRate
            }

            If ShowOwnedEntryModal(entryForm) = DialogResult.OK Then
                LogActivity(FrmLogin.CurrentUser.UserID, FrmLogin.CurrentUser.FullName, FrmLogin.CurrentUser.Username, FrmLogin.CurrentUser.Role, "VAT updated.")
                ReloadData()
            End If
        End Sub

        Private Sub DGVtable_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellDoubleClick
            If e.RowIndex < 0 Then Exit Sub
            UpdateVat()
        End Sub
    End Class
End Namespace
