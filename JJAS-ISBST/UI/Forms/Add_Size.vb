Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Measurement
    Public Property SizeID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return SizeID.HasValue
        End Get
    End Property

    Private Sub Add_Size_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtSize)
        BlockCopyPaste(txtDescription)
        ConfigureMode()

        If IsEditMode Then
            LoadSizeForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Size"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Size"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadSizeForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Size, Description FROM tbl_Size WHERE SizeID = @SizeID", connection)
                command.Parameters.AddWithValue("@SizeID", SizeID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtSize.Text = reader("Size").ToString()
                        txtDescription.Text = reader("Description").ToString()
                    Else
                        MessageBox.Show("Selected size record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM tbl_Size WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND SizeID <> @SizeID"
        End If

        sql &= ") THEN 1 ELSE 0 END"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@SizeID", SizeID.Value)
                End If

                connection.Open()
                Return Convert.ToBoolean(command.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSize.KeyPress
        Dim tb As TextBox = CType(sender, TextBox)

        If tb.SelectionStart = 0 Then
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        Else
            If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                If e.KeyChar = "-"c Then
                    If tb.Text.Contains("-") Then
                        e.Handled = True
                    End If
                Else
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim sizeValue As String = txtSize.Text.Trim()
        Dim description As String = txtDescription.Text.Trim()

        If String.IsNullOrWhiteSpace(sizeValue) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If sizeValue.Length < 2 Then
            MessageBox.Show("Size name must be at least 2 characters long.", "Invalid Size", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If sizeValue.Contains("  ") Then
            MessageBox.Show("Size name cannot contain multiple consecutive spaces.", "Invalid Size", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Size", sizeValue) Then
            MessageBox.Show("Size already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this size?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Size SET Size=@Size, Description=@Description, DateCreated=@DateCreated WHERE SizeID=@SizeID"
                Else
                    sql = "INSERT INTO tbl_Size (Size, Description, DateCreated) VALUES (@Size, @Description, @DateCreated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@Size", sizeValue)
                    command.Parameters.AddWithValue("@Description", description)
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@SizeID", SizeID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Size.", "Added Size."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving size: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
