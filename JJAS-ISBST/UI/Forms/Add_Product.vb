Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Product
    Public Property ProductID As Integer?
    Public Property LockSellingPrice As Boolean

    Private _currentImagePath As String = String.Empty

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return ProductID.HasValue
        End Get
    End Property

    Private Sub Add_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtProductName)
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtBarcodeNumber)
        BlockCopyPaste(txtSellingPrice)

        LoadComboBox(cbColor, "tbl_Color", "Color", "ColorID")
        LoadComboBox(cbSize, "tbl_Size", "Size", "SizeID")
        LoadComboBox(cbCategory, "tbl_Category", "Category", "CategoryID")
        LoadComboBox(cbBrand, "tbl_Brand", "Brand", "BrandID")

        ConfigureMode()

        If IsEditMode Then
            LoadProductForEdit()
        End If

        txtSellingPrice.ReadOnly = LockSellingPrice
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Product"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Product"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadProductForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT BarcodeNumber, Product, SellingPrice, Description, BrandID, CategoryID, ColorID, SizeID, ImagePath FROM tbl_Products WHERE ProductID = @ProductID", connection)
                command.Parameters.AddWithValue("@ProductID", ProductID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtBarcodeNumber.Text = reader("BarcodeNumber").ToString()
                        txtProductName.Text = reader("Product").ToString()
                        txtSellingPrice.Text = Convert.ToDecimal(reader("SellingPrice")).ToString("0.##")
                        txtDescription.Text = reader("Description").ToString()

                        cbBrand.SelectedValue = Convert.ToInt32(reader("BrandID"))
                        cbCategory.SelectedValue = Convert.ToInt32(reader("CategoryID"))
                        cbColor.SelectedValue = Convert.ToInt32(reader("ColorID"))
                        cbSize.SelectedValue = Convert.ToInt32(reader("SizeID"))

                        _currentImagePath = If(reader("ImagePath") Is DBNull.Value, String.Empty, reader("ImagePath").ToString())

                        If Not String.IsNullOrWhiteSpace(_currentImagePath) AndAlso IO.File.Exists(_currentImagePath) Then
                            Using tempImg As Image = Image.FromFile(_currentImagePath)
                                PictureBox1.Image = CType(tempImg.Clone(), Image)
                            End Using
                            PictureBox1.Tag = _currentImagePath
                        Else
                            PictureBox1.Image = Nothing
                            PictureBox1.Tag = Nothing
                        End If
                    Else
                        MessageBox.Show("Selected product record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Sub LoadComboBox(cbo As ComboBox, tableName As String, displayMember As String, valueMember As String)
        Dim dt As New DataTable()
        Dim sql As String = $"SELECT {valueMember}, {displayMember} FROM {tableName} WHERE IsActive = 1"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using da As New SqlDataAdapter(sql, connection)
                da.Fill(dt)
            End Using
        End Using

        Dim newRow As DataRow = dt.NewRow()
        newRow(displayMember) = "-- Select Option --"
        newRow(valueMember) = 0
        dt.Rows.InsertAt(newRow, 0)

        cbo.DataSource = dt
        cbo.DisplayMember = displayMember
        cbo.ValueMember = valueMember
        cbo.SelectedIndex = 0
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_Products WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND ProductID <> @ProductID"
        End If

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@ProductID", ProductID.Value)
                End If

                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub txtSellingPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSellingPrice.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then
            e.Handled = False
        ElseIf e.KeyChar = "."c Then
            e.Handled = txtSellingPrice.Text.Contains(".")
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtBarcodeNumber_keypress(sender As Object, e As KeyPressEventArgs) Handles txtBarcodeNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtProductName.Text) OrElse
           String.IsNullOrWhiteSpace(txtSellingPrice.Text) OrElse
           cbBrand.SelectedValue Is Nothing OrElse
           cbCategory.SelectedValue Is Nothing OrElse
           cbColor.SelectedValue Is Nothing OrElse
           cbSize.SelectedValue Is Nothing Then

            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cbBrand.SelectedValue = 0 Then
            MessageBox.Show("Please select a valid Brand.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cbSize.SelectedValue = 0 Then
            MessageBox.Show("Please select a valid Size.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cbColor.SelectedValue = 0 Then
            MessageBox.Show("Please select a valid Color.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cbCategory.SelectedValue = 0 Then
            MessageBox.Show("Please select a valid Category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Product", txtProductName.Text.Trim()) Then
            MessageBox.Show("Product already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("BarcodeNumber", txtBarcodeNumber.Text.Trim()) Then
            MessageBox.Show("Barcode Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim imagePath As String = _currentImagePath
        If PictureBox1.Tag IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(PictureBox1.Tag.ToString()) Then
            imagePath = PictureBox1.Tag.ToString()
        End If

        If String.IsNullOrWhiteSpace(imagePath) AndAlso Not IsEditMode Then
            Dim defaultImagePath As String = IO.Path.Combine(Application.StartupPath, "Resources\no_image_available.png")
            If IO.File.Exists(defaultImagePath) Then
                imagePath = defaultImagePath
            End If
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Products SET BarcodeNumber=@BarcodeNumber, Product=@Product, SellingPrice=@SellingPrice, Description=@Description, BrandID=@BrandID, CategoryID=@CategoryID, ColorID=@ColorID, SizeID=@SizeID, ImagePath=@ImagePath, DateCreated=@DateCreated WHERE ProductID=@ProductID"
                Else
                    sql = "INSERT INTO tbl_Products (BarcodeNumber, Product, SellingPrice, Description, BrandID, CategoryID, ColorID, SizeID, ImagePath, DateCreated) VALUES (@BarcodeNumber, @Product, @SellingPrice, @Description, @BrandID, @CategoryID, @ColorID, @SizeID, @ImagePath, @DateCreated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@BarcodeNumber", txtBarcodeNumber.Text.Trim())
                    command.Parameters.AddWithValue("@Product", txtProductName.Text.Trim())
                    command.Parameters.AddWithValue("@SellingPrice", Decimal.Parse(txtSellingPrice.Text.Trim()))
                    command.Parameters.AddWithValue("@Description", txtDescription.Text.Trim())
                    command.Parameters.AddWithValue("@BrandID", cbBrand.SelectedValue)
                    command.Parameters.AddWithValue("@CategoryID", cbCategory.SelectedValue)
                    command.Parameters.AddWithValue("@ColorID", cbColor.SelectedValue)
                    command.Parameters.AddWithValue("@SizeID", cbSize.SelectedValue)
                    command.Parameters.AddWithValue("@ImagePath", If(String.IsNullOrWhiteSpace(imagePath), CType(DBNull.Value, Object), imagePath))
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@ProductID", ProductID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Product.", "Added Product."))
            MessageBox.Show(If(IsEditMode, "Product updated successfully!", "Product added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"

            If ofd.ShowDialog() = DialogResult.OK Then
                Dim imagePath As String = ofd.FileName

                If Not IO.File.Exists(imagePath) Then
                    MessageBox.Show("The selected image file does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                Dim fileInfo As New IO.FileInfo(imagePath)
                If fileInfo.Length > 5 * 1024 * 1024 Then
                    MessageBox.Show("Image file is too large. Please select an image smaller than 5 MB.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                If PictureBox1.Image IsNot Nothing Then
                    PictureBox1.Image.Dispose()
                    PictureBox1.Image = Nothing
                End If

                Try
                    Using tempImg As Image = Image.FromFile(imagePath)
                        PictureBox1.Image = CType(tempImg.Clone(), Image)
                    End Using
                    PictureBox1.Tag = imagePath
                    _currentImagePath = imagePath
                Catch ex As OutOfMemoryException
                    MessageBox.Show("The selected file is not a valid image or is corrupted.", "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("Error loading image: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
