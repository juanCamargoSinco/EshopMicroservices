namespace Ordering.Domain.ValueObjects;
//Los value objects tambien pueden tener reglas de negocio
public record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

        return new OrderName(value);
    }
}