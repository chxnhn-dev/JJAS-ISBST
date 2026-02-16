Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports JJAS_ISBST.Login
Public Class Add_Measurement
    Dim SizeId As String

    Private Sub Add_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSize)
        BlockCopyPaste(txtDescription)
    End Sub

    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"
        SELECT CASE 
            WHEN EXISTS (
                SELECT 1 FROM tbl_Size WHERE LOWER({fieldName}) = LOWER(@Value)
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
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim Size As String = txtSize.Text.Trim

        If String.IsNullOrWhiteSpace(txtSize.Text) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        If IsDuplicate("Size", Size) Then
            MessageBox.Show("Size already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If conn.State = ConnectionState.Closed Then conn.Open()

        If MessageBox.Show("Are you sure you want to add this size?",
               "Confirm Edit",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


            Try
                cmd = New SqlCommand("INSERT INTO tbl_Size (Size, Description, DateCreated) 
                          VALUES (@Size, @Description, @DateCreated)", conn)

                With cmd.Parameters
                    .AddWithValue("@Size", txtSize.Text.Trim())
                    .AddWithValue("@Description", txtDescription.Text.Trim())
                    .AddWithValue("@DateCreated", DateTime.Now)
                End With

                cmd.ExecuteNonQuery()
                conn.Close()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added Size.")

                MsgBox("Added successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while adding user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class