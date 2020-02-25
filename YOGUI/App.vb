Public Class App
    'Attributs
    Private _name As String
    Private _filePath As String
    Private _group As String
    Private _category As String

    'Events
    Public Event nameChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String)
    Public Event filePathChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String)
    Public Event executionStarts(ByRef sender As App, ByVal index As Byte)
    Public Event groupChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String)
    Public Event categoryChange(ByRef sender As App, ByVal oldVal As String, ByVal newVal As String)

    'Errors
    Public Class InvalidFileName
        Inherits Exception
    End Class

    'Constructors
    Public Sub New()
        Me.New("")
    End Sub
    Public Sub New(Name As String)
        _name = Name
    End Sub

    'Propertys
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            Dim oldVal As String = _name
            If oldVal <> value Then
                _name = value
                RaiseEvent nameChange(Me, oldVal, value)
            End If
        End Set
    End Property
    Public Property FilePath As String
        Get
            Return _filePath
        End Get
        Set(value As String)
            For Each badChar As Char In IO.Path.GetInvalidPathChars
                If value.Contains(badChar) Then
                    Throw New InvalidFileName
                End If
            Next
            Dim oldVal As String = _filePath
            If oldVal <> value Then
                _filePath = value
                RaiseEvent filePathChange(Me, oldVal, value)
            End If
        End Set
    End Property
    Public Property Group As String
        Get
            Return _group
        End Get
        Set(value As String)
            Dim oldVal As String = _filePath
            If oldVal <> value Then
                _group = value
                RaiseEvent groupChange(Me, oldVal, value)
            End If
        End Set
    End Property
    Public Property Category As String
        Get
            Return _category
        End Get
        Set(value As String)
            Dim oldVal As String = _filePath
            If oldVal <> value Then
                _category = value
                RaiseEvent categoryChange(Me, oldVal, value)
            End If
        End Set
    End Property

    'Subs & Functions
    Public Sub ExecuteCommands(index As Byte)
        RaiseEvent executionStarts(Me, index)
        Try

        Catch ex As Exception
            MsgBox("Error while execution", MsgBoxStyle.Critical)
        End Try
    End Sub
    Public Function generateXML() As XDocument
        Dim XML As New XDocument(<conf><app><name></name></app></conf>)
        XML.Element("conf").Element("app").Element("name").Value = _name
        Return XML
    End Function
    Public Sub saveXML()
        saveXML(generateXML())
    End Sub
    Public Sub saveXML(XML As XDocument)
        If _filePath = "" Then Throw New InvalidFileName
        Try
            Dim path As String = IO.Path.GetDirectoryName(_filePath)
            If My.Computer.FileSystem.DirectoryExists(path) = False Then
                My.Computer.FileSystem.CreateDirectory(path)
            End If
            XML.Save(_filePath)
        Catch ex As Exception
            MsgBox("Error while saving AppFile" & vbCrLf & _filePath, MsgBoxStyle.Critical)
        End Try
    End Sub
    Public Sub loadXML()
        If My.Computer.FileSystem.FileExists(_filePath) = False Then Throw New InvalidFileName
        Try
            Dim XML As XDocument = XDocument.Load(_filePath)
            Me.Name = XML.Element("conf").Element("app").Element("name").Value
        Catch ex As Exception
            MsgBox("Error while loading AppFile" & vbCrLf & _filePath, MsgBoxStyle.Critical)
        End Try
    End Sub
    Public Sub loadXML(filePath As String)
        Me.FilePath = filePath
        Me.loadXML()
    End Sub
End Class
