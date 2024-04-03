Imports System.Runtime.Serialization

<Serializable()> _
Public Class NodeProxy

    Public Sub New()

    End Sub
    Public Sub New(ByVal Checked As Boolean)
        CheckedStatus = Checked
    End Sub

    Public Folders As New Dictionary(Of String, NodeProxy)
    Public Files As New Dictionary(Of String, NodeProxy)

    Public Name As String

    Public CheckedStatus As Boolean

    ''' <summary>
    ''' Either this node of one of its children (at some level) contains a decision
    ''' </summary>
    ''' <remarks></remarks>
    Public NodeOrChildIsSignificant As Boolean

    Public NodeType As NodeTypes

    Public Enum NodeTypes As Short
        Folder = 1
        File = 2
    End Enum

End Class

<Serializable()> _
Public Class DiskTreeRoot

    Public Drives As New List(Of NodeProxy)

    <OptionalField()> _
    Public Wildcards As New List(Of WildcardFilter)

    <OnDeserializing()>
    Private Sub SetWildcardDefault(ByVal sc As StreamingContext)
        Wildcards = New List(Of WildcardFilter)
    End Sub
    
End Class

<Serializable()> _
Public Class WildcardFilter

    Public Wildcard As String = String.Empty

    Public ForInclude As Boolean = True

    ''' <summary>
    ''' Create new object to hold wildcard filter
    ''' </summary>
    ''' <param name="WildcardString">Filter to be applied when searching for wildcards</param>
    ''' <param name="Include">If true, objects matching the filter will be included; if false, they'll be excluded.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal WildcardString As String, ByVal Include As Boolean)
        Wildcard = WildcardString
        ForInclude = Include
    End Sub

End Class
