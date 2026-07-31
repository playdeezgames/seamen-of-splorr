Imports Metaphor.Persistence

Friend Class ItemTypeModel
    Implements IItemTypeModel

    Private ReadOnly market As IFeature
    Private ReadOnly itemType As String

    Private Sub New(market As IFeature, itemType As String)
        Me.market = market
        Me.itemType = itemType
    End Sub

    Public ReadOnly Property Name As String Implements IItemTypeModel.Name
        Get
            Return market.GetItemTypeName(itemType)
        End Get
    End Property

    Public ReadOnly Property UnitBuyPrice As Double Implements IItemTypeModel.UnitBuyPrice
        Get
            Return market.GetUnitBuyPrice(itemType)
        End Get
    End Property

    Public ReadOnly Property MaximumBuyQuantity As Integer Implements IItemTypeModel.MaximumBuyQuantity
        Get
            Return CInt(market.World.Avatar.GetJools() / UnitBuyPrice)
        End Get
    End Property

    Public Sub Buy(quantity As Integer) Implements IItemTypeModel.Buy
        Dim world = market.World
        Dim avatar = world.Avatar
        quantity = Math.Clamp(quantity, 0, CInt(avatar.GetJools() / UnitBuyPrice))
        Dim jools = quantity * UnitBuyPrice
        avatar.ChangeDimension(Dimensions.JOOLS, -jools)
        market.Buy(itemType, quantity)
        Utility.Repeat(quantity, Sub()
                                     avatar.Ship.GetCargoHold().Inventory.CreateItemOfType(itemType)
                                 End Sub)
        world.ClearMessages()
        world.AddMessage($"{avatar.Name} buys {quantity} {Name} for {jools:f2} jools.")
        world.AddMessage($"{avatar.Name} now has {avatar.GetJools:f2} jools.")
    End Sub

    Friend Shared Function Create(market As IFeature, itemType As String) As IItemTypeModel
        Return New ItemTypeModel(market, itemType)
    End Function
End Class
