Imports System.ComponentModel

Public Class Debuggerle
    Private Sub Debuggerle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.DataSource = GUI.globalConfig.allConfigs
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        DataGridView1.AutoResizeColumns()
        DataGridView1.Update()
        DataGridView1.Refresh()
    End Sub

    Private Sub Debuggerle_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Timer1.Start()
    End Sub

    Private Sub Debuggerle_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Timer1.Stop()
        e.Cancel = True
        Me.Hide()
    End Sub
End Class