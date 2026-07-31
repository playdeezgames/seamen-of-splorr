Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class SetHeadingPrompt
    Inherits MetaphorDialog

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New SetHeadingPrompt(context, model, previous)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        Return DialogPrompt.CreateDoublePrompt($"New Heading(0-360, currently {Model.Avatar.Ship.CurrentHeading:f2})?", AddressOf ChooseNewHeading)
    End Function

    Private Function ChooseNewHeading(value As Double) As IDialog
        Model.Avatar.Ship.SetHeading(value)
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
