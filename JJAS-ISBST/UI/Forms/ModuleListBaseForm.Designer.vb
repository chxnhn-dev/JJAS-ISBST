<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ModuleListBaseForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ModuleListBaseForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.switchtimer = New System.Windows.Forms.Timer(Me.components)
        Me.panelmenu = New Guna.UI2.WinForms.Guna2Panel()
        Me.btnLogout = New Guna.UI2.WinForms.Guna2Button()
        Me.btnAuditTrail = New Guna.UI2.WinForms.Guna2Button()
        Me.btnTransaction = New Guna.UI2.WinForms.Guna2Button()
        Me.btnPos = New Guna.UI2.WinForms.Guna2Button()
        Me.btnInventory = New Guna.UI2.WinForms.Guna2Button()
        Me.btnDelivery = New Guna.UI2.WinForms.Guna2Button()
        Me.btnFileMaintenance = New Guna.UI2.WinForms.Guna2Button()
        Me.btnHome = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel21 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel17 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel3 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblSearch = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        Me.BtnPrint = New Guna.UI2.WinForms.Guna2Button()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Guna2Panel4 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblPage = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnNextPage = New Guna.UI2.WinForms.Guna2Button()
        Me.btnPreviousPage = New Guna.UI2.WinForms.Guna2Button()
        Me.DGVtable = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.panelmenu.SuspendLayout()
        Me.Guna2Panel3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2Panel4.SuspendLayout()
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'switchtimer
        '
        '
        'panelmenu
        '
        Me.panelmenu.Controls.Add(Me.btnLogout)
        Me.panelmenu.Controls.Add(Me.btnAuditTrail)
        Me.panelmenu.Controls.Add(Me.btnTransaction)
        Me.panelmenu.Controls.Add(Me.btnPos)
        Me.panelmenu.Controls.Add(Me.btnInventory)
        Me.panelmenu.Controls.Add(Me.btnDelivery)
        Me.panelmenu.Controls.Add(Me.btnFileMaintenance)
        Me.panelmenu.Controls.Add(Me.btnHome)
        Me.panelmenu.Controls.Add(Me.Guna2Panel21)
        Me.panelmenu.Controls.Add(Me.Guna2Panel17)
        Me.panelmenu.Controls.Add(Me.PictureBox2)
        Me.panelmenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelmenu.FillColor = System.Drawing.Color.Black
        Me.panelmenu.Location = New System.Drawing.Point(0, 0)
        Me.panelmenu.Name = "panelmenu"
        Me.panelmenu.Size = New System.Drawing.Size(234, 885)
        Me.panelmenu.TabIndex = 103
        '
        'btnLogout
        '
        Me.btnLogout.Animated = True
        Me.btnLogout.AutoRoundedCorners = True
        Me.btnLogout.BackColor = System.Drawing.Color.Transparent
        Me.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogout.DefaultAutoSize = True
        Me.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnLogout.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnLogout.FillColor = System.Drawing.Color.Black
        Me.btnLogout.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnLogout.IndicateFocus = True
        Me.btnLogout.Location = New System.Drawing.Point(17, 462)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnLogout.Size = New System.Drawing.Size(201, 44)
        Me.btnLogout.TabIndex = 9
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseTransparentBackground = True
        '
        'btnAuditTrail
        '
        Me.btnAuditTrail.Animated = True
        Me.btnAuditTrail.AutoRoundedCorners = True
        Me.btnAuditTrail.BackColor = System.Drawing.Color.Transparent
        Me.btnAuditTrail.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAuditTrail.DefaultAutoSize = True
        Me.btnAuditTrail.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnAuditTrail.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnAuditTrail.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnAuditTrail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnAuditTrail.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnAuditTrail.FillColor = System.Drawing.Color.Black
        Me.btnAuditTrail.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuditTrail.ForeColor = System.Drawing.Color.White
        Me.btnAuditTrail.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAuditTrail.IndicateFocus = True
        Me.btnAuditTrail.Location = New System.Drawing.Point(17, 418)
        Me.btnAuditTrail.Name = "btnAuditTrail"
        Me.btnAuditTrail.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnAuditTrail.Size = New System.Drawing.Size(201, 44)
        Me.btnAuditTrail.TabIndex = 8
        Me.btnAuditTrail.Text = "Audit Trail"
        Me.btnAuditTrail.UseTransparentBackground = True
        '
        'btnTransaction
        '
        Me.btnTransaction.Animated = True
        Me.btnTransaction.AutoRoundedCorners = True
        Me.btnTransaction.BackColor = System.Drawing.Color.Transparent
        Me.btnTransaction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnTransaction.DefaultAutoSize = True
        Me.btnTransaction.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnTransaction.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnTransaction.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnTransaction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnTransaction.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnTransaction.FillColor = System.Drawing.Color.Black
        Me.btnTransaction.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransaction.ForeColor = System.Drawing.Color.White
        Me.btnTransaction.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnTransaction.IndicateFocus = True
        Me.btnTransaction.Location = New System.Drawing.Point(17, 374)
        Me.btnTransaction.Name = "btnTransaction"
        Me.btnTransaction.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnTransaction.Size = New System.Drawing.Size(201, 44)
        Me.btnTransaction.TabIndex = 7
        Me.btnTransaction.Text = "Transaction"
        Me.btnTransaction.UseTransparentBackground = True
        '
        'btnPos
        '
        Me.btnPos.Animated = True
        Me.btnPos.AutoRoundedCorners = True
        Me.btnPos.BackColor = System.Drawing.Color.Transparent
        Me.btnPos.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPos.DefaultAutoSize = True
        Me.btnPos.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPos.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPos.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPos.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPos.FillColor = System.Drawing.Color.Black
        Me.btnPos.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPos.ForeColor = System.Drawing.Color.White
        Me.btnPos.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPos.IndicateFocus = True
        Me.btnPos.Location = New System.Drawing.Point(17, 330)
        Me.btnPos.Name = "btnPos"
        Me.btnPos.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnPos.Size = New System.Drawing.Size(201, 44)
        Me.btnPos.TabIndex = 6
        Me.btnPos.Text = "POS"
        Me.btnPos.UseTransparentBackground = True
        '
        'btnInventory
        '
        Me.btnInventory.Animated = True
        Me.btnInventory.AutoRoundedCorners = True
        Me.btnInventory.BackColor = System.Drawing.Color.Transparent
        Me.btnInventory.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnInventory.DefaultAutoSize = True
        Me.btnInventory.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnInventory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnInventory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnInventory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnInventory.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnInventory.FillColor = System.Drawing.Color.Black
        Me.btnInventory.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInventory.ForeColor = System.Drawing.Color.White
        Me.btnInventory.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnInventory.IndicateFocus = True
        Me.btnInventory.Location = New System.Drawing.Point(17, 286)
        Me.btnInventory.Name = "btnInventory"
        Me.btnInventory.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnInventory.Size = New System.Drawing.Size(201, 44)
        Me.btnInventory.TabIndex = 5
        Me.btnInventory.Text = "Inventory"
        Me.btnInventory.UseTransparentBackground = True
        '
        'btnDelivery
        '
        Me.btnDelivery.Animated = True
        Me.btnDelivery.AutoRoundedCorners = True
        Me.btnDelivery.BackColor = System.Drawing.Color.Transparent
        Me.btnDelivery.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelivery.DefaultAutoSize = True
        Me.btnDelivery.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnDelivery.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnDelivery.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnDelivery.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnDelivery.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnDelivery.FillColor = System.Drawing.Color.Black
        Me.btnDelivery.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelivery.ForeColor = System.Drawing.Color.White
        Me.btnDelivery.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelivery.IndicateFocus = True
        Me.btnDelivery.Location = New System.Drawing.Point(17, 242)
        Me.btnDelivery.Name = "btnDelivery"
        Me.btnDelivery.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnDelivery.Size = New System.Drawing.Size(201, 44)
        Me.btnDelivery.TabIndex = 4
        Me.btnDelivery.Text = "Delivery"
        Me.btnDelivery.UseTransparentBackground = True
        '
        'btnFileMaintenance
        '
        Me.btnFileMaintenance.Animated = True
        Me.btnFileMaintenance.AutoRoundedCorners = True
        Me.btnFileMaintenance.BackColor = System.Drawing.Color.Transparent
        Me.btnFileMaintenance.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFileMaintenance.DefaultAutoSize = True
        Me.btnFileMaintenance.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnFileMaintenance.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnFileMaintenance.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnFileMaintenance.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnFileMaintenance.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnFileMaintenance.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFileMaintenance.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFileMaintenance.ForeColor = System.Drawing.Color.White
        Me.btnFileMaintenance.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFileMaintenance.IndicateFocus = True
        Me.btnFileMaintenance.Location = New System.Drawing.Point(17, 198)
        Me.btnFileMaintenance.Name = "btnFileMaintenance"
        Me.btnFileMaintenance.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnFileMaintenance.Size = New System.Drawing.Size(201, 44)
        Me.btnFileMaintenance.TabIndex = 3
        Me.btnFileMaintenance.Text = "File Maintenance"
        Me.btnFileMaintenance.UseTransparentBackground = True
        '
        'btnHome
        '
        Me.btnHome.Animated = True
        Me.btnHome.AutoRoundedCorners = True
        Me.btnHome.BackColor = System.Drawing.Color.Transparent
        Me.btnHome.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnHome.DefaultAutoSize = True
        Me.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnHome.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnHome.FillColor = System.Drawing.Color.Black
        Me.btnHome.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHome.ForeColor = System.Drawing.Color.White
        Me.btnHome.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnHome.IndicateFocus = True
        Me.btnHome.Location = New System.Drawing.Point(17, 154)
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnHome.Size = New System.Drawing.Size(201, 44)
        Me.btnHome.TabIndex = 2
        Me.btnHome.Text = "Home"
        Me.btnHome.UseTransparentBackground = True
        '
        'Guna2Panel21
        '
        Me.Guna2Panel21.Dock = System.Windows.Forms.DockStyle.Right
        Me.Guna2Panel21.FillColor = System.Drawing.Color.Black
        Me.Guna2Panel21.Location = New System.Drawing.Point(218, 154)
        Me.Guna2Panel21.Name = "Guna2Panel21"
        Me.Guna2Panel21.Size = New System.Drawing.Size(16, 731)
        Me.Guna2Panel21.TabIndex = 76
        '
        'Guna2Panel17
        '
        Me.Guna2Panel17.Dock = System.Windows.Forms.DockStyle.Left
        Me.Guna2Panel17.FillColor = System.Drawing.Color.Black
        Me.Guna2Panel17.Location = New System.Drawing.Point(0, 154)
        Me.Guna2Panel17.Name = "Guna2Panel17"
        Me.Guna2Panel17.Size = New System.Drawing.Size(17, 731)
        Me.Guna2Panel17.TabIndex = 75
        '
        'Guna2Panel3
        '
        Me.Guna2Panel3.BorderRadius = 15
        Me.Guna2Panel3.Controls.Add(Me.lblSearch)
        Me.Guna2Panel3.Controls.Add(Me.txtSearch)
        Me.Guna2Panel3.Controls.Add(Me.btnAdd)
        Me.Guna2Panel3.Controls.Add(Me.BtnPrint)
        Me.Guna2Panel3.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel3.Location = New System.Drawing.Point(241, 12)
        Me.Guna2Panel3.Name = "Guna2Panel3"
        Me.Guna2Panel3.Size = New System.Drawing.Size(1548, 127)
        Me.Guna2Panel3.TabIndex = 104
        '
        'lblSearch
        '
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(17, 19)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(161, 27)
        Me.lblSearch.TabIndex = 54
        Me.lblSearch.Text = "Search Delivery:"
        '
        'txtSearch
        '
        Me.txtSearch.Animated = True
        Me.txtSearch.AutoRoundedCorners = True
        Me.txtSearch.BackColor = System.Drawing.Color.Transparent
        Me.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearch.DefaultText = ""
        Me.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.txtSearch.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(17, 54)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.PlaceholderText = "Nike" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Adidas" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Puma"
        Me.txtSearch.SelectedText = ""
        Me.txtSearch.Size = New System.Drawing.Size(470, 36)
        Me.txtSearch.TabIndex = 0
        '
        'BtnPrint
        '
        Me.BtnPrint.Animated = True
        Me.BtnPrint.AutoRoundedCorners = True
        Me.BtnPrint.BackColor = System.Drawing.Color.Transparent
        Me.BtnPrint.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnPrint.DefaultAutoSize = True
        Me.BtnPrint.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.BtnPrint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.BtnPrint.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.BtnPrint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.BtnPrint.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BtnPrint.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.BtnPrint.ForeColor = System.Drawing.Color.White
        Me.BtnPrint.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.BtnPrint.Image = Global.JJAS_ISBST.My.Resources.Resources.printer
        Me.BtnPrint.IndicateFocus = True
        Me.BtnPrint.Location = New System.Drawing.Point(1412, 35)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.BtnPrint.Size = New System.Drawing.Size(120, 55)
        Me.BtnPrint.TabIndex = 56
        Me.BtnPrint.Text = " Print"
        Me.BtnPrint.UseTransparentBackground = True
        '
        'btnAdd
        '
        Me.btnAdd.Animated = True
        Me.btnAdd.AutoRoundedCorners = True
        Me.btnAdd.BackColor = System.Drawing.Color.Transparent
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.DefaultAutoSize = True
        Me.btnAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnAdd.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnAdd.Image = Global.JJAS_ISBST.My.Resources.Resources.add
        Me.btnAdd.IndicateFocus = True
        Me.btnAdd.Location = New System.Drawing.Point(1419, 35)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnAdd.Size = New System.Drawing.Size(113, 55)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = " Add"
        Me.btnAdd.UseTransparentBackground = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(234, 154)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 57
        Me.PictureBox2.TabStop = False
        '
        'Guna2Panel4
        '
        Me.Guna2Panel4.BorderRadius = 15
        Me.Guna2Panel4.Controls.Add(Me.lblPage)
        Me.Guna2Panel4.Controls.Add(Me.btnNextPage)
        Me.Guna2Panel4.Controls.Add(Me.btnPreviousPage)
        Me.Guna2Panel4.Controls.Add(Me.DGVtable)
        Me.Guna2Panel4.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel4.Location = New System.Drawing.Point(240, 145)
        Me.Guna2Panel4.Name = "Guna2Panel4"
        Me.Guna2Panel4.Size = New System.Drawing.Size(1548, 728)
        Me.Guna2Panel4.TabIndex = 105
        '
        'lblPage
        '
        Me.lblPage.BackColor = System.Drawing.Color.Transparent
        Me.lblPage.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPage.ForeColor = System.Drawing.Color.White
        Me.lblPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPage.Location = New System.Drawing.Point(1443, 36)
        Me.lblPage.Name = "lblPage"
        Me.lblPage.Size = New System.Drawing.Size(81, 20)
        Me.lblPage.TabIndex = 33
        Me.lblPage.Text = "Page 1 of 1"
        '
        'btnNextPage
        '
        Me.btnNextPage.Animated = True
        Me.btnNextPage.AutoRoundedCorners = True
        Me.btnNextPage.BackColor = System.Drawing.Color.Transparent
        Me.btnNextPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNextPage.DefaultAutoSize = True
        Me.btnNextPage.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnNextPage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnNextPage.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnNextPage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnNextPage.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnNextPage.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNextPage.ForeColor = System.Drawing.Color.White
        Me.btnNextPage.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnNextPage.Image = Global.JJAS_ISBST.My.Resources.Resources.right_arrow
        Me.btnNextPage.IndicateFocus = True
        Me.btnNextPage.Location = New System.Drawing.Point(73, 18)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.btnNextPage.Size = New System.Drawing.Size(50, 38)
        Me.btnNextPage.TabIndex = 1
        Me.btnNextPage.Text = " "
        Me.btnNextPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.btnNextPage.UseTransparentBackground = True
        '
        'btnPreviousPage
        '
        Me.btnPreviousPage.Animated = True
        Me.btnPreviousPage.AutoRoundedCorners = True
        Me.btnPreviousPage.BackColor = System.Drawing.Color.Transparent
        Me.btnPreviousPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPreviousPage.DefaultAutoSize = True
        Me.btnPreviousPage.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPreviousPage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPreviousPage.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPreviousPage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPreviousPage.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnPreviousPage.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPreviousPage.ForeColor = System.Drawing.Color.White
        Me.btnPreviousPage.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnPreviousPage.Image = Global.JJAS_ISBST.My.Resources.Resources.left_arrow
        Me.btnPreviousPage.IndicateFocus = True
        Me.btnPreviousPage.Location = New System.Drawing.Point(17, 18)
        Me.btnPreviousPage.Name = "btnPreviousPage"
        Me.btnPreviousPage.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.btnPreviousPage.Size = New System.Drawing.Size(50, 38)
        Me.btnPreviousPage.TabIndex = 0
        Me.btnPreviousPage.Text = " "
        Me.btnPreviousPage.UseTransparentBackground = True
        '
        'DGVtable
        '
        Me.DGVtable.AllowUserToAddRows = False
        Me.DGVtable.AllowUserToDeleteRows = False
        Me.DGVtable.AllowUserToResizeColumns = False
        Me.DGVtable.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.DGVtable.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVtable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DGVtable.ColumnHeadersHeight = 4
        Me.DGVtable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGVtable.DefaultCellStyle = DataGridViewCellStyle3
        Me.DGVtable.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.Location = New System.Drawing.Point(17, 76)
        Me.DGVtable.Margin = New System.Windows.Forms.Padding(4)
        Me.DGVtable.Name = "DGVtable"
        Me.DGVtable.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVtable.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DGVtable.RowHeadersVisible = False
        Me.DGVtable.RowHeadersWidth = 51
        Me.DGVtable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        Me.DGVtable.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.DGVtable.Size = New System.Drawing.Size(1516, 636)
        Me.DGVtable.TabIndex = 2
        Me.DGVtable.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.DGVtable.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.DGVtable.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.DGVtable.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.DGVtable.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.DGVtable.ThemeStyle.BackColor = System.Drawing.Color.White
        Me.DGVtable.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.DGVtable.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DGVtable.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.DGVtable.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        Me.DGVtable.ThemeStyle.HeaderStyle.Height = 4
        Me.DGVtable.ThemeStyle.ReadOnly = True
        Me.DGVtable.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.DGVtable.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DGVtable.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DGVtable.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.DGVtable.ThemeStyle.RowsStyle.Height = 22
        Me.DGVtable.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'ModuleListBaseForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1800, 885)
        Me.Controls.Add(Me.Guna2Panel4)
        Me.Controls.Add(Me.Guna2Panel3)
        Me.Controls.Add(Me.panelmenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ModuleListBaseForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ModuleListBaseForm"
        Me.panelmenu.ResumeLayout(False)
        Me.panelmenu.PerformLayout()
        Me.Guna2Panel3.ResumeLayout(False)
        Me.Guna2Panel3.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2Panel4.ResumeLayout(False)
        Me.Guna2Panel4.PerformLayout()
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents switchtimer As Timer
    Friend WithEvents panelmenu As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnLogout As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnAuditTrail As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnTransaction As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPos As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnInventory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnDelivery As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnFileMaintenance As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnHome As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel21 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Panel17 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents Guna2Panel3 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblSearch As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BtnPrint As Guna.UI2.WinForms.Guna2Button

    Friend WithEvents Guna2Panel4 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblPage As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnNextPage As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPreviousPage As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents DGVtable As Guna.UI2.WinForms.Guna2DataGridView
End Class

