<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Add_Product
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Add_Product))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtSellingPrice = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.RichTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbSize = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbColor = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbCategory = New System.Windows.Forms.ComboBox()
        Me.cbBrand = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBarcodeNumber = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.Panel1.Controls.Add(Me.btnBrowse)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.txtSellingPrice)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtDescription)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cbSize)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cbColor)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cbCategory)
        Me.Panel1.Controls.Add(Me.cbBrand)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtProductName)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtBarcodeNumber)
        Me.Panel1.Controls.Add(Me.btnAdd)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 45)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(869, 387)
        Me.Panel1.TabIndex = 77
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBrowse.FlatAppearance.BorderSize = 0
        Me.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowse.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowse.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnBrowse.Location = New System.Drawing.Point(12, 281)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(246, 45)
        Me.btnBrowse.TabIndex = 132
        Me.btnBrowse.Text = "BROWSE"
        Me.btnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label9.Location = New System.Drawing.Point(12, 13)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(129, 25)
        Me.Label9.TabIndex = 131
        Me.Label9.Text = "Product Img:"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.PictureBox1.Location = New System.Drawing.Point(12, 41)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(246, 225)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 130
        Me.PictureBox1.TabStop = False
        '
        'txtSellingPrice
        '
        Me.txtSellingPrice.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSellingPrice.Location = New System.Drawing.Point(568, 105)
        Me.txtSellingPrice.MaxLength = 14
        Me.txtSellingPrice.Name = "txtSellingPrice"
        Me.txtSellingPrice.Size = New System.Drawing.Size(289, 33)
        Me.txtSellingPrice.TabIndex = 129
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label7.Location = New System.Drawing.Point(563, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(126, 25)
        Me.Label7.TabIndex = 128
        Me.Label7.Text = "Selling Price:"
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(568, 168)
        Me.txtDescription.MaxLength = 200
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(289, 158)
        Me.txtDescription.TabIndex = 82
        Me.txtDescription.Text = ""
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label5.Location = New System.Drawing.Point(563, 140)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(215, 25)
        Me.Label5.TabIndex = 81
        Me.Label5.Text = "Description: (Optional)"
        '
        'cbSize
        '
        Me.cbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSize.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSize.FormattingEnabled = True
        Me.cbSize.Location = New System.Drawing.Point(266, 293)
        Me.cbSize.Name = "cbSize"
        Me.cbSize.Size = New System.Drawing.Size(291, 33)
        Me.cbSize.TabIndex = 80
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label8.Location = New System.Drawing.Point(264, 265)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 25)
        Me.Label8.TabIndex = 79
        Me.Label8.Text = "Size:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label3.Location = New System.Drawing.Point(264, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 25)
        Me.Label3.TabIndex = 78
        Me.Label3.Text = "Color:"
        '
        'cbColor
        '
        Me.cbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColor.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbColor.FormattingEnabled = True
        Me.cbColor.Location = New System.Drawing.Point(268, 165)
        Me.cbColor.Name = "cbColor"
        Me.cbColor.Size = New System.Drawing.Size(288, 33)
        Me.cbColor.TabIndex = 77
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label4.Location = New System.Drawing.Point(262, 201)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 25)
        Me.Label4.TabIndex = 75
        Me.Label4.Text = "Category:"
        '
        'cbCategory
        '
        Me.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCategory.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbCategory.FormattingEnabled = True
        Me.cbCategory.Items.AddRange(New Object() {"Select option"})
        Me.cbCategory.Location = New System.Drawing.Point(268, 229)
        Me.cbCategory.Name = "cbCategory"
        Me.cbCategory.Size = New System.Drawing.Size(289, 33)
        Me.cbCategory.TabIndex = 76
        '
        'cbBrand
        '
        Me.cbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBrand.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbBrand.FormattingEnabled = True
        Me.cbBrand.Location = New System.Drawing.Point(269, 101)
        Me.cbBrand.Name = "cbBrand"
        Me.cbBrand.Size = New System.Drawing.Size(288, 33)
        Me.cbBrand.TabIndex = 74
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label2.Location = New System.Drawing.Point(264, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 25)
        Me.Label2.TabIndex = 73
        Me.Label2.Text = "Brand:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label1.Location = New System.Drawing.Point(563, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 25)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "Product Name:"
        '
        'txtProductName
        '
        Me.txtProductName.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProductName.Location = New System.Drawing.Point(568, 41)
        Me.txtProductName.MaxLength = 50
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(290, 33)
        Me.txtProductName.TabIndex = 72
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label6.Location = New System.Drawing.Point(264, 13)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(170, 25)
        Me.Label6.TabIndex = 69
        Me.Label6.Text = "Barcode Number:"
        '
        'txtBarcodeNumber
        '
        Me.txtBarcodeNumber.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarcodeNumber.Location = New System.Drawing.Point(269, 41)
        Me.txtBarcodeNumber.MaxLength = 50
        Me.txtBarcodeNumber.Name = "txtBarcodeNumber"
        Me.txtBarcodeNumber.Size = New System.Drawing.Size(288, 33)
        Me.txtBarcodeNumber.TabIndex = 70
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(746, 338)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(84, 37)
        Me.btnAdd.TabIndex = 68
        Me.btnAdd.Text = "  Add"
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExit.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatAppearance.BorderSize = 0
        Me.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnExit.Image = CType(resources.GetObject("btnExit.Image"), System.Drawing.Image)
        Me.btnExit.Location = New System.Drawing.Point(834, 0)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(35, 45)
        Me.btnExit.TabIndex = 78
        Me.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label12.Location = New System.Drawing.Point(8, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(131, 25)
        Me.Label12.TabIndex = 76
        Me.Label12.Text = "Add Product:"
        '
        'Add_Product
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(869, 432)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label12)
        Me.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Add_Product"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add_Product"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents cbSize As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cbColor As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cbCategory As ComboBox
    Friend WithEvents cbBrand As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtProductName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtBarcodeNumber As TextBox
    Friend WithEvents txtDescription As RichTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtSellingPrice As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnBrowse As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents PictureBox1 As PictureBox
End Class
