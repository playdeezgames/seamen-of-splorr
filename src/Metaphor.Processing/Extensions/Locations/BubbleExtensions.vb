Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module BubbleExtensions
    Friend Sub DescribeBubble(bubble As ILocation)
        Dim world = bubble.World
        world.AddMessage($"Bubble: {bubble.Name}")
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
    Function DepthDifference(fromLocation As ILocation, toLocation As ILocation) As Double
        Return Math.Abs(fromLocation.GetDepth() - toLocation.GetDepth())
    End Function
    <Extension>
    Function HeadingTo(fromLocation As ILocation, toLocation As ILocation) As Double
        Dim deltaX = toLocation.GetLongitude() - fromLocation.GetLongitude()
        Dim deltaY = toLocation.GetLatitude() - fromLocation.GetLatitude()
        Dim heading = Math.Atan2(deltaY, deltaX) * 360.0 / Math.PI / 2
        Return If(heading < 0.0, heading + 360.0, heading)
    End Function
    <Extension>
    Friend Function GetBubbleName(bubble As ILocation) As String
        Return If(bubble.HasTag(Tags.KNOWN), bubble.Name, "UNKNOWN BUBBLE")
    End Function
#Region "Fueling Station"
    <Extension>
    Friend Function GetFuelingStation(bubble As ILocation) As IFeature
        Return bubble.Features.SingleOrDefault(Function(x) x.EntitySubtype = FeatureSubtypes.FUELING_STATION)
    End Function
#End Region
#Region "Job Board"
    <Extension>
    Friend Sub CreateJobBoard(bubble As ILocation)
        bubble.CreateFeature(FeatureSubtypes.JOB_BOARD, "Job Board", "Here are listed various errand person jobs for making a small amount of jools.", AddressOf InitializeJobBoard)
    End Sub
    Private Sub InitializeJobBoard(feature As IFeature)
        feature.CreateVerb(VerbSubtypes.ACCEPT_DELIVERY, "Take Delivery Assignment", "Desperate for jools, you will take whatever whereever!")
    End Sub
    <Extension>
    Friend Function CreateRecipient(bubble As ILocation) As ICharacter
        Dim characterName As String = GenerateName(bubble)
        Return bubble.CreateCharacter(CharacterSubtypes.RECIPIENT, characterName, "They/Them", $"This is {characterName} of {bubble.Name}.", AddressOf InitializeRecipient)
    End Function
    Private Sub InitializeRecipient(character As ICharacter)
        character.CreateVerb(VerbSubtypes.DELIVER_PACKAGE, "Deliver Package", "You deliver the package, right in their package delivery hole.")
    End Sub
    Private Function GenerateName(bubble As ILocation) As String
        Return "Nacho Mama"
    End Function
#End Region
End Module
