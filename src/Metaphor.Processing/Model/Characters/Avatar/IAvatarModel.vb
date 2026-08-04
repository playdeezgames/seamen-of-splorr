Public Interface IAvatarModel
    Sub ShowStatus()
    ReadOnly Property Inventory As IInventoryModel
    ReadOnly Property Verbs As IEnumerable(Of IVerbModel)
    Sub Look()
    ReadOnly Property Ship As IShipModel
    ReadOnly Property IsDead As Boolean
    ReadOnly Property CanStow As Boolean
    ReadOnly Property KnownBubbles As IAvatarKnownBubblesModel
    ReadOnly Property Mode As String
End Interface
