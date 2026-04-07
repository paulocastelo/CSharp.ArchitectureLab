using System.ComponentModel.DataAnnotations;

namespace ResultPattern.Sample.Application;

public sealed class CreateProductRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
