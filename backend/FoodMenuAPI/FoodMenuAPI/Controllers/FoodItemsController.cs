using FoodMenuAPI.DTOs;
using FoodMenuAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly IFoodItemService _service;

        public FoodItemsController(IFoodItemService service)
        {
            _service = service;
        }

        // GET: api/fooditems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var foodItems = await _service.GetAllAsync();
            return Ok(foodItems);
        }

        // GET: api/fooditems/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodItem = await _service.GetByIdAsync(id);

            if (foodItem == null)
                return NotFound(new { message = "Food item not found." });

            return Ok(foodItem);
        }

        // POST: api/fooditems
        [HttpPost]
        public async Task<IActionResult> Create(FoodItemCreateDto dto)
        {
            try
            {
                var createdItem = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdItem.Id },
                    createdItem);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/fooditems/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            FoodItemUpdateDto dto)
        {
            try
            {
                var updatedItem = await _service.UpdateAsync(id, dto);

                if (updatedItem == null)
                    return NotFound(new { message = "Food item not found." });

                return Ok(updatedItem);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/fooditems/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Food item not found." });

            return NoContent();
        }
    }
}