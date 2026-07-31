
Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class ChoosePronounsPrompt
    Inherits MetaphorPickerMenu

    Private ReadOnly name As String

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource, name As String)
        MyBase.New(context, model, previous)
        Me.name = name
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "What are yer pronouns?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Dim choices = Enumerable.Empty(Of LaunchDelegate).
                Append(ChoosePronouns("He/Him")).
                Append(ChoosePronouns("He/They")).
                Append(ChoosePronouns("They/He")).
                Append(ChoosePronouns("They/Them")).
                Append(ChoosePronouns("They/She")).
                Append(ChoosePronouns("She/They")).
                Append(ChoosePronouns("She/Her")).
                Append(ChoosePronouns("Any")).
                Append(ChoosePronouns("Use Name Only")).
                OrderBy(Function(x) Guid.NewGuid)
            Return choices.
                Append(ChooseCustomPronouns("Custom..."))
        End Get
    End Property

    Private Function ChoosePronouns(pronouns As String) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled(pronouns, EmbarkActivity.Launch(Context, Model, Previous, name, pronouns))
               End Function
    End Function

    Private Function ChooseCustomPronouns(pronouns As String) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled(pronouns, ChooseCustomPronounsPrompt.Launch(Context, Model, Previous, name))
               End Function
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource, name As String) As DialogSource
        Return Function() New ChoosePronounsPrompt(context, model, previous, name)
    End Function
End Class
