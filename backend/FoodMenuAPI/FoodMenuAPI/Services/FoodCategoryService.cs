using FoodMenuAPI.DTOs.FoodCategoryDtos;
using FoodMenuAPI.Models;
using FoodMenuAPI.Repositories.Interfaces;
using FoodMenuAPI.Services.Interfaces;

namespace FoodMenuAPI.Services
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _repository;

        public FoodCategoryService(
            IFoodCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FoodCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(category =>
                MapToResponseDto(category)).ToList();
        }

        public async Task<FoodCategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            return MapToResponseDto(category);
        }

        public async Task<FoodCategoryResponseDto> CreateAsync(
            FoodCategoryCreateDto dto)
        {
            string categoryName = dto.Name.Trim();

            bool nameExists =
                await _repository.ExistsByNameAsync(categoryName);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "A category with this name already exists.");
            }

            var category = new FoodCategory
            {
                Name = categoryName,
                Description = dto.Description?.Trim(),
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(category);

            return MapToResponseDto(category);
        }

        public async Task<FoodCategoryResponseDto?> UpdateAsync(
            int id, FoodCategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            string categoryName = dto.Name.Trim();

            bool nameExists =
                await _repository.ExistsByNameAsync(categoryName, id);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "A category with this name already exists.");
            }

            category.Name = categoryName;
            category.Description = dto.Description?.Trim();
            category.IsActive = dto.IsActive;

            await _repository.UpdateAsync(category);

            return MapToResponseDto(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return false;
            }

            await _repository.DeleteAsync(category);

            return true;
        }

        private static FoodCategoryResponseDto MapToResponseDto(
            FoodCategory category)
        {
            return new FoodCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
        }
    }
}