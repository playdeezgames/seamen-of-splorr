Imports TGGD.Provision

Public Class WorldData
    Inherits EntityData
    Public Property Messages As New List(Of MessageData)
    Public Property Locations As New Dictionary(Of Guid, LocationData)
    Public Property AvatarId As Guid?
    Public Property Inventories As New Dictionary(Of Guid, InventoryData)
    Public Property Entities As New Dictionary(Of Guid, EntityData)
    Public Property Verbs As New Dictionary(Of Guid, EntityData)
    Public Property AdFinishes As DateTimeOffset?
    Public Property BubbleIds As New HashSet(Of Guid)
End Class
