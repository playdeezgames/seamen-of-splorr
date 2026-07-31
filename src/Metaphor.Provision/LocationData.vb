Public Class LocationData
    Inherits InventoriedEntityData
    Public Property CharacterIds As New HashSet(Of Guid)
    Public Property FeatureIds As New HashSet(Of Guid)
    Public Property IslandCommodities As New Dictionary(Of String, IslandCommodityData)
End Class
