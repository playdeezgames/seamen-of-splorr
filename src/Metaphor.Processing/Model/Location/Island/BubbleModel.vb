Imports Metaphor.Persistence

Friend Class BubbleModel
    Implements IBubbleModel

    Private ReadOnly bubble As ILocation

    Private Sub New(bubble As ILocation)
        Me.bubble = bubble
    End Sub

    Public ReadOnly Property Name As String Implements IBubbleModel.Name
        Get
            Dim ship = bubble.World.Avatar.GetShip()
            Return $"{bubble.GetBubbleName()}(Distance: {bubble.DistanceTo(ship):f2}nm, Heading: {ship.HeadingTo(bubble):f2}°, Depth: {bubble.GetDepth():f2}ftm)"
        End Get
    End Property

    Public Sub SetHeadingFor() Implements IBubbleModel.SetHeadingFor
        Dim world = bubble.World
        Dim avatar = world.Avatar
        avatar.SetMode(Nothing)
        Dim ship = avatar.GetShip()
        ship.SetHeading(ship.HeadingTo(bubble))
        Dim depthDelta = bubble.GetDepth() - ship.GetDepth()
        Dim distance = bubble.DistanceTo(ship)
        ship.SetHydroplane(Utility.ToDegrees(Math.Asin(depthDelta / distance / Grimoire.FTM_PER_NM)))
        world.AddMessage($"{avatar.Name} heads for {bubble.GetBubbleName()} by setting a heading of {ship.GetHeading():f2}.")
        avatar.Look()
    End Sub

    Friend Shared Function Create(bubble As ILocation) As IBubbleModel
        Return New BubbleModel(bubble)
    End Function
End Class
