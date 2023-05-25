using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using rpg_backend.Data;
using rpg_backend.Dtos.Character;
using rpg_backend.Dtos.Skill;
using rpg_backend.Dtos.User;
using rpg_backend.Models;

namespace rpg_backend.nunitTests;

[TestFixture]
public class CharacterControllerTests : IDisposable
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    public CharacterControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }
    
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetAllCharacters_NotAuthorized_Returns401()
    {
        
        var response = await _client.GetAsync("/api/Character/GetAll");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAllCharacters_Authorized_Returns200()
    {
        var mockCharacters = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = new List<GetCharacterDto>()
            {
                new GetCharacterDto()
                {
                    Id = 2,
                    Name = "Oak",
                    HitPoints = 100,
                    Strength = 10,
                    Defense = 10,
                    Intelligence = 10,
                }
            },
            Success = true,
            Message = "Success"
        };

        _factory.CharacterServiceMock.Setup(r => r.GetAllCharacters()).ReturnsAsync(mockCharacters);

        var user = new UserLoginDto()
        {
            Username = "oak",
            Password = "1"
        };

        var res = await _client.PostAsync("auth/login", JsonContent.Create(user));
        var token = JsonConvert.DeserializeObject<ServiceResponse<string>>(await res.Content.ReadAsStringAsync()).Data;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/Character/GetAll");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var data = JsonConvert.DeserializeObject<ServiceResponse<List<GetCharacterDto>>>(await response.Content.ReadAsStringAsync());
        Assert.That(data.Data.ElementAt(0).Name, Is.EqualTo("Oak"));
    }
}