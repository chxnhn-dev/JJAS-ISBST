Imports System.Data.SqlClient
Imports JJAS_ISBST.Login

Public Class Add_Brand
    Public Property BrandID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return BrandID.HasValue
        End Get
    End Property

    Private Sub Add_Brand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtbrand)
        ConfigureMode()

        If IsEditMode Then
            LoadBrandForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Brand"
            btnAdd.Text = "Update"
        Else
            Me.Text = "Add Brand"
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub LoadBrandForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Brand FROM tbl_Brand WHERE BrandID = @BrandID", connection)
                command.Parameters.AddWithValue("@BrandID", BrandID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtbrand.Text = reader("Brand").ToString()
                    Else
                        MessageBox.Show("Selected brand record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT COUNT(*) FROM tbl_Brand WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND BrandID <> @BrandID"
        End If

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@BrandID", BrandID.Value)
                End If

                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub txtbrand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbrand.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = False
            Exit Sub
        End If

        Dim allowedSymbols As String = "&'+-"
        If allowedSymbols.Contains(e.KeyChar) Then
            e.Handled = (txtbrand.Text.IndexOfAny(allowedSymbols.ToCharArray()) <> -1)
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim brandName As String = txtbrand.Text.Trim()

        If String.IsNullOrWhiteSpace(brandName) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If brandName.Length < 2 Then
            MessageBox.Show("Brand name must be at least 2 characters long.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If brandName.Contains("  ") Then
            MessageBox.Show("Brand name cannot contain multiple consecutive spaces.", "Invalid Brand", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Brand", brandName) Then
            MessageBox.Show("Brand already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this brand?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Brand SET Brand=@Brand, DateCreated=@DateCreated WHERE BrandID=@BrandID"
                Else
                    sql = "INSERT INTO tbl_Brand (Brand, DateCreated) VALUES (@Brand, @DateCreated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@Brand", brandName)
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@BrandID", BrandID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Brand.", "Added Brand."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving brand: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
