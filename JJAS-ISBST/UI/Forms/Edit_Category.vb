Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Edit_Category
    Public Property CategoryID As Integer
    Public Property Category As String
    Public Property Description As String
    Private Sub Edit_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtcategory.Text = Category
        BlockCopyPaste(txtcategory)

    End Sub

    Private Function IsDuplicate(fieldName As String, value As String, currentColorId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_Category WHERE LOWER(" & fieldName & ") = Lower(@Value) AND CategoryID <> @CategoryID)
           SELECT 1 ELSE SELECT 0"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@CategoryID", currentColorId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcategory.KeyPress
        Dim tb As TextBox = CType(sender, TextBox)

        ' First character: allow only letters
        If tb.SelectionStart = 0 Then
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True ' Block digits or special chars at first character
            End If
        Else
            ' After first character: allow letters, space, one hyphen only, and control keys
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                ' Check if hyphen
                If e.KeyChar = "-"c Then
                    ' Block if hyphen already exists
                    If tb.Text.Contains("-") Then
                        e.Handled = True
                    End If
                Else
                    ' Block any other invalid characters
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' 1. Validate fields
        If String.IsNullOrWhiteSpace(txtcategory.Text) Then
            MessageBox.Show("Please fill in all required fields.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtcategory.Text.Length < 2 Then
            MessageBox.Show("Category name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtcategory.Text.Contains("  ") Then
            MessageBox.Show("Category name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Category", txtcategory.Text, CategoryID) Then
            MessageBox.Show("Category name already exists.", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to update this category?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            Try

                Dim sql As String = "UPDATE tbl_Category SET " &
                                "Category=@Category, DateCreated=@DateCreated " &
                                "WHERE CategoryID=@CategoryID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Category", txtcategory.Text.Trim)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@CategoryID", CategoryID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Category.")
                MessageBox.Show("Updated successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("An error occurred while editing category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
