Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module LocationVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, location As ILocation, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbSubtypes.MOVE, AddressOf CanMove},
            {VerbSubtypes.DOCK, AddressOf CanDock},
            {VerbSubtypes.SET_HEADING, AddressOf CanSetHeading},
            {VerbSubtypes.SET_SPEED, AddressOf CanSetSpeed},
            {VerbSubtypes.SET_HYDROPLANE, AddressOf CanSetHydroplane},
            {VerbSubtypes.UNDOCK, AddressOf CanUndock},
            {VerbSubtypes.DISEMBARK, AddressOf CanDisembark},
            {VerbSubtypes.EMBARK, AddressOf CanEmbark},
            {VerbSubtypes.RAISE_SNORKEL, AddressOf CanRaiseSnorkel},
            {VerbSubtypes.LOWER_SNORKEL, AddressOf CanLowerSnorkel},
            {VerbSubtypes.CHARGE_BATTERY, AddressOf CanChargeBatteries}
        }

    Private Function CanChargeBatteries(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Return Not location.IsDimensionMinimum(Dimensions.FUEL) AndAlso Not location.IsDimensionMinimum(Dimensions.OXYGEN)
    End Function

    Private Function CanLowerSnorkel(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return ship.IsSnorkelRaised()
    End Function

    Private Function CanRaiseSnorkel(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso ship.IsAtSnorkelDepth()
    End Function

    Private Function CanSetHydroplane(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso Not ship.IsDocked()
    End Function

    Private Function CanEmbark(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Return location.IsDocked()
    End Function

    Private Function CanDisembark(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Return Not location.IsSnorkelRaised() AndAlso location.IsDocked()
    End Function

    Private Function CanUndock(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso ship.IsDocked()
    End Function

    Private Function CanSetSpeed(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso Not ship.IsDocked()
    End Function

    Private Function CanSetHeading(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso Not ship.IsDocked()
    End Function

    Private Function CanDock(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso
            Not ship.IsDocked() AndAlso
            verb.World.Bubbles.Any(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE AndAlso x.DepthDifference(ship) <= MAXIMUM_DEPTH_DIFFERENCE)
    End Function

    Private Function CanMove(verb As IVerb, ship As ILocation, actor As ICharacter) As Boolean
        Return Not ship.IsSnorkelRaised() AndAlso Not ship.IsDocked() AndAlso ship.GetSpeed() > SPEED_FULL_STOP AndAlso ship.GetBattery() > 0.0
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, location As ILocation, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, location, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbSubtypes.SET_HEADING, AddressOf HandleSetHeading},
            {VerbSubtypes.SET_SPEED, AddressOf HandleSetSpeed},
            {VerbSubtypes.SET_HYDROPLANE, AddressOf HandleSetHydroplane},
            {VerbSubtypes.MOVE, AddressOf HandleMove},
            {VerbSubtypes.DOCK, AddressOf HandleDock},
            {VerbSubtypes.UNDOCK, AddressOf HandleUndock},
            {VerbSubtypes.EMBARK, AddressOf HandleEmbark},
            {VerbSubtypes.DISEMBARK, AddressOf HandleDisembark},
            {VerbSubtypes.RAISE_SNORKEL, AddressOf HandleRaiseSnorkel},
            {VerbSubtypes.LOWER_SNORKEL, AddressOf HandleLowerSnorkel},
            {VerbSubtypes.CHARGE_BATTERY, AddressOf HandleChargeBattery}
        }

    Private Sub HandleChargeBattery(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim charge = {
            location.GetDimension(Dimensions.ENGINE),
            location.GetOxygen(),
            location.GetFuel()}.Min()
        If charge > 0.0 Then
            Dim world = actor.World
            location.ChangeDimension(Dimensions.BATTERY, charge)
            world.AddMessage($"Battery is now: {location.GetBattery():f2}/{location.GetMaximumBattery():f2}")
            location.ChangeDimension(Dimensions.FUEL, -charge)
            world.AddMessage($"Fuels is now: {location.GetFuel():f2}/{location.GetMaximumFuel():f2}")
            If Not location.IsSnorkelRaised And Not location.IsDocked Then
                location.ChangeDimension(Dimensions.OXYGEN, -charge)
                world.AddMessage($"Oxygen is now: {location.GetOxygen():f2}/{location.GetMaximumOxygen():f2}")
            End If
        End If
    End Sub

    Private Sub HandleLowerSnorkel(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim world = verb.World
        world.AddMessage($"{actor.Name} lowers the snorkel.")
        ship.ClearTag(Tags.SNORKEL_RAISED)
    End Sub

    Private Sub HandleRaiseSnorkel(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim world = verb.World
        world.AddMessage($"{actor.Name} raises the snorkel.")
        ship.ReplenishOxygen()
        ship.SetTag(Tags.SNORKEL_RAISED)
    End Sub

    Private Sub HandleSetHydroplane(verb As IVerb, location As ILocation, actor As ICharacter)
        verb.World.Avatar.SetMode(Modes.SETTING_HYDROPLANE)
    End Sub

    Private Sub HandleDisembark(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = fromLocation.GetDocked()
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleEmbark(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim fromLocation = avatar.Location
        Dim destination = fromLocation.GetDocked()
        avatar.Location = destination
        world.AddMessage($"{avatar.Name} moves from {fromLocation.Name} to {destination.Name}.")
        avatar.Look()
    End Sub

    Private Sub HandleUndock(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim bubble = ship.GetDocked()
        bubble.Undock()
        ship.Undock()
    End Sub

    Private Sub HandleDock(verb As IVerb, ship As ILocation, actor As ICharacter)
        Dim bubble = verb.World.Bubbles.Single(Function(x) x.DistanceTo(ship) <= DOCKING_DISTANCE)
        ship.ReplenishOxygen()
        ship.Dock(bubble)
        bubble.Dock(ship)
        bubble.SetTag(Tags.KNOWN)
        verb.World.Avatar.AddKnownBubble(bubble)
    End Sub

    Private Sub HandleMove(verb As IVerb, location As ILocation, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim ship = avatar.GetShip()
        Dim speed = Math.Min(ship.GetSpeed(), ship.GetBattery())
        Dim headingRadians = Utility.ToRadians(ship.GetHeading())
        Dim bubbleRadians = Utility.ToRadians(ship.GetHydroplane())
        Dim deltaLongitude = speed * Math.Cos(headingRadians)
        Dim deltaLatitude = speed * Math.Sin(headingRadians)
        Dim deltaDepth = speed * Math.Sin(bubbleRadians) * Grimoire.FTM_PER_NM
        Dim nextLongitude = ship.GetLongitude() + deltaLongitude
        Dim nextLatitude = ship.GetLatitude() + deltaLatitude
        Dim nextDepth = ship.GetDepth() + deltaDepth
        ship.SetLongitude(nextLongitude)
        ship.SetLatitude(nextLatitude)
        ship.SetDepth(nextDepth)
        ship.ChangeDimension(Dimensions.BATTERY, -speed)
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
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, location, actor)
            Return
        End If
    End Sub

End Module
