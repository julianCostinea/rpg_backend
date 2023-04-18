namespace rpg_backend.Models;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; } = "Fists";
    public int Damage { get; set; } = 10;
    public Character Character { get; set; }
}