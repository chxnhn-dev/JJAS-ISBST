Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin
Imports Microsoft.VisualBasic.ApplicationServices

' Note: For better separation of concerns, consider moving database operations (e.g., data retrieval, updates) to a dedicated data access layer, such as a SupplierRepository class.
' This would make the form code cleaner and easier to test. Example structure:
' Public Class SupplierRepository
'     Public Function GetSuppliers(searchText As String) As DataTable
'         ' Implementation here
'     End Function
'     Public Sub DeactivateSupplier(supplierId As Integer)
'         ' Implementation here
'     End Sub
'     ' Other methods...
' End Class

Public Class FrmManageSupplier
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColDelete As String = "colDelete"
    Private Const ColSupplierId As String = "SupplierId"
    Private selectedSupplierId As Integer = -1
    Dim formtoshow As Form

    Private Sub Admin_Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable copy-paste for search field
        Try
            BlockCopyPaste(txtSearch)
        Catch ex As Exception
            ' Log or handle the error instead of ignoring it
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        DisplayData("")

        ' UI Polish: Add tooltips to clarify field requirements
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(txtSearch, "Search by company or supplier name.")
        tooltip.SetToolTip(lblPlaceholder, "Search by company or supplier name.")
        tooltip.SetToolTip(btnAdd, "Add a new supplier.")


        Select Case FrmLogin.CurrentUser.Role.ToLower()
            Case "staff"
                btnPos.Visible = False
                btnUser.Visible = False
                btnVat.Visible = False
                btnDiscount.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False
        End Select

        RearrangeButtons(Panelmenu)

        ' UI Polish: Enable/disable buttons based on selection and role
        'UpdateButtonStates()
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

    Private Function SafeCellString(row As DataGridViewRow, colName As String) As String
        ' Enhanced Validation: Added null checks for robustness
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(colName) Then Return String.Empty
        Dim val = row.Cells(colName).Value
        If val Is Nothing OrElse IsDBNull(val) Then Return String.Empty
        Return val.ToString().Trim()  ' Trim for consistency
    End Function

    ' Refactoring: Moved to a potential SupplierRepository class for better separation
    Private Sub DisplayData(searchText As String)
        Try
            Dim dt As New DataTable()
            Dim sql As String = "
            SELECT SupplierId,
                   Company,
                   ContactNumber,
                   Address
            FROM tbl_supplier
            WHERE isactive = 1 AND (@search = '' OR Company LIKE @search)
            ORDER BY SupplierId DESC"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText.Trim() & "%")

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            DGVtable.DataSource = dt

            If DGVtable.Columns.Contains("SupplierId") Then
                DGVtable.Columns("SupplierId").Visible = False
            End If

            EnsureActionColumns()
            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ApplyStandardGridLayout(DGVtable)
            DGVtable.ClearSelection()

            ' Update buttons after loading data
            'UpdateButtonStates()

        Catch ex As InvalidOperationException
            MessageBox.Show("Database connection failed. Please check your configuration.")
        Catch ex As SqlException
            MessageBox.Show("SQL Error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message)
        Finally
            ' Optional: clean up or log if needed
        End Try
    End Sub

    Private Sub EnsureActionColumns()
        If Not DGVtable.Columns.Contains(ColViewEdit) Then
            Dim viewCol As New DataGridViewButtonColumn() With {
                .Name = ColViewEdit,
                .HeaderText = "Action",
                .Text = "View/Edit",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVtable.Columns.Add(viewCol)
        End If

        If Not DGVtable.Columns.Contains(ColDelete) Then
            Dim deleteCol As New DataGridViewButtonColumn() With {
                .Name = ColDelete,
                .HeaderText = "Delete",
                .Text = "Delete",
                .UseColumnTextForButtonValue = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVtable.Columns.Add(deleteCol)
        End If
    End Sub

    'Private Sub UpdateButtonStates()
    '    ' UI Polish: Enable/disable buttons based on selection and role
    '    Dim hasSelection As Boolean = DGVtable.SelectedRows.Count > 0
    '    btnEdit.Enabled = hasSelection
    '    btnDelete.Enabled = hasSelection
    '    ' btnAdd is always enabled unless restricted by role
    'End Sub

    Private Sub DGVsize_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim supplierId As Integer
        If TryGetSupplierId(row, supplierId) Then
            selectedSupplierId = supplierId
        Else
            selectedSupplierId = -1
        End If
        ' UI Polish: Update button states on selection change

        'UpdateButtonStates()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim f As New FrmSupplierEntry()
        If f.ShowDialog() = DialogResult.OK Then
            DisplayData("")
        End If
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If DGVtable.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a supplier first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtable.SelectedRows(0)
        Dim supplierId As Integer
        If Not TryGetSupplierId(row, supplierId) Then
            MessageBox.Show("Invalid Supplier ID selected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        OpenEditModalById(supplierId)

    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        DisplayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then lblPlaceholder.Visible = True
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then e.SuppressKeyPress = True
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If selectedSupplierId = -1 Then
            MessageBox.Show("Please select a supplier to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        DeleteById(selectedSupplierId)
    End Sub

    Private Sub OpenEditModalById(supplierId As Integer)
        Try
            Dim f As New FrmSupplierEntry With {
                .SupplierId = supplierId
            }

            If f.ShowDialog() = DialogResult.OK Then
                DisplayData("")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while editing supplier: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteById(supplierId As Integer)
        If MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try

                Using conn As SqlConnection = DataAccess.GetConnection()
                    Using cmd As New SqlCommand("UPDATE tbl_supplier SET isactive = 0 WHERE SupplierId = @SupplierId", conn)
                        cmd.Parameters.AddWithValue("@SupplierId", supplierId)
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                ' Security: Ensure CurrentUser details are securely retrieved (e.g., from a session or authenticated context, not hardcoded)
                ' Assuming CurrentUser is properly set via login/session management
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted supplier.")
                MessageBox.Show("Supplier Deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                DisplayData("")
                DGVtable.ClearSelection()
                selectedSupplierId = -1
                'UpdateButtonStates()
            Catch ex As Exception
                MessageBox.Show("An error occurred while deleting brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub DGVsize_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub
        If Not DGVtable.Columns.Contains(ColViewEdit) OrElse Not DGVtable.Columns.Contains(ColDelete) Then Exit Sub

        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim supplierId As Integer
        If Not TryGetSupplierId(row, supplierId) Then Exit Sub
        selectedSupplierId = supplierId

        If colName = ColViewEdit Then
            OpenEditModalById(supplierId)
        ElseIf colName = ColDelete Then
            DeleteById(supplierId)
        End If
    End Sub

    Private Function TryGetSupplierId(row As DataGridViewRow, ByRef supplierId As Integer) As Boolean
        supplierId = -1
        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColSupplierId) Then Return False

        Dim raw As Object = row.Cells(ColSupplierId).Value
        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

        Return Integer.TryParse(raw.ToString(), supplierId)
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
        formtoshow = New FrmManageUser()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs) Handles btnMeasurement.Click
        formtoshow = New FrmManageSize()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnCategory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New FrmManageCategory()
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

    Private Sub btnProduct_Click(sender As Object, e As EventArgs) Handles btnProduct.Click
        formtoshow = New FrmManageProduct()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDiscount_Click(sender As Object, e As EventArgs) Handles btnDiscount.Click
        formtoshow = New FrmManageDiscount()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnVat_Click(sender As Object, e As EventArgs) Handles btnVat.Click
        formtoshow = New FrmVAT()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDelivery_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        formtoshow = New FrmManageDeliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnCategory.Click
        formtoshow = New FrmManageCategory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click
        formtoshow = New FrmPOS()
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

    Private Sub Button1_Click(sender As Object, e As EventArgs)
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

            ' Show FrmLogin form again
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs)
        DisplayData("")
    End Sub

    ' Navigation methods unchanged...
End Class

