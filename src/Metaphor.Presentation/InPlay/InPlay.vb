Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Class InPlay
    Inherits MetaphorDialog

    Private Sub New(context As IDisplayContext, model As IWorldModel, previous As DialogSource)
        MyBase.New(context, model, previous)
    End Sub

    Friend Shared Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource) As DialogSource
        Return Function() New InPlay(context, model, previous)
    End Function

    Private Delegate Function LaunchDelegate(
                                     context As IDisplayContext,
                                     model As IWorldModel,
                                     previous As DialogSource) As DialogSource

    Private modeLaunchers As New Dictionary(Of String, LaunchDelegate) From
        {
            {Modes.PICKING_KNOWN_BUBBLE, AddressOf ChooseKnownBubblePrompt.Launch},
            {Modes.SETTING_HEADING, AddressOf SetHeadingPrompt.Launch},
            {Modes.SETTING_SPEED, AddressOf SetSpeedPrompt.Launch},
            {Modes.SETTING_HYDROPLANE, AddressOf SetHydroplanePrompt.Launch},
            {Modes.BUYING_FUEL, AddressOf BuyFuelPrompt.Launch}
        }

    Public Overrides Function Run() As IDialogPrompt
        If Model.Ad.InProgress Then
            Return AdPrompt.Launch(Context, Model, Previous).Invoke().Run()
        End If
        Dim launchDelgate As LaunchDelegate = Nothing
        If modeLaunchers.TryGetValue(Model.Avatar.Mode, launchDelgate) Then
            Return launchDelgate.Invoke(Context, Model, Previous).Invoke.Run()
        End If
        Return NavigationMenu.Launch(Context, Model, Previous).Invoke().Run()
    End Function
End Class
