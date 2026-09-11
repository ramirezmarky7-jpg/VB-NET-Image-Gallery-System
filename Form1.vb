Imports System.Drawing
Imports System.IO

Public Class Form1
    Private imagePaths As List(Of String)
    Private imageNames As List(Of String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Image Gallery"
        Me.Size = New Size(900, 600)

        imagePaths = New List(Of String)
        imageNames = New List(Of String)

        ' Create 5 sample images
        CreateImages()

        ' ListBox
        Dim lst As New ListBox()
        lst.Location = New Point(10, 10)
        lst.Size = New Size(120, 400)
        For i = 1 To 5
            lst.Items.Add("Image " & i)
        Next
        AddHandler lst.SelectedIndexChanged, AddressOf lst_Changed
        Me.Controls.Add(lst)

        ' PictureBox
        Dim pic As New PictureBox()
        pic.Name = "pic"
        pic.Location = New Point(150, 10)
        pic.Size = New Size(300, 250)
        pic.BorderStyle = BorderStyle.Fixed3D
        pic.SizeMode = PictureBoxSizeMode.StretchImage
        Me.Controls.Add(pic)

        ' Labels
        Dim lbl1 As New Label()
        lbl1.Name = "lbl1"
        lbl1.Location = New Point(150, 270)
        lbl1.Size = New Size(300, 30)
        Me.Controls.Add(lbl1)

        Dim lbl2 As New Label()
        lbl2.Name = "lbl2"
        lbl2.Location = New Point(150, 310)
        lbl2.Size = New Size(300, 30)
        Me.Controls.Add(lbl2)

        ' Chart
        Dim chart As New System.Windows.Forms.DataVisualization.Charting.Chart()
        chart.Location = New Point(470, 10)
        chart.Size = New Size(400, 350)
        Dim ca As New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        chart.ChartAreas.Add(ca)
        Dim series As New System.Windows.Forms.DataVisualization.Charting.Series()
        series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
        chart.Series.Add(series)
        For i = 1 To 5
            series.Points.AddXY("Image " & i, i * 10)
        Next
        Me.Controls.Add(chart)
    End Sub

    Private Sub CreateImages()
        Dim path = Application.StartupPath & "\Images\"
        If Not Directory.Exists(path) Then Directory.CreateDirectory(path)

        Dim colors As Color() = {Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Purple}
        
        For i = 1 To 5
            Dim file = path & "Image" & i & ".bmp"
            imagePaths.Add(file)
            imageNames.Add("Image " & i)
            
            If Not File.Exists(file) Then
                Dim bmp As New Bitmap(200, 150)
                Dim g As Graphics = Graphics.FromImage(bmp)
                g.Clear(colors(i - 1))
                g.DrawString("Image " & i, New Font("Arial", 14), Brushes.White, 50, 60)
                bmp.Save(file)
                g.Dispose()
            End If
        Next
    End Sub

    Private Sub lst_Changed(sender As Object, e As EventArgs)
        Dim lst As ListBox = CType(sender, ListBox)
        Dim idx = lst.SelectedIndex
        If idx >= 0 Then
            Dim pic As PictureBox = CType(Me.Controls("pic"), PictureBox)
            Dim lbl1 As Label = CType(Me.Controls("lbl1"), Label)
            Dim lbl2 As Label = CType(Me.Controls("lbl2"), Label)

            pic.Image = Image.FromFile(imagePaths(idx))
            lbl1.Text = "Name: " & imageNames(idx)
            lbl2.Text = "Size: " & New FileInfo(imagePaths(idx)).Length & " bytes"
        End If
    End Sub
End Class
