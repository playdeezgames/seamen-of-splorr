Imports Metaphor.Persistence

Friend Interface IInitializationContext
    ReadOnly Property ChosenName As String
    ReadOnly Property ChosenPronouns As String
    ReadOnly Property WorldWidth As Double
    ReadOnly Property WorldHeight As Double
    ReadOnly Property WorldDepth As Double
    Property Ship As ILocation
    ReadOnly Property BubbleGenerationAttempts As Integer
    ReadOnly Property MinimumBubbleDistance As Double
    Function GenerateName() As String
    ReadOnly Property SnorkelDepth As Double
    ReadOnly Property MinimumBubbleDepth As Double
    ReadOnly Property MaximumBubbleDepth As Double
End Interface
