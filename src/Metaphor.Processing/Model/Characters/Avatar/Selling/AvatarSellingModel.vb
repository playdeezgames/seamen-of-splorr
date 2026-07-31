Imports Metaphor.Persistence

Friend Class AvatarSellingModel
    Implements IAvatarSellingModel

    Private ReadOnly avatar As ICharacter

    Private Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub
    Public ReadOnly Property Active As Boolean Implements IAvatarSellingModel.Active
        Get
            Return avatar.HasTag(Tags.SELLING)
        End Get
    End Property

    Public ReadOnly Property ItemStacks As IEnumerable(Of IItemStackModel) Implements IAvatarSellingModel.ItemStacks
        Get
            Return avatar.Ship.GetCargoHold().Inventory.ItemStacks.Select(AddressOf ItemStackModel.Create)
        End Get
    End Property

    Public Sub Cancel() Implements IAvatarSellingModel.Cancel
        avatar.ClearTag(Tags.SELLING)
    End Sub

    Friend Shared Function Create(avatar As ICharacter) As IAvatarSellingModel
        Return New AvatarSellingModel(avatar)
    End Function
End Class
