Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ShipExtensions

    Friend Sub DescribeShip(ship As ILocation)
        Dim world = ship.World
        world.AddMessage($"Heading: {ship.GetHeading():f2}")
        world.AddMessage($"Speed: {ship.GetSpeed():f2}")
        world.AddMessage($"Fouling: {ship.GetFoulingPercent():f2}%")
        ShowVisibleIslands(world, ship)
    End Sub

    Private Sub ShowVisibleIslands(world As IWorld, ship As ILocation)
        If ship.IsMoored Then Return
        Dim visibility = ship.GetVisibility()
        Dim visibleIslands = world.Islands.Where(Function(x) x.IsVisibleTo(ship)).OrderBy(Function(x) x.DistanceTo(ship))
        If visibleIslands.Any Then
            world.AddMessage("Visible Islands:")
            For Each visibleIsland In visibleIslands
                world.Avatar.AddKnownIsland(visibleIsland)
                world.AddMessage($"- {visibleIsland.GetIslandName()}(Distance: {visibleIsland.DistanceTo(ship):f2}, Heading: {ship.HeadingTo(visibleIsland):f2})")
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
    Friend Function GetFoulingPercent(ship As ILocation) As Double
        Return ship.GetDimension(Dimensions.FOULING) * 100.0 / ship.GetDimensionMaximum(Dimensions.FOULING)
    End Function
    <Extension>
    Friend Sub SetHeading(ship As ILocation, heading As Double)
        ship.SetDimension(Dimensions.HEADING, heading)
    End Sub
    <Extension>
    Friend Sub SetSpeed(ship As ILocation, speed As Double)
        ship.SetDimension(Dimensions.SPEED, speed)
    End Sub
    <Extension>
    Friend Function IsMoored(ship As ILocation) As Boolean
        Return ship.Features.Any(Function(x) x.EntityType = FeatureTypes.MOORINGS)
    End Function
    <Extension>
    Friend Function GetCargoHold(ship As ILocation) As IFeature
        Return ship.Features.Single(Function(x) x.EntityType = FeatureTypes.CARGO_HOLD)
    End Function
End Module
