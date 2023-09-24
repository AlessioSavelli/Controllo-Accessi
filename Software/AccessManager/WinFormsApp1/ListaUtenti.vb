Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Public Class ListaUtenti
    Dim Thread_gestione_change_id As Thread
    Private Sub ListaUtenti_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CaricaScuoleInComboBox(BoxScuola)
        GroupBox2.Enabled = False
        StartData.Value = DateTime.Now.AddMonths(-1)

        Dim loading_th As New Thread(AddressOf LoadUserList)
        loading_th.SetApartmentState(ApartmentState.STA)
        loading_th.IsBackground = False
        loading_th.Start()
    End Sub
    Private Sub AggiornaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AggiornaToolStripMenuItem.Click

        'avvia il thread di aggiornamento
        Dim loading_th As New Thread(AddressOf LoadUserList)
        loading_th.SetApartmentState(ApartmentState.STA)
        loading_th.IsBackground = False
        loading_th.Start()
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
    Private Sub ViewAllLogs_CheckedChanged(sender As Object, e As EventArgs) Handles ViewAllLogs.CheckedChanged
        If ViewAllLogs.Checked Then
            EndData.Enabled = False
            StartData.Enabled = False
        Else
            EndData.Enabled = True
            StartData.Enabled = True
        End If
    End Sub
    Private Sub ModifyUser_CheckedChanged_1(sender As Object, e As EventArgs) Handles ModifyUser.CheckedChanged
        GroupBox1.Enabled = ModifyUser.Checked
    End Sub
    Private Sub ChangePic_Click(sender As Object, e As EventArgs) Handles ChangePic.Click
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "File immagine|*.png;*.jpg;*.jpeg"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog.FileName
            PictureBox1.ImageLocation = selectedFilePath
        End If
    End Sub

    Private Async Sub ModifyButton_Click(sender As Object, e As EventArgs) Handles ModifyButton.Click
        Dim userData As UserData = DirectCast(PictureBox1.Tag, UserData)
#Region "Modifica immagine profilo"
        ' Verifica se la proprietà ImageLocation di PictureBox1 contiene un percorso valido
        If Not String.IsNullOrEmpty(PictureBox1.ImageLocation) AndAlso File.Exists(PictureBox1.ImageLocation) Then
            ' Ottieni il percorso dell'immagine da ImageLocation
            Dim imagePath As String = PictureBox1.ImageLocation

            ' Ottieni il nome dell'immagine dal tag UserData.picPath di PictureBox1


            ' Crea una cartella se non esiste
            Dim targetFolderPath As String = Path.Combine(Application.StartupPath, "conf/pic")
            If Not Directory.Exists(targetFolderPath) Then
                Directory.CreateDirectory(targetFolderPath)
            End If

            ' Carica l'immagine
            Dim originalImage As New Bitmap(imagePath)

            ' Ridimensiona l'immagine a 128x128 pixel
            Dim resizedImage As New Bitmap(128, 128)
            Using graphics As Graphics = Graphics.FromImage(resizedImage)
                graphics.DrawImage(originalImage, 0, 0, 128, 128)
            End Using

            ' Salva l'immagine nella cartella con il nome specificato
            Dim savedImagePath As String = Path.Combine(targetFolderPath, userData.PicPath + ".png")
            resizedImage.Save(savedImagePath, ImageFormat.Png)

            ' Rilascia le risorse delle immagini
            originalImage.Dispose()
            resizedImage.Dispose()
        End If
#End Region
#Region "Modifica Genere"
        If RadioButton1.Checked Then
            userData.Gender = "M"
        ElseIf RadioButton2.Checked Then
            userData.Gender = "F"
        ElseIf RadioButton3.Checked Then
            userData.Gender = "ND"
        End If
#End Region
#Region "Modifica Altri dati utente"
        userData.Name = BoxNome.Text
        userData.Birthday = ConvertiTestoInData(BoxEta.Text)
        userData.School = BoxScuola.Text
#End Region
        Await DataBase.UpdateStudentDataAsync(userData)
        ModifyUser.Checked = False
        Dim selectedItem As ListViewItem = DirectCast(BoxNome.Tag, ListViewItem)
        selectedItem.SubItems(0).Text = userData.Name
        selectedItem.SubItems(1).Text = userData.Gender
        selectedItem.SubItems(3).Text = BoxEta.Text
        selectedItem.SubItems(5).Text = userData.School
        selectedItem.Tag = userData
        LogsPresenze.Tag = userData
        If Me.Tag IsNot Nothing Then
            Dim formBadge As ScritturaBadge = DirectCast(Me.Tag, ScritturaBadge)
            formBadge.closing()
            Me.Tag = Nothing
        End If
    End Sub

    Private Sub BadgeChanger_Click(sender As Object, e As EventArgs) Handles BadgeChanger.Click
        If BadgeChanger.Text = "Cambia/Riscrivi Badge" Then
            BadgeChanger.Text = "Termina Operazione su Badge"
            Dim userData As UserData = DirectCast(PictureBox1.Tag, UserData)
            Dim selectedItem As ListViewItem = DirectCast(BoxNome.Tag, ListViewItem)

            ReWriteBadge.Visible = True

            Dim new_badge As New ScritturaBadge(ScritturaBadge.OverwriteOption.ASK_BEFOR_OVERWRITE)

            ' Imposta le proprietà del controllo come desiderato, ad esempio dimensioni e posizione.
            new_badge.Size = ReWriteBadge.Size
            new_badge.Location = New Point(0, 0)
            new_badge.BackColor = Color.Transparent ' Imposta il colore di sfondo desiderato.
            new_badge._userData = userData
            ' Aggiungi il controllo alla finestra principale.
            ReWriteBadge.Controls.Add(new_badge)

            ' Porta il controllo in primo piano per sovrapporsi agli altri.
            new_badge.BringToFront()

            ' Rendi il controllo visibile.
            new_badge.Visible = True

            ' Imposta il focus sul controllo, se necessario.
            new_badge.Focus()
            Me.Tag = new_badge

            'Avvio il thread che attende di leggere il badge
            If Thread_gestione_change_id IsNot Nothing AndAlso Thread_gestione_change_id.IsAlive Then
                Thread_gestione_change_id.Abort()
            End If
            Thread_gestione_change_id = New Thread(AddressOf WaitingForChangeBadgeID)
            Thread_gestione_change_id.IsBackground = False
            Thread_gestione_change_id.SetApartmentState(ApartmentState.STA)
            Thread_gestione_change_id.Start(New Object() {userData, selectedItem})

        Else
            BadgeChanger.Text = "Cambia/Riscrivi Badge"
            If Me.Tag IsNot Nothing Then
                Dim formBadge As ScritturaBadge = DirectCast(Me.Tag, ScritturaBadge)
                formBadge.closing()
                Me.Tag = Nothing
            End If
            ReWriteBadge.Visible = False
            Label6.BackColor = Label5.BackColor
        End If

    End Sub

    Private Sub ListaUtenti_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Me.Tag IsNot Nothing Then
            Try
                Dim formBadge As ScritturaBadge = DirectCast(Me.Tag, ScritturaBadge)
                formBadge.closing()
                Me.Tag = Nothing
            Catch
            End Try
        End If
    End Sub
    Private Async Sub WaitingForChangeBadgeID(parms() As Object)
        Dim old_UserData As UserData = DirectCast(parms(0), UserData)
        Dim listViewItem As ListViewItem = DirectCast(parms(1), ListViewItem)
        Dim badgeWriter As ScritturaBadge = DirectCast(Me.Tag, ScritturaBadge) 'identifica l'item dove aggiornare i dati nella lista utente
        'Attende che il valore del badge venga sostituito dal form di modifica badge
        While Not (badgeWriter.WritingStatus = 5) 'magic number
            Thread.Sleep(350)
            'se me.tag e' nullo vuol dire che hanno smesso di scrivere il badge.
            Invoke(Sub()
                       If Me.Tag Is Nothing Then
                           Exit Sub
                       End If
                   End Sub)
        End While
        'se arriva a questo punto vuol dire che il badge e' stato scritto con successo
        Invoke(Sub()
                   Label6.Text = badgeWriter._userData.Badge
                   Label6.BackColor = Color.Green
                   listViewItem.SubItems(4).Text = badgeWriter._userData.Badge
               End Sub) ' aggiorna l'uid del badge


        'visto che non cambia alcun dato oltre all'id del badge aggiorno il database
        Await DataBase.UpdateStudentDataAsync(badgeWriter._userData)

        Dim th As New Thread(AddressOf AutomaticTerminateBadgeWritingOperation)
        th.IsBackground = True
        th.SetApartmentState(ApartmentState.STA)
        th.Start()
    End Sub
    Private Sub AutomaticTerminateBadgeWritingOperation()
        Thread.Sleep(10000)
        'Se dopo 5secondi non e' stato cliccato il pulsante di end modifica lo clicca in automatico il thread
        Try
            Invoke(Sub()
                       BadgeChanger.PerformClick()
                   End Sub)
        Catch
        End Try
    End Sub
#Region "Caricamento degli alunni nella listview"
    Private Async Sub LoadUserList()
        Thread.Sleep(350)
        Dim schoolGroups As New Dictionary(Of String, ListViewGroup)()
        ' Caricare gli alunni utilizzando la funzione EstraiUtentiDaDatabase
        Dim userDataList As List(Of UserData) = Await DataBase.EstraiUtentiDaDatabase()
        If userDataList Is Nothing Then Exit Sub
        If userDataList.Count = 0 Then Exit Sub
        ' Creazione di una lista per gli elementi ListViewItem da aggiungere
        Dim listViewItems As New List(Of ListViewItem)()
        ' Ciclo sugli UserData e crea gli elementi ListViewItem
        For Each userData As UserData In userDataList
            ' Verifica se il gruppo per la scuola esiste già nel dizionario
            If Not schoolGroups.ContainsKey(userData.School) Then
                ' Se il gruppo non esiste, crealo e aggiungilo alla ListView
                Dim group As New ListViewGroup(userData.School)

                UserList.Invoke(Sub() UserList.Groups.Add(group))
                schoolGroups(userData.School) = group
            End If
            ' Crea un elemento ListViewItem per l'utente e assegnalo al gruppo corrispondente
            Dim item As New ListViewItem(userData.Name)
            item.SubItems.AddRange(New String() {userData.Gender, userData.Birthday, userData.RegDate, userData.Badge, userData.School})
            item.Tag = userData
            item.Group = schoolGroups(userData.School) ' Assegna il gruppo corrispondente all'elemento

            listViewItems.Add(item)
        Next

        ' Aggiungi tutti gli elementi ListViewItem alla ListView principale in modo sincronizzato
        SyncLock UserList

            UserList.Invoke(Sub()
                                UserList.Items.Clear()
                                UserList.Items.AddRange(listViewItems.ToArray())
                            End Sub)
        End SyncLock

    End Sub

    'gestice la selezione dell'utente facendo double click
    Private Sub UserList_DoubleClick(sender As Object, e As EventArgs) Handles UserList.DoubleClick
        GroupBox2.Enabled = True
        ModifyUser.Enabled = True
        ' Verifica se è stato selezionato un elemento nella UserList
        If UserList.SelectedItems.Count > 0 Then
            ' Ottieni l'elemento ListView selezionato
            Dim selectedItem As ListViewItem = UserList.SelectedItems(0)

            ' Ottieni il UserData dall'elemento ListView
            Dim userData As UserData = DirectCast(selectedItem.Tag, UserData)

            ' Popola i controlli con i dati dell'utente
            BoxNome.Text = userData.Name
            BoxScuola.Text = userData.School
            BoxEta.Text = userData.Birthday
            Label6.Text = userData.Badge

            ' Imposta il genere sui RadioButtons
            Select Case userData.Gender
                Case "M"
                    RadioButton1.Checked = True
                Case "F"
                    RadioButton2.Checked = True
                Case "ND"
                    RadioButton3.Checked = True
            End Select

            ' Carica l'immagine dell'utente in PictureBox1
            If File.Exists(Path.Combine(Application.StartupPath, "conf/pic", userData.PicPath)) Then
                PictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "conf/pic", userData.PicPath))
            End If

            PictureBox1.Tag = userData

            ' Imposta il tag di LogsPresenze con UserData
            BoxNome.Tag = selectedItem

            LogsPresenze.Tag = userData

            NrLogsText.Text = "0"
            NrPresenzeText.Text = "0"
        End If
    End Sub

#End Region

#Region "Caricamento dati di statistica"
    Private Sub LoadLogs_Click(sender As Object, e As EventArgs) Handles LoadLogs.Click

        Dim loadingLogs_th As New Thread(AddressOf SearchDataInDB)
        loadingLogs_th.SetApartmentState(ApartmentState.STA)
        loadingLogs_th.IsBackground = False
        loadingLogs_th.Start()
    End Sub
    Private Sub SearchDataInDB()
        ' Ottieni l'oggetto UserData dal tag di LogsPresenze
        Dim userData As UserData = DirectCast(LogsPresenze.Tag, UserData)

        ' Popola la TreeView temporanea con i log
        Dim tempTreeView As New TreeView()
        PopulateTreeViewWithLogs(userData, tempTreeView)

        ' Utilizza Invoke per aggiornare la TreeView principale dall'interfaccia utente
        SyncLock GroupBox2
            Invoke(Sub()
                       GroupBox2.Enabled = False
                       ' Pulisci la TreeView principale
                       LogsPresenze.Nodes.Clear()

                       ' Aggiungi tutti i nodi dalla TreeView temporanea alla TreeView principale
                       For Each node As TreeNode In tempTreeView.Nodes
                           LogsPresenze.Nodes.Add(DirectCast(node.Clone(), TreeNode))
                       Next

                       GroupBox2.Enabled = True
                   End Sub)
            'Adesso avvia la statica degli ingressi in aula
            Dim th_ingressi_aula As New Thread(AddressOf StatisticaIngressiInAula)
            th_ingressi_aula.SetApartmentState(ApartmentState.STA)
            th_ingressi_aula.IsBackground = False
            th_ingressi_aula.Start(tempTreeView)
        End SyncLock
    End Sub
    Private Async Sub PopulateTreeViewWithLogs(ByVal userData As UserData, ByVal tempTreeView As TreeView)
        ' Pulisci la TreeView temporanea
        tempTreeView.Nodes.Clear()

        ' Dizionario per tenere traccia delle iterazioni per data
        Dim iterationsByDate As New Dictionary(Of Date, Integer)

        Dim startDate As DateTime = StartData.Value.Date
        Dim endDate As DateTime = EndData.Value
        Dim logs As List(Of LogEntry)

        If ViewAllLogs.Checked Then
            ' Se ViewAllLogs è selezionato, estrai tutti i log dell'utente
            startDate = New DateTime(1900, 1, 1) 'seleziona tutte le presenze dell'utente
        End If
        logs = Await DataBase.ExtractLogsFromDateAsync(startDate, endDate, userData.Id)
        Invoke(Sub() NrLogsText.Text = logs.Count)
        ' Loop attraverso i log dell'utente e popola il dizionario e la TreeView temporanea

        For Each logEntry As LogEntry In logs
            Dim logDate As Date = logEntry.data.Date
            Dim dayOfWeek As String = logDate.ToString("dddd") ' Ottieni il giorno della settimana come stringa
            Dim logNode As TreeNode = FindOrCreateNodeByDate(logDate, tempTreeView.Nodes)

            ' Aggiorna il conteggio delle iterazioni per questa data
            If iterationsByDate.ContainsKey(logDate) Then
                iterationsByDate(logDate) += 1
            Else
                iterationsByDate(logDate) = 1
            End If

            ' Aggiungi il log come nodo figlio con il giorno della settimana
            Dim logNodeText As String = $"{logEntry.data:HH:mm} - {logEntry.scuola}"
            Dim newNode As New TreeNode(logNodeText)
            newNode.Tag = logEntry
            logNode.Nodes.Add(newNode)
        Next


    End Sub

    Private Function FindOrCreateNodeByDate(ByVal dateToFind As Date, ByVal nodes As TreeNodeCollection) As TreeNode
        Dim yearNode As TreeNode = Nothing
        Dim monthNode As TreeNode = Nothing
        Dim dayNode As TreeNode = Nothing

        ' Trova o crea nodi per anno, mese e giorno
        For Each node As TreeNode In nodes
            If node.Text = dateToFind.Year.ToString() Then
                yearNode = node
                Exit For
            End If
        Next

        If yearNode Is Nothing Then
            yearNode = New TreeNode(dateToFind.Year.ToString())
            nodes.Add(yearNode)
        End If

        For Each node As TreeNode In yearNode.Nodes
            If node.Text = dateToFind.ToString("MMMM") Then
                monthNode = node
                Exit For
            End If
        Next

        If monthNode Is Nothing Then
            monthNode = New TreeNode(dateToFind.ToString("MMMM"))
            yearNode.Nodes.Add(monthNode)
        End If

        Dim dayText As String = $"{dateToFind.ToString("dd/MM")} ({dateToFind.ToString("dddd")})"
        For Each node As TreeNode In monthNode.Nodes
            If node.Text = dayText Then
                dayNode = node
                Exit For
            End If
        Next

        If dayNode Is Nothing Then
            dayNode = New TreeNode(dayText)
            monthNode.Nodes.Add(dayNode)
        End If

        Return dayNode
    End Function

#Region "Calcolo statistico delle reali presenze in aula"
    Private Sub StatisticaIngressiInAula(ByVal tempTreeView As Object)

        SyncLock NrPresenzeText
            Dim Stima As Integer = CalculateClassroomEntries(tempTreeView)
            Dim colorallaert As Color
            If Stima < +6 Then
                colorallaert = Color.Red
            ElseIf Stima <= 12 Then
                colorallaert = Color.DarkOrange
            Else
                colorallaert = Color.DarkGreen
            End If
            Invoke(Sub()
                       NrPresenzeText.BackColor = colorallaert
                       NrPresenzeText.Text = Stima

                   End Sub)

        End SyncLock

    End Sub
    Private Function CalculateClassroomEntries(treeView As TreeView) As Integer
        Dim classroomUsages As New List(Of UserClassroomUsage)

        For Each yearNode As TreeNode In treeView.Nodes
            For Each monthNode As TreeNode In yearNode.Nodes
                For Each dayNode As TreeNode In monthNode.Nodes

                    Dim currentDate As DateTime = DateTime.ParseExact(dayNode.Text.Split(" (")(0) & "/" & yearNode.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)

                    For Each timeNode As TreeNode In dayNode.Nodes
                        If TypeOf timeNode.Tag Is LogEntry Then
                            Dim logEntry As LogEntry = TryCast(timeNode.Tag, LogEntry)

                            ' Assumiamo che il badge di ingresso in aula contenga "IN" e "OUT" per distinguere l'ingresso e l'uscita
                            If logEntry.data.Hour >= 8 AndAlso logEntry.data.Hour < 17 AndAlso logEntry.data.Date = currentDate Then
                                Dim entryFound As Boolean = False

                                For Each usage As UserClassroomUsage In classroomUsages
                                    ' Cerca un'istanza utente già registrata
                                    If usage.UserId = logEntry.user_Id Then
                                        ' Controlla se l'ingresso avviene entro 30 minuti dall'uscita precedente
                                        Dim lastExit As DateTime = usage.ClassroomEntries(usage.ClassroomEntries.Count - 1).ExitTime
                                        If (logEntry.data - lastExit).TotalMinutes <= 30 Then
                                            ' Aggiungi il timestamp di ingresso all'ultima registrazione
                                            usage.ClassroomEntries(usage.ClassroomEntries.Count - 1).EntryTime = logEntry.data
                                            entryFound = True
                                        End If
                                    End If
                                Next

                                If Not entryFound Then
                                    ' Crea una nuova istanza utente con un nuovo ingresso
                                    Dim newUserUsage As New UserClassroomUsage With {
                                        .UserId = logEntry.user_Id
                                    }
                                    Dim newEntry As New ClassroomEntry With {
                                        .EntryTime = logEntry.data,
                                        .ExitTime = logEntry.data.AddMinutes(30)
                                    }
                                    newUserUsage.ClassroomEntries.Add(newEntry)
                                    classroomUsages.Add(newUserUsage)
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        Next

        ' Calcola il numero totale di ingressi in aula per l'utente nella treeview
        Dim totalEntries As Integer = 0
        For Each usage As UserClassroomUsage In classroomUsages
            totalEntries += usage.ClassroomEntries.Count
        Next

        Return totalEntries
    End Function
#End Region

#End Region

#Region "Eliminazione utenti"
    Private Async Sub EliminaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EliminaToolStripMenuItem.Click
        If UserList.SelectedItems.Count > 0 Then
            ' Recupera l'elemento selezionato
            Dim selectedItem As ListViewItem = UserList.SelectedItems(0)

            ' Ottieni il nome e la scuola dall'elemento selezionato
            Dim name As String = selectedItem.SubItems(0).Text
            Dim school As String = selectedItem.SubItems(5).Text

            ' Chiedi conferma per l'eliminazione
            Dim result As DialogResult = MessageBox.Show("Vuoi eliminare l'utente selezionato?" & vbCrLf & "Nome: " & name & vbCrLf & "Scuola: " & school, "Conferma eliminazione", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ' Elimina l'elemento selezionato
                Dim userData As UserData = DirectCast(selectedItem.Tag, UserData)
                Await DataBase.DeleteStudentDataAsync(userData)
                UserList.Items.Remove(selectedItem)
            End If
        End If
    End Sub
#End Region
#Region "Gestione Stampa Badge"
    Private Sub StampaBadgeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StampaBadgeToolStripMenuItem.Click
        If UserList.SelectedItems.Count > 0 Then
            Dim badgePrinter As StampaBadge
            If Me.Tag IsNot Nothing Then
                badgePrinter = DirectCast(Me.Tag, StampaBadge)
                Me.Controls.Remove(badgePrinter)
                badgePrinter.Dispose()
            End If
            ' Recupera l'elemento selezionato
            Dim selectedItem As ListViewItem = UserList.SelectedItems(0)
            badgePrinter = New StampaBadge
            badgePrinter.userData = DirectCast(selectedItem.Tag, UserData)

            ' Aggiungi badgePrinter al contenitore desiderato nel form padre.
            Me.Controls.Add(badgePrinter)

            ' Imposta la posizione del badgePrinter al centro del contenitore del form padre.
            badgePrinter.Location = New Point((Me.ClientSize.Width - badgePrinter.Width) / 2, (Me.ClientSize.Height - badgePrinter.Height) / 2)

            ' Porta il badgePrinter davanti a tutti gli altri controlli.
            badgePrinter.BringToFront()

            ' Mostra badgePrinter.
            badgePrinter.Visible = True

            AddHandler badgePrinter.CloseRequested, AddressOf badgePrinter_CloseRequested

            Me.Tag = badgePrinter
        End If
    End Sub
    ' Dentro il tuo Form
    Private Sub badgePrinter_CloseRequested(sender As Object, e As EventArgs)
        Dim badgePrinter As StampaBadge
        If Me.Tag IsNot Nothing Then
            badgePrinter = DirectCast(Me.Tag, StampaBadge)
            Me.Controls.Remove(badgePrinter)
            badgePrinter.Dispose()
        End If
    End Sub


#End Region
End Class