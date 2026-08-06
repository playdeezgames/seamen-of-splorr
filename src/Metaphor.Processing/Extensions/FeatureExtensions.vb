Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FeatureExtensions
    <Extension>
    Friend Sub Examine(feature As IFeature)
        Dim world = feature.World
        world.ClearMessages()
        Dim character = world.Avatar
        world.AddMessage($"{character.Name} interacts with {feature.Name}.")
        world.AddMessage(feature.Flavor)
        Dim itemStacks = feature.Inventory.ItemStacks
        If itemStacks.Any Then
            world.AddMessage("Item Stacks:")
            For Each itemStack In itemStacks
                world.AddMessage($"- {itemStack.Top.Name}(x{itemStack.Count})")
            Next
        End If
    End Sub
    <Extension>
    Friend Function IsCargoHold(feature As IFeature) As Boolean
        Return feature.EntitySubtype = FeatureSubtypes.CARGO_HOLD
    End Function
    <Extension>
    Friend Function GetItemTypeName(market As IFeature, itemType As String) As String
        Return InventoryExtensions.GetItemTypeName(itemType)
    End Function
    <Extension>
    Friend Sub SetDestination(feature As IFeature, location As ILocation)
        feature.SetYoke(Yokes.DESTINATION, location.EntityId)
    End Sub
    <Extension>
    Friend Function GetDestination(feature As IFeature) As ILocation
        Return feature.World.GetLocation(feature.GetYoke(Yokes.DESTINATION))
    End Function
    <Extension>
    Friend Function GetFuelPrice(feature As IFeature) As Double
        Return feature.GetDimension(Dimensions.FUEL_PRICE)
    End Function
End Module
