Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module IslandExtensions
    Friend Sub DescribeIsland(island As ILocation)
        Dim world = island.World
        world.AddMessage($"Island: {island.Name}")
    End Sub

    <Extension>
    Function IsVisibleTo(fromLocation As ILocation, toLocation As ILocation) As Boolean
        Return fromLocation.DistanceTo(toLocation) <= Math.Min(fromLocation.GetVisibility(), toLocation.GetVisibility())
    End Function
    <Extension>
    Function DistanceTo(fromLocation As ILocation, toLocation As ILocation) As Double
        Return Utility.Distance(
            (fromLocation.GetLongitude(), fromLocation.GetLatitude()),
            (toLocation.GetLongitude(), toLocation.GetLatitude()))
    End Function
    <Extension>
    Function HeadingTo(fromLocation As ILocation, toLocation As ILocation) As Double
        Dim deltaX = toLocation.GetLongitude() - fromLocation.GetLongitude()
        Dim deltaY = toLocation.GetLatitude() - fromLocation.GetLatitude()
        Dim heading = Math.Atan2(deltaY, deltaX) * 360.0 / Math.PI / 2
        Return If(heading < 0.0, heading + 360.0, heading)
    End Function
    <Extension>
    Friend Function GetIslandName(island As ILocation) As String
        Return If(island.HasTag(Tags.KNOWN), island.Name, "UNKNOWN ISLAND")
    End Function
    <Extension>
    Friend Sub CreateJobBoard(island As ILocation)
        island.CreateFeature(FeatureTypes.JOB_BOARD, "Job Board", "Here are listed various errand person jobs for making a small amount of jools.", AddressOf InitializeJobBoard)
    End Sub
    Private Sub InitializeJobBoard(feature As IFeature)
        feature.CreateVerb(VerbTypes.ACCEPT_DELIVERY, "Take Delivery Assignment", "Desperate for jools, you will take whatever whereever!")
    End Sub
    <Extension>
    Friend Function CreateRecipient(island As ILocation) As ICharacter
        Dim characterName As String = GenerateName(island)
        Return island.CreateCharacter(CharacterTypes.RECIPIENT, characterName, "They/Them", $"This is {characterName} of {island.Name}.", AddressOf InitializeRecipient)
    End Function

    Private Sub InitializeRecipient(character As ICharacter)
        character.CreateVerb(VerbTypes.DELIVER_PACKAGE, "Deliver Package", "You deliver the package, right in their package delivery hole.")
    End Sub

    Private Function GenerateName(island As ILocation) As String
        Return "Nacho Mama"
    End Function
    <Extension>
    Friend Function GetMarket(island As ILocation) As IFeature
        Return island.Features.SingleOrDefault(Function(x) x.EntityType = FeatureTypes.MARKET)
    End Function
    <Extension>
    Friend Function GetUnfoulingPrice(island As ILocation) As Double
        Dim fouling = island.World.Avatar.Ship.GetDimension(Dimensions.FOULING)
        Dim labor = fouling * UNFOULING_LABOR_FACTOR
        Return island.IslandCommodities(CommodityTypes.LABOUR).MarketPrice * labor
    End Function
End Module
