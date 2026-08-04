Public Delegate Sub ItemInitializer(item As IItem)
Public Interface IItem
    Inherits IMetaphorEntity
    Property Inventory As IInventory
End Interface
