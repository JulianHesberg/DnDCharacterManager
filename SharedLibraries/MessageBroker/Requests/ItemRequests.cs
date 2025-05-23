using MessageBroker.Interfaces;

namespace MessageBroker.Requests;

public class PurchaseItemRequest : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public int GoldAmount { get; set; }
    public string MessageType => nameof(PurchaseItemRequest);
}

public class SellItemRequest : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public string MessageType => nameof(SellItemRequest);
}

public class CraftItemRequest : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MessageType => nameof(CraftItemRequest);
}

public class RollbackItemCraftedRequest : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public string MessageType => nameof(RollbackItemCraftedRequest);
}