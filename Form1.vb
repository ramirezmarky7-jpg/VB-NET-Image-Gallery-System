Imports System.Drawing
Imports System.IO

Public Class Form1
    Private imagePaths As List(Of String)
    Private imageNames As List(Of String)
    Private currentImageIndex As Integer = -1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Image Gallery System"
        Me.Size = New Size(1000, 700)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Initialize lists
        imagePaths = New List(Of String)
        imageNames = New List(Of String)

        ' Create sample images and add paths
        CreateSampleImages()

        ' Setup UI Controls
        SetupControls()

        ' Load images into ListBox
        LoadImagesIntoListBox()
    End Sub

    Private Sub CreateSampleImages()
        ' Define image paths
        Dim imagePath As String = Application.StartupPath & "\Images\"
        
        If Not Directory.Exists(imagePath) Then
            Directory.CreateDirectory(imagePath)
        End If

        ' Create 5 sample images if they don't exist
        Dim imageCount As Integer = 5
        For i As Integer = 1 To imageCount
            Dim filePath As String = imagePath & "Image" & i & ".bmp"
            If Not File.Exists(filePath) Then
                CreateSampleImage(filePath, 200, 150, i)
            End If
            imagePaths.Add(filePath)
            imageNames.Add("Image " & i)
        Next
    End Sub

    Private Sub CreateSampleImage(filePath As String, width As Integer, height As Integer, imageNumber As Integer)
        Dim bmp As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(bmp)

        ' Fill with different colors for each image
        Select Case imageNumber
            Case 1
                g.Clear(Color.Red)
            Case 2
                g.Clear(Color.Blue)
            Case 3
                g.Clear(Color.Green)
            Case 4
                g.Clear(Color.Yellow)
            Case 5
                g.Clear(Color.Purple)
        End Select

        ' Draw text on image
        Dim font As New Font("Arial", 16, FontStyle.Bold)
        Dim brush As New SolidBrush(Color.White)
        g.DrawString("Image " & imageNumber, font, brush, 50, 60)

        bmp.Save(filePath)
        g.Dispose()
        brush.Dispose()
        font.Dispose()
    End Sub

    Private Sub SetupControls()
        ' ListBox for image selection
        Dim lstImages As New ListBox()
        lstImages.Name = "lstImages"
        lstImages.Location = New Point(10, 10)
        lstImages.Size = New Size(150, 400)
        AddHandler lstImages.SelectedIndexChanged, AddressOf ListBox_SelectedIndexChanged
        Me.Controls.Add(lstImages)

        ' Label for "Images"
        Dim lblImageList As New Label()
        lblImageList.Text = "Images:"
        lblImageList.Location = New Point(10, -20)
        lblImageList.Size = New Size(150, 30)
        lblImageList.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblImageList)

        ' PictureBox for image display
        Dim picBox As New PictureBox()
        picBox.Name = "picBox"
        picBox.Location = New Point(170, 10)
        picBox.Size = New Size(400, 350)
        picBox.BorderStyle = BorderStyle.Fixed3D
        picBox.SizeMode = PictureBoxSizeMode.StretchImage
        picBox.BackColor = Color.LightGray
        Me.Controls.Add(picBox)

        ' Label for image name
        Dim lblImageName As New Label()
        lblImageName.Name = "lblImageName"
        lblImageName.Text = "Image Name: -"
        lblImageName.Location = New Point(170, 365)
        lblImageName.Size = New Size(400, 25)
        lblImageName.Font = New Font("Arial", 10)
        Me.Controls.Add(lblImageName)

        ' Label for image size
        Dim lblImageSize As New Label()
        lblImageSize.Name = "lblImageSize"
        lblImageSize.Text = "Image Size: -"
        lblImageSize.Location = New Point(170, 390)
        lblImageSize.Size = New Size(400, 25)
        lblImageSize.Font = New Font("Arial", 10)
        Me.Controls.Add(lblImageSize)

        ' Label for file size
        Dim lblFileSize As New Label()
        lblFileSize.Name = "lblFileSize"
        lblFileSize.Text = "File Size: -"
        lblFileSize.Location = New Point(170, 415)
        lblFileSize.Size = New Size(400, 25)
        lblFileSize.Font = New Font("Arial", 10)
        Me.Controls.Add(lblFileSize)

        ' Chart for image statistics
        Dim chart As New System.Windows.Forms.DataVisualization.Charting.Chart()
        chart.Name = "chartStats"
        chart.Location = New Point(580, 10)
        chart.Size = New Size(400, 300)
        chart.BackColor = Color.White
        
        Dim chartArea As New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        chart.ChartAreas.Add(chartArea)
        
        Dim series As New System.Windows.Forms.DataVisualization.Charting.Series()
        series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
        chart.Series.Add(series)
        
        chart.Titles.Add("Image File Sizes (KB)")
        
        Me.Controls.Add(chart)

        ' Label for chart title
        Dim lblChartTitle As New Label()
        lblChartTitle.Text = "Statistics:"
        lblChartTitle.Location = New Point(580, -20)
        lblChartTitle.Size = New Size(400, 30)
        lblChartTitle.Font = New Font("Arial", 12, FontStyle.Bold)
        Me.Controls.Add(lblChartTitle)

        ' Button to refresh chart
        Dim btnRefresh As New Button()
        btnRefresh.Text = "Refresh Chart"
        btnRefresh.Location = New Point(580, 320)
        btnRefresh.Size = New Size(400, 40)
        btnRefresh.Font = New Font("Arial", 10)
        AddHandler btnRefresh.Click, AddressOf BtnRefresh_Click
        Me.Controls.Add(btnRefresh)

        ' Info label at bottom
        Dim lblInfo As New Label()
        lblInfo.Name = "lblInfo"
        lblInfo.Text = "Select an image from the list to view details"
        lblInfo.Location = New Point(10, 620)
        lblInfo.Size = New Size(970, 30)
        lblInfo.Font = New Font("Arial", 9, FontStyle.Italic)
        lblInfo.ForeColor = Color.Gray
        Me.Controls.Add(lblInfo)
    End Sub

    Private Sub LoadImagesIntoListBox()
        Dim lstImages As ListBox = CType(Me.Controls("lstImages"), ListBox)
        lstImages.Items.Clear()
        For Each name As String In imageNames
            lstImages.Items.Add(name)
        Next
    End Sub

    Private Sub ListBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim lstImages As ListBox = CType(sender, ListBox)
        currentImageIndex = lstImages.SelectedIndex

        If currentImageIndex >= 0 Then
            DisplayImage(currentImageIndex)
            UpdateChart()
        End If
    End Sub

    Private Sub DisplayImage(index As Integer)
        Dim picBox As PictureBox = CType(Me.Controls("picBox"), PictureBox)
        Dim lblImageName As Label = CType(Me.Controls("lblImageName"), Label)
        Dim lblImageSize As Label = CType(Me.Controls("lblImageSize"), Label)
        Dim lblFileSize As Label = CType(Me.Controls("lblFileSize"), Label)

        If index >= 0 AndAlso index < imagePaths.Count Then
            Dim imagePath As String = imagePaths(index)
            
            If File.Exists(imagePath) Then
                Dim img As Image = Image.FromFile(imagePath)
                picBox.Image = img

                ' Update labels
                lblImageName.Text = "Image Name: " & imageNames(index)
                lblImageSize.Text = "Image Size: " & img.Width & "x" & img.Height & " pixels"

                ' Get file size
                Dim fileInfo As New FileInfo(imagePath)
                Dim fileSizeKB As Double = fileInfo.Length / 1024
                lblFileSize.Text = "File Size: " & fileSizeKB.ToString("F2") & " KB"
            End If
        End If
    End Sub

    Private Sub UpdateChart()
        Dim chart As System.Windows.Forms.DataVisualization.Charting.Chart = _
            CType(Me.Controls("chartStats"), System.Windows.Forms.DataVisualization.Charting.Chart)
        
        Dim series As System.Windows.Forms.DataVisualization.Charting.Series = chart.Series(0)
        series.Points.Clear()

        ' Add data points for all images
        For i As Integer = 0 To imagePaths.Count - 1
            If File.Exists(imagePaths(i)) Then
                Dim fileInfo As New FileInfo(imagePaths(i))
                Dim fileSizeKB As Double = fileInfo.Length / 1024
                series.Points.AddXY(imageNames(i), fileSizeKB)
            End If
        Next
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs)
        UpdateChart()
        MessageBox.Show("Chart refreshed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
