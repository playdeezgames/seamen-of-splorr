Imports System.Runtime.CompilerServices

Public Module Utility
    Friend Function ToRadians(degrees As Double) As Double
        Return degrees * Math.PI / 180.0
    End Function
    Friend Function Distance(fromPosition As (Longitude As Double, Latitude As Double), toPosition As (Longitude As Double, Latitude As Double)) As Double
        Return Math.Sqrt((fromPosition.Longitude - toPosition.Longitude) * (fromPosition.Longitude - toPosition.Longitude) + (fromPosition.Latitude - toPosition.Latitude) * (fromPosition.Latitude - toPosition.Latitude))
    End Function
    Friend Sub Repeat(iterations As Integer, activity As Action)
        For Each iteration In Enumerable.Range(1, iterations)
            activity.Invoke()
        Next
    End Sub
    <Extension>
    Public Function DescribeHydroplane(hydroplane As Double) As String
        If hydroplane > 0.0 Then
            Return $"{Math.Abs(hydroplane):f2}° Down Bubble"
        ElseIf hydroplane < 0.0 Then
            Return $"{Math.Abs(hydroplane):f2}° Up Bubble"
        Else
            Return "Zero Bubble"
        End If
    End Function
End Module
