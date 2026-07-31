Imports Metaphor.Persistence

Friend Class AvatarUnfoulingModel
    Implements IAvatarUnfoulingModel

    Private ReadOnly avatar As ICharacter

    Private Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Active As Boolean Implements IAvatarUnfoulingModel.Active
        Get
            Return avatar.HasTag(Tags.UNFOULING)
        End Get
    End Property

    Public ReadOnly Property CanAfford As Boolean Implements IAvatarUnfoulingModel.CanAfford
        Get
            Return avatar.GetJools() >= avatar.Location.GetUnfoulingPrice()
        End Get
    End Property

    Public Sub Confirm() Implements IAvatarUnfoulingModel.Confirm
        Dim price = avatar.Location.GetUnfoulingPrice()
        Dim world = avatar.World
        world.AddMessage($"{avatar.Name} pays {price:f2} jools to have the ship completely unfouled.")
        avatar.ChangeDimension(Dimensions.JOOLS, -price)
        avatar.Ship.SetDimension(Dimensions.FOULING, avatar.Ship.GetDimensionMinimum(Dimensions.FOULING))
        avatar.ClearTag(Tags.UNFOULING)
    End Sub

    Public Sub Cancel() Implements IAvatarUnfoulingModel.Cancel
        avatar.ClearTag(Tags.UNFOULING)
    End Sub

    Friend Shared Function Create(avatar As ICharacter) As IAvatarUnfoulingModel
        Return New AvatarUnfoulingModel(avatar)
    End Function
End Class
