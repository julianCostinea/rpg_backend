using AutoMapper;
using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Fight;
using rpg_backend.Dtos.Skill;

namespace rpg_backend;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<Skill, GetSkillDto>();
        CreateMap<Character, HighscoreDto>();
    }
}