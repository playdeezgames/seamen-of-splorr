Imports Metaphor.Persistence

Friend Class AvatarKnownIslandsModel
    Implements IAvatarKnownIslandsModel
    Private ReadOnly avatar As ICharacter

    Private Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IIslandModel) Implements IAvatarKnownIslandsModel.All
        Get
            Return avatar.GetKnownIslands().Select(AddressOf IslandModel.Create)
        End Get
    End Property

    Public Sub HeadFor(islandModel As IIslandModel) Implements IAvatarKnownIslandsModel.HeadFor
        avatar.SetMode(Nothing)
        If islandModel IsNot Nothing Then
            islandModel.SetHeadingFor()
        End If
    End Sub

    Friend Shared Function Create(avatar As ICharacter) As IAvatarKnownIslandsModel
        Return New AvatarKnownIslandsModel(avatar)
    End Function
End Class
