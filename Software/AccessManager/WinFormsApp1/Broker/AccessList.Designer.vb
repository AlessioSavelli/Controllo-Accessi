<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AccessList
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
        Me.AccessList1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.item = New System.Windows.Forms.Label()
        Me.ListRicerca = New System.Windows.Forms.ListView()
        Me.ItemCount = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'AccessList1
        '
        Me.AccessList1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccessList1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.AccessList1.FullRowSelect = True
        Me.AccessList1.GridLines = True
        Me.AccessList1.HideSelection = False
        Me.AccessList1.Location = New System.Drawing.Point(0, 35)
        Me.AccessList1.MultiSelect = False
        Me.AccessList1.Name = "AccessList1"
        Me.AccessList1.Size = New System.Drawing.Size(809, 334)
        Me.AccessList1.TabIndex = 0
        Me.AccessList1.UseCompatibleStateImageBehavior = False
        Me.AccessList1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Nome"
        Me.ColumnHeader1.Width = 240
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Orario"
        Me.ColumnHeader2.Width = 130
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Scuola"
        Me.ColumnHeader3.Width = 230
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "UID"
        Me.ColumnHeader4.Width = 200
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Cerca : "
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(64, 6)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(642, 23)
        Me.TextBox1.TabIndex = 2
        '
        'item
        '
        Me.item.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.item.AutoSize = True
        Me.item.Location = New System.Drawing.Point(743, 9)
        Me.item.Name = "item"
        Me.item.Size = New System.Drawing.Size(13, 15)
        Me.item.TabIndex = 3
        Me.item.Text = "0"
        '
        'ListRicerca
        '
        Me.ListRicerca.HideSelection = False
        Me.ListRicerca.Location = New System.Drawing.Point(371, 125)
        Me.ListRicerca.Name = "ListRicerca"
        Me.ListRicerca.Size = New System.Drawing.Size(121, 97)
        Me.ListRicerca.TabIndex = 4
        Me.ListRicerca.UseCompatibleStateImageBehavior = False
        Me.ListRicerca.Visible = False
        '
        'ItemCount
        '
        Me.ItemCount.Enabled = True
        Me.ItemCount.Interval = 1000
        '
        'AccessList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(811, 370)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListRicerca)
        Me.Controls.Add(Me.item)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AccessList1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AccessList"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Lista Accessi del mese"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AccessList1 As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents item As Label
    Friend WithEvents ListRicerca As ListView
    Friend WithEvents ItemCount As Timer
End Class
