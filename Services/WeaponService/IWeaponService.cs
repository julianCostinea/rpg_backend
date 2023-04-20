using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Weapon;

namespace rpg_backend.Services.WeaponService;

public interface IWeaponService
{
    Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
}