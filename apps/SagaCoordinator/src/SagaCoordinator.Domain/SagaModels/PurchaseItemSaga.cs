using SagaCoordinator.Domain.Interfaces;

namespace SagaCoordinator.Domain.SagaModels;

public class PurchaseItemSaga : ISaga
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }  
    public int GoldAmount { get; set; }  
    public SagaState State { get; set; }  
}