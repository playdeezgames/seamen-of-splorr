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

    Public Overrides Function Run() As IDialogPrompt
        If Model.Ad.InProgress Then
            Return AdPrompt.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.Unfouling.Active Then
            Return UnfoulingMenu.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.Selling.Active Then
            Return SellingMenu.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.Buying.Active Then
            Return BuyingMenu.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.KnownIslands.IsPicking Then
            Return ChooseKnownIslandPrompt.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.Ship.IsSettingHeading Then
            Return SetHeadingPrompt.Launch(Context, Model, Previous).Invoke().Run()
        End If
        If Model.Avatar.Ship.IsSettingSpeed Then
            Return SetSpeedPrompt.Launch(Context, Model, Previous).Invoke().Run()
        End If
        Return NavigationMenu.Launch(Context, Model, Previous).Invoke().Run()
    End Function
End Class
