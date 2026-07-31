Public Interface IAvatarBuyingModel
    ReadOnly Property Active As Boolean
    Sub Cancel()
    ReadOnly Property ItemTypes As IEnumerable(Of IItemTypeModel)
End Interface
