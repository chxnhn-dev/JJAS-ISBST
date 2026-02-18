Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin

Public Class FrmVAT
    Dim formtoshow As Form
    Dim dr As SqlDataReader

    Private Sub Admin_Vat_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        displayData()

    End Sub
    Private Sub displayData()
        Try
            reload("Select VatID, 
                           Vat_Rate, 
                           DateUpdated 
                           FROM tbl_Vat", DGVtable)

            If DGVtable.Columns.Contains("VatID") Then
                DGVtable.Columns("VatID").Visible = False
            End If
            DGVtable.ClearSelection()
            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyStandardGridLayout(DGVtable)
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading vat: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            ' Auto-select the first row if nothing is selected
            If DGVtable.CurrentRow Is Nothing AndAlso DGVtable.Rows.Count > 0 Then
                DGVtable.Rows(0).Selected = True
            End If

            ' Check again if there is a row to edit
            If DGVtable.CurrentRow Is Nothing Then
                MessageBox.Show("No VAT record available to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' Get selected VAT info
            Dim vatId As Integer = Convert.ToInt32(DGVtable.CurrentRow.Cells("VatID").Value)
            Dim vatRate As Decimal = Convert.ToDecimal(DGVtable.CurrentRow.Cells("Vat_Rate").Value)

            ' Open FrmVATEntry form
            Dim f As New FrmVATEntry()
            f.VatID = vatId
            f.VatRate = vatRate

            If f.ShowDialog() = DialogResult.OK Then
                displayData()
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "VAT updated.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error updating VAT: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub
    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs) Handles btnMeasurement.Click
        formtoshow = New FrmManageSize()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub
    Private Sub btnUser_Click(sender As Object, e As EventArgs) Handles btnUser.Click
        formtoshow = New FrmManageUser()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnBrand_Click(sender As Object, e As EventArgs) Handles btnBrand.Click
        formtoshow = New FrmManageBrand()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        formtoshow = New FrmManageColor()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New FrmManageCategory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        formtoshow = New FrmManageProduct()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub
    Private Sub btnSupplier_Click(sender As Object, e As EventArgs) Handles btnSupplier.Click
        formtoshow = New FrmManageSupplier()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        formtoshow = New FrmManageDiscount()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New FrmManageDeliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New FrmInventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        formtoshow = New FrmPOS()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                "Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' ?? Log audit trail

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

            ' Clear current user info
            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show FrmLogin form again
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        formtoshow = New FrmDashboard()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New FrmTransactions()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New FrmAuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs)
        displayData()
    End Sub
End Class
