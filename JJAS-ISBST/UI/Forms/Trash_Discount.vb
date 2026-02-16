Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Trash_Discount
    Private Sub Trash_Discount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        displayData("")
    End Sub
    Private Sub displayData(Searchtext As String)
        Try
            Dim dt As New DataTable
            Dim query As String = "
        SELECT DiscountID, 
               DiscountName,
               DiscountValue,
               Description,
               DateUpdated
        FROM tbl_Discount WHERE IsActive = 0 
        AND (@Search = '' or DiscountNAme LIKE @Search or DiscountValue LIKE @Search) 
        ORDER BY Dateupdated DESC;"

            Using conn As SqlConnection = DataAccess.GetConnection()

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Search", "%" & Searchtext & "%")

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
            DGVtrash.DataSource = dt

            If DGVtrash.Columns.Contains("DiscountID") Then
                DGVtrash.Columns("DiscountID").Visible = False
            End If

            DGVtrash.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DGVtrash.ClearSelection()
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        displayData(txtSearch.Text)
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
                    Dim sql As String = "UPDATE tbl_Discount SET IsActive = 1, DateUpdated=@DateUpdated WHERE DiscountID = @DiscountID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@DiscountID", ID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Restored Discount")
                MessageBox.Show("Row restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                displayData("")
                DGVtrash.ClearSelection()

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while restoring discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If DGVtrash.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtrash.SelectedRows(0)
        Dim ID As String = row.Cells(0).Value.ToString()

        If MessageBox.Show("Are you sure you want to permanently delete this row?",
                           "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

            Using conn As SqlConnection = DataAccess.GetConnection()
                conn.Open()
                Dim sql As String = "DELETE FROM tbl_Discount WHERE ColorID = @ColorID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@ColorID", ID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Deleted Discount Permanently")
            MessageBox.Show("Row deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DGVtrash.ClearSelection()
            displayData("")
            Me.DialogResult = DialogResult.OK
            Me.Close()
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