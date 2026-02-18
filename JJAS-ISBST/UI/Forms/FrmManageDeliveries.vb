
Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin
Imports System.Text

Public Class FrmManageDeliveries
    Private Const ColViewEdit As String = "colViewEdit"
    Private Const ColPost As String = "colPost"
    Private Const ColDelete As String = "colDelete"
    Private DeliveryID As Integer = -1
    Private DeliveryProductID As Integer = -1
    Dim formtoshow As Form

    Private Sub Admin_Deliveries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        LoadDeliveries(String.Empty)

        Select Case FrmLogin.CurrentUser.Role?.ToLower()
            Case "staff"
                btnPos.Visible = False
                btnTransaction.Visible = False
                btnAuditTrail.Visible = False
        End Select

        RearrangeButtons(panelMenu)
    End Sub

    Private Sub RearrangeButtons(panel As Panel)
        Dim y As Integer = 10
        ' Only visible Buttons, keep order stable
        For Each ctrl As Control In panel.Controls.OfType(Of Button)().Where(Function(b) b.Visible)
            ctrl.Location = New Point(10, y)
            y += ctrl.Height + 10
        Next
    End Sub

    Private Function IsAdminUser() As Boolean
        Return String.Equals(FrmLogin.CurrentUser.Role, "admin", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub LoadDeliveries(searchText As String)
        Dim dt As New DataTable()
        Dim sql As New StringBuilder()
        sql.AppendLine("SELECT dp.DeliveryProductID,")
        sql.AppendLine("       d.DeliveryID,")
        sql.AppendLine("       d.OrderNumber,")
        sql.AppendLine("       d.DeliveryDate,")
        sql.AppendLine("       s.Company AS CompanyName,")
        sql.AppendLine("       p.Product AS ProductName,")
        sql.AppendLine("       p.BarcodeNumber,")
        sql.AppendLine("       dp.Quantity,")
        sql.AppendLine("       dp.CostPrice,")
        sql.AppendLine("       dp.Status,")
        sql.AppendLine("       p.SellingPrice,")
        sql.AppendLine("       p.ImagePath")
        sql.AppendLine("FROM tbl_delivery_products dp")
        sql.AppendLine("INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID")
        sql.AppendLine("INNER JOIN tbl_supplier s ON d.supplierID = s.supplierid")
        sql.AppendLine("INNER JOIN tbl_products p ON dp.ProductID = p.ProductID")
        sql.AppendLine("WHERE dp.Status = 'Pending'")
        sql.AppendLine("AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)")
        sql.AppendLine("ORDER BY p.DateCreated DESC;")

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql.ToString(), conn)
                    Dim param As New SqlParameter("@search", SqlDbType.VarChar, 150)
                    param.Value = If(String.IsNullOrWhiteSpace(searchText), String.Empty, "%" & searchText.Trim() & "%")
                    cmd.Parameters.Add(param)

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            EnsureProductImageColumn(dt)
            LoadImagesIntoDataTable(dt)

            DGVtable.DataSource = dt
            FormatDeliveriesGrid()
            DGVtable.ClearSelection()
        Catch ex As Exception
            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, $"Error loading deliveries: {ex.Message}")
            MessageBox.Show("An error occurred while loading deliveries. Check logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub EnsureProductImageColumn(dt As DataTable)
        If Not dt.Columns.Contains("ProductImage") Then
            dt.Columns.Add("ProductImage", GetType(Image))
        End If
    End Sub

    Private Sub LoadImagesIntoDataTable(dt As DataTable)
        For Each row As DataRow In dt.Rows
            Try
                Dim path As String = row("ImagePath")?.ToString()
                If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                    ' Avoid locking the original file by creating a separate Bitmap
                    Using tempImg As Image = Image.FromFile(path)
                        row("ProductImage") = New Bitmap(tempImg)
                    End Using
                Else
                    row("ProductImage") = Nothing
                End If
            Catch
                row("ProductImage") = Nothing
            End Try
        Next
    End Sub

    Private Sub FormatDeliveriesGrid()
        Dim hiddenCols() As String = {"ImagePath", "DeliveryProductID", "DeliveryID", "Status"}
        For Each colName In hiddenCols
            If DGVtable.Columns.Contains(colName) Then
                DGVtable.Columns(colName).Visible = False
            End If
        Next

        EnsureActionColumns()
        UpdatePostButtonStates()

        If DGVtable.Columns.Contains("ProductImage") Then
            DGVtable.Columns("ProductImage").DisplayIndex = 0
            Dim imgCol As DataGridViewImageColumn = DirectCast(DGVtable.Columns("ProductImage"), DataGridViewImageColumn)
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            imgCol.Width = 150
            imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If

        With DGVtable
            If .Columns.Contains("OrderNumber") Then .Columns("OrderNumber").HeaderText = "Order #"
            If .Columns.Contains("DeliveryDate") Then .Columns("DeliveryDate").HeaderText = "Date"
            If .Columns.Contains("CompanyName") Then .Columns("CompanyName").HeaderText = "Company"
            If .Columns.Contains("ProductName") Then .Columns("ProductName").HeaderText = "Product"
            If .Columns.Contains("Quantity") Then .Columns("Quantity").HeaderText = "Qty"
            If .Columns.Contains("CostPrice") Then .Columns("CostPrice").HeaderText = "Cost Price"
            If .Columns.Contains("SellingPrice") Then .Columns("SellingPrice").HeaderText = "Selling Price"
            If .Columns.Contains("CostPrice") Then
                .Columns("CostPrice").DefaultCellStyle.Format = "C2"
                .Columns("CostPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
            End If

            If .Columns.Contains("SellingPrice") Then
                .Columns("SellingPrice").DefaultCellStyle.Format = "C2"
                .Columns("SellingPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
            End If
        End With

        DGVtable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVtable.Columns
            If col.Name = "ProductImage" OrElse col.Name = ColViewEdit OrElse col.Name = ColPost OrElse col.Name = ColDelete Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next

        ApplyStandardGridLayout(DGVtable)

        DGVtable.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVtable.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVtable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
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

        If IsAdminUser() Then
            If Not DGVtable.Columns.Contains(ColPost) Then
                Dim postCol As New DataGridViewButtonColumn() With {
                    .Name = ColPost,
                    .HeaderText = "Post",
                    .UseColumnTextForButtonValue = False,
                    .Width = 100,
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
                }
                DGVtable.Columns.Add(postCol)
            End If
        ElseIf DGVtable.Columns.Contains(ColPost) Then
            DGVtable.Columns.Remove(ColPost)
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

    Private Sub UpdatePostButtonStates()
        If Not DGVtable.Columns.Contains(ColPost) Then Exit Sub

        For Each row As DataGridViewRow In DGVtable.Rows
            If row.IsNewRow Then Continue For

            Dim statusText As String = If(row.Cells("Status").Value, "").ToString().Trim()
            Dim isPosted As Boolean = String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase)
            Dim postCell As DataGridViewButtonCell = TryCast(row.Cells(ColPost), DataGridViewButtonCell)
            If postCell Is Nothing Then Continue For

            postCell.Value = If(isPosted, "Posted", "Post")
            postCell.ReadOnly = isPosted
            postCell.Style.ForeColor = If(isPosted, Color.DimGray, Color.Black)
            postCell.Style.BackColor = If(isPosted, Color.Gainsboro, Color.White)
        Next
    End Sub

    Private Sub DGVdeliveries_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtable.DataBindingComplete
        FormatDeliveriesGrid()
    End Sub


    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadDeliveries(txtSearch.Text)
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
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using f As New FrmDeliveryEntry()
            If f.ShowDialog() = DialogResult.OK Then
                LoadDeliveries(String.Empty)
            End If
        End Using
    End Sub

    Private Sub DGVdeliveries_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < DGVtable.Rows.Count Then
            Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
            Try
                Dim valDeliveryID = row.Cells("DeliveryID").Value
                Dim valDeliveryProductID = row.Cells("DeliveryProductID").Value

                DeliveryID = If(valDeliveryID IsNot Nothing AndAlso Not IsDBNull(valDeliveryID), Convert.ToInt32(valDeliveryID), -1)
                DeliveryProductID = If(valDeliveryProductID IsNot Nothing AndAlso Not IsDBNull(valDeliveryProductID), Convert.ToInt32(valDeliveryProductID), -1)
            Catch ex As Exception
                DeliveryID = -1
                DeliveryProductID = -1
            End Try
        End If
    End Sub

    ' Switch timer
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub

    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        Dim role As String = FrmLogin.CurrentUser.Role?.ToLower()

        If role = "staff" Then
            formtoshow = New FileMaintenance.Category()
        Else
            formtoshow = New FileMaintenance.User()
        End If

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

    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If DeliveryID = -1 Then
            MessageBox.Show("Please select a delivery first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        OpenEditModalById(DeliveryID)
    End Sub

    Private Sub OpenEditModalById(deliveryId As Integer)
        Using f As New FrmDeliveryEntry()
            f.deliveriesid = deliveryId
            If f.ShowDialog() = DialogResult.OK Then
                LoadDeliveries(String.Empty)
            End If
        End Using
    End Sub

    Private Sub DeleteDeliveryProductById(deliveryProductId As Integer)
        Dim result = MessageBox.Show("Are you sure you want to delete this delivery?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As SqlConnection = DataAccess.GetConnection()
                    Using cmd As New SqlCommand("DELETE FROM tbl_delivery_products WHERE DeliveryProductID = @DeliveryProductID", conn)
                        cmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Cancelled Deliveries.")
                MessageBox.Show("Delivery has been delete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDeliveries(String.Empty)
            Catch ex As Exception
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, $"Error cancelling delivery product: {ex.Message}")
                MessageBox.Show("Failed to cancel product. See log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub DGVdeliveries_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtable.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        If e.ColumnIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DGVtable.Rows(e.RowIndex)
        Dim deliveryId As Integer = Convert.ToInt32(row.Cells("DeliveryID").Value)
        Dim deliveryProductId As Integer = Convert.ToInt32(row.Cells("DeliveryProductID").Value)

        deliveryId = deliveryId
        deliveryProductId = deliveryProductId

        Dim colName As String = DGVtable.Columns(e.ColumnIndex).Name
        If colName = ColViewEdit Then
            OpenEditModalById(deliveryId)
        ElseIf colName = ColPost Then
            If Not IsAdminUser() Then
                MessageBox.Show("Only Admin users can post deliveries to inventory.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim statusText As String = If(row.Cells("Status").Value, "").ToString().Trim()
            If String.Equals(statusText, "Posted", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("This record is already posted.", "Already Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            PostDeliveryProductById(deliveryProductId)
        ElseIf colName = ColDelete Then
            DeleteDeliveryProductById(deliveryProductId)
        End If
    End Sub

    Private Sub PostDeliveryProductById(deliveryProductId As Integer)
        If Not IsAdminUser() Then
            MessageBox.Show("Only Admin users can post deliveries to inventory.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm As DialogResult = MessageBox.Show("Do you want to post this product to inventory?", "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using tran = conn.BeginTransaction()
                    ' Step 1: Get product info from pending delivery
                    Dim productID As Integer
                    Dim qtyToAdd As Integer
                    Dim newCostPrice As Decimal

                    Using getCmd As New SqlCommand("SELECT ProductID, Quantity, CostPrice FROM tbl_delivery_products WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending'", conn, tran)
                        getCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                        Using rdr As SqlDataReader = getCmd.ExecuteReader()
                            If rdr.Read() Then
                                productID = Convert.ToInt32(rdr("ProductID"))
                                qtyToAdd = Convert.ToInt32(rdr("Quantity"))
                                newCostPrice = Convert.ToDecimal(rdr("CostPrice"))
                            Else
                                MessageBox.Show("This record is already posted or no longer available for posting.", "Already Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                tran.Rollback()
                                Return
                            End If
                        End Using
                    End Using

                    ' Step 2: Check for existing Posted record for same product
                    Dim postedDeliveryProductID As Object
                    Using checkCmd As New SqlCommand("SELECT TOP 1 DeliveryProductID FROM tbl_delivery_products WHERE ProductID = @ProductID AND Status = 'Posted'", conn, tran)
                        checkCmd.Parameters.Add(New SqlParameter("@ProductID", SqlDbType.Int) With {.Value = productID})
                        postedDeliveryProductID = checkCmd.ExecuteScalar()
                    End Using

                    If postedDeliveryProductID IsNot Nothing AndAlso Not IsDBNull(postedDeliveryProductID) Then
                        Dim postedID = Convert.ToInt32(postedDeliveryProductID)
                        Dim oldQty As Integer = 0
                        Dim oldCostPrice As Decimal = 0D

                        Using cmdGet As New SqlCommand("SELECT Quantity, CostPrice FROM tbl_delivery_products WHERE DeliveryProductID = @PostedID", conn, tran)
                            cmdGet.Parameters.Add(New SqlParameter("@PostedID", SqlDbType.Int) With {.Value = postedID})
                            Using rdr As SqlDataReader = cmdGet.ExecuteReader()
                                If rdr.Read() Then
                                    oldQty = Convert.ToInt32(rdr("Quantity"))
                                    oldCostPrice = Convert.ToDecimal(rdr("CostPrice"))
                                End If
                            End Using
                        End Using

                        Dim totalQty As Integer = oldQty + qtyToAdd
                        Dim weightedCost As Decimal

                        If totalQty > 0 Then
                            weightedCost = Math.Round(((oldCostPrice * oldQty) + (newCostPrice * qtyToAdd)) / totalQty, 2)
                        Else
                            ' fallback
                            weightedCost = newCostPrice
                        End If

                        Using updateCmd As New SqlCommand("UPDATE tbl_delivery_products SET Quantity = @TotalQty, CostPrice = @WeightedCost, DateUpdated = GETDATE() WHERE DeliveryProductID = @PostedID", conn, tran)
                            updateCmd.Parameters.Add(New SqlParameter("@TotalQty", SqlDbType.Int) With {.Value = totalQty})
                            updateCmd.Parameters.Add(New SqlParameter("@WeightedCost", SqlDbType.Decimal) With {.Value = weightedCost})
                            updateCmd.Parameters.Add(New SqlParameter("@PostedID", SqlDbType.Int) With {.Value = postedID})
                            updateCmd.ExecuteNonQuery()
                        End Using

                        Using delCmd As New SqlCommand("DELETE FROM tbl_delivery_products WHERE DeliveryProductID = @PendingID", conn, tran)
                            delCmd.Parameters.Add(New SqlParameter("@PendingID", SqlDbType.Int) With {.Value = deliveryProductId})
                            delCmd.ExecuteNonQuery()
                        End Using
                    Else
                        ' No existing posted record — mark current as Posted
                        Using postCmd As New SqlCommand("UPDATE tbl_delivery_products SET Status = 'Posted', DateUpdated = GETDATE() WHERE DeliveryProductID = @DeliveryProductID AND Status = 'Pending'", conn, tran)
                            postCmd.Parameters.Add(New SqlParameter("@DeliveryProductID", SqlDbType.Int) With {.Value = deliveryProductId})
                            Dim affected As Integer = postCmd.ExecuteNonQuery()
                            If affected = 0 Then
                                MessageBox.Show("This record is already posted.", "Already Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                tran.Rollback()
                                Return
                            End If
                        End Using
                    End If

                    tran.Commit()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Posted delivery with cost merge.")
            MessageBox.Show("Product successfully posted and cost price updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadDeliveries(String.Empty)
        Catch ex As Exception
            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, $"Error posting delivery product: {ex.Message}")
            MessageBox.Show("Failed to post product. See log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then

            Try
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    SessionService.EndCurrentSession("Logout")
                End Using
            Catch ex As Exception
                MsgBox("Error logging out: " & ex.Message)
            End Try

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "User Logged Out.")

            FrmLogin.CurrentUser.UserID = 0
            FrmLogin.CurrentUser.Username = ""
            FrmLogin.CurrentUser.Role = ""
            FrmLogin.CurrentUser.FullName = ""

            Me.Hide()
            Dim f As New FrmLogin()
            f.Show()
        End If
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
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
End Class

