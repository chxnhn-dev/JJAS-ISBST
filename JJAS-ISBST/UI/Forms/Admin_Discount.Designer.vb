<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Admin_Discount
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin_Discount))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnCategory = New System.Windows.Forms.Button()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnSupplier = New System.Windows.Forms.Button()
        Me.btnColor = New System.Windows.Forms.Button()
        Me.btnBrand = New System.Windows.Forms.Button()
        Me.btnMeasurement = New System.Windows.Forms.Button()
        Me.btnUser = New System.Windows.Forms.Button()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.btnVat = New System.Windows.Forms.Button()
        Me.btnDiscount = New System.Windows.Forms.Button()
        Me.btnProduct = New System.Windows.Forms.Button()
        Me.switchtimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DGVsize = New System.Windows.Forms.DataGridView()
        Me.lblPlaceholder = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnAuditTrail = New System.Windows.Forms.Button()
        Me.btnTransaction = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.btnInventory = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnDashboard = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Panel8.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.DGVsize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCategory
        '
        Me.btnCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnCategory.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCategory.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnCategory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnCategory.FlatAppearance.BorderSize = 0
        Me.btnCategory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnCategory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCategory.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnCategory.Image = CType(resources.GetObject("btnCategory.Image"), System.Drawing.Image)
        Me.btnCategory.Location = New System.Drawing.Point(0, 37)
        Me.btnCategory.Name = "btnCategory"
        Me.btnCategory.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnCategory.Size = New System.Drawing.Size(164, 37)
        Me.btnCategory.TabIndex = 45
        Me.btnCategory.Text = " Category"
        Me.btnCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCategory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCategory.UseVisualStyleBackColor = False
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel8.Controls.Add(Me.Label12)
        Me.Panel8.Controls.Add(Me.Button1)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel8.Location = New System.Drawing.Point(182, 0)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(1418, 43)
        Me.Panel8.TabIndex = 95
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label12.Location = New System.Drawing.Point(7, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(186, 21)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Discount Maintenance:"
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
        Me.Button1.Size = New System.Drawing.Size(38, 43)
        Me.Button1.TabIndex = 42
        Me.Button1.Text = "  "
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnSupplier
        '
        Me.btnSupplier.BackColor = System.Drawing.Color.Transparent
        Me.btnSupplier.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSupplier.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnSupplier.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnSupplier.FlatAppearance.BorderSize = 0
        Me.btnSupplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSupplier.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSupplier.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnSupplier.Image = CType(resources.GetObject("btnSupplier.Image"), System.Drawing.Image)
        Me.btnSupplier.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSupplier.Location = New System.Drawing.Point(0, 222)
        Me.btnSupplier.Name = "btnSupplier"
        Me.btnSupplier.Padding = New System.Windows.Forms.Padding(5, 0, 40, 0)
        Me.btnSupplier.Size = New System.Drawing.Size(164, 37)
        Me.btnSupplier.TabIndex = 47
        Me.btnSupplier.Text = " Supplier"
        Me.btnSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSupplier.UseVisualStyleBackColor = False
        '
        'btnColor
        '
        Me.btnColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnColor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnColor.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnColor.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnColor.FlatAppearance.BorderSize = 0
        Me.btnColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnColor.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnColor.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnColor.Image = CType(resources.GetObject("btnColor.Image"), System.Drawing.Image)
        Me.btnColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnColor.Location = New System.Drawing.Point(0, 148)
        Me.btnColor.Name = "btnColor"
        Me.btnColor.Padding = New System.Windows.Forms.Padding(0, 0, 60, 0)
        Me.btnColor.Size = New System.Drawing.Size(164, 37)
        Me.btnColor.TabIndex = 44
        Me.btnColor.Text = " Color"
        Me.btnColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnColor.UseVisualStyleBackColor = False
        '
        'btnBrand
        '
        Me.btnBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBrand.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBrand.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnBrand.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBrand.FlatAppearance.BorderSize = 0
        Me.btnBrand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnBrand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnBrand.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrand.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrand.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnBrand.Image = CType(resources.GetObject("btnBrand.Image"), System.Drawing.Image)
        Me.btnBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBrand.Location = New System.Drawing.Point(0, 111)
        Me.btnBrand.Name = "btnBrand"
        Me.btnBrand.Padding = New System.Windows.Forms.Padding(0, 0, 60, 0)
        Me.btnBrand.Size = New System.Drawing.Size(164, 37)
        Me.btnBrand.TabIndex = 43
        Me.btnBrand.Text = " Brand"
        Me.btnBrand.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBrand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnBrand.UseVisualStyleBackColor = False
        '
        'btnMeasurement
        '
        Me.btnMeasurement.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnMeasurement.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnMeasurement.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnMeasurement.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnMeasurement.FlatAppearance.BorderSize = 0
        Me.btnMeasurement.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnMeasurement.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnMeasurement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMeasurement.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMeasurement.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnMeasurement.Image = CType(resources.GetObject("btnMeasurement.Image"), System.Drawing.Image)
        Me.btnMeasurement.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMeasurement.Location = New System.Drawing.Point(0, 74)
        Me.btnMeasurement.Name = "btnMeasurement"
        Me.btnMeasurement.Padding = New System.Windows.Forms.Padding(0, 0, 40, 0)
        Me.btnMeasurement.Size = New System.Drawing.Size(164, 37)
        Me.btnMeasurement.TabIndex = 42
        Me.btnMeasurement.Text = " Size"
        Me.btnMeasurement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMeasurement.UseVisualStyleBackColor = False
        '
        'btnUser
        '
        Me.btnUser.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnUser.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUser.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUser.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnUser.FlatAppearance.BorderSize = 0
        Me.btnUser.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnUser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUser.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUser.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnUser.Image = CType(resources.GetObject("btnUser.Image"), System.Drawing.Image)
        Me.btnUser.Location = New System.Drawing.Point(0, 0)
        Me.btnUser.Name = "btnUser"
        Me.btnUser.Padding = New System.Windows.Forms.Padding(21, 0, 45, 0)
        Me.btnUser.Size = New System.Drawing.Size(164, 37)
        Me.btnUser.TabIndex = 41
        Me.btnUser.Text = " User"
        Me.btnUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnUser.UseVisualStyleBackColor = False
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel7.Controls.Add(Me.btnVat)
        Me.Panel7.Controls.Add(Me.btnDiscount)
        Me.Panel7.Controls.Add(Me.btnSupplier)
        Me.Panel7.Controls.Add(Me.btnProduct)
        Me.Panel7.Controls.Add(Me.btnColor)
        Me.Panel7.Controls.Add(Me.btnBrand)
        Me.Panel7.Controls.Add(Me.btnMeasurement)
        Me.Panel7.Controls.Add(Me.btnCategory)
        Me.Panel7.Controls.Add(Me.btnUser)
        Me.Panel7.Location = New System.Drawing.Point(188, 49)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(164, 840)
        Me.Panel7.TabIndex = 97
        '
        'btnVat
        '
        Me.btnVat.BackColor = System.Drawing.Color.Transparent
        Me.btnVat.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnVat.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnVat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnVat.FlatAppearance.BorderSize = 0
        Me.btnVat.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnVat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnVat.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVat.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVat.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnVat.Image = CType(resources.GetObject("btnVat.Image"), System.Drawing.Image)
        Me.btnVat.Location = New System.Drawing.Point(0, 296)
        Me.btnVat.Name = "btnVat"
        Me.btnVat.Padding = New System.Windows.Forms.Padding(15, 0, 60, 0)
        Me.btnVat.Size = New System.Drawing.Size(164, 37)
        Me.btnVat.TabIndex = 48
        Me.btnVat.Text = " Vat"
        Me.btnVat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnVat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnVat.UseVisualStyleBackColor = False
        '
        'btnDiscount
        '
        Me.btnDiscount.BackColor = System.Drawing.Color.Black
        Me.btnDiscount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDiscount.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnDiscount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDiscount.FlatAppearance.BorderSize = 0
        Me.btnDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDiscount.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDiscount.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnDiscount.Image = CType(resources.GetObject("btnDiscount.Image"), System.Drawing.Image)
        Me.btnDiscount.Location = New System.Drawing.Point(0, 259)
        Me.btnDiscount.Name = "btnDiscount"
        Me.btnDiscount.Padding = New System.Windows.Forms.Padding(15, 0, 23, 0)
        Me.btnDiscount.Size = New System.Drawing.Size(164, 37)
        Me.btnDiscount.TabIndex = 49
        Me.btnDiscount.Text = " Discount"
        Me.btnDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDiscount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDiscount.UseVisualStyleBackColor = False
        '
        'btnProduct
        '
        Me.btnProduct.BackColor = System.Drawing.Color.Transparent
        Me.btnProduct.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnProduct.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnProduct.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnProduct.FlatAppearance.BorderSize = 0
        Me.btnProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProduct.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProduct.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnProduct.Image = CType(resources.GetObject("btnProduct.Image"), System.Drawing.Image)
        Me.btnProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnProduct.Location = New System.Drawing.Point(0, 185)
        Me.btnProduct.Name = "btnProduct"
        Me.btnProduct.Padding = New System.Windows.Forms.Padding(0, 0, 45, 0)
        Me.btnProduct.Size = New System.Drawing.Size(164, 37)
        Me.btnProduct.TabIndex = 46
        Me.btnProduct.Text = " Product"
        Me.btnProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnProduct.UseVisualStyleBackColor = False
        '
        'switchtimer
        '
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
        Me.btnEdit.Location = New System.Drawing.Point(93, 30)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 37)
        Me.btnEdit.TabIndex = 36
        Me.btnEdit.Text = "  Edit"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel3.Controls.Add(Me.btnRefresh)
        Me.Panel3.Controls.Add(Me.DGVsize)
        Me.Panel3.Controls.Add(Me.lblPlaceholder)
        Me.Panel3.Controls.Add(Me.txtSearch)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Controls.Add(Me.btnDelete)
        Me.Panel3.Controls.Add(Me.btnAdd)
        Me.Panel3.Controls.Add(Me.btnEdit)
        Me.Panel3.Location = New System.Drawing.Point(358, 49)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1236, 839)
        Me.Panel3.TabIndex = 96
        '
        'DGVsize
        '
        Me.DGVsize.AllowUserToAddRows = False
        Me.DGVsize.AllowUserToDeleteRows = False
        Me.DGVsize.AllowUserToResizeColumns = False
        Me.DGVsize.AllowUserToResizeRows = False
        Me.DGVsize.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVsize.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DGVsize.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVsize.Location = New System.Drawing.Point(12, 98)
        Me.DGVsize.Name = "DGVsize"
        Me.DGVsize.ReadOnly = True
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVsize.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.DGVsize.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.DGVsize.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.DGVsize.Size = New System.Drawing.Size(1214, 725)
        Me.DGVsize.TabIndex = 50
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
        Me.lblPlaceholder.Location = New System.Drawing.Point(865, 31)
        Me.lblPlaceholder.Name = "lblPlaceholder"
        Me.lblPlaceholder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlaceholder.Size = New System.Drawing.Size(94, 25)
        Me.lblPlaceholder.TabIndex = 49
        Me.lblPlaceholder.Text = "     Search"
        Me.lblPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.ForeColor = System.Drawing.Color.White
        Me.txtSearch.Location = New System.Drawing.Point(869, 30)
        Me.txtSearch.MaxLength = 20
        Me.txtSearch.Multiline = True
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(357, 28)
        Me.txtSearch.TabIndex = 47
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Location = New System.Drawing.Point(869, 58)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(357, 1)
        Me.Panel2.TabIndex = 48
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelete.FlatAppearance.BorderSize = 0
        Me.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.Location = New System.Drawing.Point(174, 30)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(114, 37)
        Me.btnDelete.TabIndex = 37
        Me.btnDelete.Text = "  Delete"
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDelete.UseVisualStyleBackColor = False
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
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Controls.Add(Me.btnAuditTrail)
        Me.Panel1.Controls.Add(Me.btnTransaction)
        Me.Panel1.Controls.Add(Me.Button5)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.btnInventory)
        Me.Panel1.Controls.Add(Me.btnLogout)
        Me.Panel1.Controls.Add(Me.btnDashboard)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(182, 900)
        Me.Panel1.TabIndex = 98
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
        Me.btnTransaction.TabIndex = 54
        Me.btnTransaction.Text = " Transaction"
        Me.btnTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTransaction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnTransaction.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.Black
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Button5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.Location = New System.Drawing.Point(0, 273)
        Me.Button5.Name = "Button5"
        Me.Button5.Padding = New System.Windows.Forms.Padding(0, 0, 30, 0)
        Me.Button5.Size = New System.Drawing.Size(182, 37)
        Me.Button5.TabIndex = 48
        Me.Button5.Text = " POS"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.Black
        Me.Button4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button4.FlatAppearance.BorderSize = 0
        Me.Button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.Location = New System.Drawing.Point(0, 236)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(182, 37)
        Me.Button4.TabIndex = 47
        Me.Button4.Text = " Inventory"
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button4.UseVisualStyleBackColor = False
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
        Me.btnInventory.Location = New System.Drawing.Point(0, 199)
        Me.btnInventory.Name = "btnInventory"
        Me.btnInventory.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnInventory.Size = New System.Drawing.Size(182, 37)
        Me.btnInventory.TabIndex = 42
        Me.btnInventory.Text = " Delivery"
        Me.btnInventory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnInventory.UseVisualStyleBackColor = False
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
        'btnDashboard
        '
        Me.btnDashboard.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDashboard.FlatAppearance.BorderSize = 0
        Me.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDashboard.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDashboard.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnDashboard.Image = CType(resources.GetObject("btnDashboard.Image"), System.Drawing.Image)
        Me.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDashboard.Location = New System.Drawing.Point(0, 162)
        Me.btnDashboard.Name = "btnDashboard"
        Me.btnDashboard.Padding = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.btnDashboard.Size = New System.Drawing.Size(182, 37)
        Me.btnDashboard.TabIndex = 22
        Me.btnDashboard.Text = " File Maintenance"
        Me.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDashboard.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Black
        Me.Button2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.Location = New System.Drawing.Point(0, 125)
        Me.Button2.Name = "Button2"
        Me.Button2.Padding = New System.Windows.Forms.Padding(0, 0, 65, 0)
        Me.Button2.Size = New System.Drawing.Size(182, 37)
        Me.Button2.TabIndex = 45
        Me.Button2.Text = " Home"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button2.UseVisualStyleBackColor = False
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
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnRefresh.Image = CType(resources.GetObject("btnRefresh.Image"), System.Drawing.Image)
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(294, 27)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(93, 43)
        Me.btnRefresh.TabIndex = 107
        Me.btnRefresh.Text = "       Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'Admin_Discount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1600, 900)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Admin_Discount"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Admin_Discount"
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DGVsize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnCategory As Button
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label12 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents btnSupplier As Button
    Friend WithEvents btnColor As Button
    Friend WithEvents btnBrand As Button
    Friend WithEvents btnMeasurement As Button
    Friend WithEvents btnUser As Button
    Friend WithEvents Panel7 As Panel
    Friend WithEvents btnDiscount As Button
    Friend WithEvents btnVat As Button
    Friend WithEvents btnProduct As Button
    Friend WithEvents switchtimer As Timer
    Friend WithEvents btnEdit As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button5 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents btnInventory As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnDashboard As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnTransaction As Button
    Friend WithEvents lblPlaceholder As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnAuditTrail As Button
    Friend WithEvents DGVsize As DataGridView
    Friend WithEvents btnRefresh As Button
End Class
