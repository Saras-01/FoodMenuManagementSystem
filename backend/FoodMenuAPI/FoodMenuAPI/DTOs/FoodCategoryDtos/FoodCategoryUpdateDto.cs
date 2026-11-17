using System.ComponentModel.DataAnnotations;

namespace FoodMenuAPI.DTOs.FoodCategoryDtos
{
    public class FoodCategoryUpdateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250,
            ErrorMessage = "Description cannot exceed 250 characters.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}