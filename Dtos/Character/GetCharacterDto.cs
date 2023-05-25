using rpg_backend.Dtos.Skill;

namespace rpg_backend.Dtos.Character;

public class GetCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Frodo";
    public int HitPoints { get; set; } = 100;
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    // public RpgClass Class { get; set; } = RpgClass.Knight;
    // //this needs automapperprofile
    // public GetWeaponDto Weapon { get; set; }
    // //also needs automapperprofile
    // public List<GetSkillDto> Skills { get; set; }
    public int Fights { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }
}