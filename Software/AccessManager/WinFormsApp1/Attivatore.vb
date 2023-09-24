Public Class Attivatore
    Public Event add_access(ByVal name As String, ByVal uid As String, ByVal time As String)
    Private Sub Attivatore_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(860, 227)
        LCD_line1.Text = ""
        LCD_line2.Text = ""
        LCD_line1.UseWaitCursor = False
        LCD_line2.UseWaitCursor = False
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Me.Size = New Size(860, 433)
        Else
            Me.Size = New Size(860, 227)
        End If
    End Sub

    Private Sub LCD_line1_TextChanged(sender As Object, e As EventArgs) Handles LCD_line1.TextChanged
        If LcdClear.Enabled Then LcdClear.Stop()
        LcdClear.Start()
        If EventHandler.Enabled Then EventHandler.Stop()
        EventHandler.Start()
    End Sub

    Private Sub LCD_line2_TextChanged(sender As Object, e As EventArgs) Handles LCD_line2.TextChanged
        If LcdClear.Enabled Then LcdClear.Stop()
        LcdClear.Start()
        If EventHandler.Enabled Then EventHandler.Stop()
        EventHandler.Start()
    End Sub

    Private Sub LcdClear_Tick(sender As Object, e As EventArgs) Handles LcdClear.Tick
        LCD_line1.Text = ""
        LCD_line2.Text = ""
        LcdClear.Stop()
    End Sub

    Private Sub EventHandler_Tick(sender As Object, e As EventArgs) Handles EventHandler.Tick
        If LCD_line2.Text.Contains("Ciao") Then

            RaiseEvent add_access(LCD_line1.Text, CorrectionHEXValues(CardUID.Text), DateTime.Now.ToString(DataTimeFormat))
            Threading.Thread.Sleep(2)
        End If
        EventHandler.Stop()

    End Sub

End Class