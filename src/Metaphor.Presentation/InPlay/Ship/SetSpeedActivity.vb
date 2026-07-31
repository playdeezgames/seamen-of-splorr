Imports Metaphor.Processing
Imports TGGD.Presentation

Friend Module SetSpeedActivity
    Friend Function Launch(context As IDisplayContext, model As IWorldModel, previous As DialogSource, speed As Double) As DialogSource
        Return Function()
                   model.Avatar.Ship.SetSpeed(speed)
                   Return InPlay.Launch(context, model, previous).Invoke()
               End Function
    End Function
End Module
