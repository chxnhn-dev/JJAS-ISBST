Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin
Imports Microsoft.VisualBasic.ApplicationServices
Public Class Admin_Brand
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"
    Private Const ColBrandId As String = "BrandID"
    Private selectedId As Integer = -1
    Dim formtoshow As Form

    Private Sub Admin_Brand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")

        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(txtSearch, "Search by company or supplier name.")
        tooltip.SetToolTip(lblPlaceholder, "Search by Brand.")
        tooltip.SetToolTip(btnAdd, "Add a new Brand.")
        tooltip.SetToolTip(btnEdit, "Edit the selected Brand.")
        tooltip.SetToolTip(btnDelete, "Deactivate the selected Brand.")


        Select Case FrmLogin.CurrentUser.Role.ToLower()
            Case "staff"
                btnPos.Visible = False
                btnUser.Visible = False
                btnVat.Visible = False
                btnDiscount.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False

                btnAuditTrail.Image = Nothing
                btnTransaction.Image = Nothing
                btnPos.Image = Nothing
                btnUser.Image = Nothing
                btnVat.Image = Nothing
                btnDiscount.Image = Nothing
        End Select

        RearrangeButtons(Panelmenu) ' replace PanelMenu with your actual panel name

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
            Dim dt As New DataTable()
            Dim sql As String = "
                SELECT BrandID, 
                       Brand
                FROM tbl_Brand WHERE IsActive = 1 AND (@Search = '' or Brand LIKE @Search)"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    Dim searchValue As String = "%" & searchText & "%"
                    cmd.Parameters.AddWithValue("@search", searchValue)
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            DGVsize.DataSource = dt

            If DGVsize.Columns.Contains("BrandID") Then
                DGVsize.Columns("BrandID").Visible = False
            End If

            EnsureActionColumns()
            DGVsize.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyStandardGridLayout(DGVsize)
            DGVsize.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim f As New Add_Brand()
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub

    Private Sub DGVSize_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim brandId As Integer
        If TryGetBrandId(row, brandId) Then
            selectedId = brandId
        Else
            selectedId = -1
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVsize.SelectedRows(0)
        Dim brandId As Integer
        If Not TryGetBrandId(row, brandId) Then
            MessageBox.Show("Invalid Brand ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        OpenEditModalById(brandId)
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

    Private Function DeleteValidation(Id As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM tbl_Products WHERE BrandID = @id"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", Id)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count = 0
            End Using
        End Using
    End Function
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedId = -1 Then
            MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        DeleteById(selectedId)
    End Sub

    Private Sub OpenEditModalById(brandId As Integer)
        Try
            Dim f As New Add_Brand With {
                .BrandID = brandId
            }

            If f.ShowDialog() = DialogResult.OK Then
                displayData("")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while editing brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteById(brandId As Integer)
        If Not DeleteValidation(brandId) Then
            MessageBox.Show("Cannot delete. Still used in Product", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DGVsize.ClearSelection()
            Exit Sub
        End If

        Try
            If MessageBox.Show("Are you sure you want to delete this brand?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Dim sql As String = "Delete tbl_Brand WHERE BrandID = @BrandID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@BrandID", brandId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted Brand.")
                MessageBox.Show("Brand deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                displayData("")
                DGVsize.ClearSelection()
                selectedId = -1
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while deleting brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DGVsize_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub
        If Not DGVsize.Columns.Contains(ColViewEdit) OrElse Not DGVsize.Columns.Contains(ColDelete) Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim brandId As Integer
        If Not TryGetBrandId(row, brandId) Then Exit Sub
        selectedId = brandId

        If colName = ColViewEdit Then
            OpenEditModalById(brandId)
        ElseIf colName = ColDelete Then
            DeleteById(brandId)
        End If
    End Sub

    Private Function TryGetBrandId(row As DataGridViewRow, ByRef brandId As Integer) As Boolean
        brandId = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColBrandId) Then Return False

        Dim raw As Object = row.Cells(ColBrandId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), brandId)
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

    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        formtoshow = New Admin_Color()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New admin_category()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub
    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        formtoshow = New Admin_Product()
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

    Private Sub btnBrand_Click(sender As Object, e As EventArgs) Handles btnBrand.Click

    End Sub

    Private Sub Panel7_Paint(sender As Object, e As PaintEventArgs) Handles Panel7.Paint

    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnPos.Click
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
            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""


            ' Close current form
            ' Close current form
            Me.Hide()

            ' Show Login form again
            Dim f As New FrmLogin()
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

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        formtoshow = New Admin_AuditTrail()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        displayData("")
    End Sub
End Class

