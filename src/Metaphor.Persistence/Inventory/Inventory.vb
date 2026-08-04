Imports Metaphor.Provision

Friend Class Inventory
    Inherits MetaphorEntity(Of InventoryData)
    Implements IInventory

    Public Sub New(world As IWorld, data As WorldData, inventoryId As Guid)
        MyBase.New(world, data, inventoryId)
    End Sub

    Public ReadOnly Property HasItems As Boolean Implements IInventory.HasItems
        Get
            Return Data.ItemIds.Count <> 0
        End Get
    End Property

    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements IInventory.Items
        Get
            Return Data.ItemIds.Select(Function(x) Item.Create(world, _data, x))
        End Get
    End Property

    Public ReadOnly Property ItemStacks As IEnumerable(Of IItemStack) Implements IInventory.ItemStacks
        Get
            Return Items.GroupBy(Function(x) x.EntitySubtype).Select(Function(x) ItemStack.Create(Me, x.Key))
        End Get
    End Property

    Public Overrides ReadOnly Property Exists As Boolean
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overrides ReadOnly Property Data As InventoryData
        Get
            Return _data.Inventories(EntityId)
        End Get
    End Property

    Public Overrides Sub Remove()
        For Each item In Items
            item.Remove()
        Next
        _data.Inventories.Remove(EntityId)
    End Sub

    Friend Shared Function Create(world As IWorld, data As WorldData, inventoryId As Guid?) As IInventory
        Return If(inventoryId.HasValue, New Inventory(world, data, inventoryId.Value), Nothing)
    End Function

    Public Function CreateItem(itemType As String, name As String, flavor As String, Optional initializer As ItemInitializer = Nothing) As IItem Implements IInventory.CreateItem
        Dim itemId = Guid.NewGuid
        _data.Items(itemId) = New ItemData With
            {
                .EntityType = EntityTypes.ITEM_ENTITY,
                .Name = name,
                .Flavor = flavor,
                .EntitySubtype = itemType,
                .InventoryId = EntityId
            }
        Data.ItemIds.Add(itemId)
        Dim result As IItem = Item.Create(world, _data, itemId)
        initializer?.Invoke(result)
        Return result
    End Function
End Class
