Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports WinFormsApp1.MQTTnet.Samples.Client

Public Class main


    Shared MQTT_CLIENT As ClientMQTT
    Dim MQTT_USER As String
    Dim maintopic As String = ""





    Dim lettore_badge_form As New Attivatore
    Dim accessi_mensili_form As New AccessList
    Dim aula_virtuale_form As New AlunniPresenti

    Dim logsBox As LogExplorer
    Private Sub LOGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LOGToolStripMenuItem.Click
        If logsBox Is Nothing OrElse logsBox.IsDisposed Then
            logsBox = New LogExplorer()
            logsBox.Show()
        Else
            logsBox.Activate()
        End If
    End Sub

#Region "MQTT-Manager"
    Private Async Sub Connect_mqtt()
        Using newAskPass As New BrokerPassword
            If newAskPass.ShowDialog() = DialogResult.OK Then
                Await MQTT_CLIENT.Connect_Client(MQTT_USER, newAskPass.password)
                If MQTT_CLIENT.OUTPUT = "Server Connesso" Then
                    StatusBar.Value = 100
                    subscribe_topic()
                Else
                    StatusBar.Value = 0
                End If
                StatusText.Text = If(MQTT_CLIENT.OUTPUT = "Server Connesso", "Attesa lettore", "Server non connesso")
            Else
                StatusText.Text = "Inserire password"
                StatusBar.Value = 0
            End If
        End Using
    End Sub
    Private Function hex_to_string(ByVal hex As String) As String
        hex = hex.Replace(" ", "").Replace("&H", "")

        Dim result As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            Dim hexByte As String = hex.Substring(i, 2)
            Dim byteValue As Byte
            If Byte.TryParse(hexByte, Globalization.NumberStyles.HexNumber, Nothing, byteValue) Then
                result.Append(ChrW(byteValue))
            End If
        Next

        Return result.ToString()
    End Function
    Private Function recive_topic_message(ByVal e)
        Dim message As String = Encoding.UTF8.GetString(e.ApplicationMessage.Payload)
        ' Dim qos As String = e.ApplicationMessage.QualityOfServiceLevel
        Dim topic As String = e.ApplicationMessage.Topic.ToString.Replace(maintopic, "")
        Select Case topic
            Case "CARD/UID/"
                Invoke(Sub() lettore_badge_form.CardUID.Text = CorrectionHEXValues(message.ToString))
            Case "CARD/LOGIN/"
                If message.ToString = "1" Then
                    Invoke(Sub() lettore_badge_form.LCD_line2.Text = "Ciao")
                Else
                    Invoke(Sub() lettore_badge_form.LCD_line2.Text = "Accesso Negato")
                    Invoke(Sub() lettore_badge_form.LCD_line1.Text = " ")
                End If
            Case "CARD/BLOCK/0"
                Invoke(Sub() lettore_badge_form.CardBlock0.Text = CorrectionHEXValues(message.ToString))
            Case "CARD/BLOCK/1"
                Invoke(Sub() lettore_badge_form.CardBlock1.Text = CorrectionHEXValues(message.ToString))
               ' Invoke(Sub() lettore_badge_form.LCD_line1.Text = hex_to_string(message.ToString))
            Case "CARD/BLOCK/2"
                Invoke(Sub() lettore_badge_form.CardBlock2.Text = CorrectionHEXValues(message.ToString))
                Invoke(Sub() lettore_badge_form.LCD_line1.Text = hex_to_string(message.ToString))
            Case "CARD/UPTIME_REF"
                Invoke(Sub() lettore_badge_form.uptimereference.Text = message.ToString)
            Case "INPUT/1/ST"
            Case "INPUT/TAMPER/ST"
                If message.ToString = "1" Then
                    Invoke(Sub() lettore_badge_form.stTamper.Image = My.Resources.led_green)
                Else
                    Invoke(Sub() lettore_badge_form.stTamper.Image = My.Resources.led_red)
                End If
           ' Case "OUTPUT/RELE/1/TRG"
            Case "OUTPUT/RELE/1/ST"
                If message.ToString = "0" Then
                    Invoke(Sub() lettore_badge_form.stRele.Image = My.Resources.led_off)
                Else
                    Invoke(Sub() lettore_badge_form.stRele.Image = My.Resources.led_green)
                End If
            Case "OUTPUT/LED/1/ST"
                If message.ToString = "0" Then
                    Invoke(Sub() lettore_badge_form.Led1.Image = My.Resources.led_off)
                Else
                    Invoke(Sub() lettore_badge_form.Led1.Image = My.Resources.led_red)
                End If
            Case "OUTPUT/LED/2/ST"
                If message.ToString = "0" Then
                    Invoke(Sub() lettore_badge_form.Led2.Image = My.Resources.led_off)
                Else
                    Invoke(Sub() lettore_badge_form.Led2.Image = My.Resources.led_green)
                End If
            Case "SYS/INFO/UPTIME"
                Invoke(Sub() lettore_badge_form.uptime.Text = message.ToString())
            Case "SYS/INFO/VERS_FW"
                Invoke(Sub() lettore_badge_form.versioneFW.Text = message.ToString())
            Case Else
                Invoke(Sub() StatusText.Text = "Attivatore - " & message.ToString)
                If message.ToString = "On-Line" Then
                    Invoke(Sub() StatusBar.Value = 100)
                    Invoke(Sub() lettore_badge_form.BackColor = Color.White)
                Else
                    Invoke(Sub() StatusBar.Value = 0)
                    Invoke(Sub() lettore_badge_form.BackColor = Color.Red)
                End If

        End Select
        Return True
    End Function

    Private Async Sub subscribe_topic()
        Dim topictosub As String = IO.File.ReadAllText(Path_Progetto & "/conf/topic.sub")
        Dim topictosubline() As String
        topictosubline = Split(topictosub, vbCrLf)


        For Each line As String In topictosubline
            If line.StartsWith("#") Then
                Continue For
            End If
            If maintopic = "" Then
                maintopic = line
                Await MQTT_CLIENT.Topic_Subscribe(maintopic) 'si inscrive al main topic per ricevere lo stato di connessione del lettore
            Else
                Await MQTT_CLIENT.Topic_Subscribe(maintopic & line)
            End If
        Next
    End Sub
#End Region
    Private Sub main_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ' Ottieni l'ID del processo corrente
        Dim currentProcess As Process = Process.GetCurrentProcess()
        Dim processId As Integer = currentProcess.Id

        ' Termina il processo corrente
        Try
            currentProcess.Kill()
        Catch ex As Exception
            ' Gestisci l'eccezione se si verifica un errore durante la terminazione del processo
            MessageBox.Show("Si è verificato un errore durante la terminazione del processo corrente: " & ex.Message)
            MYlogManager.LogMessage("Errore chiusura processo." & ex.Message, "main_Closing-main")
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        ' Ottieni l'ID del processo corrente
        Dim currentProcess As Process = Process.GetCurrentProcess()
        Dim processId As Integer = currentProcess.Id

        ' Termina il processo corrente
        Try
            currentProcess.Kill()
        Catch ex As Exception
            ' Gestisci l'eccezione se si verifica un errore durante la terminazione del processo
            MessageBox.Show("Si è verificato un errore durante la terminazione del processo corrente: " & ex.Message)
            MYlogManager.LogMessage("Errore chiusura processo." & ex.Message, "ExitToolStripMenuItem_Click-main")
        End Try
    End Sub
    Private Async Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        StatusBar.Value = 0
        If Not IO.Directory.Exists(Path_Progetto & "/conf") Then
            MessageBox.Show("Impossibile aprire il programma, manca la cartella /conf." & vbCrLf & "Contattare Alessio Savelli.")
            Me.Close()
        End If
        If Not IO.File.Exists(Path_Progetto & "/conf/broker.conf") Then
            MessageBox.Show("Impossibile aprire il programma, manca il file /conf/broker.conf." & vbCrLf & "Contattare Alessio Savelli.")
            Me.Close()
        End If
        If Not IO.File.Exists(Path_Progetto & "/conf/topic.sub") Then
            MessageBox.Show("Impossibile aprire il programma, manca il file /conf/broker.conf." & vbCrLf & "Contattare Alessio Savelli.")
            Me.Close()
        End If
        If Not IO.File.Exists(Path_Progetto & "/conf/scuole.txt") Then
#Region "Creazione guidate del file scuole"
            Dim scuole As New List(Of String)
            Do
                Dim inputDialog As New InputDialog("Inserimento Scuola", "Inserisci il nome di una scuola:", "Metti il nome della scuola qui")
                Dim nuovaScuola As String = inputDialog.GetInput()

                If String.IsNullOrWhiteSpace(nuovaScuola) Then
                    ' L'utente ha premuto Annulla o ha lasciato il campo vuoto, esci dal ciclo
                    Exit Do
                End If

                scuole.Add(nuovaScuola)

                Dim scelta As DialogResult = MessageBox.Show("Vuoi inserire un'altra scuola?", "Continua?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If scelta = DialogResult.No Then
                    Exit Do
                End If
            Loop While True

            ' Scrivi le scuole nel file
            Try
                Directory.CreateDirectory(Path.GetDirectoryName(Path_Progetto & "/conf/scuole.txt"))
                File.WriteAllLines(Path_Progetto & "/conf/scuole.txt", scuole)
                MessageBox.Show("Scuole salvate con successo nel file scuole.txt.", "Operazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Si è verificato un errore durante il salvataggio delle scuole, conttattare Alessio savelli." & vbCrLf & ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MYlogManager.LogMessage("Errore salvataggio scuole." & ex.Message, "main_Load-main")
            End Try



#End Region
        End If


        If Not Directory.Exists(Path_Progetto & "/conf/pic") Then
            Try
                Directory.CreateDirectory(Path_Progetto & "/conf/pic")
            Catch ex As Exception
                MessageBox.Show("Impossibile creare la cartella 'conf/pic'. Assicurati di avere i permessi necessari o crea la cartella manualmente.", "Errore Creazione Cartella", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MYlogManager.LogMessage("Errore creazione cartella 'conf/pic'. Assicurati di avere i permessi necessari o crea la cartella manualmente." & ex.Message, "main_Load-main")
            End Try
        End If

        Await DataBase.StartDbAsync() 'avvia il database

        Dim ip As String = ""
        Dim port, timeout As Int16
        Dim packetfragm As Boolean

#Region "Broker reading setup"
        Dim brokersetup As String = IO.File.ReadAllText(Path_Progetto & "/conf/broker.conf")
        Dim brokersetupline() As String
        brokersetupline = Split(brokersetup, vbCrLf)
        For Each line As String In brokersetupline
            If line.StartsWith("#") Then
                Continue For
            End If
            Dim parmsrow() As String = Split(line, "=")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(0).ToLower
                    Case "ip"
                        ip = parmsrow(1)
                    Case "port"
                        port = parmsrow(1)
                    Case "username"
                        MQTT_USER = parmsrow(1)
                    Case "timeout(s)"
                        timeout = parmsrow(1)
                    Case "packetfragmentation"
                        If parmsrow(1).ToLower = "yes" Then
                            packetfragm = True
                        Else
                            packetfragm = False
                        End If
                End Select

            End If
        Next
#End Region




        MQTT_CLIENT = New ClientMQTT(ip, port, packetfragm, timeout)
        AddHandler MQTT_CLIENT.Recive_mqttMessage, AddressOf recive_topic_message

        Connect_mqtt()

        lettore_badge_form.MdiParent = Me
        lettore_badge_form.Show()

        accessi_mensili_form.MdiParent = Me
        accessi_mensili_form.Show()

        aula_virtuale_form.MdiParent = Me
        aula_virtuale_form.Show()


        AddHandler lettore_badge_form.add_access, AddressOf accessi_mensili_form.add_access
        AddHandler aula_virtuale_form.manual_stamping, AddressOf accessi_mensili_form.add_access 'gestisce l'inserimento dei log manuali effettuati dalla gestione aula


        AddHandler lettore_badge_form.add_access, AddressOf aula_virtuale_form.badge_reading




    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Connect_mqtt()
    End Sub

#Region "menu Strumenti"

    Dim InfoBox As AboutBox1
    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Dim newbadge As New nuovo
        newbadge.Show()
    End Sub
    Dim managment_alunni As ListaUtenti

    Private Sub ListaAccessiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListaAccessiToolStripMenuItem.Click
        ' Verifica se la finestra è già aperta
        If managment_alunni Is Nothing OrElse managment_alunni.IsDisposed Then
            ' Se non è aperta, creala
            managment_alunni = New ListaUtenti()
            managment_alunni.MdiParent = Me
            managment_alunni.Show()
        Else
            ' Se è già aperta, riportala in primo piano
            managment_alunni.BringToFront()
        End If
    End Sub

    Private Sub InfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InfoToolStripMenuItem.Click
        If InfoBox Is Nothing OrElse InfoBox.IsDisposed Then
            InfoBox = New AboutBox1()
            InfoBox.Show()
        Else
            InfoBox.Activate()
        End If
    End Sub

#End Region
End Class
