Imports System.IO
Imports System.Text

Public Class LogManager
    Private logBaseDirectory As String

    Public Sub New(logBaseDirectoryPath As String)
        If Not Directory.Exists(logBaseDirectoryPath) Then
            Directory.CreateDirectory(logBaseDirectoryPath)
        End If
        Me.logBaseDirectory = logBaseDirectoryPath
    End Sub

    Public Sub LogMessage(message As String, logType As String)
        Dim currentDate As Date = Date.Now
        Dim yearDirectory As String = Path.Combine(logBaseDirectory, currentDate.Year.ToString())
        Dim monthDirectory As String = Path.Combine(yearDirectory, currentDate.ToString("MMMM", New Globalization.CultureInfo("it-IT")))
        Dim dayOfWeek As String = currentDate.ToString("dddd", New Globalization.CultureInfo("it-IT"))
        Dim dayNumber As String = currentDate.Day.ToString("00")
        Dim logTypeDirectory As String = Path.Combine(monthDirectory, logType)
        Dim logFileName As String = Path.Combine(logTypeDirectory, $"{dayOfWeek} {dayNumber}.log")

        Try
            ' Assicura che le cartelle siano create
            Directory.CreateDirectory(yearDirectory)
            Directory.CreateDirectory(monthDirectory)
            Directory.CreateDirectory(logTypeDirectory)

            Using writer As New StreamWriter(logFileName, True, Encoding.UTF8)
                ' Registra l'orario e il messaggio dell'evento
                writer.WriteLine($"[{currentDate}] - {message}")
            End Using
        Catch ex As Exception
            ' Gestione degli errori nella registrazione dei log, ad esempio scrivendo l'errore su Console
            Console.WriteLine($"Errore nella registrazione del log ({logType}): {ex.Message}")
        End Try
    End Sub

#Region "Read log"

    Public Function CreateLogsTreeView() As TreeView
        Dim treeView As New TreeView

        If Not Directory.Exists(logBaseDirectory) Then
            MessageBox.Show("La directory dei log non esiste.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return treeView
        End If

        ' Ottieni tutte le directory degli anni
        Dim yearDirectories As String() = Directory.GetDirectories(logBaseDirectory)
        For Each yearDirectory As String In yearDirectories
            Dim yearNode As New TreeNode(Path.GetFileName(yearDirectory))
            treeView.Nodes.Add(yearNode)

            ' Ottieni tutte le directory dei mesi all'interno dell'anno
            Dim monthDirectories As String() = Directory.GetDirectories(yearDirectory)
            For Each monthDirectory As String In monthDirectories
                Dim monthNode As New TreeNode(Path.GetFileName(monthDirectory))
                yearNode.Nodes.Add(monthNode)

                ' Ottieni tutte le sottocartelle dei tipi di evento all'interno del mese
                Dim logTypeDirectories As String() = Directory.GetDirectories(monthDirectory)
                For Each logTypeDirectory As String In logTypeDirectories
                    Dim logTypeNode As New TreeNode(Path.GetFileName(logTypeDirectory))
                    monthNode.Nodes.Add(logTypeNode)

                    ' Ottieni tutti i file di log all'interno del tipo di evento
                    Dim logFiles As String() = Directory.GetFiles(logTypeDirectory)
                    For Each logFile As String In logFiles
                        Dim logFileName As String = Path.GetFileNameWithoutExtension(logFile)
                        Dim dateNode As New TreeNode(logFileName)
                        dateNode.ImageIndex = 2
                        dateNode.SelectedImageIndex = 3
                        dateNode.Tag = logFile ' Salva il percorso del file nel tag del nodo

                        logTypeNode.Nodes.Add(dateNode)
                    Next
                Next
            Next
        Next

        Return treeView
    End Function


#End Region
End Class