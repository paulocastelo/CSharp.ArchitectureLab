namespace DtoMapping.Sample.Domain;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal InternalCost { get; set; }
    public string InternalNotes { get; set; } = string.Empty;
}
