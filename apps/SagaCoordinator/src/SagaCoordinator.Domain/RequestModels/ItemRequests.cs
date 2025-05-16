namespace SagaCoordinator.Domain.RequestModels;

public class PurchaseItemRequest
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public int GoldAmount { get; set; }
}

public class SellItemRequest
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
}

public class CraftItemRequest
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}