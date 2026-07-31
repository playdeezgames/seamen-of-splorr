Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class SellItemQuantityPrompt
    Inherits MetaphorDialog

    Private ReadOnly itemStackModel As IItemStackModel

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource, itemStackModel As IItemStackModel)
        MyBase.New(context, model, previous)
        Me.itemStackModel = itemStackModel
    End Sub

    Friend Shared Function Launch(c As IDisplayContext, m As IWorldModel, p As DialogSource, itemStackModel As IItemStackModel) As DialogSource
        Return Function() New SellItemQuantityPrompt(c, m, p, itemStackModel)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        Return DialogPrompt.CreateIntegerPrompt($"Sell how many {itemStackModel.Name} @ {itemStackModel.UnitSellPrice:f4} (you have {itemStackModel.Count})?", AddressOf ChooseQuantity)
    End Function

    Private Function ChooseQuantity(quantity As Integer) As IDialog
        itemStackModel.Sell(Math.Clamp(quantity, 0, itemStackModel.Count))
        Return SellingMenu.Launch(Context, Model, Previous).Invoke()
    End Function
End Class
