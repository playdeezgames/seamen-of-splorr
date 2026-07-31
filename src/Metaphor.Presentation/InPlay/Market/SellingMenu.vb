
Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class SellingMenu
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "What do you want to sell?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Concat(Model.Avatar.Selling.ItemStacks.Select(Function(x) ChooseItemStack(x)))
        End Get
    End Property

    Private Shared Function ChooseItemStack(itemStackModel As IItemStackModel) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled($"{itemStackModel.Top.Name}(x{itemStackModel.Count}, @ {itemStackModel.UnitSellPrice:f4})", SellItemQuantityPrompt.Launch(c, m, p, itemStackModel))
               End Function
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", AddressOf CancelSelling)
    End Function

    Private Function CancelSelling() As IDialog
        Model.Avatar.Selling.Cancel()
        Return InPlay.Launch(Context, Model, Previous).Invoke()
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New SellingMenu(context, model, previous)
    End Function
End Class
