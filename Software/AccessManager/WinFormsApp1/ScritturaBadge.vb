Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports RJCP.IO.Ports

Public Class ScritturaBadge

    Public _userData As UserData

    Private Badge As InterfacciaPN532
    Private SerialPortStream As New SerialPortStream

    Public Enum OverwriteOption
        DO_NOT_OVERWRITE
        ASK_BEFOR_OVERWRITE
        JUST_OVERWRITE
    End Enum

    Dim option_ov As OverwriteOption

    Public Enum WritingSTEPs
        WAITING_INTERFACE
        WAITING_BADGE
        GET_UID_BADGE
        WAITING_OVERWRITEOPTION
        WRITING_BADGE
        WRITING_OK
        WRITING_FAIL
    End Enum
    Public WritingStatus As WritingSTEPs = WritingSTEPs.WAITING_INTERFACE

    Sub New(Optional ByVal _option As OverwriteOption = OverwriteOption.DO_NOT_OVERWRITE)

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()
        option_ov = _option
        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Public Sub closing()
        If Badge IsNot Nothing Then
            Try
                Badge.Close()
            Catch
            End Try
        End If
    End Sub

    Private Sub ScritturaBadge_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim portNames As String() = SerialPortStream.GetPortNames()
        Dim th_research As New Thread(AddressOf WritingBadge)
        th_research.IsBackground = False
        th_research.SetApartmentState(ApartmentState.STA)
        Output.ForeColor = Color.DarkGreen
        th_research.Start()
    End Sub


    Private Sub TryAgain_Click(sender As Object, e As EventArgs) Handles TryAgain.Click
        Dim portNames As String() = SerialPortStream.GetPortNames()
        Dim th_research As New Thread(AddressOf WritingBadge)
        th_research.IsBackground = False
        th_research.SetApartmentState(ApartmentState.STA)
        Output.ForeColor = Color.DarkGreen
        Output.Text = "Nuovo Tentativo in corso..."
        TryAgain.Visible = False
        th_research.Start()
    End Sub
    Public Function GetValidCOMPorts(filePath As String) As List(Of String)
        Invoke(Sub() PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/loading.gif"))
        Dim validCOMPorts As New List(Of String)

        Try
            ' Legge il contenuto del file
            Dim lines() As String = File.ReadAllLines(filePath)

            ' Utilizza una regex per verificare se ogni linea corrisponde al formato "COMx" dove x è un numero
            Dim regex As New Regex("^COM\d+$")

            For Each line As String In lines
                If regex.IsMatch(line) Then
                    validCOMPorts.Add(line)
                End If
            Next
        Catch ex As Exception
            ' Gestisci l'eccezione, ad esempio mostrando un messaggio di errore
            MessageBox.Show("Impossibile leggere il file per estrarre le porte COM valide.")
            MYlogManager.LogMessage("Errore lettura del file per estrarre le porte COM valide." & ex.Message, "GetValidCOMPorts-ScritturaBadge")
        End Try

        Return validCOMPorts
    End Function
    Private Async Sub WritingBadge()
        Dim uid_badge As String

        Dim portNames As Object
        If IO.File.Exists(Path_Progetto & "/conf/badge.conf") Then
            portNames = GetValidCOMPorts(Path_Progetto & "/conf/badge.conf")
        Else
            portNames = SerialPortStream.GetPortNames()
        End If
        WritingStatus = WritingSTEPs.WAITING_INTERFACE
#Region "Identifica lettore badge"
        For Each portName As String In portNames
            Try
                Dim periferica As New InterfacciaPN532(portName) ' Imposta la velocità della porta seriale a 9600 baud o altra velocità appropriata
                Thread.Sleep(4000) ' attende mezzo secondo che la periferica si accenda
                Dim firmwareVersion As Byte = Await periferica.RequestVersion()

                ' Verifica se il firmwareVersion è valido per il tuo dispositivo PN532
                ' Puoi confrontarlo con un valore noto o una gamma di valori validi

                If IsValidFirmwareVersion(firmwareVersion) Then
                    ' Porta COM corretta trovata
                    Badge = periferica
                    Invoke(Sub() Output.Text = "Periferica trovata. - " & portName)
                    Exit For
                End If
                periferica.Close()
            Catch ex As Exception
                ' Gestisci eventuali eccezioni qui
                MYlogManager.LogMessage("Porta " & portName & " Occupata." & vbCrLf & ex.Message, "GetValidCOMPorts-ScritturaBadge")
            End Try
        Next

#End Region

        If Badge Is Nothing Then
            ' Nessuna porta COM corretta trovata

            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.ForeColor = Color.Red
                       Output.Text = "Nessuna porta COM corretta trovata."
                       TryAgain.Visible = True
                   End Sub)
            WritingStatus = WritingSTEPs.WRITING_FAIL
            Exit Sub
        End If
        WritingStatus = WritingSTEPs.WAITING_BADGE
#Region "Attendi lettura uid badge"
        Invoke(Sub()
                   Output.Text = "Attesa lettura badge."
               End Sub)
        Dim b As Boolean = Badge.WaitForBadge().Result
        If b = False Then
            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.Text = "Errore lettura badge."
                   End Sub)
            Badge.Close()
            WritingStatus = WritingSTEPs.WRITING_FAIL
            Exit Sub
        End If

        uid_badge = Badge.GetBadgeUID().Result.Replace("&H", "0x").Replace("&h", "0x")
        _userData.Badge = uid_badge
        WritingStatus = WritingSTEPs.GET_UID_BADGE
#End Region

#Region "Verifica che l'uid del badge non e' gia' presente nel database"
        Dim findUser As List(Of UserData) = Await DataBase.ExtractUserByBadgeAsync(uid_badge)
        'significa che ha trovato un'utente con lo stesso badge
        If findUser IsNot Nothing AndAlso findUser.Count > 0 Then
            WritingStatus = WritingSTEPs.WAITING_OVERWRITEOPTION
            Select Case option_ov
                Case OverwriteOption.DO_NOT_OVERWRITE
                    Invoke(Sub()
                               PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                               Output.ForeColor = Color.Red
                               Output.Text = "Il badge che sta cercando di scrivere e' gia' associato a un'altro alunno." & vbCrLf & "Nome : " & findUser(0).Name & vbCrLf & "Registrato il : " & findUser(0).RegDate
                               TryAgain.Visible = True
                           End Sub)
                    WritingStatus = WritingSTEPs.WRITING_FAIL
                    Badge.Close()
                    Exit Sub
                Case OverwriteOption.ASK_BEFOR_OVERWRITE
                    ' Attiva il form padre o principale
                    Me.Invoke(Sub() Me.BringToFront())
                    Dim res As DialogResult = MessageBox.Show("Vuoi sovrascrivere il badge corrente che appartiene a :" & vbCrLf & "Nome : " & findUser(0).Name & vbCrLf & "Registrato il : " & findUser(0).RegDate, "Conferma", MessageBoxButtons.YesNo)
                    If res <> DialogResult.Yes Then
                        Invoke(Sub()
                                   PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                                   Output.ForeColor = Color.Red
                                   Output.Text = "Il badge che sta cercando di scrivere e' gia' associato a un'altro alunno." & vbCrLf & "Nome : " & findUser(0).Name & vbCrLf & "Registrato il : " & findUser(0).RegDate & vbCrLf & "!!!Hai deciso di non sovrascriverlo!!!"
                                   TryAgain.Visible = True
                               End Sub)
                        Badge.Close()
                        WritingStatus = WritingSTEPs.WRITING_FAIL
                        Exit Sub
                    End If
                Case OverwriteOption.JUST_OVERWRITE
            End Select
        End If
#End Region

        Invoke(Sub()
                   Output.Text = "Scrittura su Badge '" & uid_badge & "'."
               End Sub)
        WritingStatus = WritingSTEPs.WRITING_BADGE
        Dim defineProcessor As New DefineProcessor()
#Region "Lettura del brendsector da file"

        Dim filePath As String = Path_Progetto & "/conf/brandsector.conf"
        ' Carica le definizioni dal file
        defineProcessor.ProcessDefines(filePath)
        ' Trova e processa la riga di output
        Dim brendsector_string As String = defineProcessor.ProcessLine(filePath)
        Dim brendsector() As Byte = ConvertHexStringToByteArray(brendsector_string)
        If brendsector.Length <> 16 Then
            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.ForeColor = Color.Red
                       Output.Text = "Brandsector error. File /conf/brandsector.conf non valido"
                       TryAgain.Visible = True
                   End Sub)
            Badge.Close()

            Exit Sub
        End If

#End Region

#Region "Ottengo la riga due da scrivere che definisce i parametri di login al lettore"
        defineProcessor = New DefineProcessor
        defineProcessor.ProcessDefines(Path_Progetto & "/conf/line1.conf")
        Dim secndLineString As String = defineProcessor.ProcessLine(Path_Progetto & "/conf/line1.conf")
        Dim line1() As Byte = ConvertHexStringToByteArray(brendsector_string)

        If Not (line1 IsNot Nothing AndAlso line1.Length = 16) Then
            ' Si è verificato un errore nella lettura dei primi 16 byte

            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.ForeColor = Color.Red
                       Output.Text = "line1 error. File /conf/line1.conf non valido"
                       TryAgain.Visible = True
                   End Sub)
            Badge.Close()
            WritingStatus = WritingSTEPs.WRITING_FAIL
            Exit Sub
        End If
#End Region

        Dim lcd_lin() As Byte = GeneraNomeUtente(_userData.Name)


        Dim result As InterfacciaPN532.SERIAL_HEADER = Badge.writeBadge(line1, lcd_lin, 1, brendsector).Result

        If result = InterfacciaPN532.SERIAL_HEADER.ERROR_TIMEOUT Then

            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.ForeColor = Color.Red
                       Output.Text = "Dispostio per la scrittura ha datto l'errore TIMEOUT. Riprovare, se l'errore persiste contattare il supporto!"
                       TryAgain.Visible = True
                   End Sub)
            Badge.Close()
            WritingStatus = WritingSTEPs.WRITING_FAIL
            Exit Sub
        End If
        If Not (result = InterfacciaPN532.SERIAL_HEADER.SUCCESS_WRITE_BADGE) Then
            'se la scrittura non e' andata a buon fine allora insiste 3/4 volte
            For i = 2 To 16 'tenta di trovare un settore del badge scrivibile
                Invoke(Sub()
                           Output.ForeColor = Color.Orange
                           Output.Text = "Tentativo di scrittura " & i & " / 16"
                       End Sub)
                result = Badge.writeBadge(line1, lcd_lin, i, brendsector).Result
                If (result = InterfacciaPN532.SERIAL_HEADER.SUCCESS_WRITE_BADGE) Then
                    Exit For
                End If
            Next
            Invoke(Sub()
                       PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/ErrorBadge.gif")
                       Output.ForeColor = Color.Red
                       Output.Text = "Impossibile scrivere sul badge, prova con un'altro!"
                       TryAgain.Visible = True
                   End Sub)
            Badge.Close()
            WritingStatus = WritingSTEPs.WRITING_FAIL
            Exit Sub
        End If
        Invoke(Sub()
                   PictureBox1.Image = Image.FromFile(Path_Progetto & "/conf/extra/SuccessBadge.gif")
                   Output.ForeColor = Color.DarkGreen
                   Output.Text = "Badge scritto con successo!"
               End Sub)

        'Adesso aggiunge l'utente del database

        WritingStatus = WritingSTEPs.WRITING_OK
        Badge.Close()
        Exit Sub
    End Sub

    ' Aggiungi la tua funzione IsValidFirmwareVersion per verificare la versione del firmware qui

    Private Function IsValidFirmwareVersion(ByVal firmwareVersion As Byte) As Boolean
        ' Estrai la parte più significativa dei 8 bit dalla versione del firmware
        Dim chipIdentifier As Byte = &H7 'Magic Number
        ' Verifica se il chipIdentifier corrisponde a quello del PN532
        If chipIdentifier = firmwareVersion Then ' &H32 rappresenta il valore esadecimale per "2"
            Return True ' La versione del firmware indica un chip PN532 valido
        End If
        Return False ' La versione del firmware non corrisponde a un chip PN532

    End Function

    Private Function GeneraNomeUtente(Username As String) As Byte()
        ' Imposta una lunghezza massima di 16 caratteri
        Dim maxLength As Integer = 16
        Dim nomeUtente As String = Username.Trim() ' Rimuovi spazi bianchi iniziali e finali
        ' Limita il nome utente alla lunghezza massima
        If nomeUtente.Length > maxLength Then
            nomeUtente = nomeUtente.Substring(0, maxLength)
        End If
        ' Aggiungi spazi vuoti per raggiungere la lunghezza desiderata
        nomeUtente = nomeUtente.PadRight(maxLength)
        ' Converti il nome utente in un array di byte UTF-8
        Dim utf8 As New UTF8Encoding()
        Dim nomeUtenteBytes As Byte() = utf8.GetBytes(nomeUtente)
        Return nomeUtenteBytes
    End Function


End Class
