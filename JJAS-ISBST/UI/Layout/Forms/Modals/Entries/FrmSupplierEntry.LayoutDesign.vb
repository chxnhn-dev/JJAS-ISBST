Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms

Partial Class FrmSupplierEntry
    Private _modernUiInitialized As Boolean
    Private _pnlHeaderDivider As Panel
    Private _pnlActionsDivider As Panel

    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        Dim fixedSize As New Size(820, 690)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize
        AutoScaleMode = AutoScaleMode.None
        AutoSize = False
        FormBorderStyle = FormBorderStyle.None
        BackColor = Color.FromArgb(21, 22, 25)
        ForeColor = Color.White

        pnlMainContainer.BackColor = Color.FromArgb(28, 29, 33)
        pnlMainContainer.Dock = DockStyle.Fill
        pnlMainContainer.Padding = New Padding(24)

        pnlHeader.BackColor = pnlMainContainer.BackColor
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 60

        pnlBodyHost.BackColor = pnlMainContainer.BackColor
        pnlBodyHost.Dock = DockStyle.Fill
        pnlBodyHost.Padding = New Padding(0, 20, 0, 0)

        pnlCard.FillColor = Color.FromArgb(34, 36, 42)
        pnlCard.BorderColor = Color.FromArgb(58, 61, 68)
        pnlCard.BorderThickness = 1
        pnlCard.BorderRadius = 14
        pnlCard.Dock = DockStyle.None
        pnlCard.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom
        pnlCard.Padding = New Padding(20)

        pnlFields.BackColor = pnlCard.FillColor
        pnlFields.Dock = DockStyle.Fill
        pnlFields.AutoScroll = False
        pnlFields.Padding = New Padding(0, 0, 0, 56)

        pnlActions.BackColor = pnlCard.FillColor
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Height = 88
        pnlActions.Padding = New Padding(16, 12, 16, 0)

        lblTitle.AutoSize = True
        lblTitle.BackColor = Color.Transparent
        lblTitle.ForeColor = Color.FromArgb(245, 247, 250)
        lblTitle.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
        lblTitle.Location = New Point(0, 0)
        lblTitle.Margin = Padding.Empty

        lblSubtitle.AutoSize = True
        lblSubtitle.BackColor = Color.Transparent
        lblSubtitle.ForeColor = Color.FromArgb(170, 175, 184)
        lblSubtitle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        lblSubtitle.Location = New Point(4, 35)
        lblSubtitle.Margin = Padding.Empty

        StyleHeaderCloseButton(btnExit)
        btnExit.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExit.TabStop = True

        StyleInputLabel(lblCompany)
        StyleInputLabel(lblContactNumber)
        StyleInputLabel(lblAddress)
        StyleInputLabel(lblReturnWindow)

        StyleFieldTextBox(txtCompany, "ABC Supplier")
        txtCompany.MaxLength = 50
        txtCompany.TabIndex = 0

        StyleFieldTextBox(txtContactNumber, "09123456789")
        txtContactNumber.MaxLength = 11
        txtContactNumber.TabIndex = 1

        StyleFieldTextBox(txtAddress, "Supplier address", True)
        txtAddress.MaxLength = 250
        txtAddress.TabIndex = 2

        StyleCheckBox(chkAcceptsReturnRefund)
        chkAcceptsReturnRefund.Text = "Accepts Return/Refund"
        chkAcceptsReturnRefund.TabIndex = 3

        StyleNumericInput(nudReturnWindowDays)
        nudReturnWindowDays.Minimum = 1D
        nudReturnWindowDays.Maximum = 365D
        nudReturnWindowDays.Value = 7D
        nudReturnWindowDays.MinimumSize = New Size(120, 44)
        nudReturnWindowDays.TabIndex = 4

        StylePrimaryButton(btnAdd)
        btnAdd.Text = "Save"
        btnAdd.TabIndex = 0

        StyleSecondaryButton(btnCancel)
        btnCancel.Text = "Cancel"
        btnCancel.TabIndex = 1

        If _pnlHeaderDivider Is Nothing Then
            _pnlHeaderDivider = New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = 1,
                .BackColor = Color.FromArgb(56, 60, 68)
            }
            pnlHeader.Controls.Add(_pnlHeaderDivider)
            _pnlHeaderDivider.SendToBack()
        End If

        If _pnlActionsDivider Is Nothing Then
            _pnlActionsDivider = New Panel With {
                .Dock = DockStyle.Top,
                .Height = 1,
                .BackColor = Color.FromArgb(52, 56, 64)
            }
            pnlActions.Controls.Add(_pnlActionsDivider)
            _pnlActionsDivider.SendToBack()
        End If

        AddHandler pnlHeader.Resize, AddressOf pnlHeader_Resize
        AddHandler pnlBodyHost.ClientSizeChanged, AddressOf pnlBodyHost_ClientSizeChanged
        AddHandler pnlFields.ClientSizeChanged, AddressOf pnlFields_ClientSizeChanged
        AddHandler pnlActions.ClientSizeChanged, AddressOf pnlActions_ClientSizeChanged

        LayoutCardContainer()
        pnlActions.BringToFront()
        pnlHeader_Resize(pnlHeader, EventArgs.Empty)
        LayoutFieldControls()
        LayoutActionButtons()

        _modernUiInitialized = True
        ResumeLayout(True)
    End Sub

    Private Sub pnlHeader_Resize(sender As Object, e As EventArgs)
        If btnExit Is Nothing OrElse btnExit.IsDisposed Then
            Return
        End If

        btnExit.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - 24 - btnExit.Width), 12)
    End Sub

    Private Sub pnlBodyHost_ClientSizeChanged(sender As Object, e As EventArgs)
        LayoutCardContainer()
        LayoutFieldControls()
        LayoutActionButtons()
    End Sub

    Private Sub pnlFields_ClientSizeChanged(sender As Object, e As EventArgs)
        LayoutFieldControls()
    End Sub

    Private Sub pnlActions_ClientSizeChanged(sender As Object, e As EventArgs)
        LayoutActionButtons()
    End Sub

    Private Sub LayoutFieldControls()
        If pnlFields Is Nothing OrElse pnlFields.IsDisposed Then
            Return
        End If

        Dim fieldWidth As Integer = Math.Min(660, GetFieldsContentWidth(pnlFields))
        Dim leftX As Integer = GetFieldsLeft(pnlFields, fieldWidth)
        Dim y As Integer = 8

        y = PositionField(lblCompany, txtCompany, leftX, fieldWidth, y)
        y = PositionField(lblContactNumber, txtContactNumber, leftX, fieldWidth, y)
        y = PositionField(lblAddress, txtAddress, leftX, fieldWidth, y, 84)

        chkAcceptsReturnRefund.Location = New Point(leftX, y)
        chkAcceptsReturnRefund.AutoSize = True
        chkAcceptsReturnRefund.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        y = chkAcceptsReturnRefund.Bottom + 12

        y = PositionField(lblReturnWindow, nudReturnWindowDays, leftX, fieldWidth, y)
        y += 8
        nudReturnWindowDays.BringToFront()

        chkAcceptsReturnRefund.BringToFront()
    End Sub

    Private Sub LayoutCardContainer()
        If pnlBodyHost Is Nothing OrElse pnlBodyHost.IsDisposed Then
            Return
        End If
        If pnlCard Is Nothing OrElse pnlCard.IsDisposed Then
            Return
        End If

        Dim availableWidth As Integer = Math.Max(0, pnlBodyHost.ClientSize.Width - pnlBodyHost.Padding.Left - pnlBodyHost.Padding.Right)
        Dim availableHeight As Integer = Math.Max(0, pnlBodyHost.ClientSize.Height - pnlBodyHost.Padding.Top - pnlBodyHost.Padding.Bottom)

        If availableWidth <= 0 OrElse availableHeight <= 0 Then
            Return
        End If

        Dim cardWidth As Integer = Math.Min(740, availableWidth)
        cardWidth = Math.Max(Math.Min(560, availableWidth), cardWidth)

        pnlCard.Location = New Point(
            pnlBodyHost.Padding.Left + Math.Max(0, (availableWidth - cardWidth) \ 2),
            pnlBodyHost.Padding.Top)
        pnlCard.Size = New Size(cardWidth, availableHeight)
    End Sub

    Private Function PositionField(labelControl As Label,
                                   inputControl As Control,
                                   leftX As Integer,
                                   fieldWidth As Integer,
                                   y As Integer,
                                   Optional inputHeight As Integer = 44) As Integer
        labelControl.Location = New Point(leftX, y)
        labelControl.MaximumSize = New Size(fieldWidth, 0)
        labelControl.AutoSize = True

        inputControl.Location = New Point(leftX, labelControl.Bottom + 6)
        inputControl.Size = New Size(fieldWidth, inputHeight)
        inputControl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Return inputControl.Bottom + 10
    End Function

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then
            Return 560
        End If

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then
            clientWidth = fieldsPanel.Width
        End If

        If clientWidth <= 0 Then
            Return 560
        End If

        Dim width As Integer = clientWidth - fieldsPanel.Padding.Left - fieldsPanel.Padding.Right - 6
        Return Math.Max(240, width)
    End Function

    Private Function GetFieldsLeft(fieldsPanel As Panel, fieldWidth As Integer) As Integer
        Dim availableWidth As Integer = GetFieldsContentWidth(fieldsPanel)
        Return fieldsPanel.Padding.Left + Math.Max(0, (availableWidth - fieldWidth) \ 2)
    End Function

    Private Sub LayoutActionButtons()
        If pnlActions Is Nothing OrElse pnlActions.IsDisposed Then
            Return
        End If

        Dim rightX As Integer = pnlActions.ClientSize.Width - pnlActions.Padding.Right
        Dim spacing As Integer = 12

        btnAdd.Size = New Size(176, 44)
        btnCancel.Size = New Size(100, 40)
        btnAdd.Margin = Padding.Empty
        btnCancel.Margin = Padding.Empty

        btnAdd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnCancel.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        Dim innerTop As Integer = pnlActions.Padding.Top
        Dim innerBottom As Integer = Math.Max(innerTop, pnlActions.ClientSize.Height - pnlActions.Padding.Bottom)
        Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
        Dim primaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnAdd.Height) \ 2)
        Dim secondaryY As Integer = innerTop + Math.Max(0, (innerHeight - btnCancel.Height) \ 2)

        btnAdd.Location = New Point(Math.Max(0, rightX - btnAdd.Width), primaryY)
        btnCancel.Location = New Point(Math.Max(0, btnAdd.Left - spacing - btnCancel.Width), secondaryY)

        btnCancel.BringToFront()
        btnAdd.BringToFront()
    End Sub

    Private Sub StyleInputLabel(lbl As Label)
        If lbl Is Nothing Then
            Return
        End If

        lbl.BackColor = Color.Transparent
        lbl.ForeColor = Color.FromArgb(240, 242, 245)
        lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lbl.Margin = Padding.Empty
        lbl.AutoSize = True
    End Sub

    Private Sub StyleFieldTextBox(tb As Guna2TextBox, placeholder As String, Optional multiline As Boolean = False)
        If tb Is Nothing Then
            Return
        End If

        tb.Animated = False
        tb.AutoRoundedCorners = False
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
        tb.Margin = Padding.Empty
        tb.Cursor = Cursors.IBeam
        tb.BackColor = Color.FromArgb(34, 36, 42)
        tb.Multiline = multiline
        tb.ScrollBars = If(multiline, ScrollBars.Vertical, ScrollBars.None)
        tb.MinimumSize = New Size(120, If(multiline, 96, 44))
        tb.DisabledState.BorderColor = Color.FromArgb(69, 73, 80)
        tb.DisabledState.FillColor = Color.FromArgb(45, 48, 54)
        tb.DisabledState.ForeColor = Color.FromArgb(154, 158, 166)
        tb.DisabledState.PlaceholderForeColor = Color.FromArgb(118, 122, 130)
    End Sub

    Private Sub StyleNumericInput(nud As Guna2NumericUpDown)
        If nud Is Nothing Then
            Return
        End If

        nud.BorderRadius = 12
        nud.BorderThickness = 1
        nud.BorderColor = Color.FromArgb(76, 80, 88)
        nud.FillColor = Color.FromArgb(41, 44, 51)
        nud.ForeColor = Color.FromArgb(238, 241, 245)
        nud.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        nud.BackColor = Color.FromArgb(34, 36, 42)
        nud.UseTransparentBackground = False
        nud.UpDownButtonFillColor = Color.FromArgb(62, 66, 75)
        nud.UpDownButtonForeColor = Color.FromArgb(238, 241, 245)
        nud.DisabledState.BorderColor = Color.FromArgb(69, 73, 80)
        nud.DisabledState.FillColor = Color.FromArgb(45, 48, 54)
        nud.DisabledState.ForeColor = Color.FromArgb(154, 158, 166)
        nud.DisabledState.UpDownButtonFillColor = Color.FromArgb(52, 56, 64)
        nud.DisabledState.UpDownButtonForeColor = Color.FromArgb(130, 134, 142)
        nud.FocusedState.BorderColor = Color.FromArgb(0, 122, 204)
        nud.Margin = Padding.Empty
        nud.Cursor = Cursors.IBeam
        nud.TextOffset = New Point(0, 1)
        nud.ThousandsSeparator = False
    End Sub

    Private Sub StylePrimaryButton(button As Guna2Button)
        If button Is Nothing Then
            Return
        End If

        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
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
        button.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
        button.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        button.Cursor = Cursors.Hand
        button.Image = Nothing
    End Sub

    Private Sub StyleSecondaryButton(button As Guna2Button)
        If button Is Nothing Then
            Return
        End If

        button.Enabled = True
        button.Visible = True
        button.Animated = True
        button.UseTransparentBackground = False
        button.DefaultAutoSize = False
        button.AutoRoundedCorners = False
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
        button.DisabledState.FillColor = Color.FromArgb(58, 62, 70)
        button.DisabledState.ForeColor = Color.FromArgb(170, 174, 181)
        button.Cursor = Cursors.Hand
        button.Image = Nothing
    End Sub

    Private Sub StyleHeaderCloseButton(button As Guna2Button)
        StyleSecondaryButton(button)
        button.Size = New Size(36, 36)
        button.BorderRadius = 10
        button.Text = String.Empty
        button.FillColor = Color.FromArgb(62, 66, 75)
        button.BorderColor = Color.FromArgb(74, 79, 88)
        button.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        button.HoverState.BorderColor = Color.FromArgb(106, 112, 124)

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
        If chk Is Nothing Then
            Return
        End If

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

    Private Sub RefreshSupplierVisuals()
        For Each tb As Guna2TextBox In {txtCompany, txtContactNumber, txtAddress}
            If tb Is Nothing Then
                Continue For
            End If

            tb.ForeColor = Color.FromArgb(238, 241, 245)
            tb.Invalidate()
            tb.Update()
        Next

        If nudReturnWindowDays IsNot Nothing Then
            nudReturnWindowDays.Invalidate()
            nudReturnWindowDays.Update()
        End If

        If pnlFields IsNot Nothing Then
            pnlFields.Invalidate()
            pnlFields.Update()
        End If
    End Sub
End Class
