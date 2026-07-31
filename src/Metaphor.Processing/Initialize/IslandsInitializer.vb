Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module IslandsInitializer
    Friend Sub Initialize(world As IWorld, context As IInitializationContext)
        Dim islandCoordinates = GenerateCoordinates(context)
        Dim islandNames = GenerateNames(context, islandCoordinates.Count)
        Do While islandCoordinates.Count <> 0
            Dim name = islandNames.Dequeue
            Dim coordinate = islandCoordinates.Dequeue
            Dim island = world.CreateLocation(LocationTypes.ISLAND, name, $"This island is called `{name}`.", InitializeIsland(coordinate))
            world.AddIsland(island)
        Loop
    End Sub

    Private Function InitializeIsland(coordinate As (Longitude As Double, Latitude As Double)) As LocationInitializer
        Return Sub(island)
                   island.SetDimension(Dimensions.VISIBILITY, RNG.RollDice("3d8"))
                   island.SetDimension(Dimensions.LONGITUDE, coordinate.Longitude)
                   island.SetDimension(Dimensions.LATITUDE, coordinate.Latitude)
                   island.CreateVerb(VerbTypes.EMBARK, "Embark", "You step onto the ship.")
                   island.CreateShipwright()
                   island.CreateJobBoard()
                   island.InitializeCommodities()
                   island.CreateMarket()
               End Sub
    End Function
    <Extension>
    Private Sub CreateShipwright(island As ILocation)
        island.CreateFeature(FeatureTypes.SHIPWRIGHT, "Shipwright", "A shipwright works on ships. Among other things, they can remove fouling from a vessel.", AddressOf InitializeShipwright)
    End Sub

    Private Sub InitializeShipwright(feature As IFeature)
        feature.CreateVerb(VerbTypes.UNFOUL, "Unfoul ship", "You can pay the shipwright to remove the fouling from yer ship.")
    End Sub

    <Extension>
    Private Sub CreateMarket(island As ILocation)
        island.CreateFeature(FeatureTypes.MARKET, "Market", "A place where you can buy and sell goods.", AddressOf InitializeMarket)
    End Sub

    Private Sub InitializeMarket(market As IFeature)
        market.AddItemType(ItemTypes.HARDTACK)
        market.AddItemType(ItemTypes.BAG_O_GRAIN)
        market.CreateVerb(VerbTypes.BUY, "Buy...", String.Empty)
        market.CreateVerb(VerbTypes.SELL, "Sell...", String.Empty)
        market.CreateVerb(VerbTypes.PRICES, "Prices...", String.Empty)
    End Sub

    <Extension>
    Private Sub InitializeCommodities(island As ILocation)
        For Each commodity In island.World.GetCommodities()
            island.CreateCommodity(commodity.CommodityType, AddressOf InitializeIslandCommodity)
        Next
    End Sub

    Private Sub InitializeIslandCommodity(islandCommodity As IIslandCommodity)
        islandCommodity.Supply = RNG.RollDice("3d6")
        islandCommodity.Demand = RNG.RollDice("3d6")
    End Sub

    Private Function GenerateNames(context As IInitializationContext, count As Integer) As Queue(Of String)
        Dim result As New HashSet(Of String)
        result.Add("Ümläüt")
        While result.Count < count
            result.Add(context.GenerateName())
        End While
        Return New Queue(Of String)(result)
    End Function

    Private Function GenerateCoordinates(context As IInitializationContext) As Queue(Of (Longitude As Double, Latitude As Double))
        Dim result As New List(Of (Longitude As Double, Latitude As Double))
        Do Until Not GenerateCoordinate(result, context, 0)

        Loop
        Return New Queue(Of (Longitude As Double, Latitude As Double))(result)
    End Function

    Private Function GenerateCoordinate(
                                       coordinates As List(Of (Longitude As Double, Latitude As Double)),
                                       context As IInitializationContext,
                                       attempt As Integer) As Boolean
        If attempt >= context.IslandGenerationAttempts Then
            Return False
        End If
        Dim longitude = RNG.FromRange(0.0, context.WorldWidth)
        Dim latitude = RNG.FromRange(0.0, context.WorldHeight)
        If coordinates.All(Function(x) Utility.Distance(x, (longitude, latitude)) >= context.MinimumIslandDistance) Then
            coordinates.Add((longitude, latitude))
            Return True
        End If
        Return GenerateCoordinate(coordinates, context, attempt + 1)
    End Function
End Module
