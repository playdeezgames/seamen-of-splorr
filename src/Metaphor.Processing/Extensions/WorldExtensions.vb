Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module WorldExtensions
    <Extension>
    Function GetCommodities(world As IWorld) As IEnumerable(Of ICommodity)
        Return world.CommodityIds.Select(Function(x) world.Commodities(x))
    End Function
End Module
