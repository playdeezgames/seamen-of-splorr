Public Interface IShipModel
    ReadOnly Property IsSettingHeading As Boolean
    ReadOnly Property IsSettingSpeed As Boolean
    Sub SetHeading(heading As Double)
    ReadOnly Property CurrentHeading As Double
    Sub SetSpeed(speed As Double)
    ReadOnly Property CurrentSpeed As Double
End Interface
