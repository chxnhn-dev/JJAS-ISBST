<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBrandEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBrandEntry))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnedit = New Guna.UI2.WinForms.Guna2Button()
        Me.EentryName = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnadd = New Guna.UI2.WinForms.Guna2Button()
        Me.txtValue = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.Guna2BorderlessForm2 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel1.Controls.Add(Me.btnedit)
        Me.Panel1.Controls.Add(Me.EentryName)
        Me.Panel1.Controls.Add(Me.btnadd)
        Me.Panel1.Controls.Add(Me.txtValue)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 51)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(319, 182)
        Me.Panel1.TabIndex = 74
        '
        'btnedit
        '
        Me.btnedit.Animated = True
        Me.btnedit.AutoRoundedCorners = True
        Me.btnedit.BackColor = System.Drawing.Color.Transparent
        Me.btnedit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnedit.DefaultAutoSize = True
        Me.btnedit.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnedit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnedit.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnedit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnedit.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnedit.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.btnedit.ForeColor = System.Drawing.Color.White
        Me.btnedit.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnedit.Image = Global.JJAS_ISBST.My.Resources.Resources.edit
        Me.btnedit.IndicateFocus = True
        Me.btnedit.Location = New System.Drawing.Point(165, 103)
        Me.btnedit.Name = "btnedit"
        Me.btnedit.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnedit.Size = New System.Drawing.Size(142, 55)
        Me.btnedit.TabIndex = 75
        Me.btnedit.Text = " Update"
        Me.btnedit.UseTransparentBackground = True
        Me.btnedit.Visible = False
        '
        'EentryName
        '
        Me.EentryName.BackColor = System.Drawing.Color.Transparent
        Me.EentryName.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EentryName.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.EentryName.Location = New System.Drawing.Point(17, 10)
        Me.EentryName.Name = "EentryName"
        Me.EentryName.Size = New System.Drawing.Size(53, 27)
        Me.EentryName.TabIndex = 74
        Me.EentryName.Text = "Label:"
        '
        'btnadd
        '
        Me.btnadd.Animated = True
        Me.btnadd.AutoRoundedCorners = True
        Me.btnadd.BackColor = System.Drawing.Color.Transparent
        Me.btnadd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnadd.DefaultAutoSize = True
        Me.btnadd.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnadd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnadd.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnadd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnadd.FillColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnadd.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.btnadd.ForeColor = System.Drawing.Color.White
        Me.btnadd.HoverState.FillColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.btnadd.Image = Nothing
        Me.btnadd.IndicateFocus = True
        Me.btnadd.Location = New System.Drawing.Point(194, 103)
        Me.btnadd.Name = "btnadd"
        Me.btnadd.Padding = New System.Windows.Forms.Padding(8, 10, 10, 8)
        Me.btnadd.Size = New System.Drawing.Size(113, 55)
        Me.btnadd.TabIndex = 73
        Me.btnadd.Text = "Save"
        Me.btnadd.UseTransparentBackground = True
        '
        'txtValue
        '
        Me.txtValue.Animated = True
        Me.txtValue.AutoRoundedCorners = True
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.DefaultText = ""
        Me.txtValue.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtValue.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtValue.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtValue.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtValue.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtValue.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.Color.Black
        Me.txtValue.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtValue.Location = New System.Drawing.Point(12, 44)
        Me.txtValue.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.PlaceholderText = "Nike"
        Me.txtValue.SelectedText = ""
        Me.txtValue.Size = New System.Drawing.Size(295, 36)
        Me.txtValue.TabIndex = 72
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label12.Location = New System.Drawing.Point(12, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 28)
        Me.Label12.TabIndex = 73
        Me.Label12.Text = "Label"
        '
        'btnexit
        '
        Me.btnexit.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.btnexit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnexit.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnexit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnexit.FlatAppearance.BorderSize = 0
        Me.btnexit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnexit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnexit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnexit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnexit.Image = CType(resources.GetObject("btnexit.Image"), System.Drawing.Image)
        Me.btnexit.Location = New System.Drawing.Point(284, 0)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(35, 51)
        Me.btnexit.TabIndex = 76
        Me.btnexit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnexit.UseVisualStyleBackColor = False
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 30
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = True
        '
        'Guna2BorderlessForm2
        '
        Me.Guna2BorderlessForm2.AnimateWindow = True
        Me.Guna2BorderlessForm2.BorderRadius = 30
        Me.Guna2BorderlessForm2.ContainerControl = Me
        Me.Guna2BorderlessForm2.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm2.TransparentWhileDrag = True
        '
        'FrmBrandEntry
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(319, 233)
        Me.Controls.Add(Me.btnexit)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label12)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmBrandEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmBrandEntry"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label12 As Label
    Friend WithEvents btnexit As Button
    Friend WithEvents txtValue As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnadd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnedit As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents EentryName As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2BorderlessForm2 As Guna.UI2.WinForms.Guna2BorderlessForm
End Class

