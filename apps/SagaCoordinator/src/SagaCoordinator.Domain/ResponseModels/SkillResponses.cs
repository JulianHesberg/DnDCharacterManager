namespace SagaCoordinator.Domain.ResponseModels;

public class Skill
{
    public string SkillId { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }
}

public class SkillListResponse
{
    public List<Skill> Skills { get; set; }
}