<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Edit_User
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Edit_User))
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cbxShowpassword = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtConfirmPass = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtpassword = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtusername = New System.Windows.Forms.TextBox()
        Me.cbrole = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtaddress = New System.Windows.Forms.RichTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtcontactnumber = New System.Windows.Forms.TextBox()
        Me.txtfirstname = New System.Windows.Forms.TextBox()
        Me.txtlastname = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtemail = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
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
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.Location = New System.Drawing.Point(529, 341)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(87, 37)
        Me.btnEdit.TabIndex = 48
        Me.btnEdit.Text = "  Edit"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExit.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatAppearance.BorderSize = 0
        Me.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnExit.Image = CType(resources.GetObject("btnExit.Image"), System.Drawing.Image)
        Me.btnExit.Location = New System.Drawing.Point(610, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(35, 42)
        Me.btnExit.TabIndex = 67
        Me.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label12.Location = New System.Drawing.Point(12, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(101, 25)
        Me.Label12.TabIndex = 62
        Me.Label12.Text = "User Edit: "
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel2.Controls.Add(Me.cbxShowpassword)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.btnEdit)
        Me.Panel2.Controls.Add(Me.txtConfirmPass)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.txtpassword)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.txtusername)
        Me.Panel2.Controls.Add(Me.cbrole)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.txtaddress)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.txtcontactnumber)
        Me.Panel2.Controls.Add(Me.txtfirstname)
        Me.Panel2.Controls.Add(Me.txtlastname)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.txtemail)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(0, 42)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(645, 390)
        Me.Panel2.TabIndex = 68
        '
        'cbxShowpassword
        '
        Me.cbxShowpassword.AutoSize = True
        Me.cbxShowpassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxShowpassword.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.cbxShowpassword.Location = New System.Drawing.Point(355, 306)
        Me.cbxShowpassword.Name = "cbxShowpassword"
        Me.cbxShowpassword.Size = New System.Drawing.Size(140, 24)
        Me.cbxShowpassword.TabIndex = 66
        Me.cbxShowpassword.Text = "Show password"
        Me.cbxShowpassword.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label2.Location = New System.Drawing.Point(346, 185)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 24)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Password:"
        '
        'txtConfirmPass
        '
        Me.txtConfirmPass.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirmPass.Location = New System.Drawing.Point(350, 271)
        Me.txtConfirmPass.MaxLength = 50
        Me.txtConfirmPass.Name = "txtConfirmPass"
        Me.txtConfirmPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPass.Size = New System.Drawing.Size(266, 29)
        Me.txtConfirmPass.TabIndex = 46
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label1.Location = New System.Drawing.Point(346, 244)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(167, 24)
        Me.Label1.TabIndex = 45
        Me.Label1.Text = "Confirm Password:"
        '
        'txtpassword
        '
        Me.txtpassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpassword.Location = New System.Drawing.Point(350, 212)
        Me.txtpassword.MaxLength = 50
        Me.txtpassword.Name = "txtpassword"
        Me.txtpassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtpassword.Size = New System.Drawing.Size(266, 29)
        Me.txtpassword.TabIndex = 44
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label11.Location = New System.Drawing.Point(351, 278)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(270, 24)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "                                                    "
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label10.Location = New System.Drawing.Point(346, 126)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(102, 24)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Username:"
        '
        'txtusername
        '
        Me.txtusername.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtusername.Location = New System.Drawing.Point(350, 153)
        Me.txtusername.MaxLength = 50
        Me.txtusername.Name = "txtusername"
        Me.txtusername.Size = New System.Drawing.Size(269, 29)
        Me.txtusername.TabIndex = 42
        '
        'cbrole
        '
        Me.cbrole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbrole.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbrole.FormattingEnabled = True
        Me.cbrole.Items.AddRange(New Object() {"Staff", "Cashier"})
        Me.cbrole.Location = New System.Drawing.Point(22, 35)
        Me.cbrole.Name = "cbrole"
        Me.cbrole.Size = New System.Drawing.Size(271, 32)
        Me.cbrole.TabIndex = 25
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label9.Location = New System.Drawing.Point(18, 11)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 24)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Role:"
        '
        'txtaddress
        '
        Me.txtaddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtaddress.Location = New System.Drawing.Point(350, 38)
        Me.txtaddress.MaxLength = 200
        Me.txtaddress.Name = "txtaddress"
        Me.txtaddress.Size = New System.Drawing.Size(271, 85)
        Me.txtaddress.TabIndex = 23
        Me.txtaddress.Text = ""
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label3.Location = New System.Drawing.Point(18, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 24)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "First Name:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label6.Location = New System.Drawing.Point(20, 185)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(152, 24)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Contact Number:"
        '
        'txtcontactnumber
        '
        Me.txtcontactnumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcontactnumber.Location = New System.Drawing.Point(24, 212)
        Me.txtcontactnumber.MaxLength = 11
        Me.txtcontactnumber.Name = "txtcontactnumber"
        Me.txtcontactnumber.Size = New System.Drawing.Size(269, 29)
        Me.txtcontactnumber.TabIndex = 19
        '
        'txtfirstname
        '
        Me.txtfirstname.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtfirstname.Location = New System.Drawing.Point(22, 94)
        Me.txtfirstname.MaxLength = 50
        Me.txtfirstname.Name = "txtfirstname"
        Me.txtfirstname.Size = New System.Drawing.Size(271, 29)
        Me.txtfirstname.TabIndex = 19
        '
        'txtlastname
        '
        Me.txtlastname.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtlastname.Location = New System.Drawing.Point(24, 153)
        Me.txtlastname.MaxLength = 50
        Me.txtlastname.Name = "txtlastname"
        Me.txtlastname.Size = New System.Drawing.Size(269, 29)
        Me.txtlastname.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label8.Location = New System.Drawing.Point(346, 11)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(85, 24)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Address:"
        '
        'txtemail
        '
        Me.txtemail.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtemail.Location = New System.Drawing.Point(24, 271)
        Me.txtemail.MaxLength = 50
        Me.txtemail.Name = "txtemail"
        Me.txtemail.Size = New System.Drawing.Size(269, 29)
        Me.txtemail.TabIndex = 21
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label5.Location = New System.Drawing.Point(20, 126)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 24)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Last Name:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label7.Location = New System.Drawing.Point(20, 244)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 24)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Email:"
        '
        'Edit_User
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(645, 432)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label12)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Edit_User"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit_User"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents cbxShowpassword As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtConfirmPass As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtpassword As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents txtusername As TextBox
    Friend WithEvents cbrole As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtaddress As RichTextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtcontactnumber As TextBox
    Friend WithEvents txtfirstname As TextBox
    Friend WithEvents txtlastname As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtemail As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
End Class
