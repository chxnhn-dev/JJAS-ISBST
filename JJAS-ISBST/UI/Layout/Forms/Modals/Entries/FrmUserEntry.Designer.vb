<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUserEntry
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pnlMainContainer = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.btnCancel = New Guna.UI2.WinForms.Guna2Button()
        Me.btnClear = New Guna.UI2.WinForms.Guna2Button()
        Me.btnExit = New Guna.UI2.WinForms.Guna2Button()
        Me.cbxShowpassword = New Guna.UI2.WinForms.Guna2CheckBox()
        Me.txtConfirmPass = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtpassword = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtusername = New Guna.UI2.WinForms.Guna2TextBox()
        Me.cbrole = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtaddress = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtcontactnumber = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtfirstname = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtlastname = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtemail = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.SuspendLayout()
        '
        'pnlMainContainer
        '
        Me.pnlMainContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlMainContainer.Name = "pnlMainContainer"
        Me.pnlMainContainer.Size = New System.Drawing.Size(200, 100)
        Me.pnlMainContainer.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(100, 23)
        Me.lblTitle.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(0, 0)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(180, 45)
        Me.btnAdd.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(180, 45)
        Me.btnCancel.TabIndex = 0
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.ForeColor = System.Drawing.Color.White
        Me.btnClear.Location = New System.Drawing.Point(0, 0)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(180, 45)
        Me.btnClear.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnExit.ForeColor = System.Drawing.Color.White
        Me.btnExit.Location = New System.Drawing.Point(0, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(180, 45)
        Me.btnExit.TabIndex = 0
        '
        'cbxShowpassword
        '
        Me.cbxShowpassword.AutoSize = True
        Me.cbxShowpassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cbxShowpassword.CheckedState.BorderRadius = 0
        Me.cbxShowpassword.CheckedState.BorderThickness = 0
        Me.cbxShowpassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cbxShowpassword.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cbxShowpassword.ForeColor = System.Drawing.Color.White
        Me.cbxShowpassword.Location = New System.Drawing.Point(0, 0)
        Me.cbxShowpassword.Name = "cbxShowpassword"
        Me.cbxShowpassword.Size = New System.Drawing.Size(108, 19)
        Me.cbxShowpassword.TabIndex = 0
        Me.cbxShowpassword.Text = "Show password"
        Me.cbxShowpassword.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.cbxShowpassword.UncheckedState.BorderRadius = 0
        Me.cbxShowpassword.UncheckedState.BorderThickness = 0
        Me.cbxShowpassword.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        '
        'txtConfirmPass
        '
        Me.txtConfirmPass.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConfirmPass.DefaultText = ""
        Me.txtConfirmPass.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtConfirmPass.Location = New System.Drawing.Point(0, 0)
        Me.txtConfirmPass.MaxLength = 50
        Me.txtConfirmPass.Name = "txtConfirmPass"
        Me.txtConfirmPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPass.PlaceholderText = ""
        Me.txtConfirmPass.SelectedText = ""
        Me.txtConfirmPass.Size = New System.Drawing.Size(200, 36)
        Me.txtConfirmPass.TabIndex = 0
        '
        'txtpassword
        '
        Me.txtpassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtpassword.DefaultText = ""
        Me.txtpassword.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtpassword.Location = New System.Drawing.Point(0, 0)
        Me.txtpassword.MaxLength = 50
        Me.txtpassword.Name = "txtpassword"
        Me.txtpassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtpassword.PlaceholderText = ""
        Me.txtpassword.SelectedText = ""
        Me.txtpassword.Size = New System.Drawing.Size(200, 36)
        Me.txtpassword.TabIndex = 0
        '
        'txtusername
        '
        Me.txtusername.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtusername.DefaultText = ""
        Me.txtusername.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtusername.Location = New System.Drawing.Point(0, 0)
        Me.txtusername.MaxLength = 50
        Me.txtusername.Name = "txtusername"
        Me.txtusername.PlaceholderText = ""
        Me.txtusername.SelectedText = ""
        Me.txtusername.Size = New System.Drawing.Size(200, 36)
        Me.txtusername.TabIndex = 0
        '
        'cbrole
        '
        Me.cbrole.BackColor = System.Drawing.Color.Transparent
        Me.cbrole.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbrole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbrole.FocusedColor = System.Drawing.Color.Empty
        Me.cbrole.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cbrole.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cbrole.ItemHeight = 30
        Me.cbrole.Location = New System.Drawing.Point(0, 0)
        Me.cbrole.Name = "cbrole"
        Me.cbrole.Size = New System.Drawing.Size(140, 36)
        Me.cbrole.TabIndex = 0
        '
        'txtaddress
        '
        Me.txtaddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtaddress.DefaultText = ""
        Me.txtaddress.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtaddress.Location = New System.Drawing.Point(0, 0)
        Me.txtaddress.MaxLength = 200
        Me.txtaddress.Multiline = True
        Me.txtaddress.Name = "txtaddress"
        Me.txtaddress.PlaceholderText = ""
        Me.txtaddress.SelectedText = ""
        Me.txtaddress.Size = New System.Drawing.Size(200, 36)
        Me.txtaddress.TabIndex = 0
        '
        'txtcontactnumber
        '
        Me.txtcontactnumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtcontactnumber.DefaultText = ""
        Me.txtcontactnumber.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtcontactnumber.Location = New System.Drawing.Point(0, 0)
        Me.txtcontactnumber.MaxLength = 11
        Me.txtcontactnumber.Name = "txtcontactnumber"
        Me.txtcontactnumber.PlaceholderText = ""
        Me.txtcontactnumber.SelectedText = ""
        Me.txtcontactnumber.Size = New System.Drawing.Size(200, 36)
        Me.txtcontactnumber.TabIndex = 0
        '
        'txtfirstname
        '
        Me.txtfirstname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtfirstname.DefaultText = ""
        Me.txtfirstname.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtfirstname.Location = New System.Drawing.Point(0, 0)
        Me.txtfirstname.MaxLength = 50
        Me.txtfirstname.Name = "txtfirstname"
        Me.txtfirstname.PlaceholderText = ""
        Me.txtfirstname.SelectedText = ""
        Me.txtfirstname.Size = New System.Drawing.Size(200, 36)
        Me.txtfirstname.TabIndex = 0
        '
        'txtlastname
        '
        Me.txtlastname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtlastname.DefaultText = ""
        Me.txtlastname.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtlastname.Location = New System.Drawing.Point(0, 0)
        Me.txtlastname.MaxLength = 50
        Me.txtlastname.Name = "txtlastname"
        Me.txtlastname.PlaceholderText = ""
        Me.txtlastname.SelectedText = ""
        Me.txtlastname.Size = New System.Drawing.Size(200, 36)
        Me.txtlastname.TabIndex = 0
        '
        'txtemail
        '
        Me.txtemail.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtemail.DefaultText = ""
        Me.txtemail.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtemail.Location = New System.Drawing.Point(0, 0)
        Me.txtemail.MaxLength = 50
        Me.txtemail.Name = "txtemail"
        Me.txtemail.PlaceholderText = ""
        Me.txtemail.SelectedText = ""
        Me.txtemail.Size = New System.Drawing.Size(200, 36)
        Me.txtemail.TabIndex = 0
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 30
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = True
        '
        'FrmUserEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(22, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(760, 760)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximumSize = New System.Drawing.Size(760, 760)
        Me.MinimumSize = New System.Drawing.Size(760, 760)
        Me.Name = "FrmUserEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmUserEntry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlMainContainer As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnCancel As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnClear As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnExit As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents cbxShowpassword As Guna.UI2.WinForms.Guna2CheckBox
    Friend WithEvents txtConfirmPass As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtpassword As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtusername As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cbrole As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents txtaddress As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtcontactnumber As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtfirstname As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtlastname As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtemail As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
End Class
