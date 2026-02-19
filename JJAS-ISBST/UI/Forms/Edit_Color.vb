Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin

Public Class Edit_Color
    Public Property ColorID As Integer
    Public Property Color As String
    Public Property Description As String
    Private Sub Edit_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtColor.Text = Color
    End Sub

    Private Function IsDuplicate(fieldName As String, value As String, currentColorId As Integer) As Boolean
        Dim sql As String = "
        IF EXISTS (SELECT 1 FROM tbl_Color WHERE " & fieldName & " = @Value AND ColorID <> @ColorID)
           SELECT 1 ELSE SELECT 0"

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                cmd.Parameters.AddWithValue("@ColorID", currentColorId)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtColor.KeyPress
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
        If String.IsNullOrWhiteSpace(txtColor.Text) Then
            MessageBox.Show("Please fill in all required fields.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtColor.Text.Length < 2 Then
            MessageBox.Show("Color name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtColor.Text.Contains("  ") Then
            MessageBox.Show("Color name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Color", txtColor.Text, ColorID) Then
            MessageBox.Show("Color name already exists.", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to update this color?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            Try

                Dim sql As String = "UPDATE tbl_Color SET " &
                                "Color=@Color, DateCreated=@DateCreated " &
                                "WHERE ColorID=@ColorID"

                Using conn As SqlConnection = DataAccess.GetConnection()
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Color", txtColor.Text)
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now)
                        cmd.Parameters.AddWithValue("@ColorID", ColorID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Edited Color.")
                MessageBox.Show("Updated successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MessageBox.Show("An error occurred while editing color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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