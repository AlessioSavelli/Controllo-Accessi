Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading

Public Class nuovo
    Private Sub nuovo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CaricaScuoleInComboBox(BoxScuola)
    End Sub


    Private Sub BoxEta_TextChanged(sender As Object, e As EventArgs) Handles BoxEta.TextChanged
        If BoxEta.TextLength > 6 Then
            If VerificaDataDiNascita(BoxEta.Text) Then
                BoxEta.BackColor = Color.Green
            Else
                BoxEta.BackColor = Color.Red
            End If
        Else
            BoxEta.BackColor = Color.White
        End If
    End Sub

    'Gestisce l'immagine di profilo
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Apre una finestra di dialogo per selezionare una foto
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "File immagine|*.png;*.jpg;*.jpeg"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog.FileName

            ' Carica l'immagine nella PictureBox
            PictureBox1.ImageLocation = selectedFilePath

            ' Salva il percorso del file nell'attributo Tag della PictureBox
            PictureBox1.Tag = selectedFilePath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim utente As New UserData
        utente.Name = BoxNome.Text
        utente.Birthday = ConvertiTestoInData(BoxEta.Text)
        utente.RegDate = DateTime.Now
        utente.School = BoxScuola.Text
#Region "Lettura del Genere"
        If RadioButton1.Checked Then
            utente.Gender = "M"
        ElseIf RadioButton2.Checked Then
            utente.Gender = "F"
        ElseIf RadioButton3.Checked Then
            utente.Gender = "ND"
        End If
#End Region

#Region "Carica la foto profilo"
        ' Controlla se il percorso dell'immagine è valido
        If PictureBox1.Tag IsNot Nothing Then
            Dim imagePath As String = PictureBox1.Tag.ToString()

            If Not String.IsNullOrWhiteSpace(imagePath) AndAlso File.Exists(imagePath) Then
                ' Cartella di destinazione
                Dim targetFolderPath As String = Path.Combine(Application.StartupPath, "conf/pic")

                ' Crea la cartella se non esiste
                If Not Directory.Exists(targetFolderPath) Then
                    Directory.CreateDirectory(targetFolderPath)
                End If

                ' Genera un nuovo nome per il file basato sull'hash MD5
                Dim md5 As MD5 = MD5.Create()
                Dim inputBytes As Byte() = Encoding.UTF8.GetBytes(Path.GetFileNameWithoutExtension(imagePath) + "PicPhotoHASH")
                Dim hashBytes As Byte() = md5.ComputeHash(inputBytes)
                Dim hashValue As String = BitConverter.ToString(hashBytes).Replace("-", "").ToLower()

                ' Estensione del file originale
                Dim fileExtension As String = Path.GetExtension(imagePath)

                ' Crea il percorso di destinazione per il file
                Dim targetFilePath As String = Path.Combine(targetFolderPath, hashValue + fileExtension)
                utente.PicPath = hashValue + fileExtension

                ' Sposta il file nell'archivio di destinazione
                File.Move(imagePath, targetFilePath)

                ' Aggiorna il percorso nel Tag della PictureBox
                PictureBox1.Tag = targetFilePath

                ' Aggiorna l'immagine nella PictureBox
                PictureBox1.ImageLocation = targetFilePath
            End If
        End If

#End Region

#Region "Crea badge"
        Dim write_just_new_badge As Boolean = True 'con questa opzione su true evita di riscrivere badge che gia' esistono salvati nel database
        Dim new_badge As New ScritturaBadge(ScritturaBadge.OverwriteOption.DO_NOT_OVERWRITE)

        ' Imposta le proprietà del controllo come desiderato, ad esempio dimensioni e posizione.
        new_badge.Size = Me.Size
        new_badge.Location = New Point(0, 0)
        new_badge.BackColor = Color.Transparent ' Imposta il colore di sfondo desiderato.
        new_badge._userData = utente
        ' Aggiungi il controllo alla finestra principale.
        Me.Controls.Add(new_badge)

        ' Porta il controllo in primo piano per sovrapporsi agli altri.
        new_badge.BringToFront()

        ' Rendi il controllo visibile.
        new_badge.Visible = True

        ' Imposta il focus sul controllo, se necessario.
        new_badge.Focus()
        Me.Tag = new_badge
#End Region
        Dim thread_waiting_fordb As New Thread(AddressOf WaitingForAddUser)
        thread_waiting_fordb.IsBackground = False
        thread_waiting_fordb.SetApartmentState(ApartmentState.STA)
        thread_waiting_fordb.Start(new_badge)

    End Sub
    Private Async Sub WaitingForAddUser(badgeWriter As ScritturaBadge)

        'Attende che il valore del badge venga sostituito dal form di modifica badge
        While Not (badgeWriter.WritingStatus = 5) 'magic number
            Thread.Sleep(350)
        End While

        'visto che non cambia alcun dato oltre all'id del badge aggiorno il database
        Await DataBase.AddNewUser(badgeWriter._userData)
    End Sub
    Private Sub nuovo_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Me.Tag IsNot Nothing Then
            Dim new_badge As ScritturaBadge = DirectCast(Me.Tag, ScritturaBadge)
            new_badge.closing()
        End If
    End Sub
End Class