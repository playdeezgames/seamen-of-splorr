Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module CharacterExtensions
    <Extension>
    Private Function IsAvatar(character As ICharacter) As Boolean
        Return If(character.World.Avatar?.EntityId = character.EntityId, False)
    End Function
    <Extension>
    Friend Sub Look(character As ICharacter)
        Dim world = character.World
        If character.IsDead Then
            world.AddMessage($"{character.Name} is dead.")
            Return
        End If
        Dim location = character.Location
        World.AddMessage(location.Flavor)
        location.Describe()
        ShowOtherCharacters(character)
        ShowFeatures(character)
        If location.Inventory.HasItems Then
            world.AddMessage("There are items on the ground.")
        End If
    End Sub

    <Extension>
    Friend Sub ShowOtherCharacters(character As ICharacter)
        Dim others = character.Location.GetOtherCharacters(character)
        If others.Any Then
            character.World.AddMessage("Characters:")
            For Each other In others
                character.World.AddMessage($"- {other.Name}")
            Next
        End If
    End Sub

    <Extension>
    Friend Sub ShowFeatures(character As ICharacter)
        Dim features = character.Location.Features
        If features.Any Then
            character.World.AddMessage($"Features:")
            For Each feature In features
                character.World.AddMessage($"- {feature.Name}")
            Next
        End If
    End Sub
    <Extension>
    Friend Sub ShowStatus(character As ICharacter)
        Dim world = character.World
        world.AddMessage($"{character.Name}'s Status:")
        world.AddMessage(character.Flavor)
        world.AddMessage($"Flesh: {character.GetFleshGrams()}/{character.GetMaximumFleshGrams()}g")
        world.AddMessage($"Health: {character.GetHealth()}/{character.GetMaximumHealth()}")
        world.AddMessage($"Satiety: {character.GetSatiety()}/{character.GetMaximumSatiety()}")
        world.AddMessage($"Stomach: {character.GetStomach()}/{character.GetMaximumStomach()}")
        world.AddMessage($"Jools: {character.GetJools():f2}")
    End Sub
    <Extension>
    Friend Function GetFleshGrams(character As ICharacter) As Integer
        Return character.GetCounter(Counters.FLESH_GRAMS)
    End Function
    <Extension>
    Friend Function GetMaximumFleshGrams(character As ICharacter) As Integer
        Return character.GetCounterMaximum(Counters.FLESH_GRAMS)
    End Function
    <Extension>
    Friend Function GetHealth(character As ICharacter) As Integer
        Return character.GetCounter(Counters.HEALTH)
    End Function
    <Extension>
    Friend Function GetSatiety(character As ICharacter) As Integer
        Return character.GetCounter(Counters.SATIETY)
    End Function
    <Extension>
    Friend Function GetStomach(character As ICharacter) As Integer
        Return character.GetCounter(Counters.STOMACH)
    End Function
    <Extension>
    Friend Function GetMaximumHealth(character As ICharacter) As Integer
        Return character.GetCounterMaximum(Counters.HEALTH)
    End Function
    <Extension>
    Friend Function GetMaximumSatiety(character As ICharacter) As Integer
        Return character.GetCounterMaximum(Counters.SATIETY)
    End Function
    <Extension>
    Friend Function GetMaximumStomach(character As ICharacter) As Integer
        Return character.GetCounterMaximum(Counters.STOMACH)
    End Function
    <Extension>
    Friend Sub DoBiology(character As ICharacter, amount As Integer)
        character.ApplyHunger(amount)
    End Sub
    <Extension>
    Private Sub ApplyHunger(character As ICharacter, amount As Integer)
        Dim world = character.World
        Dim stomach = Math.Min(character.GetStomach(), amount)
        amount -= stomach
        If stomach > 0 Then
            world.AddMessage($"{character.Name}'s stomach goes down by {stomach}.")
            character.ChangeCounter(Counters.STOMACH, -stomach)
            world.AddMessage($"{character.Name} now has a stomach of {character.GetStomach}/{character.GetMaximumStomach}.")
            stomach = Math.Min(stomach, character.GetMaximumSatiety - character.GetSatiety)
            If stomach > 0 Then
                world.AddMessage($"{character.Name}'s satiety goes up by {stomach}.")
                character.ChangeCounter(Counters.SATIETY, stomach)
                world.AddMessage($"{character.Name} now has a satiety of {character.GetSatiety}/{character.GetMaximumSatiety}.")
            End If
        End If
        Dim satiety = Math.Min(character.GetSatiety(), amount)
        amount -= satiety
        If satiety > 0 Then
            world.AddMessage($"{character.Name}'s satiety goes down by {satiety}.")
            character.ChangeCounter(Counters.SATIETY, -satiety)
            world.AddMessage($"{character.Name} now has a satiety of {character.GetSatiety}/{character.GetMaximumSatiety}.")
        End If
        Dim health = Math.Min(character.GetHealth(), amount)
        amount -= health
        If health > 0 Then
            world.AddMessage($"{character.Name}'s health goes down by {health}.")
            character.ChangeCounter(Counters.HEALTH, -health)
            world.AddMessage($"{character.Name} now has a health of {character.GetHealth}/{character.GetMaximumHealth}.")
        End If
    End Sub
    <Extension>
    Friend Function IsDead(character As ICharacter) As Boolean
        Return character.IsCounterMinimum(Counters.HEALTH)
    End Function
End Module
