using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using rpg_backend.Dtos.Fight;
using rpg_backend.Services.FightService;

namespace rpg_backend.Controllers;

[ApiController]
[EnableCors("localhost")]
[Route("api/[controller]")]
public class FightController:ControllerBase
{
    private readonly IFightService _fightService;
    
    

    public FightController(IFightService fightService)
    {
        _fightService = fightService;
    }
    
    [HttpPost("Weapon")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
    {
        return Ok(await _fightService.WeaponAttack(request));
    }
    
    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
    {
        return Ok(await _fightService.SkillAttack(request));
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
    {
        return Ok(await _fightService.Fight(request));
    }
    
    [HttpGet("Highscore")]
    public async Task<ActionResult<ServiceResponse<List<HighscoreDto>>>> GetHighscore()
    {
        return Ok(await _fightService.GetHighscore());
    }
}