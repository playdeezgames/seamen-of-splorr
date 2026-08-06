Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FuelingStationExtensions
    <Extension>
    Sub CreateFuelingStation(bubble As ILocation)
        bubble.CreateFeature(FeatureSubtypes.FUELING_STATION, "Fueling Station", "This is a place where you can buy fuel.", AddressOf InitializeFuelingStation)
    End Sub
    Private Sub InitializeFuelingStation(feature As IFeature)
    End Sub
End Module
