Imports Metaphor.Provision
Imports TGGD.Provision

Friend Class Item
    Inherits MetaphorEntity(Of EntityData)
    Implements IItem

    Private Sub New(world As IWorld, data As WorldData, itemId As Guid)
        MyBase.New(world, data, itemId)
    End Sub

    Public Property Container As IInventory Implements IItem.Container
        Get
            Return Persistence.Inventory.Create(World, _data, GetYoke(Yokes.CONTAINER))
        End Get
        Set(value As IInventory)
            _data.Inventories(GetYoke(Yokes.CONTAINER).Value).ItemIds.Remove(EntityId)
            SetYoke(Yokes.CONTAINER, value.EntityId)
            _data.Inventories(GetYoke(Yokes.CONTAINER).Value).ItemIds.Add(EntityId)
        End Set
    End Property

    Public Overrides ReadOnly Property Exists As Boolean
        Get
            Return _data.Items.ContainsKey(EntityId)
        End Get
    End Property

    Protected Overrides ReadOnly Property Data As EntityData
        Get
            Return _data.Items(EntityId)
        End Get
    End Property

    Public Overrides Sub Remove()
        _data.Inventories(GetYoke(Yokes.CONTAINER).Value).ItemIds.Remove(EntityId)
        _data.Items.Remove(EntityId)
    End Sub

    Friend Shared Function Create(world As IWorld, data As WorldData, itemId As Guid?) As IItem
        Return If(
            itemId.HasValue,
            New Item(world, data, itemId.Value),
            Nothing)
    End Function
End Class
