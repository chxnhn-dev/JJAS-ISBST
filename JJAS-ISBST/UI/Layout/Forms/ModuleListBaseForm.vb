Imports System.Data.SqlClient
Imports Guna.UI2.WinForms

Public MustInherit Class ModuleListBaseForm
    Protected Enum ModuleTab
        DiscountTab
        DeliveriesTab
        InventoryTab
        TransactionsTab
        SupplierReturnsTab
        AuditTrailTab
    End Enum

    Private ReadOnly _activeFillColor As Color = Color.FromArgb(30, 32, 30)
    Private ReadOnly _inactiveFillColor As Color = Color.Black
    Private _paginationState As PaginationState
    Private _lastSearchText As String = String.Empty

    Public Sub New()
        InitializeComponent()
    End Sub

    Protected MustOverride ReadOnly Property CurrentModuleTab As ModuleTab

    Protected Overridable ReadOnly Property SearchCaption As String
        Get
            Return "Search:"
        End Get
    End Property

    Protected Overridable ReadOnly Property SearchPlaceholder As String
        Get
            Return String.Empty
        End Get
    End Property

    Protected Overridable ReadOnly Property ShowAddButton As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property ShowPrintButton As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property SupportsPagination As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property DefaultPageSize As Integer
        Get
            Return 20
        End Get
    End Property

    Protected Overridable Sub ConfigureModulePermissions()
    End Sub

    Protected Overridable Sub LoadModuleData(searchText As String)
        DGVtable.DataSource = Nothing
    End Sub

    Protected Overridable Sub HandleAddAction()
    End Sub

    Protected Overridable Sub HandlePrintAction()
    End Sub

    Protected Overridable Function CanNavigateToModule(targetTab As ModuleTab) As Boolean
        Return True
    End Function

    Protected Function CreatePaginationRequest(searchText As String) As PagedQueryRequest
        EnsurePaginationState()

        Dim normalizedSearch As String = NormalizeSearchText(searchText)
        If Not String.Equals(_lastSearchText, normalizedSearch, StringComparison.OrdinalIgnoreCase) Then
            _paginationState.ResetToFirstPage()
            _lastSearchText = normalizedSearch
        End If

        Return _paginationState.BuildRequest(normalizedSearch)
    End Function

    Protected Function LoadPagedData(searchText As String,
                                     fetchPage As Func(Of PagedQueryRequest, PagedQueryResult)) As PagedQueryResult
        If fetchPage Is Nothing Then
            Return PagedQueryResult.Empty(CreatePaginationRequest(searchText))
        End If

        Dim request As PagedQueryRequest = CreatePaginationRequest(searchText)
        Dim result As PagedQueryResult = fetchPage(request)
        If result Is Nothing Then
            result = PagedQueryResult.Empty(request)
        End If

        Dim safeTotalPages As Integer = Math.Max(1, result.TotalPages)
        If request.PageIndex > safeTotalPages AndAlso safeTotalPages > 0 Then
            _paginationState.SetPageIndex(safeTotalPages)
            request = CreatePaginationRequest(searchText)
            result = fetchPage(request)
            If result Is Nothing Then
                result = PagedQueryResult.Empty(request)
            End If
        End If

        ApplyPaginationResult(result)
        Return result
    End Function

    Protected Sub ApplyPaginationResult(result As PagedQueryResult)
        If Not SupportsPagination Then Return
        EnsurePaginationState()
        _paginationState.ApplyResult(result)
        SyncPaginationUi()
    End Sub

    Protected Sub ReloadData()
        Dim searchText As String = NormalizeSearchText(txtSearch.Text)
        DGVtable.SuspendLayout()

        Try
            LoadModuleData(searchText)
            DGVtable.ClearSelection()
            If SupportsPagination Then
                SyncPaginationUi()
            End If
        Finally
            DGVtable.ResumeLayout()
        End Try
    End Sub

    Protected Sub SetSearchLabel(text As String)
        lblSearch.Text = text
    End Sub

    Protected Sub ConfigurePrimaryButtons(showAdd As Boolean, showPrint As Boolean)
        btnAdd.Visible = showAdd
        BtnPrint.Visible = showPrint

        If showAdd AndAlso showPrint Then
            btnAdd.Location = New Point(1288, 35)
            BtnPrint.Location = New Point(1412, 35)
        ElseIf showAdd Then
            btnAdd.Location = New Point(1419, 35)
        ElseIf showPrint Then
            BtnPrint.Location = New Point(1412, 35)
        End If
    End Sub

    Protected Sub SetPageDisplay(currentPage As Integer, totalPages As Integer)
        lblPage.Text = $"Page {currentPage} of {totalPages}"
    End Sub

    Protected Sub SyncPaginationUi()
        If SupportsPagination Then
            EnsurePaginationState()
            SetPageDisplay(_paginationState.PageIndex, _paginationState.TotalPages)
            btnPreviousPage.Enabled = _paginationState.CanGoPrevious
            btnNextPage.Enabled = _paginationState.CanGoNext
            Return
        End If

        SetPageDisplay(1, 1)
        btnPreviousPage.Enabled = False
        btnNextPage.Enabled = False
    End Sub

    Protected Sub SetActiveSidebarButton(activeButton As Guna2Button)
        For Each btn As Guna2Button In GetSidebarNavigationButtons()
            If btn Is Nothing Then Continue For

            Dim isActive As Boolean = btn Is activeButton
            btn.FillColor = If(isActive, _activeFillColor, _inactiveFillColor)
            btn.HoverState.FillColor = _activeFillColor
        Next
    End Sub

    Private Iterator Function GetSidebarNavigationButtons() As IEnumerable(Of Guna2Button)
        Yield btnHome
        Yield btnFileMaintenance
        Yield btnDelivery
        Yield btnInventory
        Yield btnPos
        Yield btnTransaction
        Yield btnReturns
        Yield btnReports
        Yield btnAuditTrail
    End Function

    Protected Sub ApplyStaffSidebarRestrictions()
        btnPos.Visible = False
        btnAuditTrail.Visible = False
    End Sub

    Protected Sub ApplyCashierSidebarRestrictions()
        btnFileMaintenance.Visible = True
        btnFileMaintenance.Text = "Discount"
        btnDelivery.Visible = False
        btnReturns.Visible = False
        btnAuditTrail.Visible = False
    End Sub

    Protected Function IsAdminUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "admin", StringComparison.OrdinalIgnoreCase)
    End Function

    Protected Function IsStaffUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "staff", StringComparison.OrdinalIgnoreCase)
    End Function

    Protected Function IsCashierUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "cashier", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub ApplySidebarUserPanel()
        lblFirstname.Text = ResolveSidebarFirstName()
        lblUserLevel.Text = ResolveSidebarRoleDisplay()
    End Sub

    Private Sub ModuleListBaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySidebarUserPanel()
        btnFileMaintenance.Text = "File Maintenance"
        SetSearchLabel(SearchCaption)
        txtSearch.PlaceholderText = SearchPlaceholder
        ConfigurePrimaryButtons(ShowAddButton, ShowPrintButton)
        ConfigureModulePermissions()

        If IsCashierUser() Then
            ApplyCashierSidebarNavigationOrder(Guna2Panel2, btnHome, btnPos, btnInventory, btnFileMaintenance, btnTransaction, btnReports, btnLogout)
        ElseIf IsStaffUser() Then
            ApplyStaffSidebarNavigationOrder(Guna2Panel2, btnHome, btnFileMaintenance, btnDelivery, btnReturns, btnInventory, btnTransaction, btnReports, btnLogout)
        End If

        SetActiveSidebarButton(GetSidebarButton(CurrentModuleTab))
        EnsurePaginationState()
        SyncPaginationUi()
        ReloadData()
    End Sub

    Private Function GetSidebarButton(tab As ModuleTab) As Guna2Button
        Select Case tab
            Case ModuleTab.DiscountTab
                Return btnFileMaintenance
            Case ModuleTab.DeliveriesTab
                Return btnDelivery
            Case ModuleTab.InventoryTab
                Return btnInventory
            Case ModuleTab.TransactionsTab
                Return btnTransaction
            Case ModuleTab.SupplierReturnsTab
                Return btnReturns
            Case ModuleTab.AuditTrailTab
                Return btnAuditTrail
            Case Else
                Return Nothing
        End Select
    End Function

    Private Function BuildModuleForm(tab As ModuleTab) As Form
        Select Case tab
            Case ModuleTab.DiscountTab
                Return New FrmDiscountCashier()
            Case ModuleTab.DeliveriesTab
                Return New DeliveriesModuleForm()
            Case ModuleTab.InventoryTab
                Return New InventoryModuleForm()
            Case ModuleTab.TransactionsTab
                Return New TransactionsModuleForm()
            Case ModuleTab.SupplierReturnsTab
                Return New SupplierReturnsModuleForm()
            Case ModuleTab.AuditTrailTab
                Return New AuditTrailModuleForm()
            Case Else
                Return Nothing
        End Select
    End Function

    Protected Sub OpenForm(nextForm As Form)
        If nextForm Is Nothing Then Return
        nextForm.Show()
        Me.Hide()
    End Sub

    Private Sub NavigateToModule(targetTab As ModuleTab)
        If CurrentModuleTab = targetTab Then Return
        If Not CanNavigateToModule(targetTab) Then Return
        OpenForm(BuildModuleForm(targetTab))
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ReloadData()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If Not SupportsPagination Then Return
        EnsurePaginationState()

        If _paginationState.TryMovePrevious() Then
            ReloadData()
        Else
            SyncPaginationUi()
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If Not SupportsPagination Then Return
        EnsurePaginationState()

        If _paginationState.TryMoveNext() Then
            ReloadData()
        Else
            SyncPaginationUi()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        HandleAddAction()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        HandlePrintAction()
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        If IsCashierUser() Then
            OpenForm(New FrmDashboardCashier())
            Return
        End If

        If IsStaffUser() Then
            OpenForm(New FrmDashboardStaff())
            Return
        End If

        OpenForm(New frmHome())
    End Sub

    Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        If IsCashierUser() Then
            If CurrentModuleTab = ModuleTab.DiscountTab Then Return
            OpenForm(New FrmDiscountCashier())
        ElseIf IsStaffUser() Then
            OpenForm(New FileMaintenance.Category())
        Else
            OpenForm(New FileMaintenance.User())
        End If
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        NavigateToModule(ModuleTab.DeliveriesTab)
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        NavigateToModule(ModuleTab.InventoryTab)
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        OpenForm(New FrmPOS())
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        NavigateToModule(ModuleTab.TransactionsTab)
    End Sub

    Private Sub btnReturns_Click(sender As Object, e As EventArgs) Handles btnReturns.Click
        NavigateToModule(ModuleTab.SupplierReturnsTab)
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        OpenForm(New FrmReports())
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        NavigateToModule(ModuleTab.AuditTrailTab)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                      "Logout",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging out: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub EnsurePaginationState()
        If Not SupportsPagination Then Return
        If _paginationState Is Nothing Then
            _paginationState = New PaginationState(DefaultPageSize)
        End If
    End Sub

    Private Function NormalizeSearchText(value As String) As String
        Return If(value, String.Empty).Trim()
    End Function

End Class
