using rpg_backend.Dtos.Skill;

namespace rpg_backend.Services.SkillService;

public interface ISkillService
{
    Task<ServiceResponse<List<GetSkillDto>>> GetAllSkills();
}