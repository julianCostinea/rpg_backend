using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rpg_backend.Data;
using rpg_backend.Dtos.Character;

namespace rpg_backend.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var dbCharacters = await _context.Characters
            .Include(c=>c.Weapon)
            .Include(c=>c.Skills)
            .Where(c => c.User.Id == GetUserId()).ToListAsync();
        if (dbCharacters.Count == 0)
        {
            response.Message = "No characters found";
            return response;
        }

        response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
        serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        Character character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        //no need for async as we are not waiting for the result
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        serviceResponse.Data = await _context.Characters.Where(c => c.User.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
        serviceResponse.Message = "Character added successfully";

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            // character.Name = updatedCharacter.Name;
            // character.Class = updatedCharacter.Class;
            // character.Defense = updatedCharacter.Defense;
            // character.HitPoints = updatedCharacter.HitPoints;
            // character.Intelligence = updatedCharacter.Intelligence;
            // character.Strength = updatedCharacter.Strength;

            if (character.User.Id == GetUserId())
            {
                _mapper.Map(updatedCharacter, character);

                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
                response.Message = "Character updated successfully";
            } else
            {
                response.Success = false;
                response.Message = "Character not found";
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            Character character =
                await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                response.Data = _context.Characters.Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            else
            {
                response.Success = false;
                response.Message = "Character not found";
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
        try
        {
            Character character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
            if (character == null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            Skill skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
            if (skill == null)
            {
                response.Success = false;
                response.Message = "Skill not found";
                return response;
            }

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            response.Message = "Skill added to character";
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }
}