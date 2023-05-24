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

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetAllCharacters_NotAuthorized_Returns401()
    {
        var mockCharacters = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = new List<GetCharacterDto>()
            {
                new GetCharacterDto { Name = "A", Id = 1 },
                new GetCharacterDto { Name = "B", Id = 2 },
            }
        };

        //mock serviceResponse to return mockCharacters

        _factory.CharacterServiceMock.Setup(r => r.GetAllCharacters()).ReturnsAsync(mockCharacters);

        var response = await _client.GetAsync("/api/Character/GetAll");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

        // var data = JsonConvert.DeserializeObject<IEnumerable<Character>>(await response.Content.ReadAsStringAsync());

        // for (var i = 0; i < data.Count(); i++)
        // {
        //     Assert.That(data.ElementAt(i).Name, Is.EqualTo(mockCharacters.ElementAt(i).Name));
        // }

        // Assert.That(data.Count(), Is.EqualTo(2));
        //
        // _factory.CharacterServiceMock.Verify(r => r.GetAllCharacters(), Times.Once);
    }

    [Test]
    public async Task GetAllCharacters_Authorized_Returns200()
    {
        var mockCharacters = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = null,
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
    }
    
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}