Imports System.Drawing
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Partial Class FrmBrandEntry
    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        Dim fixedSize As New Size(760, 400)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize
        AutoScaleMode = AutoScaleMode.None
        AutoSize = False
        FormBorderStyle = FormBorderStyle.None
        BackColor = Color.FromArgb(21, 22, 25)
        ForeColor = Color.White

        Dim pnlMainContainer As New Panel With {
            .Name = "pnlMainContainer",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(24),
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        Dim pnlHeader As New Panel With {
            .Name = "pnlHeader",
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
        Label12.ForeColor = Color.FromArgb(245, 247, 250)
        Label12.BackColor = pnlHeader.BackColor
        Label12.Location = New Point(0, 0)
        Label12.Margin = Padding.Empty

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

        btnexit.Visible = False
        btnexit.Size = New Size(1, 1)
        btnexit.Location = New Point(-2000, -2000)

        _btnCloseProxy = New Guna2Button()
        StyleHeaderCloseButton(_btnCloseProxy)
        _btnCloseProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnCloseProxy.Location = New Point(pnlHeader.Width - 60, 12)
        AddHandler _btnCloseProxy.Click, Sub() btnexit_Click(btnexit, EventArgs.Empty)

        AddHandler pnlHeader.Resize,
            Sub()
                If _btnCloseProxy Is Nothing OrElse _btnCloseProxy.IsDisposed Then Return
                _btnCloseProxy.Location = New Point(pnlHeader.ClientSize.Width - 24 - _btnCloseProxy.Width, 12)
            End Sub

        pnlHeader.Controls.Add(_btnCloseProxy)
        pnlHeader.Controls.Add(_lblSubtitle)
        pnlHeader.Controls.Add(Label12)
        pnlHeader.Controls.Add(pnlHeaderDivider)

        Dim pnlBodyHost As New Panel With {
            .Name = "pnlBodyHost",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 20, 0, 0),
            .BackColor = Color.FromArgb(28, 29, 33)
        }

        Dim pnlCard As Guna2Panel = CreateCardPanel()
        pnlCard.Dock = DockStyle.Fill
        pnlCard.Padding = New Padding(20)

        Panel1.BackColor = Color.FromArgb(34, 36, 42)
        Panel1.Dock = DockStyle.Fill
        Panel1.Padding = Padding.Empty
        Panel1.Margin = Padding.Empty
        For Each ctl As Control In New Control() {btnedit, EentryName, btnadd, txtValue}
            If ctl IsNot Nothing AndAlso ctl.Parent IsNot Nothing Then
                ctl.Parent.Controls.Remove(ctl)
            End If
        Next

        _pnlActions = New Panel With {
            .Name = "pnlActions",
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
            .Name = "pnlFields",
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(0, 0, 0, 30),
            .BackColor = Color.FromArgb(34, 36, 42)
        }

        StyleInputLabel(EentryName)
        StyleFieldTextBox(txtValue)
        txtValue.MaxLength = 50

        StylePrimaryButton(btnadd)
        StylePrimaryButton(btnedit)
        btnadd.Margin = Padding.Empty
        btnedit.Margin = Padding.Empty

        _btnCancelProxy = New Guna2Button()
        StyleSecondaryButton(_btnCancelProxy)
        _btnCancelProxy.Text = "Cancel"
        _btnCancelProxy.Margin = Padding.Empty
        AddHandler _btnCancelProxy.Click, Sub() btnexit_Click(btnexit, EventArgs.Empty)

        _btnClearProxy = New Guna2Button()
        StyleSecondaryButton(_btnClearProxy)
        _btnClearProxy.Text = "Clear"
        _btnClearProxy.Margin = Padding.Empty
        AddHandler _btnClearProxy.Click, AddressOf btnClearProxy_Click

        _pnlFields.Controls.Add(txtValue)
        _pnlFields.Controls.Add(EentryName)

        _pnlActions.Controls.Add(_btnCancelProxy)
        _pnlActions.Controls.Add(_btnClearProxy)
        _pnlActions.Controls.Add(btnadd)
        _pnlActions.Controls.Add(btnedit)
        _pnlActions.Controls.Add(pnlActionsDivider)

        Panel1.Controls.Add(_pnlFields)
        Panel1.Controls.Add(_pnlActions)

        pnlCard.Controls.Add(Panel1)
        pnlBodyHost.Controls.Add(pnlCard)

        pnlMainContainer.Controls.Add(pnlBodyHost)
        pnlMainContainer.Controls.Add(pnlHeader)
        Controls.Add(pnlMainContainer)

        AddHandler _pnlFields.ClientSizeChanged, Sub() LayoutFieldControls()
        AddHandler _pnlActions.ClientSizeChanged, Sub() LayoutActionButtons()

        _modernUiInitialized = True
        ResumeLayout(True)
    End Sub

    Private Sub btnClearProxy_Click(sender As Object, e As EventArgs)
        txtValue.Clear()
        txtValue.Focus()
        RefreshSharedEntryVisuals()
    End Sub

    Private Sub LayoutFieldControls()
        If _pnlFields Is Nothing OrElse _pnlFields.IsDisposed Then
            Return
        End If

        Dim contentWidth As Integer = GetFieldsContentWidth(_pnlFields)
        Dim y As Integer = 16

        EentryName.Location = New Point(0, y)
        EentryName.MaximumSize = New Size(contentWidth, 0)

        Dim labelHeight As Integer = Math.Max(24, EentryName.Height)
        txtValue.Location = New Point(0, EentryName.Top + labelHeight + 6)
        txtValue.Size = New Size(contentWidth, 44)
        txtValue.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        _pnlFields.AutoScrollMinSize = New Size(0, txtValue.Bottom + 14 + _pnlFields.Padding.Bottom)
    End Sub

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then
            Return 620
        End If

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then clientWidth = fieldsPanel.Width
        If clientWidth <= 0 Then Return 620

        Dim scrollbarAllowance As Integer = If(fieldsPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim width As Integer = clientWidth - fieldsPanel.Padding.Left - fieldsPanel.Padding.Right - scrollbarAllowance - 6
        Return Math.Max(240, width)
    End Function

    Private Sub LayoutActionButtons()
        If _pnlActions Is Nothing OrElse _pnlActions.IsDisposed Then
            Return
        End If

        Dim primaryButton As Guna2Button = If(btnedit.Visible, btnedit, btnadd)
        Dim hiddenPrimary As Guna2Button = If(btnedit.Visible, btnadd, btnedit)
        Dim rightX As Integer = _pnlActions.ClientSize.Width - _pnlActions.Padding.Right
        Dim spacing As Integer = 12

        primaryButton.Size = New Size(176, 44)
        _btnClearProxy.Size = New Size(100, 40)
        _btnCancelProxy.Size = New Size(100, 40)

        primaryButton.Margin = Padding.Empty
        _btnClearProxy.Margin = Padding.Empty
        _btnCancelProxy.Margin = Padding.Empty

        primaryButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnClearProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnCancelProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        If hiddenPrimary IsNot Nothing Then
            hiddenPrimary.Location = New Point(-2000, -2000)
        End If

        Dim innerTop As Integer = _pnlActions.Padding.Top
        Dim innerBottom As Integer = Math.Max(innerTop, _pnlActions.ClientSize.Height - _pnlActions.Padding.Bottom)
        Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
        Dim primaryY As Integer = innerTop + Math.Max(0, (innerHeight - primaryButton.Height) \ 2)
        Dim secondaryY As Integer = innerTop + Math.Max(0, (innerHeight - _btnClearProxy.Height) \ 2)

        primaryButton.Location = New Point(Math.Max(0, rightX - primaryButton.Width), primaryY)
        _btnClearProxy.Location = New Point(Math.Max(0, primaryButton.Left - spacing - _btnClearProxy.Width), secondaryY)
        _btnCancelProxy.Location = New Point(Math.Max(0, _btnClearProxy.Left - spacing - _btnCancelProxy.Width), secondaryY)

        _btnCancelProxy.BringToFront()
        _btnClearProxy.BringToFront()
        primaryButton.BringToFront()
    End Sub

    Private Sub RefreshSharedEntryVisuals()
        If txtValue IsNot Nothing Then
            txtValue.ForeColor = Color.FromArgb(238, 241, 245)
            txtValue.Invalidate()
            txtValue.Update()
        End If

        If _pnlFields IsNot Nothing Then
            _pnlFields.Invalidate()
            _pnlFields.Update()
        End If
    End Sub

    Private Sub ForceImmediateFieldPaint()
        RefreshSharedEntryVisuals()

        txtValue.SelectionStart = txtValue.TextLength
        txtValue.SelectionLength = 0
        txtValue.Invalidate()
        txtValue.Update()

        Me.ActiveControl = Nothing
        Me.Invalidate(True)
        Me.Update()

        If Me.Visible AndAlso Me.IsHandleCreated Then
            Application.DoEvents()
        End If
    End Sub

    Private Function CreateCardPanel() As Guna2Panel
        Return New Guna2Panel With {
            .FillColor = Color.FromArgb(34, 36, 42),
            .BorderColor = Color.FromArgb(58, 61, 68),
            .BorderThickness = 1,
            .BorderRadius = 14,
            .Margin = Padding.Empty
        }
    End Function

    Private Sub StyleInputLabel(lbl As Guna2HtmlLabel)
        If lbl Is Nothing Then Return

        lbl.BackColor = Color.Transparent
        lbl.ForeColor = Color.FromArgb(240, 242, 245)
        lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lbl.Margin = Padding.Empty
    End Sub

    Private Sub StyleFieldTextBox(tb As Guna2TextBox)
        If tb Is Nothing Then Return

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
        tb.Margin = Padding.Empty
        tb.Cursor = Cursors.IBeam
        tb.BackColor = Color.FromArgb(34, 36, 42)
    End Sub

    Private Sub StylePrimaryButton(button As Guna2Button)
        If button Is Nothing Then Return

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
        button.HoverState.CustomBorderColor = button.CustomBorderColor
        button.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
        button.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        button.Cursor = Cursors.Hand
        button.Image = Nothing
        button.ImageSize = Size.Empty
    End Sub

    Private Sub StyleSecondaryButton(button As Guna2Button)
        If button Is Nothing Then Return

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
End Class
