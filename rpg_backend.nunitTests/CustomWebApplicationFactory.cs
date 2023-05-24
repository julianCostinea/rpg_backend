using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using rpg_backend.Data;
using rpg_backend.Services.CharacterService;

namespace rpg_backend.nunitTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    public Mock<ICharacterService> CharacterServiceMock { get; }

    public CustomWebApplicationFactory()
    {
        CharacterServiceMock = new Mock<ICharacterService>();
    }
    
    
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(CharacterServiceMock.Object);
        });
    }
}