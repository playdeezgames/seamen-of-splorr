Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class BuyFuelPrompt
    Inherits MetaphorDialog

    Public Sub New(
                  context As IDisplayContext,
                  model As IWorldModel,
                  previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Friend Shared Function Launch(
                          context As IDisplayContext,
                          model As IWorldModel,
                          previous As DialogSource) As DialogSource
        Return Function() New BuyFuelPrompt(context, model, previous)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        For Each message In Model.Messages
            Context.Render(message.Text, message.HintNames.ToDictionary(Function(x) x, Function(x) message.GetHint(x)))
        Next
        Return DialogPrompt.CreateDoublePrompt("How many units?", AddressOf BuyFuel)
    End Function

    Private Function BuyFuel(value As Double) As IDialog
        Model.Avatar.BuyFuel(value)
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
