Public Interface IShipModel
    Sub SetHeading(heading As Double)
    ReadOnly Property CurrentHeading As Double
    Sub SetSpeed(speed As Double)
    ReadOnly Property CurrentSpeed As Double
    ReadOnly Property MaximumSpeed As Double

End Interface
