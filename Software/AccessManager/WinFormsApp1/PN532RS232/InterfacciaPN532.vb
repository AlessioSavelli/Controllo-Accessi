Imports System.Collections.Concurrent
Imports System.Threading
Imports RJCP.IO.Ports

Public Class InterfacciaPN532

    Public Enum SERIAL_HEADER As Byte
        ERROR_MODULE = &H1
        VERS_MODULE = &H2
        WAIT_BADGE = &H3
        PRESENT_BADGE = &H4
        GET_UID_BADGE = &H5
        WRITE_BADGE = &H6
        SUCCESS_WRITE_BADGE = &H7


        ERROR_WRITE_BADGE_UID = &HF1
        ERROR_WRITE_BADGE_AUTH1 = &HF2
        ERROR_WRITE_BADGE_AUTH2 = &HF3
        ERROR_WRITE_BADGE_BRENDSECTOR = &HF4
        ERROR_WRITE_BADGE_LINE1 = &HF5
        ERROR_WRITE_BADGE_LINE2 = &HF6
        ERROR_WRITE_BADGE_TRAILER = &HF7

        ERROR_TIMEOUT = &HF0
        ERROR_FRAME_LENGHT = &HFD
        ERROR_FRAME = &HFE
        END_COMUNICATE = &HFF


    End Enum

    Private WithEvents _serial As SerialPortStream
    Private dataBuffer As ConcurrentQueue(Of Byte)
    Private dataReadyEvent As AutoResetEvent
    Private Const timeoutMilliseconds As Integer = 2000 ' 2 secondi
    Public Sub New(portName As String)
        _serial = New SerialPortStream(portName, 115200)
        dataBuffer = New ConcurrentQueue(Of Byte)()
        dataReadyEvent = New AutoResetEvent(False)

        ' Imposta la gestione degli eventi per la porta seriale
        AddHandler _serial.DataReceived, AddressOf SerialDataReceivedHandler
        ' Imposta il controllo di flusso su None
        _serial.Handshake = Handshake.None
        ' Apri la porta seriale
        _serial.Open()
    End Sub
    Public Sub Close()
        HW_reset() 'Resetta Arduino
        ' Chiudi la porta seriale e rilascia le risorse
        If _serial IsNot Nothing Then
            _serial.Close()
            _serial.Dispose()
        End If
    End Sub
    Private Sub SerialDataReceivedHandler(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        ' Leggi i dati dalla porta seriale e mettili nel buffer
        While _serial.BytesToRead > 0
            Dim receivedByte As Byte = CByte(_serial.ReadByte())
            dataBuffer.Enqueue(receivedByte)
        End While

        ' Segnala che ci sono dati pronti
        dataReadyEvent.Set()
    End Sub

    ' Metodo per inviare un comando al modulo PN532 (rendilo asincrono)
    Private Async Function SendCommand(ByVal commandCode As Byte) As Task
        ' Esempio di come inviare un comando al modulo PN532
        ' Comandi specifici possono essere implementati qui
        Dim buffer() As Byte = {commandCode, SERIAL_HEADER.END_COMUNICATE}
        Await _serial.WriteAsync(buffer, 0, buffer.Length)
    End Function

    ' Metodo per inviare un comando al modulo PN532 (rendilo asincrono)
    Private Async Function SendCommand(ByVal buffer() As Byte) As Task
        ' Aggiungi SERIAL_HEADER.END_COMUNICATE alla fine del buffer
        Dim extendedBuffer(buffer.Length) As Byte
        Array.Copy(buffer, extendedBuffer, buffer.Length)
        extendedBuffer(buffer.Length) = CByte(SERIAL_HEADER.END_COMUNICATE)

        ' Esempio di come inviare un comando al modulo PN532
        ' Comandi specifici possono essere implementati qui
        Await _serial.WriteAsync(extendedBuffer, 0, extendedBuffer.Length)
    End Function

    Public Async Function RequestVersion() As Task(Of Byte)
        Dim expectedResponse As Byte = SERIAL_HEADER.VERS_MODULE
        Dim endCommunicate As Byte = SERIAL_HEADER.END_COMUNICATE

        ' Invia il comando VERS_MODULE
        Await SendCommand(SERIAL_HEADER.VERS_MODULE)

        ' Attendere che ci siano dati nel buffer
        If Not dataReadyEvent.WaitOne(timeoutMilliseconds) Then Return SERIAL_HEADER.ERROR_MODULE

        ' Leggi i dati dal buffer
        Dim bytesRead As Integer = 0
        Dim responseBuffer(2) As Byte

        While bytesRead < 3
            Dim receivedByte As Byte
            If dataBuffer.TryDequeue(receivedByte) Then
                responseBuffer(bytesRead) = receivedByte
                bytesRead += 1
            End If
        End While

        ' Se la risposta è quella attesa, esci dal ciclo
        If responseBuffer(0) = expectedResponse AndAlso responseBuffer(2) = endCommunicate Then
            Return responseBuffer(1) ' Restituisci il dato ricevuto
        End If
        Return SERIAL_HEADER.ERROR_MODULE
    End Function

    Public Async Function WaitForBadge() As Task(Of Boolean)
        Dim timeout As DateTime
        Dim responseBuffer(1) As Byte

        Do
            ' Invia il comando WAIT_BADGE
            Await SendCommand(SERIAL_HEADER.WAIT_BADGE)

            ' Attendi la risposta per un breve periodo (ad esempio, 5 secondo)
            timeout = DateTime.Now.AddMilliseconds(5000)

            While DateTime.Now < timeout AndAlso dataBuffer.Count < 2
                ' Attendi finché non hai abbastanza dati nel buffer
            End While

            ' Leggi i due byte dalla coda dataBuffer
            Dim bytesRead As Integer = 0
            While bytesRead < 2
                If dataBuffer.TryDequeue(responseBuffer(bytesRead)) Then
                    bytesRead += 1
                End If
            End While

            ' Verifica se abbiamo ricevuto la conferma corretta
            If bytesRead = 2 AndAlso responseBuffer(0) = SERIAL_HEADER.PRESENT_BADGE AndAlso responseBuffer(1) = SERIAL_HEADER.END_COMUNICATE Then
                ' Ricevuta la conferma PRESENT_BADGE
                Return True
            End If

            ' Se non abbiamo ricevuto una risposta valida, attendi per un breve periodo e riprova
            Thread.Sleep(1000) ' Attendiamo 1 secondo prima di riprovare

        Loop While True ' Continua finché non ricevi una risposta valida
        Return False
    End Function


    Public Async Function GetBadgeUID() As Task(Of String)
        Dim expectedHeader As Byte = SERIAL_HEADER.GET_UID_BADGE
        Dim expectedFooter As Byte = SERIAL_HEADER.END_COMUNICATE
        Dim uidString As String = Nothing

        ' Ciclo finché non ricevi una risposta valida o il timeout scade
        Do
            ' Invia il comando GET_UID_BADGE
            Await SendCommand(SERIAL_HEADER.GET_UID_BADGE)

            ' Attendi la risposta
            Dim responseBuffer(255) As Byte
            Dim bytesRead As Integer = 0

            Dim timeout As DateTime = DateTime.Now.AddMilliseconds(5000) ' Timeout di 5 secondi

            While DateTime.Now < timeout
                ' Attendere finché non si verifica uno dei seguenti casi:
                ' 1. La risposta contiene l'header atteso (GET_UID_BADGE).
                ' 2. La risposta contiene l'end footer atteso (END_COMUNICATE).
                ' 3. Il timeout scade.

                If dataBuffer.Count > 0 Then
                    Dim receivedByte As Byte
                    If dataBuffer.TryDequeue(receivedByte) Then
                        ' Aggiungi il byte letto al buffer della risposta
                        responseBuffer(bytesRead) = receivedByte
                        bytesRead += 1

                        ' Se abbiamo letto l'end footer atteso, esci dal ciclo principale
                        If receivedByte = expectedFooter Then
                            Exit While
                        End If
                    End If
                End If
            End While

            ' Verifica se abbiamo ricevuto una risposta valida
            If bytesRead >= 3 AndAlso responseBuffer(0) = expectedHeader AndAlso responseBuffer(bytesRead - 1) = expectedFooter Then
                ' Estrai la lunghezza dell'UID dalla risposta
                Dim uidLength As Integer = responseBuffer(1)

                ' Estrai l'UID dalla risposta
                Dim uidBytes(uidLength - 1) As Byte
                Array.Copy(responseBuffer, 2, uidBytes, 0, uidLength)

                ' Formatta l'UID in una stringa con lo spazio vuoto finale
                uidString = "&H" & BitConverter.ToString(uidBytes).Replace("-", " &H").ToLower()
            End If

            ' Ripeti l'invio del comando solo se il timeout è scaduto o la risposta non è valida
        Loop While String.IsNullOrEmpty(uidString)

        ' Restituisci l'UID formattato
        Return uidString & " "
    End Function

    Public Async Function writeBadge(ByVal lcd_lin1() As Byte, ByVal lcd_lin2() As Byte, ByVal sector_id As Byte, ByVal brendSector() As Byte) As Task(Of SERIAL_HEADER)
        ' Verifica se le lunghezze degli array sono corrette
        If lcd_lin1.Length <> 16 OrElse lcd_lin2.Length <> 16 OrElse brendSector.Length <> 16 Then
            Return False ' Lunghezze non valide
        End If

        ' Invia il comando WRITE_BADGE seguito dai dati
        Dim command() As Byte = {SERIAL_HEADER.WRITE_BADGE, sector_id}
        Dim payload() As Byte = New Byte(47) {} ' 48 byte di payload (16 + 16 + 16)
        brendSector.CopyTo(payload, 0)
        lcd_lin1.CopyTo(payload, 16)
        lcd_lin2.CopyTo(payload, 32)

        command = command.Concat(New Byte() {CByte(payload.Length)}).ToArray() ' Aggiungi la lunghezza del payload
        command = command.Concat(payload).ToArray() ' Aggiungi il payload

        Await SendCommand(command)

        ' Attendi la risposta (SUCCESS_WRITE_BADGE o ERROR_WRITE_BADGE seguito da END_COMUNICATE)
        Dim timeout As DateTime = DateTime.Now.AddMilliseconds(5000) ' Timeout di 5 secondi
        Dim responseBuffer(1) As Byte
        Dim bytesRead As Integer = 0

        While DateTime.Now < timeout AndAlso bytesRead < 2
            If dataBuffer.TryDequeue(responseBuffer(bytesRead)) Then
                bytesRead += 1
            End If
        End While

        '' Verifica la risposta ricevuta
        'If bytesRead = 2 AndAlso responseBuffer(0) = SERIAL_HEADER.SUCCESS_WRITE_BADGE AndAlso responseBuffer(1) = SERIAL_HEADER.END_COMUNICATE Then
        '    Return True ' Scrittura del badge avvenuta con successo
        'ElseIf bytesRead = 2 AndAlso responseBuffer(0) = SERIAL_HEADER.ERROR_WRITE_BADGE AndAlso responseBuffer(1) = SERIAL_HEADER.END_COMUNICATE Then
        '    Return False ' Errore nella scrittura del badge
        'End If
        'Return False ' Risposta non valida o timeout
        If bytesRead = 2 Then Return responseBuffer(0)
        Return SERIAL_HEADER.ERROR_TIMEOUT
    End Function

    Private Sub HW_reset()
        _serial.RtsEnable = True
        Thread.Sleep(350)
        _serial.RtsEnable = False
    End Sub
End Class
