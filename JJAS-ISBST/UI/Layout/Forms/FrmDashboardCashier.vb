Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports Guna.UI2.WinForms

Public Class FrmDashboardCashier
    Private Const DashboardRefreshIntervalMs As Integer = 15000
    Private Const RecentTransactionLimit As Integer = 50
    Private Const StockMovementLimit As Integer = 100

    Private ReadOnly _activeSidebarFillColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _inactiveSidebarFillColor As Color = Color.Black
    Private ReadOnly _transactionService As New TransactionService()
    Private ReadOnly _refreshTimer As New Timer()

    Private Sub FrmDashboardCashier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySidebarUserPanel()
        ConfigureDashboardGrids()
        SetActiveSidebarButton(btnHome)
        LoadDashboardData(True)
        StartRefreshTimer()
    End Sub

    Private Sub FrmDashboardCashier_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
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
        Label1.Text = "Name: " & ResolveCashierDisplayName()
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
            Dim cashierUserId As Integer = ResolveCashierUserId()
            Dim summary As CashierDashboardSummary = _transactionService.GetCashierDashboardSummary(cashierUserId)

            lblTransactionsTodayValue.Text = summary.TransactionsToday.ToString("N0")
            lblItemSoldValue.Text = summary.ItemsSoldToday.ToString("N0")

            Dim pesoSign As String = ChrW(&H20B1)
            lblSalesTodayValue.Text = $"{pesoSign}{summary.SalesToday:N2}"
            lblProfitTodayValue.Text = $"{pesoSign}{summary.ProfitToday:N2}"

            dgvTable.DataSource = _transactionService.GetRecentCashierTransactions(cashierUserId, RecentTransactionLimit)
            ApplyRecentTransactionsGridFormatting()

            dtgTopSellingProduct.DataSource = _transactionService.GetCashierStockMovement(cashierUserId, StockMovementLimit)
            ApplyStockMovementGridFormatting()
        Catch ex As Exception
            If Not showErrors Then Return

            MessageBox.Show("Error loading cashier dashboard: " & ex.Message,
                            "Dashboard",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyRecentTransactionsGridFormatting()
        If Not GridHelpers.IsGridReady(dgvTable) Then Return

        ApplyStandardGridLayout(dgvTable)
        dgvTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        GridHelpers.ApplyColumnSetup(dgvTable, "Transaction No", Sub(col) col.HeaderText = "Trans. No")
        GridHelpers.ApplyColumnSetup(dgvTable, "Sale Date", Sub(col)
                                                                col.HeaderText = "Date"
                                                                col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                                                            End Sub)
        GridHelpers.ApplyColumnSetup(dgvTable, "Items", Sub(col)
                                                            col.DefaultCellStyle.Format = "N0"
                                                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                        End Sub)
        GridHelpers.ApplyColumnSetup(dgvTable, "Total Amount", Sub(col)
                                                                   col.HeaderText = "Total Amount"
                                                                   col.DefaultCellStyle.Format = "N2"
                                                                   col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                               End Sub)

        dgvTable.ClearSelection()
    End Sub

    Private Sub ApplyStockMovementGridFormatting()
        If Not GridHelpers.IsGridReady(dtgTopSellingProduct) Then Return

        ApplyStandardGridLayout(dtgTopSellingProduct)
        dtgTopSellingProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        GridHelpers.ApplyColumnSetup(dtgTopSellingProduct, "Sale Date", Sub(col)
                                                                             col.HeaderText = "Date"
                                                                             col.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                                                                         End Sub)
        GridHelpers.ApplyColumnSetup(dtgTopSellingProduct, "Transaction No", Sub(col) col.HeaderText = "Trans. No")
        GridHelpers.ApplyColumnSetup(dtgTopSellingProduct, "Deducted Qty", Sub(col)
                                                                                col.HeaderText = "Deducted"
                                                                                col.DefaultCellStyle.Format = "N0"
                                                                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                                            End Sub)
        GridHelpers.ApplyColumnSetup(dtgTopSellingProduct, "Current Stock", Sub(col)
                                                                                 col.DefaultCellStyle.Format = "N0"
                                                                                 col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                                                                             End Sub)

        dtgTopSellingProduct.ClearSelection()
    End Sub

    Private Function ResolveCashierFilterName() As String
        Dim cashierName As String = If(FrmLogin.CurrentUser.FullName, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        cashierName = If(SessionContext.FullName, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        cashierName = If(FrmLogin.CurrentUser.Username, String.Empty).Trim()
        If cashierName.Length > 0 Then Return cashierName

        Return If(SessionContext.Username, String.Empty).Trim()
    End Function

    Private Function ResolveCashierUserId() As Integer
        Dim userId As Integer = FrmLogin.CurrentUser.UserID
        If userId > 0 Then
            Return userId
        End If

        If SessionContext IsNot Nothing AndAlso SessionContext.PrincipalID > 0 Then
            Return SessionContext.PrincipalID
        End If

        Return 0
    End Function

    Private Function ResolveCashierDisplayName() As String
        Dim cashierName As String = ResolveCashierFilterName()
        If cashierName.Length > 0 Then
            Return cashierName
        End If

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
        Yield btnPos
        Yield btnInventory
        Yield btnDiscount
        Yield btnTransaction
        Yield btnReports
        Yield btnLogout
    End Function

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        SwitchSidebarNavigation(btnHome)
        ApplySidebarUserPanel()
        LoadDashboardData(True)
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        SwitchSidebarNavigation(btnPos, New FrmPOS())
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        SwitchSidebarNavigation(btnInventory, New InventoryModuleForm())
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        SwitchSidebarNavigation(btnDiscount, New FrmDiscountCashier())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        SwitchSidebarNavigation(btnTransaction, New TransactionsModuleForm())
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SwitchSidebarNavigation(btnReports, New FrmReports())
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
        ApplyRecentTransactionsGridFormatting()
    End Sub

    Private Sub dtgTopSellingProduct_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dtgTopSellingProduct.DataBindingComplete
        ApplyStockMovementGridFormatting()
    End Sub
End Class
