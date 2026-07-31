Public Interface IAvatarKnownIslandsModel
    ReadOnly Property IsPicking As Boolean
    Sub HeadFor(islandModel As IIslandModel)
    ReadOnly Property All As IEnumerable(Of IIslandModel)
End Interface
