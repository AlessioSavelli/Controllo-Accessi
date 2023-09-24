
Imports MQTTnet
Imports System.Threading
Imports MQTTnet.Client
Imports SQLitePCL

Namespace MQTTnet.Samples.Client
    Public Class ClientMQTT

        Dim MQTT_SERVER As String = "127.0.0.1"
        Dim MQTT_PORT As Int16 = 8883
        Dim TIMEOUT_CONNECTION As Int16 = 0
        Dim PACKET_FRAGMENTATION As Boolean = True

        Dim mqttClientOptions As New MqttClientOptions
        Dim mqttClient As IMqttClient

        Public Event Recive_mqttMessage(ByVal e)

        Public OUTPUT As String = ""

        Public Sub New()
            Dim mqttFactory = New MqttFactory()
            mqttClient = mqttFactory.CreateMqttClient()
            AddHandler mqttClient.ApplicationMessageReceivedAsync, AddressOf raise_recive_event
        End Sub


        Public Sub New(ByRef mqtt_server, Optional ByRef mqtt_port = 8883, Optional ByRef PacketFragmentation = True, Optional ByRef timeout = 0)
            Me.MQTT_SERVER = mqtt_server
            Me.MQTT_PORT = mqtt_port
            Me.TIMEOUT_CONNECTION = TIMEOUT_CONNECTION
            Me.PACKET_FRAGMENTATION = PACKET_FRAGMENTATION
            Dim mqttFactory = New MqttFactory()
            mqttClient = mqttFactory.CreateMqttClient()
            AddHandler mqttClient.ApplicationMessageReceivedAsync, AddressOf raise_recive_event
        End Sub
        Public Async Function Clean_Disconnect() As Task

            Dim mqttClientOptions = New MqttClientOptionsBuilder().WithTcpServer(MQTT_SERVER).Build()
            Await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None)
            Await mqttClient.DisconnectAsync(New MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build())

        End Function

        Public Async Function Connect_Client(Optional ByVal User As String = Nothing, Optional ByVal pass As String = Nothing) As Task


            Dim mqttClientBuilder As New MqttClientOptionsBuilder

            If PACKET_FRAGMENTATION Then
                mqttClientOptions = mqttClientBuilder.WithTcpServer(MQTT_SERVER, MQTT_PORT).Build()

            Else
                mqttClientOptions = mqttClientBuilder.WithTcpServer(MQTT_SERVER, MQTT_PORT).WithoutPacketFragmentation().Build()
            End If

            If pass IsNot Nothing Then
                mqttClientOptions.Credentials = New MqttClientCredentials(User, System.Text.Encoding.ASCII.GetBytes(pass))
            ElseIf User IsNot Nothing Then
                mqttClientOptions.Credentials = New MqttClientCredentials(User)
            End If

            If (TIMEOUT_CONNECTION > 0) Then
                Try

                    Using timeoutToken = New CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT_CONNECTION))
                        Await mqttClient.ConnectAsync(mqttClientOptions, timeoutToken.Token)
                    End Using
                    OUTPUT = "Server Connesso"
                Catch
                    Console.WriteLine("Timeout while connecting.")
                    OUTPUT = "Server timeout"

                End Try

            Else

                Try
                    Dim response = Await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None)
                    Console.WriteLine("The MQTT client is connected.")
                    OUTPUT = "Server Connesso"

                Catch ex As Exception
                    OUTPUT = "Errore di connessione"
                    MYlogManager.LogMessage("Errore di connessione MQTT." & ex.Message, "Connect_client-MQTT")
                End Try

                'Dim mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build()
                'Await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None)

            End If




        End Function
        Private Function raise_recive_event(ByVal e)
            RaiseEvent Recive_mqttMessage(e)
            Return True
        End Function

        Public Async Function Disconnect_Clean() As Task

            Dim mqttClientOptions = New MqttClientOptionsBuilder().WithTcpServer(MQTT_SERVER).Build()
            Await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None)
            Await mqttClient.DisconnectAsync(MqttClientDisconnectOptionsReason.ImplementationSpecificError)

            OUTPUT = "Server Disconnesso &H00"
        End Function

        Public Async Function Disconnect_Non_Clean() As Task
            Dim mqttClientOptions = New MqttClientOptionsBuilder().WithTcpServer(MQTT_SERVER).Build()
            Await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None)
            mqttClient.Dispose()
            OUTPUT = "Server Disconnesso &H01"
        End Function



        Public Async Function Ping_Server() As Task
            Dim mqttClientOptions = New MqttClientOptionsBuilder().WithTcpServer(MQTT_SERVER).Build()
            Await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None)
            Await mqttClient.PingAsync(CancellationToken.None)
            Console.WriteLine("The MQTT server replied to the ping request.")
            OUTPUT = "Ping Server OK!"

        End Function

        Public Async Function Topic_Subscribe(ByVal topic) As Task
            Await mqttClient.SubscribeAsync(New MqttTopicFilterBuilder().WithTopic(topic).Build())
            Console.WriteLine("### SUBSCRIBED TO " & topic & " ###")
        End Function

        Public Async Function Publish_topic(ByVal topic As String, ByVal message As String, Optional retain As Boolean = False) As Task

            Dim _message As New MqttApplicationMessageBuilder
            _message.WithTopic(topic)
            _message.WithPayload(message)
            _message.WithRetainFlag(retain)

            Await mqttClient.PublishAsync(_message.Build(), CancellationToken.None)
        End Function



    End Class
End Namespace


