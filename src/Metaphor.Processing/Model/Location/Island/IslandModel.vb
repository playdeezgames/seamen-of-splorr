Imports Metaphor.Persistence

Friend Class IslandModel
    Implements IIslandModel

    Private ReadOnly island As ILocation

    Private Sub New(island As ILocation)
        Me.island = island
    End Sub

    Public ReadOnly Property Name As String Implements IIslandModel.Name
        Get
            Dim ship = island.World.Avatar.Ship
            Return $"{island.GetIslandName()}(Distance: {island.DistanceTo(ship):f2}, Heading: {ship.HeadingTo(island):f2})"
        End Get
    End Property

    Public Sub SetHeadingFor() Implements IIslandModel.SetHeadingFor
        Dim world = island.World
        Dim avatar = world.Avatar
        avatar.ClearTag(Tags.CHOOSING_KNOWN_ISLAND)
        Dim ship = avatar.Ship
        ship.SetHeading(ship.HeadingTo(island))
        world.AddMessage($"{avatar.Name} heads for {island.GetIslandName()} by setting a heading of {ship.GetHeading():f2}.")
        avatar.Look()
    End Sub

    Friend Shared Function Create(island As ILocation) As IIslandModel
        Return New IslandModel(island)
    End Function
End Class
