Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence
Friend Delegate Sub ItemGenerator(inventory As IInventory)
Friend Module InventoryExtensions
    Private ReadOnly itemTypeNames As New Dictionary(Of String, String) From
        {
            {ItemSubtypes.HARDTACK, "Hardtack"},
            {ItemSubtypes.BAG_O_GRAIN, "Bag O'Grain"}
        }
    Private ReadOnly itemGenerator As New Dictionary(Of String, ItemGenerator) From
        {
            {ItemSubtypes.HARDTACK, AddressOf CreateHardtack},
            {ItemSubtypes.BAG_O_GRAIN, AddressOf CreateBagOGrain}
        }

    Private Sub CreateBagOGrain(inventory As IInventory)
        inventory.CreateItem(
            ItemSubtypes.BAG_O_GRAIN,
            GetItemTypeName(ItemSubtypes.BAG_O_GRAIN),
            "This is a bag of grain. No, it isn't Irish. The `O'` means `of`!")
    End Sub

    Private Sub CreateHardtack(inventory As IInventory)
        inventory.CreateItem(
            ItemSubtypes.HARDTACK,
            GetItemTypeName(ItemSubtypes.HARDTACK),
            "This is food, technically.",
            AddressOf InitializeHardtack)
    End Sub
    <Extension>
    Friend Sub CreateItemOfType(inventory As IInventory, itemType As String)
        itemGenerator(itemType).Invoke(inventory)
    End Sub

    Private Sub InitializeHardtack(item As IItem)
        item.SetCounter(Counters.STOMACH, 10)
        item.CreateVerb(VerbSubtypes.EAT, "Eat", $"You eat the {item.Name}.")
    End Sub
    <Extension>
    Friend Function CreateDeliveryItem(inventory As IInventory, recipient As ICharacter) As IItem
        Dim item = inventory.CreateItem(ItemSubtypes.PACKAGE, "Package", "Its a package. You deliver them.")
        item.SetRecipient(recipient)
        Return item
    End Function

    Friend Function GetItemTypeName(itemType As String) As String
        Return itemTypeNames(itemType)
    End Function
End Module
