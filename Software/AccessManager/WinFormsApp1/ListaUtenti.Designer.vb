<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ListaUtenti

    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.UserList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader()
        Me.AzioniUtente = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StampaBadgeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AggiornaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.EliminaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BadgeChanger = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ModifyButton = New System.Windows.Forms.Button()
        Me.ChangePic = New System.Windows.Forms.Button()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.BoxEta = New System.Windows.Forms.MaskedTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BoxScuola = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BoxNome = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.NrPresenzeText = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.NrLogsText = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LoadLogs = New System.Windows.Forms.Button()
        Me.ViewAllLogs = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.EndData = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.StartData = New System.Windows.Forms.DateTimePicker()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LogsPresenze = New System.Windows.Forms.TreeView()
        Me.ModifyUser = New System.Windows.Forms.CheckBox()
        Me.SqliteCommand1 = New Microsoft.Data.Sqlite.SqliteCommand()
        Me.ReWriteBadge = New System.Windows.Forms.Panel()
        Me.AzioniUtente.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'UserList
        '
        Me.UserList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.UserList.ContextMenuStrip = Me.AzioniUtente
        Me.UserList.FullRowSelect = True
        Me.UserList.GridLines = True
        Me.UserList.HideSelection = False
        Me.UserList.Location = New System.Drawing.Point(6, 12)
        Me.UserList.Name = "UserList"
        Me.UserList.Size = New System.Drawing.Size(1125, 270)
        Me.UserList.TabIndex = 30
        Me.UserList.UseCompatibleStateImageBehavior = False
        Me.UserList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Nome"
        Me.ColumnHeader1.Width = 180
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Sesso"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Compleanno"
        Me.ColumnHeader3.Width = 120
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Iscrizione"
        Me.ColumnHeader4.Width = 120
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Badge"
        Me.ColumnHeader5.Width = 180
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Scuola"
        Me.ColumnHeader6.Width = 180
        '
        'AzioniUtente
        '
        Me.AzioniUtente.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StampaBadgeToolStripMenuItem, Me.ToolStripSeparator1, Me.AggiornaToolStripMenuItem, Me.ToolStripSeparator2, Me.EliminaToolStripMenuItem})
        Me.AzioniUtente.Name = "AzioniUtente"
        Me.AzioniUtente.Size = New System.Drawing.Size(181, 104)
        '
        'StampaBadgeToolStripMenuItem
        '
        Me.StampaBadgeToolStripMenuItem.Name = "StampaBadgeToolStripMenuItem"
        Me.StampaBadgeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.StampaBadgeToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.StampaBadgeToolStripMenuItem.Text = "Stampa Badge"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'AggiornaToolStripMenuItem
        '
        Me.AggiornaToolStripMenuItem.Name = "AggiornaToolStripMenuItem"
        Me.AggiornaToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.AggiornaToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AggiornaToolStripMenuItem.Text = "Aggiorna"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(177, 6)
        '
        'EliminaToolStripMenuItem
        '
        Me.EliminaToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.EliminaToolStripMenuItem.Name = "EliminaToolStripMenuItem"
        Me.EliminaToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.EliminaToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.EliminaToolStripMenuItem.Text = "Elimina"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BadgeChanger)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.ModifyButton)
        Me.GroupBox1.Controls.Add(Me.ChangePic)
        Me.GroupBox1.Controls.Add(Me.RadioButton3)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.BoxEta)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.BoxScuola)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.BoxNome)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Enabled = False
        Me.GroupBox1.Location = New System.Drawing.Point(12, 300)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(553, 209)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = " "
        '
        'BadgeChanger
        '
        Me.BadgeChanger.Location = New System.Drawing.Point(213, 170)
        Me.BadgeChanger.Name = "BadgeChanger"
        Me.BadgeChanger.Size = New System.Drawing.Size(207, 27)
        Me.BadgeChanger.TabIndex = 44
        Me.BadgeChanger.Text = "Cambia/Riscrivi Badge"
        Me.BadgeChanger.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(280, 150)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 17)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "ND"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(213, 149)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 15)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Badge nr :"
        '
        'ModifyButton
        '
        Me.ModifyButton.Location = New System.Drawing.Point(426, 144)
        Me.ModifyButton.Name = "ModifyButton"
        Me.ModifyButton.Size = New System.Drawing.Size(102, 27)
        Me.ModifyButton.TabIndex = 42
        Me.ModifyButton.Text = "Modifica"
        Me.ModifyButton.UseVisualStyleBackColor = True
        '
        'ChangePic
        '
        Me.ChangePic.Location = New System.Drawing.Point(7, 145)
        Me.ChangePic.Name = "ChangePic"
        Me.ChangePic.Size = New System.Drawing.Size(102, 27)
        Me.ChangePic.TabIndex = 41
        Me.ChangePic.Text = "Cambia"
        Me.ChangePic.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(481, 109)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(42, 19)
        Me.RadioButton3.TabIndex = 40
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "ND"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(441, 109)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(31, 19)
        Me.RadioButton2.TabIndex = 39
        Me.RadioButton2.Text = "F"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(397, 109)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(36, 19)
        Me.RadioButton1.TabIndex = 38
        Me.RadioButton1.Text = "M"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'BoxEta
        '
        Me.BoxEta.BackColor = System.Drawing.SystemColors.Info
        Me.BoxEta.Location = New System.Drawing.Point(298, 108)
        Me.BoxEta.Mask = "00/00/0000"
        Me.BoxEta.Name = "BoxEta"
        Me.BoxEta.Size = New System.Drawing.Size(93, 23)
        Me.BoxEta.TabIndex = 37
        Me.BoxEta.ValidatingType = GetType(Date)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label4.Location = New System.Drawing.Point(172, 111)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 16)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Data di nascita :"
        '
        'BoxScuola
        '
        Me.BoxScuola.BackColor = System.Drawing.SystemColors.Info
        Me.BoxScuola.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.BoxScuola.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BoxScuola.FormattingEnabled = True
        Me.BoxScuola.Location = New System.Drawing.Point(298, 71)
        Me.BoxScuola.Name = "BoxScuola"
        Me.BoxScuola.Size = New System.Drawing.Size(230, 28)
        Me.BoxScuola.TabIndex = 35
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label3.Location = New System.Drawing.Point(224, 77)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 16)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Scuola : "
        '
        'BoxNome
        '
        Me.BoxNome.BackColor = System.Drawing.SystemColors.Info
        Me.BoxNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BoxNome.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BoxNome.ForeColor = System.Drawing.Color.Red
        Me.BoxNome.Location = New System.Drawing.Point(179, 39)
        Me.BoxNome.MaxLength = 16
        Me.BoxNome.Name = "BoxNome"
        Me.BoxNome.Size = New System.Drawing.Size(349, 26)
        Me.BoxNome.TabIndex = 33
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label2.Location = New System.Drawing.Point(176, 20)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 16)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Nome e Cognome : "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 21)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 15)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Foto :"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(7, 38)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(102, 100)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 30
        Me.PictureBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.LoadLogs)
        Me.GroupBox2.Controls.Add(Me.ViewAllLogs)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.EndData)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.StartData)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.LogsPresenze)
        Me.GroupBox2.Location = New System.Drawing.Point(571, 301)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(560, 382)
        Me.GroupBox2.TabIndex = 33
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Estrazione presenze"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.NrPresenzeText)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.NrLogsText)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Location = New System.Drawing.Point(343, 164)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(211, 212)
        Me.GroupBox3.TabIndex = 46
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Statistiche"
        '
        'NrPresenzeText
        '
        Me.NrPresenzeText.AutoSize = True
        Me.NrPresenzeText.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.NrPresenzeText.Location = New System.Drawing.Point(87, 77)
        Me.NrPresenzeText.Name = "NrPresenzeText"
        Me.NrPresenzeText.Size = New System.Drawing.Size(19, 21)
        Me.NrPresenzeText.TabIndex = 3
        Me.NrPresenzeText.Text = "0"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(43, 62)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(118, 15)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Stima Ingressi in aula"
        '
        'NrLogsText
        '
        Me.NrLogsText.AutoSize = True
        Me.NrLogsText.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.NrLogsText.Location = New System.Drawing.Point(87, 34)
        Me.NrLogsText.Name = "NrLogsText"
        Me.NrLogsText.Size = New System.Drawing.Size(19, 21)
        Me.NrLogsText.TabIndex = 1
        Me.NrLogsText.Text = "0"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(43, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(109, 15)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Timbrature Rilevate"
        '
        'LoadLogs
        '
        Me.LoadLogs.Location = New System.Drawing.Point(340, 131)
        Me.LoadLogs.Name = "LoadLogs"
        Me.LoadLogs.Size = New System.Drawing.Size(214, 27)
        Me.LoadLogs.TabIndex = 45
        Me.LoadLogs.Text = "Carica"
        Me.LoadLogs.UseVisualStyleBackColor = True
        '
        'ViewAllLogs
        '
        Me.ViewAllLogs.AutoSize = True
        Me.ViewAllLogs.Location = New System.Drawing.Point(469, 34)
        Me.ViewAllLogs.Name = "ViewAllLogs"
        Me.ViewAllLogs.Size = New System.Drawing.Size(90, 19)
        Me.ViewAllLogs.TabIndex = 39
        Me.ViewAllLogs.Text = "Carica Tutto"
        Me.ViewAllLogs.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(343, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 15)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Data Fine :"
        '
        'EndData
        '
        Me.EndData.Location = New System.Drawing.Point(340, 102)
        Me.EndData.Name = "EndData"
        Me.EndData.Size = New System.Drawing.Size(214, 23)
        Me.EndData.TabIndex = 37
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(343, 38)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 15)
        Me.Label8.TabIndex = 36
        Me.Label8.Text = "Data Inizio :"
        '
        'StartData
        '
        Me.StartData.Location = New System.Drawing.Point(340, 56)
        Me.StartData.Name = "StartData"
        Me.StartData.Size = New System.Drawing.Size(214, 23)
        Me.StartData.TabIndex = 35
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 15)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "Presenze :"
        '
        'LogsPresenze
        '
        Me.LogsPresenze.Location = New System.Drawing.Point(6, 38)
        Me.LogsPresenze.Name = "LogsPresenze"
        Me.LogsPresenze.Size = New System.Drawing.Size(328, 338)
        Me.LogsPresenze.TabIndex = 33
        '
        'ModifyUser
        '
        Me.ModifyUser.AutoSize = True
        Me.ModifyUser.Enabled = False
        Me.ModifyUser.Location = New System.Drawing.Point(19, 288)
        Me.ModifyUser.Name = "ModifyUser"
        Me.ModifyUser.Size = New System.Drawing.Size(111, 19)
        Me.ModifyUser.TabIndex = 48
        Me.ModifyUser.Text = "Modifica Utente"
        Me.ModifyUser.UseVisualStyleBackColor = True
        '
        'SqliteCommand1
        '
        Me.SqliteCommand1.CommandTimeout = 30
        Me.SqliteCommand1.Connection = Nothing
        Me.SqliteCommand1.Transaction = Nothing
        Me.SqliteCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None
        '
        'ReWriteBadge
        '
        Me.ReWriteBadge.Location = New System.Drawing.Point(6, 515)
        Me.ReWriteBadge.Name = "ReWriteBadge"
        Me.ReWriteBadge.Size = New System.Drawing.Size(565, 168)
        Me.ReWriteBadge.TabIndex = 49
        Me.ReWriteBadge.Visible = False
        '
        'ListaUtenti
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1143, 684)
        Me.Controls.Add(Me.ReWriteBadge)
        Me.Controls.Add(Me.ModifyUser)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.UserList)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ListaUtenti"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ListaUtenti"
        Me.AzioniUtente.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UserList As ListView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ChangePic As Button
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents BoxEta As MaskedTextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents BoxScuola As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents BoxNome As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents ViewAllLogs As CheckBox
    Friend WithEvents Label9 As Label
    Friend WithEvents EndData As DateTimePicker
    Friend WithEvents Label8 As Label
    Friend WithEvents StartData As DateTimePicker
    Friend WithEvents Label7 As Label
    Friend WithEvents LogsPresenze As TreeView
    Friend WithEvents ModifyButton As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents NrPresenzeText As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents NrLogsText As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents LoadLogs As Button
    Friend WithEvents ModifyUser As CheckBox
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents AzioniUtente As ContextMenuStrip
    Friend WithEvents EliminaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BadgeChanger As Button
    Friend WithEvents SqliteCommand1 As Microsoft.Data.Sqlite.SqliteCommand
    Friend WithEvents ReWriteBadge As Panel
    Friend WithEvents StampaBadgeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents AggiornaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
End Class
