Imports Metaphor.Persistence

Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly avatar As ICharacter

    Private Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Inventory As IInventoryModel Implements IAvatarModel.Inventory
        Get
            Return InventoryModel.Create(avatar.Inventory)
        End Get
    End Property

    Public ReadOnly Property Verbs As IEnumerable(Of IVerbModel) Implements IAvatarModel.Verbs
        Get
            Return avatar.Verbs.Select(Function(x) CharacterVerbModel.Create(avatar, x))
        End Get
    End Property

    Public ReadOnly Property Ship As IShipModel Implements IAvatarModel.Ship
        Get
            Return ShipModel.Create(avatar.Ship)
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements IAvatarModel.IsDead
        Get
            Return avatar.IsDead
        End Get
    End Property

    Public ReadOnly Property CanStow As Boolean Implements IAvatarModel.CanStow
        Get
            Return avatar.Location.Features.Any(Function(x) x.IsCargoHold())
        End Get
    End Property

    Public ReadOnly Property Selling As IAvatarSellingModel Implements IAvatarModel.Selling
        Get
            Return AvatarSellingModel.Create(avatar)
        End Get
    End Property

    Public ReadOnly Property Buying As IAvatarBuyingModel Implements IAvatarModel.Buying
        Get
            Return AvatarBuyingModel.Create(avatar)
        End Get
    End Property

    Public ReadOnly Property KnownIslands As IAvatarKnownIslandsModel Implements IAvatarModel.KnownIslands
        Get
            Return AvatarKnownIslandsModel.Create(avatar)
        End Get
    End Property

    Public ReadOnly Property Unfouling As IAvatarUnfoulingModel Implements IAvatarModel.Unfouling
        Get
            Return AvatarUnfoulingModel.Create(avatar)
        End Get
    End Property

    Public Sub ShowStatus() Implements IAvatarModel.ShowStatus
        avatar.World.ClearMessages()
        avatar.ShowStatus()
    End Sub

    Public Sub Look() Implements IAvatarModel.Look
        avatar.World.ClearMessages()
        avatar.Look()
    End Sub

    Friend Shared Function Create(avatar As ICharacter) As IAvatarModel
        Return New AvatarModel(avatar)
    End Function
End Class
