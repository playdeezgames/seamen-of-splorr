Public Class FeatureData
    Inherits MetaphorEntityData
    Public Property LocationId As Guid
    Public Property ItemTypes As New HashSet(Of String)
End Class
