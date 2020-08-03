<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GUI
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(356, 28)
        '
        'VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem
        '
        Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem.Name = "VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem"
        Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem.Size = New System.Drawing.Size(355, 24)
        Me.VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem.Text = "Verzeichnis der Admin-Konsole öffnen"
        '
        'GUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(896, 647)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "GUI"
        Me.Text = "GUI"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem As ToolStripMenuItem
End Class
