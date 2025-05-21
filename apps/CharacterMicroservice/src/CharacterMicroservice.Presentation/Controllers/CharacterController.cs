using System;
using CharacterMicroservice.Application.Commands.CharacterItemsCommands;
using CharacterMicroservice.Application.Commands.CharacterNotesCommands;
using CharacterMicroservice.Application.Commands.CharacterSheetCommands;
using CharacterMicroservice.Application.Commands.CharacterSkillsCommands;
using CharacterMicroservice.Application.Queries;
using CharacterMicroservice.Domain.Models.Entity.Read;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CharacterMicroservice.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharacterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // WRITE SIDE Endpoints
    // --------------------

    [HttpPost]
    public async Task<IActionResult> CreateCharacter([FromBody] CharacterSheet model)
    {
        var command = new CreateCharacterCommand(model);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { characterId = id }, null);
    }

    [HttpPut("{characterId:int}")]
    public async Task<IActionResult> UpdateCharacter(int characterId, [FromBody] CharacterSheet model)
    {
        model.Id = characterId;
        await _mediator.Send(new UpdateCharacterCommand(model));
        return NoContent();
    }

    [HttpDelete("{characterId:int}")]
    public async Task<IActionResult> DeleteCharacter(int characterId)
    {
        await _mediator.Send(new RemoveCharacterCommand(characterId));
        return NoContent();
    }

    [HttpPost("{characterId:int}/items")]
    public async Task<IActionResult> AddItem(int characterId, [FromBody] string itemId)
    {
        var item = new CharacterItems { CharacterId = characterId, ItemId = itemId };
        await _mediator.Send(new CreateCharacterItemCommand(item));
        return NoContent();
    }

    [HttpDelete("{characterId:int}/items/{itemId}")]
    public async Task<IActionResult> RemoveItem(int characterId, string itemId)
    {
        var item = new CharacterItems { CharacterId = characterId, ItemId = itemId };
        await _mediator.Send(new RemoveCharacterItemCommand(item));
        return NoContent();
    }

    [HttpPost("{characterId:int}/skills")]
    public async Task<IActionResult> AddSkill(int characterId, [FromBody] string skillId)
    {
        var skill = new CharacterSkills { CharacterId = characterId, SkillId = skillId };
        await _mediator.Send(new CreateCharacterSkillCommand(skill));
        return NoContent();
    }

    [HttpDelete("{characterId:int}/skills/{skillId}")]
    public async Task<IActionResult> RemoveSkill(int characterId, string skillId)
    {
        var skill = new CharacterSkills { CharacterId = characterId, SkillId = skillId };
        await _mediator.Send(new RemoveCharacterSkillCommand(skill));
        return NoContent();
    }

    [HttpPost("{characterId:int}/notes")]
    public async Task<IActionResult> AddNote(int characterId, [FromBody] string content)
    {
        var note = new CharacterNotes { CharacterId = characterId, Content = content };
        await _mediator.Send(new CreateCharacterNoteCommand(note));
        return NoContent();
    }

    [HttpDelete("{characterId:int}/notes/{noteId:int}")]
    public async Task<IActionResult> RemoveNote(int characterId, int noteId)
    {
        var note = new CharacterNotes { CharacterId = characterId, NoteId = noteId };
        await _mediator.Send(new RemoveCharacterNoteCommand(note));
        return NoContent();
    }

    // READ SIDE Endpoints
    // --------------------

    [HttpGet("{characterId:int}")]
    public async Task<ActionResult<CharacterRead>> GetById(int characterId)
    {
        var result = await _mediator.Send(new GetCharacterByIdQuery(characterId));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CharacterRead>>> GetAll()
    {
        var list = await _mediator.Send(new GetAllCharactersQuery());
        return Ok(list);
    }
}
