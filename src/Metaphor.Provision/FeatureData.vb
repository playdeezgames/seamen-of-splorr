Imports TGGD.Provision

Public Class FeatureData
    Inherits EntityData
    Public Property LocationId As Guid
    Public Property ItemTypes As New HashSet(Of String)
End Class
