Imports System.Data.SqlClient

Public Class add_pos

    Public IsEditMode As Boolean = False

    Public selectedID As Integer = -1
    Public ProductID As Integer = -1
    Public SelectedProductName As String = ""
    Public SelectedBarcode As String = ""
    Public SelectedQuantity As Integer = 0
    Public SelectedSellingPrice As Decimal = 0D
    Public SelectedImagePath As String = ""
    Public selectedCostPrice As Decimal = 0D
    Public SelectedCategory As String = ""

    Private currentSellingPrice As Decimal = 0D
    Private currentImagePath As String = ""

    Private Sub add_pos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            BlockCopyPaste(txtQuantity)
            txtSearch.Text = SelectedBarcode
            ' Change title and button text based on mode
            If IsEditMode Then
                Label12.Text = "Edit Product"
                btnAdd.Visible = False
                txtSearch.ReadOnly = True
                btnAdd.Image = Nothing
            Else
                Label12.Text = "Add Product"
                btnEdit.Visible = False

                btnEdit.Image = Nothing


            End If

        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtBarcodeNumber_keypress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    ' 🔍 Search product
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            selectedID = -1
            ProductID = -1
            SelectedBarcode = ""
            txtProductName.Clear()
            txtQuantity.Clear()
            currentSellingPrice = 0D
            currentImagePath = ""
            selectedCostPrice = 0D
            SelectedCategory = ""
        End If

        Dim sql As String = "
            SELECT dp.DeliveryProductID,
                   p.ProductID,
                   p.Product AS ProductName,
                   p.BarcodeNumber,
                   p.SellingPrice,
                   dp.Quantity AS AvailableQty,
                   dp.CostPrice,
                   c.Category,
                   p.ImagePath
            FROM tbl_delivery_products dp
            INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
            INNER JOIN tbl_deliveries d ON dp.DeliveryID = d.DeliveryID
            INNER JOIN tbl_Category C ON p.CategoryID = C.CategoryID
            WHERE dp.Status = 'Posted' AND dp.Quantity > 0
              AND (p.BarcodeNumber LIKE @search)
            ORDER BY d.DateCreated DESC;"

        Dim searchValue As String = "%" & txtSearch.Text & "%"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@search", searchValue)
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        selectedID = Convert.ToInt32(reader("DeliveryProductID"))
                        ProductID = Convert.ToInt32(reader("ProductID"))
                        txtProductName.Text = reader("ProductName").ToString()
                        SelectedBarcode = reader("BarcodeNumber").ToString()
                        currentSellingPrice = Convert.ToDecimal(reader("SellingPrice"))
                        currentImagePath = reader("ImagePath").ToString()
                        selectedCostPrice = Convert.ToDecimal(reader("CostPrice"))
                        SelectedCategory = reader("Category").ToString()
                    Else
                        selectedID = -1
                        ProductID = -1
                        txtProductName.Clear()
                        SelectedBarcode = ""
                        txtQuantity.Clear()
                        currentSellingPrice = 0D
                        currentImagePath = ""
                        selectedCostPrice = 0D
                        SelectedCategory = ""
                    End If
                End Using
            End Using
        End Using
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
    ' ➕ Add product to POS
    Private Function ValidateAndPrepareProduct() As Boolean
        Try

            If String.IsNullOrWhiteSpace(txtProductName.Text) OrElse
               String.IsNullOrWhiteSpace(txtQuantity.Text) Then
                MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Dim enteredQty As Integer
            If Not Integer.TryParse(txtQuantity.Text, enteredQty) OrElse enteredQty <= 0 Then
                MessageBox.Show("Invalid Quantity. Please enter a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Using conn As SqlConnection = DataAccess.GetConnection()
                Dim sql As String = "
                SELECT dp.Quantity AS AvailableQty, p.SellingPrice
                FROM tbl_delivery_products dp
                INNER JOIN tbl_products p ON dp.ProductID = p.ProductID
                WHERE dp.Status = 'Posted' AND dp.DeliveryProductID = @DeliveryProductID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@DeliveryProductID", selectedID)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim availableQty As Integer = Convert.ToInt32(reader("AvailableQty"))
                            If enteredQty > availableQty Then
                                MessageBox.Show($"Quantity entered ({enteredQty}) cannot be higher than available stock ({availableQty}).", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return False
                            End If
                            currentSellingPrice = Convert.ToDecimal(reader("SellingPrice"))
                        Else
                            MessageBox.Show("Product not found in database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return False
                        End If
                    End Using
                End Using
            End Using

            Me.SelectedProductName = txtProductName.Text.Trim()
            Me.SelectedBarcode = SelectedBarcode
            Me.SelectedQuantity = enteredQty
            Me.SelectedSellingPrice = currentSellingPrice
            Me.SelectedImagePath = currentImagePath
            Me.selectedCostPrice = selectedCostPrice
            Me.SelectedCategory = SelectedCategory

            Return True
        Catch ex As Exception
            MessageBox.Show("Validation failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If ValidateAndPrepareProduct() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If ValidateAndPrepareProduct() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub


    ' ❌ EXIT BUTTON
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Try
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error closing form: " & ex.Message, "Close Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs) Handles Panel8.Paint

    End Sub
End Class
