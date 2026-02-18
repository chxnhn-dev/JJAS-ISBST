Imports System.Data.SqlClient

Imports JJAS_ISBST
Imports JJAS_ISBST.Login
Imports Microsoft.VisualBasic.ApplicationServices
Public Class Admin_Product
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"
    Private Const ColProductId As String = "ProductID"
    Dim SelectedID As Integer = -1
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

            EnsureActionColumns()

            With DGVsize
                If .Columns.Contains("SellingPrice") Then
                    .Columns("SellingPrice").DefaultCellStyle.Format = "C2"
                    .Columns("SellingPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
                End If
            End With

            DGVsize.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            For Each col As DataGridViewColumn In DGVsize.Columns
                If col.Name = "ProductImage" OrElse col.Name = ColViewEdit OrElse col.Name = ColDelete Then
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                Else
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End If
            Next

            ApplyStandardGridLayout(DGVsize)
            DGVsize.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading products: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub EnsureActionColumns()
        If Not DGVsize.Columns.Contains(ColViewEdit) Then
            Dim viewCol As New DataGridViewButtonColumn() With {
                .Name = ColViewEdit,
                .HeaderText = "Action",
                .Text = "View/Edit",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVsize.Columns.Add(viewCol)
        End If

        If Not DGVsize.Columns.Contains(ColDelete) Then
            Dim deleteCol As New DataGridViewButtonColumn() With {
                .Name = ColDelete,
                .HeaderText = "Delete",
                .Text = "Delete",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVsize.Columns.Add(deleteCol)
        End If
    End Sub

    Private Sub DGVsize_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVsize.DataBindingComplete
        ' Set row height
        ApplyStandardGridLayout(DGVsize)

        ' Set font size for cells
        DGVsize.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

        ' Optional: make header text bigger too
        DGVsize.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

        ' Optional: center align headers
        DGVsize.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim f As New Add_Product()
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub
    Private Sub DGVSize_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim productId As Integer
        If TryGetProductId(row, productId) Then
            SelectedID = productId
        Else
            SelectedID = -1
        End If
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVsize.SelectedRows(0)
        Dim productId As Integer
        If Not TryGetProductId(row, productId) Then
            MessageBox.Show("Invalid Product ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        OpenEditModalById(productId)
    End Sub

    Private Sub OpenEditModalById(productId As Integer)
        Dim row As DataGridViewRow = DGVsize.Rows.Cast(Of DataGridViewRow)().
            FirstOrDefault(Function(r)
                               If r.IsNewRow Then Return False
                               Dim idValue As Integer
                               Return TryGetProductId(r, idValue) AndAlso idValue = productId
                           End Function)
        If row Is Nothing Then
            MessageBox.Show("Selected product row was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim f As New Add_Product

        Try

            f.ProductID = productId

            If Not Validation(SelectedID) Then
                f.LockSellingPrice = True
            End If

            If f.ShowDialog() = DialogResult.OK Then
                ' optional actions after save
            End If

            displayData("") ' ✅ only call once

        Catch ex As Exception
            MessageBox.Show("An error occurred while editing product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub DeleteById(productId As Integer)
        If Not Validation(productId) Then
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
                        cmd.Parameters.AddWithValue("@ProductID", productId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted product.")

                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)


                displayData("")
                DGVsize.ClearSelection()
                SelectedID = -1
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while deleting product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DGVsize_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub
        If Not DGVsize.Columns.Contains(ColViewEdit) OrElse Not DGVsize.Columns.Contains(ColDelete) Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim productId As Integer
        If Not TryGetProductId(row, productId) Then Exit Sub
        SelectedID = productId

        If colName = ColViewEdit Then
            OpenEditModalById(productId)
        ElseIf colName = ColDelete Then
            DeleteById(productId)
        End If
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
    Private Sub btnDelete_Click(sender As Object, e As EventArgs)

        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVsize.SelectedRows(0)
        Dim productId As Integer
        If Not TryGetProductId(row, productId) Then
            MessageBox.Show("Invalid Product ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        DeleteById(productId)
    End Sub

    Private Function TryGetProductId(row As DataGridViewRow, ByRef productId As Integer) As Boolean
        productId = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColProductId) Then Return False

        Dim raw As Object = row.Cells(ColProductId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), productId)
    End Function


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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs)
        displayData("")
    End Sub
End Class
