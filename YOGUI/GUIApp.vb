Public Class GUIApp
    Inherits App

    'Attributs
    Private Const margin As Integer = 5
    Private btn As New Button
    Private pic As New PictureBox
    Public appPannel As New Panel
    Private _picPath

    'Events
    Public Event picChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String)

    'Constructors
    Public Sub New()
        Me.New("")
    End Sub
    Public Sub New(name As String)
        MyBase.New(name)

        'appPannel settings
        Me.appPannel.Size = New Size(250, 60)
        Me.appPannel.BorderStyle = BorderStyle.FixedSingle

        'Pic settings
        Me.pic.BackgroundImageLayout = ImageLayout.Stretch
        Dim picSize As Integer = appPannel.Size.Height - margin * 2
        Me.pic.Size = New Size(picSize, picSize)
        Me.pic.Location = New Point(margin, margin)
        Me.appPannel.Controls.Add(Me.pic)
        'Btn settings
        Me.btn.Text = Me.Name
        Me.btn.Size = New Size(Me.appPannel.Size.Width - (margin * 4 + picSize), picSize)
        Me.btn.Location = New Point(margin * 3 + picSize, margin)
        Me.btn.Anchor = AnchorStyles.Top & AnchorStyles.Left & AnchorStyles.Bottom & AnchorStyles.Right
        Me.appPannel.Controls.Add(Me.btn)
    End Sub

    'Propertys
    Public Property PicPath As String
        Get
            Return _picPath
        End Get
        Set(value As String)
            Dim oldVal As String = _picPath
            If oldVal <> value Then
                If value = "" Then
                    pic.BackgroundImage = New Bitmap(1, 1)
                    pic.Visible = False
                Else
                    If My.Computer.FileSystem.FileExists(value) = False Then Throw New InvalidFileName
                    Try
                        Me.pic.BackgroundImage = New Bitmap(value)
                        _picPath = value
                        pic.Visible = True
                        RaiseEvent picChange(Me, oldVal, value)
                    Catch ex As Exception
                        MsgBox("Error while loding Picturebox" & vbCrLf & "App: " & Me.Name & vbCrLf & "PicturePath: " & value)
                    End Try
                End If
            End If
        End Set
    End Property

    'Eventreactions
    Private Sub btnNameChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String) Handles MyBase.nameChange
        btn.Text = newVal
    End Sub
End Class
