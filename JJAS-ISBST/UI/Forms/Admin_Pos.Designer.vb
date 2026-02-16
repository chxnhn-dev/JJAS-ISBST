<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Admin_Pos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin_Pos))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.panelmenu = New System.Windows.Forms.Panel()
        Me.btnAuditTrail = New System.Windows.Forms.Button()
        Me.btnTransaction = New System.Windows.Forms.Button()
        Me.btnPos = New System.Windows.Forms.Button()
        Me.btnInventory = New System.Windows.Forms.Button()
        Me.btnDelivery = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnFileMaintenance = New System.Windows.Forms.Button()
        Me.btnHome = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DGVCart = New System.Windows.Forms.DataGridView()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.lblPlaceholder = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.switchtimer = New System.Windows.Forms.Timer(Me.components)
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtVatRate = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.txtDiscountAmt = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnNewTransaction = New System.Windows.Forms.Button()
        Me.btnPay = New System.Windows.Forms.Button()
        Me.txtChange = New System.Windows.Forms.TextBox()
        Me.txtCash = New System.Windows.Forms.TextBox()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.cbdiscount = New System.Windows.Forms.ComboBox()
        Me.txtSubtotal = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblTransactionNo = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.datetimer = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtVatable = New System.Windows.Forms.TextBox()
        Me.txtVat = New System.Windows.Forms.TextBox()
        Me.panelmenu.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel8.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.DGVCart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelmenu
        '
        Me.panelmenu.BackColor = System.Drawing.Color.Black
        Me.panelmenu.Controls.Add(Me.btnAuditTrail)
        Me.panelmenu.Controls.Add(Me.btnTransaction)
        Me.panelmenu.Controls.Add(Me.btnPos)
        Me.panelmenu.Controls.Add(Me.btnInventory)
        Me.panelmenu.Controls.Add(Me.btnDelivery)
        Me.panelmenu.Controls.Add(Me.btnLogout)
        Me.panelmenu.Controls.Add(Me.btnFileMaintenance)
        Me.panelmenu.Controls.Add(Me.btnHome)
        Me.panelmenu.Controls.Add(Me.PictureBox1)
        Me.panelmenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelmenu.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.panelmenu.Location = New System.Drawing.Point(0, 0)
        Me.panelmenu.Name = "panelmenu"
        Me.panelmenu.Size = New System.Drawing.Size(182, 900)
        Me.panelmenu.TabIndex = 87
        '
        'btnAuditTrail
        '
        Me.btnAuditTrail.BackColor = System.Drawing.Color.Black
        Me.btnAuditTrail.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAuditTrail.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnAuditTrail.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAuditTrail.FlatAppearance.BorderSize = 0
        Me.btnAuditTrail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAuditTrail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAuditTrail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuditTrail.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuditTrail.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnAuditTrail.Image = CType(resources.GetObject("btnAuditTrail.Image"), System.Drawing.Image)
        Me.btnAuditTrail.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAuditTrail.Location = New System.Drawing.Point(0, 347)
        Me.btnAuditTrail.Name = "btnAuditTrail"
        Me.btnAuditTrail.Padding = New System.Windows.Forms.Padding(0, 0, 30, 0)
        Me.btnAuditTrail.Size = New System.Drawing.Size(182, 37)
        Me.btnAuditTrail.TabIndex = 56
        Me.btnAuditTrail.Text = "  Audit Trail"
        Me.btnAuditTrail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuditTrail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAuditTrail.UseVisualStyleBackColor = False
        '
        'btnTransaction
        '
        Me.btnTransaction.BackColor = System.Drawing.Color.Black
        Me.btnTransaction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnTransaction.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnTransaction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnTransaction.FlatAppearance.BorderSize = 0
        Me.btnTransaction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnTransaction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTransaction.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransaction.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnTransaction.Image = CType(resources.GetObject("btnTransaction.Image"), System.Drawing.Image)
        Me.btnTransaction.Location = New System.Drawing.Point(0, 310)
        Me.btnTransaction.Name = "btnTransaction"
        Me.btnTransaction.Padding = New System.Windows.Forms.Padding(0, 0, 25, 0)
        Me.btnTransaction.Size = New System.Drawing.Size(182, 37)
        Me.btnTransaction.TabIndex = 53
        Me.btnTransaction.Text = " Transaction"
        Me.btnTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTransaction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnTransaction.UseVisualStyleBackColor = False
        '
        'btnPos
        '
        Me.btnPos.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPos.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPos.FlatAppearance.BorderSize = 0
        Me.btnPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPos.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPos.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnPos.Image = CType(resources.GetObject("btnPos.Image"), System.Drawing.Image)
        Me.btnPos.Location = New System.Drawing.Point(0, 273)
        Me.btnPos.Name = "btnPos"
        Me.btnPos.Padding = New System.Windows.Forms.Padding(0, 0, 30, 0)
        Me.btnPos.Size = New System.Drawing.Size(182, 37)
        Me.btnPos.TabIndex = 48
        Me.btnPos.Text = " POS"
        Me.btnPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPos.UseVisualStyleBackColor = False
        '
        'btnInventory
        '
        Me.btnInventory.BackColor = System.Drawing.Color.Black
        Me.btnInventory.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnInventory.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnInventory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnInventory.FlatAppearance.BorderSize = 0
        Me.btnInventory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnInventory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInventory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInventory.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnInventory.Image = CType(resources.GetObject("btnInventory.Image"), System.Drawing.Image)
        Me.btnInventory.Location = New System.Drawing.Point(0, 236)
        Me.btnInventory.Name = "btnInventory"
        Me.btnInventory.Size = New System.Drawing.Size(182, 37)
        Me.btnInventory.TabIndex = 47
        Me.btnInventory.Text = " Inventory"
        Me.btnInventory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnInventory.UseVisualStyleBackColor = False
        '
        'btnDelivery
        '
        Me.btnDelivery.BackColor = System.Drawing.Color.Black
        Me.btnDelivery.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelivery.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnDelivery.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelivery.FlatAppearance.BorderSize = 0
        Me.btnDelivery.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelivery.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelivery.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelivery.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelivery.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnDelivery.Image = CType(resources.GetObject("btnDelivery.Image"), System.Drawing.Image)
        Me.btnDelivery.Location = New System.Drawing.Point(0, 199)
        Me.btnDelivery.Name = "btnDelivery"
        Me.btnDelivery.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnDelivery.Size = New System.Drawing.Size(182, 37)
        Me.btnDelivery.TabIndex = 49
        Me.btnDelivery.Text = " Delivery"
        Me.btnDelivery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDelivery.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Black
        Me.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnLogout.Location = New System.Drawing.Point(0, 863)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Padding = New System.Windows.Forms.Padding(15, 0, 20, 0)
        Me.btnLogout.Size = New System.Drawing.Size(182, 37)
        Me.btnLogout.TabIndex = 41
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'btnFileMaintenance
        '
        Me.btnFileMaintenance.BackColor = System.Drawing.Color.Black
        Me.btnFileMaintenance.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFileMaintenance.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnFileMaintenance.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFileMaintenance.FlatAppearance.BorderSize = 0
        Me.btnFileMaintenance.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFileMaintenance.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFileMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFileMaintenance.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFileMaintenance.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnFileMaintenance.Image = CType(resources.GetObject("btnFileMaintenance.Image"), System.Drawing.Image)
        Me.btnFileMaintenance.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFileMaintenance.Location = New System.Drawing.Point(0, 162)
        Me.btnFileMaintenance.Name = "btnFileMaintenance"
        Me.btnFileMaintenance.Padding = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.btnFileMaintenance.Size = New System.Drawing.Size(182, 37)
        Me.btnFileMaintenance.TabIndex = 22
        Me.btnFileMaintenance.Text = " File Maintenance"
        Me.btnFileMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFileMaintenance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnFileMaintenance.UseVisualStyleBackColor = False
        '
        'btnHome
        '
        Me.btnHome.BackColor = System.Drawing.Color.Black
        Me.btnHome.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnHome.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnHome.FlatAppearance.BorderSize = 0
        Me.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHome.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHome.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnHome.Image = CType(resources.GetObject("btnHome.Image"), System.Drawing.Image)
        Me.btnHome.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnHome.Location = New System.Drawing.Point(0, 125)
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Padding = New System.Windows.Forms.Padding(0, 0, 65, 0)
        Me.btnHome.Size = New System.Drawing.Size(182, 37)
        Me.btnHome.TabIndex = 45
        Me.btnHome.Text = " Home"
        Me.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnHome.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(182, 125)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel8.Controls.Add(Me.Label12)
        Me.Panel8.Controls.Add(Me.Button1)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel8.Location = New System.Drawing.Point(182, 0)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(1418, 42)
        Me.Panel8.TabIndex = 88
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label12.Location = New System.Drawing.Point(7, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(90, 21)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Deliveries:"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.Location = New System.Drawing.Point(1380, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(38, 42)
        Me.Button1.TabIndex = 42
        Me.Button1.Text = "  "
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel3.Controls.Add(Me.DGVCart)
        Me.Panel3.Controls.Add(Me.btnEdit)
        Me.Panel3.Controls.Add(Me.lblPlaceholder)
        Me.Panel3.Controls.Add(Me.txtSearch)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Controls.Add(Me.btnRemove)
        Me.Panel3.Controls.Add(Me.btnAdd)
        Me.Panel3.Location = New System.Drawing.Point(188, 49)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1107, 839)
        Me.Panel3.TabIndex = 89
        '
        'DGVCart
        '
        Me.DGVCart.AllowUserToAddRows = False
        Me.DGVCart.AllowUserToDeleteRows = False
        Me.DGVCart.AllowUserToResizeColumns = False
        Me.DGVCart.AllowUserToResizeRows = False
        Me.DGVCart.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVCart.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGVCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVCart.Location = New System.Drawing.Point(9, 98)
        Me.DGVCart.Name = "DGVCart"
        Me.DGVCart.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVCart.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DGVCart.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.DGVCart.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DGVCart.RowTemplate.Height = 50
        Me.DGVCart.Size = New System.Drawing.Size(1086, 725)
        Me.DGVCart.TabIndex = 73
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnEdit.FlatAppearance.BorderSize = 0
        Me.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.Location = New System.Drawing.Point(93, 31)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 37)
        Me.btnEdit.TabIndex = 72
        Me.btnEdit.Text = "  Edit"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'lblPlaceholder
        '
        Me.lblPlaceholder.AutoSize = True
        Me.lblPlaceholder.BackColor = System.Drawing.Color.Transparent
        Me.lblPlaceholder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.lblPlaceholder.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlaceholder.ForeColor = System.Drawing.Color.Gray
        Me.lblPlaceholder.Image = CType(resources.GetObject("lblPlaceholder.Image"), System.Drawing.Image)
        Me.lblPlaceholder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPlaceholder.Location = New System.Drawing.Point(734, 31)
        Me.lblPlaceholder.Name = "lblPlaceholder"
        Me.lblPlaceholder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlaceholder.Size = New System.Drawing.Size(216, 25)
        Me.lblPlaceholder.TabIndex = 49
        Me.lblPlaceholder.Text = "     Search / Add Product"
        Me.lblPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.ForeColor = System.Drawing.Color.White
        Me.txtSearch.Location = New System.Drawing.Point(738, 30)
        Me.txtSearch.MaxLength = 20
        Me.txtSearch.Multiline = True
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(357, 28)
        Me.txtSearch.TabIndex = 47
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Location = New System.Drawing.Point(738, 58)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(357, 1)
        Me.Panel2.TabIndex = 48
        '
        'btnRemove
        '
        Me.btnRemove.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnRemove.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRemove.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnRemove.FlatAppearance.BorderSize = 0
        Me.btnRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnRemove.Image = CType(resources.GetObject("btnRemove.Image"), System.Drawing.Image)
        Me.btnRemove.Location = New System.Drawing.Point(174, 31)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(101, 37)
        Me.btnRemove.TabIndex = 38
        Me.btnRemove.Text = "  Void"
        Me.btnRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(12, 30)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 37)
        Me.btnAdd.TabIndex = 35
        Me.btnAdd.Text = "  Add"
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'switchtimer
        '
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel4.Controls.Add(Me.txtVatRate)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.Panel6)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Controls.Add(Me.txtDiscountAmt)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.btnNewTransaction)
        Me.Panel4.Controls.Add(Me.btnPay)
        Me.Panel4.Controls.Add(Me.txtChange)
        Me.Panel4.Controls.Add(Me.txtCash)
        Me.Panel4.Controls.Add(Me.txtTotal)
        Me.Panel4.Controls.Add(Me.cbdiscount)
        Me.Panel4.Controls.Add(Me.txtSubtotal)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.lblTransactionNo)
        Me.Panel4.Controls.Add(Me.lblDate)
        Me.Panel4.Location = New System.Drawing.Point(1301, 49)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(291, 839)
        Me.Panel4.TabIndex = 90
        '
        'txtVatRate
        '
        Me.txtVatRate.Location = New System.Drawing.Point(161, 253)
        Me.txtVatRate.Name = "txtVatRate"
        Me.txtVatRate.ReadOnly = True
        Me.txtVatRate.Size = New System.Drawing.Size(116, 20)
        Me.txtVatRate.TabIndex = 46
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(13, 253)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 23)
        Me.Label11.TabIndex = 45
        Me.Label11.Text = "Vat Rate:"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.White
        Me.Panel6.Location = New System.Drawing.Point(17, 410)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(260, 1)
        Me.Panel6.TabIndex = 44
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.Location = New System.Drawing.Point(17, 310)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(260, 1)
        Me.Panel5.TabIndex = 43
        '
        'txtDiscountAmt
        '
        Me.txtDiscountAmt.Location = New System.Drawing.Point(161, 208)
        Me.txtDiscountAmt.Name = "txtDiscountAmt"
        Me.txtDiscountAmt.ReadOnly = True
        Me.txtDiscountAmt.Size = New System.Drawing.Size(116, 20)
        Me.txtDiscountAmt.TabIndex = 42
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(13, 208)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(142, 23)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Discount Amt:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(17, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 18)
        Me.Label3.TabIndex = 40
        Me.Label3.Text = "Transaction No:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(17, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 18)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "Date:"
        '
        'btnNewTransaction
        '
        Me.btnNewTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnNewTransaction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNewTransaction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnNewTransaction.FlatAppearance.BorderSize = 0
        Me.btnNewTransaction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnNewTransaction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnNewTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewTransaction.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewTransaction.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnNewTransaction.Image = CType(resources.GetObject("btnNewTransaction.Image"), System.Drawing.Image)
        Me.btnNewTransaction.Location = New System.Drawing.Point(17, 786)
        Me.btnNewTransaction.Name = "btnNewTransaction"
        Me.btnNewTransaction.Size = New System.Drawing.Size(159, 37)
        Me.btnNewTransaction.TabIndex = 38
        Me.btnNewTransaction.Text = "  New Transaction"
        Me.btnNewTransaction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnNewTransaction.UseVisualStyleBackColor = False
        '
        'btnPay
        '
        Me.btnPay.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPay.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPay.FlatAppearance.BorderSize = 0
        Me.btnPay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPay.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPay.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnPay.Image = CType(resources.GetObject("btnPay.Image"), System.Drawing.Image)
        Me.btnPay.Location = New System.Drawing.Point(182, 786)
        Me.btnPay.Name = "btnPay"
        Me.btnPay.Size = New System.Drawing.Size(75, 37)
        Me.btnPay.TabIndex = 37
        Me.btnPay.Text = "  Pay"
        Me.btnPay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPay.UseVisualStyleBackColor = False
        '
        'txtChange
        '
        Me.txtChange.Location = New System.Drawing.Point(161, 494)
        Me.txtChange.Name = "txtChange"
        Me.txtChange.ReadOnly = True
        Me.txtChange.Size = New System.Drawing.Size(116, 20)
        Me.txtChange.TabIndex = 16
        '
        'txtCash
        '
        Me.txtCash.Location = New System.Drawing.Point(161, 454)
        Me.txtCash.Name = "txtCash"
        Me.txtCash.Size = New System.Drawing.Size(116, 20)
        Me.txtCash.TabIndex = 15
        '
        'txtTotal
        '
        Me.txtTotal.Location = New System.Drawing.Point(161, 353)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.ReadOnly = True
        Me.txtTotal.Size = New System.Drawing.Size(116, 20)
        Me.txtTotal.TabIndex = 14
        '
        'cbdiscount
        '
        Me.cbdiscount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbdiscount.FormattingEnabled = True
        Me.cbdiscount.Location = New System.Drawing.Point(161, 167)
        Me.cbdiscount.Name = "cbdiscount"
        Me.cbdiscount.Size = New System.Drawing.Size(116, 21)
        Me.cbdiscount.TabIndex = 13
        '
        'txtSubtotal
        '
        Me.txtSubtotal.Location = New System.Drawing.Point(161, 126)
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.ReadOnly = True
        Me.txtSubtotal.Size = New System.Drawing.Size(116, 20)
        Me.txtSubtotal.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(16, 490)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 23)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Change:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(16, 450)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 23)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Cash:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(16, 350)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 23)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Total:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(13, 167)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 23)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Discount:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(13, 127)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 23)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Subtotal:"
        '
        'lblTransactionNo
        '
        Me.lblTransactionNo.AutoSize = True
        Me.lblTransactionNo.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionNo.ForeColor = System.Drawing.Color.White
        Me.lblTransactionNo.Location = New System.Drawing.Point(142, 76)
        Me.lblTransactionNo.Name = "lblTransactionNo"
        Me.lblTransactionNo.Size = New System.Drawing.Size(119, 18)
        Me.lblTransactionNo.TabIndex = 2
        Me.lblTransactionNo.Text = "Transaction No:"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(62, 37)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(45, 18)
        Me.lblDate.TabIndex = 1
        Me.lblDate.Text = "Date:"
        '
        'datetimer
        '
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(1604, 519)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 23)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Vatable:"
        Me.Label1.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Enabled = False
        Me.Label4.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(1604, 560)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 23)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Vat:"
        Me.Label4.Visible = False
        '
        'txtVatable
        '
        Me.txtVatable.Enabled = False
        Me.txtVatable.Location = New System.Drawing.Point(1752, 522)
        Me.txtVatable.Name = "txtVatable"
        Me.txtVatable.ReadOnly = True
        Me.txtVatable.Size = New System.Drawing.Size(116, 20)
        Me.txtVatable.TabIndex = 10
        Me.txtVatable.Visible = False
        '
        'txtVat
        '
        Me.txtVat.Enabled = False
        Me.txtVat.Location = New System.Drawing.Point(1752, 563)
        Me.txtVat.Name = "txtVat"
        Me.txtVat.ReadOnly = True
        Me.txtVat.Size = New System.Drawing.Size(116, 20)
        Me.txtVat.TabIndex = 11
        Me.txtVat.Visible = False
        '
        'Admin_Pos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1600, 900)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.panelmenu)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtVatable)
        Me.Controls.Add(Me.txtVat)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Admin_Pos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Admin_Pos"
        Me.panelmenu.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DGVCart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents panelmenu As Panel
    Friend WithEvents btnPos As Button
    Friend WithEvents btnInventory As Button
    Friend WithEvents btnDelivery As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnFileMaintenance As Button
    Friend WithEvents btnHome As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label12 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnAdd As Button
    Friend WithEvents switchtimer As Timer
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lblTransactionNo As Label
    Friend WithEvents lblDate As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtChange As TextBox
    Friend WithEvents txtCash As TextBox
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents cbdiscount As ComboBox
    Friend WithEvents txtSubtotal As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents btnPay As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnNewTransaction As Button
    Friend WithEvents datetimer As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDiscountAmt As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents btnTransaction As Button
    Friend WithEvents lblPlaceholder As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnAuditTrail As Button
    Friend WithEvents txtVatRate As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents btnEdit As Button
    Friend WithEvents DGVCart As DataGridView
    Friend WithEvents txtVat As TextBox
    Friend WithEvents txtVatable As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
End Class
