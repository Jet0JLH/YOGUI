Public Class Vault
    Public allConfigs As List(Of ConfigSet)
    Public Sub New()
        allConfigs = New List(Of ConfigSet)
    End Sub
    Public Function setConfig(config As Config) As ConfigSet
        Dim found As Boolean = False
        For i As Integer = 0 To allConfigs.Count - 1
            If allConfigs(i).Name = config.Name Then
                found = True
                allConfigs(i).addConfig(config)
                Return allConfigs(i)
                Exit For
            End If
        Next
        If found = False Then
            Dim tempConfigSet As New ConfigSet(config.Name)
            tempConfigSet.addConfig(config)
            allConfigs.Add(tempConfigSet)
            Return tempConfigSet
        End If
        Return Nothing
    End Function
    Public Function getConfig(name As String, Optional createNewConfigWhenNotExist As Boolean = True) As ConfigSet
        For i As Integer = 0 To allConfigs.Count - 1
            If allConfigs(i).Name = name Then
                Return allConfigs(i)
            End If
        Next
        If createNewConfigWhenNotExist Then 'Sollte noch keine Konfig vorhanden sein, so wird diese leer angelegt außer es ist expliziet nicht gewünscht
            Dim newConfig As New ConfigSet(name)
            newConfig.addConfig(New Config(name, ""))
            allConfigs.Add(newConfig)
            Return newConfig
        Else
            Return New ConfigSet
        End If
    End Function
    Public Function ConfigExist(name As String) As Boolean
        For i As Integer = 0 To allConfigs.Count - 1
            If allConfigs(i).Name = name Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Sub deleteConfigs()
        allConfigs.Clear()
    End Sub
    Public Function deleteConfigs(name As String) As Integer
        Dim delCount As Integer = 0
        Dim indexes As New List(Of Integer)
        If name = "*" Then
            allConfigs.Clear()
        ElseIf name.StartsWith("*") And name.EndsWith("*") Then 'Beinhaltet Name
            name = name.Substring(1, name.Length - 2)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.Contains(name) Then
                    indexes.Add(i)
                    delCount += 1
                End If
            Next
        ElseIf name.StartsWith("*") Then 'Ende mit Name
            name = name.Substring(1)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.EndsWith(name) Then
                    indexes.Add(i)
                    delCount += 1
                End If
            Next
        ElseIf name.EndsWith("*") Then 'Beginnt mit Name
            name = name.Substring(0, name.Length - 1)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.StartsWith(name) Then
                    indexes.Add(i)
                    delCount += 1
                End If
            Next
        Else 'Exakter Name
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name = name Then
                    allConfigs.RemoveAt(i)
                    delCount = 1
                    Exit For
                End If
            Next
        End If
        For i As Integer = indexes.Count - 1 To 0 Step -1
            allConfigs.RemoveAt(indexes(i))
        Next
        Return delCount
    End Function
    Public Function getConfigs(name As String) As List(Of ConfigSet)
        Dim tempList As New List(Of ConfigSet)
        If name = "*" Then
            Return allConfigs
        ElseIf name.StartsWith("*") And name.EndsWith("*") Then 'Beinhaltet Name
            name = name.Substring(1, name.Length - 2)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.Contains(name) Then
                    tempList.Add(allConfigs(i))
                End If
            Next
        ElseIf name.StartsWith("*") Then 'Ende mit Name
            name = name.Substring(1)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.EndsWith(name) Then
                    tempList.Add(allConfigs(i))
                End If
            Next
        ElseIf name.EndsWith("*") Then 'Beginnt mit Name
            name = name.Substring(0, name.Length - 1)
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name.StartsWith(name) Then
                    tempList.Add(allConfigs(i))
                End If
            Next
        Else 'Exakter Name
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Name = name Then
                    tempList.Add(allConfigs(i))
                    Exit For
                End If
            Next
        End If
        Return tempList
    End Function
End Class
Public Class ConfigSet
    Public Event ValueChanged(ByRef sender As ConfigSet, ByVal Name As String, ByVal oldValue As String, ByVal newValue As String)
    Protected allConfigs As List(Of Config)
    Protected _Name As String
    Protected lastAktivValue As String
    Public Tag As Object
    Public Sub New(Optional name As String = "")
        Me.Name = name
        allConfigs = New List(Of Config)
    End Sub
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property
    Public Sub removeConfig(Optional origin As Origins = -1)
        If origin = -1 Then 'Seems that all entrys schould be removed
            allConfigs.Clear()
        Else
            For i As Integer = 0 To allConfigs.Count - 1
                If allConfigs(i).Origin = origin Then
                    allConfigs.RemoveAt(i)
                    CheckActivValue()
                    Exit For
                End If
            Next
        End If
    End Sub
    Public Sub addConfig(config As Config)
        Dim found As Boolean = False
        For i As Integer = 0 To allConfigs.Count - 1
            If allConfigs(i).Origin = config.Origin Then 'In this case we only need to update the config
                allConfigs(i) = config
                found = True
                CheckActivValue()
                Exit For
            End If
        Next
        If Not found Then allConfigs.Add(config) : allConfigs = allConfigs.OrderBy(Function(s) s.Origin).ToList : CheckActivValue()
    End Sub
    Public ReadOnly Property Value As String
        Get
            If allConfigs.Count > 0 Then
                For i As Integer = 0 To allConfigs.Count - 1
                    If allConfigs(i).LockLevel = LockLevels.Locked Then
                        Return Environment.ExpandEnvironmentVariables(allConfigs(i).Value)
                    End If
                Next
                Return Environment.ExpandEnvironmentVariables(allConfigs(allConfigs.Count - 1).Value)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property Origin As Origins
        Get
            If allConfigs.Count > 0 Then
                For i As Integer = 0 To allConfigs.Count - 1
                    If allConfigs(i).LockLevel = LockLevels.Locked Then
                        Return allConfigs(i).Origin
                    End If
                Next
                Return allConfigs(allConfigs.Count - 1).Origin
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private Sub CheckActivValue()
        Dim currentActiveValue As String = Value
        Dim tempLastActiveValue As String = lastAktivValue
        If currentActiveValue <> tempLastActiveValue Then
            lastAktivValue = currentActiveValue
            RaiseEvent ValueChanged(Me, Name, tempLastActiveValue, currentActiveValue)
        End If
    End Sub
End Class
Public Class Config
    Protected _Name As String
    Protected _Value As String
    Protected _Origin As Origins
    Protected _LockLevel As LockLevels
    Public Sub New(Optional name As String = "", Optional value As String = "", Optional origin As Origins = Origins.DefaultValue, Optional lockLevel As LockLevels = LockLevels.Unlocked)
        Me.Name = name
        Me.Value = value
        Me.Origin = origin
        Me.LockLevel = lockLevel
    End Sub
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property
    Public Property Value As String
        Get
            Return _Value
        End Get
        Set(value As String)
            _Value = value
        End Set
    End Property
    Public Property Origin As Origins
        Get
            Return _Origin
        End Get
        Set(value As Origins)
            _Origin = value
        End Set
    End Property
    Public Property LockLevel As LockLevels
        Get
            Return _LockLevel
        End Get
        Set(value As LockLevels)
            _LockLevel = value
        End Set
    End Property
End Class
Public Enum Origins
    DefaultValue = 0
    Domain = 10
    Computer = 20
    Group = 30
    User = 40
End Enum
Public Enum LockLevels
    Unlocked = 0
    Pref = 10
    LockPref = 20
    Locked = 30
End Enum