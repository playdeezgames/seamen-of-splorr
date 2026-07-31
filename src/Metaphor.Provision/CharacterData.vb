Public Class CharacterData
    Inherits InventoriedEntityData
    Public Property LocationId As Guid
    Public Property Pronouns As String
    Public Property ShipId As Guid?
    Public Property KnownIslandIds As New HashSet(Of Guid)
End Class
