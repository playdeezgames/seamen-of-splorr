Imports Metaphor.Persistence

Friend Class ItemStackModel
    Implements IItemStackModel

    Private ReadOnly ItemStack As IItemStack

    Private Sub New(itemStack As IItemStack)
        Me.ItemStack = itemStack
    End Sub

    Public ReadOnly Property Top As IItemModel Implements IItemStackModel.Top
        Get
            Return ItemModel.Create(ItemStack.Top)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IItemStackModel.Name
        Get
            Return $"{ItemStack.Top.Name}(x{ItemStack.Count})"
        End Get
    End Property

    Public ReadOnly Property Items As IEnumerable(Of IItemModel) Implements IItemStackModel.Items
        Get
            Return ItemStack.Items.Select(AddressOf ItemModel.Create)
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IItemStackModel.Count
        Get
            Return ItemStack.Count
        End Get
    End Property

    Public ReadOnly Property UnitSellPrice As Double Implements IItemStackModel.UnitSellPrice
        Get
            Return If(ItemStack.Top.World.Avatar.Location.GetMarket()?.GetUnitSellPrice(ItemStack.ItemType), 0.0)
        End Get
    End Property

    Public Sub Drop(dropCount As Integer) Implements IItemStackModel.Drop
        dropCount = Math.Min(dropCount, ItemStack.Count)
        Dim world = ItemStack.Top.World
        Dim character = world.Avatar
        world.ClearMessages()
        character.World.AddMessage($"{character.Name} drops {dropCount} {ItemStack.Top.Name}.")
        Utility.Repeat(dropCount, Sub() ItemStack.Top.Inventory = character.Location.Inventory)
    End Sub

    Public Sub Take(takeCount As Integer) Implements IItemStackModel.Take
        takeCount = Math.Min(takeCount, ItemStack.Count)
        Dim world = ItemStack.Top.World
        Dim character = world.Avatar
        world.ClearMessages()
        character.World.AddMessage($"{character.Name} takes {takeCount} {ItemStack.Top.Name}.")
        Utility.Repeat(takeCount, Sub() ItemStack.Top.Inventory = character.Inventory)
    End Sub

    Public Sub Stow(stowCount As Integer) Implements IItemStackModel.Stow
        stowCount = Math.Min(stowCount, ItemStack.Count)
        Dim world = ItemStack.Top.World
        Dim character = world.Avatar
        Dim cargoHold = character.Location.GetCargoHold()
        world.ClearMessages()
        character.World.AddMessage($"{character.Name} stows {stowCount} {ItemStack.Top.Name}.")
        Utility.Repeat(stowCount, Sub() ItemStack.Top.Inventory = cargoHold.Inventory)
    End Sub

    Public Sub Sell(quantity As Integer) Implements IItemStackModel.Sell
        Dim world = ItemStack.Top.World
        Dim avatar = world.Avatar
        Dim market = avatar.Location.GetMarket()
        If market Is Nothing Then
            Return
        End If
        world.ClearMessages()
        Dim jools = market.GetUnitSellPrice(ItemStack.ItemType) * quantity
        world.AddMessage($"{avatar.Name} sells {quantity} {ItemStack.Top.Name} for {jools:f2} jools.")
        avatar.ChangeDimension(Dimensions.JOOLS, jools)
        world.AddMessage($"{avatar.Name} now has {avatar.GetJools:f2} jools.")
        market.Sell(ItemStack.ItemType, quantity)
        Utility.Repeat(quantity, Sub() ItemStack.Top.Remove())
    End Sub

    Friend Shared Function Create(itemStack As IItemStack) As IItemStackModel
        Return New ItemStackModel(itemStack)
    End Function
End Class
