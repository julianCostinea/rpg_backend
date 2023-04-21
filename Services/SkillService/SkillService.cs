using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rpg_backend.Data;
using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Skill;

namespace rpg_backend.Services.SkillService;

public class SkillService:ISkillService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SkillService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ServiceResponse<List<GetSkillDto>>> GetAllSkills()
    {
        var response = new ServiceResponse<List<GetSkillDto>>();
        var dbSkills =  _context.Skills.Select(c => _mapper.Map<GetSkillDto>(c)).ToList();
            

        response.Data = dbSkills;
        return response;
    }
}
