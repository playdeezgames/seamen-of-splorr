
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
            Concat(Enumerable.Range(1, 10).Select(AddressOf ChooseSpeed))
        End Get
    End Property

    Private Function ChooseSpeed(value As Integer) As LaunchDelegate
        Dim percentage = value * 10
        Dim speed = Model.Avatar.Ship.MaximumSpeed * percentage / 100.0
        Return Function(c, m, p)
                   Return DialogChoice.Create(True, $"{percentage}% ({speed:f2} knots)", SetSpeedActivity.Launch(c, m, p, speed))
               End Function
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New SetSpeedPrompt(context, model, previous)
    End Function

    Private Function ChooseFullStop(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Full Stop", SetSpeedActivity.Launch(context, model, previous, 0.0))
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", SetSpeedActivity.Launch(context, model, previous, model.Avatar.Ship.CurrentSpeed))
    End Function
End Class
