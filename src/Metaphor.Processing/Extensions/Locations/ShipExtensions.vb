Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ShipExtensions
#Region "Describe"
    Friend Sub DescribeShip(ship As ILocation)
        Dim world = ship.World
        world.AddMessage($"Heading: {ship.GetHeading():f2}°")
        world.AddMessage($"Speed: {ship.GetSpeed():f2} knots")
        world.AddMessage($"Depth: {ship.GetDepth():f2} fathoms")
        world.AddMessage($"Hydroplane: {Utility.DescribeHydroplane(ship.GetHydroplane())}")
        world.AddMessage($"Oxygen: {ship.GetOxygen():f2}/{ship.GetMaximumOxygen():f2}")
        world.AddMessage($"Battery: {ship.GetBattery():f2}/{ship.GetMaximumBattery():f2}")
        world.AddMessage($"Diesel: {ship.GetFuel():f2}/{ship.GetMaximumFuel():f2}")
        If ship.IsSnorkelRaised Then
            world.AddMessage($"Snorkel: Raised")
        End If
        ShowVisibleBubbles(world, ship)
        ShowBoundFor(ship)
    End Sub
    Private Sub ShowBoundFor(ship As ILocation)
        Dim boundFor = ship.GetBoundFor()
        If boundFor IsNot Nothing Then
            Dim world = ship.World
            world.AddMessage($"Bound for: {boundFor.GetBubbleName()}")
        End If
    End Sub
    Private Sub ShowVisibleBubbles(world As IWorld, ship As ILocation)
        If ship.IsDocked Then Return
        Dim visibility = ship.GetVisibility()
        Dim visibleBubbles = world.Bubbles.Where(Function(x) x.IsVisibleTo(ship)).OrderBy(Function(x) x.DistanceTo(ship))
        If visibleBubbles.Any Then
            world.AddMessage("Visible Bubbles:")
            For Each visibleBubble In visibleBubbles
                world.Avatar.AddKnownBubble(visibleBubble)
                world.AddMessage($"- {visibleBubble.GetBubbleName()}(Distance: {visibleBubble.DistanceTo(ship):f2}nm, Heading: {ship.HeadingTo(visibleBubble):f2}°, Depth: {visibleBubble.GetDepth():f2}ftm)")
            Next
        End If
    End Sub
#End Region
#Region "Dimensions"
    <Extension>
    Friend Function GetHydroplane(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.HYDROPLANE)
    End Function
    <Extension>
    Friend Function GetLongitude(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.LONGITUDE)
    End Function
    <Extension>
    Friend Sub SetLongitude(ship As ILocation, longitude As Double)
        ship.SetDimension(Dimensions.LONGITUDE, longitude)
    End Sub
    <Extension>
    Friend Function GetLatitude(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.LATITUDE)
    End Function
    <Extension>
    Friend Sub SetDepth(ship As ILocation, depth As Double)
        ship.SetDimension(Dimensions.DEPTH, depth)
    End Sub
    <Extension>
    Friend Sub SetLatitude(ship As ILocation, latitude As Double)
        ship.SetDimension(Dimensions.LATITUDE, latitude)
    End Sub
    <Extension>
    Friend Function GetHeading(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.HEADING)
    End Function
    <Extension>
    Friend Function GetSpeed(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.SPEED)
    End Function
    <Extension>
    Friend Function GetDepth(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.DEPTH)
    End Function
    <Extension>
    Friend Function GetOxygen(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.OXYGEN)
    End Function
    <Extension>
    Friend Function GetBattery(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.BATTERY)
    End Function
    <Extension>
    Friend Function GetFuel(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.FUEL)
    End Function
    <Extension>
    Friend Function GetMaximumOxygen(ship As ILocation) As Double
        Return ship.GetDimensionMaximum(Dimensions.OXYGEN)
    End Function
    <Extension>
    Friend Sub ReplenishOxygen(ship As ILocation)
        ship.SetDimension(Dimensions.OXYGEN, ship.GetMaximumOxygen())
    End Sub
    <Extension>
    Friend Function GetMaximumBattery(ship As ILocation) As Double
        Return ship.GetDimensionMaximum(Dimensions.BATTERY)
    End Function
    <Extension>
    Friend Function GetMaximumFuel(ship As ILocation) As Double
        Return ship.GetDimensionMaximum(Dimensions.FUEL)
    End Function
    <Extension>
    Friend Sub SetHeading(ship As ILocation, heading As Double)
        ship.SetDimension(Dimensions.HEADING, heading)
    End Sub
    <Extension>
    Friend Sub SetHydroplane(ship As ILocation, hydroplane As Double)
        ship.SetDimension(Dimensions.HYDROPLANE, hydroplane)
    End Sub
    <Extension>
    Friend Sub SetSpeed(ship As ILocation, speed As Double)
        ship.SetDimension(Dimensions.SPEED, speed)
    End Sub
#End Region
#Region "Tags"
    <Extension>
    Friend Function IsSnorkelRaised(ship As ILocation) As Boolean
        Return ship.HasTag(Tags.SNORKEL_RAISED)
    End Function
    <Extension>
    Friend Function IsAtSnorkelDepth(ship As ILocation) As Boolean
        Return ship.IsDimensionMinimum(Dimensions.DEPTH)
    End Function
#End Region
#Region "Features"
    <Extension>
    Friend Function GetCargoHold(ship As ILocation) As IFeature
        Return ship.Features.Single(Function(x) x.EntitySubtype = FeatureSubtypes.CARGO_HOLD)
    End Function
#End Region
#Region "Heading For"
    <Extension>
    Friend Sub HeadFor(ship As ILocation, bubble As ILocation)
        If bubble IsNot Nothing Then
            ship.SetYoke(Yokes.BOUND_FOR, bubble.EntityId)
        Else
            ship.ClearYoke(Yokes.BOUND_FOR)
        End If
    End Sub
    <Extension>
    Friend Function GetBoundFor(ship As ILocation) As ILocation
        Return ship.World.GetLocation(ship.GetYoke(Yokes.BOUND_FOR))
    End Function
#End Region
End Module
