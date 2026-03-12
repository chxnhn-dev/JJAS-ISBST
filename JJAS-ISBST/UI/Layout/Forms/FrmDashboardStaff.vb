Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Drawing
Imports Guna.UI2.WinForms

Public Class FrmDashboardStaff
    Private Const DashboardRefreshIntervalMs As Integer = 15000
    Private Const LowStockLimit As Integer = 100
    Private Const StockMovementLimit As Integer = 150
    Private Const LowStockThreshold As Integer = 10
    Private Const ColImagePath As String = "ImagePath"
    Private Const ColProductImage As String = "ProductImage"

    Private ReadOnly _activeSidebarFillColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _inactiveSidebarFillColor As Color = Color.Black
    Private ReadOnly _transactionService As New TransactionService()
    Private ReadOnly _refreshTimer As New Timer()

    Private Sub FrmDashboardStaff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySidebarUserPanel()
        ApplyStaffSidebarNavigationOrder(Guna2Panel2, btnHome, btnFileMaintenance, btnDelivery, btnReturn, btnInventory, btnTransaction, btnReports, btnLogout)
        ConfigureDashboardGrids()
        SetActiveSidebarButton(btnHome)
        LoadDashboardData(True)
        StartRefreshTimer()
    End Sub

    Private Sub FrmDashboardStaff_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        StopRefreshTimer()
    End Sub

    Private Sub RefreshTimer_Tick(sender As Object, e As EventArgs)
        LoadDashboardData(False)
    End Sub

    Private Sub StartRefreshTimer()
        StopRefreshTimer()
        _refreshTimer.Interval = DashboardRefreshIntervalMs
        AddHandler _refreshTimer.Tick, AddressOf RefreshTimer_Tick
        _refreshTimer.Start()
    End Sub

    Private Sub StopRefreshTimer()
        RemoveHandler _refreshTimer.Tick, AddressOf RefreshTimer_Tick
        _refreshTimer.Stop()
    End Sub

    Private Sub ApplySidebarUserPanel()
        lblFirstname.Text = ResolveSidebarFirstName()
        lblUserLevel.Text = ResolveSidebarRoleDisplay()
        Label1.Text = "Name: " & ResolveStaffDisplayName()
    End Sub

    Private Sub ConfigureDashboardGrids()
        ConfigureDashboardGrid(dgvTable)
        ConfigureDashboardGrid(dtgTopSellingProduct)
    End Sub

    Private Shared Sub ConfigureDashboardGrid(grid As DataGridView)
        If grid Is Nothing Then Return

        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AllowUserToResizeRows = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub LoadDashboardData(showErrors As Boolean)
        Try
            Dim summary As StaffDashboardSummary = _transactionService.GetStaffDashboardSummary()
            lblTotalProductsValue.Text = summary.TotalProducts.ToString("N0")
            lblPendingDeliveriesValue.Text = summary.PendingDeliveries.ToString("N0")
            lblReturnsValue.Text = summary.ReturnsCount.ToString("N0")
            lblLowStockItemsValue.Text = summary.TransactionsToday.ToString("N0")

            Dim lowStockData = _transactionService.GetLowStockItems(LowStockLimit, LowStockThreshold)
            ImageLoader.AddAndLoadImages(lowStockData, ColImagePath, ColProductImage)
            dgvTable.DataSource = lowStockData
            ApplyLowStockGridFormatting()

            Dim stockMovementData = _transactionService.GetStockMovement(StockMovementLimit)
            ImageLoader.AddAndLoadImages(stockMovementData, ColImagePath, ColProductImage)
            dtgTopSellingProduct.DataSource = stockMovementData
            ApplyStockMovementGridFormatting()
        Catch ex As Exception
            If Not showErrors Then Return

            MessageBox.Show("Error loading staff dashboard: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyLowStockGridFormatting()
        If Not GridHelpers.IsGridReady(dgvTable) Then Return

        ApplyStandardGridLayout(dgvTable)
        dgvTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"Order No", Sub(col) col.HeaderText = "Order #"},
            {"Stock", Sub(col)
                          col.DefaultCellStyle.Format = "N0"
                          col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                      End Sub},
            {"Updated", Sub(col)
                            col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                        End Sub}
        }
        GridHelpers.ApplyColumnSetup(dgvTable, columnActions)
        ConfigureProductImageColumn(dgvTable)
        AlignGridCellsLeft(dgvTable)

        dgvTable.ClearSelection()
    End Sub

    Private Sub ApplyStockMovementGridFormatting()
        If Not GridHelpers.IsGridReady(dtgTopSellingProduct) Then Return

        ApplyStandardGridLayout(dtgTopSellingProduct)
        dtgTopSellingProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim columnActions As New Dictionary(Of String, Action(Of DataGridViewColumn)) From {
            {"Sale Date", Sub(col)
                              col.HeaderText = "Date"
                              col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                          End Sub},
            {"Transaction No", Sub(col) col.HeaderText = "Trans. No"},
            {"Deducted Qty", Sub(col)
                                 col.HeaderText = "Deducted"
                                 col.DefaultCellStyle.Format = "N0"
                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                             End Sub},
            {"Current Stock", Sub(col)
                                  col.DefaultCellStyle.Format = "N0"
                                  col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                              End Sub}
        }
        GridHelpers.ApplyColumnSetup(dtgTopSellingProduct, columnActions)
        ConfigureProductImageColumn(dtgTopSellingProduct)
        AlignGridCellsLeft(dtgTopSellingProduct)

        dtgTopSellingProduct.ClearSelection()
    End Sub

    Private Shared Sub ConfigureProductImageColumn(grid As DataGridView)
        If grid Is Nothing Then Return

        GridHelpers.ApplyColumnSetup(grid, ColImagePath, Sub(col) col.Visible = False)

        Dim imageColumn As DataGridViewColumn = Nothing
        If Not GridHelpers.TryGetColumn(grid, imageColumn, ColProductImage) Then Return

        imageColumn.HeaderText = "Image"
        imageColumn.DisplayIndex = 0
        imageColumn.Width = 74
        imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        imageColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        Dim typedImageColumn As DataGridViewImageColumn = TryCast(imageColumn, DataGridViewImageColumn)
        If typedImageColumn IsNot Nothing Then
            typedImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
            typedImageColumn.DefaultCellStyle.NullValue = GetMissingProductImagePlaceholder()
        End If
    End Sub

    Private Shared Sub AlignGridCellsLeft(grid As DataGridView)
        If grid Is Nothing Then Return

        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        For Each col As DataGridViewColumn In grid.Columns
            If Not col.Visible Then Continue For
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Next
    End Sub

    Private Function ResolveStaffDisplayName() As String
        Dim displayName As String = If(FrmLogin.CurrentUser.FullName, String.Empty).Trim()
        If displayName.Length > 0 Then Return displayName

        displayName = If(SessionContext.FullName, String.Empty).Trim()
        If displayName.Length > 0 Then Return displayName

        displayName = If(FrmLogin.CurrentUser.Username, String.Empty).Trim()
        If displayName.Length > 0 Then Return displayName

        Return "User"
    End Function

    Private Sub SwitchSidebarNavigation(activeButton As Guna2Button, Optional nextForm As Form = Nothing)
        SetActiveSidebarButton(activeButton)
        If nextForm Is Nothing Then Return

        StopRefreshTimer()
        nextForm.Show()
        Me.Hide()
    End Sub

    Private Sub SetActiveSidebarButton(activeButton As Guna2Button)
        For Each button As Guna2Button In GetSidebarNavigationButtons()
            If button Is Nothing Then Continue For

            Dim isActive As Boolean = button Is activeButton
            button.FillColor = If(isActive, _activeSidebarFillColor, _inactiveSidebarFillColor)
            button.HoverState.FillColor = _activeSidebarFillColor
        Next
    End Sub

    Private Iterator Function GetSidebarNavigationButtons() As IEnumerable(Of Guna2Button)
        Yield btnHome
        Yield btnFileMaintenance
        Yield btnDelivery
        Yield btnReturn
        Yield btnInventory
        Yield btnTransaction
        Yield btnReports
        Yield btnLogout
    End Function

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        SwitchSidebarNavigation(btnHome)
        ApplySidebarUserPanel()
        LoadDashboardData(True)
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        SwitchSidebarNavigation(btnFileMaintenance, New FileMaintenance.Category())
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        SwitchSidebarNavigation(btnDelivery, New DeliveriesModuleForm())
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        SwitchSidebarNavigation(btnReturn, New SupplierReturnsModuleForm())
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        SwitchSidebarNavigation(btnInventory, New InventoryModuleForm())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        SwitchSidebarNavigation(btnTransaction, New TransactionsModuleForm())
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SwitchSidebarNavigation(btnReports, New FrmReports())
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        SwitchSidebarNavigation(btnDelivery, New DeliveriesModuleForm())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                      "Logout",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        StopRefreshTimer()

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging out: " & ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try

        LogActivity(FrmLogin.CurrentUser.UserID,
                    FrmLogin.CurrentUser.FullName,
                    FrmLogin.CurrentUser.Username,
                    FrmLogin.CurrentUser.Role,
                    "User Logged Out.")

        FrmLogin.CurrentUser.UserID = 0
        FrmLogin.CurrentUser.Username = ""
        FrmLogin.CurrentUser.Role = ""
        FrmLogin.CurrentUser.FullName = ""

        Me.Hide()
        Dim loginForm As New FrmLogin()
        loginForm.Show()
    End Sub

    Private Sub dgvTable_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvTable.DataBindingComplete
        ApplyLowStockGridFormatting()
    End Sub

    Private Sub dtgTopSellingProduct_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dtgTopSellingProduct.DataBindingComplete
        ApplyStockMovementGridFormatting()
    End Sub
End Class
