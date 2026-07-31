Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module CharacterVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, character As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, character As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbTypes.HEAD_FOR_KNOWN_ISLAND, AddressOf CanHeadForKnownIsland},
            {VerbTypes.DELIVER_PACKAGE, AddressOf CanDeliverPackage}
        }

    Private Function CanDeliverPackage(verb As IVerb, character As ICharacter) As Boolean
        Return verb.World.Avatar.Inventory.Items.Any(Function(x) If(x.Recipient?.EntityId = character.EntityId, False))
    End Function

    Private Function CanHeadForKnownIsland(verb As IVerb, character As ICharacter) As Boolean
        Dim avatar = verb.World.Avatar
        Return Not avatar.Ship.IsMoored AndAlso avatar.KnownIslands.Any
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, character As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntityType, handler) Then
            Return handler.Invoke(verb, character)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbTypes.HEAD_FOR_KNOWN_ISLAND, AddressOf HandleHeadForKnownIsland},
            {VerbTypes.DELIVER_PACKAGE, AddressOf HandleDeliverPackage}
        }

    Private Sub HandleDeliverPackage(verb As IVerb, character As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim item = avatar.Inventory.Items.Single(Function(x) If(x.Recipient?.EntityId = character.EntityId, False))
        world.AddMessage($"{avatar.Name} receives {item.GetJools():f2} jools.")
        avatar.ChangeDimension(Dimensions.JOOLS, item.GetJools())
        avatar.ClearTag(Tags.DELIVERING)
        item.Remove()
        character.Remove()
    End Sub

    Private Sub HandleHeadForKnownIsland(verb As IVerb, character As ICharacter)
        character.SetTag(Tags.CHOOSING_KNOWN_ISLAND)
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, character As ICharacter)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntityType, handler) Then
            handler.Invoke(verb, character)
            Return
        End If
    End Sub
End Module
