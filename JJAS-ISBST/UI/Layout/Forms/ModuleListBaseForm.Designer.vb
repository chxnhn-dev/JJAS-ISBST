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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Guna2Panel3 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblSearch = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.BtnPrint = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel4 = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblPage = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnNextPage = New Guna.UI2.WinForms.Guna2Button()
        Me.btnPreviousPage = New Guna.UI2.WinForms.Guna2Button()
        Me.DGVtable = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.panelMenu = New Guna.UI2.WinForms.Guna2Panel()
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
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Panel11 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2CirclePictureBox1 = New Guna.UI2.WinForms.Guna2CirclePictureBox()
        Me.lblFirstname = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblUserLevel = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Panel3.SuspendLayout()
        Me.Guna2Panel4.SuspendLayout()
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelMenu.SuspendLayout()
        Me.Guna2Panel2.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.Guna2Panel11.SuspendLayout()
        CType(Me.Guna2CirclePictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Guna2Panel3
        '
        Me.Guna2Panel3.BorderRadius = 15
        Me.Guna2Panel3.Controls.Add(Me.lblSearch)
        Me.Guna2Panel3.Controls.Add(Me.txtSearch)
        Me.Guna2Panel3.Controls.Add(Me.btnAdd)
        Me.Guna2Panel3.Controls.Add(Me.BtnPrint)
        Me.Guna2Panel3.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel3.Location = New System.Drawing.Point(231, 6)
        Me.Guna2Panel3.Name = "Guna2Panel3"
        Me.Guna2Panel3.Size = New System.Drawing.Size(1557, 127)
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
        'Guna2Panel4
        '
        Me.Guna2Panel4.BorderRadius = 15
        Me.Guna2Panel4.Controls.Add(Me.lblPage)
        Me.Guna2Panel4.Controls.Add(Me.btnNextPage)
        Me.Guna2Panel4.Controls.Add(Me.btnPreviousPage)
        Me.Guna2Panel4.Controls.Add(Me.DGVtable)
        Me.Guna2Panel4.FillColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Guna2Panel4.Location = New System.Drawing.Point(231, 139)
        Me.Guna2Panel4.Name = "Guna2Panel4"
        Me.Guna2Panel4.Size = New System.Drawing.Size(1557, 739)
        Me.Guna2Panel4.TabIndex = 105
        '
        'lblPage
        '
        Me.lblPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPage.BackColor = System.Drawing.Color.Transparent
        Me.lblPage.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPage.ForeColor = System.Drawing.Color.White
        Me.lblPage.Location = New System.Drawing.Point(1452, 36)
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
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        Me.DGVtable.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle6
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVtable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DGVtable.ColumnHeadersHeight = 34
        Me.DGVtable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGVtable.DefaultCellStyle = DataGridViewCellStyle8
        Me.DGVtable.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DGVtable.Location = New System.Drawing.Point(17, 76)
        Me.DGVtable.Margin = New System.Windows.Forms.Padding(4)
        Me.DGVtable.Name = "DGVtable"
        Me.DGVtable.ReadOnly = True
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVtable.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.DGVtable.RowHeadersVisible = False
        Me.DGVtable.RowHeadersWidth = 51
        Me.DGVtable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black
        Me.DGVtable.RowsDefaultCellStyle = DataGridViewCellStyle10
        Me.DGVtable.Size = New System.Drawing.Size(1516, 645)
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
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 30
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = True
        '
        'panelMenu
        '
        Me.panelMenu.BorderRadius = 15
        Me.panelMenu.Controls.Add(Me.Guna2Panel2)
        Me.panelMenu.Controls.Add(Me.Guna2Panel1)
        Me.panelMenu.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelMenu.FillColor = System.Drawing.Color.Black
        Me.panelMenu.Location = New System.Drawing.Point(0, 0)
        Me.panelMenu.Name = "panelMenu"
        Me.panelMenu.Size = New System.Drawing.Size(225, 885)
        Me.panelMenu.TabIndex = 106
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
        Me.btnReturns.TabIndex = 79
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
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2Panel1.Controls.Add(Me.Guna2Panel11)
        Me.Guna2Panel1.CustomizableEdges.TopRight = False
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(225, 124)
        Me.Guna2Panel1.TabIndex = 111
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
        'ModuleListBaseForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1800, 885)
        Me.Controls.Add(Me.panelMenu)
        Me.Controls.Add(Me.Guna2Panel4)
        Me.Controls.Add(Me.Guna2Panel3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ModuleListBaseForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ModuleListBaseForm"
        Me.Guna2Panel3.ResumeLayout(False)
        Me.Guna2Panel3.PerformLayout()
        Me.Guna2Panel4.ResumeLayout(False)
        Me.Guna2Panel4.PerformLayout()
        CType(Me.DGVtable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelMenu.ResumeLayout(False)
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel11.ResumeLayout(False)
        CType(Me.Guna2CirclePictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Guna2Panel3 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblSearch As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BtnPrint As Guna.UI2.WinForms.Guna2Button

    Friend WithEvents Guna2Panel4 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblPage As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnNextPage As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPreviousPage As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents DGVtable As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents panelMenu As Guna.UI2.WinForms.Guna2Panel
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
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Panel11 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2CirclePictureBox1 As Guna.UI2.WinForms.Guna2CirclePictureBox
    Friend WithEvents lblFirstname As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblUserLevel As Guna.UI2.WinForms.Guna2HtmlLabel
End Class
