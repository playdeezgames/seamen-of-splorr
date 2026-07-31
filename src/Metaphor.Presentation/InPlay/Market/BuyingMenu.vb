
Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class BuyingMenu
    Inherits MetaphorPickerMenu

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Public Overrides ReadOnly Property PromptText As String
        Get
            Return "What would you like to buy?"
        End Get
    End Property

    Protected Overrides ReadOnly Property Launchers As IEnumerable(Of LaunchDelegate)
        Get
            Return Enumerable.Empty(Of LaunchDelegate).
                Append(AddressOf ChooseNeverMind).
                Concat(Model.Avatar.Buying.ItemTypes.Select(AddressOf ChooseItemType))
        End Get
    End Property

    Private Function ChooseItemType(itemTypeModel As IItemTypeModel) As LaunchDelegate
        Return Function(c, m, p)
                   Return DialogChoice.CreateEnabled($"{itemTypeModel.Name} @ {itemTypeModel.UnitBuyPrice:f4}", BuyItemTypeQuantityPrompt.Launch(c, m, p, itemTypeModel))
               End Function
    End Function

    Private Function ChooseNeverMind(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As IDialogChoice
        Return DialogChoice.CreateEnabled("Never Mind", AddressOf CancelBuying)
    End Function

    Private Function CancelBuying() As IDialog
        Model.Avatar.Buying.Cancel()
        Return InPlay.Launch(Context, Model, Previous).Invoke
    End Function

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New BuyingMenu(context, model, previous)
    End Function
End Class
