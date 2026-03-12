Imports System.Data.SqlClient
Imports Guna.UI2.WinForms

Namespace FileMaintenance
    Public MustInherit Class FileMaintenanceBaseForm
        Protected Enum MaintenanceTab
            UserTab
            CategoryTab
            SizeTab
            BrandTab
            ColorTab
            ProductTab
            SupplierTab
            DiscountTab
            VatTab
        End Enum

        Private ReadOnly ActiveFillColor As Drawing.Color = Drawing.Color.FromArgb(85, 95, 95)
        Private ReadOnly InactiveFillColor As Drawing.Color = Drawing.Color.FromArgb(75, 78, 75)
        Private ReadOnly HoverFillColor As Drawing.Color = Drawing.Color.FromArgb(88, 92, 88)
        Private ReadOnly ActiveBorderColor As Drawing.Color = Drawing.Color.FromArgb(135, 160, 156)
        Private ReadOnly InactiveBorderColor As Drawing.Color = Drawing.Color.FromArgb(60, 63, 60)
        Private ReadOnly HoverBorderColor As Drawing.Color = Drawing.Color.FromArgb(100, 110, 100)
        Private Const ActiveBorderThickness As Integer = 2
        Private Const InactiveBorderThickness As Integer = 1
        Private ReadOnly SidebarActiveFillColor As Drawing.Color = Drawing.Color.FromArgb(30, 32, 30)
        Private ReadOnly SidebarInactiveFillColor As Drawing.Color = Drawing.Color.Black
        Private ReadOnly vatEditButton As New Guna2Button()
        Private _activeTabButton As Guna2Button
        Private _isReady As Boolean = False
        Private _paginationState As PaginationState
        Private _lastSearchText As String = String.Empty

        Protected MustOverride ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
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

        Protected Overridable ReadOnly Property ShowSearchSection As Boolean
            Get
                Return True
            End Get
        End Property

        Protected MustOverride Sub InitializeServices()
        Protected MustOverride Sub LoadTableData(searchText As String)
        Protected MustOverride Sub HandlePrimaryAction()

        Protected Function ShowOwnedEntryModal(entryForm As Form) As DialogResult
            If entryForm Is Nothing OrElse entryForm.IsDisposed Then
                Return DialogResult.Cancel
            End If

            Return EntryModalHost.ShowLikeUserProduct(Me, entryForm)
        End Function

        Protected Overridable ReadOnly Property UseEditPrimaryAction As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overridable ReadOnly Property SupportsPagination As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overridable ReadOnly Property DefaultPageSize As Integer
            Get
                Return 10
            End Get
        End Property

        Protected Overridable Sub GoToPreviousPage()
            If Not SupportsPagination Then Return
            EnsurePaginationState()

            If _paginationState.TryMovePrevious() Then
                ReloadData()
            End If
        End Sub

        Protected Overridable Sub GoToNextPage()
            If Not SupportsPagination Then Return
            EnsurePaginationState()

            If _paginationState.TryMoveNext() Then
                ReloadData()
            End If
        End Sub

        Protected Overridable Function GetPaginationText() As String
            If Not SupportsPagination Then
                Return "Page 1 of 1"
            End If

            EnsurePaginationState()
            Return _paginationState.GetPageLabel()
        End Function

        Protected Overridable Function CanLoadTableData() As Boolean
            Return DGVtable IsNot Nothing
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
        End Sub

        Protected Sub ReloadData()
            If Not _isReady Then Return
            If Not CanLoadTableData() Then Return

            Dim searchText As String = String.Empty
            If ShowSearchSection AndAlso txtSearch IsNot Nothing Then
                searchText = txtSearch.Text.Trim()
            End If

            If DGVtable IsNot Nothing Then
                DGVtable.SuspendLayout()
            End If

            Try
                LoadTableData(searchText)
                UpdatePaginationState()
                If DGVtable IsNot Nothing Then
                    DGVtable.ClearSelection()
                End If
            Finally
                If DGVtable IsNot Nothing Then
                    DGVtable.ResumeLayout()
                End If
            End Try
        End Sub

        Protected Sub ApplyDefaultGridLayout()
            ApplyStandardGridLayout(DGVtable)
        End Sub

        Private Sub ApplySidebarUserPanel()
            lblFirstname.Text = ResolveSidebarFirstName()
            lblUserLevel.Text = ResolveSidebarRoleDisplay()
        End Sub

        Private Sub FileMaintenanceBaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ApplySidebarUserPanel()
            ConfigureTopNavigationStyles()
            ConfigureRoleVisibility()
            SetMainSidebarActiveButton(btnFileMaintenance)
            ConfigureSearchSection()
            ConfigurePrimaryActionButton()
            EnsurePaginationState()
            UpdatePaginationState()
        End Sub

        Protected Overrides Sub OnShown(e As EventArgs)
            MyBase.OnShown(e)

            If Not _isReady Then
                Try
                    InitializeServices()
                    _isReady = True
                Catch ex As Exception
                    _isReady = False
                    MessageBox.Show("Failed to initialize services: " & ex.Message,
                                    "Initialization Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    Return
                End Try
            End If

            ReloadData()
        End Sub

        Private Sub ConfigureSearchSection()
            RemoveHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged

            If ShowSearchSection Then
                lblSearch.Visible = True
                txtSearch.Visible = True
                txtSearch.TabStop = True
                lblSearch.Text = SearchCaption
                txtSearch.PlaceholderText = SearchPlaceholder
                AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
                Return
            End If

            txtSearch.Clear()
            lblSearch.Visible = False
            txtSearch.Visible = False
            txtSearch.TabStop = False

            Dim panelShift As Integer = Guna2Panel4.Top - Guna2Panel3.Top
            If panelShift > 0 Then
                Guna2Panel4.Top -= panelShift
                Guna2Panel4.Height += panelShift
                DGVtable.Height += panelShift
            End If

            Guna2Panel3.Visible = False

            If btnAdd.Parent IsNot Guna2Panel4 Then
                Guna2Panel4.Controls.Add(btnAdd)
            End If

            btnAdd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            PositionPrimaryActionButton(btnAdd)
        End Sub

        Private Sub ConfigurePrimaryActionButton()
            If UseEditPrimaryAction Then
                btnAdd.Visible = False
                ConfigureVatEditOverlay()
            Else
                vatEditButton.Visible = False
                btnAdd.Text = " Add"
                btnAdd.Image = My.Resources.add
                btnAdd.Visible = True
            End If
        End Sub

        Private Sub ConfigureVatEditOverlay()
            vatEditButton.Animated = btnAdd.Animated
            vatEditButton.AutoRoundedCorners = btnAdd.AutoRoundedCorners
            vatEditButton.BackColor = btnAdd.BackColor
            vatEditButton.Cursor = btnAdd.Cursor
            vatEditButton.DefaultAutoSize = btnAdd.DefaultAutoSize
            vatEditButton.FillColor = btnAdd.FillColor
            vatEditButton.Font = btnAdd.Font
            vatEditButton.ForeColor = btnAdd.ForeColor
            vatEditButton.HoverState.FillColor = btnAdd.HoverState.FillColor
            vatEditButton.Image = My.Resources.edit
            vatEditButton.IndicateFocus = btnAdd.IndicateFocus
            vatEditButton.Location = btnAdd.Location
            vatEditButton.Name = "btnEditAction"
            vatEditButton.Padding = btnAdd.Padding
            vatEditButton.Size = btnAdd.Size
            vatEditButton.TabIndex = btnAdd.TabIndex
            vatEditButton.Text = " Edit"
            vatEditButton.UseTransparentBackground = btnAdd.UseTransparentBackground

            RemoveHandler vatEditButton.Click, AddressOf VatEditButton_Click
            AddHandler vatEditButton.Click, AddressOf VatEditButton_Click

            Dim actionHost As Control = If(ShowSearchSection, CType(Guna2Panel3, Control), CType(Guna2Panel4, Control))
            If vatEditButton.Parent IsNot actionHost Then
                actionHost.Controls.Add(vatEditButton)
            End If

            vatEditButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            PositionPrimaryActionButton(vatEditButton)
            vatEditButton.BringToFront()
            vatEditButton.Visible = True
        End Sub

        Private Sub PositionPrimaryActionButton(button As Guna2Button)
            If button Is Nothing Then Return

            If ShowSearchSection Then
                button.Location = btnAdd.Location
                Return
            End If

            Dim targetX As Integer = Guna2Panel4.Width - button.Width - 15
            If lblPage IsNot Nothing Then
                targetX = Math.Min(targetX, lblPage.Left - button.Width - 16)
            End If

            button.Location = New Point(Math.Max(17, targetX), 10)
        End Sub

        Private Sub VatEditButton_Click(sender As Object, e As EventArgs)
            HandlePrimaryAction()
        End Sub

        Private Sub ConfigureRoleVisibility()
            Dim role As String = FrmLogin.CurrentUser.Role
            btnFileMaintenance.Text = "File Maintenance"

            If String.Equals(role, "staff", StringComparison.OrdinalIgnoreCase) Then
                btnPos.Visible = False
                btnReturns.Visible = True
                btnAuditTrail.Visible = False

                btnUser.Visible = False
                btnDiscount.Visible = False
                btnVat.Visible = False

                ApplyStaffSidebarNavigationOrder(Guna2Panel2, btnHome, btnFileMaintenance, btnDelivery, btnReturns, btnInventory, btnTransaction, btnReports, btnLogout)
            ElseIf String.Equals(role, "cashier", StringComparison.OrdinalIgnoreCase) Then
                btnFileMaintenance.Text = "Discount"
                btnDelivery.Visible = False
                btnReturns.Visible = False
                btnAuditTrail.Visible = False

                btnUser.Visible = False
                btnCategory.Visible = False
                btnSize.Visible = False
                btnBrand.Visible = False
                btnColor.Visible = False
                btnProduct.Visible = False
                btnSupplier.Visible = False
                btnVat.Visible = False
                btnDiscount.Visible = True

                ApplyCashierSidebarNavigationOrder(Guna2Panel2, btnHome, btnPos, btnInventory, btnFileMaintenance, btnTransaction, btnReports, btnLogout)
            End If
        End Sub

        Private Sub SetMainSidebarActiveButton(activeButton As Guna2Button)
            For Each button As Guna2Button In GetMainSidebarButtons()
                If button Is Nothing Then Continue For

                Dim isActive As Boolean = button Is activeButton
                button.FillColor = If(isActive, SidebarActiveFillColor, SidebarInactiveFillColor)
                button.HoverState.FillColor = SidebarActiveFillColor
            Next
        End Sub

        Private Iterator Function GetMainSidebarButtons() As IEnumerable(Of Guna2Button)
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

        Private Sub ConfigureTopNavigationStyles()
            Dim buttonMap As New Dictionary(Of MaintenanceTab, Guna2Button) From {
                {MaintenanceTab.UserTab, btnUser},
                {MaintenanceTab.CategoryTab, btnCategory},
                {MaintenanceTab.SizeTab, btnSize},
                {MaintenanceTab.BrandTab, btnBrand},
                {MaintenanceTab.ColorTab, btnColor},
                {MaintenanceTab.ProductTab, btnProduct},
                {MaintenanceTab.SupplierTab, btnSupplier},
                {MaintenanceTab.DiscountTab, btnDiscount},
                {MaintenanceTab.VatTab, btnVat}
            }

            _activeTabButton = Nothing
            For Each pair In buttonMap
                ConfigureTabButtonInteraction(pair.Value)

                Dim isActive As Boolean = pair.Key = CurrentMaintenanceTab
                If isActive Then
                    _activeTabButton = pair.Value
                End If

                ApplyTabStyle(pair.Value, isActive)
            Next
        End Sub

        Private Sub ConfigureTabButtonInteraction(button As Guna2Button)
            If button Is Nothing Then Return

            button.Cursor = System.Windows.Forms.Cursors.Hand

            RemoveHandler button.MouseEnter, AddressOf TabButton_MouseEnter
            RemoveHandler button.MouseLeave, AddressOf TabButton_MouseLeave
            AddHandler button.MouseEnter, AddressOf TabButton_MouseEnter
            AddHandler button.MouseLeave, AddressOf TabButton_MouseLeave
        End Sub

        Private Sub ApplyTabStyle(button As Guna2Button, isActive As Boolean)
            If button Is Nothing Then Return

            Dim fillColor As Drawing.Color = If(isActive, ActiveFillColor, InactiveFillColor)
            Dim hoverColor As Drawing.Color = If(isActive, ActiveFillColor, HoverFillColor)
            Dim borderColor As Drawing.Color = If(isActive, ActiveBorderColor, InactiveBorderColor)
            Dim hoverBorderColor As Drawing.Color = If(isActive, ActiveBorderColor, hoverBorderColor)
            Dim borderThickness As Integer = If(isActive, ActiveBorderThickness, InactiveBorderThickness)

            button.FillColor = fillColor
            button.BorderColor = borderColor
            button.BorderThickness = borderThickness
            button.HoverState.FillColor = hoverColor
            button.HoverState.BorderColor = hoverBorderColor
        End Sub

        Private Sub TabButton_MouseEnter(sender As Object, e As EventArgs)
            Dim button As Guna2Button = TryCast(sender, Guna2Button)
            If button Is Nothing Then Return
            If button Is _activeTabButton Then Return

            button.FillColor = HoverFillColor
            button.BorderColor = HoverBorderColor
            button.BorderThickness = ActiveBorderThickness
        End Sub

        Private Sub TabButton_MouseLeave(sender As Object, e As EventArgs)
            Dim button As Guna2Button = TryCast(sender, Guna2Button)
            If button Is Nothing Then Return

            ApplyTabStyle(button, button Is _activeTabButton)
        End Sub

        Private Sub UpdatePaginationState()
            btnPreviousPage.Visible = SupportsPagination
            btnNextPage.Visible = SupportsPagination
            lblPage.Visible = SupportsPagination

            If SupportsPagination Then
                EnsurePaginationState()
                lblPage.Text = GetPaginationText()
                btnPreviousPage.Enabled = _paginationState.CanGoPrevious
                btnNextPage.Enabled = _paginationState.CanGoNext
            End If
        End Sub

        Private Sub EnsurePaginationState()
            If _paginationState Is Nothing Then
                _paginationState = New PaginationState(DefaultPageSize)
            End If
        End Sub

        Private Function NormalizeSearchText(searchText As String) As String
            Return If(searchText, String.Empty).Trim()
        End Function

        Private Sub OpenForm(nextForm As Form)
            If nextForm Is Nothing Then Return

            nextForm.Show()
            Me.Hide()
        End Sub

        Private Function BuildMaintenanceForm(tab As MaintenanceTab) As Form
            Select Case tab
                Case MaintenanceTab.UserTab
                    Return New User()
                Case MaintenanceTab.CategoryTab
                    Return New Category()
                Case MaintenanceTab.SizeTab
                    Return New Size()
                Case MaintenanceTab.BrandTab
                    Return New Brand()
                Case MaintenanceTab.ColorTab
                    Return New Color()
                Case MaintenanceTab.ProductTab
                    Return New Product()
                Case MaintenanceTab.SupplierTab
                    Return New Supplier()
                Case MaintenanceTab.DiscountTab
                    Return New Discount()
                Case MaintenanceTab.VatTab
                    Return New Vat()
                Case Else
                    Return Nothing
            End Select
        End Function

        Private Sub NavigateToMaintenanceTab(targetTab As MaintenanceTab)
            If CurrentMaintenanceTab = targetTab Then Return

            Dim role As String = FrmLogin.CurrentUser.Role
            Dim staffBlocked As Boolean = String.Equals(role, "staff", StringComparison.OrdinalIgnoreCase) AndAlso
                                         (targetTab = MaintenanceTab.UserTab OrElse targetTab = MaintenanceTab.DiscountTab OrElse targetTab = MaintenanceTab.VatTab)
            Dim cashierBlocked As Boolean = String.Equals(role, "cashier", StringComparison.OrdinalIgnoreCase) AndAlso
                                           targetTab <> MaintenanceTab.DiscountTab
            If staffBlocked OrElse cashierBlocked Then Return

            OpenForm(BuildMaintenanceForm(targetTab))
        End Sub

        Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            HandlePrimaryAction()
        End Sub

        Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
            If Not ShowSearchSection Then Return
            If Not _isReady Then Return
            ReloadData()
        End Sub

        Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
            If Not _isReady Then Return
            GoToPreviousPage()
            UpdatePaginationState()
        End Sub

        Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
            If Not _isReady Then Return
            GoToNextPage()
            UpdatePaginationState()
        End Sub

        Private Sub btnUser_Click(sender As Object, e As EventArgs) Handles btnUser.Click
            NavigateToMaintenanceTab(MaintenanceTab.UserTab)
        End Sub

        Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
            NavigateToMaintenanceTab(MaintenanceTab.CategoryTab)
        End Sub

        Private Sub btnSize_Click(sender As Object, e As EventArgs) Handles btnSize.Click
            NavigateToMaintenanceTab(MaintenanceTab.SizeTab)
        End Sub

        Private Sub btnBrand_Click(sender As Object, e As EventArgs) Handles btnBrand.Click
            NavigateToMaintenanceTab(MaintenanceTab.BrandTab)
        End Sub

        Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
            NavigateToMaintenanceTab(MaintenanceTab.ColorTab)
        End Sub

        Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
            NavigateToMaintenanceTab(MaintenanceTab.ProductTab)
        End Sub

        Private Sub btnSupplier_Click(sender As Object, e As EventArgs) Handles btnSupplier.Click
            NavigateToMaintenanceTab(MaintenanceTab.SupplierTab)
        End Sub

        Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
            NavigateToMaintenanceTab(MaintenanceTab.DiscountTab)
        End Sub

        Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
            NavigateToMaintenanceTab(MaintenanceTab.VatTab)
        End Sub

        Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
            SetMainSidebarActiveButton(btnHome)
            If String.Equals(FrmLogin.CurrentUser.Role, "cashier", StringComparison.OrdinalIgnoreCase) Then
                OpenForm(New FrmDashboardCashier())
                Return
            End If

            If String.Equals(FrmLogin.CurrentUser.Role, "staff", StringComparison.OrdinalIgnoreCase) Then
                OpenForm(New FrmDashboardStaff())
                Return
            End If

            OpenForm(New frmHome())
        End Sub

        Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
            SetMainSidebarActiveButton(btnFileMaintenance)
            Dim role As String = FrmLogin.CurrentUser.Role
            If String.Equals(role, "staff", StringComparison.OrdinalIgnoreCase) Then
                NavigateToMaintenanceTab(MaintenanceTab.CategoryTab)
            ElseIf String.Equals(role, "cashier", StringComparison.OrdinalIgnoreCase) Then
                OpenForm(New FrmDiscountCashier())
            Else
                NavigateToMaintenanceTab(MaintenanceTab.UserTab)
            End If
        End Sub

        Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
            SetMainSidebarActiveButton(btnDelivery)
            OpenForm(New DeliveriesModuleForm())
        End Sub

        Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
            SetMainSidebarActiveButton(btnInventory)
            OpenForm(New InventoryModuleForm())
        End Sub

        Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
            SetMainSidebarActiveButton(btnPos)
            OpenForm(New FrmPOS())
        End Sub

        Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
            SetMainSidebarActiveButton(btnTransaction)
            OpenForm(New TransactionsModuleForm())
        End Sub

        Private Sub btnReturns_Click(sender As Object, e As EventArgs) Handles btnReturns.Click
            SetMainSidebarActiveButton(btnReturns)
            OpenForm(New SupplierReturnsModuleForm())
        End Sub

        Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
            SetMainSidebarActiveButton(btnReports)
            OpenForm(New FrmReports())
        End Sub

        Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
            SetMainSidebarActiveButton(btnAuditTrail)
            OpenForm(New AuditTrailModuleForm())
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

        Private Sub Guna2Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel3.Paint

        End Sub
    End Class
End Namespace
