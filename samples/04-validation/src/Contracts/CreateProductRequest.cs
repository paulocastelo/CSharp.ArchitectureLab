using System.ComponentModel.DataAnnotations;

namespace Validation.Sample.Contracts;

public sealed class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price must be positive.")]
    public decimal UnitPrice { get; set; }
}
