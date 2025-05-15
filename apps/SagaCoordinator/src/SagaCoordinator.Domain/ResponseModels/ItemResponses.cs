namespace SagaCoordinator.Domain.ResponseModels;

public class Item
{
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ItemListResponse
{
    public List<Item> Items { get; set; }
}

public class ItemCostResponse
{
    public string ItemId { get; set; }
    public decimal Price { get; set; }
}