Imports Metaphor.Provision
Imports TGGD.Persistence

Friend MustInherit Class MetaphorEntity(Of TData As MetaphorEntityData)
    Inherits Entity(Of TData)
    Implements IMetaphorEntity

    Protected Sub New(world As IWorld, data As WorldData, entityId As Guid)
        Me.World = world
        Me._data = data
        Me.EntityId = entityId
    End Sub

    Public MustOverride Sub Remove() Implements IMetaphorEntity.Remove
    Public ReadOnly Property World As IWorld Implements IMetaphorEntity.World

    Public ReadOnly Property Name As String Implements IMetaphorEntity.Name
        Get
            Return Data.Name
        End Get
    End Property

    Public ReadOnly Property Flavor As String Implements IMetaphorEntity.Flavor
        Get
            Return Data.Flavor
        End Get
    End Property

    Public ReadOnly Property EntityId As Guid Implements IMetaphorEntity.EntityId

    Public ReadOnly Property EntitySubtype As String Implements IMetaphorEntity.EntitySubtype
        Get
            Return Data.EntitySubtype
        End Get
    End Property

    Public MustOverride ReadOnly Property Exists As Boolean Implements IMetaphorEntity.Exists
    Protected ReadOnly _data As WorldData

    Public ReadOnly Property Verbs As IEnumerable(Of IVerb) Implements IMetaphorEntity.Verbs
        Get
            Return Data.Yokages(Yokages.VERBS).Select(Function(x) Verb.Create(World, _data, x))
        End Get
    End Property


    Public Function CreateVerb(
                              verbSubtype As String,
                              name As String,
                              flavor As String,
                              Optional initializer As VerbInitializer = Nothing) As IVerb Implements IMetaphorEntity.CreateVerb
        Dim verbId = Guid.NewGuid
        _data.Verbs(verbId) = New VerbData With
            {
                .EntityType = EntityTypes.VERB_ENTITY,
                .EntitySubtype = verbSubtype,
                .Name = name,
                .Flavor = flavor
            }
        AddToYokage(Yokages.VERBS, verbId)
        Dim result As IVerb = Verb.Create(World, _data, verbId)
        initializer?.Invoke(result)
        Return result
    End Function
End Class
