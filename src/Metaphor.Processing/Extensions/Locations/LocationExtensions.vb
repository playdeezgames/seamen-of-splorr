Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module LocationExtensions
    <Extension>
    Friend Sub Describe(location As ILocation)
        Select Case location.EntitySubtype
            Case LocationSubtypes.SHIP
                DescribeShip(location)
            Case LocationSubtypes.BUBBLE
                DescribeBubble(location)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub
    <Extension>
    Friend Sub Dock(fromLocation As ILocation, toLocation As ILocation)
        fromLocation.SetYoke(Yokes.DOCKED, toLocation.EntityId)
    End Sub
    <Extension>
    Friend Sub Undock(location As ILocation)
        location.ClearYoke(Yokes.DOCKED)
    End Sub
    <Extension>
    Friend Function IsDocked(location As ILocation) As Boolean
        Return location.GetYoke(Yokes.DOCKED).HasValue
    End Function
    <Extension>
    Friend Function GetDocked(location As ILocation) As ILocation
        Return location.World.GetLocation(location.GetYoke(Yokes.DOCKED))
    End Function
End Module
