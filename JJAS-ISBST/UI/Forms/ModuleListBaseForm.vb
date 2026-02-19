Imports Guna.UI2.WinForms

Public Class ModuleListBaseForm
    Public Sub New()
        InitializeComponent()
    End Sub

    Protected Sub SetSearchLabel(text As String)
        lblSearch.Text = text
    End Sub

    Protected Sub ConfigurePrimaryButtons(showAdd As Boolean, showPrint As Boolean)
        btnAdd.Visible = showAdd
        BtnPrint.Visible = showPrint

        If showAdd Then
            btnAdd.Location = New Point(1419, 35)
        End If

        If showPrint Then
            BtnPrint.Location = New Point(1412, 35)
        End If
    End Sub

    Protected Sub SetPageDisplay(currentPage As Integer, totalPages As Integer)
        lblPage.Text = $"Page {currentPage} of {totalPages}"
    End Sub

    Protected Sub SetActiveSidebarButton(activeButton As Guna2Button)
        Dim defaultColor As Color = Color.Black
        Dim activeColor As Color = Color.FromArgb(30, 32, 30)

        For Each ctrl As Control In panelmenu.Controls
            Dim btn As Guna2Button = TryCast(ctrl, Guna2Button)
            If btn Is Nothing Then Continue For
            btn.FillColor = If(btn Is activeButton, activeColor, defaultColor)
        Next
    End Sub
End Class

