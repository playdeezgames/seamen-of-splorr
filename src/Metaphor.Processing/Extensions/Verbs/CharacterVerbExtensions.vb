Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module CharacterVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, character As ICharacter, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbSubtypes.HEAD_FOR_KNOWN_BUBBLE, AddressOf CanHeadForKnownBubble},
            {VerbSubtypes.DELIVER_PACKAGE, AddressOf CanDeliverPackage}
        }

    Private Function CanDeliverPackage(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
        Return verb.World.Avatar.Inventory.Items.Any(Function(x) If(x.GetRecipient()?.EntityId = character.EntityId, False))
    End Function

    Private Function CanHeadForKnownBubble(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
        Dim avatar = verb.World.Avatar
        Return Not avatar.GetShip().IsSnorkelRaised() AndAlso
            Not avatar.GetShip().IsMoored AndAlso
            avatar.GetKnownBubbles().Any
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, character, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbSubtypes.HEAD_FOR_KNOWN_BUBBLE, AddressOf HandleHeadForKnownBubble},
            {VerbSubtypes.DELIVER_PACKAGE, AddressOf HandleDeliverPackage},
            {VerbSubtypes.WAIT, AddressOf HandleWait}
        }

    Private Sub HandleWait(verb As IVerb, character As ICharacter, actor As ICharacter)
        actor.DoBiology(1)
    End Sub

    Private Sub HandleDeliverPackage(verb As IVerb, character As ICharacter, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        Dim item = avatar.Inventory.Items.Single(Function(x) If(x.GetRecipient()?.EntityId = character.EntityId, False))
        world.AddMessage($"{avatar.Name} receives {item.GetJools():f2} jools.")
        avatar.ChangeDimension(Dimensions.JOOLS, item.GetJools())
        avatar.ClearTag(Tags.DELIVERING)
        item.Remove()
        character.Remove()
    End Sub

    Private Sub HandleHeadForKnownBubble(verb As IVerb, character As ICharacter, actor As ICharacter)
        character.SetMode(Modes.PICKING_KNOWN_BUBBLE)
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, character As ICharacter, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, character, actor)
            Return
        End If
    End Sub
    <Extension>
    Friend Function GetKnownBubbles(character As ICharacter) As IEnumerable(Of ILocation)
        Return character.GetYokage(Yokages.KNOWN_BUBBLES).Select(AddressOf character.World.GetLocation)
    End Function
End Module
