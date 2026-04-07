using System.ComponentModel.DataAnnotations;

namespace DtoMapping.Sample.Application.Contracts;

public sealed class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal InternalCost { get; set; }

    [MaxLength(200)]
    public string InternalNotes { get; set; } = string.Empty;
}
