Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module WorldInitializer
    <Extension>
    Friend Sub Initialize(world As IWorld, context As IInitializationContext)
        world.Clear()
        world.CreateCommodity(CommodityTypes.GRAIN, "Grain", AddressOf InitializeGrain)
        world.CreateCommodity(CommodityTypes.LABOUR, "Labour", AddressOf InitializeLabour)
        IslandsInitializer.Initialize(world, context)
        world.CreateLocation(LocationTypes.SHIP, "The Blue Ship", "Yer on the Blue Ship.", ShipInitializer.Initialize(context))
        world.AddMessage("Avast!")
        world.Avatar.Look()
    End Sub

    Private Sub InitializeLabour(commodity As ICommodity)
        commodity.BasePrice = 5.0
        commodity.SupplyFactor = 0.01
        commodity.DemandFactor = 0.005
    End Sub

    Private Sub InitializeGrain(commodity As ICommodity)
        commodity.BasePrice = 1.0
        commodity.SupplyFactor = 0.01
        commodity.DemandFactor = 0.005
    End Sub
End Module
