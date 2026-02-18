Imports System.Data.SqlClient

Public Class FrmDeliveryProductEntry
    Public Property ProductID As Integer?

    Public selectedID As Integer = -1
    Public SelectedProductName As String = ""
    Public SelectedBarcode As String = ""
    Public SelectedQuantity As Integer = 0
    Public SelectedCostPrice As Decimal = 0D
    Public SelectedSellingPrice As Decimal = 0D
    Public SelectedImagePath As String = ""

    Private currentSellingPrice As Decimal = 0D
    Private currentImagePath As String = ""

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return ProductID.HasValue
        End Get
    End Property

    Private Sub Add_Product_Deliveries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            BlockCopyPaste(txtSearch)
            BlockCopyPaste(txtQuantity)
            BlockCopyPaste(txtCostPrice)
            BlockCopyPaste(txtSellingPrice)
        Catch ex As Exception
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ConfigureMode()

        If IsEditMode Then
            LoadProductForEdit()
        Else
            lblPlaceholder.Visible = String.IsNullOrWhiteSpace(txtSearch.Text)
        End If

        UpdateAddButtonState()
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Product Delivery"
            Label12.Text = "Edit Product:"
            btnAdd.Text = "  Update"
            txtSearch.ReadOnly = False
            lblPlaceholder.Visible = False
        Else
            Me.Text = "Add Product Delivery"
            Label12.Text = "Add Product:"
            btnAdd.Text = "  Add"
            txtSearch.ReadOnly = False
        End If
    End Sub

    Private Sub LoadProductForEdit()
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand("SELECT ProductID, Product, BarcodeNumber, SellingPrice, ImagePath FROM tbl_Products WHERE ProductID = @ProductID", conn)
                cmd.Parameters.AddWithValue("@ProductID", ProductID.Value)
                conn.Open()

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        selectedID = Convert.ToInt32(reader("ProductID"))
                        txtProductName.Text = reader("Product").ToString().Trim()
                        SelectedBarcode = reader("BarcodeNumber").ToString().Trim()
                        txtSearch.Text = SelectedBarcode
                        currentSellingPrice = If(IsDBNull(reader("SellingPrice")), 0D, Convert.ToDecimal(reader("SellingPrice")))
                        currentImagePath = If(IsDBNull(reader("ImagePath")), "", reader("ImagePath").ToString().Trim())
                    Else
                        MessageBox.Show("Selected product was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                        Exit Sub
                    End If
                End Using
            End Using
        End Using

        txtQuantity.Text = If(SelectedQuantity > 0, SelectedQuantity.ToString(), "")
        txtCostPrice.Text = If(SelectedCostPrice > 0D, SelectedCostPrice.ToString("0.##"), "")
        txtSellingPrice.Text = If(SelectedSellingPrice > 0D, SelectedSellingPrice.ToString("0.##"), currentSellingPrice.ToString("0.##"))
    End Sub

    Private Sub UpdateAddButtonState()
        Dim quantity As Integer
        Dim costPrice As Decimal
        Dim sellingPrice As Decimal

        Dim hasProduct As Boolean = selectedID > 0 AndAlso Not String.IsNullOrWhiteSpace(txtProductName.Text)
        Dim hasQuantity As Boolean = Integer.TryParse(txtQuantity.Text.Trim(), quantity) AndAlso quantity > 0
        Dim hasCost As Boolean = Decimal.TryParse(txtCostPrice.Text.Trim(), costPrice) AndAlso costPrice >= 0D
        Dim hasSelling As Boolean = Decimal.TryParse(txtSellingPrice.Text.Trim(), sellingPrice) AndAlso sellingPrice > 0D
        Dim hasPriceRule As Boolean = hasCost AndAlso hasSelling AndAlso costPrice < sellingPrice

        btnAdd.Enabled = hasProduct AndAlso hasQuantity AndAlso hasCost AndAlso hasSelling AndAlso hasPriceRule
    End Sub

    Private Sub txtQuantity_TextChanged(sender As Object, e As EventArgs) Handles txtQuantity.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub txtCostPrice_TextChanged(sender As Object, e As EventArgs) Handles txtCostPrice.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub txtSellingPrice_TextChanged(sender As Object, e As EventArgs) Handles txtSellingPrice.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            lblPlaceholder.Visible = True
            SelectedBarcode = ""
            txtProductName.Clear()
            selectedID = -1
            currentSellingPrice = 0D
            currentImagePath = ""
            txtSellingPrice.Clear()
            UpdateAddButtonState()
            Exit Sub
        End If

        lblPlaceholder.Visible = False

        Dim sql As String = "SELECT TOP 1 ProductID, Product, BarcodeNumber, SellingPrice, ImagePath FROM tbl_Products WHERE IsActive = 1 AND BarcodeNumber LIKE @search"
        Dim searchValue As String = "%" & txtSearch.Text.Trim() & "%"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@search", searchValue)
                conn.Open()

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        selectedID = Convert.ToInt32(reader("ProductID"))
                        txtProductName.Text = reader("Product").ToString().Trim()
                        SelectedBarcode = reader("BarcodeNumber").ToString().Trim()
                        currentSellingPrice = If(IsDBNull(reader("SellingPrice")), 0D, Convert.ToDecimal(reader("SellingPrice")))
                        currentImagePath = If(IsDBNull(reader("ImagePath")), "", reader("ImagePath").ToString().Trim())
                        txtSellingPrice.Text = currentSellingPrice.ToString("0.##")
                    Else
                        selectedID = -1
                        txtProductName.Clear()
                        SelectedBarcode = ""
                        currentSellingPrice = 0D
                        currentImagePath = ""
                        txtSellingPrice.Clear()
                    End If
                End Using
            End Using
        End Using

        UpdateAddButtonState()
    End Sub

    Private Sub txtCostPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCostPrice.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If txtCostPrice.Text.Contains(".") Then
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtSellingPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSellingPrice.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            If txtSellingPrice.Text.Contains(".") Then
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If String.IsNullOrWhiteSpace(txtSearch.Text) AndAlso Not IsEditMode Then
            lblPlaceholder.Visible = True
        End If
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim productName As String = txtProductName.Text.Trim()
        Dim costPriceText As String = txtCostPrice.Text.Trim()
        Dim sellingPriceText As String = txtSellingPrice.Text.Trim()
        Dim quantityText As String = txtQuantity.Text.Trim()

        If String.IsNullOrEmpty(productName) OrElse String.IsNullOrEmpty(costPriceText) OrElse String.IsNullOrEmpty(sellingPriceText) OrElse String.IsNullOrEmpty(quantityText) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If selectedID = -1 Then
            MessageBox.Show("Please search and select a valid product first.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim costPrice As Decimal
        If Not Decimal.TryParse(costPriceText, costPrice) OrElse costPrice < 0D Then
            MessageBox.Show("Invalid Cost Price. Please enter a valid non-negative number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(quantityText, quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Invalid Quantity. Please enter a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim sellingPrice As Decimal
        If Not Decimal.TryParse(sellingPriceText, sellingPrice) OrElse sellingPrice <= 0D Then
            MessageBox.Show("Invalid Selling Price. Please enter a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If costPrice >= sellingPrice Then
            MessageBox.Show($"Cost Price ({costPrice:C}) cannot be equal or higher than Selling Price ({sellingPrice:C}).",
                            "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        SelectedProductName = productName
        SelectedBarcode = If(String.IsNullOrWhiteSpace(SelectedBarcode), txtSearch.Text.Trim(), SelectedBarcode)
        SelectedQuantity = quantity
        SelectedCostPrice = costPrice
        SelectedSellingPrice = sellingPrice
        SelectedImagePath = currentImagePath

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs) Handles Panel8.Paint

    End Sub
End Class
