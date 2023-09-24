Imports System.ComponentModel

Public Class BrokerPassword
    Dim ok_clicked As Boolean = False
    Public password As String = ""
    ' TODO: inserire il codice per l'esecuzione dell'autenticazione personalizzata tramite il nome utente e la password forniti 
    ' Vedere https://go.microsoft.com/fwlink/?LinkId=35339.  
    ' L'entità personalizzata può essere quindi collegata all'entità del thread corrente nel modo seguente: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' dove CustomPrincipal è l'implementazione di IPrincipal utilizzata per eseguire l'autenticazione. 
    ' My.User restituirà quindi informazioni sull'identità incapsulate nell'oggetto CustomPrincipal,
    ' quali il nome utente, il nome visualizzato e così via.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        password = Me.PasswordTextBox.Text
        ok_clicked = True

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub BrokerPassword_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not ok_clicked Then Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class
