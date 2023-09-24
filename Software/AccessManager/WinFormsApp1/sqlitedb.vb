Imports System.Data.Common
Imports System.IO
Imports System.Threading
Imports System.Transactions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Data.Sqlite


Public Class sqlitedb
    Private database_name As String
    Private db_path As String = "/"
    Private db_connection As SqliteConnection

    Sub New(ByVal path As String, Optional ByVal name_db As String = "def_db.db")
        database_name = name_db
        db_path = path
    End Sub

    Public Async Function StartDbAsync() As Task
        Dim dbpath As String = Path.Combine(db_path, database_name)
        Dim newCreation As Boolean = False
        If Not File.Exists(dbpath) Then
            Try
                File.WriteAllBytes(dbpath, My.Resources.empity_db)
                newCreation = True
            Catch ex As Exception
                MessageBox.Show("Impossibile creare il database. Contattare l'assistenza.")
                MYlogManager.LogMessage("Impossibile creare il database." & vbCrLf & ex.Message, "Error-DB")
                Exit Function
            End Try
        End If

        Try
            db_connection = New SqliteConnection($"Filename={dbpath}")
            Await db_connection.OpenAsync()
            If newCreation Then
                Await PopulateNewDb()
            End If
        Catch ex As Exception
            MessageBox.Show("Impossibile aprire la connessione al database. Contattare l'assistenza.")
            MYlogManager.LogMessage("Impossibile creare il database." & vbCrLf & ex.Message, "Error-DB")
            If File.Exists(dbpath) Then
                Try
                    File.Delete(dbpath)
                Catch
                End Try
            End If
            Exit Function
        End Try
    End Function
    Private Async Function PopulateNewDb() As Task
        Try
            Using transaction As SqliteTransaction = db_connection.BeginTransaction()
                Await CreateTableIfNotExists("students", "id INTEGER PRIMARY KEY, name TEXT NOT NULL, gender TEXT NOT NULL, school TEXT NOT NULL, bday DATETIME NOT NULL, picPath TEXT NOT NULL, regDate DATETIME NOT NULL, badge TEXT NOT NULL")
                Await CreateTableIfNotExists("login", "id INTEGER PRIMARY KEY, name TEXT NOT NULL, school TEXT NOT NULL, date DATETIME NOT NULL, badge TEXT NOT NULL, uid TEXT NOT NULL")
                transaction.Commit()
            End Using
        Catch ex As Exception
            MYlogManager.LogMessage("Impossibile creare le tabelle nel database." & vbCrLf & ex.Message, "Error-DB")
            MessageBox.Show("Impossibile creare le tabelle nel database. Contattare l'assistenza.")
        End Try
    End Function

    Private Async Function CreateTableIfNotExists(tableName As String, tableSchema As String) As Task
        Dim db_command As SqliteCommand = db_connection.CreateCommand()
        db_command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} ({tableSchema});"
        Await db_command.ExecuteNonQueryAsync()
    End Function

    Public Async Function AddNewUser(userData As UserData) As Task
        Try
            db_connection.Open()

            Dim query As String = "INSERT INTO students (name, gender, school, bday, picPath, regDate, badge) " &
                                  "VALUES (@Name, @Gender, @School, @Bday, @PicPath, @RegDate, @Badge)"

            Using transaction As SqliteTransaction = db_connection.BeginTransaction()
                Using command As New SqliteCommand(query, db_connection, transaction)
                    command.Parameters.AddWithValue("@Name", userData.Name)
                    command.Parameters.AddWithValue("@Gender", userData.Gender)
                    command.Parameters.AddWithValue("@School", userData.School)
                    command.Parameters.AddWithValue("@Bday", userData.Birthday)
                    command.Parameters.AddWithValue("@PicPath", userData.PicPath)
                    command.Parameters.AddWithValue("@RegDate", userData.RegDate)
                    command.Parameters.AddWithValue("@Badge", userData.Badge)

                    Await command.ExecuteNonQueryAsync()
                End Using
                transaction.Commit()
            End Using
        Catch ex As Exception
            MYlogManager.LogMessage("Impossibile inserire i dati del nuovo utente nel database." & vbCrLf & ex.Message, "Error-DB")
            MessageBox.Show("Impossibile inserire i dati del nuovo utente nel database. Contattare l'assistenza.")
        End Try

    End Function


    Dim lockObject As String = "ciao"
    Public Async Function AddNewLoginAsync(ByVal name As String, ByVal data As String, ByVal badge As String, ByVal school As String, ByVal uid As String) As Task
        Try
            Await Task.Run(Sub()
                               SyncLock lockObject
                                   Using transaction As SqliteTransaction = db_connection.BeginTransaction()
                                       Dim sql As String = "INSERT INTO login (name, school, date, badge, uid) VALUES (@name, @school, @date, @badge, @uid);"

                                       Using db_command As New SqliteCommand(sql, db_connection, transaction)
                                           db_command.Parameters.AddWithValue("@name", name)
                                           db_command.Parameters.AddWithValue("@school", school)
                                           db_command.Parameters.AddWithValue("@date", data)
                                           db_command.Parameters.AddWithValue("@badge", badge)
                                           db_command.Parameters.AddWithValue("@uid", uid)

                                           db_command.ExecuteNonQuery()
                                       End Using

                                       transaction.Commit()
                                   End Using
                               End SyncLock
                           End Sub)
        Catch ex As Exception
            MYlogManager.LogMessage("Impossibile inserire i dati nel database." & vbCrLf & ex.Message, "Error-DB")
            MessageBox.Show("Impossibile inserire i dati nel database. Contattare l'assistenza.")
        End Try
    End Function
    Public Async Function ExtractUserByBadgeAsync(ByVal badge As String) As Task(Of List(Of UserData))
        Try

            Dim sql As String = "SELECT * FROM students WHERE badge = @badge;"
            Dim parameters As New List(Of SqliteParameter) From {
            New SqliteParameter("@badge", badge)
}

            Return Await ExecuteUserDataQueryAsync(sql, parameters)
        Catch ex As Exception
            MYlogManager.LogMessage("Errore Estrazione utente by badge (badge = " & badge & ")." & vbCrLf & ex.Message, "Allert-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try


    End Function

    Private Async Function ExecuteUserDataQueryAsync(ByVal sql As String, ByVal parameters As List(Of SqliteParameter)) As Task(Of List(Of UserData))
        Dim output As New List(Of UserData)()

        Try

            Using dbCommand As New SqliteCommand(sql, db_connection)
                For Each param In parameters
                    dbCommand.Parameters.Add(param)
                Next

                Using reader As SqliteDataReader = Await dbCommand.ExecuteReaderAsync()
                    While reader.Read()
                        Dim dataObject As New UserData()

                        dataObject.Id = reader.GetInt32(reader.GetOrdinal("id"))
                        dataObject.Name = reader.GetString(reader.GetOrdinal("name"))
                        dataObject.Gender = reader.GetString(reader.GetOrdinal("gender"))
                        dataObject.School = reader.GetString(reader.GetOrdinal("school"))
                        dataObject.Birthday = reader.GetDateTime(reader.GetOrdinal("bday"))
                        dataObject.PicPath = reader.GetString(reader.GetOrdinal("picPath"))
                        dataObject.RegDate = reader.GetDateTime(reader.GetOrdinal("regDate"))
                        dataObject.Badge = reader.GetString(reader.GetOrdinal("badge"))

                        output.Add(dataObject)
                    End While
                End Using
            End Using
            Return output
        Catch ex As Exception
            MYlogManager.LogMessage("Errore query " & sql & vbCrLf & parameters.ToString & "." & vbCrLf & ex.Message, "Allert-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try


    End Function
    Public Async Function ExtractLogsFromDateAsync(ByVal startDate As DateTime, ByVal endDate As DateTime, Optional ByVal uid As String = "") As Task(Of List(Of LogEntry))
        Try

            Dim logs As New List(Of LogEntry)()
            Dim formattedStartDate As String = startDate.ToString(DataTimeFormat)
            Dim formattedEndDate As String = endDate.ToString(DataTimeFormat)

            Dim sql As String
            Dim parameters As New List(Of SqliteParameter) From {
            New SqliteParameter("@startDate", formattedStartDate),
            New SqliteParameter("@endDate", formattedEndDate)
        }

            If String.IsNullOrWhiteSpace(uid) Then
                ' Query per estrarre tutti i log nell'intervallo specificato

                sql = "SELECT * FROM login WHERE date >= @startDate AND date <= @endDate;"
            Else
                ' Query per estrarre i log di un utente specifico nell'intervallo specificato
                sql = "SELECT * FROM login WHERE date >= @startDate AND date <= @endDate AND uid = @uid;"
                parameters.Add(New SqliteParameter("@uid", uid))
            End If

            Dim output As List(Of LogEntry) = Await ExecuteLogQueryAsync(sql, parameters)
            Return output
        Catch ex As Exception
            MYlogManager.LogMessage("Errore Extract Logs." & vbCrLf & ex.Message, "Error-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try


    End Function


    Private Async Function ExecuteLogQueryAsync(ByVal sql As String, ByVal parameters As List(Of SqliteParameter)) As Task(Of List(Of LogEntry))
        Dim logs As New List(Of LogEntry)()

        Try

            Using dbCommand As New SqliteCommand(sql, db_connection)
                For Each param In parameters
                    dbCommand.Parameters.Add(param)
                Next

                Using reader As SqliteDataReader = Await dbCommand.ExecuteReaderAsync()
                    While reader.Read()
                        Dim log As New LogEntry()

                        log.name = reader.GetString(reader.GetOrdinal("name"))
                        log.scuola = reader.GetString(reader.GetOrdinal("school"))
                        log.data = reader.GetDateTime(reader.GetOrdinal("date"))
                        log.UID_badge = reader.GetString(reader.GetOrdinal("badge"))
                        log.user_Id = reader.GetInt32(reader.GetOrdinal("uid"))


                        logs.Add(log)
                    End While
                End Using
            End Using

            Return logs
        Catch ex As Exception
            MYlogManager.LogMessage("Errore query " & sql & vbCrLf & parameters.ToString & "." & vbCrLf & ex.Message, "Allert-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try

    End Function
    Public Async Function EstraiUtentiDaDatabase() As Task(Of List(Of UserData))
        Try

            Dim utenti As New List(Of UserData)

            Dim sql As String = "SELECT * FROM students ORDER BY school, name"
            Dim parameters As New List(Of SqliteParameter)()

            Dim output As List(Of UserData) = Await ExecuteUserQueryAsync(sql, parameters)
            Return output
        Catch ex As Exception
            MYlogManager.LogMessage("Errore Estrazione Utenti." & vbCrLf & ex.Message, "Error-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try


    End Function

    Private Async Function ExecuteUserQueryAsync(ByVal sql As String, ByVal parameters As List(Of SqliteParameter)) As Task(Of List(Of UserData))
        Dim utenti As New List(Of UserData)()

        Try

            Using dbCommand As New SqliteCommand(sql, db_connection)
                For Each param In parameters
                    dbCommand.Parameters.Add(param)
                Next

                Using reader As SqliteDataReader = Await dbCommand.ExecuteReaderAsync()
                    While reader.Read()
                        Dim dataObject As New UserData()

                        dataObject.Id = reader.GetInt32(reader.GetOrdinal("id"))
                        dataObject.Name = reader.GetString(reader.GetOrdinal("name"))
                        dataObject.Gender = reader.GetString(reader.GetOrdinal("gender"))
                        dataObject.School = reader.GetString(reader.GetOrdinal("school"))
                        dataObject.Birthday = reader.GetDateTime(reader.GetOrdinal("bday"))
                        dataObject.PicPath = reader.GetString(reader.GetOrdinal("picPath"))
                        dataObject.RegDate = reader.GetDateTime(reader.GetOrdinal("regDate"))
                        dataObject.Badge = reader.GetString(reader.GetOrdinal("badge"))



                        utenti.Add(dataObject)
                    End While
                End Using
            End Using

            Return utenti
        Catch ex As Exception
            MYlogManager.LogMessage("Errore query " & sql & vbCrLf & parameters.ToString & "." & vbCrLf & ex.Message, "Allert-DB")
            ' Gestire l'errore in modo appropriato, ad esempio, loggandolo o mostrando un messaggio d'errore.
            Return Nothing
        End Try


    End Function
    Public Async Function UpdateStudentDataAsync(userData As UserData) As Task
        Using dbTransaction As SqliteTransaction = db_connection.BeginTransaction()
            Try

                Dim sql As String = "UPDATE students SET name = @name, gender = @gender, school = @school, bday = @birthday, picPath = @picPath, regDate = @regDate, badge = @badge WHERE id = @id"

                Using dbCommand As New SqliteCommand(sql, db_connection, dbTransaction)
                    dbCommand.Parameters.AddWithValue("@name", userData.Name)
                    dbCommand.Parameters.AddWithValue("@gender", userData.Gender)
                    dbCommand.Parameters.AddWithValue("@school", userData.School)
                    dbCommand.Parameters.AddWithValue("@birthday", userData.Birthday)
                    dbCommand.Parameters.AddWithValue("@picPath", userData.PicPath)
                    dbCommand.Parameters.AddWithValue("@regDate", userData.RegDate)
                    dbCommand.Parameters.AddWithValue("@badge", userData.Badge)
                    dbCommand.Parameters.AddWithValue("@id", userData.Id)

                    Await dbCommand.ExecuteNonQueryAsync()
                End Using

                dbTransaction.Commit()
            Catch ex As Exception
                dbTransaction.Rollback()
                MYlogManager.LogMessage("Errore Fallita Aggiornamento dati utente sul db, " & userData.Name & " " & userData.Id & vbCrLf & ex.Message, "Allert-DB")
                Throw ' Gestione dell'eccezione a tua discrezione
            End Try


        End Using
    End Function

    Public Async Function DeleteStudentDataAsync(userData As UserData) As Task
        Using dbTransaction As SqliteTransaction = db_connection.BeginTransaction()
            Try
                Dim sql As String = "DELETE FROM students WHERE id = @id"

                Using dbCommand As New SqliteCommand(sql, db_connection, dbTransaction)
                    dbCommand.Parameters.AddWithValue("@id", userData.Id)

                    Await dbCommand.ExecuteNonQueryAsync()
                End Using

                dbTransaction.Commit()
            Catch ex As Exception
                dbTransaction.Rollback()
                MYlogManager.LogMessage("Errore Fallita eliminazione utente da db, " & userData.Name & " " & userData.Id & vbCrLf & ex.Message, "Allert-DB")
                Throw ' Gestione dell'eccezione a tua discrezione
            End Try
        End Using
    End Function
End Class
