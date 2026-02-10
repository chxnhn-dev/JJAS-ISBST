Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Color
    Dim SizeId As String

    Private Sub Add_Brand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtBrand)
    End Sub

    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"
        SELECT CASE 
            WHEN EXISTS (
                SELECT 1 FROM tbl_Color WHERE LOWER({fieldName}) = LOWER(@Value)
            ) THEN 1 
            ELSE 0 
        END
    "

        Using conn As SqlConnection = DataAccess.GetConnection()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Value", fieldValue)
                conn.Open()
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBrand.KeyPress
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

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim fieldName As String = txtBrand.Text.Trim()

        If String.IsNullOrWhiteSpace(txtBrand.Text) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtBrand.Text.Length < 2 Then
            MessageBox.Show("Color name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtBrand.Text.Contains("  ") Then
            MessageBox.Show("Color name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Color", fieldName) Then
            MessageBox.Show("Color already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If conn.State = ConnectionState.Closed Then conn.Open()
        If MessageBox.Show("Are you sure you want to add this color?",
                       "Confirm Edit",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                cmd = New SqlCommand("INSERT INTO tbl_Color (Color, DateCreated) 
                          VALUES (@Color, @DateCreated)", conn)

                With cmd.Parameters
                    .AddWithValue("@Color", txtBrand.Text.Trim())
                    .AddWithValue("@DateCreated", DateTime.Now)
                End With

                cmd.ExecuteNonQuery()
                conn.Close()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added Color.")

                MsgBox("Added successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while adding color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub txtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBrand.KeyPress
        ' Allow only letters, space, and control keys (like Backspace)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True ' Block the keypress
        End If
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class