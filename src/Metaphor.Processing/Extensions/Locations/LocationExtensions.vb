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
    Friend Sub MoorTo(fromLocation As ILocation, toLocation As ILocation, verbName As String)
        Dim moorings = fromLocation.CreateFeature(FeatureSubtypes.MOORINGS, $"Moorings to {toLocation.Name}", $"Lines securely fasten {fromLocation.Name} to {toLocation.Name}.")
        moorings.SetDestination(toLocation)
    End Sub
    <Extension>
    Friend Sub RemoveMoorings(location As ILocation)
        location.Features.Single(Function(x) x.EntitySubtype = FeatureSubtypes.MOORINGS).Remove()
    End Sub
End Module
