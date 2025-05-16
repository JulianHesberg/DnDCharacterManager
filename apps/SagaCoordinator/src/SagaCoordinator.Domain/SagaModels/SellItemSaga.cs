using SagaCoordinator.Domain.Interfaces;

namespace SagaCoordinator.Domain.SagaModels;

public class SellItemSaga : ISaga
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }  
    public string ItemId { get; set; }  
    public SagaState State { get; set; }  
}