Imports System.IO
Imports System.IO.Compression

Friend Module Program

    <STAThread()>
    Friend Sub Main(args As String())
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException

#Region "Genera la cartella CONF con dati standard"
        If Not IO.Directory.Exists(Path.Combine(Path_Progetto, "conf")) Then
            ' Directory di origine da comprimere
            Dim sourceDirectory As String = Path_Progetto
            ' Nome del file ZIP di destinazione
            Dim zipFileName As String = Path.Combine(Path_Progetto, "conf_std.zip")

            ' Estrai il contenuto del file ZIP
            ZipFile.ExtractToDirectory(zipFileName, sourceDirectory)

        End If

#End Region



        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New main)
    End Sub


    Private Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
        ' Gestisci l'eccezione qui
        Dim exception As Exception = e.Exception
        ' Registra l'eccezione come errore su un file di log specifico
        MYlogManager.LogMessage("Eccezione nell'applicazione: " & exception.Message & vbCrLf & vbCrLf & vbCrLf & "StackTrace: " & exception.StackTrace, "ThreadException")

    End Sub

    Private Sub CurrentDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        ' Gestisci l'eccezione qui
        Dim exception As Exception = DirectCast(e.ExceptionObject, Exception)
        ' Registra l'eccezione come errore su un file di log specifico
        MYlogManager.LogMessage("Eccezione non gestita nell'applicazione: " & exception.Message & vbCrLf & vbCrLf & vbCrLf & "StackTrace: " & exception.StackTrace, "UnhandledException")
    End Sub

End Module
