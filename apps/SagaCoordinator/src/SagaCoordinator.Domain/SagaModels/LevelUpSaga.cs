using SagaCoordinator.Domain.Interfaces;

namespace SagaCoordinator.Domain.SagaModels;

public class LevelUpSaga : ISaga
{
    public Guid SagaId { get; set; }  
    public int CharacterId { get; set; }  
    public int Level { get; set; }  
    public SagaState State { get; set; } 
}