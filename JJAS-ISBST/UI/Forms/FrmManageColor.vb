'Imports System.Data.SqlClient
'Imports JJAS_ISBST.FrmLogin
'Imports Microsoft.VisualBasic.ApplicationServices
'Public Class FrmManageColor
'    Private Const ColViewEdit As String = "colViewEdit"
'    Private Const ColDelete As String = "colDelete"
'    Private Const ColColorId As String = "COlorID"
'    Private selectedId As Integer = -1
'    Dim formtoshow As Form
'    Private Sub Admin_Color_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        displayData("")
'        BlockCopyPaste(txtSearch)


'        Dim tooltip As New ToolTip()
'        tooltip.SetToolTip(txtSearch, "Search by Category.")
'        tooltip.SetToolTip(lblPlaceholder, "Search by Category.")
'        tooltip.SetToolTip(btnAdd, "Add a new Category.")



'        Select Case FrmLogin.CurrentUser.Role.ToLower()

'            Case "staff"
'                btnPos.Visible = False
'                btnUser.Visible = False
'                btnVat.Visible = False
'                btnDiscount.Visible = False
'                btnTransaction.Visible = False
'                btnAuditTrail.Visible = False

'                btnAuditTrail.Image = Nothing
'                btnTransaction.Image = Nothing

'                btnPos.Image = Nothing
'                btnUser.Image = Nothing
'                btnVat.Image = Nothing
'                btnDiscount.Image = Nothing
'        End Select

'        RearrangeButtons(panelmenu) ' replace PanelMenu with your actual panel name

'    End Sub
'    Private Sub RearrangeButtons(panel As Panel)
'        Dim y As Integer = 10
'        For Each ctrl As Control In panel.Controls
'            If TypeOf ctrl Is Button AndAlso ctrl.Visible Then
'                ctrl.Location = New Point(10, y)
'                y += ctrl.Height + 10
'            End If
'        Next
'    End Sub
'    Private Sub displayData(searchtext As String)
'        Try
'            Dim dt As New DataTable()
'            Dim sql As String = "
'                SELECT COlorID, 
'                       Color
'                FROM tbl_Color WHERE IsActive = 1 AND (@Search = '' or Color LIKE @Search)"

'            Using conn As SqlConnection = DataAccess.GetConnection()
'                Using cmd As New SqlCommand(sql, conn)
'                    Dim searchValue As String = "%" & searchtext & "%"
'                    cmd.Parameters.AddWithValue("@search", searchValue)
'                    Dim da As New SqlDataAdapter(cmd)
'                    da.Fill(dt)
'                End Using
'            End Using

'            DGVtable.DataSource = dt

'            If DGVtable.Columns.Contains("COlorID") Then
'                DGVtable.Columns("COlorID").Visible = False
'            End If

'            EnsureActionColumns()
'            DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
'            ApplyStandardGridLayout(DGVtable)
'            DGVtable.ClearSelection()
'        Catch ex As Exception
'            MessageBox.Show("An error occurred while loading color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub EnsureActionColumns()
'        If Not DGVtable.Columns.Contains(ColViewEdit) Then
'            Dim viewCol As New DataGridViewButtonColumn() With {
'                .Name = ColViewEdit,
'                .HeaderText = "Action",
'                .Text = "View/Edit",
'                .UseColumnTextForButtonValue = True,
'                .Width = 100,
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
'                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
'            }
'            DGVtable.Columns.Add(viewCol)
'        End If

'        If Not DGVtable.Columns.Contains(ColDelete) Then
'            Dim deleteCol As New DataGridViewButtonColumn() With {
'                .Name = ColDelete,
'                .HeaderText = "Delete",
'                .Text = "Delete",
'                .UseColumnTextForButtonValue = True,
'                .Width = 100,
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
'                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
'            }
'            DGVtable.Columns.Add(deleteCol)
'        End If
'    End Sub
'    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
'        Dim f As New FrmColorEntry()
'        If f.ShowDialog() = DialogResult.OK Then
'            displayData("")
'        End If
'    End Sub
'    Private Sub DGVSize_CellClick(sender As Object, e As DataGridViewCellEventArgs)
'        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

'        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
'        If colName = ColViewEdit OrElse colName = ColDelete Then Exit Sub

'        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
'        Dim colorId As Integer
'        If TryGetColorId(row, colorId) Then
'            selectedId = colorId
'        Else
'            selectedId = -1
'        End If
'    End Sub
'    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
'        displayData(txtSearch.Text)
'    End Sub

'    Private Sub txtSearch_Enter(sender As Object, e As EventArgs)
'        lblPlaceholder.Visible = False
'    End Sub

'    Private Sub txtSearch_Leave(sender As Object, e As EventArgs)
'        If txtSearch.Text.Trim() = "" Then
'            lblPlaceholder.Visible = True
'        End If
'    End Sub
'    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs)
'        ' Prevent scanner's Enter from triggering anything
'        If e.KeyCode = Keys.Enter Then
'            e.SuppressKeyPress = True
'        End If
'    End Sub
'    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs)
'        txtSearch.Focus()
'    End Sub
'    Private Function DeleteValidation(Id As Integer) As Boolean
'        Dim sql As String = "SELECT COUNT(*) FROM tbl_Products WHERE ColorID = @id"
'        Using conn As SqlConnection = DataAccess.GetConnection()
'            Using cmd As New SqlCommand(sql, conn)
'                cmd.Parameters.AddWithValue("@id", Id)
'                conn.Open()
'                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
'                Return count = 0
'            End Using
'        End Using
'    End Function
'    Private Sub OpenEditModalById(colorId As Integer)
'        Try
'            Dim f As New FrmColorEntry With {
'                .ColorID = colorId
'            }

'            If f.ShowDialog() = DialogResult.OK Then
'                displayData("")
'            End If
'        Catch ex As Exception
'            MessageBox.Show("An error occurred while editing color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub DeleteById(colorId As Integer)
'        If Not DeleteValidation(colorId) Then
'            MessageBox.Show("This Color is still used in Products. Cannot delete.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            DGVtable.ClearSelection()
'            Exit Sub
'        End If
'        Try

'            If MessageBox.Show("Are you sure you want to delete this row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
'                Using conn As SqlConnection = DataAccess.GetConnection()
'                    conn.Open()
'                    Dim sql As String = "Delete tbl_Color WHERE ColorID = @ColorID"
'                    Using cmd As New SqlCommand(sql, conn)
'                        cmd.Parameters.AddWithValue("@ColorID", colorId)
'                        cmd.ExecuteNonQuery()
'                    End Using
'                End Using


'                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted Color.")
'                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)


'                displayData("")
'                DGVtable.ClearSelection()
'                selectedId = -1
'            End If
'        Catch ex As Exception
'            MessageBox.Show("An error occurred while deleting color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub DGVsize_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
'        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub
'        If Not DGVtable.Columns.Contains(ColViewEdit) OrElse Not DGVtable.Columns.Contains(ColDelete) Then Exit Sub

'        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
'        If colName <> ColViewEdit AndAlso colName <> ColDelete Then Exit Sub

'        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
'        Dim colorId As Integer
'        If Not TryGetColorId(row, colorId) Then Exit Sub
'        selectedId = colorId

'        If colName = ColViewEdit Then
'            OpenEditModalById(colorId)
'        ElseIf colName = ColDelete Then
'            DeleteById(colorId)
'        End If
'    End Sub

'    Private Function TryGetColorId(row As DataGridViewRow, ByRef colorId As Integer) As Boolean
'        colorId = -1
'        If row Is Nothing OrElse Not row.DataGridView.Columns.Contains(ColColorId) Then Return False

'        Dim raw As Object = row.Cells(ColColorId).Value
'        If raw Is Nothing OrElse IsDBNull(raw) Then Return False

'        Return Integer.TryParse(raw.ToString(), colorId)
'    End Function
'    Private Sub StartSwitchTimer()
'        switchtimer.Interval = 1000
'        switchtimer.Start()
'    End Sub
'    Private Sub switchTimer_Tick(sender As Object, e As EventArgs)
'        switchtimer.Stop()
'        Me.Hide()
'    End Sub

'    Private Sub btnUser_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageUser()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs)
'        Try
'            Using conn As SqlConnection = DataAccess.GetConnection()
'                conn.Open()
'                SessionService.EndCurrentSession("Logout")
'            End Using
'        Catch ex As Exception
'            MsgBox("Error logging out: " & ex.Message)
'        End Try


'        Me.DialogResult = DialogResult.OK
'        Me.Close()
'    End Sub

'    Private Sub btnMeasurement_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageSize()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnBrand_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageBrand()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs)

'    End Sub

'    Private Sub btnCategory_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageCategory()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnProduct_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageProduct()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnSupplier_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageSupplier()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnDiscount_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageDiscount()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnVat_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmVAT()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnInventory_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmManageDeliveries()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub Button4_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmInventory()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub Button5_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmPOS()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnLogout_Click(sender As Object, e As EventArgs)
'        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
'                                                "Logout",
'                                                MessageBoxButtons.YesNo,
'                                                MessageBoxIcon.Question)

'        If confirm = DialogResult.Yes Then
'            ' 🧾 Log audit trail



'            Try
'                Using conn As SqlConnection = DataAccess.GetConnection()
'                    conn.Open()
'                    SessionService.EndCurrentSession("Logout")
'                End Using
'            Catch ex As Exception
'                MsgBox("Error logging out: " & ex.Message)
'            End Try


'            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

'            ' Clear current user info
'            FrmLogin.CurrentUser.UserID = 0
'            FrmLogin.CurrentUser.Username = ""
'            FrmLogin.CurrentUser.Role = ""
'            FrmLogin.CurrentUser.FullName = ""


'            ' Close current form
'            ' Close current form
'            Me.Hide()

'            ' Show FrmLogin form again
'            Dim f As New FrmLogin()
'            f.Show()
'        End If
'    End Sub

'    Private Sub btnHome_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmDashboard()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnTransaction_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmTransactions()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs)
'        formtoshow = New FrmAuditTrail()
'        formtoshow.Show()
'        StartSwitchTimer()
'    End Sub

'    Private Sub btnRefresh_Click(sender As Object, e As EventArgs)
'        displayData("")
'    End Sub

'End Class

