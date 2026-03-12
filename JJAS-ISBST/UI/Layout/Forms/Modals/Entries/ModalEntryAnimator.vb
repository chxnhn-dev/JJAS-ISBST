Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Guna.UI2.WinForms

Friend Module ModalEntryAnimator
    Private Const AnimationTickMs As Integer = 15
    Private Const FadeStep As Double = 0.12R

    Private ReadOnly _stateByForm As New ConditionalWeakTable(Of Form, AnimationState)()

    Private NotInheritable Class AnimationState
        Public Property IsPrepared As Boolean
        Public Property HasAnimated As Boolean
        Public Property DisableOpenAnimation As Boolean
        Public Property UseGunaOpenAnimation As Boolean
    End Class

    Public Sub DisableOpenAnimation(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return

        Dim state As AnimationState = _stateByForm.GetOrCreateValue(form)
        state.DisableOpenAnimation = True
    End Sub

    Public Sub UseGunaOpenAnimation(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return

        Dim state As AnimationState = _stateByForm.GetOrCreateValue(form)
        state.UseGunaOpenAnimation = True
        state.DisableOpenAnimation = True ' skip custom fade timer when using Guna borderless animation
    End Sub

    Public Sub PrepareForOpenAnimation(form As Form)
        If form Is Nothing OrElse IsInDesignMode(form) Then Return

        Dim state As AnimationState = _stateByForm.GetOrCreateValue(form)
        If state.IsPrepared Then Return

        state.IsPrepared = True

        If state.UseGunaOpenAnimation Then
            SetGunaWindowAnimation(form, True)
            Try
                form.Opacity = 1R
            Catch
            End Try
            Return
        End If

        SetGunaWindowAnimation(form, False)

        If state.DisableOpenAnimation Then
            Try
                form.Opacity = 1R
            Catch
            End Try
            Return
        End If

        Try
            form.Opacity = 0R
        Catch
            ' Ignore opacity failures on unsupported hosts.
        End Try
    End Sub

    Public Sub PlayOpenAnimation(form As Form)
        If form Is Nothing OrElse IsInDesignMode(form) Then Return
        If form.IsDisposed OrElse Not form.IsHandleCreated Then Return

        Dim state As AnimationState = _stateByForm.GetOrCreateValue(form)
        If state.HasAnimated Then Return

        If state.UseGunaOpenAnimation Then
            state.HasAnimated = True
            Try
                form.Opacity = 1R
            Catch
            End Try
            Return
        End If

        If state.DisableOpenAnimation Then
            state.HasAnimated = True
            Try
                form.Opacity = 1R
            Catch
            End Try
            Return
        End If

        state.HasAnimated = True

        form.BeginInvoke(New MethodInvoker(
            Sub()
                RunOpenAnimation(form)
            End Sub))
    End Sub

    ' Compatibility shim for older callers; no centering is performed.
    Public Sub PrepareCenteredStart(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return
    End Sub

    ' Compatibility shim for older callers; CenterParent handles positioning.
    Public Sub CenterModalToOwnerOrScreen(form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return
    End Sub

    Private Sub RunOpenAnimation(form As Form)
        If form Is Nothing OrElse form.IsDisposed OrElse Not form.IsHandleCreated Then Return

        Try
            form.Opacity = 0R
        Catch
        End Try

        Dim animationTimer As New Timer With {.Interval = AnimationTickMs}
        AddHandler animationTimer.Tick,
            Sub(sender As Object, e As EventArgs)
                If form Is Nothing OrElse form.IsDisposed OrElse Not form.IsHandleCreated Then
                    animationTimer.Stop()
                    animationTimer.Dispose()
                    Return
                End If

                Dim nextOpacity As Double
                Try
                    nextOpacity = Math.Min(1R, form.Opacity + FadeStep)
                    form.Opacity = nextOpacity
                Catch
                    nextOpacity = 1R
                End Try

                If nextOpacity >= 1R Then
                    animationTimer.Stop()
                    animationTimer.Dispose()
                    Try
                        form.Opacity = 1R
                    Catch
                    End Try
                End If
            End Sub

        animationTimer.Start()
    End Sub

    Private Sub SetGunaWindowAnimation(form As Form, animateWindow As Boolean)
        Try
            Dim flags As BindingFlags = BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public
            For Each field As FieldInfo In form.GetType().GetFields(flags)
                If Not GetType(Guna2BorderlessForm).IsAssignableFrom(field.FieldType) Then
                    Continue For
                End If

                Dim borderless As Guna2BorderlessForm = TryCast(field.GetValue(form), Guna2BorderlessForm)
                If borderless Is Nothing Then Continue For

                borderless.AnimateWindow = animateWindow
                If borderless.ContainerControl Is Nothing Then
                    borderless.ContainerControl = form
                End If
            Next
        Catch
            ' Ignore reflection/config issues.
        End Try
    End Sub

    Private Function IsInDesignMode(form As Form) As Boolean
        If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return True
        Return form.Site IsNot Nothing AndAlso form.Site.DesignMode
    End Function
End Module
