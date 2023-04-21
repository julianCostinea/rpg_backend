using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using rpg_backend.Dtos.Skill;
using rpg_backend.Services.SkillService;

namespace rpg_backend.Controllers;

[ApiController]
[EnableCors("localhost")]
[Route("api/[controller]")]
[Authorize]
public class SkillController:ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetSkillDto>>>> GetAllSkills()
    {
        return Ok(await _skillService.GetAllSkills());
    }   
}
