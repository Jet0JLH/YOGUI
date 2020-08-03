Public Class GUI
    Public globalConfig As Vault
    Dim TabControl1 As TabControl
    Private Sub GUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        globalConfig = New Vault()
        TabControl1 = New TabControl()
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TabControl1.Location = New System.Drawing.Point(11, 35)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(650, 480)
        Me.TabControl1.TabIndex = 1
        Me.Controls.Add(TabControl1)

        Me.ContextMenuStrip = Me.ContextMenuStrip1

        AddHandler TabControl1.KeyUp, AddressOf ShortcutHandler
    End Sub

    Private Sub GUI_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        connectHandler()
        createDefaultConfig()
    End Sub

    Public Sub createDefaultConfig()
        globalConfig.setConfig(New Config("main.form.title", "YOGUI", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.icon", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.backcolor", "#F0F0F0", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.backimage", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.tabs.alignment", "top", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.tabs.multiline", "false", Origins.DefaultValue))
    End Sub

    Public Function getTabTemplate(ByVal name As String) As TabPage
        Dim tabTemplate As New TabPage(name)
        Dim layoutPanel As New FlowLayoutPanel()
        layoutPanel.Dock = DockStyle.Fill
        layoutPanel.FlowDirection = FlowDirection.TopDown
        tabTemplate.Controls.Add(layoutPanel)
        Return tabTemplate
    End Function

    Sub debugging(text As String, Optional level As Loglevel = 0)
        Dim timestamp As String = My.Computer.Clock.LocalTime.ToString
        Dim levelStr As String = ""
        Select Case level
            Case 0
                levelStr = "[DEBUG]"
            Case 1
                levelStr = "[INFO]"
            Case 2
                levelStr = "[WARNING]"
            Case 3
                levelStr = "[ERROR]"
            Case 4
                levelStr = "[CRITICAL]"
        End Select
        Console.WriteLine(timestamp & vbTab & levelStr & vbTab & text)
        Debuggerle.RichTextBox1.AppendText(timestamp & vbTab & levelStr & vbTab & text & vbCrLf)
    End Sub
    Enum Loglevel
        Debug = 0
        Info = 1
        Warning = 2
        Err = 3
        Critical = 4
    End Enum

#Region "Handler"
    Private Sub connectHandler()
        AddHandler globalConfig.getConfig("main.form.title").ValueChanged, AddressOf handlerMainWindowTitle
        AddHandler globalConfig.getConfig("main.form.icon").ValueChanged, AddressOf handlerFormIcons
        AddHandler globalConfig.getConfig("main.form.backcolor").ValueChanged, AddressOf handlerFormBackColor
        AddHandler globalConfig.getConfig("main.form.backimage").ValueChanged, AddressOf handlerFormBackImage
        AddHandler globalConfig.getConfig("main.form.tabs.alignment").ValueChanged, AddressOf handlerTabAlignment
        AddHandler globalConfig.getConfig("main.form.tabs.multiline").ValueChanged, AddressOf handlerTabMultiline
    End Sub
    Private Sub disconnectHandler()
        RemoveHandler globalConfig.getConfig("main.form.title").ValueChanged, AddressOf handlerMainWindowTitle
        RemoveHandler globalConfig.getConfig("main.form.icon").ValueChanged, AddressOf handlerFormIcons
        RemoveHandler globalConfig.getConfig("main.form.backcolor").ValueChanged, AddressOf handlerFormBackColor
        RemoveHandler globalConfig.getConfig("main.form.backimage").ValueChanged, AddressOf handlerFormBackImage
        RemoveHandler globalConfig.getConfig("main.form.tabs.alignment").ValueChanged, AddressOf handlerTabAlignment
        RemoveHandler globalConfig.getConfig("main.form.tabs.multiline").ValueChanged, AddressOf handlerTabMultiline
    End Sub
    Sub handlerMainWindowTitle(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        Me.Text = newValue
    End Sub
    Sub handlerFormIcons(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        If newValue <> "" Then
            Try
                If My.Computer.FileSystem.FileExists(newValue) = False Then
                    debugging("Formicon file dosen't exist", Loglevel.Warning)
                    Exit Sub
                End If
                Dim ico As New Icon(newValue)
                Me.Icon = ico
                Debuggerle.Icon = ico
            Catch ex As Exception
                debugging("Error while loading formicon", Loglevel.Err)
            End Try
        End If
    End Sub
    Sub handlerFormBackColor(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        Try
            Me.BackColor = Drawing.ColorTranslator.FromHtml(newValue)
            Debuggerle.BackColor = Me.BackColor
        Catch ex As Exception
            debugging("Error while translate hex to color for backcolor", LogLevel.Err)
        End Try
    End Sub
    Sub handlerFormBackImage(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        Try
            If newValue = "" Then
                Me.BackgroundImage = Nothing
                Debuggerle.BackgroundImage = Nothing
            ElseIf My.Computer.FileSystem.FileExists(newValue) = False Then
                debugging("Formbackgroundimage file dosen't exist", LogLevel.Warning)
            Else
                Me.BackgroundImage = New Bitmap(newValue)
                Debuggerle.BackgroundImage = Me.BackgroundImage
            End If
        Catch ex As Exception
            debugging("Error while loading formbackgroundfile", LogLevel.Err)
        End Try
    End Sub
    Sub handlerTabAlignment(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        Select Case newValue.ToLower
            Case "top"
                TabControl1.Alignment = TabAlignment.Top
            Case "bottom"
                TabControl1.Alignment = TabAlignment.Bottom
            Case "left"
                TabControl1.Alignment = TabAlignment.Left
            Case "right"
                TabControl1.Alignment = TabAlignment.Right
            Case Else
                TabControl1.Alignment = TabAlignment.Top
        End Select
    End Sub
    Sub handlerTabMultiline(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
        If newValue.ToLower = "true" Then
            TabControl1.Multiline = True
        Else
            TabControl1.Multiline = False
        End If
    End Sub

    Private Sub ShortcutHandler(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = (Keys.Control + Keys.Delete) Then
            Debuggerle.Show()
        End If
    End Sub
    Private Sub VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerzeichnisDerAdminKonsoleÖffnenToolStripMenuItem.Click
        Shell("explorer " & Application.StartupPath, AppWinStyle.NormalFocus)
    End Sub
#End Region
End Class
