namespace MessageBroker.Requests;

public class PurchaseItemRequest
{
    public int CharacterId { get; set; }
    public int GoldAmount { get; set; }
}

public class SellItemRequest
{
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
}

public class CraftItemRequest
{
    public int CharacterId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}