Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Product
    Private Sub Add_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtProductName)
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtBarcodeNumber)
        BlockCopyPaste(txtSellingPrice)
        LoadComboBox(cbColor, "tbl_Color", "Color", "ColorID")
        LoadComboBox(cbSize, "tbl_Size", "Size", "SizeID")
        LoadComboBox(cbCategory, "tbl_Category", "Category", "CategoryID")
        LoadComboBox(cbBrand, "tbl_Brand", "Brand", "BrandID")
    End Sub
    Private Sub SaveImagePath(productID As Integer, filePath As String)
        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "UPDATE tbl_Products SET ImagePath = @Path WHERE ProductID = @ProductID"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Path", filePath)
                cmd.Parameters.AddWithValue("@ProductID", productID)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    Private Sub LoadImagePath(productID As Integer, pictureBox As PictureBox)
        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "SELECT ImagePath FROM tbl_Products WHERE ProductID = @ProductID"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@ProductID", productID)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                    Dim path As String = result.ToString()
                    If IO.File.Exists(path) Then
                        pictureBox.Image = Image.FromFile(path)
                    Else
                        pictureBox.Image = Nothing
                    End If
                Else
                    pictureBox.Image = Nothing
                End If
            End Using
        End Using
    End Sub
    Private Sub SaveProduct(productID As Integer)
        Dim filePath As String = PictureBox1.Tag.ToString()

        Using conn As SqlConnection = DataAccess.GetConnection()
            Dim sql As String = "UPDATE tbl_Products SET ImagePath = @Path WHERE ProductID = @ProductID"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Path", filePath)
                cmd.Parameters.AddWithValue("@ProductID", productID)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    Private Sub txtSellingPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSellingPrice.KeyPress

        If Char.IsDigit(e.KeyChar) OrElse e.KeyChar = ControlChars.Back Then

            e.Handled = False
        ElseIf e.KeyChar = "."c Then

            If txtSellingPrice.Text.Contains(".") Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else

            e.Handled = True
        End If
    End Sub

    Private Sub txtBarcodeNumber_keypress(sender As Object, e As KeyPressEventArgs) Handles txtBarcodeNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    Public Sub LoadComboBox(cbo As ComboBox, tableName As String, displayMember As String, valueMember As String)
        Dim dt As New DataTable()
        Dim sql As String = $"SELECT {valueMember}, {displayMember} FROM {tableName} WHERE IsActive = 1"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using da As New SqlDataAdapter(sql, conn)
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
    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim exists As Boolean = False
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_Products WHERE LOWER({fieldName}) = LOWER(@Value)"
        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", fieldValue)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                exists = (count > 0)
            End Using
        End Using
        Return exists
    End Function


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

        If IsDuplicate("Product", txtProductName.Text) Then
            MessageBox.Show("Product already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If IsDuplicate("BarcodeNumber", txtBarcodeNumber.Text) Then
            MessageBox.Show("Barcode Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim imagePath As String

        If PictureBox1.Tag Is Nothing OrElse String.IsNullOrWhiteSpace(PictureBox1.Tag.ToString()) Then
            ' Use default "No Image Available" image from project resources or PictureBox1.Image
            Dim defaultImagePath As String = IO.Path.Combine(Application.StartupPath, "Resources\no_image_available.png")

            ' If default image file exists, use it; otherwise leave blank
            If IO.File.Exists(defaultImagePath) Then
                imagePath = defaultImagePath
            Else
                imagePath = String.Empty
            End If
        Else
            imagePath = PictureBox1.Tag.ToString()
        End If

        If MessageBox.Show("Are you sure you want to add this product?",
               "Confirm Edit",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Try
                Using conn As SqlConnection = DataAccess.GetConnection()
                    If conn.State = ConnectionState.Closed Then conn.Open()

                    Dim sql As String = "INSERT INTO tbl_Products 
                            (BarcodeNumber, Product, SellingPrice, Description, BrandID, CategoryID, ColorID, SizeID, ImagePath, DateCreated) 
                            VALUES (@BarcodeNumber, @Product, @SellingPrice, @Description, @BrandID, @CategoryID, @ColorID, @SizeID, @ImagePath, @DateCreated)"

                    Using cmd As New SqlCommand(sql, conn)
                        With cmd.Parameters
                            .AddWithValue("@BarcodeNumber", txtBarcodeNumber.Text.Trim())
                            .AddWithValue("@Product", txtProductName.Text.Trim())
                            .AddWithValue("@SellingPrice", txtSellingPrice.Text.Trim())
                            .AddWithValue("@Description", txtDescription.Text.Trim())
                            .AddWithValue("@BrandID", cbBrand.SelectedValue)
                            .AddWithValue("@CategoryID", cbCategory.SelectedValue)
                            .AddWithValue("@ColorID", cbColor.SelectedValue)
                            .AddWithValue("@SizeID", cbSize.SelectedValue)
                            .AddWithValue("@ImagePath", imagePath)
                            .AddWithValue("@DateCreated", DateTime.Now)
                        End With
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added Product.")

                MsgBox("Product added successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while adding user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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

                Catch ex As OutOfMemoryException
                    MessageBox.Show("The selected file is not a valid image or is corrupted.",
                                    "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("Error loading image: " & ex.Message)
                End Try
            End If
        End Using
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
