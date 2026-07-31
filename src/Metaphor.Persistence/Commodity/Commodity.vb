Imports Metaphor.Provision

Friend Class Commodity
    Implements ICommodity

    Private Sub New(world As IWorld, data As WorldData, commodityType As String)
        Me.World = world
        Me.CommodityType = commodityType
        Me._data = data
    End Sub

    Friend Shared Function Create(world As World, data As WorldData, commodityType As String) As ICommodity
        Return New Commodity(world, data, commodityType)
    End Function

    Public ReadOnly Property CommodityType As String Implements ICommodity.CommodityType
    Private ReadOnly _data As WorldData

    Friend ReadOnly Property Data As CommodityData
        Get
            Return _data.Commodities(CommodityType)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ICommodity.Name
        Get
            Return Data.Name
        End Get
    End Property

    Public Property BasePrice As Double Implements ICommodity.BasePrice
        Get
            Return Data.BasePrice
        End Get
        Set(value As Double)
            Data.BasePrice = value
        End Set
    End Property

    Public Property SupplyFactor As Double Implements ICommodity.SupplyFactor
        Get
            Return Data.SupplyFactor
        End Get
        Set(value As Double)
            Data.SupplyFactor = value
        End Set
    End Property

    Public Property DemandFactor As Double Implements ICommodity.DemandFactor
        Get
            Return Data.DemandFactor
        End Get
        Set(value As Double)
            Data.DemandFactor = value
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements ICommodity.World
End Class
