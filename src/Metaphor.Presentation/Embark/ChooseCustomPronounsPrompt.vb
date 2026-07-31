Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class ChooseCustomPronounsPrompt
    Inherits MetaphorDialog

    Private name As String

    Public Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource, name As String)
        MyBase.New(context, model, previous)
        Me.name = name
    End Sub

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource, name As String) As DialogSource
        Return Function() New ChooseCustomPronounsPrompt(context, model, previous, name)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        Return DialogPrompt.CreateStringPrompt("What are yer pronouns?", AddressOf ChooseCustomPronouns)
    End Function

    Private Function ChooseCustomPronouns(value As String) As IDialog
        Return EmbarkActivity.Launch(Context, Model, Previous, name, value).Invoke()
    End Function
End Class
