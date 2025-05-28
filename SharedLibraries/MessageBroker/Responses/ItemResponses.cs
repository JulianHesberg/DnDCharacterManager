using MessageBroker.Interfaces;

namespace SagaCoordinator.Domain.ResponseModels;

public class Item
{
    public Guid SagaId { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
}

public class ItemListResponse : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public List<Item> Items { get; set; }
    public string MessageType => nameof(ItemListResponse);
}

public class ItemCostResponse : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string MessageType => nameof(ItemCostResponse);
}

public class ItemCraftedResponse : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public string MessageType => nameof(ItemCraftedResponse);
}
