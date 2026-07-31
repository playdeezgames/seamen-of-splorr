Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class BuyItemTypeQuantityPrompt
    Inherits MetaphorDialog

    Private ReadOnly itemTypeModel As IItemTypeModel

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource, itemTypeModel As IItemTypeModel)
        MyBase.New(context, model, previous)
        Me.itemTypeModel = itemTypeModel
    End Sub

    Friend Shared Function Launch(c As IDisplayContext, m As IWorldModel, p As DialogSource, itemTypeModel As IItemTypeModel) As DialogSource
        Return Function() New BuyItemTypeQuantityPrompt(c, m, p, itemTypeModel)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        Return DialogPrompt.CreateIntegerPrompt($"How many {itemTypeModel.Name} @ {itemTypeModel.UnitBuyPrice:f4} (can afford {itemTypeModel.MaximumBuyQuantity})?", AddressOf ChooseQuantity)
    End Function

    Private Function ChooseQuantity(quantity As Integer) As IDialog
        itemTypeModel.Buy(quantity)
        Return BuyingMenu.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
