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

        // Get all food items
        public async Task<List<FoodItemResponseDto>> GetAllAsync()
        {
            var foodItems = await _repository.GetAllAsync();

            return foodItems.Select(f => new FoodItemResponseDto
            {
                Id = f.Id,
                Name = f.Name,
                Category = f.Category,
                Description = f.Description,
                Price = f.Price,
                IsAvailable = f.IsAvailable
            }).ToList();
        }

        // Get food item by ID
        public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return null;

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

        // Create food item
        public async Task<FoodItemResponseDto> CreateAsync(
            FoodItemCreateDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
            {
                throw new InvalidOperationException(
                    "A food item with this name already exists.");
            }

            var foodItem = new FoodItem
            {
                Name = dto.Name,
                Category = dto.Category,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _repository.AddAsync(foodItem);

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

        // Update food item
        public async Task<FoodItemResponseDto?> UpdateAsync(
            int id,
            FoodItemUpdateDto dto)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return null;

            if (await _repository.ExistsByNameAsync(dto.Name, id))
            {
                throw new InvalidOperationException(
                    "Another food item with this name already exists.");
            }

            foodItem.Name = dto.Name;
            foodItem.Category = dto.Category;
            foodItem.Description = dto.Description;
            foodItem.Price = dto.Price;
            foodItem.IsAvailable = dto.IsAvailable;

            await _repository.UpdateAsync(foodItem);

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

        // Delete food item
        public async Task<bool> DeleteAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return false;

            await _repository.DeleteAsync(foodItem);

            return true;
        }
    }
}