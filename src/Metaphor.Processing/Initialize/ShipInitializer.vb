Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module ShipInitializer
    Friend Function Initialize(context As IInitializationContext) As Persistence.LocationInitializer
        Return Sub(ship)
                   context.Ship = ship
                   ship.InitializeDimension(Dimensions.HEADING, RNG.FromRange(HEADING_MINIMUM, HEADING_MAXIMUM), HEADING_MINIMUM, HEADING_MAXIMUM)
                   ship.InitializeDimension(Dimensions.SPEED, SPEED_AHEAD_TWO_THIRDS, SPEED_FULL_STOP, SPEED_AHEAD_FLANK)
                   ship.InitializeDimension(Dimensions.LONGITUDE, context.WorldWidth / 2, 0.0, context.WorldWidth)
                   ship.InitializeDimension(Dimensions.LATITUDE, context.WorldHeight / 2, 0.0, context.WorldHeight)
                   ship.InitializeDimension(Dimensions.DEPTH, (context.SnorkelDepth + context.WorldDepth) / 2, context.SnorkelDepth, context.WorldDepth)
                   ship.InitializeDimension(Dimensions.HYDROPLANE, 0.0, MINIMUM_HYDROPLANE, MAXIMUM_HYDROPLANE)
                   ship.InitializeCounter(Counters.OXYGEN, 500, 0, 1000)
                   ship.InitializeDimension(Dimensions.BATTERY, 500, 0, 1000)
                   ship.InitializeDimension(Dimensions.DIESEL, 500, 0, 1000)
                   ship.SetDimension(Dimensions.VISIBILITY, 100.0)
                   ship.CreateVerb(VerbTypes.MOVE, "Move", "Steady as she goes.")
                   ship.CreateVerb(VerbTypes.DOCK, "Dock", "You moor the ship to the pier.")
                   ship.CreateVerb(VerbTypes.UNDOCK, "Undock", "You cast away from the pier.")
                   ship.CreateVerb(VerbTypes.SET_HEADING, "Set Heading", "You use the helm to set a new heading.")
                   ship.CreateVerb(VerbTypes.SET_SPEED, "Set Speed", "You use the engines to set a new speed.")
                   ship.CreateVerb(VerbTypes.SET_HYDROPLANE, "Set Hydroplane", "You angle the hydroplane to change depth when moving.")
                   ship.CreateVerb(VerbTypes.DISEMBARK, "Disembark", "You step off the ship.")
                   ship.CreateVerb(VerbTypes.RAISE_SNORKEL, "Raise Snorkel", "You raise the snorkel mast.")
                   ship.CreateVerb(VerbTypes.LOWER_SNORKEL, "Lower Snorkel", "You lowerr the snorkel mast.")
                   ship.CreateCharacter(CharacterTypes.N00B, context.ChosenName, context.ChosenPronouns, $"{context.ChosenName}'s pronouns are {context.ChosenPronouns}.", InitializeAvatar(context))
                   ship.CreateFeature(FeatureTypes.CARGO_HOLD, "Cargo Hold", "This is the cargo hold. It is where you hold yer cargo.", AddressOf InitializeCargoHold)
               End Sub
    End Function

    Private Sub InitializeCargoHold(feature As IFeature)
#If DEBUG Then
        Utility.Repeat(100, Sub() feature.Inventory.CreateItemOfType(ItemTypes.HARDTACK))
#End If
    End Sub

    Private Function InitializeAvatar(context As IInitializationContext) As CharacterInitializer
        Return Sub(character)
                   character.World.Avatar = character
                   character.SetShip(character.Location)
                   character.InitializeCounter(Counters.FLESH_GRAMS, 454, 0, 454)
                   character.InitializeCounter(Counters.HEALTH, 100, 0, 100)
                   character.InitializeCounter(Counters.SATIETY, 100, 0, 100)
                   character.InitializeCounter(Counters.STOMACH, 0, 0, 50)
#If DEBUG Then
                   character.InitializeDimension(Dimensions.JOOLS, 100.0, 0.0, Double.MaxValue)
#Else
                   character.InitializeDimension(Dimensions.JOOLS, 0.0, 0.0, Double.MaxValue)
#End If
                   Utility.Repeat(10, Sub() character.Inventory.CreateItemOfType(ItemTypes.HARDTACK))
                   character.CreateVerb(VerbTypes.HEAD_FOR_KNOWN_BUBBLE, "Head for known bubble...", String.Empty)
               End Sub
    End Function
End Module
