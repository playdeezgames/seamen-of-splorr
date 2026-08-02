Public Interface IShipModel
    Sub SetHeading(heading As Double)
    ReadOnly Property CurrentHeading As Double
    Sub SetSpeed(speed As Double)
    ReadOnly Property CurrentSpeed As Double
    ReadOnly Property MaximumSpeed As Double
    ReadOnly Property CurrentHydroplane As Double
    ReadOnly Property MinimumHydroplane As Double
    ReadOnly Property MaximumHydroplane As Double
    Sub SetHydroplane(hydroplane As Double)
End Interface
