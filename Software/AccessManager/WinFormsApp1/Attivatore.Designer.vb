<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Attivatore
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LCD_line1 = New System.Windows.Forms.TextBox()
        Me.LCD_line2 = New System.Windows.Forms.TextBox()
        Me.Led1 = New System.Windows.Forms.PictureBox()
        Me.Led2 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.stRele = New System.Windows.Forms.PictureBox()
        Me.stInput = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.stTamper = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.uptime = New System.Windows.Forms.Label()
        Me.versioneFW = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.uptimereference = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CardBlock1 = New System.Windows.Forms.TextBox()
        Me.CardBlock0 = New System.Windows.Forms.TextBox()
        Me.CardUID = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CardBlock2 = New System.Windows.Forms.TextBox()
        Me.LcdClear = New System.Windows.Forms.Timer(Me.components)
        Me.EventHandler = New System.Windows.Forms.Timer(Me.components)
        CType(Me.Led1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Led2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stRele, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.stTamper, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'LCD_line1
        '
        Me.LCD_line1.BackColor = System.Drawing.Color.Green
        Me.LCD_line1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LCD_line1.Cursor = System.Windows.Forms.Cursors.No
        Me.LCD_line1.Font = New System.Drawing.Font("Segoe UI", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LCD_line1.Location = New System.Drawing.Point(15, 17)
        Me.LCD_line1.Margin = New System.Windows.Forms.Padding(4)
        Me.LCD_line1.Name = "LCD_line1"
        Me.LCD_line1.ReadOnly = True
        Me.LCD_line1.Size = New System.Drawing.Size(824, 50)
        Me.LCD_line1.TabIndex = 0
        Me.LCD_line1.Text = "Line1"
        Me.LCD_line1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LCD_line2
        '
        Me.LCD_line2.BackColor = System.Drawing.Color.Green
        Me.LCD_line2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LCD_line2.Cursor = System.Windows.Forms.Cursors.No
        Me.LCD_line2.Font = New System.Drawing.Font("Segoe UI", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LCD_line2.Location = New System.Drawing.Point(15, 67)
        Me.LCD_line2.Margin = New System.Windows.Forms.Padding(4)
        Me.LCD_line2.Name = "LCD_line2"
        Me.LCD_line2.ReadOnly = True
        Me.LCD_line2.Size = New System.Drawing.Size(824, 50)
        Me.LCD_line2.TabIndex = 1
        Me.LCD_line2.Text = "Line2"
        Me.LCD_line2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Led1
        '
        Me.Led1.Image = Global.WinFormsApp1.My.Resources.Resources.led_off
        Me.Led1.Location = New System.Drawing.Point(753, 125)
        Me.Led1.Margin = New System.Windows.Forms.Padding(4)
        Me.Led1.Name = "Led1"
        Me.Led1.Size = New System.Drawing.Size(39, 39)
        Me.Led1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Led1.TabIndex = 2
        Me.Led1.TabStop = False
        '
        'Led2
        '
        Me.Led2.Image = Global.WinFormsApp1.My.Resources.Resources.led_off
        Me.Led2.Location = New System.Drawing.Point(800, 125)
        Me.Led2.Margin = New System.Windows.Forms.Padding(4)
        Me.Led2.Name = "Led2"
        Me.Led2.Size = New System.Drawing.Size(39, 39)
        Me.Led2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Led2.TabIndex = 3
        Me.Led2.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 134)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Stato Rele' : "
        '
        'stRele
        '
        Me.stRele.Image = Global.WinFormsApp1.My.Resources.Resources.led_off
        Me.stRele.Location = New System.Drawing.Point(104, 125)
        Me.stRele.Margin = New System.Windows.Forms.Padding(4)
        Me.stRele.Name = "stRele"
        Me.stRele.Size = New System.Drawing.Size(39, 39)
        Me.stRele.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.stRele.TabIndex = 5
        Me.stRele.TabStop = False
        '
        'stInput
        '
        Me.stInput.Image = Global.WinFormsApp1.My.Resources.Resources.led_off
        Me.stInput.Location = New System.Drawing.Point(269, 125)
        Me.stInput.Margin = New System.Windows.Forms.Padding(4)
        Me.stInput.Name = "stInput"
        Me.stInput.Size = New System.Drawing.Size(39, 39)
        Me.stInput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.stInput.TabIndex = 7
        Me.stInput.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(174, 134)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Stato Input  : "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.stTamper)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.uptime)
        Me.GroupBox1.Controls.Add(Me.versioneFW)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 198)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(824, 53)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Info"
        '
        'stTamper
        '
        Me.stTamper.Image = Global.WinFormsApp1.My.Resources.Resources.led_off
        Me.stTamper.Location = New System.Drawing.Point(630, 26)
        Me.stTamper.Margin = New System.Windows.Forms.Padding(4)
        Me.stTamper.Name = "stTamper"
        Me.stTamper.Size = New System.Drawing.Size(18, 21)
        Me.stTamper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.stTamper.TabIndex = 11
        Me.stTamper.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(522, 23)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(111, 21)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Stato Tamper : "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(91, 23)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 21)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Vers. FW : "
        '
        'uptime
        '
        Me.uptime.AutoSize = True
        Me.uptime.BackColor = System.Drawing.Color.Lime
        Me.uptime.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.uptime.Location = New System.Drawing.Point(388, 23)
        Me.uptime.Name = "uptime"
        Me.uptime.Size = New System.Drawing.Size(19, 21)
        Me.uptime.TabIndex = 14
        Me.uptime.Text = "0"
        '
        'versioneFW
        '
        Me.versioneFW.AutoSize = True
        Me.versioneFW.BackColor = System.Drawing.Color.Lime
        Me.versioneFW.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.versioneFW.Location = New System.Drawing.Point(179, 23)
        Me.versioneFW.Name = "versioneFW"
        Me.versioneFW.Size = New System.Drawing.Size(53, 21)
        Me.versioneFW.TabIndex = 12
        Me.versioneFW.Text = "1.0BV"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.uptimereference)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.CardBlock1)
        Me.GroupBox2.Controls.Add(Me.CardBlock0)
        Me.GroupBox2.Controls.Add(Me.CardUID)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 246)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Size = New System.Drawing.Size(824, 160)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Badge"
        '
        'uptimereference
        '
        Me.uptimereference.AutoSize = True
        Me.uptimereference.BackColor = System.Drawing.Color.Lime
        Me.uptimereference.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.uptimereference.Location = New System.Drawing.Point(665, 26)
        Me.uptimereference.Name = "uptimereference"
        Me.uptimereference.Size = New System.Drawing.Size(19, 21)
        Me.uptimereference.TabIndex = 15
        Me.uptimereference.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 123)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 21)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Block 2 : "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 91)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 21)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Block 1 : "
        '
        'CardBlock1
        '
        Me.CardBlock1.Location = New System.Drawing.Point(78, 88)
        Me.CardBlock1.Multiline = True
        Me.CardBlock1.Name = "CardBlock1"
        Me.CardBlock1.ReadOnly = True
        Me.CardBlock1.Size = New System.Drawing.Size(741, 26)
        Me.CardBlock1.TabIndex = 4
        Me.CardBlock1.Text = "00000000"
        Me.CardBlock1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CardBlock0
        '
        Me.CardBlock0.Location = New System.Drawing.Point(78, 56)
        Me.CardBlock0.Multiline = True
        Me.CardBlock0.Name = "CardBlock0"
        Me.CardBlock0.ReadOnly = True
        Me.CardBlock0.Size = New System.Drawing.Size(739, 26)
        Me.CardBlock0.TabIndex = 3
        Me.CardBlock0.Text = "00000000"
        Me.CardBlock0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CardUID
        '
        Me.CardUID.Location = New System.Drawing.Point(78, 23)
        Me.CardUID.Name = "CardUID"
        Me.CardUID.ReadOnly = True
        Me.CardUID.Size = New System.Drawing.Size(581, 29)
        Me.CardUID.TabIndex = 2
        Me.CardUID.Text = "00000000"
        Me.CardUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 21)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Block 0 : "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 21)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "UID : "
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 171)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(156, 25)
        Me.CheckBox1.TabIndex = 10
        Me.CheckBox1.Text = "Mostra info badge"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CardBlock2
        '
        Me.CardBlock2.Location = New System.Drawing.Point(91, 366)
        Me.CardBlock2.Multiline = True
        Me.CardBlock2.Name = "CardBlock2"
        Me.CardBlock2.ReadOnly = True
        Me.CardBlock2.Size = New System.Drawing.Size(741, 26)
        Me.CardBlock2.TabIndex = 5
        Me.CardBlock2.Text = "00000000"
        Me.CardBlock2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LcdClear
        '
        Me.LcdClear.Interval = 5000
        '
        'EventHandler
        '
        Me.EventHandler.Interval = 1000
        '
        'Attivatore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(844, 409)
        Me.ControlBox = False
        Me.Controls.Add(Me.CardBlock2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.stInput)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.stRele)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Led2)
        Me.Controls.Add(Me.Led1)
        Me.Controls.Add(Me.LCD_line2)
        Me.Controls.Add(Me.LCD_line1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Attivatore"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Lettore Badge"
        CType(Me.Led1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Led2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stRele, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stInput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.stTamper, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents CheckBox1 As CheckBox
    Public WithEvents uptime As Label
    Public WithEvents versioneFW As Label
    Public WithEvents LCD_line1 As TextBox
    Public WithEvents LCD_line2 As TextBox
    Public WithEvents Led1 As PictureBox
    Public WithEvents Led2 As PictureBox
    Public WithEvents stRele As PictureBox
    Public WithEvents stInput As PictureBox
    Public WithEvents CardBlock0 As TextBox
    Public WithEvents CardUID As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Public WithEvents CardBlock1 As TextBox
    Public WithEvents CardBlock2 As TextBox
    Public WithEvents uptimereference As Label
    Friend WithEvents LcdClear As Timer
    Public WithEvents stTamper As PictureBox
    Friend WithEvents Label8 As Label
    Friend WithEvents EventHandler As Timer
End Class
