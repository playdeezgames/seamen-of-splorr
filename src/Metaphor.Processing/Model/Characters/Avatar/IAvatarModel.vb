Public Interface IAvatarModel
    Sub ShowStatus()
    ReadOnly Property Inventory As IInventoryModel
    ReadOnly Property Verbs As IEnumerable(Of IVerbModel)
    Sub Look()
    ReadOnly Property Ship As IShipModel
    ReadOnly Property IsDead As Boolean
    ReadOnly Property CanStow As Boolean
    ReadOnly Property Unfouling As IAvatarUnfoulingModel
    ReadOnly Property Selling As IAvatarSellingModel
    ReadOnly Property Buying As IAvatarBuyingModel
    ReadOnly Property KnownIslands As IAvatarKnownIslandsModel
End Interface
