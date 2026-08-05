Imports Metaphor.Persistence

Friend Class ShipModel
    Implements IShipModel

    Private ReadOnly ship As ILocation

    Private Sub New(ship As ILocation)
        Me.ship = ship
    End Sub

    Public ReadOnly Property CurrentHeading As Double Implements IShipModel.CurrentHeading
        Get
            Return ship.GetHeading()
        End Get
    End Property

    Public ReadOnly Property CurrentSpeed As Double Implements IShipModel.CurrentSpeed
        Get
            Return ship.GetSpeed()
        End Get
    End Property

    Public ReadOnly Property MaximumSpeed As Double Implements IShipModel.MaximumSpeed
        Get
            Return ship.GetDimensionMaximum(Dimensions.SPEED)
        End Get
    End Property

    Public ReadOnly Property CurrentHydroplane As Double Implements IShipModel.CurrentHydroplane
        Get
            Return ship.GetHydroplane()
        End Get
    End Property

    Public ReadOnly Property MinimumHydroplane As Double Implements IShipModel.MinimumHydroplane
        Get
            Return ship.GetDimensionMinimum(Dimensions.HYDROPLANE)
        End Get
    End Property

    Public ReadOnly Property MaximumHydroplane As Double Implements IShipModel.MaximumHydroplane
        Get
            Return ship.GetDimensionMaximum(Dimensions.HYDROPLANE)
        End Get
    End Property

    Public Sub SetHeading(heading As Double) Implements IShipModel.SetHeading
        ship.HeadFor(Nothing)
        ship.SetHeading(heading)
        ship.World.Avatar.SetMode(Nothing)
        ship.World.Avatar.Look()
    End Sub

    Public Sub SetSpeed(speed As Double) Implements IShipModel.SetSpeed
        ship.SetSpeed(speed)
        ship.World.Avatar.SetMode(Nothing)
        ship.World.Avatar.Look()
    End Sub

    Public Sub SetHydroplane(hydroplane As Double) Implements IShipModel.SetHydroplane
        ship.SetHydroplane(hydroplane)
        ship.World.Avatar.SetMode(Nothing)
        ship.World.Avatar.Look()
    End Sub

    Friend Shared Function Create(ship As ILocation) As IShipModel
        Return New ShipModel(ship)
    End Function
End Class
