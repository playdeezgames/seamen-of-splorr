Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module LocationVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, location As ILocation, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbTypes.MOVE, AddressOf CanMove},
            {VerbTypes.DOCK, AddressOf CanDock},
            {VerbTypes.SET_HEADING, AddressOf CanSetHeading},
            {VerbTypes.SET_SPEED, AddressOf CanSetSpeed},
            {VerbTypes.SET_HYDROPLANE, AddressOf CanSetHydroplane},
            {VerbTypes.UNDOCK, AddressOf CanUndock},
            {VerbTypes.DISEMBARK, AddressOf CanDisembark},
            {VerbTypes.EMBARK, AddressOf CanEmbark}
        }

    Private Function CanSetHydroplane(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsMoored
    End Function

    Private Function CanEmbark(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Return location.Features.Any(Function(x) x.EntityType = FeatureTypes.MOORINGS)
    End Function

    Private Function CanDisembark(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Return location.Features.Any(Function(x) x.EntityType = FeatureTypes.MOORINGS)
    End Function

    Private Function CanUndock(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return ship.IsMoored
    End Function

    Private Function CanSetSpeed(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsMoored
    End Function

    Private Function CanSetHeading(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsMoored
    End Function

    Private Function CanDock(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsMoored AndAlso verb.World.Bubbles.Any(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE)
    End Function

    Private Function CanMove(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsMoored AndAlso ship.GetSpeed() > SPEED_FULL_STOP
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntityType, handler) Then
            Return handler.Invoke(verb, location, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbTypes.SET_HEADING, AddressOf HandleSetHeading},
            {VerbTypes.SET_SPEED, AddressOf HandleSetSpeed},
            {VerbTypes.SET_HYDROPLANE, AddressOf HandleSetHydroplane},
            {VerbTypes.MOVE, AddressOf HandleMove},
            {VerbTypes.DOCK, AddressOf HandleDock},
            {VerbTypes.UNDOCK, AddressOf HandleUndock},
            {VerbTypes.EMBARK, AddressOf HandleEmbark},
            {VerbTypes.DISEMBARK, AddressOf HandleDisembark}
        }

    Private Sub HandleSetHydroplane(verb As IVerb, location As ILocation, actor As ICharacter)
        verb.World.Avatar.SetMode(Modes.SETTING_HYDROPLANE)
    End Sub

    Private Sub HandleDisembark(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = location.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).GetDestination()
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleEmbark(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = location.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).GetDestination()
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleUndock(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim island = ship.Features.Single(Function(x) x.EntityType = FeatureTypes.MOORINGS).GetDestination()
        island.RemoveMoorings()
        ship.RemoveMoorings()
    End Sub

    Private Sub HandleDock(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim island = verb.World.Bubbles.Single(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE)
        ship.MoorTo(island, "Disembark")
        island.MoorTo(ship, "Embark")
        island.SetTag(Tags.KNOWN)
        verb.World.Avatar.AddKnownIsland(island)
    End Sub

    Private Sub HandleMove(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim ship = avatar.GetShip()
        Dim speed = ship.GetSpeed()
        Dim fouling = speed * speed
        Dim radians = ship.GetHeading() * Math.PI * 2 / HEADING_MAXIMUM
        Dim deltaLongitude = speed * Math.Cos(radians)
        Dim deltaLatitude = speed * Math.Sin(radians)
        Dim nextLongitude = ship.GetLongitude() + deltaLongitude
        Dim nextLatitude = ship.GetLatitude() + deltaLatitude
        ship.SetLongitude(nextLongitude)
        ship.SetLatitude(nextLatitude)
        avatar.DoBiology(1)
        avatar.Look()
    End Sub

    Private Sub HandleSetSpeed(verb As IVerb, location As ILocation, actor As ICharacter)
        verb.World.Avatar.SetMode(Modes.SETTING_SPEED)
    End Sub

    Private Sub HandleSetHeading(verb As IVerb, location As ILocation, actor As ICharacter)
        verb.World.Avatar.SetMode(Modes.SETTING_HEADING)
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntityType, handler) Then
            handler.Invoke(verb, location, actor)
            Return
        End If
    End Sub

End Module
