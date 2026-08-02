Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class SetHydroplanePrompt
    Inherits MetaphorPickerMenu

    Public Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "What bubble?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Concat(Enumerable.Range(-4, 9).Select(AddressOf ChooseBubble))
        End Get
    End Property

    Private Function ChooseBubble(value As Integer) As LaunchDelegate
        Return Function(c, m, p)
                   Dim hydroplane = value * 5.0
                   Return DialogChoice.CreateEnabled(Utility.DescribeHydroplane(hydroplane), SetHydroplane(hydroplane))
               End Function
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New SetHydroplanePrompt(context, model, previous)
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled($"Never Mind({Utility.DescribeHydroplane(model.Avatar.Ship.CurrentHydroplane)})", SetHydroplane(model.Avatar.Ship.CurrentHydroplane))
    End Function

    Private Function SetHydroplane(hydroplane As Double) As DialogSource
        Return Function()
                   Model.Avatar.Ship.SetHydroplane(hydroplane)
                   Return InPlay.Launch(Context, Model, Previous).Invoke()
               End Function
    End Function
End Class
