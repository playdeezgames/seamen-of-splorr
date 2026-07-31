Imports Metaphor.Provision

Friend Class IslandCommodity
    Implements IIslandCommodity

    Private Sub New(world As IWorld, data As WorldData, locationId As Guid, commodityType As String)
        Me.World = world
        Me._data = data
        Me.locationId = locationId
        Me.commodityType = commodityType
    End Sub

    Private ReadOnly Property Data As IslandCommodityData
        Get
            Return _data.Locations(locationId).IslandCommodities(commodityType)
        End Get
    End Property

    Public ReadOnly Property Commodity As ICommodity Implements IIslandCommodity.Commodity
        Get
            Return World.Commodities(commodityType)
        End Get
    End Property

    Public Property Supply As Double Implements IIslandCommodity.Supply
        Get
            Return Data.Supply
        End Get
        Set(value As Double)
            Data.Supply = value
        End Set
    End Property

    Public Property Demand As Double Implements IIslandCommodity.Demand
        Get
            Return Data.Demand
        End Get
        Set(value As Double)
            Data.Demand = value
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements IIslandCommodity.World

    Public ReadOnly Property Name As String Implements ICommodity.Name
        Get
            Return Commodity.Name
        End Get
    End Property

    Public Property BasePrice As Double Implements ICommodity.BasePrice
        Get
            Return Commodity.BasePrice
        End Get
        Set(value As Double)
            Commodity.BasePrice = value
        End Set
    End Property

    Public Property SupplyFactor As Double Implements ICommodity.SupplyFactor
        Get
            Return Commodity.SupplyFactor
        End Get
        Set(value As Double)
            Commodity.SupplyFactor = value
        End Set
    End Property

    Public Property DemandFactor As Double Implements ICommodity.DemandFactor
        Get
            Return Commodity.DemandFactor
        End Get
        Set(value As Double)
            Commodity.DemandFactor = value
        End Set
    End Property

    Public ReadOnly Property CommodityType As String Implements ICommodity.CommodityType

    Public ReadOnly Property MarketPrice As Double Implements IIslandCommodity.MarketPrice
        Get
            Return BasePrice * Demand / Supply
        End Get
    End Property

    Private ReadOnly _data As WorldData
    Private ReadOnly locationId As Guid

    Public Sub Buy(quantity As Double) Implements IIslandCommodity.Buy
        Demand += quantity * DemandFactor
    End Sub

    Public Sub Sell(quantity As Double) Implements IIslandCommodity.Sell
        Supply += quantity * SupplyFactor
    End Sub

    Friend Shared Function Create(world As IWorld, data As WorldData, entityId As Guid, commodityType As String) As IIslandCommodity
        Return New IslandCommodity(world, data, entityId, commodityType)
    End Function
End Class
