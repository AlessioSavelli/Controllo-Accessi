Imports System.Drawing.Printing
Imports System.IO


Public Class StampaBadge
    Public Event CloseRequested As EventHandler

    Public userData As UserData

    Private Sub StampaBadge_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox2.Text = "300"
        ComboBox2.Enabled = False ' va aggiustata la ridimensione per DPI
        ComboBox2.SelectedItem = 0
        ComboBox1.SelectedIndex = 0

        PictureSelector.Tag = userData.PicPath

        PictureSelector.Items.Add("Foto Profilo")
        'Carica le foto alternative di default
        If Not IO.Directory.Exists(Path_Progetto & "/conf/pic/def") Then
            If Not IO.Directory.Exists(Path_Progetto & "/conf/pic") Then
                Directory.CreateDirectory(Path_Progetto & "/conf/pic")
            End If
            Directory.CreateDirectory(Path_Progetto & "/conf/pic/def")
        End If


        ' Ottieni un elenco di tutti i file di immagine con estensioni JPG, PNG e Bitmap
        Dim allowedExtensions As String() = {".jpg", ".jpeg", ".png", ".bmp"}

        Dim imageFiles As New List(Of String)()

        For Each extension As String In allowedExtensions
            imageFiles.AddRange(Directory.GetFiles(Path_Progetto & "/conf/pic/def", "*" & extension))
        Next

        ' Itera attraverso l'elenco dei file di immagine
        For Each imagePath As String In imageFiles
            ' Aggiungi il nome del file (senza il percorso) alla ComboBox
            PictureSelector.Items.Add(Path.GetFileName(imagePath))
        Next

        ' Imposta l'elemento selezionato predefinito (se necessario)
        If PictureSelector.Items.Count > 0 Then
            PictureSelector.SelectedIndex = 0
        End If
        PictureSelector.Items.Add("Carica...")

        ' Imposta l'immagine dalle risorse di My.Resources.Stampa
        Dim imageFromResources As Image = PrintButton.Image

        ' Definisci le dimensioni desiderate per l'immagine all'interno del pulsante
        Dim desiredImageSize As Size = PrintButton.Size ' Modifica le dimensioni come preferisci

        ' Ridimensiona l'immagine alle dimensioni desiderate
        Dim resizedImage As New Bitmap(imageFromResources, desiredImageSize)

        ' Imposta l'immagine ridimensionata come immagine del pulsante
        PrintButton.Image = resizedImage



    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim conver_badge As New PrintableBadge(userData)
        Dim image As Image
        Select Case ComboBox1.SelectedIndex
            Case 0
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                image = conver_badge.GenerateBadgeImage(conver_badge.BigCard, CSng(ComboBox2.Text))
                PictureBox1.Image = image
                PictureBox1.Tag = image
            Case 1
                PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
                image = conver_badge.GenerateBadgeImage(conver_badge.SmallCard, CSng(ComboBox2.Text))
                PictureBox1.Image = image
                PictureBox1.Tag = image
            Case 2
                PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
                image = conver_badge.GenerateBadgeImage(conver_badge.CoinCard, CSng(ComboBox2.Text))
                PictureBox1.Image = image
                PictureBox1.Tag = image
        End Select

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ComboBox1_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RaiseEvent CloseRequested(Me, EventArgs.Empty)
    End Sub
    Private pageSettings As New PageSettings()
    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If PictureBox1.Tag Is Nothing Then Exit Sub

        If ExportJPG.Checked Then
            ' Ottieni l'immagine dalla PictureBox
            Dim originalImage As Image = DirectCast(PictureBox1.Tag, Image)

            ' Crea un'immagine di sfondo bianco delle stesse dimensioni dell'immagine originale
            Dim background As New Bitmap(originalImage.Width, originalImage.Height)
            Using g As Graphics = Graphics.FromImage(background)
                g.Clear(Color.White) ' Imposta lo sfondo bianco
                g.DrawImage(originalImage, 0, 0) ' Sovrapponi l'immagine originale
            End Using

            ' Esporta l'immagine come file JPEG
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "JPEG Image|*.jpg"
            If saveDialog.ShowDialog() = DialogResult.OK Then
                background.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
            End If

            ' Rilascia le risorse dell'immagine di sfondo
            background.Dispose()
        Else
            ' Crea un oggetto PrintDocument
            Dim pd As New PrintDocument()

            ' Imposta l'orientamento della pagina a "paesaggio"
            pd.DefaultPageSettings.Landscape = True

            ' Gestisci l'evento PrintPage per definire il contenuto della pagina da stampare
            AddHandler pd.PrintPage, AddressOf PrintPageHandler

            ' Crea un oggetto PrintPreviewDialog
            Dim ppd As New PrintPreviewDialog()
            ppd.Document = pd
            Button1.Tag = ppd
            ppd.ShowDialog()

        End If
    End Sub

    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        ' Recupera l'immagine dalla PictureBox
        Dim image As Image = DirectCast(PictureBox1.Tag, Image)

        ' Definisci i margini della pagina
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim topMargin As Integer = e.MarginBounds.Top

        ' Imposta l'orientamento della pagina in base alle impostazioni di pagina
        If pageSettings.Landscape Then
            ' Pagina orizzontale
            e.Graphics.DrawImage(image, topMargin, leftMargin)
        Else
            ' Pagina verticale
            e.Graphics.DrawImage(image, leftMargin, topMargin)
        End If

        ' Indica che non ci sono altre pagine da stampare
        e.HasMorePages = False


    End Sub


    Private Sub PictureSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PictureSelector.SelectedIndexChanged
        Select Case PictureSelector.Text
            Case "Foto Profilo"
                userData.PicPath = CStr(PictureSelector.Tag)
            Case "Carica..."
                ' Apre una finestra di dialogo per selezionare una foto
                Dim openFileDialog As New OpenFileDialog()
                openFileDialog.Filter = "File immagine|*.png;*.jpg;*.jpeg"

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim selectedFilePath As String = openFileDialog.FileName
                    If Not File.Exists(selectedFilePath) Then
                        MessageBox.Show("Il file sorgente non esiste.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                    Dim extract_name() As String = openFileDialog.FileName.Split("\")

                    Dim sourceFile As Byte() = IO.File.ReadAllBytes(selectedFilePath)

                    If IO.File.Exists(Path_Progetto & "/conf/pic/def/" & extract_name(extract_name.Length - 1)) Then
                        MessageBox.Show("Il file sorgente ha lo stesso nome di una foto gia' caricata, prego cambiare nome e ripetere l'operazione.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    IO.File.WriteAllBytes(Path_Progetto & "/conf/pic/def/" & extract_name(extract_name.Length - 1), sourceFile)
                    userData.PicPath = "def/" & extract_name(extract_name.Length - 1) 'cambia temporaneamente la foto di profilo da usare

                    PictureSelector.Items.RemoveAt(PictureSelector.Items.Count - 1)
                    PictureSelector.Items.Add(extract_name(extract_name.Length - 1))
                    PictureSelector.Items.Add("Carica...")
                    PictureSelector.SelectedIndex = (PictureSelector.Items.Count - 2)
                End If
                Exit Sub
            Case Else
                userData.PicPath = "def/" & PictureSelector.Text 'cambia temporaneamente la foto di profilo da usare
        End Select
        ComboBox1_SelectedIndexChanged(Nothing, Nothing)
    End Sub
End Class
