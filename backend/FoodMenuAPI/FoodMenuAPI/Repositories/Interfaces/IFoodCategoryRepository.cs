using FoodMenuAPI.Models;

namespace FoodMenuAPI.Repositories.Interfaces
{
    public interface IFoodCategoryRepository
    {
        Task<List<FoodCategory>> GetAllAsync();

        Task<FoodCategory?> GetByIdAsync(int id);

        Task<FoodCategory?> GetByNameAsync(string name);

        Task AddAsync(FoodCategory category);

        Task UpdateAsync(FoodCategory category);

        Task DeleteAsync(FoodCategory category);

        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    }
}