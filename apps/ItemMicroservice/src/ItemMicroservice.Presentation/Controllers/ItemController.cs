using ItemMicroservice.Application.Service.Interfaces;
using ItemMicroservice.Domain.Entities;
using ItemMicroservice.Domain.Entities.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ItemMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemInputDto dto)
        {
            var item = new Item
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };

            await _service.CreateAsync(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ItemInputDto dto)
        {
            var updatedItem = new Item
            {
                Id = id,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };

            await _service.UpdateAsync(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
