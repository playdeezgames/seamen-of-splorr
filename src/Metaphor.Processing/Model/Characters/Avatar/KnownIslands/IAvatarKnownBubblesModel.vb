Public Interface IAvatarKnownBubblesModel
    Sub HeadFor(bubbleModel As IBubbleModel)
    ReadOnly Property All As IEnumerable(Of IBubbleModel)
End Interface
