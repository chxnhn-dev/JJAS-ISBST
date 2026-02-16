Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Category
    Private Sub Add_Brand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtBrand)
    End Sub

    Public Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        ' Validate fieldName to prevent SQL injection
        Dim allowedFields As String() = {"Category", "AnotherField"} ' list your allowed column names here
        If Not allowedFields.Contains(fieldName) Then
            Throw New ArgumentException("Invalid field name.")
        End If

        Dim sql As String = $"SELECT CASE WHEN EXISTS (
                            SELECT 1 FROM tbl_Category WHERE LOWER({fieldName}) = LOWER(@Value)
                         ) THEN 1 ELSE 0 END"

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
            MessageBox.Show("Category name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtBrand.Text.Contains("  ") Then
            MessageBox.Show("Category name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Category", fieldName) Then
            MessageBox.Show("Category already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If conn.State = ConnectionState.Closed Then conn.Open()

        If MessageBox.Show("Are you sure you want to add this category?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Try
                cmd = New SqlCommand("INSERT INTO tbl_Category (Category, DateCreated) 
                          VALUES (@Category, @DateCreated)", conn)

                With cmd.Parameters
                    .AddWithValue("@Category", txtBrand.Text.Trim())
                    .AddWithValue("@DateCreated", DateTime.Now)
                End With

                cmd.ExecuteNonQuery()
                conn.Close()

                LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, "Added Category.")

                MsgBox("Added successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("An error occurred while adding category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Private Sub txtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBrand.KeyPress
        ' Allow only letters, space, and control keys (like Backspace)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True ' Block the keypress
        End If
    End Sub


End Class