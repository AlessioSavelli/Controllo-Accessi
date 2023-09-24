Imports System.IO
Imports System.Threading
Imports System.Drawing
Imports System.Text.Json
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
Imports System.Diagnostics.Eventing.Reader
Imports System.Reflection

Public Class AlunniPresenti

    Public Event manual_stamping(ByVal name As String, ByVal uid As String, ByVal time As String)
    Private Sub ConteggioAlunni_Tick(sender As Object, e As EventArgs) Handles ConteggioAlunni.Tick
        Me.Text = Me.Text.Split(":")(0) & ": " & AlunniPresenti1.Items.Count
    End Sub
    Private Sub AlunniPresenti_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CaricaScuoleInComboBox(ScuolaUtente)
    End Sub

    Public Sub badge_reading(ByVal name As String, ByVal uid As String, ByVal time As String)
        If String.IsNullOrEmpty(uid) Or String.IsNullOrEmpty(time) Then
            Exit Sub
        End If

        Dim new_alunno_th As New Thread(AddressOf managment_alunno)
        new_alunno_th.IsBackground = False
        new_alunno_th.SetApartmentState(Threading.ApartmentState.STA)
        Dim parms As String() = New String() {name, uid, time}
        new_alunno_th.Start(parms)
    End Sub
#Region "Gestione alunno"
    Private Async Sub managment_alunno(ByVal obj As Object)
        Dim name As String = CStr(obj(0))
        Dim uid_badge As String = CStr(obj(1))
        Dim time As String = CStr(obj(2))
        Dim userDataList As List(Of UserData)

        If (uid_badge = ident_guest_user) Then
            userDataList = New List(Of UserData)()
            userDataList.Add(CreateGuestUserData(uid_badge, name))
        Else
            userDataList = Await DataBase.ExtractUserByBadgeAsync(uid_badge.Replace(ident_manual_stamping, ""))
        End If

        Dim userId As Integer = userDataList(0).Id ' Usiamo l'ID come chiave

        Dim userAlreadyExistst As Boolean = False

        SyncLock AlunniPresenti1
            ' Verifica se l'utente esiste già o se è un utente ospite
            If Not (userDataList(0).Id = ident_guest_user_id) Then
                AlunniPresenti1.Invoke(Sub() userAlreadyExistst = AlunniPresenti1.Items.Cast(Of ListViewItem).Any(Function(item) DirectCast(item.Tag, UserData).Id = userId))
            End If
            If userAlreadyExistst And Not (userDataList(0).Id = ident_guest_user_id) Then
                ' Rimuovi l'utente dall'aula utilizzando l'ID come chiave
                Invoke(Sub()
                           Dim itemToRemove As ListViewItem = AlunniPresenti1.Items.Cast(Of ListViewItem).
       FirstOrDefault(Function(item) DirectCast(item.Tag, UserData).Id = userId)
                           If itemToRemove IsNot Nothing Then
                               If itemToRemove.BackColor = Color.Lime Then
                                   Exit Sub
                               End If

                               ' Rimuovi l'immagine associata all'utente
                               Dim userImageKey As String = DirectCast(itemToRemove.ImageKey, String)
                               If UserPicture.Images.ContainsKey(userImageKey) Then
                                   Dim userImage As Image = UserPicture.Images(userImageKey)
                                   userImage.Dispose() ' Libera la memoria
                                   UserPicture.Images.RemoveByKey(userImageKey)
                               End If

                               AlunniPresenti1.Items.Remove(itemToRemove)
                           End If
                       End Sub)
            Else

#Region "Gestione inserimento immaggine dell'utente"

                ' Costruisci il percorso completo dell'immagine utente

                Dim imagePath As String = Path.Combine(Application.StartupPath, "conf/pic", "defimage.png")
                If Not String.IsNullOrEmpty(userDataList(0).PicPath) Then
                    If userDataList(0).PicPath.Replace(" ", "") IsNot Nothing Then
                        imagePath = Path.Combine(Application.StartupPath, "conf/pic", userDataList(0).PicPath)
                    End If
                End If
                Dim item As ListViewItem
                ' Verifica se l'immagine esiste nel percorso specificato
                If File.Exists(imagePath) Then
                    ' Carica l'immagine e ridimensionala a 128x128 pixel
                    Dim userImage As New Bitmap(imagePath)
                    If userImage.Width <> 128 Or userImage.Height <> 128 Then
                        userImage = New Bitmap(userImage, New Size(128, 128))
                    End If

                    ' Imposta l'immagine utente nell'ImageList con una chiave univoca
                    Dim imageKey As String = Guid.NewGuid().ToString()
                    Invoke(Sub() UserPicture.Images.Add(imageKey, userImage))

                    ' Crea un elemento ListViewItem con l'immagine utente
                    item = New ListViewItem(userDataList(0).Name, imageKey)
                Else 'DA SISTEMARE
                    ' L'immagine utente non esiste o non è stata specificata, quindi usa l'immagine di default (indice 0)
                    item = New ListViewItem(userDataList(0).Name, 0)
                End If

                ' Imposta il background e il tag
                item.BackColor = Color.Lime
                item.Tag = userDataList(0)

                ' Aggiungi l'elemento ListViewItem all'aula
                Invoke(Sub()
                           AlunniPresenti1.Items.Add(item)
                       End Sub)

#End Region

                Dim start_timer As New Thread(AddressOf timerRimozione)
                start_timer.SetApartmentState(ApartmentState.STA)
                start_timer.IsBackground = False
                start_timer.Start(item)
            End If
        End SyncLock
    End Sub
    Private Sub timerRimozione(ByVal items As ListViewItem)
        Thread.Sleep(tempo_minimo_di_uscita * 60 * 1000)
        Dim itemToreset As ListViewItem = DirectCast(items, ListViewItem)
        Invoke(Sub() itemToreset.BackColor = Color.White)
    End Sub

#End Region
#Region "Gestione timbrature manuali"
    Private Sub ChiudiAulaTimbraUscitaATuttiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Click
        If ConfermaRimozioneUtenti() = False Then Exit Sub
        Dim start_removing As New Thread(AddressOf RemoveAllUsersAndGenerateLogs)
        start_removing.SetApartmentState(ApartmentState.STA)
        start_removing.IsBackground = False
        start_removing.Start()
        MenuModifica.Enabled = False
    End Sub

    Private Sub UscitaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UscitaToolStripMenuItem.Click
        If ConfermaRimozioneUtenti() = False Then Exit Sub
        Dim start_removing As New Thread(AddressOf RemoveSelectedUsersAndGenerateLogs)
        start_removing.SetApartmentState(ApartmentState.STA)
        start_removing.IsBackground = False
        start_removing.Start()
        MenuModifica.Enabled = False
    End Sub
    Public Function ConfermaRimozioneUtenti() As Boolean
        ' Visualizza un MsgBox di conferma con TopMost impostato su True
        Dim result As DialogResult
        If InvokeRequired Then
            Invoke(Sub() result = MessageBox.Show("Sei sicuro di voler rimuovere gli utenti selezionati?", "Conferma Rimozione", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly))
        Else
            result = MessageBox.Show("Sei sicuro di voler rimuovere gli utenti selezionati?", "Conferma Rimozione", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly)
        End If
        ' Restituisci True se l'utente ha confermato, altrimenti False
        Return (result = DialogResult.Yes)
    End Function
    Private Sub RemoveSelectedUsersAndGenerateLogs()
        Dim logs As New List(Of LogEntry)

        ' Ottieni gli utenti selezionati
        Dim selectedItems As New List(Of ListViewItem)
        Invoke(
        Sub()
            For Each item As ListViewItem In AlunniPresenti1.SelectedItems
                selectedItems.Add(item)
            Next
        End Sub)

        ' Genera un log di uscita manuale solo per gli utenti selezionati
        For Each item As ListViewItem In selectedItems
            Dim userData As UserData = CType(item.Tag, UserData)
            Dim logEntry As New LogEntry()
            logEntry.name = userData.Name
            logEntry.scuola = userData.School
            logEntry.data = DateTime.Now ' Puoi impostare la data di uscita manualmente qui
            logEntry.UID_badge = userData.Badge
            logEntry.user_Id = userData.Id
            logs.Add(logEntry)
        Next

        ' Esegui l'evento manual_stamping per ogni log generato
        For Each logEntry As LogEntry In logs
            RaiseEvent manual_stamping(logEntry.name, logEntry.UID_badge & ident_manual_stamping, logEntry.data.ToString(DataTimeFormat))
            Thread.Sleep(1000)
        Next

        ' Rimuovi solo gli utenti selezionati dalla ListView
        Invoke(
        Sub()
            For Each item As ListViewItem In selectedItems
                AlunniPresenti1.Items.Remove(item)
            Next
            MenuModifica.Enabled = True
        End Sub
    )
    End Sub
    Private Sub RemoveAllUsersAndGenerateLogs()
        Dim logs As New List(Of LogEntry)
        ' Deseleziona tutti gli elementi selezionati nella ListView
        Invoke(Sub() AlunniPresenti1.SelectedItems.Clear())
        ' Copia l'elenco degli utenti presenti in modo sincronizzato
        Dim itemsCopy As List(Of ListViewItem)
        itemsCopy = CopyListViewItems(AlunniPresenti1)

        ' Rimuovi tutti gli utenti dalla lista


        ' Genera un log di uscita manuale per ciascun utente rimosso
        For Each item As ListViewItem In itemsCopy
            Dim userData As UserData = CType(item.Tag, UserData)
            Dim logEntry As New LogEntry()
            logEntry.name = userData.Name
            logEntry.scuola = userData.School
            logEntry.data = DateTime.Now ' Puoi impostare la data di uscita manualmente qui
            logEntry.UID_badge = userData.Badge
            logEntry.user_Id = userData.Id
            logs.Add(logEntry)
        Next

        ' Esegui l'evento manual_stamping per ogni log generato
        For Each logEntry As LogEntry In logs
            RaiseEvent manual_stamping(logEntry.name, logEntry.UID_badge & ident_manual_stamping, logEntry.data.ToString(DataTimeFormat))
        Next
        Invoke(Sub()
                   AlunniPresenti1.Items.Clear()
                   MenuModifica.Enabled = True
               End Sub)

    End Sub

    Private Function CopyListViewItems(ByVal listView As ListView) As List(Of ListViewItem)
        Dim itemsCopy As New List(Of ListViewItem)
        ' Copia gli elementi dalla ListView in modo sincronizzato
        SyncLock listView
            Invoke(Sub()
                       For Each item As ListViewItem In listView.Items
                           itemsCopy.Add(CType(item.Clone(), ListViewItem))
                       Next
                   End Sub)
        End SyncLock

        Return itemsCopy
    End Function

    'chiusura tab per la timbratura manuale
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TimbraturaManuale.Visible = False
    End Sub
    Private Sub IngressoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IngressoToolStripMenuItem.Click

        ListaAlunniScuole.Nodes.Clear()
        TimbraturaManuale.Visible = True
        Dim getAllUser As New Thread(AddressOf populateTreeViewWithUsers)
        getAllUser.SetApartmentState(ApartmentState.STA)
        getAllUser.IsBackground = False
        getAllUser.Start()

    End Sub

#Region "Gestione box inserimenti manuali"

    Private Async Sub populateTreeViewWithUsers()
        Dim utenti As List(Of UserData) = Await DataBase.EstraiUtentiDaDatabase()
        If utenti Is Nothing Then Exit Sub
        If utenti.Count = 0 Then Exit Sub
        Dim backColorTreeView As Color
        Invoke(Sub() backColorTreeView = ListaAlunniScuole.BackColor)

        Dim treeview_output As New TreeView()

        ' Creazione di un nodo radice per la TreeView
        Dim rootNode As New TreeNode("Elenco Utenti per Scuola")
        treeview_output.Nodes.Add(rootNode)

        ' Creazione di un dizionario per organizzare gli utenti per scuola
        Dim utentiPerScuola As New Dictionary(Of String, List(Of UserData))()

        ' Organizzazione degli utenti nel dizionario per scuola
        For Each utente As UserData In utenti
            If Not utentiPerScuola.ContainsKey(utente.School) Then
                utentiPerScuola(utente.School) = New List(Of UserData)()
            End If
            utentiPerScuola(utente.School).Add(utente)
        Next

        ' Carica i colori delle scuole dal file
        Dim pathToFile As String = Path_Progetto & "/conf/school.color"
        Dim coloriScuola As Dictionary(Of String, Color) = CaricaColoriScuola(pathToFile)

        ' Aggiunta di nodi per ogni scuola e utente con colorazione
        For Each scuola As String In utentiPerScuola.Keys
            Dim scuolaNode As New TreeNode(scuola)
            rootNode.Nodes.Add(scuolaNode)

            ' Ottieni il colore per la scuola
            Dim coloreScuola As Color = OttieniColoreScuola(scuola, coloriScuola, backColorTreeView)

            For Each utente As UserData In utentiPerScuola(scuola)
                Dim utenteNode As New TreeNode(utente.Name)
                utenteNode.Tag = utente ' Memorizza la classe UserData nel tag del nodo utente
                utenteNode.ForeColor = coloreScuola ' Imposta il colore per l'utente
                scuolaNode.Nodes.Add(utenteNode)
            Next
        Next
        If treeview_output.Nodes.Count > 0 Then
            SyncLock ListaAlunniScuole
                ' Utilizza un Invoke per aggiungere i nodi in modo sicuro alla treeview.
                ListaAlunniScuole.Invoke(Sub()
                                             For Each node As TreeNode In treeview_output.Nodes
                                                 ListaAlunniScuole.Nodes.Add(DirectCast(node.Clone(), TreeNode))
                                             Next
                                         End Sub)
            End SyncLock
        End If
        SalvaColoriScuola(coloriScuola, pathToFile)
    End Sub

    Private Function CaricaColoriScuola(ByVal pathToFile As String) As Dictionary(Of String, Color)
        Dim coloriScuola As New Dictionary(Of String, Color)()

        ' Verifica se il file esiste prima di tentare di leggerlo
        If File.Exists(pathToFile) Then
            Try
                Using reader As New StreamReader(pathToFile)
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine()
                        Dim parts As String() = line.Split(","c)
                        If parts.Length = 4 Then
                            Dim scuola As String = parts(0)
                            Dim r As Integer
                            Dim g As Integer
                            Dim b As Integer
                            If Integer.TryParse(parts(1), r) AndAlso Integer.TryParse(parts(2), g) AndAlso Integer.TryParse(parts(3), b) Then
                                Dim colore As Color = Color.FromArgb(r, g, b)
                                coloriScuola(scuola) = colore
                            End If
                        End If
                    End While
                End Using
            Catch ex As Exception
                ' Gestisci eventuali errori nella lettura del file
                Console.WriteLine("Errore nella lettura del file: " & ex.Message)
                MYlogManager.LogMessage("Errore nella lettura del file: " & ex.Message, "CaricaColoriScuola-AlunniPresenti")
            End Try
        End If

        Return coloriScuola
    End Function
    Private Sub SalvaColoriScuola(ByVal coloriScuola As Dictionary(Of String, Color), ByVal pathToFile As String)
        Try
            Using writer As New StreamWriter(pathToFile)
                For Each coppia In coloriScuola
                    Dim scuola As String = coppia.Key
                    Dim colore As Color = coppia.Value
                    writer.WriteLine($"{scuola},{colore.R},{colore.G},{colore.B}")
                Next
            End Using
        Catch ex As Exception
            ' Gestisci eventuali eccezioni qui, ad esempio:
            MessageBox.Show("Impossibile salvare i colori delle scuole.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MYlogManager.LogMessage("Errore Impossibile salvare i colori delle scuole." & ex.Message, "SalvaColoriScuola-AlunniPresenti")
        End Try
    End Sub


    ' Funzione per ottenere un colore univoco per una scuola, assicurandosi che i colori non siano uguali o simili
    Function OttieniColoreScuola(ByVal scuola As String, ByVal coloriScuola As Dictionary(Of String, Color), ByVal coloreRiferimento As Color) As Color
        If Not coloriScuola.ContainsKey(scuola) Then
            Dim random As New Random()
            Dim minDistance As Integer = 100  ' Distanza minima tra colori (modificabile)
            Dim newColor As Color

            ' Continua a generare nuovi colori finché non si trova uno sufficientemente distante dagli altri e abbastanza contrastante
            Do
                newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))

                ' Controlla la distanza tra il nuovo colore e gli altri colori delle scuole
                Dim isColorUnique As Boolean = True
                For Each existingColor As Color In coloriScuola.Values
                    Dim distance As Integer = Math.Abs(CInt(existingColor.R) - CInt(newColor.R)) +
                                     Math.Abs(CInt(existingColor.G) - CInt(newColor.G)) +
                                     Math.Abs(CInt(existingColor.B) - CInt(newColor.B))

                    If distance < minDistance Then
                        isColorUnique = False
                        Exit For
                    End If
                Next

                ' Controlla anche il contrasto rispetto al colore di riferimento
                Dim contrast As Double = CalcolaContrasto(newColor, coloreRiferimento)
                If contrast >= 3.0 Then
                    isColorUnique = True
                End If

                ' Se il colore è sufficientemente distante e contrastante, assegnalo alla scuola
                If isColorUnique Then
                    coloriScuola(scuola) = newColor
                End If
            Loop While Not coloriScuola.ContainsKey(scuola)
        End If

        Return coloriScuola(scuola)
    End Function

    Function CalcolaContrasto(ByVal colore1 As Color, ByVal colore2 As Color) As Double
        ' Formula per il calcolo del contrasto tra due colori (WCAG 2.0)
        Dim luminance1 As Double = (0.299 * colore1.R + 0.587 * colore1.G + 0.114 * colore1.B) / 255.0
        Dim luminance2 As Double = (0.299 * colore2.R + 0.587 * colore2.G + 0.114 * colore2.B) / 255.0
        Dim luminanceMax As Double = Math.Max(luminance1, luminance2)
        Dim luminanceMin As Double = Math.Min(luminance1, luminance2)

        Return (luminanceMax + 0.05) / (luminanceMin + 0.05)
    End Function

    Private Sub ListaAlunniScuole_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles ListaAlunniScuole.NodeMouseClick
        ' Verifica se il nodo cliccato è un nodo utente (non un nodo scuola o root)
        If e.Node.Parent IsNot Nothing AndAlso e.Node.Tag IsNot Nothing AndAlso TypeOf e.Node.Tag Is UserData Then
            Dim dataObject As UserData = DirectCast(e.Node.Tag, UserData)
            ' Carica il nome dell'utente nella TextBox NomeTimbrante
            NomeTimbrante.Text = dataObject.Name
            ' Assegna il tag dell'utente all'oggetto NomeTimbrante.Tag
            NomeTimbrante.Tag = dataObject
            CheckBox1.Checked = False
        End If
    End Sub
    Private Sub ListaAlunniScuole_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles ListaAlunniScuole.NodeMouseDoubleClick
        ' Verifica se il nodo cliccato è un nodo scuola (non un nodo utente o root)
        If e.Node.Parent IsNot Nothing AndAlso e.Node.Tag Is Nothing Then
            Dim scuola As String = e.Node.Text

            ' Mostra il dialogo di modifica del colore per la scuola
            If EditorColoreScuola.ShowDialog() = DialogResult.OK Then
                ' Ottieni il colore selezionato
                Dim nuovoColore As Color = EditorColoreScuola.Color

                Dim thread_modifica As New Thread(AddressOf ModificaColoreScuola)
                thread_modifica.SetApartmentState(ApartmentState.STA)
                thread_modifica.IsBackground = False
                thread_modifica.Start(New Object() {scuola, nuovoColore})

            End If
        End If
    End Sub
    Private Sub ModificaColoreScuola(ByVal obj As Object)
        Dim scuola As String = CStr(obj(0))
        Dim nuovoColore As Color = CType(obj(1), Color)


        ' Carica i colori delle scuole dal file
        Dim pathToFile As String = Path_Progetto & "/conf/school.color"
        Dim coloriScuola As Dictionary(Of String, Color) = CaricaColoriScuola(pathToFile)

        ' Aggiorna il colore della scuola nel dizionario dei colori
        coloriScuola(scuola) = nuovoColore

        ' Salva i colori aggiornati nel file
        SalvaColoriScuola(coloriScuola, pathToFile)
    End Sub


    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        NotRegistredUser.Enabled = CheckBox1.Checked
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

    Private Sub NomeUtente_TextChanged(sender As Object, e As EventArgs) Handles NomeUtente.TextChanged
        NomeTimbrante.Text = NomeUtente.Text
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim objectData As New UserData
        If CheckBox1.Checked Then
            If BoxEta.BackColor <> Color.Green Then
                MessageBox.Show("Data dell'utente guest inserito non valida.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If NomeUtente.Text.Replace(" ", "") = "" Then
                MessageBox.Show("Nome utente non valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If ScuolaUtente.SelectedItem.ToString().Replace(" ", "") = "" Then
                MessageBox.Show("Scuola selezionata non valida.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            objectData.Id = ident_guest_user_id
            objectData.Name = NomeUtente.Text
            objectData.School = ScuolaUtente.SelectedItem.ToString()
            objectData.Birthday = ConvertiTestoInData(BoxEta.Text)
            objectData.Badge = ident_guest_user
        Else
            objectData = DirectCast(NomeTimbrante.Tag, UserData)
            objectData.Badge  = objectData.Badge & ident_manual_stamping
        End If
        If objectData IsNot Nothing Then
            badge_reading(objectData.Name, objectData.Badge, DateTime.Now.ToString(DataTimeFormat)) 'inserisce l'utente nella classe
            RaiseEvent manual_stamping(objectData.Name, objectData.Badge, DateTime.Now.ToString(DataTimeFormat)) 'logga l'evento
            TimbraturaManuale.Visible = False
            CheckBox1.Checked = False
        Else
            MessageBox.Show("Nessun utente selezionato.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub







#End Region



#End Region
End Class