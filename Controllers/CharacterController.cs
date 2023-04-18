using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using rpg_backend.Dtos.Character;
using rpg_backend.Services.CharacterService;

namespace rpg_backend.Controllers;

[EnableCors("localhost")]
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CharacterController: ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id));
    }
    
    // [AllowAnonymous]
    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
        //below now solved by httpContextAccessor
        // int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(await _characterService.GetAllCharacters());
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = await _characterService.AddCharacter(newCharacter);
        if (response.Data == null)
        {
            response.Message = "Character could not be added";
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    
}