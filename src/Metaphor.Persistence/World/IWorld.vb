Imports TGGD.Persistence

Public Interface IWorld
    Inherits IEntity
    Function Save(filename As String) As Task
    ReadOnly Property Messages As IEnumerable(Of IMessage)
    Sub ClearMessages()
    Sub AddMessage(text As String, Optional hints As IDictionary(Of String, String) = Nothing)
    Function CreateLocation(locationType As String, name As String, flavor As String, Optional initializer As LocationInitializer = Nothing) As ILocation
    Property Avatar As ICharacter
    Property AdFinish As DateTimeOffset?
    Sub AddIsland(island As ILocation)
    ReadOnly Property Islands As IEnumerable(Of ILocation)
    Function CreateCommodity(commodityType As String, name As String, Optional initializer As CommodityInitializer = Nothing) As ICommodity
    ReadOnly Property Commodities(commodityType As String) As ICommodity
    ReadOnly Property CommodityIds As IEnumerable(Of String)
End Interface
