Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class StowFromItemStackPrompt
    Inherits MetaphorDialog

    Private ReadOnly itemStackModel As IItemStackModel

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource, itemStackModel As IItemStackModel)
        MyBase.New(context, model, previous)
        Me.itemStackModel = itemStackModel
    End Sub

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource, itemStackModel As IItemStackModel) As DialogSource
        Return Function() New StowFromItemStackPrompt(context, model, previous, itemStackModel)
    End Function

    Public Overrides Function Run() As IDialogPrompt
        Return DialogPrompt.CreateIntegerPrompt($"Stow how many {itemStackModel.Top.Name}? (Available:{itemStackModel.Count})", AddressOf ChooseStow)
    End Function

    Private Function ChooseStow(value As Integer) As IDialog
        itemStackModel.Stow(value)
        Return InventoryItemStackMenu.Launch(Context, Model, Previous, itemStackModel).Invoke()
    End Function
End Class
