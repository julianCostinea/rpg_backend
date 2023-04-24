using Microsoft.AspNetCore.Mvc;
using rpg_backend.Dtos.Fight;
using rpg_backend.Services.FightService;

namespace rpg_backend.Controllers;

[ApiController]
[Route("[controller]")]
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
}