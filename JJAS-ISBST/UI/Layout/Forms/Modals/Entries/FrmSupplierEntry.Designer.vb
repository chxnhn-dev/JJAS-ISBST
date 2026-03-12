<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSupplierEntry
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
        Me.pnlBodyHost = New System.Windows.Forms.Panel()
        Me.pnlCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlFields = New System.Windows.Forms.Panel()
        Me.lblReturnWindow = New System.Windows.Forms.Label()
        Me.nudReturnWindowDays = New Guna.UI2.WinForms.Guna2NumericUpDown()
        Me.chkAcceptsReturnRefund = New Guna.UI2.WinForms.Guna2CheckBox()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.txtAddress = New Guna.UI2.WinForms.Guna2TextBox()
        Me.lblContactNumber = New System.Windows.Forms.Label()
        Me.txtContactNumber = New Guna.UI2.WinForms.Guna2TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.txtCompany = New Guna.UI2.WinForms.Guna2TextBox()
        Me.pnlActions = New System.Windows.Forms.Panel()
        Me.btnCancel = New Guna.UI2.WinForms.Guna2Button()
        Me.btnAdd = New Guna.UI2.WinForms.Guna2Button()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.btnExit = New Guna.UI2.WinForms.Guna2Button()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.Guna2BorderlessForm2 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.pnlMainContainer.SuspendLayout()
        Me.pnlBodyHost.SuspendLayout()
        Me.pnlCard.SuspendLayout()
        Me.pnlFields.SuspendLayout()
        CType(Me.nudReturnWindowDays, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlActions.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMainContainer
        '
        Me.pnlMainContainer.Controls.Add(Me.pnlBodyHost)
        Me.pnlMainContainer.Controls.Add(Me.pnlHeader)
        Me.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMainContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlMainContainer.Name = "pnlMainContainer"
        Me.pnlMainContainer.Size = New System.Drawing.Size(820, 690)
        Me.pnlMainContainer.TabIndex = 0
        '
        'pnlBodyHost
        '
        Me.pnlBodyHost.Controls.Add(Me.pnlCard)
        Me.pnlBodyHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBodyHost.Location = New System.Drawing.Point(0, 60)
        Me.pnlBodyHost.Name = "pnlBodyHost"
        Me.pnlBodyHost.Size = New System.Drawing.Size(820, 630)
        Me.pnlBodyHost.TabIndex = 1
        '
        'pnlCard
        '
        Me.pnlCard.Controls.Add(Me.pnlFields)
        Me.pnlCard.Controls.Add(Me.pnlActions)
        Me.pnlCard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCard.Location = New System.Drawing.Point(0, 0)
        Me.pnlCard.Name = "pnlCard"
        Me.pnlCard.Size = New System.Drawing.Size(820, 630)
        Me.pnlCard.TabIndex = 0
        '
        'pnlFields
        '
        Me.pnlFields.Controls.Add(Me.lblReturnWindow)
        Me.pnlFields.Controls.Add(Me.nudReturnWindowDays)
        Me.pnlFields.Controls.Add(Me.chkAcceptsReturnRefund)
        Me.pnlFields.Controls.Add(Me.lblAddress)
        Me.pnlFields.Controls.Add(Me.txtAddress)
        Me.pnlFields.Controls.Add(Me.lblContactNumber)
        Me.pnlFields.Controls.Add(Me.txtContactNumber)
        Me.pnlFields.Controls.Add(Me.lblCompany)
        Me.pnlFields.Controls.Add(Me.txtCompany)
        Me.pnlFields.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFields.Location = New System.Drawing.Point(0, 0)
        Me.pnlFields.Name = "pnlFields"
        Me.pnlFields.Size = New System.Drawing.Size(820, 542)
        Me.pnlFields.TabIndex = 0
        '
        'lblReturnWindow
        '
        Me.lblReturnWindow.AutoSize = True
        Me.lblReturnWindow.Location = New System.Drawing.Point(0, 0)
        Me.lblReturnWindow.Name = "lblReturnWindow"
        Me.lblReturnWindow.Size = New System.Drawing.Size(114, 13)
        Me.lblReturnWindow.TabIndex = 8
        Me.lblReturnWindow.Text = "Return Window (Days)"
        '
        'nudReturnWindowDays
        '
        Me.nudReturnWindowDays.BackColor = System.Drawing.Color.Transparent
        Me.nudReturnWindowDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.nudReturnWindowDays.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.nudReturnWindowDays.Location = New System.Drawing.Point(0, 0)
        Me.nudReturnWindowDays.Maximum = New Decimal(New Integer() {365, 0, 0, 0})
        Me.nudReturnWindowDays.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudReturnWindowDays.Name = "nudReturnWindowDays"
        Me.nudReturnWindowDays.Size = New System.Drawing.Size(200, 36)
        Me.nudReturnWindowDays.TabIndex = 7
        Me.nudReturnWindowDays.UpDownButtonFillColor = System.Drawing.Color.FromArgb(CType(CType(62, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.nudReturnWindowDays.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'chkAcceptsReturnRefund
        '
        Me.chkAcceptsReturnRefund.AutoSize = True
        Me.chkAcceptsReturnRefund.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.chkAcceptsReturnRefund.CheckedState.BorderRadius = 0
        Me.chkAcceptsReturnRefund.CheckedState.BorderThickness = 0
        Me.chkAcceptsReturnRefund.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.chkAcceptsReturnRefund.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.chkAcceptsReturnRefund.Location = New System.Drawing.Point(0, 0)
        Me.chkAcceptsReturnRefund.Name = "chkAcceptsReturnRefund"
        Me.chkAcceptsReturnRefund.Size = New System.Drawing.Size(149, 19)
        Me.chkAcceptsReturnRefund.TabIndex = 6
        Me.chkAcceptsReturnRefund.Text = "Accepts Return/Refund"
        Me.chkAcceptsReturnRefund.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.chkAcceptsReturnRefund.UncheckedState.BorderRadius = 0
        Me.chkAcceptsReturnRefund.UncheckedState.BorderThickness = 0
        Me.chkAcceptsReturnRefund.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Location = New System.Drawing.Point(0, 0)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(52, 13)
        Me.lblAddress.TabIndex = 5
        Me.lblAddress.Text = "Address *"
        '
        'txtAddress
        '
        Me.txtAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress.DefaultText = ""
        Me.txtAddress.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAddress.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtAddress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAddress.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtAddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAddress.Location = New System.Drawing.Point(0, 0)
        Me.txtAddress.MaxLength = 250
        Me.txtAddress.Multiline = True
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.PlaceholderText = ""
        Me.txtAddress.SelectedText = ""
        Me.txtAddress.Size = New System.Drawing.Size(200, 36)
        Me.txtAddress.TabIndex = 4
        '
        'lblContactNumber
        '
        Me.lblContactNumber.AutoSize = True
        Me.lblContactNumber.Location = New System.Drawing.Point(0, 0)
        Me.lblContactNumber.Name = "lblContactNumber"
        Me.lblContactNumber.Size = New System.Drawing.Size(91, 13)
        Me.lblContactNumber.TabIndex = 3
        Me.lblContactNumber.Text = "Contact Number *"
        '
        'txtContactNumber
        '
        Me.txtContactNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactNumber.DefaultText = ""
        Me.txtContactNumber.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtContactNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtContactNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtContactNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtContactNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtContactNumber.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtContactNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtContactNumber.Location = New System.Drawing.Point(0, 0)
        Me.txtContactNumber.MaxLength = 11
        Me.txtContactNumber.Name = "txtContactNumber"
        Me.txtContactNumber.PlaceholderText = ""
        Me.txtContactNumber.SelectedText = ""
        Me.txtContactNumber.Size = New System.Drawing.Size(200, 36)
        Me.txtContactNumber.TabIndex = 2
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.Location = New System.Drawing.Point(0, 0)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(83, 13)
        Me.lblCompany.TabIndex = 1
        Me.lblCompany.Text = "Supplier Name *"
        '
        'txtCompany
        '
        Me.txtCompany.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCompany.DefaultText = ""
        Me.txtCompany.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCompany.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCompany.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCompany.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCompany.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCompany.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtCompany.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCompany.Location = New System.Drawing.Point(0, 0)
        Me.txtCompany.MaxLength = 50
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.PlaceholderText = ""
        Me.txtCompany.SelectedText = ""
        Me.txtCompany.Size = New System.Drawing.Size(200, 36)
        Me.txtCompany.TabIndex = 0
        '
        'pnlActions
        '
        Me.pnlActions.Controls.Add(Me.btnCancel)
        Me.pnlActions.Controls.Add(Me.btnAdd)
        Me.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlActions.Location = New System.Drawing.Point(0, 542)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(820, 88)
        Me.pnlActions.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(0, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(180, 45)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnAdd
        '
        Me.btnAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(0, 0)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(180, 45)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Save"
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.btnExit)
        Me.pnlHeader.Controls.Add(Me.lblSubtitle)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(820, 60)
        Me.pnlHeader.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnExit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnExit.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnExit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnExit.ForeColor = System.Drawing.Color.White
        Me.btnExit.Location = New System.Drawing.Point(0, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(180, 45)
        Me.btnExit.TabIndex = 2
        '
        'lblSubtitle
        '
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Location = New System.Drawing.Point(0, 0)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(112, 13)
        Me.lblSubtitle.TabIndex = 1
        Me.lblSubtitle.Text = "Fill in the details below"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(67, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Add Supplier"
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.AnimateWindow = True
        Me.Guna2BorderlessForm1.BorderRadius = 24
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
        'FrmSupplierEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(22, Byte), Integer), CType(CType(25, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(820, 690)
        Me.Controls.Add(Me.pnlMainContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximumSize = New System.Drawing.Size(820, 690)
        Me.MinimumSize = New System.Drawing.Size(820, 690)
        Me.Name = "FrmSupplierEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmSupplierEntry"
        Me.pnlMainContainer.ResumeLayout(False)
        Me.pnlBodyHost.ResumeLayout(False)
        Me.pnlCard.ResumeLayout(False)
        Me.pnlFields.ResumeLayout(False)
        Me.pnlFields.PerformLayout()
        CType(Me.nudReturnWindowDays, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlActions.ResumeLayout(False)
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlMainContainer As Panel
    Friend WithEvents pnlBodyHost As Panel
    Friend WithEvents pnlCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlFields As Panel
    Friend WithEvents lblReturnWindow As Label
    Friend WithEvents nudReturnWindowDays As Guna.UI2.WinForms.Guna2NumericUpDown
    Friend WithEvents chkAcceptsReturnRefund As Guna.UI2.WinForms.Guna2CheckBox
    Friend WithEvents lblAddress As Label
    Friend WithEvents txtAddress As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents lblContactNumber As Label
    Friend WithEvents txtContactNumber As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents lblCompany As Label
    Friend WithEvents txtCompany As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnCancel As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnAdd As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents pnlHeader As Panel
    Friend WithEvents btnExit As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents Guna2BorderlessForm2 As Guna.UI2.WinForms.Guna2BorderlessForm
End Class
