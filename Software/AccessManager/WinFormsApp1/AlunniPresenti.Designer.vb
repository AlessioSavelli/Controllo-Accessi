<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AlunniPresenti
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AlunniPresenti))
        Me.AlunniPresenti1 = New System.Windows.Forms.ListView()
        Me.MenuModifica = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TimbraUscitaManualeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IngressoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UscitaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserPicture = New System.Windows.Forms.ImageList(Me.components)
        Me.ConteggioAlunni = New System.Windows.Forms.Timer(Me.components)
        Me.TimbraturaManuale = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.NomeTimbrante = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.NotRegistredUser = New System.Windows.Forms.GroupBox()
        Me.BoxEta = New System.Windows.Forms.MaskedTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ScuolaUtente = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NomeUtente = New System.Windows.Forms.TextBox()
        Me.ListaAlunniScuole = New System.Windows.Forms.TreeView()
        Me.EditorColoreScuola = New System.Windows.Forms.ColorDialog()
        Me.MenuModifica.SuspendLayout()
        Me.TimbraturaManuale.SuspendLayout()
        Me.NotRegistredUser.SuspendLayout()
        Me.SuspendLayout()
        '
        'AlunniPresenti1
        '
        Me.AlunniPresenti1.ContextMenuStrip = Me.MenuModifica
        Me.AlunniPresenti1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlunniPresenti1.HideSelection = False
        Me.AlunniPresenti1.LargeImageList = Me.UserPicture
        Me.AlunniPresenti1.Location = New System.Drawing.Point(0, 0)
        Me.AlunniPresenti1.Name = "AlunniPresenti1"
        Me.AlunniPresenti1.Size = New System.Drawing.Size(946, 543)
        Me.AlunniPresenti1.SmallImageList = Me.UserPicture
        Me.AlunniPresenti1.TabIndex = 0
        Me.AlunniPresenti1.UseCompatibleStateImageBehavior = False
        '
        'MenuModifica
        '
        Me.MenuModifica.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TimbraUscitaManualeToolStripMenuItem, Me.ToolStripSeparator1, Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem})
        Me.MenuModifica.Name = "MenuModifica"
        Me.MenuModifica.Size = New System.Drawing.Size(328, 76)
        '
        'TimbraUscitaManualeToolStripMenuItem
        '
        Me.TimbraUscitaManualeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IngressoToolStripMenuItem, Me.UscitaToolStripMenuItem})
        Me.TimbraUscitaManualeToolStripMenuItem.Image = CType(resources.GetObject("TimbraUscitaManualeToolStripMenuItem.Image"), System.Drawing.Image)
        Me.TimbraUscitaManualeToolStripMenuItem.Name = "TimbraUscitaManualeToolStripMenuItem"
        Me.TimbraUscitaManualeToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.TimbraUscitaManualeToolStripMenuItem.Text = "Timbratura Manuale"
        '
        'IngressoToolStripMenuItem
        '
        Me.IngressoToolStripMenuItem.Name = "IngressoToolStripMenuItem"
        Me.IngressoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.IngressoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.IngressoToolStripMenuItem.Text = "Ingresso"
        '
        'UscitaToolStripMenuItem
        '
        Me.UscitaToolStripMenuItem.Image = CType(resources.GetObject("UscitaToolStripMenuItem.Image"), System.Drawing.Image)
        Me.UscitaToolStripMenuItem.Name = "UscitaToolStripMenuItem"
        Me.UscitaToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.UscitaToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UscitaToolStripMenuItem.Text = "Uscita"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(324, 6)
        '
        'ChiudiAulaTimbraUscitaATuttiToolStripMenuItem
        '
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Image = CType(resources.GetObject("ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Name = "ChiudiAulaTimbraUscitaATuttiToolStripMenuItem"
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.ChiudiAulaTimbraUscitaATuttiToolStripMenuItem.Text = "Chiudi Aula (Timbra uscita a tutti)"
        '
        'UserPicture
        '
        Me.UserPicture.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit
        Me.UserPicture.ImageStream = CType(resources.GetObject("UserPicture.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.UserPicture.TransparentColor = System.Drawing.Color.Transparent
        Me.UserPicture.Images.SetKeyName(0, "def_avatar.png")
        Me.UserPicture.Images.SetKeyName(1, "badge.png")
        '
        'ConteggioAlunni
        '
        Me.ConteggioAlunni.Enabled = True
        Me.ConteggioAlunni.Interval = 1000
        '
        'TimbraturaManuale
        '
        Me.TimbraturaManuale.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimbraturaManuale.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TimbraturaManuale.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TimbraturaManuale.Controls.Add(Me.Button2)
        Me.TimbraturaManuale.Controls.Add(Me.NomeTimbrante)
        Me.TimbraturaManuale.Controls.Add(Me.Label3)
        Me.TimbraturaManuale.Controls.Add(Me.Button1)
        Me.TimbraturaManuale.Controls.Add(Me.CheckBox1)
        Me.TimbraturaManuale.Controls.Add(Me.NotRegistredUser)
        Me.TimbraturaManuale.Controls.Add(Me.ListaAlunniScuole)
        Me.TimbraturaManuale.Location = New System.Drawing.Point(127, 28)
        Me.TimbraturaManuale.Name = "TimbraturaManuale"
        Me.TimbraturaManuale.Size = New System.Drawing.Size(675, 492)
        Me.TimbraturaManuale.TabIndex = 1
        Me.TimbraturaManuale.Visible = False
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Button2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Button2.Location = New System.Drawing.Point(639, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(29, 34)
        Me.Button2.TabIndex = 11
        Me.Button2.Text = "X"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'NomeTimbrante
        '
        Me.NomeTimbrante.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.NomeTimbrante.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.NomeTimbrante.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.NomeTimbrante.Location = New System.Drawing.Point(332, 283)
        Me.NomeTimbrante.Name = "NomeTimbrante"
        Me.NomeTimbrante.PlaceholderText = "Attesa selezione utente"
        Me.NomeTimbrante.ReadOnly = True
        Me.NomeTimbrante.Size = New System.Drawing.Size(330, 29)
        Me.NomeTimbrante.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label3.Location = New System.Drawing.Point(332, 259)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(164, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Stai Timbrando per :"
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Button1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Button1.ImageKey = "badge.png"
        Me.Button1.ImageList = Me.UserPicture
        Me.Button1.Location = New System.Drawing.Point(407, 317)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button1.Size = New System.Drawing.Size(173, 168)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Timbra"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button1.UseVisualStyleBackColor = False
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CheckBox1.Location = New System.Drawing.Point(325, 24)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(254, 25)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "Lezione di prova senza Badge"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'NotRegistredUser
        '
        Me.NotRegistredUser.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.NotRegistredUser.Controls.Add(Me.BoxEta)
        Me.NotRegistredUser.Controls.Add(Me.Label4)
        Me.NotRegistredUser.Controls.Add(Me.Label2)
        Me.NotRegistredUser.Controls.Add(Me.ScuolaUtente)
        Me.NotRegistredUser.Controls.Add(Me.Label1)
        Me.NotRegistredUser.Controls.Add(Me.NomeUtente)
        Me.NotRegistredUser.Enabled = False
        Me.NotRegistredUser.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.NotRegistredUser.Location = New System.Drawing.Point(326, 62)
        Me.NotRegistredUser.Name = "NotRegistredUser"
        Me.NotRegistredUser.Size = New System.Drawing.Size(342, 172)
        Me.NotRegistredUser.TabIndex = 1
        Me.NotRegistredUser.TabStop = False
        Me.NotRegistredUser.Text = "Lezione di prova"
        '
        'BoxEta
        '
        Me.BoxEta.BackColor = System.Drawing.SystemColors.Window
        Me.BoxEta.Location = New System.Drawing.Point(134, 128)
        Me.BoxEta.Mask = "00/00/0000"
        Me.BoxEta.Name = "BoxEta"
        Me.BoxEta.Size = New System.Drawing.Size(105, 29)
        Me.BoxEta.TabIndex = 9
        Me.BoxEta.ValidatingType = GetType(Date)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label4.Location = New System.Drawing.Point(8, 131)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Data di nascita :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 92)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 21)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Scuola :"
        '
        'ScuolaUtente
        '
        Me.ScuolaUtente.FormattingEnabled = True
        Me.ScuolaUtente.Location = New System.Drawing.Point(134, 89)
        Me.ScuolaUtente.Name = "ScuolaUtente"
        Me.ScuolaUtente.Size = New System.Drawing.Size(202, 29)
        Me.ScuolaUtente.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nome :"
        '
        'NomeUtente
        '
        Me.NomeUtente.Location = New System.Drawing.Point(6, 49)
        Me.NomeUtente.Name = "NomeUtente"
        Me.NomeUtente.Size = New System.Drawing.Size(330, 29)
        Me.NomeUtente.TabIndex = 0
        '
        'ListaAlunniScuole
        '
        Me.ListaAlunniScuole.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListaAlunniScuole.BackColor = System.Drawing.Color.Silver
        Me.ListaAlunniScuole.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ListaAlunniScuole.Location = New System.Drawing.Point(3, 3)
        Me.ListaAlunniScuole.Name = "ListaAlunniScuole"
        Me.ListaAlunniScuole.Size = New System.Drawing.Size(317, 482)
        Me.ListaAlunniScuole.TabIndex = 0
        '
        'AlunniPresenti
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 543)
        Me.ControlBox = False
        Me.Controls.Add(Me.TimbraturaManuale)
        Me.Controls.Add(Me.AlunniPresenti1)
        Me.Name = "AlunniPresenti"
        Me.Text = "Alunni Presenti : 0"
        Me.MenuModifica.ResumeLayout(False)
        Me.TimbraturaManuale.ResumeLayout(False)
        Me.TimbraturaManuale.PerformLayout()
        Me.NotRegistredUser.ResumeLayout(False)
        Me.NotRegistredUser.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AlunniPresenti1 As ListView
    Friend WithEvents UserPicture As ImageList
    Friend WithEvents ConteggioAlunni As Timer
    Friend WithEvents MenuModifica As ContextMenuStrip
    Friend WithEvents TimbraUscitaManualeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IngressoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UscitaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ChiudiAulaTimbraUscitaATuttiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TimbraturaManuale As Panel
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents NotRegistredUser As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ScuolaUtente As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents NomeUtente As TextBox
    Friend WithEvents ListaAlunniScuole As TreeView
    Friend WithEvents BoxEta As MaskedTextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents NomeTimbrante As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents EditorColoreScuola As ColorDialog
End Class
