Imports System.Threading

Public Class LogExplorer

    Private Sub LogExplorer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim th_load As New Thread(AddressOf LoadLogs)
        th_load.IsBackground = False
        th_load.SetApartmentState(ApartmentState.STA)
        th_load.Start()
    End Sub

    Private Sub LoadLogs()
        Dim trview As TreeView = MYlogManager.CreateLogsTreeView()
        SyncLock trview
            Invoke(Sub() TreeView.Nodes.Clear()) ' Pulisci il tuo TreeView prima di copiarlo
            For Each node As TreeNode In trview.Nodes
                Invoke(Sub() TreeView.Nodes.Add(DirectCast(node.Clone(), TreeNode)))
            Next
        End SyncLock
    End Sub
    Private Sub TreeView_DoubleClick(sender As Object, e As EventArgs) Handles TreeView.DoubleClick
        Dim selectedNode As TreeNode = TreeView.SelectedNode

        ' Verifica se è stato selezionato un nodo con un percorso del file nei tag
        If selectedNode IsNot Nothing AndAlso selectedNode.Tag IsNot Nothing Then
            Dim logFilePath As String = selectedNode.Tag.ToString()

            Try
                ' Leggi il contenuto del file di log
                Dim logLines As String() = IO.File.ReadAllLines(logFilePath)

                ' Cancella il contenuto precedente nella RichTextBox
                LogsEvent.Clear()

                ' Inizializza la variabile per l'alternanza dei colori
                Dim redColor As Boolean = True

                For Each logLine As String In logLines
                    Dim parts As String() = logLine.Split(New String() {" - "}, StringSplitOptions.None)

                    If parts.Length = 2 Then
                        Dim logDate As String = parts(0)
                        Dim logMessage As String = parts(1)

                        ' Scegli il colore del testo in base al valore di redColor
                        Dim textColor As Color = If(redColor, Color.DarkRed, Color.Firebrick)

                        ' Aggiungi il testo formattato alla RichTextBox
                        LogsEvent.SelectionColor = textColor
                        LogsEvent.SelectionFont = New Font(LogsEvent.Font, FontStyle.Bold) ' Imposta il testo in grassetto
                        LogsEvent.AppendText(logDate)
                        LogsEvent.SelectionFont = New Font(LogsEvent.Font, FontStyle.Regular) ' Ripristina il testo normale
                        LogsEvent.SelectionColor = Color.Black ' Ripristina il colore del testo a nero
                        LogsEvent.AppendText(" - " & logMessage & vbCrLf)

                        ' Inverti il valore di redColor per alternare i colori
                        redColor = Not redColor
                    End If
                Next


            Catch ex As Exception
                ' Gestione degli errori nel caso in cui il file non possa essere letto
                LogsEvent.Text = "Errore nella lettura del file di log: " & ex.Message
            End Try
        End If
    End Sub



End Class
