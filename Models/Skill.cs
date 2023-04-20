namespace rpg_backend.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = "Fists";
    public int Damage { get; set; } = 10;
    public List<Character> Characters { get; set; }
}