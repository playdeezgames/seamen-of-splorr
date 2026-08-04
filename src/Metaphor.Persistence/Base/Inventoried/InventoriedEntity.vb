Imports Metaphor.Provision

Friend MustInherit Class InventoriedEntity(Of TData As MetaphorEntityData)
    Inherits MetaphorEntity(Of TData)
    Implements IInventoriedEntity

    Protected Sub New(world As IWorld, data As WorldData, entityId As Guid)
        MyBase.New(world, data, entityId)
    End Sub

    Public ReadOnly Property Inventory As IInventory Implements IInventoriedEntity.Inventory
        Get
            Dim inventoryId As Guid
            If Not Data.Yokes.TryGetValue(Yokes.INVENTORY, inventoryId) Then
                inventoryId = Guid.NewGuid
                _data.Inventories(inventoryId) = New InventoryData
                Data.Yokes(Yokes.INVENTORY) = inventoryId
            End If
            Return Persistence.Inventory.Create(World, _data, inventoryId)
        End Get
    End Property
End Class
