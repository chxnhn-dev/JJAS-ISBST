Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports Guna.UI2.WinForms

Public Module EntryFormUiStyler
    Private ReadOnly FormBackColor As Color = Color.FromArgb(22, 24, 30)
    Private ReadOnly HeaderBackColor As Color = Color.FromArgb(26, 29, 36)
    Private ReadOnly SurfaceColor As Color = Color.FromArgb(30, 33, 40)
    Private ReadOnly SurfaceAltColor As Color = Color.FromArgb(34, 36, 42)
    Private ReadOnly InputFillColor As Color = Color.FromArgb(41, 44, 51)
    Private ReadOnly BorderColor As Color = Color.FromArgb(76, 80, 88)
    Private ReadOnly HoverBorderColor As Color = Color.FromArgb(108, 114, 124)
    Private ReadOnly AccentColor As Color = Color.FromArgb(0, 122, 204)
    Private ReadOnly AccentHoverColor As Color = Color.FromArgb(20, 141, 224)
    Private ReadOnly TextColor As Color = Color.FromArgb(238, 241, 245)
    Private ReadOnly MutedTextColor As Color = Color.FromArgb(150, 154, 164)
    Private ReadOnly StandardEntryMinSize As Size = New Size(820, 520)
    Private Const StandardOuterPadding As Integer = 24

    Public Sub ApplyStandardDirectEntryLayout(form As Form,
                                              headerLabel As Label,
                                              closeButton As Control,
                                              ParamArray actionButtons() As Control)
        If form Is Nothing OrElse form.IsDisposed OrElse headerLabel Is Nothing Then
            Return
        End If

        If form.Controls.OfType(Of Control)().Any(Function(c) c.Name = "pnlMainContainer") Then
            Return
        End If

        Dim headerContainer As Control = headerLabel.Parent
        Dim bodyPanel As New Panel With {
            .Name = "pnlLegacyBodyHost",
            .BackColor = SurfaceColor
        }

        Dim controlsToMove As List(Of Control) = form.Controls.Cast(Of Control)().
            Where(Function(c) c IsNot headerContainer AndAlso
                               c IsNot headerLabel AndAlso
                               c IsNot closeButton AndAlso
                               Not String.Equals(c.Name, "pnlMainContainer", StringComparison.OrdinalIgnoreCase)).
            OrderBy(Function(c) c.TabIndex).
            ToList()

        For Each ctl As Control In controlsToMove
            form.Controls.Remove(ctl)
            bodyPanel.Controls.Add(ctl)
        Next

        ApplyStandardSingleColumnEntryLayout(form, headerLabel, closeButton, bodyPanel, actionButtons)
    End Sub

    Public Sub ApplyStandardSingleColumnEntryLayout(form As Form,
                                                    headerLabel As Label,
                                                    closeButton As Control,
                                                    bodyPanel As Panel,
                                                    ParamArray actionButtons() As Control)
        If form Is Nothing OrElse form.IsDisposed OrElse headerLabel Is Nothing OrElse bodyPanel Is Nothing Then
            Return
        End If

        If form.Controls.OfType(Of Control)().Any(Function(c) c.Name = "pnlMainContainer") Then
            Return
        End If

        form.SuspendLayout()
        Try
            EnsureStandardFormSize(form)
            EnsureCenteredModalBehavior(form)

            Dim originalBodyParent As Control = bodyPanel.Parent
            Dim originalLabelParent As Control = headerLabel.Parent
            Dim originalCloseParent As Control = If(closeButton Is Nothing, Nothing, closeButton.Parent)

            If originalBodyParent IsNot Nothing Then originalBodyParent.Controls.Remove(bodyPanel)
            If originalLabelParent IsNot Nothing Then originalLabelParent.Controls.Remove(headerLabel)
            If closeButton IsNot Nothing AndAlso originalCloseParent IsNot Nothing Then originalCloseParent.Controls.Remove(closeButton)

            Dim pnlMainContainer As New Panel With {
                .Name = "pnlMainContainer",
                .Dock = DockStyle.Fill,
                .Padding = New Padding(StandardOuterPadding),
                .BackColor = FormBackColor
            }

            Dim pnlHeader As New Panel With {
                .Name = "pnlHeader",
                .Dock = DockStyle.Top,
                .Height = 60,
                .BackColor = HeaderBackColor,
                .Padding = Padding.Empty
            }

            Dim pnlDivider As New Panel With {
                .Dock = DockStyle.Bottom,
                .Height = 1,
                .BackColor = Color.FromArgb(52, 56, 64)
            }

            Dim lblSubtitle As New Label With {
                .Name = "lblHeaderSubtitle",
                .AutoSize = True,
                .Text = "Fill in the details below",
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Regular),
                .ForeColor = MutedTextColor,
                .BackColor = HeaderBackColor,
                .Location = New Point(2, 34)
            }

            headerLabel.AutoSize = True
            headerLabel.Location = New Point(0, 4)
            headerLabel.Margin = Padding.Empty
            headerLabel.BackColor = HeaderBackColor

            pnlHeader.Controls.Add(lblSubtitle)
            pnlHeader.Controls.Add(headerLabel)

            If closeButton IsNot Nothing Then
                closeButton.Visible = False
                closeButton.Dock = DockStyle.None
                closeButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
                closeButton.Size = New Size(1, 1)
                closeButton.Location = New Point(-2000, -2000)
                pnlMainContainer.Controls.Add(closeButton)

                Dim closeProxy As Guna2Button = CreateHeaderCloseProxy(closeButton)
                If closeProxy IsNot Nothing Then
                    closeProxy.Anchor = AnchorStyles.Top Or AnchorStyles.Right
                    closeProxy.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - 16 - closeProxy.Width), 12)
                    pnlHeader.Controls.Add(closeProxy)

                    AddHandler pnlHeader.Resize,
                        Sub()
                            If closeProxy Is Nothing OrElse closeProxy.IsDisposed Then Return
                            closeProxy.Location = New Point(Math.Max(0, pnlHeader.ClientSize.Width - 16 - closeProxy.Width), 12)
                        End Sub

                    closeProxy.BringToFront()
                End If
            End If

            pnlHeader.Controls.Add(pnlDivider)
            If closeButton IsNot Nothing Then
                Dim closeProxyControl As Control = pnlHeader.Controls.Cast(Of Control)().
                    FirstOrDefault(Function(c) c IsNot closeButton AndAlso c.Name.IndexOf("btnExit", StringComparison.OrdinalIgnoreCase) >= 0)
                If closeProxyControl IsNot Nothing Then
                    closeProxyControl.BringToFront()
                End If
            End If

            Dim pnlBodyHost As New Panel With {
                .Name = "pnlBodyHost",
                .Dock = DockStyle.Fill,
                .Padding = New Padding(0, 20, 0, 0),
                .BackColor = FormBackColor
            }

            Dim pnlCard As Guna2Panel = CreateCardPanel("pnlEntryCard")
            pnlCard.Dock = DockStyle.Fill
            pnlCard.Padding = New Padding(20)

            bodyPanel.Dock = DockStyle.Fill
            bodyPanel.Margin = Padding.Empty
            bodyPanel.BackColor = SurfaceAltColor
            pnlCard.Controls.Add(bodyPanel)

            pnlBodyHost.Controls.Add(pnlCard)

            pnlMainContainer.Controls.Add(pnlBodyHost)
            pnlMainContainer.Controls.Add(pnlHeader)
            form.Controls.Add(pnlMainContainer)
            pnlMainContainer.BringToFront()

            SplitSingleColumnBodyPanel(form, bodyPanel, closeButton, actionButtons)

            If originalLabelParent IsNot Nothing AndAlso TypeOf originalLabelParent Is Panel Then
                Dim oldHeaderPanel = DirectCast(originalLabelParent, Panel)
                If oldHeaderPanel IsNot bodyPanel AndAlso oldHeaderPanel IsNot pnlHeader AndAlso oldHeaderPanel.Parent Is form Then
                    oldHeaderPanel.Visible = False
                    oldHeaderPanel.SendToBack()
                End If
            End If
        Finally
            form.ResumeLayout(True)
            form.PerformLayout()
        End Try
    End Sub

    Public Sub ApplyStandardEntryTheme(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then
            Return
        End If

        form.SuspendLayout()
        Try
            form.BackColor = FormBackColor
            form.ForeColor = TextColor

            If form.Font Is Nothing OrElse form.Font.Name <> "Segoe UI" Then
                form.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular)
            End If

            For Each ctl As Control In form.Controls
                StyleControlRecursive(ctl)
            Next

            StyleTopLevelElements(form)
        Finally
            form.ResumeLayout(True)
            form.PerformLayout()
            form.Invalidate()
        End Try
    End Sub

    Private Sub SplitSingleColumnBodyPanel(form As Form,
                                           bodyPanel As Panel,
                                           closeButton As Control,
                                           actionButtons As IEnumerable(Of Control))
        If bodyPanel Is Nothing Then Return

        If bodyPanel.Controls.OfType(Of Control)().Any(Function(c) c.Name = "pnlFieldsHost") Then
            Return
        End If

        Dim actionSet As New HashSet(Of Control)()
        If actionButtons IsNot Nothing Then
            For Each ctl In actionButtons
                If ctl IsNot Nothing Then actionSet.Add(ctl)
            Next
        End If

        Dim allControls As List(Of Control) = bodyPanel.Controls.Cast(Of Control)().ToList()
        Dim actionCtrls As List(Of Control) = allControls.Where(Function(c) actionSet.Contains(c)).ToList()
        Dim fieldCtrls As List(Of Control) = allControls.Where(Function(c) Not actionSet.Contains(c)).ToList()

        EnsureStandardActionButtons(form, bodyPanel, fieldCtrls, actionCtrls, closeButton)
        If actionCtrls.Count = 0 Then
            Return
        End If

        Dim pnlActions As New Panel With {
            .Name = "pnlActionsHost",
            .Dock = DockStyle.Bottom,
            .Height = 88,
            .BackColor = SurfaceAltColor,
            .Padding = New Padding(16, 12, 16, 0)
        }

        Dim pnlActionsDivider As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 1,
            .BackColor = Color.FromArgb(52, 56, 64)
        }
        pnlActions.Controls.Add(pnlActionsDivider)

        Dim pnlFields As New Panel With {
            .Name = "pnlFieldsHost",
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .BackColor = SurfaceAltColor,
            .Padding = New Padding(0, 0, 0, 30)
        }

        bodyPanel.SuspendLayout()
        Try
            bodyPanel.Controls.Add(pnlFields)
            bodyPanel.Controls.Add(pnlActions)

            For Each ctl In fieldCtrls
                If ctl Is Nothing Then Continue For

                Dim visualField As Control = CreateFieldVisualProxy(ctl)
                If visualField IsNot ctl Then
                    ctl.Dock = DockStyle.None
                    ctl.Anchor = AnchorStyles.Top Or AnchorStyles.Left
                    ctl.Visible = False
                    ctl.TabStop = False
                    ctl.Location = New Point(-2000, -2000)
                    ctl.Size = New Size(1, 1)
                    pnlFields.Controls.Add(ctl)
                    pnlFields.Controls.Add(visualField)
                    visualField.Visible = True
                    visualField.Enabled = ctl.Enabled
                    visualField.TabStop = True
                    visualField.TabIndex = ctl.TabIndex
                Else
                    pnlFields.Controls.Add(ctl)
                End If
            Next

            Dim visualActionCtrls As New List(Of Control)()
            For Each btn In actionCtrls
                If btn Is Nothing Then Continue For

                ' Keep the original button alive for existing event handlers/state changes.
                pnlActions.Controls.Add(btn)
                btn.Dock = DockStyle.None
                btn.Anchor = AnchorStyles.Top Or AnchorStyles.Right
                btn.Location = New Point(-2000, -2000)
                If btn.Visible Then
                    btn.Size = New Size(1, 1)
                    btn.TabStop = False
                End If

                Dim visualBtn As Control = CreateActionVisualProxy(btn)
                If visualBtn IsNot btn Then
                    pnlActions.Controls.Add(visualBtn)
                End If
                visualActionCtrls.Add(visualBtn)
            Next

            AddHandler pnlFields.ClientSizeChanged, Sub() NormalizeSingleColumnFields(pnlFields)
            AddHandler pnlActions.ClientSizeChanged, Sub() LayoutActionButtons(pnlActions, visualActionCtrls)

            NormalizeSingleColumnFields(pnlFields)
            LayoutActionButtons(pnlActions, visualActionCtrls)
        Finally
            bodyPanel.ResumeLayout(True)
        End Try
    End Sub

    Private Sub EnsureStandardFormSize(form As Form)
        If form Is Nothing Then Return

        Dim targetWidth As Integer = Math.Max(StandardEntryMinSize.Width, form.ClientSize.Width)
        Dim targetHeight As Integer = Math.Max(StandardEntryMinSize.Height, form.ClientSize.Height)

        form.MinimumSize = New Size(Math.Max(form.MinimumSize.Width, StandardEntryMinSize.Width),
                                    Math.Max(form.MinimumSize.Height, StandardEntryMinSize.Height))

        If form.ClientSize.Width < targetWidth OrElse form.ClientSize.Height < targetHeight Then
            form.ClientSize = New Size(targetWidth, targetHeight)
        End If
    End Sub

    Private Sub EnsureCenteredModalBehavior(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return
        form.StartPosition = FormStartPosition.CenterParent
    End Sub

    Private Sub CenterModalToOwnerOrScreen(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return
        form.StartPosition = FormStartPosition.CenterParent
    End Sub

    Private Function CreateCardPanel(name As String) As Guna2Panel
        Return New Guna2Panel With {
            .Name = name,
            .FillColor = SurfaceAltColor,
            .BorderColor = Color.FromArgb(58, 61, 68),
            .BorderThickness = 1,
            .BorderRadius = 14,
            .Margin = Padding.Empty
        }
    End Function

    Private Sub StyleHeaderCloseProxy(btn As Guna2Button)
        If btn Is Nothing Then Return

        btn.Animated = True
        btn.UseTransparentBackground = False
        btn.DefaultAutoSize = False
        btn.AutoRoundedCorners = False
        btn.BorderRadius = 10
        btn.BorderThickness = 1
        btn.BorderColor = BorderColor
        btn.FillColor = Color.FromArgb(62, 66, 75)
        btn.BackColor = HeaderBackColor
        btn.ForeColor = TextColor
        btn.HoverState.FillColor = Color.FromArgb(78, 83, 94)
        btn.HoverState.BorderColor = HoverBorderColor
        btn.HoverState.ForeColor = TextColor
        btn.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)

        Dim rm As New System.ComponentModel.ComponentResourceManager(GetType(FrmUserEntry))
        Dim closeImage As Image = TryCast(rm.GetObject("btnExit.Image"), Image)
        btn.Image = closeImage
        btn.ImageSize = New Size(12, 12)

        If closeImage Is Nothing Then
            btn.Text = "X"
            btn.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        End If
    End Sub

    Private Sub BuildInfoCard(card As Guna2Panel, rawHeaderText As String, sourceBodyPanel As Panel)
        If card Is Nothing Then Return

        card.Padding = New Padding(20)

        Dim normalizedTitle As String = If(rawHeaderText, String.Empty).Trim().TrimEnd(":"c)
        If String.IsNullOrWhiteSpace(normalizedTitle) Then
            normalizedTitle = "Entry Form"
        End If

        Dim lblSection As New Label With {
            .Name = "lblInfoSection",
            .AutoSize = True,
            .BackColor = SurfaceAltColor,
            .ForeColor = MutedTextColor,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Regular),
            .Text = "FORM OVERVIEW",
            .Location = New Point(20, 20)
        }

        Dim lblTitle As New Label With {
            .Name = "lblInfoTitle",
            .AutoSize = False,
            .BackColor = SurfaceAltColor,
            .ForeColor = TextColor,
            .Font = New Font("Segoe UI Semibold", 14.5F, FontStyle.Bold),
            .Text = normalizedTitle,
            .Location = New Point(20, 48),
            .Size = New Size(Math.Max(120, card.Width - 40), 46)
        }
        lblTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim visibleFieldCount As Integer = 0
        If sourceBodyPanel IsNot Nothing Then
            visibleFieldCount = sourceBodyPanel.Controls.Cast(Of Control)().
                Count(Function(c) c.Visible AndAlso IsInputLikeControl(c))
        End If

        Dim lblDesc As New Label With {
            .Name = "lblInfoBody",
            .AutoSize = False,
            .BackColor = SurfaceAltColor,
            .ForeColor = MutedTextColor,
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular),
            .Text = $"Use the fields on the right to save or update this record." & Environment.NewLine &
                    $"Detected input fields: {Math.Max(0, visibleFieldCount)}",
            .Location = New Point(20, 98),
            .Size = New Size(Math.Max(120, card.Width - 40), 62)
        }
        lblDesc.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim tipFrame As New Guna2Panel With {
            .Name = "pnlInfoTipFrame",
            .BorderColor = BorderColor,
            .BorderThickness = 1,
            .BorderRadius = 12,
            .FillColor = InputFillColor,
            .Location = New Point(20, 176),
            .Size = New Size(Math.Max(120, card.Width - 40), 120)
        }
        tipFrame.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim lblTipTitle As New Label With {
            .Name = "lblInfoTipTitle",
            .AutoSize = True,
            .BackColor = InputFillColor,
            .ForeColor = TextColor,
            .Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
            .Text = "Entry Tips",
            .Location = New Point(16, 14)
        }

        Dim lblTipBody As New Label With {
            .Name = "lblInfoTipBody",
            .AutoSize = False,
            .BackColor = InputFillColor,
            .ForeColor = MutedTextColor,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Regular),
            .Text = "Review values before saving." & Environment.NewLine &
                    "Use Clear to reset fields." & Environment.NewLine &
                    "Use Cancel to close without changes.",
            .Location = New Point(16, 38),
            .Size = New Size(Math.Max(80, tipFrame.Width - 32), 68)
        }
        lblTipBody.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        tipFrame.Controls.Add(lblTipBody)
        tipFrame.Controls.Add(lblTipTitle)

        card.Controls.Add(tipFrame)
        card.Controls.Add(lblDesc)
        card.Controls.Add(lblTitle)
        card.Controls.Add(lblSection)
    End Sub

    Private Sub EnsureStandardActionButtons(form As Form,
                                            bodyPanel As Panel,
                                            fieldCtrls As IList(Of Control),
                                            actionCtrls As IList(Of Control),
                                            closeButton As Control)
        Dim hasCancel As Boolean = actionCtrls.Any(Function(c) c IsNot Nothing AndAlso
                                                       (c.Name.IndexOf("cancel", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                                        c.Text.IndexOf("cancel", StringComparison.OrdinalIgnoreCase) >= 0))

        Dim hasClear As Boolean = actionCtrls.Any(Function(c) c IsNot Nothing AndAlso
                                                      (c.Name.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                                       c.Text.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0))

        If Not hasCancel Then
            Dim btnCancelAuto As New Guna2Button With {
                .Name = "btnCancelAuto",
                .Text = "Cancel",
                .Visible = True
            }
            AddHandler btnCancelAuto.Click,
                Sub()
                    If closeButton IsNot Nothing AndAlso Not closeButton.IsDisposed Then
                        TriggerControlClick(closeButton)
                    ElseIf form IsNot Nothing AndAlso Not form.IsDisposed Then
                        form.DialogResult = DialogResult.Cancel
                        form.Close()
                    End If
                End Sub
            actionCtrls.Add(btnCancelAuto)
        End If

        Dim containsGrid As Boolean = fieldCtrls.Any(Function(c) TypeOf c Is DataGridView OrElse TypeOf c Is Guna2DataGridView)
        If Not hasClear AndAlso Not containsGrid Then
            Dim btnClearAuto As New Guna2Button With {
                .Name = "btnClearAuto",
                .Text = "Clear",
                .Visible = True
            }
            AddHandler btnClearAuto.Click,
                Sub()
                    ClearFieldControls(bodyPanel.Controls.OfType(Of Control)().
                        FirstOrDefault(Function(c) String.Equals(c.Name, "pnlFieldsHost", StringComparison.OrdinalIgnoreCase)))
                End Sub
            actionCtrls.Add(btnClearAuto)
        End If
    End Sub

    Private Sub TriggerControlClick(ctl As Control)
        If ctl Is Nothing OrElse ctl.IsDisposed Then Return

        If TypeOf ctl Is Button Then
            DirectCast(ctl, Button).PerformClick()
            Return
        End If

        If TypeOf ctl Is Guna2Button Then
            DirectCast(ctl, Guna2Button).PerformClick()
            Return
        End If

        If TypeOf ctl Is IButtonControl Then
            DirectCast(ctl, IButtonControl).PerformClick()
            Return
        End If

        ctl.Focus()
        SendKeys.SendWait("{ENTER}")
    End Sub

    Private Function CreateActionVisualProxy(source As Control) As Control
        If source Is Nothing Then
            Return Nothing
        End If

        If TypeOf source Is Guna2Button Then
            Return source
        End If

        If TypeOf source Is Button Then
            Dim legacyButton As Button = DirectCast(source, Button)
            Dim proxy As New Guna2Button With {
                .Name = legacyButton.Name & "_Proxy",
                .Text = legacyButton.Text,
                .Enabled = legacyButton.Enabled,
                .Visible = legacyButton.Visible,
                .Cursor = Cursors.Hand,
                .Image = Nothing
            }

            AddHandler proxy.Click, Sub() TriggerControlClick(legacyButton)
            AddHandler legacyButton.EnabledChanged, Sub() proxy.Enabled = legacyButton.Enabled
            AddHandler legacyButton.TextChanged, Sub() proxy.Text = legacyButton.Text
            AddHandler legacyButton.VisibleChanged, Sub() proxy.Visible = legacyButton.Visible

            Return proxy
        End If

        Return source
    End Function

    Private Function CreateFieldVisualProxy(source As Control) As Control
        If source Is Nothing Then
            Return Nothing
        End If

        If TypeOf source Is Guna2TextBox OrElse TypeOf source Is Guna2ComboBox OrElse TypeOf source Is Guna2DateTimePicker Then
            Return source
        End If

        If TypeOf source Is TextBox Then
            Dim legacy As TextBox = DirectCast(source, TextBox)
            Dim proxy As New Guna2TextBox With {
                .Name = legacy.Name & "_ProxyField",
                .Text = legacy.Text,
                .PlaceholderText = String.Empty,
                .Enabled = legacy.Enabled,
                .Visible = legacy.Visible,
                .ReadOnly = legacy.ReadOnly,
                .MaxLength = legacy.MaxLength,
                .Cursor = Cursors.IBeam,
                .Animated = False,
                .Multiline = legacy.Multiline,
                .ScrollBars = If(legacy.Multiline, ScrollBars.Vertical, ScrollBars.None)
            }

            If legacy.Name.IndexOf("price", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               legacy.Name.IndexOf("rate", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               legacy.Name.IndexOf("value", StringComparison.OrdinalIgnoreCase) >= 0 Then
                proxy.TextAlign = HorizontalAlignment.Right
            End If

            Try
                BlockCopyPaste(proxy)
            Catch
            End Try

            ApplyKnownInputRestrictions(legacy.Name, proxy)

            AddHandler proxy.TextChanged,
                Sub()
                    If legacy.IsDisposed Then Return
                    If legacy.Text <> proxy.Text Then legacy.Text = proxy.Text
                End Sub
            AddHandler legacy.TextChanged,
                Sub()
                    If proxy.IsDisposed Then Return
                    If proxy.Text <> legacy.Text Then proxy.Text = legacy.Text
                End Sub
            AddHandler legacy.EnabledChanged, Sub() If Not proxy.IsDisposed Then proxy.Enabled = legacy.Enabled
            AddHandler legacy.VisibleChanged, Sub() If Not proxy.IsDisposed Then proxy.Visible = legacy.Visible

            Return proxy
        End If

        If TypeOf source Is RichTextBox Then
            Dim legacy As RichTextBox = DirectCast(source, RichTextBox)
            Dim proxy As New Guna2TextBox With {
                .Name = legacy.Name & "_ProxyField",
                .Text = legacy.Text,
                .PlaceholderText = String.Empty,
                .Enabled = legacy.Enabled,
                .Visible = legacy.Visible,
                .ReadOnly = legacy.ReadOnly,
                .MaxLength = legacy.MaxLength,
                .Cursor = Cursors.IBeam,
                .Animated = False,
                .Multiline = True,
                .ScrollBars = ScrollBars.Vertical
            }

            Try
                BlockCopyPaste(proxy)
            Catch
            End Try

            AddHandler proxy.TextChanged,
                Sub()
                    If legacy.IsDisposed Then Return
                    If legacy.Text <> proxy.Text Then legacy.Text = proxy.Text
                End Sub
            AddHandler legacy.TextChanged,
                Sub()
                    If proxy.IsDisposed Then Return
                    If proxy.Text <> legacy.Text Then proxy.Text = legacy.Text
                End Sub
            AddHandler legacy.EnabledChanged, Sub() If Not proxy.IsDisposed Then proxy.Enabled = legacy.Enabled
            AddHandler legacy.VisibleChanged, Sub() If Not proxy.IsDisposed Then proxy.Visible = legacy.Visible

            Return proxy
        End If

        Return source
    End Function

    Private Function CreateHeaderCloseProxy(source As Control) As Guna2Button
        If source Is Nothing Then
            Return Nothing
        End If

        Dim proxy As New Guna2Button With {
            .Name = If(String.IsNullOrWhiteSpace(source.Name), "btnExitProxy", source.Name & "_Proxy"),
            .Cursor = Cursors.Hand,
            .Text = String.Empty,
            .Size = New Size(36, 36)
        }

        StyleHeaderCloseProxy(proxy)

        AddHandler proxy.Click, Sub() TriggerControlClick(source)
        AddHandler source.EnabledChanged, Sub() If Not proxy.IsDisposed Then proxy.Enabled = source.Enabled
        AddHandler source.VisibleChanged, Sub() If Not proxy.IsDisposed Then proxy.Visible = True

        Return proxy
    End Function

    Private Sub ApplyKnownInputRestrictions(sourceName As String, proxy As Guna2TextBox)
        If proxy Is Nothing Then Return

        Dim nameValue As String = If(sourceName, String.Empty).ToLowerInvariant()

        If nameValue.Contains("contactnumber") Then
            AddHandler proxy.KeyPress,
                Sub(sender, e)
                    If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
                        e.Handled = True
                    End If
                End Sub
            Return
        End If

        If nameValue.Contains("discountvalue") OrElse nameValue.Contains("vatrate") Then
            AddHandler proxy.KeyPress,
                Sub(sender, e)
                    If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
                        e.Handled = False
                    ElseIf e.KeyChar = "."c Then
                        e.Handled = proxy.Text.Contains(".")
                    Else
                        e.Handled = True
                    End If
                End Sub
            Return
        End If

        If nameValue.Contains("discountname") Then
            AddHandler proxy.KeyPress,
                Sub(sender, e)
                    If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = "-"c OrElse e.KeyChar = ControlChars.Back) Then
                        e.Handled = True
                    End If
                End Sub
        End If
    End Sub

    Private Sub ClearFieldControls(container As Control)
        If container Is Nothing Then Return

        For Each ctl As Control In container.Controls
            If ctl Is Nothing OrElse ctl.Name.IndexOf("pnlActions", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Continue For
            End If

            If TypeOf ctl Is Guna2TextBox Then
                Dim tb = DirectCast(ctl, Guna2TextBox)
                If Not tb.ReadOnly Then tb.Clear()
                Continue For
            End If

            If TypeOf ctl Is TextBox Then
                Dim tb = DirectCast(ctl, TextBox)
                If Not tb.ReadOnly Then tb.Clear()
                Continue For
            End If

            If TypeOf ctl Is RichTextBox Then
                Dim rtb = DirectCast(ctl, RichTextBox)
                If Not rtb.ReadOnly Then rtb.Clear()
                Continue For
            End If

            If TypeOf ctl Is Guna2ComboBox Then
                Dim cbo = DirectCast(ctl, Guna2ComboBox)
                If cbo.Items.Count > 0 Then
                    cbo.SelectedIndex = 0
                ElseIf cbo.DropDownStyle <> ComboBoxStyle.DropDownList Then
                    cbo.Text = String.Empty
                End If
                Continue For
            End If

            If TypeOf ctl Is ComboBox Then
                Dim cbo = DirectCast(ctl, ComboBox)
                If cbo.Items.Count > 0 Then
                    cbo.SelectedIndex = 0
                ElseIf cbo.DropDownStyle <> ComboBoxStyle.DropDownList Then
                    cbo.Text = String.Empty
                End If
                Continue For
            End If

            If TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Checked = False
                Continue For
            End If

            If TypeOf ctl Is Guna2DateTimePicker Then
                Dim dtp = DirectCast(ctl, Guna2DateTimePicker)
                Dim targetDate As DateTime = DateTime.Today
                If targetDate < dtp.MinDate Then targetDate = dtp.MinDate
                If targetDate > dtp.MaxDate Then targetDate = dtp.MaxDate
                dtp.Value = targetDate
                Continue For
            End If

            If TypeOf ctl Is DateTimePicker Then
                Dim dtp = DirectCast(ctl, DateTimePicker)
                Dim targetDate As DateTime = DateTime.Today
                If targetDate < dtp.MinDate Then targetDate = dtp.MinDate
                If targetDate > dtp.MaxDate Then targetDate = dtp.MaxDate
                dtp.Value = targetDate
                Continue For
            End If

            If ctl.HasChildren AndAlso Not TypeOf ctl Is DataGridView AndAlso Not TypeOf ctl Is Guna2DataGridView Then
                ClearFieldControls(ctl)
            End If
        Next
    End Sub

    Private Sub NormalizeSingleColumnFields(pnlFields As Panel)
        If pnlFields Is Nothing OrElse pnlFields.IsDisposed Then Return

        Dim rightPadding As Integer = 18 + If(pnlFields.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim maxBottom As Integer = 0
        Dim orderedControls As List(Of Control) = pnlFields.Controls.Cast(Of Control)().
            Where(Function(c) c IsNot Nothing AndAlso c.Visible).
            OrderBy(Function(c) c.Top).
            ThenBy(Function(c) c.Left).
            ToList()

        Dim canStackSimple As Boolean = orderedControls.Count > 0 AndAlso
            orderedControls.All(Function(c) IsInputLikeControl(c) OrElse IsFieldLabelLike(c))

        If canStackSimple Then
            ApplyStackedSingleColumnLayout(pnlFields, orderedControls, rightPadding)
            Return
        End If

        For Each ctl As Control In orderedControls
            If ctl Is Nothing Then Continue For

            If IsInputLikeControl(ctl) Then
                Dim canStretch As Boolean = ShouldStretchToFullWidth(ctl, orderedControls, pnlFields.ClientSize.Width)
                ctl.Anchor = If(canStretch,
                                AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right,
                                AnchorStyles.Top Or AnchorStyles.Left)
                If canStretch Then
                    Dim targetWidth As Integer = Math.Max(80, pnlFields.ClientSize.Width - ctl.Left - rightPadding)
                    ctl.Width = targetWidth
                End If

                If TypeOf ctl Is Guna2TextBox Then
                    Dim gtb = DirectCast(ctl, Guna2TextBox)
                    gtb.AutoRoundedCorners = False
                    gtb.BorderRadius = 12
                    If Not gtb.Multiline Then
                        gtb.Height = 44
                        gtb.MinimumSize = New Size(120, 44)
                    Else
                        gtb.Height = Math.Max(112, gtb.Height)
                        gtb.MinimumSize = New Size(120, 112)
                    End If
                ElseIf TypeOf ctl Is Guna2ComboBox Then
                    ctl.Height = Math.Max(44, ctl.Height)
                ElseIf TypeOf ctl Is TextBox Then
                    If DirectCast(ctl, TextBox).Multiline Then
                        ctl.Height = Math.Max(60, ctl.Height)
                    Else
                        ctl.Height = Math.Max(29, ctl.Height)
                    End If
                ElseIf TypeOf ctl Is ComboBox Then
                    ctl.Height = Math.Max(32, ctl.Height)
                ElseIf TypeOf ctl Is DateTimePicker OrElse TypeOf ctl Is Guna2DateTimePicker Then
                    ctl.Height = Math.Max(36, ctl.Height)
                End If
            ElseIf TypeOf ctl Is DataGridView OrElse TypeOf ctl Is Guna2DataGridView Then
                ctl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                ctl.Width = Math.Max(220, pnlFields.ClientSize.Width - ctl.Left - rightPadding)
            End If

            maxBottom = Math.Max(maxBottom, ctl.Bottom)
        Next

        pnlFields.AutoScrollMinSize = New Size(0, maxBottom + 24)
    End Sub

    Private Sub ApplyStackedSingleColumnLayout(pnlFields As Panel,
                                               orderedControls As IList(Of Control),
                                               rightPadding As Integer)
        If pnlFields Is Nothing Then Return

        Dim contentWidth As Integer = Math.Max(240, pnlFields.ClientSize.Width - pnlFields.Padding.Left - pnlFields.Padding.Right - rightPadding)
        Dim x As Integer = 0
        Dim y As Integer = 16

        For Each ctl As Control In orderedControls
            If ctl Is Nothing OrElse Not ctl.Visible Then Continue For

            If IsFieldLabelLike(ctl) Then
                ctl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                ctl.Location = New Point(x, y)
                ctl.Width = Math.Max(80, contentWidth)

                If TypeOf ctl Is Label Then
                    Dim lbl = DirectCast(ctl, Label)
                    lbl.MaximumSize = New Size(contentWidth, 0)
                    lbl.AutoSize = True
                    y = lbl.Bottom + 6
                ElseIf TypeOf ctl Is Guna2HtmlLabel Then
                    Dim glbl = DirectCast(ctl, Guna2HtmlLabel)
                    glbl.MaximumSize = New Size(contentWidth, 0)
                    y = glbl.Bottom + 6
                Else
                    y = ctl.Bottom + 6
                End If

                Continue For
            End If

            If IsInputLikeControl(ctl) Then
                ctl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                ctl.Location = New Point(x, y)
                ctl.Width = Math.Max(80, contentWidth)

                If TypeOf ctl Is Guna2TextBox Then
                    Dim gtb = DirectCast(ctl, Guna2TextBox)
                    gtb.AutoRoundedCorners = False
                    gtb.BorderRadius = 12
                    If gtb.Multiline Then
                        gtb.Height = Math.Max(112, gtb.Height)
                        gtb.MinimumSize = New Size(120, 112)
                    Else
                        gtb.Height = 44
                        gtb.MinimumSize = New Size(120, 44)
                    End If
                ElseIf TypeOf ctl Is Guna2ComboBox Then
                    ctl.Height = Math.Max(44, ctl.Height)
                ElseIf TypeOf ctl Is TextBox Then
                    If DirectCast(ctl, TextBox).Multiline Then
                        ctl.Height = Math.Max(100, ctl.Height)
                    Else
                        ctl.Height = Math.Max(29, ctl.Height)
                    End If
                ElseIf TypeOf ctl Is RichTextBox Then
                    ctl.Height = Math.Max(112, ctl.Height)
                ElseIf TypeOf ctl Is ComboBox Then
                    ctl.Height = Math.Max(32, ctl.Height)
                ElseIf TypeOf ctl Is DateTimePicker OrElse TypeOf ctl Is Guna2DateTimePicker Then
                    ctl.Height = Math.Max(36, ctl.Height)
                End If

                y = ctl.Bottom + 14
            End If
        Next

        pnlFields.AutoScrollMinSize = New Size(0, y + pnlFields.Padding.Bottom)
    End Sub

    Private Function IsFieldLabelLike(ctl As Control) As Boolean
        If ctl Is Nothing OrElse Not ctl.Visible Then Return False
        If String.Equals(ctl.Name, "Label12", StringComparison.OrdinalIgnoreCase) Then Return False
        If ctl.Name.IndexOf("subtitle", StringComparison.OrdinalIgnoreCase) >= 0 Then Return False
        If ctl.Name.IndexOf("info", StringComparison.OrdinalIgnoreCase) >= 0 Then Return False

        Return TypeOf ctl Is Label OrElse TypeOf ctl Is Guna2HtmlLabel
    End Function

    Private Function ShouldStretchToFullWidth(target As Control,
                                              siblings As IList(Of Control),
                                              containerWidth As Integer) As Boolean
        If target Is Nothing Then Return False
        If containerWidth <= 0 Then Return False

        If target.Left <= 8 Then Return True

        Dim effectiveWidth As Integer = Math.Max(1, containerWidth)
        Dim isWideControl As Boolean = target.Width >= CInt(effectiveWidth * 0.55R)
        If Not isWideControl Then
            Return False
        End If

        Dim sameRowInputs As List(Of Control) = siblings.
            Where(Function(c) c IsNot target AndAlso IsInputLikeControl(c) AndAlso
                              Math.Abs(c.Top - target.Top) <= 12 AndAlso c.Visible).
            ToList()

        If sameRowInputs.Count > 0 Then
            Return False
        End If

        Return True
    End Function

    Private Sub LayoutActionButtons(pnlActions As Panel, actionCtrls As IEnumerable(Of Control))
        If pnlActions Is Nothing OrElse pnlActions.IsDisposed OrElse actionCtrls Is Nothing Then Return

        Dim ordered As List(Of Control) = actionCtrls.
            Where(Function(c) c IsNot Nothing AndAlso c.Visible).
            OrderByDescending(Function(c) ActionButtonRank(c)).
            ToList()

        Dim rightX As Integer = pnlActions.ClientSize.Width - pnlActions.Padding.Right
        Dim spacing As Integer = 10

        For Each ctl As Control In ordered
            Dim isPrimary As Boolean = IsPrimaryAction(ctl.Name, ctl.Text)
            Dim targetHeight As Integer = If(isPrimary, 48, 44)
            Dim targetWidth As Integer = If(isPrimary, 176, 100)

            If TypeOf ctl Is Guna2Button Then
                Dim gbtn = DirectCast(ctl, Guna2Button)
                gbtn.DefaultAutoSize = False
                gbtn.AutoRoundedCorners = False
                gbtn.BorderRadius = If(isPrimary, 12, 10)
            End If

            ctl.Size = New Size(targetWidth, targetHeight)
            Dim innerTop As Integer = pnlActions.Padding.Top
            Dim innerBottom As Integer = Math.Max(innerTop, pnlActions.ClientSize.Height - pnlActions.Padding.Bottom)
            Dim innerHeight As Integer = Math.Max(0, innerBottom - innerTop)
            Dim topY As Integer = innerTop + Math.Max(0, (innerHeight - targetHeight) \ 2)
            ctl.Margin = Padding.Empty
            ctl.Location = New Point(Math.Max(0, rightX - ctl.Width), topY)
            rightX = ctl.Left - spacing
            ctl.BringToFront()
        Next
    End Sub

    Private Function IsInputLikeControl(ctl As Control) As Boolean
        Return TypeOf ctl Is TextBox OrElse
               TypeOf ctl Is RichTextBox OrElse
               TypeOf ctl Is ComboBox OrElse
               TypeOf ctl Is DateTimePicker OrElse
               TypeOf ctl Is Guna2TextBox OrElse
               TypeOf ctl Is Guna2ComboBox OrElse
               TypeOf ctl Is Guna2DateTimePicker
    End Function

    Private Function ActionButtonRank(ctl As Control) As Integer
        Dim combined As String = $"{ctl.Name} {ctl.Text}".ToLowerInvariant()

        If combined.Contains("save") OrElse combined.Contains("add") OrElse combined.Contains("update") OrElse combined.Contains("edit") Then
            Return 30
        End If

        If combined.Contains("clear") Then
            Return 20
        End If

        If combined.Contains("cancel") Then
            Return 10
        End If

        Return 15
    End Function

    Private Sub StyleTopLevelElements(form As Form)
        Dim headerLabel As Control = form.Controls.Cast(Of Control)().
            FirstOrDefault(Function(c) String.Equals(c.Name, "Label12", StringComparison.OrdinalIgnoreCase))
        If headerLabel IsNot Nothing Then
            If TypeOf headerLabel Is Label Then
                Dim lbl = DirectCast(headerLabel, Label)
                lbl.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
                lbl.ForeColor = TextColor
                lbl.BackColor = If(lbl.Parent IsNot Nothing, lbl.Parent.BackColor, FormBackColor)
            End If
        End If

        Dim closeButton As Control = form.Controls.Cast(Of Control)().
            FirstOrDefault(Function(c) c.Name.Equals("btnExit", StringComparison.OrdinalIgnoreCase) OrElse c.Name.Equals("btnexit", StringComparison.OrdinalIgnoreCase))
        If closeButton IsNot Nothing Then
            closeButton.BringToFront()
        End If
    End Sub

    Private Sub StyleControlRecursive(ctl As Control)
        If ctl Is Nothing Then Return

        StyleSingleControl(ctl)

        For Each child As Control In ctl.Controls
            StyleControlRecursive(child)
        Next
    End Sub

    Private Sub StyleSingleControl(ctl As Control)
        If TypeOf ctl Is Guna2TextBox Then
            StyleGunaTextBox(DirectCast(ctl, Guna2TextBox))
            Return
        End If

        If TypeOf ctl Is Guna2ComboBox Then
            StyleGunaComboBox(DirectCast(ctl, Guna2ComboBox))
            Return
        End If

        If TypeOf ctl Is Guna2Button Then
            StyleGunaButton(DirectCast(ctl, Guna2Button))
            Return
        End If

        If TypeOf ctl Is Guna2HtmlLabel Then
            StyleGunaHtmlLabel(DirectCast(ctl, Guna2HtmlLabel))
            Return
        End If

        If TypeOf ctl Is Guna2Panel Then
            StyleGunaPanel(DirectCast(ctl, Guna2Panel))
            Return
        End If

        If TypeOf ctl Is Guna2PictureBox Then
            StyleGunaPictureBox(DirectCast(ctl, Guna2PictureBox))
            Return
        End If

        If TypeOf ctl Is Guna2DataGridView Then
            StyleGunaDataGridView(DirectCast(ctl, Guna2DataGridView))
            Return
        End If

        If TypeOf ctl Is Guna2DateTimePicker Then
            StyleGunaDateTimePicker(DirectCast(ctl, Guna2DateTimePicker))
            Return
        End If

        If TypeOf ctl Is RichTextBox Then
            StyleRichTextBox(DirectCast(ctl, RichTextBox))
            Return
        End If

        If TypeOf ctl Is TextBox Then
            StyleTextBox(DirectCast(ctl, TextBox))
            Return
        End If

        If TypeOf ctl Is ComboBox Then
            StyleComboBox(DirectCast(ctl, ComboBox))
            Return
        End If

        If TypeOf ctl Is Button Then
            StyleButton(DirectCast(ctl, Button))
            Return
        End If

        If TypeOf ctl Is Label Then
            StyleLabel(DirectCast(ctl, Label))
            Return
        End If

        If TypeOf ctl Is CheckBox Then
            StyleCheckBox(DirectCast(ctl, CheckBox))
            Return
        End If

        If TypeOf ctl Is DateTimePicker Then
            StyleDateTimePicker(DirectCast(ctl, DateTimePicker))
            Return
        End If

        If TypeOf ctl Is Panel Then
            StylePanel(DirectCast(ctl, Panel))
            Return
        End If

        If TypeOf ctl Is PictureBox Then
            StylePictureBox(DirectCast(ctl, PictureBox))
        End If
    End Sub

    Private Sub StylePanel(pnl As Panel)
        If pnl Is Nothing Then Return

        If pnl.Dock = DockStyle.Top AndAlso pnl.Height <= 64 Then
            pnl.BackColor = HeaderBackColor
        ElseIf pnl.Dock = DockStyle.Bottom OrElse pnl.Dock = DockStyle.Fill Then
            pnl.BackColor = SurfaceColor
        ElseIf pnl.BackColor = Color.Black OrElse pnl.BackColor.ToArgb() = 0 Then
            pnl.BackColor = SurfaceColor
        Else
            pnl.BackColor = Color.FromArgb(
                Math.Max(24, pnl.BackColor.R),
                Math.Max(26, pnl.BackColor.G),
                Math.Max(30, pnl.BackColor.B))
        End If
    End Sub

    Private Sub StyleLabel(lbl As Label)
        If lbl Is Nothing Then Return

        Dim isHeader As Boolean = lbl.Name.Equals("Label12", StringComparison.OrdinalIgnoreCase)
        Dim isPlaceholder As Boolean = lbl.Name.IndexOf("placeholder", StringComparison.OrdinalIgnoreCase) >= 0
        Dim isSubtitle As Boolean = lbl.Name.IndexOf("subtitle", StringComparison.OrdinalIgnoreCase) >= 0
        Dim isInfoText As Boolean = lbl.Name.IndexOf("lblinfo", StringComparison.OrdinalIgnoreCase) >= 0

        If isPlaceholder OrElse isSubtitle OrElse isInfoText Then
            lbl.ForeColor = MutedTextColor
        Else
            lbl.ForeColor = TextColor
        End If

        Dim looksLikeFieldLabel As Boolean =
            (lbl.Text IsNot Nothing AndAlso (lbl.Text.Contains(":") OrElse lbl.Text.IndexOf("optional", StringComparison.OrdinalIgnoreCase) >= 0)) AndAlso
            Not isHeader AndAlso Not isSubtitle AndAlso Not isInfoText

        If isHeader Then
            lbl.Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold)
        ElseIf lbl.Name.IndexOf("lblInfoTitle", StringComparison.OrdinalIgnoreCase) >= 0 Then
            lbl.Font = New Font("Segoe UI Semibold", 14.5F, FontStyle.Bold)
        ElseIf lbl.Name.IndexOf("lblInfoTipTitle", StringComparison.OrdinalIgnoreCase) >= 0 Then
            lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        ElseIf looksLikeFieldLabel Then
            lbl.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        ElseIf lbl.Font Is Nothing OrElse lbl.Font.Size < 10.0F Then
            lbl.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        Else
            lbl.Font = New Font("Segoe UI", lbl.Font.Size, If(lbl.Font.Bold, FontStyle.Bold, FontStyle.Regular))
        End If

        If lbl.Parent IsNot Nothing Then
            lbl.BackColor = If(isPlaceholder, InputFillColor, lbl.Parent.BackColor)
        Else
            lbl.BackColor = FormBackColor
        End If
    End Sub

    Private Sub StyleGunaHtmlLabel(lbl As Guna2HtmlLabel)
        If lbl Is Nothing Then Return

        lbl.BackColor = Color.Transparent
        lbl.ForeColor = TextColor

        If lbl.Name.Equals("Label12", StringComparison.OrdinalIgnoreCase) Then
            lbl.Font = New Font("Segoe UI Semibold", 16.0F, FontStyle.Bold)
        ElseIf lbl.Font Is Nothing OrElse lbl.Font.Size < 10.0F Then
            lbl.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        Else
            lbl.Font = New Font("Segoe UI", lbl.Font.Size, If(lbl.Font.Bold, FontStyle.Bold, FontStyle.Regular))
        End If
    End Sub

    Private Sub StyleTextBox(tb As TextBox)
        If tb Is Nothing Then Return

        tb.BackColor = InputFillColor
        tb.ForeColor = TextColor
        tb.BorderStyle = BorderStyle.FixedSingle
        tb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)

        If tb.Name.IndexOf("price", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
           tb.Name.IndexOf("rate", StringComparison.OrdinalIgnoreCase) >= 0 Then
            tb.TextAlign = HorizontalAlignment.Right
        End If
    End Sub

    Private Sub StyleRichTextBox(rtb As RichTextBox)
        If rtb Is Nothing Then Return

        rtb.BackColor = InputFillColor
        rtb.ForeColor = TextColor
        rtb.BorderStyle = BorderStyle.FixedSingle
        rtb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
    End Sub

    Private Sub StyleComboBox(cbo As ComboBox)
        If cbo Is Nothing Then Return

        cbo.BackColor = InputFillColor
        cbo.ForeColor = TextColor
        cbo.FlatStyle = FlatStyle.Flat
        cbo.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        cbo.IntegralHeight = False
    End Sub

    Private Sub StyleDateTimePicker(dtp As DateTimePicker)
        If dtp Is Nothing Then Return

        dtp.CalendarMonthBackground = InputFillColor
        dtp.CalendarForeColor = TextColor
        dtp.CalendarTitleBackColor = SurfaceAltColor
        dtp.CalendarTitleForeColor = TextColor
        dtp.BackColor = InputFillColor
        dtp.ForeColor = TextColor
        dtp.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
    End Sub

    Private Sub StyleCheckBox(chk As CheckBox)
        If chk Is Nothing Then Return

        chk.ForeColor = TextColor
        chk.BackColor = If(chk.Parent IsNot Nothing, chk.Parent.BackColor, SurfaceColor)
        chk.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        chk.UseVisualStyleBackColor = False
    End Sub

    Private Sub StyleButton(btn As Button)
        If btn Is Nothing Then Return

        Dim closeButton As Boolean = IsCloseButton(btn.Name, btn.Text)
        Dim primaryButton As Boolean = Not closeButton AndAlso IsPrimaryAction(btn.Name, btn.Text)

        btn.Enabled = True
        btn.Visible = True
        btn.UseVisualStyleBackColor = False
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = If(primaryButton, 1, 1)
        btn.FlatAppearance.BorderColor = If(primaryButton, AccentColor, BorderColor)
        btn.FlatAppearance.MouseDownBackColor = If(primaryButton, AccentHoverColor, Color.FromArgb(74, 79, 88))
        btn.FlatAppearance.MouseOverBackColor = If(primaryButton, AccentHoverColor, Color.FromArgb(78, 83, 94))
        btn.ForeColor = TextColor
        btn.Font = If(primaryButton,
                      New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
                      New Font("Segoe UI", 10.0F, FontStyle.Regular))

        If closeButton Then
            btn.BackColor = SurfaceAltColor
        ElseIf primaryButton Then
            btn.BackColor = AccentColor
        Else
            btn.BackColor = Color.FromArgb(62, 66, 75)
        End If

        If btn.Parent IsNot Nothing AndAlso btn.Parent.Dock = DockStyle.Top AndAlso closeButton Then
            btn.BackColor = SurfaceAltColor
        End If
    End Sub

    Private Sub StyleGunaButton(btn As Guna2Button)
        If btn Is Nothing Then Return

        Dim closeButton As Boolean = IsCloseButton(btn.Name, btn.Text)
        Dim primaryButton As Boolean = Not closeButton AndAlso IsPrimaryAction(btn.Name, btn.Text)

        btn.Enabled = True
        btn.Visible = True
        btn.Animated = True
        btn.UseTransparentBackground = False
        btn.Cursor = Cursors.Hand
        btn.ForeColor = If(closeButton, TextColor, Color.White)
        btn.Font = If(primaryButton,
                      New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold),
                      New Font("Segoe UI", 10.0F, FontStyle.Regular))

        If closeButton Then
            btn.DefaultAutoSize = False
            btn.AutoRoundedCorners = False
            btn.Size = New Size(36, 36)
            btn.BorderRadius = 10
            btn.BorderThickness = 1
            btn.BorderColor = BorderColor
            btn.FillColor = Color.FromArgb(62, 66, 75)
            btn.HoverState.FillColor = Color.FromArgb(78, 83, 94)
            btn.HoverState.BorderColor = HoverBorderColor
            btn.HoverState.ForeColor = TextColor
            If btn.Image IsNot Nothing Then
                btn.ImageSize = New Size(12, 12)
            End If
        ElseIf primaryButton Then
            btn.BorderRadius = 12
            btn.BorderThickness = 1
            btn.BorderColor = AccentColor
            btn.FillColor = AccentColor
            btn.HoverState.FillColor = AccentHoverColor
            btn.HoverState.BorderColor = Color.FromArgb(56, 171, 250)
            btn.HoverState.ForeColor = Color.White
            btn.DisabledState.FillColor = Color.FromArgb(72, 76, 84)
            btn.DisabledState.ForeColor = Color.FromArgb(180, 184, 192)
        Else
            btn.BorderRadius = 10
            btn.BorderThickness = 1
            btn.BorderColor = BorderColor
            btn.FillColor = Color.FromArgb(62, 66, 75)
            btn.HoverState.FillColor = Color.FromArgb(78, 83, 94)
            btn.HoverState.BorderColor = HoverBorderColor
            btn.HoverState.ForeColor = Color.White
            btn.DisabledState.FillColor = Color.FromArgb(58, 62, 70)
            btn.DisabledState.ForeColor = Color.FromArgb(170, 174, 181)
        End If
    End Sub

    Private Sub StyleGunaTextBox(tb As Guna2TextBox)
        If tb Is Nothing Then Return

        tb.Animated = False
        tb.AutoRoundedCorners = False
        tb.BorderRadius = 12
        tb.BorderThickness = 1
        tb.BorderColor = BorderColor
        tb.FocusedState.BorderColor = AccentColor
        tb.HoverState.BorderColor = HoverBorderColor
        tb.FillColor = InputFillColor
        tb.ForeColor = TextColor
        tb.PlaceholderForeColor = MutedTextColor
        tb.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        tb.Margin = New Padding(0)
        tb.MinimumSize = New Size(120, If(tb.Multiline, 112, 44))

        If tb.Name.IndexOf("price", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
           tb.Name.IndexOf("rate", StringComparison.OrdinalIgnoreCase) >= 0 Then
            tb.TextAlign = HorizontalAlignment.Right
        End If
    End Sub

    Private Sub StyleGunaComboBox(cbo As Guna2ComboBox)
        If cbo Is Nothing Then Return

        cbo.BackColor = Color.Transparent
        cbo.BorderRadius = 12
        cbo.BorderThickness = 1
        cbo.BorderColor = BorderColor
        cbo.FocusedColor = AccentColor
        cbo.FocusedState.BorderColor = AccentColor
        cbo.HoverState.BorderColor = HoverBorderColor
        cbo.FillColor = InputFillColor
        cbo.ForeColor = TextColor
        cbo.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        cbo.DrawMode = DrawMode.OwnerDrawFixed
        cbo.DropDownStyle = ComboBoxStyle.DropDownList
        cbo.IntegralHeight = False
        cbo.ItemHeight = 38
        cbo.Margin = New Padding(0)
    End Sub

    Private Sub StyleGunaPanel(pnl As Guna2Panel)
        If pnl Is Nothing Then Return

        pnl.FillColor = If(pnl.Dock = DockStyle.Top AndAlso pnl.Height <= 64, HeaderBackColor, SurfaceColor)
        pnl.BackColor = Color.Transparent
        pnl.BorderRadius = Math.Max(pnl.BorderRadius, 12)
        pnl.BorderThickness = Math.Max(pnl.BorderThickness, 1)
        If pnl.BorderColor = Color.Empty OrElse pnl.BorderColor = Color.Transparent Then
            pnl.BorderColor = Color.FromArgb(52, 56, 64)
        End If
    End Sub

    Private Sub StyleGunaPictureBox(pic As Guna2PictureBox)
        If pic Is Nothing Then Return

        pic.BackColor = Color.Transparent
        pic.FillColor = InputFillColor
        pic.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

    Private Sub StylePictureBox(pic As PictureBox)
        If pic Is Nothing Then Return

        pic.BackColor = InputFillColor
        pic.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

    Private Sub StyleGunaDateTimePicker(dtp As Guna2DateTimePicker)
        If dtp Is Nothing Then Return

        dtp.BorderRadius = 12
        dtp.FillColor = InputFillColor
        dtp.ForeColor = TextColor
        dtp.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
    End Sub

    Private Sub StyleGunaDataGridView(grid As Guna2DataGridView)
        If grid Is Nothing Then Return

        grid.BackgroundColor = SurfaceColor
        grid.GridColor = Color.FromArgb(58, 62, 70)
        grid.EnableHeadersVisualStyles = False

        grid.ColumnHeadersDefaultCellStyle.BackColor = SurfaceAltColor
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceAltColor
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextColor
        grid.DefaultCellStyle.BackColor = InputFillColor
        grid.DefaultCellStyle.ForeColor = TextColor
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(54, 62, 76)
        grid.DefaultCellStyle.SelectionForeColor = TextColor
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 41, 48)
        grid.AlternatingRowsDefaultCellStyle.ForeColor = TextColor

        grid.ThemeStyle.BackColor = SurfaceColor
        grid.ThemeStyle.GridColor = Color.FromArgb(58, 62, 70)
        grid.ThemeStyle.HeaderStyle.BackColor = SurfaceAltColor
        grid.ThemeStyle.HeaderStyle.ForeColor = TextColor
        grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        grid.ThemeStyle.RowsStyle.BackColor = InputFillColor
        grid.ThemeStyle.RowsStyle.ForeColor = TextColor
        grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(54, 62, 76)
        grid.ThemeStyle.RowsStyle.SelectionForeColor = TextColor
        grid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(38, 41, 48)
        grid.ThemeStyle.AlternatingRowsStyle.ForeColor = TextColor
    End Sub

    Private Function IsCloseButton(name As String, text As String) As Boolean
        Dim combined As String = $"{name} {text}".ToLowerInvariant()
        Return combined.Contains("exit") OrElse combined.Contains("close") OrElse combined = "x" OrElse combined.EndsWith(" x")
    End Function

    Private Function IsPrimaryAction(name As String, text As String) As Boolean
        Dim combined As String = $"{name} {text}".ToLowerInvariant()
        If combined.Contains("cancel") OrElse combined.Contains("clear") OrElse combined.Contains("browse") Then
            Return False
        End If

        Return combined.Contains("add") OrElse combined.Contains("save") OrElse combined.Contains("update") OrElse combined.Contains("edit")
    End Function
End Module
