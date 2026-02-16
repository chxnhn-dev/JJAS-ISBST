Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Trash_Supplier
    Private Sub Trash_Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSearch)
        DisplayData("")
    End Sub

    ' ✅ Display all inactive suppliers
    Private Sub DisplayData(searchText As String)

        Dim dt As New DataTable
            Dim sql As String = "
            SELECT 
                SupplierID, 
                Company, 
                ContactNumber, 
                Address, 
                DateCreated as DateUpdated
            FROM tbl_Supplier 
            WHERE IsActive = 0
            AND (@search = '' OR Company LIKE @search)
            ORDER BY DateUpdated DESC;"

            Using conn As SqlConnection = DataAccess.GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            DGVtrash.DataSource = dt

            If DGVtrash.Columns.Contains("SupplierID") Then
                DGVtrash.Columns("SupplierID").Visible = False
            End If

            DGVtrash.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DGVtrash.ClearSelection()

    End Sub

    ' ✅ Live search
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        DisplayData(txtSearch.Text)
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        lblPlaceholder.Visible = False
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If txtSearch.Text.Trim() = "" Then lblPlaceholder.Visible = True
    End Sub

    Private Sub lblPlaceholder_Click(sender As Object, e As EventArgs) Handles lblPlaceholder.Click
        txtSearch.Focus()
    End Sub

    ' ✅ Restore Supplier
    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        If DGVtrash.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a supplier to restore.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtrash.SelectedRows(0)
        Dim id As Integer = Convert.ToInt32(row.Cells("SupplierID").Value)

        Try

            If MessageBox.Show("Are you sure you want to restore this supplier?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Using conn As SqlConnection = DataAccess.GetConnection()
                    Dim sql As String = "UPDATE tbl_Supplier SET IsActive = 1, DateCreated = @DateCreated WHERE SupplierID = @SupplierID"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@SupplierID", id)
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Restored Supplier.")
                MessageBox.Show("Supplier restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                DisplayData("")
                DGVtrash.ClearSelection()
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while restoring supplier: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ✅ Permanently Delete Supplier
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVtrash.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a supplier to permanently delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim row As DataGridViewRow = DGVtrash.SelectedRows(0)
        Dim id As Integer = Convert.ToInt32(row.Cells("SupplierID").Value)

        If MessageBox.Show("Are you sure you want to permanently delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Using conn As SqlConnection = DataAccess.GetConnection()
                Dim sql As String = "DELETE FROM tbl_Supplier WHERE SupplierID = @SupplierID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@SupplierID", id)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Permanently Deleted Supplier.")

            MessageBox.Show("Supplier permanently deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DisplayData("")
            DGVtrash.ClearSelection()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub DGVtrash_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVtrash.CellClick
        ' No action needed here, but keeps last selected ID consistent
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
