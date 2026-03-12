<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPOS
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.datetimer = New System.Windows.Forms.Timer(Me.components)
        Me.txtVatable = New System.Windows.Forms.TextBox()
        Me.txtVat = New System.Windows.Forms.TextBox()
        Me.Guna2Panel4 = New Guna.UI2.WinForms.Guna2Panel()
        Me.DGVtable = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2Panel3 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblSearch = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.txtTotal = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtCash = New Guna.UI2.WinForms.Guna2TextBox()
        Me.cbdiscount = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtChange = New System.Windows.Forms.Label()
        Me.txtVatRate = New System.Windows.Forms.Label()
        Me.txtDiscountAmt = New System.Windows.Forms.Label()
        Me.txtSubtotal = New System.Windows.Forms.Label()
        Me.lblTransactionNo = New System.Windows.Forms.Label()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.Panelmenu = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel2 = New Guna.UI2.WinForms.Guna2Panel()
        Me.btnLogout = New Guna.UI2.WinForms.Guna2Button()
        Me.btnReports = New Guna.UI2.WinForms.Guna2Button()
        Me.btnAuditTrail = New Guna.UI2.WinForms.Guna2Button()
        Me.btnTransaction = New Guna.UI2.WinForms.Guna2Button()
        Me.btnPos = New Guna.UI2.WinForms.Guna2Button()
        Me.btnInventory = New Guna.UI2.WinForms.Guna2Button()
        Me.btnReturns = New Guna.UI2.WinForms.Guna2Button()
        Me.btnDelivery = New Guna.UI2.WinForms.Guna2Button()
        Me.btnFileMaintenance = New Guna.UI2.WinForms.Guna2Button()
        Me.btnHome = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel7 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel8 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel9 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel11 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblFirstname = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblUserLevel = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2CirclePictureBox1 = New Guna.UI2.WinForms.Guna2CirclePictureBox()
        Me.btnPrinter = New Guna.UI2.WinForms.Guna2Button()
        Me.btnPay = New Guna.UI2.WinForms.Guna2Button()
        Me.btnNewTransaction = New Guna.UI2.WinForms.Guna2Button()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel4.SuspendLayout()
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2Panel3.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.Panelmenu.SuspendLayout()
        Me.Guna2Panel2.SuspendLayout()
        Me.Guna2Panel9.SuspendLayout()
        Me.Guna2Panel11.SuspendLayout()
        CType(Me.Guna2CirclePictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(10, 274)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(109, 29)
        Me.Label11.TabIndex = 45
        Me.Label11.Text = "Vat Rate:"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.Location = New System.Drawing.Point(20, 444)
        Me.Panel5.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(347, 1)
        Me.Panel5.TabIndex = 43
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(10, 222)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(159, 29)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Discount Amt:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(10, 63)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(183, 29)
        Me.Label3.TabIndex = 40
        Me.Label3.Text = "Transaction No:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(15, 15)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 29)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "Date:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(10, 477)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 29)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Change:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(10, 383)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(74, 29)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Cash:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(10, 533)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 29)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Total:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(10, 331)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 29)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Discount:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(10, 169)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 29)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Subtotal:"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblDate.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(205, 24)
        Me.lblDate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(45, 18)
        Me.lblDate.TabIndex = 1
        Me.lblDate.Text = "Date:"
        '
        'datetimer
        '
        '
        'txtVatable
        '
        Me.txtVatable.Enabled = False
        Me.txtVatable.Location = New System.Drawing.Point(1921, 680)
        Me.txtVatable.Margin = New System.Windows.Forms.Padding(4)
        Me.txtVatable.Name = "txtVatable"
        Me.txtVatable.ReadOnly = True
        Me.txtVatable.Size = New System.Drawing.Size(153, 35)
        Me.txtVatable.TabIndex = 10
        Me.txtVatable.Visible = False
        '
        'txtVat
        '
        Me.txtVat.Enabled = False
        Me.txtVat.Location = New System.Drawing.Point(1921, 731)
        Me.txtVat.Margin = New System.Windows.Forms.Padding(4)
        Me.txtVat.Name = "txtVat"
        Me.txtVat.ReadOnly = True
        Me.txtVat.Size = New System.Drawing.Size(153, 35)
        Me.txtVat.TabIndex = 11
        Me.txtVat.Visible = False
        '
        'Guna2Panel4
        '
        Me.Guna2Panel4.BorderRadius = 15
        Me.Guna2Panel4.Controls.Add(Me.DGVtable)
        Me.Guna2Panel4.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel4.Location = New System.Drawing.Point(231, 138)
        Me.Guna2Panel4.Name = "Guna2Panel4"
        Me.Guna2Panel4.Size = New System.Drawing.Size(1163, 740)
        Me.Guna2Panel4.TabIndex = 92
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
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVtable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DGVtable.ColumnHeadersHeight = 34
        Me.DGVtable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.DGVtable.Size = New System.Drawing.Size(1121, 642)
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
        Me.DGVtable.ThemeStyle.HeaderStyle.Height = 34
        Me.DGVtable.ThemeStyle.ReadOnly = True
        Me.DGVtable.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.DGVtable.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.DGVtable.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DGVtable.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.DGVtable.ThemeStyle.RowsStyle.Height = 22
        Me.DGVtable.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'Guna2Panel3
        '
        Me.Guna2Panel3.BorderRadius = 15
        Me.Guna2Panel3.Controls.Add(Me.btnAdd)
        Me.Guna2Panel3.Controls.Add(Me.lblSearch)
        Me.Guna2Panel3.Controls.Add(Me.txtSearch)
        Me.Guna2Panel3.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel3.Location = New System.Drawing.Point(231, 6)
        Me.Guna2Panel3.Name = "Guna2Panel3"
        Me.Guna2Panel3.Size = New System.Drawing.Size(1162, 127)
        Me.Guna2Panel3.TabIndex = 91
        '
        'lblSearch
        '
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(17, 19)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(213, 27)
        Me.lblSearch.TabIndex = 54
        Me.lblSearch.Text = "Add / Search Product:"
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
        Me.txtSearch.ForeColor = System.Drawing.Color.Black
        Me.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(17, 54)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.PlaceholderText = "Nike" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Adidas" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Puma"
        Me.txtSearch.SelectedText = ""
        Me.txtSearch.Size = New System.Drawing.Size(470, 36)
        Me.txtSearch.TabIndex = 0
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel1.BorderRadius = 15
        Me.Guna2Panel1.Controls.Add(Me.btnPrinter)
        Me.Guna2Panel1.Controls.Add(Me.txtTotal)
        Me.Guna2Panel1.Controls.Add(Me.btnPay)
        Me.Guna2Panel1.Controls.Add(Me.btnNewTransaction)
        Me.Guna2Panel1.Controls.Add(Me.Panel1)
        Me.Guna2Panel1.Controls.Add(Me.txtCash)
        Me.Guna2Panel1.Controls.Add(Me.cbdiscount)
        Me.Guna2Panel1.Controls.Add(Me.txtChange)
        Me.Guna2Panel1.Controls.Add(Me.txtVatRate)
        Me.Guna2Panel1.Controls.Add(Me.txtDiscountAmt)
        Me.Guna2Panel1.Controls.Add(Me.txtSubtotal)
        Me.Guna2Panel1.Controls.Add(Me.Label2)
        Me.Guna2Panel1.Controls.Add(Me.Label11)
        Me.Guna2Panel1.Controls.Add(Me.lblDate)
        Me.Guna2Panel1.Controls.Add(Me.lblTransactionNo)
        Me.Guna2Panel1.Controls.Add(Me.Panel5)
        Me.Guna2Panel1.Controls.Add(Me.Label5)
        Me.Guna2Panel1.Controls.Add(Me.Label6)
        Me.Guna2Panel1.Controls.Add(Me.Label10)
        Me.Guna2Panel1.Controls.Add(Me.Label7)
        Me.Guna2Panel1.Controls.Add(Me.Label3)
        Me.Guna2Panel1.Controls.Add(Me.Label8)
        Me.Guna2Panel1.Controls.Add(Me.Label9)
        Me.Guna2Panel1.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel1.Location = New System.Drawing.Point(1400, 6)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(388, 867)
        Me.Guna2Panel1.TabIndex = 55
        '
        'txtTotal
        '
        Me.txtTotal.AutoSize = True
        Me.txtTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtTotal.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotal.ForeColor = System.Drawing.Color.White
        Me.txtTotal.Location = New System.Drawing.Point(200, 533)
        Me.txtTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(59, 29)
        Me.txtTotal.TabIndex = 56
        Me.txtTotal.Text = "0.00"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Location = New System.Drawing.Point(20, 126)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(347, 1)
        Me.Panel1.TabIndex = 53
        '
        'txtCash
        '
        Me.txtCash.Animated = True
        Me.txtCash.AutoRoundedCorners = True
        Me.txtCash.BackColor = System.Drawing.Color.Transparent
        Me.txtCash.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCash.DefaultText = ""
        Me.txtCash.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCash.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCash.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCash.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCash.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCash.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtCash.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCash.Location = New System.Drawing.Point(205, 383)
        Me.txtCash.Name = "txtCash"
        Me.txtCash.PlaceholderText = ""
        Me.txtCash.SelectedText = ""
        Me.txtCash.Size = New System.Drawing.Size(140, 36)
        Me.txtCash.TabIndex = 52
        '
        'cbdiscount
        '
        Me.cbdiscount.AutoRoundedCorners = True
        Me.cbdiscount.BackColor = System.Drawing.Color.Transparent
        Me.cbdiscount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbdiscount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbdiscount.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cbdiscount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cbdiscount.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbdiscount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbdiscount.ItemHeight = 30
        Me.cbdiscount.Location = New System.Drawing.Point(205, 324)
        Me.cbdiscount.Name = "cbdiscount"
        Me.cbdiscount.Size = New System.Drawing.Size(140, 36)
        Me.cbdiscount.TabIndex = 51
        '
        'txtChange
        '
        Me.txtChange.AutoSize = True
        Me.txtChange.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtChange.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChange.ForeColor = System.Drawing.Color.White
        Me.txtChange.Location = New System.Drawing.Point(202, 488)
        Me.txtChange.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtChange.Name = "txtChange"
        Me.txtChange.Size = New System.Drawing.Size(36, 18)
        Me.txtChange.TabIndex = 50
        Me.txtChange.Text = "0.00"
        '
        'txtVatRate
        '
        Me.txtVatRate.AutoSize = True
        Me.txtVatRate.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtVatRate.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVatRate.ForeColor = System.Drawing.Color.White
        Me.txtVatRate.Location = New System.Drawing.Point(205, 279)
        Me.txtVatRate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtVatRate.Name = "txtVatRate"
        Me.txtVatRate.Size = New System.Drawing.Size(44, 18)
        Me.txtVatRate.TabIndex = 49
        Me.txtVatRate.Text = "12.00"
        '
        'txtDiscountAmt
        '
        Me.txtDiscountAmt.AutoSize = True
        Me.txtDiscountAmt.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtDiscountAmt.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscountAmt.ForeColor = System.Drawing.Color.White
        Me.txtDiscountAmt.Location = New System.Drawing.Point(205, 227)
        Me.txtDiscountAmt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtDiscountAmt.Name = "txtDiscountAmt"
        Me.txtDiscountAmt.Size = New System.Drawing.Size(36, 18)
        Me.txtDiscountAmt.TabIndex = 48
        Me.txtDiscountAmt.Text = "0.00"
        '
        'txtSubtotal
        '
        Me.txtSubtotal.AutoSize = True
        Me.txtSubtotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.txtSubtotal.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubtotal.ForeColor = System.Drawing.Color.White
        Me.txtSubtotal.Location = New System.Drawing.Point(205, 174)
        Me.txtSubtotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.Size = New System.Drawing.Size(36, 18)
        Me.txtSubtotal.TabIndex = 47
        Me.txtSubtotal.Text = "0.00"
        '
        'lblTransactionNo
        '
        Me.lblTransactionNo.AutoSize = True
        Me.lblTransactionNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblTransactionNo.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionNo.ForeColor = System.Drawing.Color.White
        Me.lblTransactionNo.Location = New System.Drawing.Point(205, 72)
        Me.lblTransactionNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTransactionNo.Name = "lblTransactionNo"
        Me.lblTransactionNo.Size = New System.Drawing.Size(119, 18)
        Me.lblTransactionNo.TabIndex = 2
        Me.lblTransactionNo.Text = "Transaction No:"
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 30
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = True
        '
        'Panelmenu
        '
        Me.Panelmenu.BorderRadius = 15
        Me.Panelmenu.Controls.Add(Me.Guna2Panel2)
        Me.Panelmenu.Controls.Add(Me.Guna2Panel9)
        Me.Panelmenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panelmenu.FillColor = System.Drawing.Color.Black
        Me.Panelmenu.Location = New System.Drawing.Point(0, 0)
        Me.Panelmenu.Name = "Panelmenu"
        Me.Panelmenu.Size = New System.Drawing.Size(225, 885)
        Me.Panelmenu.TabIndex = 107
        '
        'Guna2Panel2
        '
        Me.Guna2Panel2.Controls.Add(Me.btnLogout)
        Me.Guna2Panel2.Controls.Add(Me.btnReports)
        Me.Guna2Panel2.Controls.Add(Me.btnAuditTrail)
        Me.Guna2Panel2.Controls.Add(Me.btnTransaction)
        Me.Guna2Panel2.Controls.Add(Me.btnPos)
        Me.Guna2Panel2.Controls.Add(Me.btnInventory)
        Me.Guna2Panel2.Controls.Add(Me.btnReturns)
        Me.Guna2Panel2.Controls.Add(Me.btnDelivery)
        Me.Guna2Panel2.Controls.Add(Me.btnFileMaintenance)
        Me.Guna2Panel2.Controls.Add(Me.btnHome)
        Me.Guna2Panel2.Controls.Add(Me.Guna2Panel7)
        Me.Guna2Panel2.Controls.Add(Me.Guna2Panel8)
        Me.Guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel2.FillColor = System.Drawing.Color.Black
        Me.Guna2Panel2.Location = New System.Drawing.Point(0, 124)
        Me.Guna2Panel2.Name = "Guna2Panel2"
        Me.Guna2Panel2.Size = New System.Drawing.Size(225, 477)
        Me.Guna2Panel2.TabIndex = 79
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
        Me.btnLogout.Location = New System.Drawing.Point(11, 396)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnLogout.Size = New System.Drawing.Size(204, 44)
        Me.btnLogout.TabIndex = 17
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseTransparentBackground = True
        '
        'btnReports
        '
        Me.btnReports.Animated = True
        Me.btnReports.AutoRoundedCorners = True
        Me.btnReports.BackColor = System.Drawing.Color.Transparent
        Me.btnReports.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnReports.DefaultAutoSize = True
        Me.btnReports.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnReports.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnReports.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnReports.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnReports.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnReports.FillColor = System.Drawing.Color.Black
        Me.btnReports.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReports.ForeColor = System.Drawing.Color.White
        Me.btnReports.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnReports.IndicateFocus = True
        Me.btnReports.Location = New System.Drawing.Point(11, 352)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnReports.Size = New System.Drawing.Size(204, 44)
        Me.btnReports.TabIndex = 81
        Me.btnReports.Text = "Reports"
        Me.btnReports.UseTransparentBackground = True
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
        Me.btnAuditTrail.Location = New System.Drawing.Point(11, 308)
        Me.btnAuditTrail.Name = "btnAuditTrail"
        Me.btnAuditTrail.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnAuditTrail.Size = New System.Drawing.Size(204, 44)
        Me.btnAuditTrail.TabIndex = 16
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
        Me.btnTransaction.Location = New System.Drawing.Point(11, 264)
        Me.btnTransaction.Name = "btnTransaction"
        Me.btnTransaction.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnTransaction.Size = New System.Drawing.Size(204, 44)
        Me.btnTransaction.TabIndex = 15
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
        Me.btnPos.Location = New System.Drawing.Point(11, 220)
        Me.btnPos.Name = "btnPos"
        Me.btnPos.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnPos.Size = New System.Drawing.Size(204, 44)
        Me.btnPos.TabIndex = 14
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
        Me.btnInventory.Location = New System.Drawing.Point(11, 176)
        Me.btnInventory.Name = "btnInventory"
        Me.btnInventory.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnInventory.Size = New System.Drawing.Size(204, 44)
        Me.btnInventory.TabIndex = 13
        Me.btnInventory.Text = "Inventory"
        Me.btnInventory.UseTransparentBackground = True
        '
        'btnReturns
        '
        Me.btnReturns.Animated = True
        Me.btnReturns.AutoRoundedCorners = True
        Me.btnReturns.BackColor = System.Drawing.Color.Transparent
        Me.btnReturns.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnReturns.DefaultAutoSize = True
        Me.btnReturns.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnReturns.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnReturns.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnReturns.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnReturns.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnReturns.FillColor = System.Drawing.Color.Black
        Me.btnReturns.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReturns.ForeColor = System.Drawing.Color.White
        Me.btnReturns.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnReturns.IndicateFocus = True
        Me.btnReturns.Location = New System.Drawing.Point(11, 132)
        Me.btnReturns.Name = "btnReturns"
        Me.btnReturns.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnReturns.Size = New System.Drawing.Size(204, 44)
        Me.btnReturns.TabIndex = 78
        Me.btnReturns.Text = "Returns"
        Me.btnReturns.UseTransparentBackground = True
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
        Me.btnDelivery.Location = New System.Drawing.Point(11, 88)
        Me.btnDelivery.Name = "btnDelivery"
        Me.btnDelivery.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnDelivery.Size = New System.Drawing.Size(204, 44)
        Me.btnDelivery.TabIndex = 12
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
        Me.btnFileMaintenance.Location = New System.Drawing.Point(11, 44)
        Me.btnFileMaintenance.Name = "btnFileMaintenance"
        Me.btnFileMaintenance.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnFileMaintenance.Size = New System.Drawing.Size(204, 44)
        Me.btnFileMaintenance.TabIndex = 11
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
        Me.btnHome.Location = New System.Drawing.Point(11, 0)
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Padding = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.btnHome.Size = New System.Drawing.Size(204, 44)
        Me.btnHome.TabIndex = 10
        Me.btnHome.Text = "Home"
        Me.btnHome.UseTransparentBackground = True
        '
        'Guna2Panel7
        '
        Me.Guna2Panel7.BackColor = System.Drawing.Color.Black
        Me.Guna2Panel7.BorderRadius = 30
        Me.Guna2Panel7.CustomizableEdges.BottomRight = False
        Me.Guna2Panel7.CustomizableEdges.TopLeft = False
        Me.Guna2Panel7.CustomizableEdges.TopRight = False
        Me.Guna2Panel7.Dock = System.Windows.Forms.DockStyle.Left
        Me.Guna2Panel7.FillColor = System.Drawing.Color.Black
        Me.Guna2Panel7.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel7.Name = "Guna2Panel7"
        Me.Guna2Panel7.Size = New System.Drawing.Size(11, 477)
        Me.Guna2Panel7.TabIndex = 76
        '
        'Guna2Panel8
        '
        Me.Guna2Panel8.Dock = System.Windows.Forms.DockStyle.Right
        Me.Guna2Panel8.FillColor = System.Drawing.Color.Black
        Me.Guna2Panel8.Location = New System.Drawing.Point(215, 0)
        Me.Guna2Panel8.Name = "Guna2Panel8"
        Me.Guna2Panel8.Size = New System.Drawing.Size(10, 477)
        Me.Guna2Panel8.TabIndex = 77
        '
        'Guna2Panel9
        '
        Me.Guna2Panel9.BackColor = System.Drawing.Color.Transparent
        Me.Guna2Panel9.Controls.Add(Me.Guna2Panel11)
        Me.Guna2Panel9.CustomizableEdges.TopRight = False
        Me.Guna2Panel9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel9.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel9.Name = "Guna2Panel9"
        Me.Guna2Panel9.Size = New System.Drawing.Size(225, 124)
        Me.Guna2Panel9.TabIndex = 112
        '
        'Guna2Panel11
        '
        Me.Guna2Panel11.BorderRadius = 15
        Me.Guna2Panel11.Controls.Add(Me.Guna2CirclePictureBox1)
        Me.Guna2Panel11.Controls.Add(Me.lblFirstname)
        Me.Guna2Panel11.Controls.Add(Me.lblUserLevel)
        Me.Guna2Panel11.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel11.Location = New System.Drawing.Point(12, 25)
        Me.Guna2Panel11.Name = "Guna2Panel11"
        Me.Guna2Panel11.Size = New System.Drawing.Size(203, 73)
        Me.Guna2Panel11.TabIndex = 116
        '
        'lblFirstname
        '
        Me.lblFirstname.AutoSize = False
        Me.lblFirstname.BackColor = System.Drawing.Color.Transparent
        Me.lblFirstname.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstname.ForeColor = System.Drawing.Color.White
        Me.lblFirstname.Location = New System.Drawing.Point(70, 6)
        Me.lblFirstname.Name = "lblFirstname"
        Me.lblFirstname.Size = New System.Drawing.Size(140, 35)
        Me.lblFirstname.TabIndex = 111
        Me.lblFirstname.Text = "Chan"
        Me.lblFirstname.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUserLevel
        '
        Me.lblUserLevel.AutoSize = False
        Me.lblUserLevel.BackColor = System.Drawing.Color.Transparent
        Me.lblUserLevel.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserLevel.ForeColor = System.Drawing.Color.White
        Me.lblUserLevel.Location = New System.Drawing.Point(70, 41)
        Me.lblUserLevel.Name = "lblUserLevel"
        Me.lblUserLevel.Size = New System.Drawing.Size(109, 32)
        Me.lblUserLevel.TabIndex = 112
        Me.lblUserLevel.Text = "Cashier"
        '
        'Guna2CirclePictureBox1
        '
        Me.Guna2CirclePictureBox1.Image = Global.JJAS_ISBST.My.Resources.Resources.ChatGPT_Image_Feb_21__2026__09_38_54_PM
        Me.Guna2CirclePictureBox1.ImageRotate = 0!
        Me.Guna2CirclePictureBox1.Location = New System.Drawing.Point(5, 6)
        Me.Guna2CirclePictureBox1.Name = "Guna2CirclePictureBox1"
        Me.Guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        Me.Guna2CirclePictureBox1.Size = New System.Drawing.Size(64, 64)
        Me.Guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Guna2CirclePictureBox1.TabIndex = 111
        Me.Guna2CirclePictureBox1.TabStop = False
        '
        'btnPrinter
        '
        Me.btnPrinter.Animated = True
        Me.btnPrinter.BackColor = System.Drawing.Color.Transparent
        Me.btnPrinter.BorderRadius = 15
        Me.btnPrinter.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPrinter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPrinter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPrinter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPrinter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPrinter.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnPrinter.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrinter.ForeColor = System.Drawing.Color.White
        Me.btnPrinter.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnPrinter.Image = Global.JJAS_ISBST.My.Resources.Resources.printer__1_
        Me.btnPrinter.IndicateFocus = True
        Me.btnPrinter.Location = New System.Drawing.Point(15, 799)
        Me.btnPrinter.Name = "btnPrinter"
        Me.btnPrinter.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnPrinter.Size = New System.Drawing.Size(360, 51)
        Me.btnPrinter.TabIndex = 57
        Me.btnPrinter.Text = "Printer Setting"
        Me.btnPrinter.UseTransparentBackground = True
        '
        'btnPay
        '
        Me.btnPay.Animated = True
        Me.btnPay.AutoRoundedCorners = True
        Me.btnPay.BackColor = System.Drawing.Color.Transparent
        Me.btnPay.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPay.DefaultAutoSize = True
        Me.btnPay.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPay.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPay.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPay.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPay.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnPay.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPay.ForeColor = System.Drawing.Color.White
        Me.btnPay.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnPay.Image = Global.JJAS_ISBST.My.Resources.Resources.coin
        Me.btnPay.IndicateFocus = True
        Me.btnPay.Location = New System.Drawing.Point(244, 587)
        Me.btnPay.Name = "btnPay"
        Me.btnPay.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnPay.Size = New System.Drawing.Size(101, 51)
        Me.btnPay.TabIndex = 55
        Me.btnPay.Text = " Pay"
        Me.btnPay.UseTransparentBackground = True
        '
        'btnNewTransaction
        '
        Me.btnNewTransaction.Animated = True
        Me.btnNewTransaction.AutoRoundedCorners = True
        Me.btnNewTransaction.BackColor = System.Drawing.Color.Transparent
        Me.btnNewTransaction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNewTransaction.DefaultAutoSize = True
        Me.btnNewTransaction.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnNewTransaction.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnNewTransaction.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnNewTransaction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnNewTransaction.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnNewTransaction.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewTransaction.ForeColor = System.Drawing.Color.White
        Me.btnNewTransaction.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnNewTransaction.Image = Global.JJAS_ISBST.My.Resources.Resources._new
        Me.btnNewTransaction.IndicateFocus = True
        Me.btnNewTransaction.Location = New System.Drawing.Point(15, 587)
        Me.btnNewTransaction.Name = "btnNewTransaction"
        Me.btnNewTransaction.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnNewTransaction.Size = New System.Drawing.Size(208, 51)
        Me.btnNewTransaction.TabIndex = 54
        Me.btnNewTransaction.Text = "  New Transaction"
        Me.btnNewTransaction.UseTransparentBackground = True
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
        Me.btnAdd.Location = New System.Drawing.Point(1025, 37)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnAdd.Size = New System.Drawing.Size(113, 55)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = " Add"
        Me.btnAdd.UseTransparentBackground = True
        '
        'FrmPOS
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1800, 885)
        Me.Controls.Add(Me.Panelmenu)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.Guna2Panel4)
        Me.Controls.Add(Me.Guna2Panel3)
        Me.Controls.Add(Me.txtVat)
        Me.Controls.Add(Me.txtVatable)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmPOS"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmPOS"
        Me.Guna2Panel4.ResumeLayout(False)
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2Panel3.ResumeLayout(False)
        Me.Guna2Panel3.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.Panelmenu.ResumeLayout(False)
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.Guna2Panel9.ResumeLayout(False)
        Me.Guna2Panel11.ResumeLayout(False)
        CType(Me.Guna2CirclePictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDate As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents datetimer As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents txtVat As TextBox
    Friend WithEvents txtVatable As TextBox
    Friend WithEvents Guna2Panel4 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents DGVtable As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Guna2Panel3 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblSearch As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtSubtotal As Label
    Friend WithEvents lblTransactionNo As Label
    Friend WithEvents txtVatRate As Label
    Friend WithEvents txtDiscountAmt As Label
    Friend WithEvents txtCash As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cbdiscount As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents txtChange As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnNewTransaction As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents txtTotal As Label
    Friend WithEvents btnPay As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents Panelmenu As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Panel2 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnLogout As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnAuditTrail As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnTransaction As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPos As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnInventory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnDelivery As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnFileMaintenance As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnHome As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel7 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Panel8 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnReturns As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnReports As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel9 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Panel11 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2CirclePictureBox1 As Guna.UI2.WinForms.Guna2CirclePictureBox
    Friend WithEvents lblFirstname As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblUserLevel As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnPrinter As Guna.UI2.WinForms.Guna2Button
End Class
