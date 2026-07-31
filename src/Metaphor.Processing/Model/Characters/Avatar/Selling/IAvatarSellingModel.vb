Public Interface IAvatarSellingModel
    ReadOnly Property Active As Boolean
    Sub Cancel()
    ReadOnly Property ItemStacks As IEnumerable(Of IItemStackModel)
End Interface
