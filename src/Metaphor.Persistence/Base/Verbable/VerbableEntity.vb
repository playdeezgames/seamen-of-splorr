Imports Metaphor.Provision

Friend MustInherit Class VerbableEntity(Of TData As VerbableEntityData)
    Inherits MetaphorEntity(Of TData)
    Implements IVerbableEntity

    Protected Sub New(world As IWorld, data As WorldData, entityId As Guid)
        MyBase.New(world, data, entityId)
    End Sub
End Class
