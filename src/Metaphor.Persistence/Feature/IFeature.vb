Public Delegate Sub FeatureInitializer(feature As IFeature)
Public Interface IFeature
    Inherits IInventoriedEntity
    ReadOnly Property Location As ILocation
    Property Destination As ILocation
    Sub AddItemType(itemType As String)
    ReadOnly Property ItemTypes As IEnumerable(Of String)
End Interface
