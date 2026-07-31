
Public Delegate Sub CommodityInitializer(commodity As ICommodity)
Public Interface ICommodity
    ReadOnly Property CommodityType As String
    ReadOnly Property Name As String
    Property BasePrice As Double
    Property SupplyFactor As Double
    Property DemandFactor As Double
    ReadOnly Property World As IWorld
End Interface
