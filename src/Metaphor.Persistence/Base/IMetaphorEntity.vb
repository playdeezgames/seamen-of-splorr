Imports TGGD.Persistence
Public Delegate Sub EntityInitializer(entity As IMetaphorEntity)
Public Interface IMetaphorEntity
    Inherits IEntity
    ReadOnly Property World As IWorld
    Sub Remove()
    ReadOnly Property Name As String
    ReadOnly Property Flavor As String
    ReadOnly Property EntityId As Guid
    ReadOnly Property EntitySubtype As String
    ReadOnly Property Exists As Boolean
    Function CreateVerb(verbSubtype As String, name As String, flavor As String, Optional initializer As VerbInitializer = Nothing) As IVerb
    ReadOnly Property Verbs As IEnumerable(Of IVerb)
    ReadOnly Property Inventory As IInventory
End Interface
