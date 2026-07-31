Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module LocationVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, location As ILocation) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, location As ILocation)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbTypes.MOVE, AddressOf CanMove},
            {VerbTypes.DOCK, AddressOf CanDock},
            {VerbTypes.SET_HEADING, AddressOf CanSetHeading},
            {VerbTypes.SET_SPEED, AddressOf CanSetSpeed},
            {VerbTypes.UNDOCK, AddressOf CanUndock},
            {VerbTypes.DISEMBARK, AddressOf CanDisembark},
            {VerbTypes.EMBARK, AddressOf CanEmbark}
        }

    Private Function CanEmbark(verb As IVerb, location As ILocation) As Boolean
        Return location.Features.Any(Function(x) x.EntityType = FeatureTypes.MOORINGS)
    End Function

    Private Function CanDisembark(verb As IVerb, location As ILocation) As Boolean
        Return location.Features.Any(Function(x) x.EntityType = FeatureTypes.MOORINGS)
    End Function

    Private Function CanUndock(verb As IVerb, ship As ILocation) As Boolean
        Return ship.IsMoored
    End Function

    Private Function CanSetSpeed(verb As IVerb, ship As ILocation) As Boolean
        Return Not ship.IsMoored
    End Function

    Private Function CanSetHeading(verb As IVerb, ship As ILocation) As Boolean
        Return Not ship.IsMoored
    End Function

    Private Function CanDock(verb As IVerb, ship As ILocation) As Boolean
        Return Not ship.IsMoored AndAlso verb.World.Islands.Any(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE)
    End Function

    Private Function CanMove(verb As IVerb, ship As ILocation) As Boolean
        Return Not ship.IsMoored AndAlso ship.GetSpeed() > SPEED_FULL_STOP
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, location As ILocation) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntityType, handler) Then
            Return handler.Invoke(verb, location)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbTypes.SET_HEADING, AddressOf HandleSetHeading},
            {VerbTypes.SET_SPEED, AddressOf HandleSetSpeed},
            {VerbTypes.MOVE, AddressOf HandleMove},
            {VerbTypes.DOCK, AddressOf HandleDock},
            {VerbTypes.UNDOCK, AddressOf HandleUndock},
            {VerbTypes.EMBARK, AddressOf HandleEmbark},
            {VerbTypes.DISEMBARK, AddressOf HandleDisembark}
        }

    Private Sub HandleDisembark(verb As IVerb, location As ILocation)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = location.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).Destination
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleEmbark(verb As IVerb, location As ILocation)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = location.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).Destination
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleUndock(verb As IVerb, ship As ILocation)
        Dim island = ship.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).Destination
        island.RemoveMoorings()
        ship.RemoveMoorings()
    End Sub

    Private Sub HandleDock(verb As IVerb, ship As ILocation)
        Dim island = verb.World.Islands.Single(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE)
        ship.MoorTo(island, "Disembark")
        island.MoorTo(ship, "Embark")
        island.SetTag(Tags.KNOWN)
        verb.World.Avatar.AddKnownIsland(island)
    End Sub

    Private Sub HandleMove(verb As IVerb, location As ILocation)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim ship = avatar.Ship
        Dim speed = ship.GetSpeed()
        Dim fouling = speed * speed
        Dim speedMultiplier = 1.0 - ship.GetDimension(Dimensions.FOULING) / ship.GetDimensionMaximum(Dimensions.FOULING) / 2
        ship.ChangeDimension(Dimensions.FOULING, fouling)
        Dim radians = ship.GetHeading() * Math.PI * 2 / HEADING_MAXIMUM
        Dim deltaLongitude = speed * Math.Cos(radians) * speedMultiplier
        Dim deltaLatitude = speed * Math.Sin(radians) * speedMultiplier
        Dim nextLongitude = ship.GetLongitude() + deltaLongitude
        Dim nextLatitude = ship.GetLatitude() + deltaLatitude
        ship.SetLongitude(nextLongitude)
        ship.SetLatitude(nextLatitude)
        avatar.DoBiology(1)
        avatar.Look()
    End Sub

    Private Sub HandleSetSpeed(verb As IVerb, location As ILocation)
        verb.World.Avatar.Ship.SetTags(Tags.SETTING_SPEED)
    End Sub

    Private Sub HandleSetHeading(verb As IVerb, location As ILocation)
        verb.World.Avatar.Ship.SetTags(Tags.SETTING_HEADING)
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, location As ILocation)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntityType, handler) Then
            handler.Invoke(verb, location)
            Return
        End If
    End Sub

End Module
