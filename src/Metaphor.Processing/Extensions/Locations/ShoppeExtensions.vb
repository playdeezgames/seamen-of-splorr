Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ShoppeExtensions
    <Extension>
    Friend Sub CreateShoppe(bubble As ILocation)
        bubble.CreateFeature(FeatureSubtypes.SHOPPE, "Shoppe", "This is the place where you can buy various sundries.", AddressOf InitializeShoppe)
    End Sub
    Private Sub InitializeShoppe(feature As IFeature)
    End Sub
End Module
