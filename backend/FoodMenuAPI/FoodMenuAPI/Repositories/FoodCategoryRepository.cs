using FoodMenuAPI.Data;
using FoodMenuAPI.Models;
using FoodMenuAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuAPI.Repositories
{
    public class FoodCategoryRepository : IFoodCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodCategory>> GetAllAsync()
        {
            return await _context.FoodCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<FoodCategory?> GetByIdAsync(int id)
        {
            return await _context.FoodCategories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<FoodCategory?> GetByNameAsync(string name)
        {
            return await _context.FoodCategories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task AddAsync(FoodCategory category)
        {
            await _context.FoodCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FoodCategory category)
        {
            _context.FoodCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FoodCategory category)
        {
            _context.FoodCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(
            string name, int? excludeId = null)
        {
            return await _context.FoodCategories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }
    }
}