Imports System.Data.SqlClient

Imports JJAS_ISBST
Imports JJAS_ISBST.Login
Imports Microsoft.VisualBasic.ApplicationServices
Public Class Admin_Product
    Dim SelectedID As String = -1
    Dim formtoshow As Form
    Private Sub Admin_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")
        Select Case Login.CurrentUser.Role.ToLower()

            Case "staff"
                btnPOS.Visible = False
                btnUser.Visible = False
                btnVat.Visible = False
                btnDiscount.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False

                btnAuditTrail.Image = Nothing
                btnTransaction.Image = Nothing
                btnPOS.Image = Nothing
                btnUser.Image = Nothing
                btnVat.Image = Nothing
                btnDiscount.Image = Nothing
        End Select

        RearrangeButtons(panelmenu) ' replace PanelMenu with your actual panel name

    End Sub
    Private Sub RearrangeButtons(panel As Panel)
        Dim y As Integer = 10
        For Each ctrl As Control In panel.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Visible Then
                ctrl.Location = New Point(10, y)
                y += ctrl.Height + 10
            End If
        Next
    End Sub
    Private Sub displayData(searchText As String)
    Try
        Dim repo As New ProductRepository()
        Dim dt As DataTable = repo.GetActiveProducts(searchText)

        ' Load image previews into a dedicated column for the grid
        ImageLoader.AddAndLoadImages(dt, "ImagePath", "ProductImage")

        DGVsize.DataSource = dt

        If DGVsize.Columns.Contains("ProductImage") Then
            DGVsize.Columns("ProductImage").DisplayIndex = 0
            Dim imgCol As DataGridViewImageColumn = DirectCast(DGVsize.Columns("ProductImage"), DataGridViewImageColumn)
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            imgCol.Width = 120
            imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If

        Dim hiddenCols() As String = {"ImagePath", "ProductID", "ColorID", "CategoryID", "BrandID", "SizeID"}
        For Each colName In hiddenCols
            If DGVsize.Columns.Contains(colName) Then
                DGVsize.Columns(colName).Visible = False
            End If
        Next

        With DGVsize
            If .Columns.Contains("SellingPrice") Then
                .Columns("SellingPrice").DefaultCellStyle.Format = "C2"
                .Columns("SellingPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
            End If
        End With

        DGVsize.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVsize.Columns
            If col.Name = "ProductImage" Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        DGVsize.ClearSelection()
    Catch ex As Exception
        MessageBox.Show("An error occurred while loading products: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
End Sub

    Private Sub DGVsize_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        ' Set row height
        DGVsize.RowTemplate.Height = 100
        For Each row As DataGridViewRow In DGVsize.Rows
            row.Height = 100
        Next

        ' Set font size for cells
        DGVsize.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

        ' Optional: make header text bigger too
        DGVsize.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

        ' Optional: center align headers
        DGVsize.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim Add As New Add_Product
        If Add.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub
    Private Sub DGVSize_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
            SelectedID = row.Cells(0).Value.ToString()
        End If
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If


        Dim row As DataGridViewRow = DGVsize.SelectedRows(0)
        Dim f As New Edit_Product

        Try

            f.ProductID = row.Cells("ProductID").Value.ToString()
            f.BarcodeNumber = row.Cells("BarcodeNumber").Value.ToString()
            f.Product = row.Cells("Product").Value.ToString()
            f.SellingPrice = row.Cells("SellingPrice").Value.ToString()
            f.Description = row.Cells("Description").Value.ToString()
            f.SizeID = row.Cells("SizeID").Value.ToString
            f.BrandID = row.Cells("BrandID").Value.ToString
            f.ColorID = row.Cells("ColorID").Value.ToString
            f.CategoryID = row.Cells("CategoryID").Value.ToString


            If row.Cells("ImagePath").Value IsNot Nothing Then
                f.ImagePath = row.Cells("ImagePath").Value.ToString()
            Else
                f.ImagePath = String.Empty
            End If

            If Not Validation(SelectedID) Then
                f.txtSellingPrice.ReadOnly = True
            End If

            If f.ShowDialog() = DialogResult.OK Then
                ' optional actions after save
            End If

            displayData("") ' ✅ only call once

        Catch ex As Exception
            MessageBox.Show("An error occurred while editing product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then
            lblPlaceholder.Visible = True
        End If
    End Sub
    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        ' Prevent scanner's Enter from triggering anything
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub
    Private Function Validation(Id As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM tbl_Delivery_Products WHERE ProductID = @ProductID"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@ProductID", Id)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count = 0
            End Using
        End Using
    End Function
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not Validation(SelectedID) Then
            MessageBox.Show("Cannot delete. This product is still Used", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DGVsize.ClearSelection()
            Exit Sub
        End If

        Try

            If MessageBox.Show("Are you sure you want to delete this row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Dim sql As String = "Delete tbl_Products WHERE ProductID = @ProductID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@ProductID", SelectedID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted product.")

                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)


                displayData("")
                DGVsize.ClearSelection()
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while deleting product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub btnUser_Click(sender As Object, e As EventArgs) Handles btnUser.Click
        formtoshow = New Admin_User()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub


    Private Sub btnTrash_Click(sender As Object, e As EventArgs)
        Dim f As New Trash_Product()
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                SessionService.EndCurrentSession("Logout")
            End Using
        Catch ex As Exception
            MsgBox("Error logging out: " & ex.Message)
        End Try


        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs) Handles btnMeasurement.Click
        formtoshow = New Admin_Size()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnBrand_Click(sender As Object, e As EventArgs) Handles btnBrand.Click
        formtoshow = New Admin_Brand()
        formtoshow.Show()
        StartSwitchTimer()


    End Sub


    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New admin_category()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnSupplier_Click(sender As Object, e As EventArgs) Handles btnSupplier.Click
        formtoshow = New Admin_Supplier()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        formtoshow = New Admin_Discount()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
        formtoshow = New Admin_Vat()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub


    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        formtoshow = New Admin_Color()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnPOS.Click
        formtoshow = New Admin_Pos()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                "Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' 🧾 Log audit trail

            Try
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    SessionService.EndCurrentSession("Logout")
                End Using
            Catch ex As Exception
                MsgBox("Error logging out: " & ex.Message)
            End Try


            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

            ' Clear current user info
            Login.CurrentUser.UserID = 0
            Login.CurrentUser.Username = ""
            Login.CurrentUser.Role = ""
            Login.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show Login form again
            Dim f As New Login()
            f.Show()
        End If
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        formtoshow = New Admin_Home()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub panelmenu_Paint(sender As Object, e As PaintEventArgs) Handles panelmenu.Paint

    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New Admin_AuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        displayData("")
    End Sub
End Class