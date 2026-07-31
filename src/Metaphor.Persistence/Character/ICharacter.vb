Public Delegate Sub CharacterInitializer(character As ICharacter)
Public Interface ICharacter
    Inherits IInventoriedEntity
    Property Location As ILocation
    ReadOnly Property Pronouns As String
    Property Ship As ILocation
    Sub AddKnownIsland(island As ILocation)
    ReadOnly Property KnownIslands As IEnumerable(Of ILocation)
End Interface
