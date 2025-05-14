using Microsoft.AspNetCore.Mvc;
using SkillMicroservice.Application;
using SkillMicroservice.Application.Dtos;

namespace SkillMicroservice.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillService _service;

    public SkillController(ISkillService service)
    {
        _service = service;
    }

    // GET: api/skill
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _service.GetAllSkillsAsync();
        return Ok(skills);
    }

    // GET: api/skill/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _service.GetSkillByIdAsync(id);
        if (skill == null)
            return NotFound();

        return Ok(skill);
    }

    // POST: api/skill
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateSkillAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/skill/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var updated = await _service.UpdateSkillAsync(dto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/skill/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteSkillAsync(id);
        if (deleted == null)
            return NotFound();

        return Ok(deleted);
    }
}
