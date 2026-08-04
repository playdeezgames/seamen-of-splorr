Imports Metaphor.Persistence

Friend Class AvatarKnownBubblesModel
    Implements IAvatarKnownBubblesModel
    Private ReadOnly avatar As ICharacter

    Private Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IBubbleModel) Implements IAvatarKnownBubblesModel.All
        Get
            Return avatar.GetKnownBubbles().Select(AddressOf BubbleModel.Create)
        End Get
    End Property

    Public Sub HeadFor(bubbleModel As IBubbleModel) Implements IAvatarKnownBubblesModel.HeadFor
        avatar.SetMode(Nothing)
        If bubbleModel IsNot Nothing Then
            bubbleModel.SetHeadingFor()
        End If
    End Sub

    Friend Shared Function Create(avatar As ICharacter) As IAvatarKnownBubblesModel
        Return New AvatarKnownBubblesModel(avatar)
    End Function
End Class
