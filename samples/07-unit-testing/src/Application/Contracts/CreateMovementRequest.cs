using System.ComponentModel.DataAnnotations;
using UnitTesting.Sample.Domain;

namespace UnitTesting.Sample.Application.Contracts;

public sealed class CreateMovementRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public StockMovementType Type { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
