Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ItemExtensions
    <Extension>
    Friend Sub SetRecipient(item As IItem, character As ICharacter)
        item.SetYoke(Yokes.RECIPIENT, character.EntityId)
    End Sub
    <Extension>
    Friend Function GetRecipient(item As IItem) As ICharacter
        Return item.World.GetCharacter(item.GetYoke(Yokes.RECIPIENT))
    End Function
End Module
