namespace UnitTesting.Sample.Domain;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int CurrentBalance { get; set; }
}
