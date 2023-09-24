<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScritturaBadge
    Inherits System.Windows.Forms.UserControl

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Output = New System.Windows.Forms.TextBox()
        Me.TryAgain = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PictureBox1.Location = New System.Drawing.Point(318, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(205, 165)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Output
        '
        Me.Output.BackColor = System.Drawing.SystemColors.Control
        Me.Output.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Output.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Output.ForeColor = System.Drawing.Color.Green
        Me.Output.Location = New System.Drawing.Point(0, 14)
        Me.Output.Multiline = True
        Me.Output.Name = "Output"
        Me.Output.Size = New System.Drawing.Size(266, 166)
        Me.Output.TabIndex = 1
        Me.Output.Text = "Rilevamento perferica di lettura..."
        Me.Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TryAgain
        '
        Me.TryAgain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TryAgain.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TryAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.TryAgain.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TryAgain.Location = New System.Drawing.Point(24, 94)
        Me.TryAgain.Name = "TryAgain"
        Me.TryAgain.Size = New System.Drawing.Size(223, 29)
        Me.TryAgain.TabIndex = 2
        Me.TryAgain.Text = "Riprova"
        Me.TryAgain.UseVisualStyleBackColor = False
        Me.TryAgain.Visible = False
        '
        'ScritturaBadge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TryAgain)
        Me.Controls.Add(Me.Output)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "ScritturaBadge"
        Me.Size = New System.Drawing.Size(584, 183)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Output As TextBox
    Friend WithEvents TryAgain As Button
End Class
