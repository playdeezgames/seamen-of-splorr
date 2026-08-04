Public Delegate Sub CharacterInitializer(character As ICharacter)
Public Interface ICharacter
    Inherits IMetaphorEntity
    Property Location As ILocation
    ReadOnly Property Pronouns As String
End Interface
