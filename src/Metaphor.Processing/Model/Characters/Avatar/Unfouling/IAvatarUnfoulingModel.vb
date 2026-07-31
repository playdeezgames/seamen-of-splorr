Public Interface IAvatarUnfoulingModel
    ReadOnly Property Active As Boolean
    ReadOnly Property CanAfford As Boolean
    Sub Confirm()
    Sub Cancel()
End Interface
