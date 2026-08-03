Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module WorldInitializer
    <Extension>
    Friend Sub Initialize(world As IWorld, context As IInitializationContext)
        world.Clear()
        BubbleInitializer.Initialize(world, context)
        world.CreateLocation(LocationSubtypes.SHIP, "The Blue Ship", "Yer on the Blue Ship.", ShipInitializer.Initialize(context))
        world.AddMessage("Avast!")
        world.Avatar.Look()
    End Sub
End Module
