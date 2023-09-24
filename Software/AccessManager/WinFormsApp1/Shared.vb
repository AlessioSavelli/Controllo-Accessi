Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions

Module Shared_entry
    Public Path_Progetto As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    Public MYlogManager As New LogManager(Path_Progetto & "/conf/logs")

    Public DataTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Public DataTimeFormat_BDay As String = "dd/MM/yyyy"
    Public DataBase As New sqlitedb(Path_Progetto & "/conf", "db_log.db")
    Public tempo_minimo_di_uscita As Int16 = 1 'devi stare in aula almeno un minuto prima che tu possa essere rimosso

    Public Const ident_manual_stamping As String = " (M)"
    Public Const ident_guest_user As String = "Utente Guest"
    Public Const ident_guest_user_id As Integer = 0


End Module
Module Shared_function
    Public Function CreateGuestUserData(ByVal badge As String, ByVal nomeGuest As String) As UserData
        Dim guestUserData As New UserData()
        guestUserData.Id = ident_guest_user_id
        guestUserData.Badge = badge
        guestUserData.Name = nomeGuest
        guestUserData.School = "Scuola Ospite" ' Scuola di default
        guestUserData.Gender = "N/A" ' Scuola di default
        guestUserData.Birthday = Now
        guestUserData.PicPath = ""
        guestUserData.RegDate = Now

        Return guestUserData
    End Function

    Public Sub CopiaStileListView(ByVal sourceListView As Windows.Forms.ListView, ByVal destinationListView As Windows.Forms.ListView)
        ' Copia le proprietà di stile dalla ListView di origine alla ListView di destinazione
        destinationListView.BackColor = sourceListView.BackColor
        destinationListView.ForeColor = sourceListView.ForeColor
        destinationListView.Font = sourceListView.Font
        destinationListView.View = sourceListView.View
        destinationListView.GridLines = sourceListView.GridLines
        destinationListView.FullRowSelect = sourceListView.FullRowSelect
        destinationListView.HeaderStyle = sourceListView.HeaderStyle

        ' Copia le dimensioni e gli ancoraggi
        destinationListView.Size = sourceListView.Size
        destinationListView.Anchor = sourceListView.Anchor
        destinationListView.Location = sourceListView.Location

        ' Copia le colonne dalla ListView di origine alla ListView di destinazione
        destinationListView.Columns.Clear()
        For Each column As ColumnHeader In sourceListView.Columns
            destinationListView.Columns.Add(DirectCast(column.Clone(), ColumnHeader))
        Next
    End Sub


    Public Sub CaricaScuoleInComboBox(ByVal comboBox As ComboBox)
        Dim scuoleFilePath As String = Path.Combine(Path_Progetto, "conf", "scuole.txt")

        If File.Exists(scuoleFilePath) Then
            ' Leggi le scuole dal file e aggiungile alla ComboBox
            Dim scuole As String() = File.ReadAllLines(scuoleFilePath)

            ' Assicurati che la ComboBox sia vuota prima di aggiungere le scuole
            comboBox.Items.Clear()

            ' Aggiungi le scuole alla ComboBox specificata
            comboBox.Items.AddRange(scuole)

            ' Seleziona la prima scuola come predefinita, se presente
            If comboBox.Items.Count > 0 Then
                comboBox.SelectedIndex = 0
            End If
        End If
    End Sub

    Public Function VerificaDataDiNascita(ByVal dataDiNascitaText As String) As Boolean
        Dim cultura As New CultureInfo("it-IT") ' Imposta la cultura italiana per il formato data

        ' Verifica se il testo inserito è una data valida
        Dim dataDiNascita As Date
        If Date.TryParseExact(dataDiNascitaText, DataTimeFormat_BDay, cultura, DateTimeStyles.None, dataDiNascita) Then
            ' La data è valida
            Return True
        Else
            ' La data non è valida
            Return False
        End If
    End Function
    Public Function ConvertiTestoInData(ByVal data As String) As DateTime

        Dim dataConvertita As DateTime
        If DateTime.TryParseExact(data, DataTimeFormat_BDay, Nothing, DateTimeStyles.None, dataConvertita) Then
            ' La conversione è riuscita
            Return dataConvertita
        Else
            ' Gestisci il caso in cui la conversione non sia riuscita (formato non valido)
            Throw New Exception("Formato data non valido")
        End If
    End Function
    Public Function ConvertHexStringToByteArray(ByVal inputString As String) As Byte()
        ' Rimuovi gli spazi bianchi e suddividi i byte utilizzando la virgola come delimitatore
        Dim byteStrings() As String = inputString.Split(","c, StringSplitOptions.RemoveEmptyEntries)

        ' Inizializza un array di byte per contenere i byte convertiti
        Dim byteArray(byteStrings.Length - 1) As Byte

        For i As Integer = 0 To byteStrings.Length - 1
            ' Rimuovi spazi vuoti iniziali e finali
            Dim byteValue As String = byteStrings(i).Trim()

            ' Verifica se il byte inizia con "&H" e rimuovilo, se presente
            If byteValue.StartsWith("&H", StringComparison.OrdinalIgnoreCase) Then
                byteValue = byteValue.Substring(2)
            End If

            ' Converte il byte esadecimale in un byte e lo inserisce nell'array
            byteArray(i) = Convert.ToByte(byteValue, 16)
        Next

        Return byteArray

    End Function

    Function CorrectionHEXValues(inputString As String) As String
        inputString = inputString.Replace("0x0 ", "0x00 ")
        inputString = inputString.Replace("0x1 ", "0x01 ")
        inputString = inputString.Replace("0x2 ", "0x02 ")
        inputString = inputString.Replace("0x3 ", "0x03 ")
        inputString = inputString.Replace("0x4 ", "0x04 ")
        inputString = inputString.Replace("0x5 ", "0x05 ")
        inputString = inputString.Replace("0x6 ", "0x06 ")
        inputString = inputString.Replace("0x7 ", "0x07 ")
        inputString = inputString.Replace("0x8 ", "0x08 ")
        inputString = inputString.Replace("0x9 ", "0x09 ")
        inputString = inputString.Replace("0xa ", "0x0a ")
        inputString = inputString.Replace("0xb ", "0x0b ")
        inputString = inputString.Replace("0xc ", "0x0c ")
        inputString = inputString.Replace("0xd ", "0x0d ")
        inputString = inputString.Replace("0xe ", "0x0e ")
        inputString = inputString.Replace("0xf ", "0x0f ")
        Return inputString
    End Function
End Module
Public Class UserData
    Public Property Id As Integer
    Public Property Name As String
    Public Property Gender As String
    Public Property School As String
    Public Property Birthday As DateTime
    Public Property PicPath As String = ""
    Public Property RegDate As DateTime
    Public Property Badge As String
End Class
Public Class LogEntry
    Public Property name As String
    Public Property user_Id As Integer
    Public Property data As DateTime
    Public Property scuola As String
    Public Property UID_badge As String
End Class
Public Class ClassroomEntry
    Public Property EntryTime As DateTime
    Public Property ExitTime As DateTime
End Class

Public Class UserClassroomUsage
    Public Property UserId As Integer
    Public Property ClassroomEntries As New List(Of ClassroomEntry)
End Class
Public Class ListViewItemGroup
    Public Name As String
    Public Items As List(Of ListViewItem)

    Public Sub New(name As String)
        Me.Name = name
        Me.Items = New List(Of ListViewItem)()
    End Sub
End Class

Public Class DefineProcessor
    Private defineDictionary As New Dictionary(Of String, String)()

    Public Sub New()
        ' Inizializza il dizionario delle definizioni
        defineDictionary = New Dictionary(Of String, String)()
    End Sub

    Public Sub ProcessDefines(filePath As String)
        ' Inizializza il dizionario dei define
        defineDictionary = New Dictionary(Of String, String)()

        ' Leggi il contenuto del file linea per linea
        Dim lines() As String = File.ReadAllLines(filePath)

        For Each line As String In lines
            ' Rimuovi i commenti dalla linea
            Dim cleanLine As String = RemoveComments(line)

            ' Trova definizioni di define
            Dim match As Match = Regex.Match(cleanLine, "^#define\s+(\w+)\s+(.+)$")
            If match.Success Then
                Dim defineName As String = match.Groups(1).Value
                Dim defineValue As String = match.Groups(2).Value

                ' Controlla se il valore del define è il nome di un altro define
                If defineDictionary.ContainsKey(defineValue) Then
                    defineValue = defineDictionary(defineValue)
                End If

                ' Aggiungi o aggiorna il define nel dizionario
                If Not defineDictionary.ContainsKey(defineName) Then
                    defineDictionary.Add(defineName, defineValue)
                Else
                    defineDictionary(defineName) = defineValue
                End If
            End If
        Next
    End Sub
    Private Function RemoveComments(line As String) As String
        ' Rimuovi i commenti dalla linea
        Dim commentIndex As Integer = line.IndexOf("//")
        If commentIndex >= 0 Then
            line = line.Substring(0, commentIndex)
        End If
        Return line.Trim()
    End Function
    Public Function ProcessLine(filePath As String) As String
        ' Leggi il contenuto del file
        Dim fileContent As String = File.ReadAllText(filePath)

        ' Trova la riga di output
        Dim outputLine As String = ""
        Dim outputMatch As Match = Regex.Match(fileContent, "^OUTPUT\s*=\s*(.+)", RegexOptions.Multiline)
        If outputMatch.Success Then
            outputLine = outputMatch.Groups(1).Value
        End If

        ' Sostituisci le definizioni nei valori di outputLine
        For Each defineName As String In defineDictionary.Keys
            Dim defineValue As String = defineDictionary(defineName)
            outputLine = Regex.Replace(outputLine, "\b" & Regex.Escape(defineName) & "\b", defineValue)
        Next

        Return outputLine
    End Function
End Class