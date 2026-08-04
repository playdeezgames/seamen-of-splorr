Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class ChooseKnownBubblePrompt
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "Which Known Bubble?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Concat(Model.Avatar.KnownBubbles.All.Select(AddressOf ChooseKnownBubble))
        End Get
    End Property

    Private Function ChooseKnownBubble(bubbleModel As IBubbleModel, arg2 As Integer) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled(bubbleModel.Name, HeadForBubble(c, m, p, bubbleModel))
               End Function
    End Function

    Private Shared Function HeadForBubble(c As IDisplayContext, m As IWorldModel, p As DialogSource, bubbleModel As IBubbleModel) As DialogSource
        Return Function()
                   bubbleModel.SetHeadingFor()
                   Return InPlay.Launch(c, m, p).Invoke
               End Function
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New ChooseKnownBubblePrompt(context, model, previous)
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", AddressOf CancelChoosingKnownBubble)
    End Function

    Private Function CancelChoosingKnownBubble() As IDialog
        Model.Avatar.KnownBubbles.HeadFor(Nothing)
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
