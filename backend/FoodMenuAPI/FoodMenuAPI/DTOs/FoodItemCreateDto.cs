using System.ComponentModel.DataAnnotations;

public class FoodItemCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;
}