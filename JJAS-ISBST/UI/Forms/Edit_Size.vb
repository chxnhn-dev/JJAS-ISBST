Imports System.Data.SqlClient
Imports JJAS_ISBST.Login
Imports Microsoft.VisualBasic.ApplicationServices

Public Class Edit_Size
    Public Property SizeID As Integer
    Public Property Size As String
    Public Property Description As String
    Private Sub Edit_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtDescription)
        BlockCopyPaste(txtSize)
        txtSize.Text = Size
        txtDescription.Text = Description
    End Sub
    Private Function IsDuplicate(fieldName As String, value As String, currentColorId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_Size WHERE " & fieldName & " = @Value AND SizeID <> @SizeID)
           SELECT 1 ELSE SELECT 0"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@SizeID", currentColorId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSize.KeyPress
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
        If String.IsNullOrWhiteSpace(txtsize.Text) Then
            MessageBox.Show("Please fill in all required fields.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtSize.Text.Length < 2 Then
            MessageBox.Show("Size name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtSize.Text.Contains("  ") Then
            MessageBox.Show("Size name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Size", txtsize.Text, SizeID) Then
            MessageBox.Show("Size name already exists.", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If MessageBox.Show("Are you sure you want to update this size?",
                   "Confirm Edit",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            Try

                Dim sql As String = "UPDATE tbl_Size SET " &
                                "Size=@Size, Description=@Description, DateCreated=@DateCreated " &
                                "WHERE SizeID=@SizeID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@size", txtsize.Text.Trim)
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@SizeID", SizeID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Size.")
                MessageBox.Show("Size updated successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("An error occurred while editing brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class