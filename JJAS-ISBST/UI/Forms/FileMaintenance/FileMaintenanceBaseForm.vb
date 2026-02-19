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
        Private ReadOnly vatEditButton As New Guna2Button()

        Protected MustOverride ReadOnly Property CurrentMaintenanceTab As MaintenanceTab
        Protected MustOverride ReadOnly Property SearchCaption As String
        Protected MustOverride ReadOnly Property SearchPlaceholder As String
        Protected MustOverride Sub LoadTableData(searchText As String)
        Protected MustOverride Sub HandlePrimaryAction()

        Protected Overridable ReadOnly Property UseEditPrimaryAction As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overridable ReadOnly Property SupportsPagination As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overridable Sub GoToPreviousPage()
        End Sub

        Protected Overridable Sub GoToNextPage()
        End Sub

        Protected Overridable Function GetPaginationText() As String
            Return "Page 1 of 1"
        End Function

        Protected Sub ReloadData()
            LoadTableData(txtSearch.Text.Trim())
            UpdatePaginationState()
            DGVtable.ClearSelection()
        End Sub

        Protected Sub ApplyDefaultGridLayout()
            ApplyStandardGridLayout(DGVtable)
        End Sub

        Private Sub FileMaintenanceBaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ConfigureTopNavigationStyles()
            ConfigureRoleVisibility()
            ConfigureSearchSection()
            ConfigurePrimaryActionButton()
            UpdatePaginationState()

            ReloadData()
        End Sub

        Private Sub ConfigureSearchSection()
            lblSearch.Text = SearchCaption
            txtSearch.PlaceholderText = SearchPlaceholder
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

            If vatEditButton.Parent Is Nothing Then
                Guna2Panel3.Controls.Add(vatEditButton)
            End If

            vatEditButton.BringToFront()
            vatEditButton.Visible = True
        End Sub

        Private Sub VatEditButton_Click(sender As Object, e As EventArgs)
            HandlePrimaryAction()
        End Sub

        Private Sub ConfigureRoleVisibility()
            Dim role As String = FrmLogin.CurrentUser.Role
            If String.Equals(role, "staff", StringComparison.OrdinalIgnoreCase) Then
                btnPos.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False

                btnUser.Visible = False
                btnDiscount.Visible = False
                btnVat.Visible = False
            End If
        End Sub

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

            For Each pair In buttonMap
                ApplyTabStyle(pair.Value, pair.Key = CurrentMaintenanceTab)
            Next
        End Sub

        Private Sub ApplyTabStyle(button As Guna2Button, isActive As Boolean)
            If button Is Nothing Then Return

            button.FillColor = If(isActive, ActiveFillColor, InactiveFillColor)
            button.HoverState.FillColor = If(isActive, ActiveFillColor, InactiveFillColor)
        End Sub

        Private Sub UpdatePaginationState()
            btnPreviousPage.Visible = SupportsPagination
            btnNextPage.Visible = SupportsPagination
            lblPage.Visible = SupportsPagination

            If SupportsPagination Then
                lblPage.Text = GetPaginationText()
            End If
        End Sub

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
            If staffBlocked Then Return

            OpenForm(BuildMaintenanceForm(targetTab))
        End Sub

        Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            HandlePrimaryAction()
        End Sub

        Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
            ReloadData()
        End Sub

        Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
            GoToPreviousPage()
            UpdatePaginationState()
        End Sub

        Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
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
            OpenForm(New FrmDashboard())
        End Sub

        Private Sub btnFileMaintenance_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
            Dim role As String = FrmLogin.CurrentUser.Role
            If String.Equals(role, "staff", StringComparison.OrdinalIgnoreCase) Then
                NavigateToMaintenanceTab(MaintenanceTab.CategoryTab)
            Else
                NavigateToMaintenanceTab(MaintenanceTab.UserTab)
            End If
        End Sub

        Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
            OpenForm(New Admin_Deliveries())
        End Sub

        Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
            OpenForm(New Admin_Inventory())
        End Sub

        Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
            OpenForm(New FrmPOS())
        End Sub

        Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
            OpenForm(New Admin_transaction())
        End Sub

        Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
            OpenForm(New Admin_AuditTrail())
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
    End Class
End Namespace
