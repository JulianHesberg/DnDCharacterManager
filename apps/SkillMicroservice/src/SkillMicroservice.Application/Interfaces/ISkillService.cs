using SkillMicroservice.Application.Dtos;

namespace SkillMicroservice.Application;

public interface ISkillService
{
	public  Task<SkillDto> GetSkillByIdAsync(int id);
	public  Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
	public  Task<SkillDto> CreateSkillAsync(CreateSkillDto skill);
	public  Task<SkillDto> UpdateSkillAsync(UpdateSkillDto skill);
	public  Task<SkillDto> DeleteSkillAsync(int id);

}