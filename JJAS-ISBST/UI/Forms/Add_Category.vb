Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Category
    Public Property CategoryID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return CategoryID.HasValue
        End Get
    End Property

    Private Sub Add_Category_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtBrand)
        ConfigureMode()

        If IsEditMode Then
            LoadCategoryForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Category"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Category"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadCategoryForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Category FROM tbl_Category WHERE CategoryID = @CategoryID", connection)
                command.Parameters.AddWithValue("@CategoryID", CategoryID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtBrand.Text = reader("Category").ToString()
                    Else
                        MessageBox.Show("Selected category record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM tbl_Category WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND CategoryID <> @CategoryID"
        End If

        sql &= ") THEN 1 ELSE 0 END"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@CategoryID", CategoryID.Value)
                End If

                connection.Open()
                Return Convert.ToBoolean(command.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub txtBrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBrand.KeyPress
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
        Dim categoryName As String = txtBrand.Text.Trim()

        If String.IsNullOrWhiteSpace(categoryName) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If categoryName.Length < 2 Then
            MessageBox.Show("Category name must be at least 2 characters long.", "Invalid Category", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If categoryName.Contains("  ") Then
            MessageBox.Show("Category name cannot contain multiple consecutive spaces.", "Invalid Category", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Category", categoryName) Then
            MessageBox.Show("Category already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this category?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Category SET Category=@Category, DateCreated=@DateCreated WHERE CategoryID=@CategoryID"
                Else
                    sql = "INSERT INTO tbl_Category (Category, DateCreated) VALUES (@Category, @DateCreated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@Category", categoryName)
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@CategoryID", CategoryID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Category.", "Added Category."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
