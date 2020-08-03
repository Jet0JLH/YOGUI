Public Class GUI
    Private Sub GUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub GUI_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    End Sub

    Public Function getTabTemplate(ByVal name As String) As TabPage
        Dim tabTemplate As New TabPage(name)
        Dim layoutPanel As New FlowLayoutPanel()
        layoutPanel.Dock = DockStyle.Fill
        layoutPanel.FlowDirection = FlowDirection.TopDown
        tabTemplate.Controls.Add(layoutPanel)
        Return tabTemplate
    End Function
End Class
