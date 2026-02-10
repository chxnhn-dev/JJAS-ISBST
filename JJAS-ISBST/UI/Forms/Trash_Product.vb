Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Trash_Product
    Private Sub Trash_Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")
    End Sub
    Private Sub displayData(searchText As String)
        Dim dt As New DataTable()
        Dim sql As String = "
    SELECT p.ProductID,
           p.BarcodeNumber,
           p.Product,
           p.SellingPrice,
           p.Description,
           p.DateCreated AS DateUpdated,
           p.ImagePath,
           b.BrandID,
           c.CategoryID,
           col.ColorID,
           s.SizeID
    FROM tbl_Products p
    INNER JOIN tbl_Brand b ON p.BrandID = b.BrandID
    INNER JOIN tbl_Category c ON p.CategoryID = c.CategoryID
    INNER JOIN tbl_Color col ON p.ColorID = col.ColorID
    INNER JOIN tbl_Size s ON p.SizeID = s.SizeID
    WHERE p.IsActive = 0
    AND (@search = '' OR p.BarcodeNumber LIKE @search OR p.Product LIKE @search)
    ORDER BY p.DateCreated DESC;"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                Dim searchValue As String = "%" & searchText & "%"
                cmd.Parameters.AddWithValue("@search", searchValue)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        If Not dt.Columns.Contains("ProductImage") Then
            dt.Columns.Add("ProductImage", GetType(Image))
        End If

        ' ✅ Load images
        For Each row As DataRow In dt.Rows
            Dim path As String = row("ImagePath").ToString()
            If Not String.IsNullOrEmpty(path) AndAlso IO.File.Exists(path) Then
                Using tempImg As Image = Image.FromFile(path)
                    row("ProductImage") = New Bitmap(tempImg)
                End Using
            Else
                row("ProductImage") = Nothing
            End If
        Next

        DGVtrash.DataSource = dt

        If DGVtrash.Columns.Contains("ProductImage") Then
            DGVtrash.Columns("ProductImage").DisplayIndex = 0
            Dim imgCol As DataGridViewImageColumn = DirectCast(DGVtrash.Columns("ProductImage"), DataGridViewImageColumn)
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
            imgCol.Width = 120
            imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None ' 🔑 Prevent resizing
        End If

        DGVtrash.DataSource = dt

        Dim hiddenCols() As String = {"ImagePath", "ProductID", "ColorID", "CategoryID", "BrandID", "SizeID"}
        For Each colName In hiddenCols
            If DGVtrash.Columns.Contains(colName) Then
                DGVtrash.Columns(colName).Visible = False
            End If
        Next

        DGVtrash.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        For Each col As DataGridViewColumn In DGVtrash.Columns
            If col.Name = "ProductImage" Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None ' lock image width
            Else
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        Next
        DGVtrash.ClearSelection()
    End Sub
    Private Sub DGVtrash_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtrash.DataBindingComplete
        ' Set row height
        DGVtrash.RowTemplate.Height = 50
        For Each row As DataGridViewRow In DGVtrash.Rows
            row.Height = 50
        Next

        ' Set font size for cells
        DGVtrash.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

        ' Optional: make header text bigger too
        DGVtrash.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)

        ' Optional: center align headers
        DGVtrash.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    Private Sub DGVsize_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVtrash.DataBindingComplete
        DGVtrash.RowTemplate.Height = 60
        For Each row As DataGridViewRow In DGVtrash.Rows
            row.Height = 60
        Next
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text.Trim)
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
    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        If DGVtrash.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to restore.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtrash.SelectedRows(0)
        Dim ID As String = row.Cells(0).Value.ToString()

        Try

            If MessageBox.Show("Are you sure you want to restore this row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Dim sql As String = "UPDATE tbl_Products SET IsActive = 1, DateCreated=@DateCreated WHERE ProductID = @ProductID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@ProductID", ID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Restore Product.")
                MessageBox.Show("Row restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                displayData("")
                DGVtrash.ClearSelection()

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while restoring product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVtrash.SelectedRows.Count > 0 Then
            Dim confirmDelete As DialogResult = MessageBox.Show("Are you sure you want to delete this row?",
                                                            "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            Dim id As Integer = Convert.ToInt32(DGVtrash.CurrentRow.Cells(0).Value)
            If confirmDelete = DialogResult.Yes Then

                conn.Open()

                Dim deleteCmd As New SqlCommand("DELETE FROM tbl_Products WHERE ProductID = @ProductID", conn)
                deleteCmd.Parameters.AddWithValue("@ProductID", id)
                deleteCmd.ExecuteNonQuery()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted Product Permanently.")
                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                conn.Close()
                displayData("")

            End If
        Else
            MsgBox("Please select a row to delete.", MsgBoxStyle.Exclamation, "No Selection")
        End If
    End Sub

    Private Sub DGVtrash_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtrash.CellClick
        Dim id As String = ""
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DGVtrash.Rows(e.RowIndex)
            id = row.Cells(0).Value.ToString()
        End If
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class