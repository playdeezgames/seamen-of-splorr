Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ShipExtensions

    Friend Sub DescribeShip(ship As ILocation)
        Dim world = ship.World
        world.AddMessage($"Heading: {ship.GetHeading():f2}°")
        world.AddMessage($"Speed: {ship.GetSpeed():f2} knots")
        world.AddMessage($"Depth: {ship.GetDepth():f2} fathoms")
        world.AddMessage($"Hydroplane: {Utility.DescribeHydroplane(ship.GetHydroplane())}")
        world.AddMessage($"Oxygen: {ship.GetOxygen()}/{ship.GetMaximumOxygen()}")
        world.AddMessage($"Battery: {ship.GetBattery():f2}/{ship.GetMaximumBattery():f2}")
        world.AddMessage($"Diesel: {ship.GetDiesel():f2}/{ship.GetMaximumDiesel():f2}")
        If ship.IsSnorkelRaised Then
            world.AddMessage($"Snorkel: Raised")
        End If
        ShowVisibleIslands(world, ship)
    End Sub
    <Extension>
    Friend Function GetHydroplane(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.HYDROPLANE)
    End Function

    Private Sub ShowVisibleIslands(world As IWorld, ship As ILocation)
        If ship.IsMoored Then Return
        Dim visibility = ship.GetVisibility()
        Dim visibleIslands = world.Bubbles.Where(Function(x) x.IsVisibleTo(ship)).OrderBy(Function(x) x.DistanceTo(ship))
        If visibleIslands.Any Then
            world.AddMessage("Visible Islands:")
            For Each visibleIsland In visibleIslands
                world.Avatar.AddKnownIsland(visibleIsland)
                world.AddMessage($"- {visibleIsland.GetBubbleName()}(Distance: {visibleIsland.DistanceTo(ship):f2}nm, Heading: {ship.HeadingTo(visibleIsland):f2}°, Depth: {visibleIsland.GetDepth():f2}ftm)")
            Next
        End If
    End Sub

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
    Friend Function GetOxygen(ship As ILocation) As Integer
        Return ship.GetCounter(Counters.OXYGEN)
    End Function
    <Extension>
    Friend Function GetBattery(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.BATTERY)
    End Function
    <Extension>
    Friend Function GetDiesel(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.DIESEL)
    End Function
    <Extension>
    Friend Function GetMaximumOxygen(ship As ILocation) As Integer
        Return ship.GetCounterMaximum(Counters.OXYGEN)
    End Function
    <Extension>
    Friend Function GetMaximumBattery(ship As ILocation) As Double
        Return ship.GetDimensionMaximum(Dimensions.BATTERY)
    End Function
    <Extension>
    Friend Function GetMaximumDiesel(ship As ILocation) As Double
        Return ship.GetDimensionMaximum(Dimensions.DIESEL)
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
    <Extension>
    Friend Function IsMoored(ship As ILocation) As Boolean
        Return ship.Features.Any(Function(x) x.EntitySubtype = FeatureSubtypes.MOORINGS)
    End Function
    <Extension>
    Friend Function GetCargoHold(ship As ILocation) As IFeature
        Return ship.Features.Single(Function(x) x.EntitySubtype = FeatureSubtypes.CARGO_HOLD)
    End Function
    <Extension>
    Friend Function IsSnorkelRaised(ship As ILocation) As Boolean
        Return ship.HasTag(Tags.SNORKEL_RAISED)
    End Function
    <Extension>
    Friend Function IsAtSnorkelDepth(ship As ILocation) As Boolean
        Return ship.IsDimensionMinimum(Dimensions.DEPTH)
    End Function
End Module
