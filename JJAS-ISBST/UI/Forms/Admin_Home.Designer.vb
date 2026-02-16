<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Admin_Home
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin_Home))
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.switchtimer = New System.Windows.Forms.Timer(Me.components)
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.lblSalesToday = New System.Windows.Forms.Label()
        Me.lbsalestoday = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTransactions = New System.Windows.Forms.Label()
        Me.weweqeqwe = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblLowStock = New System.Windows.Forms.Label()
        Me.lbllowstosdasdsadck = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblDiscount = New System.Windows.Forms.Label()
        Me.panel12 = New System.Windows.Forms.Label()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.btnWeekly = New System.Windows.Forms.Button()
        Me.btnMontly = New System.Windows.Forms.Button()
        Me.btnyearly = New System.Windows.Forms.Button()
        Me.chartSales = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.chartCategory = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnHome = New System.Windows.Forms.Button()
        Me.btnFileMaintenance = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnDelivery = New System.Windows.Forms.Button()
        Me.btnInventory = New System.Windows.Forms.Button()
        Me.btnPos = New System.Windows.Forms.Button()
        Me.btnTransaction = New System.Windows.Forms.Button()
        Me.btnAuditTrail = New System.Windows.Forms.Button()
        Me.PanelMenu = New System.Windows.Forms.Panel()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.lblActiveUSer = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblProfitToday = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel6.SuspendLayout()
        CType(Me.chartSales, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chartCategory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelMenu.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'switchtimer
        '
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel5.Controls.Add(Me.lblSalesToday)
        Me.Panel5.Controls.Add(Me.lbsalestoday)
        Me.Panel5.Location = New System.Drawing.Point(188, 286)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(703, 225)
        Me.Panel5.TabIndex = 94
        '
        'lblSalesToday
        '
        Me.lblSalesToday.AutoSize = True
        Me.lblSalesToday.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblSalesToday.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSalesToday.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalesToday.ForeColor = System.Drawing.Color.White
        Me.lblSalesToday.Location = New System.Drawing.Point(0, 42)
        Me.lblSalesToday.Margin = New System.Windows.Forms.Padding(0)
        Me.lblSalesToday.Name = "lblSalesToday"
        Me.lblSalesToday.Padding = New System.Windows.Forms.Padding(105, 20, 81, 50)
        Me.lblSalesToday.Size = New System.Drawing.Size(295, 198)
        Me.lblSalesToday.TabIndex = 6
        Me.lblSalesToday.Text = "0"
        Me.lblSalesToday.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbsalestoday
        '
        Me.lbsalestoday.AutoSize = True
        Me.lbsalestoday.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lbsalestoday.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbsalestoday.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbsalestoday.ForeColor = System.Drawing.Color.White
        Me.lbsalestoday.Location = New System.Drawing.Point(0, 0)
        Me.lbsalestoday.Name = "lbsalestoday"
        Me.lbsalestoday.Padding = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.lbsalestoday.Size = New System.Drawing.Size(234, 42)
        Me.lbsalestoday.TabIndex = 0
        Me.lbsalestoday.Text = "    Sales today         "
        Me.lbsalestoday.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblTransactions)
        Me.Panel1.Controls.Add(Me.weweqeqwe)
        Me.Panel1.Location = New System.Drawing.Point(188, 58)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(350, 225)
        Me.Panel1.TabIndex = 95
        '
        'lblTransactions
        '
        Me.lblTransactions.AutoSize = True
        Me.lblTransactions.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.lblTransactions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTransactions.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactions.ForeColor = System.Drawing.Color.White
        Me.lblTransactions.Location = New System.Drawing.Point(0, 0)
        Me.lblTransactions.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTransactions.Name = "lblTransactions"
        Me.lblTransactions.Padding = New System.Windows.Forms.Padding(110, 20, 109, 0)
        Me.lblTransactions.Size = New System.Drawing.Size(354, 148)
        Me.lblTransactions.TabIndex = 5
        Me.lblTransactions.Text = " 0"
        '
        'weweqeqwe
        '
        Me.weweqeqwe.AutoSize = True
        Me.weweqeqwe.BackColor = System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.weweqeqwe.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.weweqeqwe.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.weweqeqwe.ForeColor = System.Drawing.Color.White
        Me.weweqeqwe.Location = New System.Drawing.Point(0, 143)
        Me.weweqeqwe.Name = "weweqeqwe"
        Me.weweqeqwe.Padding = New System.Windows.Forms.Padding(0, 0, 0, 50)
        Me.weweqeqwe.Size = New System.Drawing.Size(318, 82)
        Me.weweqeqwe.TabIndex = 0
        Me.weweqeqwe.Tag = "0"
        Me.weweqeqwe.Text = "               Transactions        "
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(217, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.Panel2.Controls.Add(Me.lblLowStock)
        Me.Panel2.Controls.Add(Me.lbllowstosdasdsadck)
        Me.Panel2.Location = New System.Drawing.Point(545, 58)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(346, 225)
        Me.Panel2.TabIndex = 96
        '
        'lblLowStock
        '
        Me.lblLowStock.AutoSize = True
        Me.lblLowStock.BackColor = System.Drawing.Color.FromArgb(CType(CType(217, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.lblLowStock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLowStock.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLowStock.ForeColor = System.Drawing.Color.White
        Me.lblLowStock.Location = New System.Drawing.Point(0, 0)
        Me.lblLowStock.Margin = New System.Windows.Forms.Padding(0)
        Me.lblLowStock.Name = "lblLowStock"
        Me.lblLowStock.Padding = New System.Windows.Forms.Padding(95, 20, 95, 0)
        Me.lblLowStock.Size = New System.Drawing.Size(351, 148)
        Me.lblLowStock.TabIndex = 4
        Me.lblLowStock.Text = " 0 "
        '
        'lbllowstosdasdsadck
        '
        Me.lbllowstosdasdsadck.AutoSize = True
        Me.lbllowstosdasdsadck.BackColor = System.Drawing.Color.FromArgb(CType(CType(217, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.lbllowstosdasdsadck.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbllowstosdasdsadck.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbllowstosdasdsadck.ForeColor = System.Drawing.Color.White
        Me.lbllowstosdasdsadck.Location = New System.Drawing.Point(0, 143)
        Me.lbllowstosdasdsadck.Name = "lbllowstosdasdsadck"
        Me.lbllowstosdasdsadck.Padding = New System.Windows.Forms.Padding(0, 0, 0, 50)
        Me.lbllowstosdasdsadck.Size = New System.Drawing.Size(310, 82)
        Me.lbllowstosdasdsadck.TabIndex = 0
        Me.lbllowstosdasdsadck.Text = "           Low Stock Items     "
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel4.Controls.Add(Me.lblDiscount)
        Me.Panel4.Controls.Add(Me.panel12)
        Me.Panel4.Location = New System.Drawing.Point(1249, 58)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(346, 225)
        Me.Panel4.TabIndex = 95
        '
        'lblDiscount
        '
        Me.lblDiscount.AutoSize = True
        Me.lblDiscount.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblDiscount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDiscount.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscount.ForeColor = System.Drawing.Color.White
        Me.lblDiscount.Location = New System.Drawing.Point(0, 0)
        Me.lblDiscount.Margin = New System.Windows.Forms.Padding(0)
        Me.lblDiscount.Name = "lblDiscount"
        Me.lblDiscount.Padding = New System.Windows.Forms.Padding(110, 20, 109, 0)
        Me.lblDiscount.Size = New System.Drawing.Size(354, 148)
        Me.lblDiscount.TabIndex = 7
        Me.lblDiscount.Text = " 0"
        '
        'panel12
        '
        Me.panel12.AutoSize = True
        Me.panel12.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.panel12.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panel12.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panel12.ForeColor = System.Drawing.Color.White
        Me.panel12.Location = New System.Drawing.Point(0, 143)
        Me.panel12.Name = "panel12"
        Me.panel12.Padding = New System.Windows.Forms.Padding(0, 0, 0, 50)
        Me.panel12.Size = New System.Drawing.Size(347, 82)
        Me.panel12.TabIndex = 0
        Me.panel12.Text = "                 Discount                "
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel8.Controls.Add(Me.lblUserName)
        Me.Panel8.Controls.Add(Me.Button1)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel8.Location = New System.Drawing.Point(182, 0)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(1418, 43)
        Me.Panel8.TabIndex = 100
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblUserName.Location = New System.Drawing.Point(7, 9)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(14, 21)
        Me.lblUserName.TabIndex = 1
        Me.lblUserName.Text = ":"
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
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel6.Controls.Add(Me.btnWeekly)
        Me.Panel6.Controls.Add(Me.btnMontly)
        Me.Panel6.Controls.Add(Me.btnyearly)
        Me.Panel6.Controls.Add(Me.chartSales)
        Me.Panel6.Location = New System.Drawing.Point(188, 520)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(841, 368)
        Me.Panel6.TabIndex = 95
        '
        'btnWeekly
        '
        Me.btnWeekly.BackColor = System.Drawing.Color.Transparent
        Me.btnWeekly.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnWeekly.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnWeekly.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnWeekly.FlatAppearance.BorderSize = 0
        Me.btnWeekly.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnWeekly.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnWeekly.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWeekly.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWeekly.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnWeekly.Location = New System.Drawing.Point(1, 0)
        Me.btnWeekly.Name = "btnWeekly"
        Me.btnWeekly.Size = New System.Drawing.Size(280, 60)
        Me.btnWeekly.TabIndex = 50
        Me.btnWeekly.Text = "Weekly"
        Me.btnWeekly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnWeekly.UseVisualStyleBackColor = False
        '
        'btnMontly
        '
        Me.btnMontly.BackColor = System.Drawing.Color.Transparent
        Me.btnMontly.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnMontly.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnMontly.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnMontly.FlatAppearance.BorderSize = 0
        Me.btnMontly.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnMontly.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnMontly.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMontly.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMontly.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnMontly.Location = New System.Drawing.Point(281, 0)
        Me.btnMontly.Name = "btnMontly"
        Me.btnMontly.Size = New System.Drawing.Size(280, 60)
        Me.btnMontly.TabIndex = 49
        Me.btnMontly.Text = "Montly"
        Me.btnMontly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMontly.UseVisualStyleBackColor = False
        '
        'btnyearly
        '
        Me.btnyearly.BackColor = System.Drawing.Color.Transparent
        Me.btnyearly.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnyearly.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnyearly.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnyearly.FlatAppearance.BorderSize = 0
        Me.btnyearly.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.btnyearly.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black
        Me.btnyearly.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnyearly.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnyearly.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnyearly.Location = New System.Drawing.Point(561, 0)
        Me.btnyearly.Name = "btnyearly"
        Me.btnyearly.Size = New System.Drawing.Size(280, 60)
        Me.btnyearly.TabIndex = 48
        Me.btnyearly.Text = "Yearly"
        Me.btnyearly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnyearly.UseVisualStyleBackColor = False
        '
        'chartSales
        '
        Me.chartSales.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.chartSales.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
        ChartArea1.Name = "ChartArea1"
        Me.chartSales.ChartAreas.Add(ChartArea1)
        Me.chartSales.Dock = System.Windows.Forms.DockStyle.Bottom
        Legend1.Name = "Legend1"
        Me.chartSales.Legends.Add(Legend1)
        Me.chartSales.Location = New System.Drawing.Point(0, 60)
        Me.chartSales.Name = "chartSales"
        Me.chartSales.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chartSales.Series.Add(Series1)
        Me.chartSales.Size = New System.Drawing.Size(841, 308)
        Me.chartSales.TabIndex = 0
        Me.chartSales.Text = "Chart1"
        '
        'chartCategory
        '
        Me.chartCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.chartCategory.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft
        Me.chartCategory.BorderlineColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        ChartArea2.Name = "ChartArea1"
        Me.chartCategory.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.chartCategory.Legends.Add(Legend2)
        Me.chartCategory.Location = New System.Drawing.Point(1043, 520)
        Me.chartCategory.Name = "chartCategory"
        Me.chartCategory.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale
        Series2.ChartArea = "ChartArea1"
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.chartCategory.Series.Add(Series2)
        Me.chartCategory.Size = New System.Drawing.Size(552, 368)
        Me.chartCategory.TabIndex = 0
        Me.chartCategory.Text = "Chart2"
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
        'btnHome
        '
        Me.btnHome.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
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
        'btnDelivery
        '
        Me.btnDelivery.BackColor = System.Drawing.Color.Black
        Me.btnDelivery.Cursor = System.Windows.Forms.Cursors.Arrow
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
        Me.btnInventory.TabIndex = 50
        Me.btnInventory.Text = " Inventory"
        Me.btnInventory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnInventory.UseVisualStyleBackColor = False
        '
        'btnPos
        '
        Me.btnPos.BackColor = System.Drawing.Color.Black
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
        Me.btnAuditTrail.TabIndex = 55
        Me.btnAuditTrail.Text = "  Audit Trail"
        Me.btnAuditTrail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuditTrail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAuditTrail.UseVisualStyleBackColor = False
        '
        'PanelMenu
        '
        Me.PanelMenu.BackColor = System.Drawing.Color.Black
        Me.PanelMenu.Controls.Add(Me.btnAuditTrail)
        Me.PanelMenu.Controls.Add(Me.btnTransaction)
        Me.PanelMenu.Controls.Add(Me.btnPos)
        Me.PanelMenu.Controls.Add(Me.btnInventory)
        Me.PanelMenu.Controls.Add(Me.btnDelivery)
        Me.PanelMenu.Controls.Add(Me.btnLogout)
        Me.PanelMenu.Controls.Add(Me.btnFileMaintenance)
        Me.PanelMenu.Controls.Add(Me.btnHome)
        Me.PanelMenu.Controls.Add(Me.PictureBox1)
        Me.PanelMenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelMenu.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PanelMenu.Location = New System.Drawing.Point(0, 0)
        Me.PanelMenu.Name = "PanelMenu"
        Me.PanelMenu.Size = New System.Drawing.Size(182, 900)
        Me.PanelMenu.TabIndex = 90
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.Panel9.Controls.Add(Me.lblActiveUSer)
        Me.Panel9.Controls.Add(Me.Label3)
        Me.Panel9.Location = New System.Drawing.Point(897, 58)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(346, 225)
        Me.Panel9.TabIndex = 96
        '
        'lblActiveUSer
        '
        Me.lblActiveUSer.AutoSize = True
        Me.lblActiveUSer.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblActiveUSer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblActiveUSer.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiveUSer.ForeColor = System.Drawing.Color.White
        Me.lblActiveUSer.Location = New System.Drawing.Point(0, 0)
        Me.lblActiveUSer.Margin = New System.Windows.Forms.Padding(0)
        Me.lblActiveUSer.Name = "lblActiveUSer"
        Me.lblActiveUSer.Padding = New System.Windows.Forms.Padding(110, 20, 109, 0)
        Me.lblActiveUSer.Size = New System.Drawing.Size(354, 148)
        Me.lblActiveUSer.TabIndex = 7
        Me.lblActiveUSer.Text = " 0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 143)
        Me.Label3.Name = "Label3"
        Me.Label3.Padding = New System.Windows.Forms.Padding(0, 0, 0, 50)
        Me.Label3.Size = New System.Drawing.Size(318, 82)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "               Active User          "
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel3.Controls.Add(Me.lblProfitToday)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Location = New System.Drawing.Point(897, 289)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(698, 225)
        Me.Panel3.TabIndex = 101
        '
        'lblProfitToday
        '
        Me.lblProfitToday.AutoSize = True
        Me.lblProfitToday.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblProfitToday.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblProfitToday.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProfitToday.ForeColor = System.Drawing.Color.White
        Me.lblProfitToday.Location = New System.Drawing.Point(0, 42)
        Me.lblProfitToday.Margin = New System.Windows.Forms.Padding(0)
        Me.lblProfitToday.Name = "lblProfitToday"
        Me.lblProfitToday.Padding = New System.Windows.Forms.Padding(105, 20, 81, 50)
        Me.lblProfitToday.Size = New System.Drawing.Size(295, 198)
        Me.lblProfitToday.TabIndex = 6
        Me.lblProfitToday.Text = "0"
        Me.lblProfitToday.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Padding = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.Label5.Size = New System.Drawing.Size(245, 42)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "    Profit Today         "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Admin_Home
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1600, 900)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel9)
        Me.Controls.Add(Me.chartCategory)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.PanelMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Admin_Home"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        CType(Me.chartSales, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chartCategory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelMenu.ResumeLayout(False)
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents switchtimer As Timer
    Friend WithEvents Panel5 As Panel
    Friend WithEvents lbsalestoday As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents weweqeqwe As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lbllowstosdasdsadck As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents panel12 As Label
    Friend WithEvents Panel8 As Panel
    Friend WithEvents lblUserName As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel6 As Panel
    Friend WithEvents chartSales As DataVisualization.Charting.Chart
    Friend WithEvents chartCategory As DataVisualization.Charting.Chart
    Friend WithEvents btnWeekly As Button
    Friend WithEvents btnMontly As Button
    Friend WithEvents btnyearly As Button
    Friend WithEvents lblLowStock As Label
    Friend WithEvents lblTransactions As Label
    Friend WithEvents lblSalesToday As Label
    Friend WithEvents lblDiscount As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnHome As Button
    Friend WithEvents btnFileMaintenance As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnDelivery As Button
    Friend WithEvents btnInventory As Button
    Friend WithEvents btnPos As Button
    Friend WithEvents btnTransaction As Button
    Friend WithEvents btnAuditTrail As Button
    Friend WithEvents PanelMenu As Panel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents lblActiveUSer As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblProfitToday As Label
    Friend WithEvents Label5 As Label
End Class
