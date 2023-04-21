using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Weapon;
using rpg_backend.Services.WeaponService;

namespace rpg_backend.Controllers;

[ApiController]
[EnableCors("localhost")]
[Route("api/[controller]")]
[Authorize]
public class WeaponController:ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
    {
        return Ok(await _weaponService.AddWeapon(newWeapon));
    }   
}