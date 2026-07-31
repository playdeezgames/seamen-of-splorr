Public Interface IItemTypeModel
    ReadOnly Property Name As String
    ReadOnly Property UnitBuyPrice As Double
    Sub Buy(quantity As Integer)
    ReadOnly Property MaximumBuyQuantity As Integer
End Interface
