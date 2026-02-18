Imports System.Data.SqlClient
Imports JJAS_ISBST.FrmLogin

Public Class FrmColorEntry
    Public Property ColorID As Integer?

    Private ReadOnly Property IsEditMode As Boolean
        Get
            Return ColorID.HasValue
        End Get
    End Property

    Private Sub Add_Color_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BlockCopyPaste(txtBrand)
        ConfigureMode()

        If IsEditMode Then
            LoadColorForEdit()
        End If
    End Sub

    Private Sub ConfigureMode()
        If IsEditMode Then
            Me.Text = "Edit Color"
            btnAdd.Text = "  Update"
        Else
            Me.Text = "Add Color"
            btnAdd.Text = "  Save"
        End If
    End Sub

    Private Sub LoadColorForEdit()
        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand("SELECT Color FROM tbl_Color WHERE ColorID = @ColorID", connection)
                command.Parameters.AddWithValue("@ColorID", ColorID.Value)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtBrand.Text = reader("Color").ToString()
                    Else
                        MessageBox.Show("Selected color record was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Function IsDuplicate(fieldName As String, fieldValue As String) As Boolean
        Dim sql As String = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM tbl_Color WHERE LOWER({fieldName}) = LOWER(@Value)"

        If IsEditMode Then
            sql &= " AND ColorID <> @ColorID"
        End If

        sql &= ") THEN 1 ELSE 0 END"

        Using connection As SqlConnection = DataAccess.GetConnection()
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@Value", fieldValue)

                If IsEditMode Then
                    command.Parameters.AddWithValue("@ColorID", ColorID.Value)
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
        Dim colorName As String = txtBrand.Text.Trim()

        If String.IsNullOrWhiteSpace(colorName) Then
            MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If colorName.Length < 2 Then
            MessageBox.Show("Color name must be at least 2 characters long.", "Invalid Color", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If colorName.Contains("  ") Then
            MessageBox.Show("Color name cannot contain multiple consecutive spaces.", "Invalid Color", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If IsDuplicate("Color", colorName) Then
            MessageBox.Show("Color already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim actionText As String = If(IsEditMode, "update", "add")
        If MessageBox.Show($"Are you sure you want to {actionText} this color?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Exit Sub
        End If

        Try
            Using connection As SqlConnection = DataAccess.GetConnection()
                connection.Open()

                Dim sql As String
                If IsEditMode Then
                    sql = "UPDATE tbl_Color SET Color=@Color, DateCreated=@DateCreated WHERE ColorID=@ColorID"
                Else
                    sql = "INSERT INTO tbl_Color (Color, DateCreated) VALUES (@Color, @DateCreated)"
                End If

                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@Color", colorName)
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now)

                    If IsEditMode Then
                        command.Parameters.AddWithValue("@ColorID", ColorID.Value)
                    End If

                    command.ExecuteNonQuery()
                End Using
            End Using

            LogActivity(CurrentUser.UserID, CurrentUser.FullName, CurrentUser.Username, CurrentUser.Role, If(IsEditMode, "Edited Color.", "Added Color."))
            MessageBox.Show(If(IsEditMode, "Updated successfully.", "Added successfully!"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred while saving color: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
