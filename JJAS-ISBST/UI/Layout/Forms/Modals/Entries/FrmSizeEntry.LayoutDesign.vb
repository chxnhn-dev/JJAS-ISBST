Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports Guna.UI2.WinForms
Imports JJAS_ISBST.FrmLogin

Partial Class FrmSizeEntry
    Private Sub InitializeModernUiIfNeeded()
        If _modernUiInitialized Then
            Return
        End If

        SuspendLayout()

        Dim fixedSize As New Size(820, 600)
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

        btnExit.Visible = False
        btnExit.Size = New Size(1, 1)
        btnExit.Location = New Point(-2000, -2000)

        _btnCloseProxy = New Guna2Button()
        StyleHeaderCloseButton(_btnCloseProxy)
        _btnCloseProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        _btnCloseProxy.Location = New Point(pnlHeader.Width - 60, 12)
        AddHandler _btnCloseProxy.Click, Sub() btnExit_Click(btnExit, EventArgs.Empty)

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

        EnsureCategoryControlsInitialized()

        For Each ctl As Control In New Control() {btnEdit, btnAdd, txtDescription, Label1, s, txtSize, cmbCategory, _lblCategory}
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
            .AutoScroll = False,
            .Padding = New Padding(0, 0, 0, 30),
            .BackColor = Color.FromArgb(34, 36, 42)
        }

        StyleInputLabel(_lblCategory)
        StyleInputLabel(s)
        StyleInputLabel(Label1)
        StyleFieldComboBox(cmbCategory)
        StyleFieldTextBox(txtSize, False)
        StyleFieldTextBox(txtDescription, True)

        StylePrimaryButton(btnAdd)
        StylePrimaryButton(btnEdit)
        btnAdd.Margin = Padding.Empty
        btnEdit.Margin = Padding.Empty

        _btnCancelProxy = New Guna2Button()
        StyleSecondaryButton(_btnCancelProxy)
        _btnCancelProxy.Text = "Cancel"
        _btnCancelProxy.Margin = Padding.Empty
        AddHandler _btnCancelProxy.Click, Sub() btnExit_Click(btnExit, EventArgs.Empty)

        _btnClearProxy = New Guna2Button()
        StyleSecondaryButton(_btnClearProxy)
        _btnClearProxy.Text = "Clear"
        _btnClearProxy.Margin = Padding.Empty
        AddHandler _btnClearProxy.Click, AddressOf btnClearProxy_Click

        _pnlFields.Controls.Add(cmbCategory)
        _pnlFields.Controls.Add(_lblCategory)
        _pnlFields.Controls.Add(txtDescription)
        _pnlFields.Controls.Add(Label1)
        _pnlFields.Controls.Add(txtSize)
        _pnlFields.Controls.Add(s)

        _pnlActions.Controls.Add(_btnCancelProxy)
        _pnlActions.Controls.Add(_btnClearProxy)
        _pnlActions.Controls.Add(btnAdd)
        _pnlActions.Controls.Add(btnEdit)
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
        If cmbCategory IsNot Nothing AndAlso cmbCategory.Items.Count > 0 Then
            cmbCategory.SelectedIndex = 0
        End If
        txtSize.Clear()
        txtDescription.Clear()
        If cmbCategory IsNot Nothing Then
            cmbCategory.Focus()
        Else
            txtSize.Focus()
        End If
        RefreshSizeEntryVisuals()
    End Sub

    Private Sub LayoutFieldControls()
        If _pnlFields Is Nothing OrElse _pnlFields.IsDisposed Then
            Return
        End If

        Dim contentWidth As Integer = GetFieldsContentWidth(_pnlFields)
        Dim y As Integer = 16

        If _lblCategory IsNot Nothing AndAlso cmbCategory IsNot Nothing Then
            _lblCategory.Location = New Point(0, y)
            _lblCategory.MaximumSize = New Size(contentWidth, 0)
            Dim categoryLabelHeight As Integer = Math.Max(24, _lblCategory.Height)

            cmbCategory.Location = New Point(0, _lblCategory.Top + categoryLabelHeight + 6)
            cmbCategory.Size = New Size(contentWidth, 44)
            cmbCategory.MinimumSize = New Size(120, 44)
            cmbCategory.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            y = cmbCategory.Bottom + 14
        End If

        s.Location = New Point(0, y)
        s.MaximumSize = New Size(contentWidth, 0)
        Dim sizeLabelHeight As Integer = Math.Max(24, s.Height)

        txtSize.Location = New Point(0, s.Top + sizeLabelHeight + 6)
        txtSize.Size = New Size(contentWidth, 44)
        txtSize.MinimumSize = New Size(120, 44)
        txtSize.AutoRoundedCorners = False
        txtSize.BorderRadius = 12
        txtSize.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        y = txtSize.Bottom + 14

        Label1.Location = New Point(0, y)
        Label1.MaximumSize = New Size(contentWidth, 0)
        Dim descriptionLabelHeight As Integer = Math.Max(24, Label1.Height)

        txtDescription.Location = New Point(0, Label1.Top + descriptionLabelHeight + 6)
        txtDescription.Size = New Size(contentWidth, 112)
        txtDescription.MinimumSize = New Size(120, 112)
        txtDescription.AutoRoundedCorners = False
        txtDescription.BorderRadius = 12
        txtDescription.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        y = txtDescription.Bottom + 14

        _pnlFields.AutoScrollMinSize = Size.Empty
    End Sub

    Private Function GetFieldsContentWidth(fieldsPanel As Panel) As Integer
        If fieldsPanel Is Nothing Then
            Return 640
        End If

        Dim clientWidth As Integer = fieldsPanel.ClientSize.Width
        If clientWidth <= 0 Then clientWidth = fieldsPanel.Width
        If clientWidth <= 0 Then Return 640

        Dim scrollbarAllowance As Integer = If(fieldsPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim width As Integer = clientWidth - fieldsPanel.Padding.Left - fieldsPanel.Padding.Right - scrollbarAllowance - 6
        Return Math.Max(240, width)
    End Function

    Private Sub LayoutActionButtons()
        If _pnlActions Is Nothing OrElse _pnlActions.IsDisposed Then
            Return
        End If

        Dim primaryButton As Guna2Button = If(btnEdit.Visible, btnEdit, btnAdd)
        Dim hiddenPrimary As Guna2Button = If(btnEdit.Visible, btnAdd, btnEdit)
        Dim rightX As Integer = _pnlActions.ClientSize.Width - _pnlActions.Padding.Right
        Dim spacing As Integer = 10

        primaryButton.Size = New Size(176, 48)
        _btnClearProxy.Size = New Size(100, 44)
        _btnCancelProxy.Size = New Size(100, 44)

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

    Private Sub RefreshSizeEntryVisuals()
        If cmbCategory IsNot Nothing Then
            cmbCategory.Invalidate()
            cmbCategory.Update()
        End If

        For Each tb As Guna2TextBox In {txtSize, txtDescription}
            If tb Is Nothing Then Continue For
            tb.ForeColor = Color.FromArgb(238, 241, 245)
            tb.Invalidate()
            tb.Update()
        Next

        If _pnlFields IsNot Nothing Then
            _pnlFields.Invalidate()
            _pnlFields.Update()
        End If
    End Sub

    Private Sub ForceImmediateFieldPaint()
        RefreshSizeEntryVisuals()

        For Each tb As Guna2TextBox In {txtSize, txtDescription}
            If tb Is Nothing Then Continue For
            tb.SelectionStart = tb.TextLength
            tb.SelectionLength = 0
            tb.Invalidate()
            tb.Update()
        Next

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

    Private Sub EnsureCategoryControlsInitialized()
        If _lblCategory Is Nothing Then
            _lblCategory = New Guna2HtmlLabel With {
                .Name = "lblCategory",
                .Text = "Category:",
                .BackColor = Color.Transparent
            }
        End If

        If cmbCategory Is Nothing Then
            cmbCategory = New Guna2ComboBox With {
                .Name = "cmbCategory",
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
        End If
    End Sub

    Private Sub LoadCategories()
        EnsureCategoryControlsInitialized()

        Dim sql As String = "
            SELECT CategoryID,
                   Category
            FROM dbo.tbl_Category
            WHERE IsActive = 1
            ORDER BY Category;"

        Dim dt As DataTable = Db.QueryDataTable(sql)
        Dim placeholder As DataRow = dt.NewRow()
        placeholder("CategoryID") = 0
        placeholder("Category") = "-- Select Category --"
        dt.Rows.InsertAt(placeholder, 0)

        cmbCategory.DataSource = dt
        cmbCategory.DisplayMember = "Category"
        cmbCategory.ValueMember = "CategoryID"
        cmbCategory.SelectedIndex = 0
    End Sub

    Private Function GetSelectedCategoryId() As Integer
        If cmbCategory Is Nothing OrElse cmbCategory.SelectedValue Is Nothing OrElse TypeOf cmbCategory.SelectedValue Is DataRowView Then
            Return 0
        End If

        Dim parsedValue As Integer
        If Integer.TryParse(cmbCategory.SelectedValue.ToString(), parsedValue) Then
            Return parsedValue
        End If

        Return 0
    End Function

    Private Sub StyleInputLabel(lbl As Guna2HtmlLabel)
        If lbl Is Nothing Then Return

        lbl.BackColor = Color.Transparent
        lbl.ForeColor = Color.FromArgb(240, 242, 245)
        lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        lbl.Margin = Padding.Empty
    End Sub

    Private Sub StyleFieldComboBox(cbo As Guna2ComboBox)
        If cbo Is Nothing Then Return

        cbo.Animated = False
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
        tb.MinimumSize = New Size(120, If(multiline, 112, 44))
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
