namespace SagaCoordinator.Domain.RequestModels;

public class LevelUpRequest
{
    public int CharacterId { get; set; }
    public int Level { get; set; }
}