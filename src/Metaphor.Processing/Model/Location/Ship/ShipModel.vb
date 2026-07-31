Imports Metaphor.Persistence

Friend Class ShipModel
    Implements IShipModel

    Private ReadOnly ship As ILocation

    Private Sub New(ship As ILocation)
        Me.ship = ship
    End Sub

    Public ReadOnly Property IsSettingHeading As Boolean Implements IShipModel.IsSettingHeading
        Get
            Return ship.HasTag(Tags.SETTING_HEADING)
        End Get
    End Property

    Public ReadOnly Property IsSettingSpeed As Boolean Implements IShipModel.IsSettingSpeed
        Get
            Return ship.HasTag(Tags.SETTING_SPEED)
        End Get
    End Property

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

    Public Sub SetHeading(heading As Double) Implements IShipModel.SetHeading
        ship.SetHeading(heading)
        ship.ClearTag(Tags.SETTING_HEADING)
        ship.World.Avatar.Look()
    End Sub

    Public Sub SetSpeed(speed As Double) Implements IShipModel.SetSpeed
        ship.SetSpeed(speed)
        ship.ClearTag(Tags.SETTING_SPEED)
        ship.World.Avatar.Look()
    End Sub

    Friend Shared Function Create(ship As ILocation) As IShipModel
        Return New ShipModel(ship)
    End Function
End Class
