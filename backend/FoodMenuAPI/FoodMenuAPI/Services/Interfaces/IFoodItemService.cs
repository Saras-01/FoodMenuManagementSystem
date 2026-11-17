using FoodMenuAPI.DTOs;

namespace FoodMenuAPI.Services.Interfaces
{
    public interface IFoodItemService
    {
        Task<List<FoodItemResponseDto>> GetAllAsync();

        Task<FoodItemResponseDto?> GetByIdAsync(int id);

        Task<FoodItemResponseDto> CreateAsync(FoodItemCreateDto dto);

        Task<FoodItemResponseDto?> UpdateAsync(
            int id,
            FoodItemUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}