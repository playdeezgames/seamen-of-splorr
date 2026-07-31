Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module FeatureVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, feature As IFeature) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, feature As IFeature)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbTypes.ACCEPT_DELIVERY, AddressOf CanAcceptDelivery}
        }

    Private Function CanAcceptDelivery(verb As IVerb, feature As IFeature) As Boolean
        Return Not verb.World.Avatar.HasTag(Tags.DELIVERING)
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, feature As IFeature) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntityType, handler) Then
            Return handler.Invoke(verb, feature)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbTypes.MOVE, AddressOf HandleMove},
            {VerbTypes.ACCEPT_DELIVERY, AddressOf HandleAcceptDelivery}
        }

    Private Sub HandleAcceptDelivery(verb As IVerb, feature As IFeature)
        Dim world = verb.World
        Dim avatar = world.Avatar
        avatar.SetTag(Tags.DELIVERING)
        Dim origin = feature.Location
        Dim destination = RNG.FromEnumerable(world.Islands.Where(Function(x) x.EntityId <> origin.EntityId))
        destination.SetTag(Tags.KNOWN)
        avatar.AddKnownIsland(destination)
        Dim distance = origin.DistanceTo(destination)
        Dim recipient = destination.CreateRecipient()
        Dim item = avatar.Inventory.CreateDeliveryItem(recipient)
        item.SetJools(distance)
        world.AddMessage($"Please deliver this {item.Name} to {recipient.Name} on {destination.GetIslandName()}.")
    End Sub

    Private Sub HandleMove(verb As IVerb, feature As IFeature)
        Dim world = verb.World
        Dim avatar = world.Avatar
        avatar.Location = feature.Destination
        avatar.Look()
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, feature As IFeature)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntityType, handler) Then
            handler.Invoke(verb, feature)
            Return
        End If
    End Sub
End Module
