Imports System.Drawing
Imports System.Net.Mail
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Partial Class FrmUserEntry
    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        BackColor = Color.FromArgb(21, 22, 25)
        ForeColor = Color.White

        pnlMainContainer.BackColor = Color.FromArgb(28, 29, 33)
        pnlMainContainer.Dock = DockStyle.Fill
        pnlMainContainer.Padding = New Padding(24)

        Dim pnlHeader As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(245, 247, 250)
        lblTitle.BackColor = pnlHeader.BackColor
        lblTitle.Location = New Point(0, 0)
        lblTitle.Margin = Padding.Empty
        lblTitle.Text = "Add User"

        _lblSubtitle = New Label With {
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Regular),
            .ForeColor = Color.FromArgb(170, 175, 184),
            .BackColor = pnlHeader.BackColor,
            .Location = New Point(4, 35),
            .Text = "Fill in the details below"
        }

        Dim pnlHeaderDivider As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 1,
            .BackColor = Color.FromArgb(56, 60, 68)
        }

        StyleHeaderCloseButton(btnExit)
        btnExit.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExit.TabIndex = 13
        btnExit.Location = New Point(pnlHeader.Width - 60, 12)

        AddHandler pnlHeader.Resize,
            Sub()
                btnExit.Location = New Point(pnlHeader.ClientSize.Width - 24 - btnExit.Width, 12)
            End Sub

        pnlHeader.Controls.Add(btnExit)
        pnlHeader.Controls.Add(_lblSubtitle)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(pnlHeaderDivider)

        Dim pnlBodyHost As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 20, 0, 0),
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        Dim pnlCard As Guna2Panel = CreateCardPanel()
        pnlCard.Dock = DockStyle.Fill
        pnlCard.Padding = New Padding(20)
        BuildUserCard(pnlCard)

        pnlBodyHost.Controls.Add(pnlCard)
        pnlMainContainer.Controls.Add(pnlBodyHost)
        pnlMainContainer.Controls.Add(pnlHeader)
        Controls.Add(pnlMainContainer)
        pnlMainContainer.BringToFront()

        _modernUiInitialized = True
        ResumeLayout(True)
    End Sub

    Private Function CreateCardPanel() As Guna2Panel
        Dim pnl As New Guna2Panel()
        pnl.FillColor = Color.FromArgb(34, 36, 42)
        pnl.BorderColor = Color.FromArgb(58, 61, 68)
        pnl.BorderThickness = 1
        pnl.BorderRadius = 14
        pnl.Margin = Padding.Empty
        Return pnl
    End Function

    Private Sub BuildUserCard(parentPanel As Guna2Panel)
        StylePrimaryButton(btnAdd)
        btnAdd.Text = "Save"
        btnAdd.Size = New Size(176, 44)
        btnAdd.Margin = Padding.Empty

        StyleSecondaryButton(btnClear)
        btnClear.Text = "Clear"
        btnClear.Size = New Size(100, 40)
        btnClear.Margin = Padding.Empty

        StyleSecondaryButton(btnCancel)
        btnCancel.Text = "Cancel"
        btnCancel.Size = New Size(100, 40)
        btnCancel.Margin = Padding.Empty

        _pnlActions = New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 88,
            .BackColor = Color.FromArgb(34, 36, 42),
            .Padding = New Padding(16, 12, 16, 0)
        }

        Dim pnlActionsDivider As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 1,
            .BackColor = Color.FromArgb(52, 56, 64)
        }

        _pnlFields = New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(0, 0, 0, 30),
            .BackColor = Color.FromArgb(34, 36, 42)
        }

        StyleComboBox(cbrole)
        cbrole.TabIndex = 0

        StyleTextBox(txtfirstname, "Enter first name")
        txtfirstname.MaxLength = 50
        txtfirstname.TabIndex = 1

        StyleTextBox(txtlastname, "Enter last name")
        txtlastname.MaxLength = 50
        txtlastname.TabIndex = 2

        StyleTextBox(txtcontactnumber, "09XXXXXXXXX")
        txtcontactnumber.MaxLength = 11
        txtcontactnumber.TabIndex = 3

        StyleTextBox(txtemail, "name@example.com")
        txtemail.MaxLength = 50
        txtemail.TabIndex = 4

        StyleTextBox(txtaddress, "Enter address", True)
        txtaddress.MaxLength = 200
        txtaddress.Multiline = True
        txtaddress.TabIndex = 5

        StyleTextBox(txtusername, "Enter username")
        txtusername.MaxLength = 50
        txtusername.TabIndex = 6

        StyleTextBox(txtpassword, "Enter password")
        txtpassword.MaxLength = 50
        txtpassword.PasswordChar = "*"c
        txtpassword.TabIndex = 7

        StyleTextBox(txtConfirmPass, "Confirm password")
        txtConfirmPass.MaxLength = 50
        txtConfirmPass.PasswordChar = "*"c
        txtConfirmPass.TabIndex = 8

        StyleCheckBox(cbxShowpassword)
        cbxShowpassword.Text = "Show password"
        cbxShowpassword.TabIndex = 9

        parentPanel.Controls.Add(_pnlFields)
        parentPanel.Controls.Add(_pnlActions)

        _pnlActions.Controls.Add(btnCancel)
        _pnlActions.Controls.Add(btnClear)
        _pnlActions.Controls.Add(btnAdd)
        _pnlActions.Controls.Add(pnlActionsDivider)

        Dim y As Integer = 0
        y = AddField(_pnlFields, "Role *", cbrole, y)
        y = AddField(_pnlFields, "First Name *", txtfirstname, y)
        y = AddField(_pnlFields, "Last Name *", txtlastname, y)
        y = AddField(_pnlFields, "Contact Number *", txtcontactnumber, y)
        y = AddField(_pnlFields, "Email *", txtemail, y)
        y = AddField(_pnlFields, "Address", txtaddress, y, 96)
        y = AddField(_pnlFields, "Username *", txtusername, y)
        y = AddField(_pnlFields, "Password *", txtpassword, y)
        y = AddField(_pnlFields, "Confirm Password *", txtConfirmPass, y)

        cbxShowpassword.AutoSize = True
        cbxShowpassword.Location = New Point(0, y)
        cbxShowpassword.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        _pnlFields.Controls.Add(cbxShowpassword)
        y = cbxShowpassword.Bottom + 14

        _pnlFields.AutoScrollMinSize = New Size(0, y + _pnlFields.Padding.Bottom)

        AddHandler _pnlFields.ClientSizeChanged, AddressOf pnlFields_ClientSizeChanged
        AddHandler _pnlActions.ClientSizeChanged, Sub() LayoutActionButtons()

        pnlFields_ClientSizeChanged(_pnlFields, EventArgs.Empty)
        LayoutActionButtons()
    End Sub

    Private Function AddField(container As Panel,
                              labelText As String,
                              inputControl As Control,
                              y As Integer,
                              Optional inputHeight As Integer = 44) As Integer
        Dim labelControl As New Label With {
            .AutoSize = True,
            .Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(240, 242, 245),
            .BackColor = Color.FromArgb(34, 36, 42),
            .Location = New Point(0, y),
            .Text = labelText
        }

        container.Controls.Add(labelControl)

        inputControl.Location = New Point(0, labelControl.Bottom + 6)
        inputControl.Size = New Size(GetFieldsContentWidth(container), inputHeight)
        inputControl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        inputControl.Tag = "FieldInput"
        container.Controls.Add(inputControl)

        Return inputControl.Bottom + 14
    End Function

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then
            Return 620
        End If

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then
            clientWidth = fieldsPanel.Width
        End If

        If clientWidth <= 0 Then
            Return 620
        End If

        Dim scrollbarAllowance As Integer = If(fieldsPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim width As Integer = clientWidth - fieldsPanel.Padding.Left - fieldsPanel.Padding.Right - scrollbarAllowance - 6
        Return Math.Max(240, width)
    End Function

    Private Sub pnlFields_ClientSizeChanged(sender As Object, e As EventArgs)
        Dim fieldsPanel As Panel = TryCast(sender, Panel)
        If fieldsPanel Is Nothing Then
            Return
        End If

        Dim targetWidth As Integer = GetFieldsContentWidth(fieldsPanel)
        Dim maxBottom As Integer = 0

        For Each ctrl As Control In fieldsPanel.Controls
            If TypeOf ctrl Is Guna2TextBox OrElse TypeOf ctrl Is Guna2ComboBox Then
                ctrl.Width = targetWidth
            End If
            maxBottom = Math.Max(maxBottom, ctrl.Bottom)
        Next

        fieldsPanel.AutoScrollMinSize = New Size(0, maxBottom + fieldsPanel.Padding.Bottom)
    End Sub

    Private Sub LayoutActionButtons()
        If _pnlActions Is Nothing OrElse _pnlActions.IsDisposed Then
            Return
        End If

        Dim rightX As Integer = _pnlActions.ClientSize.Width - _pnlActions.Padding.Right
        Dim spacing As Integer = 12

        btnAdd.Size = New Size(176, 44)
        btnClear.Size = New Size(100, 40)
        btnCancel.Size = New Size(100, 40)
        btnAdd.Margin = Padding.Empty
        btnClear.Margin = Padding.Empty
        btnCancel.Margin = Padding.Empty

        btnAdd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClear.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnCancel.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        Dim innerTop As Integer = _pnlActions.Padding.Top
        Dim innerBottom As Integer = Math.Max(innerTop, _pnlActions.ClientSize.Height - _pnlActions.Padding.Bottom)
        Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
        Dim primaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnAdd.Height) \ 2)
        Dim secondaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnClear.Height) \ 2)

        btnAdd.Location = New Point(Math.Max(0, rightX - btnAdd.Width), primaryY)
        btnClear.Location = New Point(Math.Max(0, btnAdd.Left - spacing - btnClear.Width), secondaryY)
        btnCancel.Location = New Point(Math.Max(0, btnClear.Left - spacing - btnCancel.Width), secondaryY)

        btnCancel.BringToFront()
        btnClear.BringToFront()
        btnAdd.BringToFront()
    End Sub

    Private Sub RefreshModernInputs()
        For Each tb As Guna2TextBox In {txtfirstname, txtlastname, txtcontactnumber, txtemail, txtaddress, txtusername, txtpassword, txtConfirmPass}
            If tb Is Nothing Then Continue For
            tb.ForeColor = Color.FromArgb(238, 241, 245)
            tb.Invalidate()
            tb.Update()
        Next

        If cbrole IsNot Nothing Then
            cbrole.Invalidate()
            cbrole.Update()
        End If
    End Sub

    Private Sub StyleTextBox(tb As Guna2TextBox, placeholder As String, Optional multiline As Boolean = False)
        tb.Animated = False
        tb.BorderRadius = 12
        tb.BorderThickness = 1
        tb.BorderColor = Color.FromArgb(76, 80, 88)
        tb.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        tb.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
        tb.FillColor = Color.FromArgb(41, 44, 51)
        tb.ForeColor = Color.FromArgb(238, 241, 245)
        tb.PlaceholderForeColor = Color.FromArgb(150, 154, 164)
        tb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        tb.PlaceholderText = placeholder
        tb.Multiline = multiline
        tb.ScrollBars = If(multiline, ScrollBars.Vertical, ScrollBars.None)
        tb.Margin = Padding.Empty
        tb.Cursor = Cursors.IBeam
        tb.BackColor = Color.FromArgb(34, 36, 42)
    End Sub

    Private Sub StyleComboBox(cbo As Guna2ComboBox)
        cbo.BackColor = Color.Transparent
        cbo.BorderRadius = 12
        cbo.BorderThickness = 1
        cbo.BorderColor = Color.FromArgb(76, 80, 88)
        cbo.FocusedColor = Color.FromArgb(0, 122, 204)
        cbo.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        cbo.HoverState.BorderColor = Color.FromArgb(108, 114, 124)
        cbo.FillColor = Color.FromArgb(41, 44, 51)
        cbo.ForeColor = Color.FromArgb(238, 241, 245)
        cbo.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        cbo.DrawMode = DrawMode.OwnerDrawFixed
        cbo.DropDownStyle = ComboBoxStyle.DropDownList
        cbo.IntegralHeight = False
        cbo.ItemHeight = 38
        cbo.Margin = Padding.Empty
        cbo.BackColor = Color.Transparent
    End Sub

    Private Sub StylePrimaryButton(button As Guna2Button)
        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.BorderRadius = 12
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(0, 122, 204)
        button.FillColor = Color.FromArgb(0, 122, 204)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 10.5F, FontStyle.Bold)
        button.HoverState.FillColor = Color.FromArgb(20, 141, 224)
        button.HoverState.ForeColor = Color.White
        button.HoverState.BorderColor = Color.FromArgb(56, 171, 250)
        button.HoverState.CustomBorderColor = button.CustomBorderColor
        button.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
        button.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        button.Cursor = Cursors.Hand

        button.Image = Nothing
        button.ImageSize = Size.Empty
    End Sub

    Private Sub StyleSecondaryButton(button As Guna2Button)
        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.BorderRadius = 10
        button.BorderThickness = 1
        button.BorderColor = Color.FromArgb(74, 79, 88)
        button.FillColor = Color.FromArgb(62, 66, 75)
        button.BackColor = Color.FromArgb(34, 36, 42)
        button.ForeColor = Color.FromArgb(242, 244, 247)
        button.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        button.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        button.HoverState.ForeColor = Color.White
        button.HoverState.BorderColor = Color.FromArgb(106, 112, 124)
        button.HoverState.CustomBorderColor = button.CustomBorderColor
        button.DisabledState.FillColor = Color.FromArgb(58, 62, 70)
        button.DisabledState.ForeColor = Color.FromArgb(170, 174, 181)
        button.Cursor = Cursors.Hand
    End Sub

    Private Sub StyleHeaderCloseButton(button As Guna2Button)
        StyleSecondaryButton(button)
        button.Size = New Size(36, 36)
        button.BorderRadius = 10
        button.Text = String.Empty

        Dim rm As New System.ComponentModel.ComponentResourceManager(GetType(FrmUserEntry))
        Dim closeImage As Image = TryCast(rm.GetObject("btnExit.Image"), Image)
        button.Image = closeImage
        button.ImageSize = New Size(12, 12)

        If closeImage Is Nothing Then
            button.Text = "X"
            button.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        End If
    End Sub

    Private Sub StyleCheckBox(chk As Guna2CheckBox)
        chk.Animated = True
        chk.AutoSize = True
        chk.BackColor = Color.FromArgb(34, 36, 42)
        chk.ForeColor = Color.FromArgb(204, 208, 216)
        chk.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
        chk.CheckMarkColor = Color.White
        chk.CheckedState.BorderColor = Color.FromArgb(0, 122, 204)
        chk.CheckedState.BorderRadius = 4
        chk.CheckedState.BorderThickness = 1
        chk.CheckedState.FillColor = Color.FromArgb(0, 122, 204)
        chk.UncheckedState.BorderColor = Color.FromArgb(76, 80, 88)
        chk.UncheckedState.BorderRadius = 4
        chk.UncheckedState.BorderThickness = 1
        chk.UncheckedState.FillColor = Color.FromArgb(41, 44, 51)
        chk.Cursor = Cursors.Hand
    End Sub
End Class
