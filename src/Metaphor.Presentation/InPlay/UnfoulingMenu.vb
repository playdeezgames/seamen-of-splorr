
Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class UnfoulingMenu
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "How would you like to proceed?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Append(AddressOf ChooseConfirm)
        End Get
    End Property

    Private Function ChooseConfirm(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.Create(model.Avatar.Unfouling.CanAfford, "Confirm!", AddressOf ConfirmUnfouling)
    End Function

    Private Function ConfirmUnfouling() As IDialog
        Model.Avatar.Unfouling.Confirm()
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", AddressOf CancelUnfouling)
    End Function

    Private Function CancelUnfouling() As IDialog
        Model.Avatar.Unfouling.Cancel()
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New UnfoulingMenu(context, model, previous)
    End Function
End Class
