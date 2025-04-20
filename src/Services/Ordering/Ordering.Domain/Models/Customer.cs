namespace Ordering.Domain.Models;
public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    //Factory method
    //Responsable de crear su propia instancia y mantener su estado
    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = id, //Id heredado de la clase base Entity a la cual se le esta diciendo que el Id sera de tipo CustomerId
            Name = name,
            Email = email
        };

        return customer;
    }
}