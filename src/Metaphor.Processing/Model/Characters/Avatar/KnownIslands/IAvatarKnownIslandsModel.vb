Public Interface IAvatarKnownIslandsModel
    Sub HeadFor(islandModel As IIslandModel)
    ReadOnly Property All As IEnumerable(Of IIslandModel)
End Interface
