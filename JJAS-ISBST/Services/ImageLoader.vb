Imports System.Data
Imports System.Drawing

Public Module ImageLoader

    ''' <summary>
    ''' Adds a DataColumn (Image) and loads images from ImagePath into memory.
    ''' Safe: uses Bitmap copy to avoid locking files.
    ''' </summary>
    Public Sub AddAndLoadImages(dt As DataTable,
                               imagePathColumnName As String,
                               outputImageColumnName As String)

        If dt Is Nothing Then Return
        If Not dt.Columns.Contains(imagePathColumnName) Then Return

        If Not dt.Columns.Contains(outputImageColumnName) Then
            dt.Columns.Add(outputImageColumnName, GetType(Image))
        End If

        For Each row As DataRow In dt.Rows
            Dim path As String = row(imagePathColumnName).ToString()
            If Not String.IsNullOrWhiteSpace(path) AndAlso IO.File.Exists(path) Then
                Try
                    Using tempImg As Image = Image.FromFile(path)
                        row(outputImageColumnName) = New Bitmap(tempImg)
                    End Using
                Catch
                    row(outputImageColumnName) = Nothing
                End Try
            Else
                row(outputImageColumnName) = Nothing
            End If
        Next
    End Sub

End Module
