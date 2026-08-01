Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ItemVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, item As IItem, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
        }

    <Extension>
    Friend Function CanPerform(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntityType, handler) Then
            Return handler.Invoke(verb, item, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbTypes.EAT, AddressOf HandleEat}
        }

    Private Sub HandleEat(verb As IVerb, item As IItem, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        world.AddMessage($"{avatar.Name} eats {item.Name}.")
        Dim stomach = item.GetCounter(Counters.STOMACH)
        world.AddMessage($"{avatar.Name} gains {stomach} stomach.")
        avatar.ChangeCounter(Counters.STOMACH, stomach)
        world.AddMessage($"{avatar.Name} now has {avatar.GetStomach}/{avatar.GetMaximumStomach} stomach.")
        item.Remove()
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, item As IItem, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        verb.World.AddMessage(verb.Flavor)
        If performTable.TryGetValue(verb.EntityType, handler) Then
            handler.Invoke(verb, item, actor)
            Return
        End If
    End Sub

End Module
