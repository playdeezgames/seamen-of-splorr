Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module BubbleInitializer
    Friend Sub Initialize(world As IWorld, context As IInitializationContext)
        Dim islandCoordinates = GenerateCoordinates(context)
        Dim islandNames = GenerateNames(context, islandCoordinates.Count)
        Do While islandCoordinates.Count <> 0
            Dim name = islandNames.Dequeue
            Dim coordinate = islandCoordinates.Dequeue
            Dim island = world.CreateLocation(LocationSubtypes.ISLAND, name, $"This island is called `{name}`.", InitializeBubble(context, coordinate))
            world.AddBubble(island)
        Loop
    End Sub

    Private Function InitializeBubble(context As IInitializationContext, coordinate As (Longitude As Double, Latitude As Double)) As LocationInitializer
        Return Sub(island)
                   island.SetDimension(Dimensions.VISIBILITY, RNG.RollDice("3d8*10"))
                   island.SetDimension(Dimensions.LONGITUDE, coordinate.Longitude)
                   island.SetDimension(Dimensions.LATITUDE, coordinate.Latitude)
                   island.SetDimension(Dimensions.DEPTH, RNG.FromRange(context.MinimumBubbleDepth, context.MaximumBubbleDepth))
                   island.CreateVerb(VerbSubtypes.EMBARK, "Embark", "You step onto the ship.")
                   island.CreateJobBoard()
                   island.InitializeCommodities()
               End Sub
    End Function

    <Extension>
    Private Sub InitializeCommodities(island As ILocation)
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
