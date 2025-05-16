namespace SagaCoordinator.Domain.RequestModels;

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