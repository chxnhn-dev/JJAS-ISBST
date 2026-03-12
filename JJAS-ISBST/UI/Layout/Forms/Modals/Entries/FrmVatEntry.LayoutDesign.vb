Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms
Imports Guna.UI2.WinForms

Partial Class FrmVATEntry
    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        Dim fixedSize As New Size(820, 520)
        ClientSize = fixedSize
        MinimumSize = fixedSize
        MaximumSize = fixedSize
        AutoScaleMode = AutoScaleMode.None
        AutoSize = False
        FormBorderStyle = FormBorderStyle.None
        BackColor = Color.FromArgb(21, 22, 25)
        ForeColor = Color.White
        AutoScroll = False

        Dim pnlMainContainer As New Panel With {
            .Name = "pnlMainContainer",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(24),
            .BackColor = Color.FromArgb(28, 29, 33),
            .AutoScroll = False
        }

        Dim pnlHeader As New Panel With {
            .Name = "pnlHeader",
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(28, 29, 33),
            .AutoScroll = False
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

        btnExit.Visible = False
        btnExit.Size = New Size(1, 1)
        btnExit.Location = New Point(-2000, -2000)

        _btnCloseProxy = New Guna2Button()
        StyleHeaderCloseButton(_btnCloseProxy)
        _btnCloseProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnCloseProxy.Location = New Point(pnlHeader.Width - 24 - _btnCloseProxy.Width, 12)
        AddHandler _btnCloseProxy.Click, Sub() btnExit_Click(btnExit, EventArgs.Empty)
        AddHandler pnlHeader.Resize,
            Sub()
                If _btnCloseProxy Is Nothing OrElse _btnCloseProxy.IsDisposed Then Return
                _btnCloseProxy.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - 24 - _btnCloseProxy.Width), 12)
            End Sub

        pnlHeader.Controls.Add(_btnCloseProxy)
        pnlHeader.Controls.Add(_lblSubtitle)
        pnlHeader.Controls.Add(Label12)
        pnlHeader.Controls.Add(pnlHeaderDivider)

        Dim pnlBodyHost As New Panel With {
            .Name = "pnlBodyHost",
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 20, 0, 0),
            .BackColor = Color.FromArgb(28, 29, 33),
            .AutoScroll = False
        }

        Dim pnlCard As Guna2Panel = CreateCardPanel()
        pnlCard.Dock = DockStyle.Fill
        pnlCard.Padding = New Padding(20)
        pnlCard.AutoScroll = False

        Panel1.BackColor = Color.FromArgb(34, 36, 42)
        Panel1.Dock = DockStyle.Fill
        Panel1.Padding = Padding.Empty
        Panel1.Margin = Padding.Empty
        Panel1.AutoScroll = False

        For Each ctl As Control In New Control() {btnUpdate, s, txtVatrate, btnExit}
            If ctl IsNot Nothing AndAlso ctl.Parent IsNot Nothing Then
                ctl.Parent.Controls.Remove(ctl)
            End If
        Next

        _pnlActions = New Panel With {
            .Name = "pnlActions",
            .Dock = DockStyle.Bottom,
            .Height = 88,
            .BackColor = Color.FromArgb(34, 36, 42),
            .Padding = New Padding(16, 12, 16, 0),
            .AutoScroll = False
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

        Dim pnlLegacyHost As New Panel With {
            .Name = "pnlLegacyHost",
            .Visible = False,
            .Size = New Size(1, 1),
            .Location = New Point(-5000, -5000),
            .AutoScroll = False
        }

        StyleInputLabel(s)
        s.Text = "Vat Rate:"

        HideLegacyControl(txtVatrate)
        HideLegacyControl(btnUpdate)
        HideLegacyControl(btnExit)

        _txtVatRateProxy = New Guna2TextBox With {.Name = "txtVatRateProxy"}
        StyleFieldTextBox(_txtVatRateProxy, False)
        _txtVatRateProxy.PlaceholderText = "12"
        _txtVatRateProxy.TextAlign = HorizontalAlignment.Right
        _txtVatRateProxy.MaxLength = txtVatrate.MaxLength
        AddHandler _txtVatRateProxy.KeyPress, AddressOf txtVatRateProxy_KeyPress

        BindTextProxy(_txtVatRateProxy, txtVatrate)

        _btnPrimaryProxy = New Guna2Button With {.Name = "btnPrimaryProxy"}
        StylePrimaryButton(_btnPrimaryProxy)
        AddHandler _btnPrimaryProxy.Click, Sub() btnUpdate_Click(btnUpdate, EventArgs.Empty)

        _btnCancelProxy = New Guna2Button With {.Name = "btnCancelProxy", .Text = "Cancel"}
        StyleSecondaryButton(_btnCancelProxy)
        AddHandler _btnCancelProxy.Click, Sub() btnExit_Click(btnExit, EventArgs.Empty)

        _btnClearProxy = New Guna2Button With {.Name = "btnClearProxy", .Text = "Clear"}
        StyleSecondaryButton(_btnClearProxy)
        AddHandler _btnClearProxy.Click, AddressOf btnClearProxy_Click

        AddHandler btnUpdate.TextChanged, Sub() SyncPrimaryButtonFromLegacy()
        AddHandler btnUpdate.EnabledChanged, Sub() SyncPrimaryButtonFromLegacy()

        _pnlFields.Controls.Add(_txtVatRateProxy)
        _pnlFields.Controls.Add(s)

        pnlLegacyHost.Controls.Add(txtVatrate)
        pnlLegacyHost.Controls.Add(btnUpdate)
        pnlLegacyHost.Controls.Add(btnExit)

        _pnlActions.Controls.Add(_btnCancelProxy)
        _pnlActions.Controls.Add(_btnClearProxy)
        _pnlActions.Controls.Add(_btnPrimaryProxy)
        _pnlActions.Controls.Add(pnlActionsDivider)

        Panel1.Controls.Add(_pnlFields)
        Panel1.Controls.Add(_pnlActions)
        Panel1.Controls.Add(pnlLegacyHost)

        pnlCard.Controls.Add(Panel1)
        pnlBodyHost.Controls.Add(pnlCard)

        pnlMainContainer.Controls.Add(pnlBodyHost)
        pnlMainContainer.Controls.Add(pnlHeader)
        Controls.Add(pnlMainContainer)

        AddHandler _pnlFields.ClientSizeChanged, Sub() LayoutFieldControls()
        AddHandler _pnlActions.ClientSizeChanged, Sub() LayoutActionButtons()

        SyncPrimaryButtonFromLegacy()
        SyncLegacyToModernFields()

        _modernUiInitialized = True
        ResumeLayout(True)
    End Sub

    Private Sub HideLegacyControl(ctrl As Control)
        If ctrl Is Nothing Then Return
        ctrl.Dock = DockStyle.None
        ctrl.Visible = False
        ctrl.TabStop = False
        ctrl.Location = New Point(-2000, -2000)
        ctrl.Size = New Size(1, 1)
        ctrl.Margin = Padding.Empty
    End Sub

    Private Sub BindTextProxy(proxy As Guna2TextBox, legacy As TextBoxBase)
        If proxy Is Nothing OrElse legacy Is Nothing Then Return

        proxy.Text = legacy.Text
        proxy.MaxLength = legacy.MaxLength

        AddHandler proxy.TextChanged,
            Sub()
                If _isSyncingFields OrElse legacy.IsDisposed Then Return
                Try
                    _isSyncingFields = True
                    If legacy.Text <> proxy.Text Then
                        legacy.Text = proxy.Text
                    End If
                Finally
                    _isSyncingFields = False
                End Try
            End Sub

        AddHandler legacy.TextChanged,
            Sub()
                If _isSyncingFields OrElse proxy.IsDisposed Then Return
                Try
                    _isSyncingFields = True
                    If proxy.Text <> legacy.Text Then
                        proxy.Text = legacy.Text
                    End If
                Finally
                    _isSyncingFields = False
                End Try
            End Sub
    End Sub

    Private Sub SyncLegacyToModernFields()
        If _txtVatRateProxy Is Nothing Then Return

        Try
            _isSyncingFields = True
            _txtVatRateProxy.Text = txtVatrate.Text
        Finally
            _isSyncingFields = False
        End Try
    End Sub

    Private Sub SyncModernToLegacyFields()
        If _txtVatRateProxy Is Nothing Then Return

        Try
            _isSyncingFields = True
            txtVatrate.Text = _txtVatRateProxy.Text
        Finally
            _isSyncingFields = False
        End Try
    End Sub

    Private Sub SyncPrimaryButtonFromLegacy()
        If _btnPrimaryProxy Is Nothing Then Return

        Dim normalized As String = If(btnUpdate.Text, String.Empty).Trim()
        If String.IsNullOrWhiteSpace(normalized) Then
            normalized = "Update"
        End If

        _btnPrimaryProxy.Text = normalized
        _btnPrimaryProxy.Enabled = btnUpdate.Enabled
        _btnPrimaryProxy.Visible = True
    End Sub

    Private Sub btnClearProxy_Click(sender As Object, e As EventArgs)
        If _txtVatRateProxy IsNot Nothing Then _txtVatRateProxy.Clear()
        SyncModernToLegacyFields()
        FocusVatRateField()
        RefreshVatVisuals()
    End Sub

    Private Sub LayoutFieldControls()
        If _pnlFields Is Nothing OrElse _pnlFields.IsDisposed Then Return
        If _txtVatRateProxy Is Nothing Then Return

        Dim contentWidth As Integer = GetFieldsContentWidth(_pnlFields)
        Dim y As Integer = 16

        s.Location = New Point(0, y)
        s.MaximumSize = New Size(contentWidth, 0)
        s.AutoSize = True
        Dim labelHeight As Integer = Math.Max(24, s.Height)

        _txtVatRateProxy.Location = New Point(0, s.Top + labelHeight + 6)
        _txtVatRateProxy.Size = New Size(contentWidth, 44)
        _txtVatRateProxy.MinimumSize = New Size(120, 44)
        _txtVatRateProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        y = _txtVatRateProxy.Bottom + 14

        _pnlFields.AutoScrollMinSize = New Size(0, y + _pnlFields.Padding.Bottom)
    End Sub

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then Return 640

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then clientWidth = fieldsPanel.Width
        If clientWidth <= 0 Then Return 640

        Dim scrollbarAllowance As Integer = If(fieldsPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim width As Integer = clientWidth - fieldsPanel.Padding.Left - fieldsPanel.Padding.Right - scrollbarAllowance - 6
        Return Math.Max(240, width)
    End Function

    Private Sub LayoutActionButtons()
        If _pnlActions Is Nothing OrElse _pnlActions.IsDisposed Then Return
        If _btnPrimaryProxy Is Nothing OrElse _btnClearProxy Is Nothing OrElse _btnCancelProxy Is Nothing Then Return

        Dim rightX As Integer = _pnlActions.ClientSize.Width - _pnlActions.Padding.Right
        Dim spacing As Integer = 10

        _btnPrimaryProxy.Size = New Size(176, 48)
        _btnClearProxy.Size = New Size(100, 44)
        _btnCancelProxy.Size = New Size(100, 44)

        _btnPrimaryProxy.Margin = Padding.Empty
        _btnClearProxy.Margin = Padding.Empty
        _btnCancelProxy.Margin = Padding.Empty

        _btnPrimaryProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnClearProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnCancelProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        Dim innerTop As Integer = _pnlActions.Padding.Top
        Dim innerBottom As Integer = Math.Max(innerTop, _pnlActions.ClientSize.Height - _pnlActions.Padding.Bottom)
        Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
        Dim primaryY As Integer = innerTop + Math.Max(0, (innerHeight - _btnPrimaryProxy.Height) \ 2)
        Dim secondaryY As Integer = innerTop + Math.Max(0, (innerHeight - _btnClearProxy.Height) \ 2)

        _btnPrimaryProxy.Location = New Point(Math.Max(0, rightX - _btnPrimaryProxy.Width), primaryY)
        _btnClearProxy.Location = New Point(Math.Max(0, _btnPrimaryProxy.Left - spacing - _btnClearProxy.Width), secondaryY)
        _btnCancelProxy.Location = New Point(Math.Max(0, _btnClearProxy.Left - spacing - _btnCancelProxy.Width), secondaryY)

        _btnCancelProxy.BringToFront()
        _btnClearProxy.BringToFront()
        _btnPrimaryProxy.BringToFront()
    End Sub

    Private Sub RefreshVatVisuals()
        If _txtVatRateProxy IsNot Nothing Then
            _txtVatRateProxy.ForeColor = Color.FromArgb(238, 241, 245)
            _txtVatRateProxy.Invalidate()
            _txtVatRateProxy.Update()
        End If

        If _pnlFields IsNot Nothing Then
            _pnlFields.Invalidate()
            _pnlFields.Update()
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

    Private Sub StyleInputLabel(lbl As Label)
        If lbl Is Nothing Then Return

        lbl.BackColor = Color.Transparent
        lbl.ForeColor = Color.FromArgb(240, 242, 245)
        lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lbl.Margin = Padding.Empty
        lbl.AutoSize = True
    End Sub

    Private Sub StyleFieldTextBox(tb As Guna2TextBox, multiline As Boolean)
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
        tb.Multiline = multiline
        tb.ScrollBars = If(multiline, ScrollBars.Vertical, ScrollBars.None)
        tb.MinimumSize = New Size(120, If(multiline, 110, 44))
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
        button.Image = Nothing
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
