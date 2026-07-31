Imports Metaphor.Persistence

Friend Interface IInitializationContext
    ReadOnly Property ChosenName As String
    ReadOnly Property ChosenPronouns As String
    ReadOnly Property WorldWidth As Double
    ReadOnly Property WorldHeight As Double
    Property Ship As ILocation
    ReadOnly Property IslandGenerationAttempts As Integer
    ReadOnly Property MinimumIslandDistance As Double
    Function GenerateName() As String
End Interface
