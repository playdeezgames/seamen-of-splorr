Imports System.Text
Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Class InitializationContext
    Implements IInitializationContext
    Private Sub New(chosenName As String, chosenPronouns As String)
        Me.ChosenName = chosenName
        Me.ChosenPronouns = chosenPronouns
    End Sub

    Public ReadOnly Property ChosenName As String Implements IInitializationContext.ChosenName

    Public Property Ship As ILocation Implements IInitializationContext.Ship

    Public ReadOnly Property ChosenPronouns As String Implements IInitializationContext.ChosenPronouns

    Public ReadOnly Property WorldWidth As Double Implements IInitializationContext.WorldWidth
        Get
            Return WORLD_WIDTH
        End Get
    End Property

    Public ReadOnly Property WorldHeight As Double Implements IInitializationContext.WorldHeight
        Get
            Return WORLD_HEIGHT
        End Get
    End Property

    Public ReadOnly Property IslandGenerationAttempts As Integer Implements IInitializationContext.IslandGenerationAttempts
        Get
            Return ISLAND_GENERATION_ATTEMPTS
        End Get
    End Property

    Public ReadOnly Property MinimumIslandDistance As Double Implements IInitializationContext.MinimumIslandDistance
        Get
            Return MINIMUM_ISLAND_DISTANCE
        End Get
    End Property

    Friend Shared Function Create(chosenName As String, chosenPronouns As String) As IInitializationContext
        Return New InitializationContext(chosenName, chosenPronouns)
    End Function

    Public Function GenerateName() As String Implements IInitializationContext.GenerateName
        Dim builder As New StringBuilder
        Dim isVowel = RNG.FromGenerator(RNG.MakeBooleanGenerator(1, 1))
        For Each dummy In Enumerable.Range(0, RNG.RollDice("3d4"))
            builder.Append(If(isVowel, GenerateVowel(), GenerateConsonant()))
            isVowel = Not isVowel
        Next
        Dim result = builder.ToString
        Return String.Concat(result.Substring(0, 1).ToUpper, result.AsSpan(1))
    End Function

    Private Shared Function GenerateConsonant() As String
        Return RNG.FromEnumerable("hklmp")
    End Function

    Private Shared Function GenerateVowel() As String
        Return RNG.FromEnumerable("aeiouäëïöü")
    End Function
End Class
