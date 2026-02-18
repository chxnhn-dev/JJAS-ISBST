Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Edit_Product

    Public Property ProductID As Integer
    Public Property BarcodeNumber As String
    Public Property Product As String
    Public Property SellingPrice As Decimal
    Public Property Description As String
    Public Property BrandID As Integer
    Public Property CategoryID As Integer
    Public Property SizeID As Integer
    Public Property ColorID As Integer
    Public Property ImagePath As String

    Private Sub Edit_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtProductName)
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtSellingPrice)
        BlockCopyPaste(txtBarcodeNumber)


        LoadComboBox(cbColor, "tbl_Color", "Color", "ColorID")
        LoadComboBox(cbSize, "tbl_Size", "Size", "SizeID")
        LoadComboBox(cbCategory, "tbl_Category", "Category", "CategoryID")
        LoadComboBox(cbBrand, "tbl_Brand", "Brand", "BrandID")


        txtProductName.Text = Product
        txtBarcodeNumber.Text = BarcodeNumber
        txtDescription.Text = Description
        txtSellingPrice.Text = SellingPrice
        cbBrand.SelectedValue = BrandID
        cbCategory.SelectedValue = CategoryID
        cbSize.SelectedValue = SizeID
        cbColor.SelectedValue = ColorID

        If Not String.IsNullOrEmpty(ImagePath) AndAlso IO.File.Exists(ImagePath) Then
            Using tempImg As Image = Image.FromFile(ImagePath)
                PictureBox1.Image = New Bitmap(tempImg)
            End Using
        Else
            PictureBox1.Image = Nothing
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

                If PictureBox1.Tag IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(PictureBox1.Tag.ToString()) Then
                    Me.ImagePath = PictureBox1.Tag.ToString()
                End If

                Try
                    Using tempImg As Image = Image.FromFile(imagePath)
                        PictureBox1.Image = CType(tempImg.Clone(), Image)
                    End Using
                    PictureBox1.Tag = imagePath

                    ' ✅ Update property so Save/Edit uses the new path
                    Me.ImagePath = imagePath

                Catch ex As OutOfMemoryException
                    MessageBox.Show("The selected file is not a valid image or is corrupted.",
                                "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("Error loading image: " & ex.Message)
                End Try
            End If
        End Using
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

            MsgBox("Selling price must be numbers only.", vbCritical, "Invalid Input")
            e.Handled = True
        End If
    End Sub
    Private Sub txtBarcodeNumber_keypress(sender As Object, e As KeyPressEventArgs) Handles txtBarcodeNumber.keypress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub
    Private Function IsDuplicate(fieldName As String, value As String, currentColorId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_Products WHERE " & fieldName & " = @Value AND ProductID <> @ProductID)
           SELECT 1 ELSE SELECT 0"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@ProductID", currentColorId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

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

        If IsDuplicate("Product", txtProductName.Text, ProductID) Then
            MessageBox.Show("Product already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("BarcodeNumber", txtBarcodeNumber.Text, ProductID) Then
            MessageBox.Show("Barcode Number already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If MessageBox.Show("Are you sure you want to update this product?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            Try

                Dim sql As String = "UPDATE tbl_Products SET " &
        "BarcodeNumber=@BarcodeNumber, Product=@Product, SellingPrice=@SellingPrice, " &
        "Description=@Description, BrandID=@Brand, CategoryID=@Category, " &
        "ColorID=@Color, SizeID=@Size, ImagePath=@ImagePath, DateCreated=@DateCreated " &
        "WHERE ProductID=@ProductID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@BarcodeNumber", txtBarcodeNumber.Text.Trim())
                        cmd.Parameters.AddWithValue("@Product", txtProductName.Text.Trim())
                        cmd.Parameters.AddWithValue("@SellingPrice", txtSellingPrice.Text.Trim())
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim())
                        cmd.Parameters.AddWithValue("@Brand", cbBrand.SelectedValue)
                        cmd.Parameters.AddWithValue("@Category", cbCategory.SelectedValue)
                        cmd.Parameters.AddWithValue("@Color", cbColor.SelectedValue)
                        cmd.Parameters.AddWithValue("@Size", cbSize.SelectedValue)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@ProductID", ProductID)
                        cmd.Parameters.AddWithValue("@ImagePath", If(String.IsNullOrEmpty(ImagePath), DBNull.Value, ImagePath))

                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Product.")
                MsgBox("Product updated successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while editing product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
