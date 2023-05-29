using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using rpg_backend.Controllers;
using rpg_backend.Data;
using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Skill;
using rpg_backend.Models;
using rpg_backend.Services.CharacterService;
using rpg_backend.Services.SkillService;

namespace rpg_backend.nunitTests;

[TestFixture]
public class CharacterControllerInMemoryTests
{
    [Test]
    public void GetAllSkills_Always_ReturnsCorrect()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "rpg_backend")
            .Options;

        using (var context = new DataContext(options))
        {
            context.Skills.Add(new Skill {Id = 1, Name = "Fireball", Damage = 30});
            context.Skills.Add(new Skill {Id = 2, Name = "Frenzy", Damage = 20});
            context.SaveChanges();
        }
        
        using (var context = new DataContext(options))
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var service = new SkillService(mapper, context, null);

            var result = service.GetAllSkills().Result;
            var skills = result.Data;
            
            Assert.That(skills.Count, Is.EqualTo(2));

        }
    }
}