Public Delegate Sub IslandCommodityInitializer(islandCommodity As IIslandCommodity)
Public Interface IIslandCommodity
    Inherits ICommodity
    ReadOnly Property Commodity As ICommodity
    Property Supply As Double
    Property Demand As Double
    Sub Buy(quantity As Double)
    Sub Sell(quantity As Double)
    ReadOnly Property MarketPrice As Double
End Interface
