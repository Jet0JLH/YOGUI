﻿Public Class GUI
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
        LoadConfigs()
        loadTabs()
    End Sub

    Public Sub createDefaultConfig()
        globalConfig.setConfig(New Config("main.form.title", "YOGUI", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.icon", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.backcolor", "#F0F0F0", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.backimage", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.tabs.alignment", "top", Origins.DefaultValue))
        globalConfig.setConfig(New Config("main.form.tabs.multiline", "false", Origins.DefaultValue))
        globalConfig.setConfig(New Config("config.domain", "settings\domainSettings.conf", Origins.DefaultValue))
        globalConfig.setConfig(New Config("config.computer", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("config.group", "", Origins.DefaultValue))
        globalConfig.setConfig(New Config("config.user", "", Origins.DefaultValue))
    End Sub

    Public Function generateTabTemplate(ByRef conf As ConfigSet) As TabPage
        Dim tabTemplate As New TabPage(conf.Value)
        Dim layoutPanel As New FlowLayoutPanel()
        layoutPanel.Dock = DockStyle.Fill
        layoutPanel.FlowDirection = FlowDirection.TopDown
        tabTemplate.Controls.Add(layoutPanel)
        tabTemplate.Tag = conf
        Return tabTemplate
    End Function

    Sub loadTabs(Optional removeOldTabs As Boolean = True)
        If removeOldTabs Then TabControl1.TabPages.Clear()
        For Each tabItem In globalConfig.getConfigs("tabs.names.*")
            'Dim tabSettingName As String = tabItem.Name.Substring(11)
            Dim allreadyExists As Boolean = False
            For Each page As TabPage In TabControl1.TabPages
                If page.Tag.Equals(tabItem) Then allreadyExists = True : Exit For
            Next
            If allreadyExists = False Then TabControl1.TabPages.Add(generateTabTemplate(tabItem))
        Next
    End Sub

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

    Private Sub LoadConfigs()
        For Each item As ConfigSet In globalConfig.getConfigs("config.*")
            If My.Computer.FileSystem.FileExists(item.Value) Then
                Select Case item.Name
                    Case "config.domain"
                        globalConfig.ImportConfig(item.Value, Origins.Domain)
                    Case "config.computer"
                        globalConfig.ImportConfig(item.Value, Origins.Computer)
                    Case "config.group"
                        globalConfig.ImportConfig(item.Value, Origins.Group)
                    Case "config.user"
                        globalConfig.ImportConfig(item.Value, Origins.User)
                End Select
            Else
                debugging("Configfile dosen't exist! " & vbCrLf & vbTab & item.Name & ": " & item.Value, Loglevel.Warning)
            End If
        Next
    End Sub

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
