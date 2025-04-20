namespace Ordering.Domain.ValueObjects;
public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;
    //Provee una manera de crear entidades aplicando las relgas o validaciones de dominio
    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }

        return new CustomerId(value);
    }
}