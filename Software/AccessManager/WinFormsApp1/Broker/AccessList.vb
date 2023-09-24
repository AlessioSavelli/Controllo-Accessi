Imports System.Diagnostics.Eventing
Imports System.DirectoryServices.ActiveDirectory
Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Threading.Thread
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock
Imports Microsoft.Data.Sqlite

Public Class AccessList

    Dim color1 As Color = Color.White ' Colore per le righe dispari
    Dim color2 As Color = Color.LightGray ' Colore per le righe pari
    Dim color3 As Color = Color.LightBlue 'identifica le righe disperi aggiunge in questa nuova sessione

    Dim queqee_list As New List(Of ListViewItem)
    Dim block_log_view As Boolean = True






    Private Sub AccessList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim today As DateTime = DateTime.Now
        Dim lastWeekStart As DateTime = today.AddDays(-7)
        Me.Text = "Lista accessi da : " & lastWeekStart.ToString(Split(DataTimeFormat, " ")(0)) & " ad OGGI"

        CopiaStileListView(AccessList1, ListRicerca)

        Dim populate_list As New Thread(AddressOf load_logs)
        populate_list.IsBackground = False
        populate_list.SetApartmentState(Threading.ApartmentState.STA)
        populate_list.Start()



    End Sub

    Public Sub add_access(ByVal name As String, ByVal uid As String, ByVal time As String)
        If String.IsNullOrEmpty(time) Then
            Exit Sub
        End If
        Dim new_adding_th As New Thread(AddressOf add_user)
        new_adding_th.IsBackground = False
        new_adding_th.SetApartmentState(Threading.ApartmentState.STA)
        Dim parms As String() = New String() {name, uid, time}
        new_adding_th.Start(parms)
    End Sub

    Private Sub ItemCount_Tick(sender As Object, e As EventArgs) Handles ItemCount.Tick
        If ListRicerca.Visible = True Then
            item.Text = "Nr : " & ListRicerca.Items.Count
        Else
            item.Text = "Nr : " & AccessList1.Items.Count
        End If
    End Sub


#Region "Caricamento log settimanali"
    Private Async Sub load_logs()
        ' Imposta block_log_view su True
        block_log_view = True

        ' Calcola la data di inizio e fine della settimana scorsa
        Dim today As DateTime = DateTime.Now
        Dim lastWeekStart As DateTime = today.AddDays(-7)

        ' Estrai i log dalla data di inizio alla data di fine
        Dim get_logs As List(Of LogEntry) = Await DataBase.ExtractLogsFromDateAsync(New DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day, 0, 0, 0), New DateTime(today.Year, today.Month, today.Day, 23, 59, 59))
        If get_logs Is Nothing Then Exit Sub
        If get_logs.Count = 0 Then Exit Sub
        Dim itemsToAdd As New List(Of ListViewItem)()

        For Each line As LogEntry In get_logs
            Dim item As New ListViewItem(line.name, 0)
            item.SubItems.AddRange(New String() {line.data.ToString(DataTimeFormat), line.scuola, line.UID_badge})
            item.Tag = line.user_Id

            ' Imposta il colore di sfondo dell'elemento in base all'alternanza di colori
            If itemsToAdd.Count Mod 2 = 0 Then
                item.BackColor = color1
            Else
                item.BackColor = color2
            End If

            itemsToAdd.Add(item)

        Next

        ' Aggiungi i log in attesa di essere aggiunti nella lista
        SyncLock queqee_list
            itemsToAdd.AddRange(queqee_list)
            queqee_list.Clear()
        End SyncLock

        itemsToAdd.Reverse()

        Invoke(Sub()
                   SyncLock AccessList1
                       ' Disabilita temporaneamente la ListView
                       AccessList1.Enabled = False
                       ' Aggiungi gli elementi alla ListView
                       AccessList1.Items.AddRange(itemsToAdd.ToArray())
                       ' Riabilita la ListView
                       AccessList1.Enabled = True
                   End SyncLock
               End Sub)

        ' Imposta block_log_view su False
        block_log_view = False
    End Sub
#End Region

#Region "log eventi di beggiatura"

    Private Async Sub add_user(ByVal obj As Object)
        Dim name As String = CStr(obj(0))
        Dim uid_badge As String = CStr(obj(1))
        Dim time As String = CStr(obj(2))

        Dim userDataList As List(Of UserData)

        If (uid_badge.Replace(ident_manual_stamping, "") = ident_guest_user) Then
            userDataList = New List(Of UserData)()
            userDataList.Add(CreateGuestUserData(uid_badge.Replace(ident_manual_stamping, ""), name))
        Else
            userDataList = Await DataBase.ExtractUserByBadgeAsync(uid_badge.Replace(ident_manual_stamping, ""))
        End If


        Dim scuola As String = userDataList(0).School ' info.GetString(3)
        Dim uid_user As String = userDataList(0).Id 'info.GetString(3)



        'aggiungi il log al database
        Await DataBase.AddNewLoginAsync(name, time, uid_badge, scuola, uid_user)

        'ora fallo vedere nella listview

        Dim item As New ListViewItem(name, 0)
        item.SubItems.AddRange(New String() {time, scuola, uid_badge})
        item.Tag = uid_user
        If (AccessList1.Items.Count) Mod 2 = 0 Then
            item.BackColor = color1
        Else
            item.BackColor = color3 'color2
        End If
        If block_log_view Then
            SyncLock queqee_list
                Invoke(Sub() queqee_list.Add(item)) 'logga l'evento in una lista tampone)
            End SyncLock

        Else
            SyncLock AccessList1
                Invoke(Sub()
                           AccessList1.Items.Insert(0, item)
                       End Sub) 'fai il cross thread
            End SyncLock
        End If

    End Sub



#End Region

#Region "Gestione eventi listview"



    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text = "" Then
                ListRicerca.Visible = False
            Else
                ListRicerca.Visible = True
                TextBox1.Enabled = False
                Dim search As New Thread(AddressOf CercaElementi)
                search.IsBackground = False
                search.SetApartmentState(Threading.ApartmentState.STA)
                search.Start(TextBox1.Text)
            End If
        End If
    End Sub
    Private Sub CercaElementi(ByVal testoDaCercare As String)
        ' Converti il testo da cercare in minuscolo una volta sola
        testoDaCercare = testoDaCercare.ToLower()
        Dim itemsToProcess As List(Of ListViewItem) = Nothing

        ' Disabilita la TextBox1
        Invoke(Sub()
                   TextBox1.Enabled = False
                   SyncLock AccessList1
                       itemsToProcess = AccessList1.Items.Cast(Of ListViewItem)().ToList()
                   End SyncLock
               End Sub)

        ' Crea una lista per gli elementi da aggiungere alla ListRicerca
        Dim itemsToAdd As New List(Of ListViewItem)()

        ' Itera attraverso gli elementi ottenuti dall'Invoke sfruttando al massimo il thread
        If itemsToProcess IsNot Nothing Then
            For Each item As ListViewItem In itemsToProcess
                ' Verifica tutti i subelementi dell'elemento
                For Each subItem As ListViewItem.ListViewSubItem In item.SubItems
                    ' Converte il testo del subelemento in minuscolo per la comparazione case-insensitive
                    Dim subItemText As String = subItem.Text.ToLower()
                    ' Controlla se il testoDaCercare è contenuto in questo subelemento (case-insensitive)
                    If subItemText.Contains(testoDaCercare) Then
                        ' Trovato un subelemento con il testo desiderato, aggiungi l'elemento alla lista temporanea
                        'itemsToAdd.Add(item)
                        itemsToAdd.Add(New ListViewItem(item.Text, item.ImageIndex))
                        For Each subItem2 As ListViewItem.ListViewSubItem In item.SubItems
                            If subItem2.Text IsNot item.Text Then
                                itemsToAdd(itemsToAdd.Count - 1).SubItems.Add(subItem2.Text)
                            End If
                        Next
                        If (itemsToAdd.Count + 1) Mod 2 = 0 Then
                            itemsToAdd(itemsToAdd.Count - 1).BackColor = color1
                        Else
                            itemsToAdd(itemsToAdd.Count - 1).BackColor = color2
                        End If

                        Exit For
                    End If
                Next


            Next
        End If

        ' Aggiungi gli elementi alla ListRicerca in un unico Invoke
        Invoke(Sub()
                   SyncLock ListRicerca
                       ListRicerca.Items.Clear()
                       ListRicerca.Items.AddRange(itemsToAdd.ToArray())
                       TextBox1.Enabled = True
                   End SyncLock
               End Sub)
    End Sub






#End Region

End Class