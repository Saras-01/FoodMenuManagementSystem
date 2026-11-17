using FoodMenuAPI.DTOs.FoodCategoryDtos;

namespace FoodMenuAPI.Services.Interfaces
{
    public interface IFoodCategoryService
    {
        Task<List<FoodCategoryResponseDto>> GetAllAsync();

        Task<FoodCategoryResponseDto?> GetByIdAsync(int id);

        Task<FoodCategoryResponseDto> CreateAsync(
            FoodCategoryCreateDto dto);

        Task<FoodCategoryResponseDto?> UpdateAsync(
            int id, FoodCategoryUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}