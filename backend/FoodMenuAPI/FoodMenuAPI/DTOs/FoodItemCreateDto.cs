using System.ComponentModel.DataAnnotations;

namespace FoodMenuAPI.DTOs
{
    public class FoodItemCreateDto
    {
        [Required(ErrorMessage = "Food name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Food category is required.")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "99999999.99")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}