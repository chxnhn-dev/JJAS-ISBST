Imports System.Data
Imports System.Drawing

Public Module ImageLoader
    Private _missingProductImagePlaceholder As Image

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

    Public Function GetMissingProductImagePlaceholder() As Image
        If _missingProductImagePlaceholder IsNot Nothing Then
            Return _missingProductImagePlaceholder
        End If

        Dim bmp As New Bitmap(18, 18)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.White)
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
            Using borderPen As New Pen(Color.FromArgb(150, 150, 150), 1.0F),
                  xPen As New Pen(Color.FromArgb(230, 0, 0), 2.0F)
                g.DrawRectangle(borderPen, 0, 0, bmp.Width - 1, bmp.Height - 1)
                g.DrawLine(xPen, 4, 4, bmp.Width - 5, bmp.Height - 5)
                g.DrawLine(xPen, bmp.Width - 5, 4, 4, bmp.Height - 5)
            End Using
        End Using

        _missingProductImagePlaceholder = bmp
        Return _missingProductImagePlaceholder
    End Function

End Module
