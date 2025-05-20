namespace SagaCoordinator.Domain.ResponseModels;

public class Item
{
    public Guid SagaId { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ItemListResponse
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public List<Item> Items { get; set; }
}

public class ItemCostResponse
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
}

public class ItemCraftedResponse
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
}
