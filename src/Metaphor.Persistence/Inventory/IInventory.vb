Public Interface IInventory
    Inherits IMetaphorEntity
    Function CreateItem(entitySubtype As String, name As String, flavor As String, Optional initializer As ItemInitializer = Nothing) As IItem
    ReadOnly Property HasItems As Boolean
    ReadOnly Property Items As IEnumerable(Of IItem)
    ReadOnly Property ItemStacks As IEnumerable(Of IItemStack)
End Interface
