Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Text
Imports JJAS_ISBST.FrmLogin

Public Class Admin_Pos
    Private Const CartEditButtonName As String = "colCartEdit"
    Private Const CartRemoveButtonName As String = "colCartRemove"
    Private preferredPrinter As String = "XP-58"
    Private receiptFont As New Font("Consolas", 10)
    Private WithEvents printDoc As New PrintDocument
    Private receiptText As String
    Dim formtoshow As Form
    Dim Costprice As Decimal = 0D
    Dim category As String = ""
    Private dtProducts As DataTable   ' Products loaded from DB (Posted only)
    Private dtPending As DataTable    ' POS cart



    Private Sub Admin_pos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ps As New PaperSize("Receipt", 200, 900)
            printDoc.DefaultPageSettings.PaperSize = ps
            printDoc.DefaultPageSettings.Margins = New Margins(5, 5, 10, 10)

            DGVCart.ClearSelection()
            BlockCopyPaste(txtSearch)
            EnsureDtPending()
            displayData("")
            LoadDiscounts()
            UpdateTotals()
            datetimer.Start()
            lblTransactionNo.Text = GenerateTransactionNo()
            lblDate.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")

            Select Case FrmLogin.CurrentUser.Role.ToLower()
                Case "cashier"
                    btnFileMaintenance.Visible = False
                    btnDelivery.Visible = False
                    btnInventory.Visible = False
                    btnAuditTrail.Visible = False
                    btnAuditTrail.Image = Nothing
                    btnFileMaintenance.Image = Nothing
                    btnDelivery.Image = Nothing
                    btnInventory.Image = Nothing
            End Select

            RearrangeButtons(panelmenu)
        Catch ex As Exception
            MessageBox.Show("Error during POS load: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RestoreAllPendingItems()
        Try
            If dtPending Is Nothing OrElse dtPending.Rows.Count = 0 Then Exit Sub

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()

                For Each row As DataRow In dtPending.Rows
                    Dim deliveryProductID As Integer = Convert.ToInt32(row("DeliveryProductID"))
                    Dim qtyToRestore As Integer = Convert.ToInt32(row("Quantity"))

                    ' ✅ Restore stock
                    Using cmd As New SqlCommand("
                    UPDATE tbl_Delivery_Products
                    SET Quantity = Quantity + @qty
                    WHERE DeliveryProductID = @id", conn)
                        cmd.Parameters.AddWithValue("@qty", qtyToRestore)
                        cmd.Parameters.AddWithValue("@id", deliveryProductID)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using

            ' ✅ Clear the cart after restoring
            dtPending.Rows.Clear()
            DGVCart.Refresh()
            UpdateTotals()

        Catch ex As Exception
            MessageBox.Show("Error restoring stock: " & ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
    Private Sub EnsureDtPending()
        If dtPending Is Nothing Then
            dtPending = New DataTable()
            dtPending.Columns.Add("DeliveryProductID", GetType(Integer))
            dtPending.Columns.Add("ProductID", GetType(Integer))
            dtPending.Columns.Add("BarcodeNumber", GetType(String))
            dtPending.Columns.Add("ProductName", GetType(String))
            dtPending.Columns.Add("Quantity", GetType(Integer))
            dtPending.Columns.Add("SellingPrice", GetType(Decimal))
            dtPending.Columns.Add("ImagePath", GetType(String))
            dtPending.Columns.Add("ProductImage", GetType(Image))
        End If
    End Sub

    ' Show available Posted products
    Private Sub displayData(searchText As String)
        Try
            dtProducts = New DataTable()

            Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   dp.ProductID,
                   d.OrderNumber,
                   p.Product AS ProductName,
                   p.BarcodeNumber,
                   dp.Quantity,
                   p.SellingPrice,
                   p.ImagePath,
                   dp.DateUpdated
            FROM tbl_delivery_products dp
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            WHERE dp.Status = 'Posted'
              AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
            ORDER BY d.DateCreated DESC;"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dtProducts)
                End Using
            End Using

            ' Add ProductImage column
            If Not dtProducts.Columns.Contains("ProductImage") Then
                dtProducts.Columns.Add("ProductImage", GetType(Image))
            End If

            ' Load product images
            For Each row As DataRow In dtProducts.Rows
                Dim path As String = row("ImagePath").ToString()
                If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                    Using tempImg As Image = Image.FromFile(path)
                        row("ProductImage") = New Bitmap(tempImg)
                    End Using
                Else
                    row("ProductImage") = Nothing
                End If
            Next

            ' Bind to product grid
            DGVCart.DataSource = dtProducts

            ' Hide unnecessary columns
            Dim hiddenCols() As String = {"ImagePath", "DeliveryProductID", "ProductID"}
            For Each colName In hiddenCols
                If DGVCart.Columns.Contains(colName) Then
                    DGVCart.Columns(colName).Visible = False
                End If
            Next

            ' Image column settings
            If DGVCart.Columns.Contains("ProductImage") Then
                Dim imgCol As DataGridViewImageColumn = DirectCast(DGVCart.Columns("ProductImage"), DataGridViewImageColumn)
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                imgCol.Width = 120
                imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                imgCol.DisplayIndex = 0
            End If

            ' Headers
            With DGVCart
                .Columns("OrderNumber").HeaderText = "Order #"
                .Columns("ProductName").HeaderText = "Product"
                .Columns("Quantity").HeaderText = "Stock Qty"
                .Columns("SellingPrice").HeaderText = "Price"
                .Columns("SellingPrice").DefaultCellStyle.Format = "C2"
                .Columns("SellingPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
            End With
        Catch ex As Exception
            MessageBox.Show("Error during POS load: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        DGVCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGVCart.ClearSelection()

        DGVCart.DataSource = dtPending
        ConfigureCartGridColumns()
        EnsureCartActionColumns()
        ApplyStandardGridLayout(DGVCart)
    End Sub

    ' Format product list
    Private Sub DGVdeliveries_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVCart.DataBindingComplete
        DGVCart.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        DGVCart.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
        DGVCart.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ConfigureCartGridColumns()
        EnsureCartActionColumns()
        ApplyStandardGridLayout(DGVCart)
    End Sub

    Private Sub ConfigureCartGridColumns()
        If DGVCart.Columns.Count = 0 Then Exit Sub

        DGVCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGVCart.MultiSelect = False
        DGVCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim hiddenCols() As String = {"ImagePath", "DeliveryProductID", "ProductID"}
        For Each colName In hiddenCols
            If DGVCart.Columns.Contains(colName) Then
                DGVCart.Columns(colName).Visible = False
            End If
        Next

        If DGVCart.Columns.Contains("ProductImage") Then
            Dim imgCol As DataGridViewImageColumn = TryCast(DGVCart.Columns("ProductImage"), DataGridViewImageColumn)
            If imgCol IsNot Nothing Then
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                imgCol.Width = 120
                imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                imgCol.DisplayIndex = 0
            End If
        End If

        If DGVCart.Columns.Contains("BarcodeNumber") Then
            DGVCart.Columns("BarcodeNumber").HeaderText = "Barcode"
        End If

        If DGVCart.Columns.Contains("ProductName") Then
            DGVCart.Columns("ProductName").HeaderText = "Product"
        End If

        If DGVCart.Columns.Contains("Quantity") Then
            DGVCart.Columns("Quantity").HeaderText = "Qty"
            DGVCart.Columns("Quantity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        If DGVCart.Columns.Contains("SellingPrice") Then
            DGVCart.Columns("SellingPrice").HeaderText = "Price"
            DGVCart.Columns("SellingPrice").DefaultCellStyle.Format = "C2"
            DGVCart.Columns("SellingPrice").DefaultCellStyle.FormatProvider = New Globalization.CultureInfo("en-PH")
            DGVCart.Columns("SellingPrice").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
    End Sub

    Private Sub EnsureCartActionColumns()
        If Not DGVCart.Columns.Contains(CartEditButtonName) Then
            Dim editCol As New DataGridViewButtonColumn() With {
                .Name = CartEditButtonName,
                .HeaderText = "",
                .Text = "Edit",
                .UseColumnTextForButtonValue = True,
                .ReadOnly = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .SortMode = DataGridViewColumnSortMode.NotSortable,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVCart.Columns.Add(editCol)
        End If

        If Not DGVCart.Columns.Contains(CartRemoveButtonName) Then
            Dim removeCol As New DataGridViewButtonColumn() With {
                .Name = CartRemoveButtonName,
                .HeaderText = "",
                .Text = "Remove",
                .UseColumnTextForButtonValue = True,
                .ReadOnly = True,
                .Width = 100,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .SortMode = DataGridViewColumnSortMode.NotSortable,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            }
            DGVCart.Columns.Add(removeCol)
        End If
    End Sub

    Private Sub RefreshCartAfterAction()
        ApplyStandardGridLayout(DGVCart)
        DGVCart.Refresh()
        DGVCart.ClearSelection()
        UpdateTotals()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            EnsureDtPending()

            Dim existingRow As DataRow = Nothing
            Dim f As New add_pos
            f.IsEditMode = False

            If f.ShowDialog() = DialogResult.OK Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()

                    ' === Start SQL Transaction ===
                    Dim transaction As SqlTransaction = conn.BeginTransaction()

                    Try
                        ' --- Step 1: Check current stock ---
                        Dim checkCmd As New SqlCommand("
                        SELECT Quantity 
                        FROM tbl_Delivery_Products 
                        WHERE DeliveryProductID = @id", conn, transaction)
                        checkCmd.Parameters.AddWithValue("@id", f.selectedID)
                        Dim currentQty As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If currentQty < f.SelectedQuantity Then
                            Throw New Exception("Not enough stock available for this item.")
                        End If

                        ' --- Step 2: Deduct stock immediately ---
                        Dim updateCmd As New SqlCommand("
                        UPDATE tbl_Delivery_Products 
                        SET Quantity = Quantity - @qty 
                        WHERE DeliveryProductID = @id", conn, transaction)
                        updateCmd.Parameters.AddWithValue("@qty", f.SelectedQuantity)
                        updateCmd.Parameters.AddWithValue("@id", f.selectedID)
                        updateCmd.ExecuteNonQuery()

                        ' --- Step 3: Commit changes to DB ---
                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw New Exception("Failed to add product: " & ex.Message)
                    End Try
                End Using

                ' === Step 4: Update POS cart ===
                Dim imgPath As String = f.SelectedImagePath
                Dim productImage As Image = Nothing

                If Not String.IsNullOrEmpty(imgPath) AndAlso IO.File.Exists(imgPath) Then
                    Using tempImg As Image = Image.FromFile(imgPath)
                        productImage = New Bitmap(tempImg)
                    End Using
                End If

                ' Check if already in cart
                For Each row As DataRow In dtPending.Rows
                    If Convert.ToInt32(row("DeliveryProductID")) = f.selectedID Then
                        existingRow = row
                        Exit For
                    End If
                Next

                If existingRow IsNot Nothing Then
                    existingRow("Quantity") = Convert.ToInt32(existingRow("Quantity")) + f.SelectedQuantity
                Else
                    dtPending.Rows.Add(
                    f.selectedID,
                    f.ProductID,
                    f.SelectedBarcode,
                    f.SelectedProductName,
                    f.SelectedQuantity,
                    f.SelectedSellingPrice,
                    imgPath,
                    productImage
                )
                End If

                Costprice = f.selectedCostPrice
                category = f.SelectedCategory
                MessageBox.Show("Product added and stock updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Add Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        txtCash.Text = ""
        txtChange.Text = "0.00"
        DGVCart.DataSource = dtPending
        DGVCart.Refresh()
        DGVCart.ClearSelection()
        UpdateTotals()
        displayData("") ' 🔄 Refresh inventory grid for all
    End Sub

    Private Sub EditCartItemQuantity(deliveryProductID As Integer)
        Try
            If dtPending Is Nothing Then Exit Sub

            Dim matchedRows As DataRow() = dtPending.Select("DeliveryProductID = " & deliveryProductID.ToString())
            If matchedRows.Length = 0 Then
                MessageBox.Show("Selected cart item was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim cartRow As DataRow = matchedRows(0)
            Dim currentQty As Integer = Convert.ToInt32(cartRow("Quantity"))

            Dim inputQty As String = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the new quantity for this item:",
                "Edit Cart Quantity",
                currentQty.ToString()
            ).Trim()

            If String.IsNullOrWhiteSpace(inputQty) Then Exit Sub

            Dim newQty As Integer
            If Not Integer.TryParse(inputQty, newQty) OrElse newQty <= 0 Then
                MessageBox.Show("Quantity must be a whole number greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If newQty = currentQty Then Exit Sub

            Dim qtyDelta As Integer = newQty - currentQty
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Using txn As SqlTransaction = conn.BeginTransaction()
                    Try
                        If qtyDelta > 0 Then
                            Dim availableStock As Integer = 0
                            Using checkCmd As New SqlCommand("
                                SELECT Quantity
                                FROM tbl_Delivery_Products
                                WHERE DeliveryProductID = @id", conn, txn)
                                checkCmd.Parameters.AddWithValue("@id", deliveryProductID)
                                Dim result As Object = checkCmd.ExecuteScalar()
                                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                                    availableStock = Convert.ToInt32(result)
                                End If
                            End Using

                            If availableStock < qtyDelta Then
                                Throw New Exception("Not enough stock available for the requested quantity.")
                            End If

                            Using deductCmd As New SqlCommand("
                                UPDATE tbl_Delivery_Products
                                SET Quantity = Quantity - @qty
                                WHERE DeliveryProductID = @id", conn, txn)
                                deductCmd.Parameters.AddWithValue("@qty", qtyDelta)
                                deductCmd.Parameters.AddWithValue("@id", deliveryProductID)
                                deductCmd.ExecuteNonQuery()
                            End Using
                        Else
                            Dim restoreQty As Integer = Math.Abs(qtyDelta)
                            Using restoreCmd As New SqlCommand("
                                UPDATE tbl_Delivery_Products
                                SET Quantity = Quantity + @qty
                                WHERE DeliveryProductID = @id", conn, txn)
                                restoreCmd.Parameters.AddWithValue("@qty", restoreQty)
                                restoreCmd.Parameters.AddWithValue("@id", deliveryProductID)
                                restoreCmd.ExecuteNonQuery()
                            End Using
                        End If

                        txn.Commit()
                    Catch
                        txn.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            cartRow("Quantity") = newQty
            RefreshCartAfterAction()
            MessageBox.Show("Cart item updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error editing product: " & ex.Message, "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadDiscounts()
        Try
            Dim dt As New DataTable()
        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "SELECT DiscountID, DiscountName, DiscountValue FROM tbl_Discount WHERE IsActive = 1"
            Using cmd As New SqlCommand(sql, conn)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        ' Insert default "No Discount"
        Dim row As DataRow = dt.NewRow()
        row("DiscountID") = 0
        row("DiscountName") = "No Discount"
        row("DiscountValue") = 0D
        dt.Rows.InsertAt(row, 0)

        cbdiscount.DataSource = dt
        cbdiscount.DisplayMember = "DiscountName"
        cbdiscount.ValueMember = "DiscountID"
            cbdiscount.SelectedIndex = 0   ' ✅ Default to "No Discount"
        Catch ex As Exception
            MessageBox.Show("Error discount load!: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub UpdateTotals()
        ' Ensure cart exists
        If dtPending Is Nothing OrElse dtPending.Rows.Count = 0 Then
            txtSubtotal.Text = "0.00"
            txtVatable.Text = "0.00"
            txtVat.Text = "0.00"
            txtTotal.Text = "0.00"
            txtDiscountAmt.Text = "0.00"
            txtVatRate.Text = "0.00"
            Return
        End If

        ' --- Subtotal ---
        Dim subtotal As Decimal = 0D
        For Each row As DataRow In dtPending.Rows
            subtotal += Convert.ToDecimal(row("Quantity")) * Convert.ToDecimal(row("SellingPrice"))
        Next

        ' --- Load VAT rate ---
        Dim vatRate As Decimal = 0.12D ' default 12%
        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "SELECT TOP 1 Vat_Rate FROM tbl_Vat"
            Using cmd As New SqlCommand(sql, conn)
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    vatRate = Convert.ToDecimal(result)
                    txtVatRate.Text = vatRate
                    If vatRate > 1D Then vatRate = vatRate / 100D
                End If
            End Using
        End Using

        ' --- Load Discount rate ---
        Dim discountRate As Decimal = 0D
        If cbdiscount.SelectedValue IsNot Nothing Then
            Dim selectedDiscountID As Integer
            If Integer.TryParse(cbdiscount.SelectedValue.ToString(), selectedDiscountID) AndAlso selectedDiscountID <> 0 Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    Dim sql As String = "SELECT DiscountValue FROM tbl_Discount WHERE DiscountID = @id"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@id", selectedDiscountID)
                        conn.Open()
                        Dim result = cmd.ExecuteScalar()
                        If result IsNot Nothing Then
                            discountRate = Convert.ToDecimal(result)
                            If discountRate > 1D Then discountRate = discountRate / 100D
                        End If
                    End Using
                End Using
            End If
        End If

        ' --- Apply discount ---
        Dim discountAmt As Decimal = Math.Round(subtotal * discountRate, 2)
        Dim discountedSubtotal As Decimal = subtotal - discountAmt  ' VAT still included

        ' --- VAT breakdown ---
        Dim vatable As Decimal = Math.Round(discountedSubtotal / (1D + vatRate), 2)
        Dim vatAmt As Decimal = Math.Round(discountedSubtotal - vatable, 2)

        ' --- Update UI ---
        txtSubtotal.Text = subtotal.ToString("N2")
        txtDiscountAmt.Text = discountAmt.ToString("N2")   ' ✅ Show discount amount
        txtVatable.Text = vatable.ToString("N2")
        txtVat.Text = vatAmt.ToString("N2")
        txtTotal.Text = discountedSubtotal.ToString("N2")
    End Sub

    ' --- PAY BUTTON CLICK ---
    Private Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        If dtPending.Rows.Count = 0 Then
            MessageBox.Show("Cart is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim cash As Decimal
        If Not Decimal.TryParse(txtCash.Text, cash) OrElse cash <= 0 Then
            MessageBox.Show("Enter valid cash amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim total As Decimal = Convert.ToDecimal(txtTotal.Text)
        If cash < total Then
            MessageBox.Show("Cash is not enough.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        txtChange.Text = (cash - total).ToString("N2")

        ' ✅ Deduct purchased quantity from stock
        Using conn As SqlConnection = DataAccess.GetConnection()
            conn.Open()

            For Each row As DataGridViewRow In DGVCart.Rows
                If Not row.IsNewRow Then
                    Dim id As Integer = Convert.ToInt32(row.Cells("DeliveryProductID").Value)
                    Dim purchasedQty As Integer = Convert.ToInt32(row.Cells("Quantity").Value)

                    ' First, get current stock
                    Dim currentQty As Integer = 0
                    Using cmdGet As New SqlCommand("SELECT Quantity FROM tbl_Delivery_Products WHERE DeliveryProductID=@DeliveryProductID", conn)
                        cmdGet.Parameters.AddWithValue("@DeliveryProductID", id)
                        Dim result = cmdGet.ExecuteScalar()
                        If result IsNot Nothing Then
                            currentQty = Convert.ToInt32(result)
                        End If
                    End Using

                    ' Deduct purchased quantity
                    Dim newQty As Integer = currentQty - purchasedQty
                    If newQty < 0 Then newQty = 0 ' Prevent negative stock

                    ' Update stock
                    Using cmdUpdate As New SqlCommand("UPDATE tbl_Delivery_Products SET Quantity=@Quantity WHERE DeliveryProductID=@DeliveryProductID", conn)
                        cmdUpdate.Parameters.AddWithValue("@Quantity", newQty)
                        cmdUpdate.Parameters.AddWithValue("@DeliveryProductID", id)
                        cmdUpdate.ExecuteNonQuery()
                    End Using
                    ' After deducting stock in your POS payment code
                    If newQty <= 0 Then
                        Using cmdDelete As New SqlCommand("DELETE FROM tbl_Delivery_Products WHERE DeliveryProductID=@id", conn)
                            cmdDelete.Parameters.AddWithValue("@id", id)
                            cmdDelete.ExecuteNonQuery()
                        End Using
                    End If

                End If
            Next
        End Using

        If MessageBox.Show("Do you want to Print the Receipt?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                printDoc.PrinterSettings.PrinterName = preferredPrinter

                If Not printDoc.PrinterSettings.IsValid Then
                    MessageBox.Show($"Printer '{preferredPrinter}' not found. Please check your printer settings.",
                                    "Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                PreviewReceipt()
                MessageBox.Show("Payment successful! (Previewed)", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information)


            Catch ex As Exception
                MessageBox.Show("Printing failed: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If


        Dim vatRate As Decimal = 0.12D ' default 12%
        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "SELECT TOP 1 Vat_Rate FROM tbl_Vat"
            Using cmd As New SqlCommand(sql, conn)
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    vatRate = Convert.ToDecimal(result)
                    If vatRate > 1D Then vatRate = vatRate / 100D
                End If
            End Using
        End Using

        Try
            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                For Each row As DataGridViewRow In DGVCart.Rows
                    If Not row.IsNewRow Then
                        Dim query As String = "
                INSERT INTO tbl_SalesHistory 
                    (Name, ProductName, BarcodeNumber, Quantity, SellingPrice, TotalAmount, Discount, VatRate, VatAmount, Vatable, TransactionNo,Category, CostPrice, SaleDate)
                VALUES 
                    (@Name, @ProductName, @BarcodeNumber, @Quantity, @Price, @Total, @Discount, @VatRate, @VatAmount, @Vatable, @TransactionNo, @Category, @CostPrice, GETDATE())"
                        Using cmd As New SqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@Name", FrmLogin.CurrentUser.FullName)
                            cmd.Parameters.AddWithValue("@ProductName", row.Cells("ProductName").Value.ToString())
                            cmd.Parameters.AddWithValue("@BarcodeNumber", row.Cells("BarcodeNumber").Value.ToString())
                            cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(row.Cells("Quantity").Value))
                            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(row.Cells("SellingPrice").Value))
                            cmd.Parameters.AddWithValue("@Total", Convert.ToDecimal(row.Cells("Quantity").Value) * Convert.ToDecimal(row.Cells("SellingPrice").Value))
                            cmd.Parameters.AddWithValue("@Discount", Convert.ToDecimal(txtDiscountAmt.Text))
                            cmd.Parameters.AddWithValue("@VatRate", vatRate)
                            cmd.Parameters.AddWithValue("@VatAmount", Convert.ToDecimal(txtVat.Text))
                            cmd.Parameters.AddWithValue("@Vatable", Convert.ToDecimal(txtVatable.Text))
                            cmd.Parameters.AddWithValue("@TransactionNo", lblTransactionNo.Text)
                            cmd.Parameters.AddWithValue("@CostPrice", Convert.ToDecimal(Costprice))
                            cmd.Parameters.AddWithValue("@Category", category.ToString)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving transaction!: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Completed sale.")


        dtPending.Rows.Clear()
        DGVCart.Refresh()
        UpdateTotals()
        txtCash.Text = ""
        txtChange.Text = "0.00"

        lblTransactionNo.Text = GenerateTransactionNo()
    End Sub
    Private Sub PreviewReceipt()
        Try
            ' Create a print preview dialog
            Using preview As New PrintPreviewDialog
                preview.Document = printDoc
                preview.Width = 600
                preview.Height = 600
                preview.StartPosition = FormStartPosition.CenterScreen
                preview.PrintPreviewControl.Zoom = 1.0 ' 100% zoom

                preview.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show("Preview failed: " & ex.Message, "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
        Try
            ' === Fonts (use monospaced para pantay) ===
            Dim fontHeader As New Font("Courier New", 9, FontStyle.Bold)
            Dim fontBody As New Font("Courier New", 8, FontStyle.Regular)
            Dim fontSmall As New Font("Courier New", 7, FontStyle.Regular)
            Dim fontBold As New Font("Courier New", 8, FontStyle.Bold)

            Dim y As Integer = 5
            Dim left As Integer = 5
            Dim lineHeight As Integer = CInt(fontBody.GetHeight(e.Graphics)) + 2
            Dim formatRight As New StringFormat() With {.Alignment = StringAlignment.Far}
            Dim pageWidth As Integer = e.PageBounds.Width - 10

            ' === HEADER ===
            e.Graphics.DrawString("JJOM'S APPAREL STORE", fontHeader, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("#03 Caliao St., Taguig City", fontSmall, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("Contact No: 0912-345-6789", fontSmall, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString(New String("="c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight

            ' === TRANSACTION INFO ===
            e.Graphics.DrawString("DATE: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("TRANS#: " & lblTransactionNo.Text, fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("CASHIER: " & FrmLogin.CurrentUser.FullName, fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString(New String("-"c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight

            ' === PRODUCT HEADER ===
            e.Graphics.DrawString("ITEM".PadRight(5) & "/ " & "QTY".PadRight(0) & " / " & "Price".PadRight(5), fontSmall, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString(New String("-"c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight

            ' === PRODUCT LIST ===
            For Each row As DataRow In dtPending.Rows
                Dim name As String = row("ProductName").ToString()
                Dim qty As Integer = CInt(row("Quantity"))
                Dim price As Decimal = CDec(row("SellingPrice"))

                ' Limit name length if too long
                If name.Length > 18 Then name = name.Substring(0, 18)

                ' Format line as: ProductName(Qty)  phpPrice
                Dim line As String = $"{name}({qty})  {price:N2}"

                e.Graphics.DrawString(line, fontSmall, Brushes.Black, left, y)
                y += lineHeight
            Next

            ' === TOTALS ===
            y += 1
            e.Graphics.DrawString(New String("="c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight

            e.Graphics.DrawString("SUBTOTAL:".PadRight(10) & txtSubtotal.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("DISCOUNT:".PadRight(10) & txtDiscountAmt.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("VAT RATE:".PadRight(10) & txtVatRate.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("VATABLE:".PadRight(10) & txtVatable.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("VAT:".PadRight(10) & txtVat.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight

            e.Graphics.DrawString(New String("-"c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight

            e.Graphics.DrawString("TOTAL:".PadRight(10) & txtTotal.Text.PadLeft(15), fontBold, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("CASH:".PadRight(10) & txtCash.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("CHANGE:".PadRight(10) & txtChange.Text.PadLeft(15), fontBody, Brushes.Black, left, y)
            y += lineHeight

            ' === FOOTER ===
            e.Graphics.DrawString(New String("="c, 42), fontSmall, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("SOLD TO: ________________", fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("ADDRESS: ________________", fontBody, Brushes.Black, left, y)
            y += lineHeight
            e.Graphics.DrawString("* THIS IS YOUR INVOICE *", fontBody, Brushes.Black, left + 20, y)
            y += lineHeight
            e.Graphics.DrawString("THANK YOU FOR SHOPPING!", fontBody, Brushes.Black, left + 15, y)

            e.HasMorePages = False

        Catch ex As Exception
            MessageBox.Show("Error during print: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Admin_Pos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If dtPending IsNot Nothing AndAlso dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If
    End Sub

    Private Sub RemoveCartItem(deliveryProductID As Integer)
        Try
            If dtPending Is Nothing Then Exit Sub

            Dim matchedRows As DataRow() = dtPending.Select("DeliveryProductID = " & deliveryProductID.ToString())
            If matchedRows.Length = 0 Then
                MessageBox.Show("Selected cart item was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim rowToRemove As DataRow = matchedRows(0)
            Dim qtyToRestore As Integer = Convert.ToInt32(rowToRemove("Quantity"))

            Dim confirmDelete As DialogResult = MessageBox.Show(
                "Are you sure you want to remove this item from cart?",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

            If confirmDelete = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()

                    Using cmd As New SqlCommand("
                        UPDATE tbl_Delivery_Products
                        SET Quantity = Quantity + @qty
                        WHERE DeliveryProductID = @id", conn)
                        cmd.Parameters.AddWithValue("@qty", qtyToRestore)
                        cmd.Parameters.AddWithValue("@id", deliveryProductID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                dtPending.Rows.Remove(rowToRemove)
                RefreshCartAfterAction()
                displayData("")
                MessageBox.Show("Product removed and stock restored.", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error removing product: " & ex.Message, "Remove Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DGVCart_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVCart.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim clickedColName As String = DGVCart.Columns(e.ColumnIndex).Name
        If Not String.Equals(clickedColName, CartEditButtonName, StringComparison.Ordinal) AndAlso
           Not String.Equals(clickedColName, CartRemoveButtonName, StringComparison.Ordinal) Then
            Exit Sub
        End If

        If Not DGVCart.Columns.Contains("DeliveryProductID") Then
            MessageBox.Show("Unable to locate row ID for this cart item.", "Missing ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim idValue As Object = DGVCart.Rows(e.RowIndex).Cells("DeliveryProductID").Value
        Dim deliveryProductID As Integer
        If idValue Is Nothing OrElse Not Integer.TryParse(idValue.ToString(), deliveryProductID) Then
            MessageBox.Show("Invalid row ID. Please refresh and try again.", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.Equals(clickedColName, CartEditButtonName, StringComparison.Ordinal) Then
            EditCartItemQuantity(deliveryProductID)
        ElseIf String.Equals(clickedColName, CartRemoveButtonName, StringComparison.Ordinal) Then
            RemoveCartItem(deliveryProductID)
        End If
    End Sub


    Private Sub btnNewTransaction_Click(sender As Object, e As EventArgs) Handles btnNewTransaction.Click
        DGVCart.Refresh()
        UpdateTotals()

        ' Reset cash/change
        txtCash.Text = ""
        txtChange.Text = "0.00"
        DGVCart.ClearSelection()
        ' Generate new date & transaction no for next sale
        lblDate.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
        lblTransactionNo.Text = GenerateTransactionNo()
    End Sub


    Private Sub txtCash_TextChanged(sender As Object, e As EventArgs) Handles txtCash.TextChanged
        ' If textbox is empty, reset change
        If String.IsNullOrWhiteSpace(txtCash.Text) Then
            txtChange.Text = "0.00"
            Exit Sub
        End If

        Dim cash As Decimal
        Dim total As Decimal = 0D

        ' Parse values safely
        Decimal.TryParse(txtCash.Text, cash)
        Decimal.TryParse(txtTotal.Text, total)

        ' Validation
        If cash < total Then
            txtChange.Text = "0.00"
            ' Optional: show error (only when user actually typed a number)
            If txtCash.Focused AndAlso txtCash.Text <> "" Then
                txtChange.Text = "Cash not enough"
            End If
        Else
            ' Compute change
            Dim changeAmt As Decimal = cash - total
            txtChange.Text = changeAmt.ToString("N2")
        End If
    End Sub
    ' Generate Transaction No (example: YYYYMMDDHHMMSS)
    ' 🧩 Keeps POS form focused and visible during printing
    Private Sub KeepFormOnTopDuringPrint()
        ' Remember previous state
        Dim wasTopMost As Boolean = Me.TopMost
        Dim wasMinimized As Boolean = (Me.WindowState = FormWindowState.Minimized)

        Try
            ' Force window to stay on top
            Me.TopMost = True
            ' If minimized, restore it before printing
            If wasMinimized Then
                Me.WindowState = FormWindowState.Normal
            End If

            ' 🔹 Print silently
            printDoc.Print()

            ' Restore position & focus after printing
            Me.TopMost = wasTopMost
            Me.WindowState = FormWindowState.Normal
            Me.Activate()



        Catch ex As Exception
            Me.TopMost = wasTopMost
            MessageBox.Show("Printing failed: " & ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub
    ' 🕒 Ensures the POS form always regains focus after background events
    Private Sub RestoreFocusAfterPrint()
        Dim restoreTimer As New Timer()
        restoreTimer.Interval = 1000 ' 1 second
        AddHandler restoreTimer.Tick, Sub()
                                          If Me.WindowState = FormWindowState.Minimized Then
                                              Me.WindowState = FormWindowState.Normal
                                          End If
                                          Me.Activate()
                                          Me.TopMost = False
                                          restoreTimer.Stop()
                                      End Sub
        restoreTimer.Start()
    End Sub

    Private Function GenerateTransactionNo() As String
        Return DateTime.Now.ToString("yyyyMMddHHmmss")
    End Function

    Private Sub cbdiscount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbdiscount.SelectedIndexChanged
        UpdateTotals()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub
    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' stop beep

            Dim barcode As String = txtSearch.Text.Trim()
            If barcode = "" Then Exit Sub

            ' 🔍 Search product in database
            Using conn As SqlConnection = DataAccess.GetConnection()
                Dim sql As String = "
                SELECT TOP 1 dp.DeliveryProductID, dp.ProductID, p.Product AS ProductName, 
                               p.SellingPrice, p.ImagePath
                FROM tbl_delivery_products dp
                INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
                WHERE dp.Status = 'Posted' AND p.BarcodeNumber = @barcode"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@barcode", barcode)
                    conn.Open()

                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            Dim deliveryProductID As Integer = Convert.ToInt32(rdr("DeliveryProductID"))
                            Dim productID As Integer = Convert.ToInt32(rdr("ProductID"))
                            Dim name As String = rdr("ProductName").ToString()
                            Dim price As Decimal = Convert.ToDecimal(rdr("SellingPrice"))
                            Dim imagePath As String = rdr("ImagePath").ToString()
                            Dim existingRow As DataRow = Nothing

                            ' 🧩 Check if already in cart
                            For Each row As DataRow In dtPending.Rows
                                If Convert.ToInt32(row("DeliveryProductID")) = deliveryProductID Then
                                    existingRow = row
                                    Exit For
                                End If
                            Next

                            If existingRow IsNot Nothing Then
                                ' ✅ Increase quantity
                                existingRow("Quantity") = Convert.ToInt32(existingRow("Quantity")) + 1
                            Else
                                ' ➕ Add new product
                                Dim productImage As Image = Nothing
                                If IO.File.Exists(imagePath) Then
                                    Using tempImg As Image = Image.FromFile(imagePath)
                                        productImage = New Bitmap(tempImg)
                                    End Using
                                End If
                                dtPending.Rows.Add(deliveryProductID, productID, barcode, name, 1, price, imagePath, productImage)
                            End If

                            ' ✅ Refresh cart and totals
                            DGVCart.DataSource = dtPending
                            DGVCart.Refresh()
                            DGVCart.ClearSelection()
                            UpdateTotals()

                            My.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Asterisk)
                        Else
                            MessageBox.Show("Product not found for barcode: " & barcode, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            End Using

            DGVCart.ClearSelection()
            txtSearch.Clear()
        End If
    End Sub
    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub
    Private Sub StartSwitchTimer()
        switchtimer.Interval = 1000
        switchtimer.Start()
    End Sub
    Private Sub switchTimer_Tick(sender As Object, e As EventArgs) Handles switchtimer.Tick
        switchtimer.Stop()
        Me.Hide()
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_Inventory()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnFileMaintenance.Click
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_User()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnDelivery.Click
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_Deliveries()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub datetimer_Tick(sender As Object, e As EventArgs) Handles datetimer.Tick
        lblDate.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")  ' Example: 09/30/2025 08:42 PM
    End Sub

    Private Sub btnPos_Click(sender As Object, e As EventArgs) Handles btnPos.Click

    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click


        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to log out?",
                                                "Logout",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' 🧾 Log audit trail

            If dtPending.Rows.Count > 0 Then
                RestoreAllPendingItems()
            End If

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
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_Home()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_transaction()
        formtoshow.Show()
        StartSwitchTimer()
    End Sub

    Private Sub btnAuditTrail_Click(sender As Object, e As EventArgs) Handles btnAuditTrail.Click
        If dtPending.Rows.Count > 0 Then
            RestoreAllPendingItems()
        End If

        formtoshow = New Admin_AuditTrail()
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

    ' === EDIT SELECTED ITEM IN CART ===


End Class

