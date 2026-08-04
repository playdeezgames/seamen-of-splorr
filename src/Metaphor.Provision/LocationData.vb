Imports TGGD.Provision

Public Class LocationData
    Inherits EntityData
    Public Property CharacterIds As New HashSet(Of Guid)
    Public Property FeatureIds As New HashSet(Of Guid)
End Class
