using System.ComponentModel.DataAnnotations;

namespace RepositoryPattern.Sample.Application.Contracts;

public sealed class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
