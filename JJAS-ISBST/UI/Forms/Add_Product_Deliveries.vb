Imports System.ComponentModel.Design
Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

' Note: For better separation of concerns, consider moving database operations (e.g., product search, validation) to a dedicated data access layer, such as a ProductRepository class.
' This would make the form code cleaner and easier to test. Example structure:
' Public Class ProductRepository
'     Public Function SearchProduct(searchTerm As String) As (ProductID As Integer, ProductName As String, Barcode As String, SellingPrice As Decimal, ImagePath As String)?
'         ' Implementation here
'     End Function
'     ' Other methods...
' End Class

Public Class Add_Product_Deliveries
    Public selectedID As Integer = -1
    Public SelectedProductName As String = ""
    Public SelectedBarcode As String = ""
    Public SelectedQuantity As Integer = 0
    Public SelectedCostPrice As Decimal = 0D
    Public SelectedSellingPrice As Decimal = 0D
    Public SelectedImagePath As String = ""

    Private currentSellingPrice As Decimal = 0D
    Private currentImagePath As String = ""

    Private Sub Add_Product_Deliveries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Disable copy-paste for numeric fields
        Try
            BlockCopyPaste(txtQuantity)
            BlockCopyPaste(txtCostPrice)
        Catch ex As Exception
            ' Log or handle the error instead of ignoring it
            MessageBox.Show("Error initializing form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' UI Polish: Add tooltips to clarify field requirements
        'Dim tooltip As New ToolTip()
        'tooltip.SetToolTip(txtSearch, "Search by product barcode.")
        'tooltip.SetToolTip(txtQuantity, "Enter the quantity (positive integer).")
        'tooltip.SetToolTip(txtCostPrice, "Enter the cost price (decimal, cannot exceed selling price).")
        'tooltip.SetToolTip(btnAdd, "Add the selected product to the delivery.")

        ' UI Polish: Enable/disable the Add button based on validation state
        UpdateAddButtonState()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub txtQuantity_TextChanged(sender As Object, e As EventArgs) Handles txtQuantity.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub txtCostPrice_TextChanged(sender As Object, e As EventArgs) Handles txtCostPrice.TextChanged
        UpdateAddButtonState()
    End Sub

    Private Sub UpdateAddButtonState()
        '' UI Polish: Enable Add button only if all required fields are filled and validations pass
        'Dim isValid As Boolean = Not String.IsNullOrWhiteSpace(txtSearch.Text.Trim()) AndAlso
        '                         Not String.IsNullOrWhiteSpace(txtBarcodeNumber.Text.Trim()) AndAlso
        '                         Not String.IsNullOrWhiteSpace(txtProductName.Text.Trim()) AndAlso
        '                         Not String.IsNullOrWhiteSpace(txtCostPrice.Text.Trim()) AndAlso
        '                         Not String.IsNullOrWhiteSpace(txtQuantity.Text.Trim()) AndAlso
        '                         selectedID > 0 AndAlso
        '                         IsValidQuantity(txtQuantity.Text.Trim()) AndAlso
        '                         IsValidCostPrice(txtCostPrice.Text.Trim())
        'btnAdd.Enabled = isValid
    End Sub

    Private Function IsValidQuantity(quantityText As String) As Boolean
        Dim quantity As Integer
        Return Integer.TryParse(quantityText, quantity) AndAlso quantity > 0
    End Function

    Private Function IsValidCostPrice(costPriceText As String) As Boolean
        Dim costPrice As Decimal
        Return Decimal.TryParse(costPriceText, costPrice) AndAlso costPrice >= 0 AndAlso costPrice <= currentSellingPrice
    End Function

    Private Sub txtCostPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCostPrice.KeyPress
        ' Enhanced Validation: Allow digits, backspace, and one decimal point
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
    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True

        End If
    End Sub

    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress
        ' Enhanced Validation: Allow digits and backspace only
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then

            e.Handled = True
        End If
    End Sub

    ' Refactoring: Moved to a potential ProductRepository class for better separation
    Private Sub txtSearchs_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            SelectedBarcode = ""
            txtProductName.Clear()
            selectedID = -1
            currentSellingPrice = 0D
            currentImagePath = ""
            UpdateAddButtonState()
            Exit Sub
        End If

        Dim sql As String = "SELECT TOP 1 ProductID, Product, BarcodeNumber, SellingPrice, ImagePath
                     FROM tbl_Products 
                     WHERE IsActive = 1 AND (BarcodeNumber LIKE @search)"

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
                    Else
                        selectedID = -1
                        txtProductName.Clear()
                        SelectedBarcode = ""
                        currentSellingPrice = 0D
                        currentImagePath = ""
                    End If
                End Using
            End Using
        End Using

        UpdateAddButtonState()
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then
            lblPlaceholder.Visible = True
        End If
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Enhanced Validation: Trim all inputs before validation
        Dim productName As String = txtProductName.Text.Trim()
        Dim costPriceText As String = txtCostPrice.Text.Trim()
        Dim quantityText As String = txtQuantity.Text.Trim()

        If String.IsNullOrEmpty(productName) OrElse
           String.IsNullOrEmpty(costPriceText) OrElse
           String.IsNullOrEmpty(quantityText) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If selectedID = -1 Then
            MessageBox.Show("Please search and select a valid product first.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim costPrice As Decimal
        If Not Decimal.TryParse(costPriceText, costPrice) OrElse costPrice < 0 Then
            MessageBox.Show("Invalid Cost Price. Please enter a valid non-negative number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(quantityText, quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Invalid Quantity. Please enter a valid positive number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Refactoring: Moved to a potential ProductRepository class for better separation
        ' --- Get Selling Price from DB (already fetched in search, but re-validate) ---
        If costPrice >= currentSellingPrice Then
            MessageBox.Show($"Cost Price ({costPrice:C}) cannot be equal or higher than Selling Price ({currentSellingPrice:C}).",
                    "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to add this product?",
               "Confirm Edit",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                ' --- Pass data back to parent form ---
                Me.SelectedProductName = productName
                Me.SelectedBarcode = SelectedBarcode
                Me.SelectedQuantity = quantity
                Me.SelectedCostPrice = costPrice
                Me.SelectedSellingPrice = currentSellingPrice
                Me.SelectedImagePath = currentImagePath

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while saving product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs) Handles Panel8.Paint

    End Sub
End Class