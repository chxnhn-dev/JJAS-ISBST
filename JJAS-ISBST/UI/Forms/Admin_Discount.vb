Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin

Public Class Admin_Discount
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"
    Private Const ColDiscountId As String = "DiscountID"
    Private selectedId As Integer = -1
    Dim formtoshow As Form
    Private Sub Admin_Discount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        displayData("")
        BlockCopyPaste(txtSearch)
    End Sub
    Private Sub displayData(Searchtext As String)
        Try
            Dim dt As New DataTable()
            Dim sql As String = "
                SELECT DiscountID, 
                       DiscountName,
                       DiscountValue,
                       Description
                FROM tbl_Discount WHERE IsActive = 1 AND (@Search = '' or DiscountName LIKE @Search)"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    Dim searchValue As String = "%" & Searchtext & "%"
                    cmd.Parameters.AddWithValue("@search", searchValue)
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            DGVsize.DataSource = dt

            If DGVsize.Columns.Contains("DiscountID") Then
                DGVsize.Columns("DiscountID").Visible = False
            End If

            EnsureActionColumns()
            DGVsize.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyStandardGridLayout(DGVsize)
            DGVsize.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        Dim f As New Add_Discount()
        If f.ShowDialog() = DialogResult.OK Then
            displayData("")
        End If
    End Sub
    Private Sub DGVSize_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim discountId As Integer
        If TryGetDiscountId(row, discountId) Then
            selectedId = discountId
        Else
            selectedId = -1
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
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) 
        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVsize.SelectedRows(0)
        Dim discountId As Integer
        If Not TryGetDiscountId(row, discountId) Then
            MessageBox.Show("Invalid Discount ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        OpenEditModalById(discountId)
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) 

        If DGVsize.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        DeleteById(selectedId)
    End Sub

    Private Sub OpenEditModalById(discountId As Integer)
        Try
            Dim f As New Add_Discount With {
                .DiscountID = discountId
            }

            If f.ShowDialog() = DialogResult.OK Then
                displayData("")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while editing Discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteById(discountId As Integer)
        Try
            If MessageBox.Show("Are you sure you want to delete this row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Dim sql As String = "Delete tbl_Discount WHERE DiscountID = @DiscountID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@DiscountID", discountId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using


                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted Discount.")
                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                displayData("")
                DGVsize.ClearSelection()
                selectedId = -1
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while deleting discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DGVsize_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVsize.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub
        If Not DGVsize.Columns.Contains(ColViewEdit) OrElse Not DGVsize.Columns.Contains(ColDelete) Then Exit Sub

        Dim colName As String = DGVsize.Columns(e.ColumnIndex).Name
        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVsize.Rows(e.RowIndex)
        Dim discountId As Integer
        If Not TryGetDiscountId(row, discountId) Then Exit Sub
        selectedId = discountId

        If colName = ColViewEdit Then
            OpenEditModalById(discountId)
        ElseIf colName = ColDelete Then
            DeleteById(discountId)
        End If
    End Sub

    Private Function TryGetDiscountId(row As DataGridViewRow, ByRef discountId As Integer) As Boolean
        discountId = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColDiscountId) Then Return False

        Dim raw As Object = row.Cells(ColDiscountId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), discountId)
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
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

    Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
        formtoshow = New Admin_Vat()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) 
        displayData("")
    End Sub
End Class
