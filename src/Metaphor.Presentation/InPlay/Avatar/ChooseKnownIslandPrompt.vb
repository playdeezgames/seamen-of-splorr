Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class ChooseKnownIslandPrompt
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "Which Known Island?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Concat(Model.Avatar.KnownIslands.All.Select(AddressOf ChooseKnownIsland))
        End Get
    End Property

    Private Function ChooseKnownIsland(islandModel As IIslandModel, arg2 As Integer) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled(islandModel.Name, HeadForIsland(c, m, p, islandModel))
               End Function
    End Function

    Private Shared Function HeadForIsland(c As IDisplayContext, m As IWorldModel, p As DialogSource, islandModel As IIslandModel) As DialogSource
        Return Function()
                   islandModel.SetHeadingFor()
                   Return InPlay.Launch(c, m, p).Invoke
               End Function
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New ChooseKnownIslandPrompt(context, model, previous)
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", AddressOf CancelChoosingKnownIsland)
    End Function

    Private Function CancelChoosingKnownIsland() As IDialog
        Model.Avatar.KnownIslands.HeadFor(Nothing)
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
