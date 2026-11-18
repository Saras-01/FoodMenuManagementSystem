using FoodMenuAPI.DTOs;
using FoodMenuAPI.Models;
using FoodMenuAPI.Repositories.Interfaces;
using FoodMenuAPI.Services.Interfaces;

namespace FoodMenuAPI.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _repository;

        public FoodItemService(IFoodItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FoodItemResponseDto>> GetAllAsync()
        {
            var foodItems = await _repository.GetAllAsync();

            return foodItems.Select(MapToResponseDto).ToList();
        }

        public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            return foodItem == null ? null : MapToResponseDto(foodItem);
        }

        public async Task<FoodItemResponseDto> CreateAsync(FoodItemCreateDto dto)
        {
            string name = dto.Name.Trim();

            if (await _repository.ExistsByNameAsync(name))
            {
                throw new InvalidOperationException(
                    "A food item with this name already exists.");
            }

            var foodItem = new FoodItem
            {
                Name = name,
                Category = dto.Category.Trim(),
                Description = dto.Description?.Trim(),
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _repository.AddAsync(foodItem);

            return MapToResponseDto(foodItem);
        }

        public async Task<FoodItemResponseDto?> UpdateAsync(
            int id,
            FoodItemUpdateDto dto)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
            {
                return null;
            }

            string name = dto.Name.Trim();

            if (await _repository.ExistsByNameAsync(name, id))
            {
                throw new InvalidOperationException(
                    "Another food item with this name already exists.");
            }

            foodItem.Name = name;
            foodItem.Category = dto.Category.Trim();
            foodItem.Description = dto.Description?.Trim();
            foodItem.Price = dto.Price;
            foodItem.IsAvailable = dto.IsAvailable;

            await _repository.UpdateAsync(foodItem);

            return MapToResponseDto(foodItem);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
            {
                return false;
            }

            await _repository.DeleteAsync(foodItem);

            return true;
        }

        private static FoodItemResponseDto MapToResponseDto(FoodItem foodItem)
        {
            return new FoodItemResponseDto
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Category = foodItem.Category,
                Description = foodItem.Description,
                Price = foodItem.Price,
                IsAvailable = foodItem.IsAvailable
            };
        }
    }
}