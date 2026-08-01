Public Class FeatureData
    Inherits InventoriedEntityData
    Public Property LocationId As Guid
    Public Property ItemTypes As New HashSet(Of String)
End Class
