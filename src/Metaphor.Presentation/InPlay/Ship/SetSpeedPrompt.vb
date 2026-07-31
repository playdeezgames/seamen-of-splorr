
Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class SetSpeedPrompt
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "New Speed?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
            Append(AddressOf ChooseNeverMind).
            Append(AddressOf ChooseFullStop).
            Append(AddressOf ChooseOneThird).
            Append(AddressOf ChooseTwoThirds).
            Append(AddressOf ChooseFull).
            Append(AddressOf ChooseFlank)
        End Get
    End Property

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New SetSpeedPrompt(context, model, previous)
    End Function

    Private Function ChooseFlank(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Ahead Flank", SetSpeedActivity.Launch(context, model, previous, 1.0))
    End Function

    Private Function ChooseFull(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Ahead Full", SetSpeedActivity.Launch(context, model, previous, 0.9))
    End Function

    Private Function ChooseTwoThirds(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Ahead Two Thirds", SetSpeedActivity.Launch(context, model, previous, 0.6))
    End Function

    Private Function ChooseOneThird(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Ahead One Third", SetSpeedActivity.Launch(context, model, previous, 0.3))
    End Function

    Private Function ChooseFullStop(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Full Stop", SetSpeedActivity.Launch(context, model, previous, 0.0))
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", SetSpeedActivity.Launch(context, model, previous, model.Avatar.Ship.CurrentSpeed))
    End Function
End Class
