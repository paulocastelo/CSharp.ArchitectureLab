namespace UnitTesting.Sample.Domain;

public sealed class StockMovement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
