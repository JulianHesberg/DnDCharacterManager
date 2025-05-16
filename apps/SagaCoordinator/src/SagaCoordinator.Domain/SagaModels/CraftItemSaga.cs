using SagaCoordinator.Domain.Interfaces;

namespace SagaCoordinator.Domain.SagaModels;

public class CraftItemSaga : ISaga
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SagaState State { get; set; }
}