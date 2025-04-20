namespace Ordering.Domain.ValueObjects;
public record ProductId
{
    public Guid Value { get; }
    //Al hacer el constructor privado, se impide crear instancias sin pasar por la validación del factory method
    private ProductId(Guid value) => Value = value;

    //static factory method
    //Es una función static que crea y devuelve instancias de una clase, en lugar de invocar directamente el constructor con new
    //Una fábrica estática se utiliza para desacoplar clases y facilitar la modificación de la implementación.
    //Por ejemplo, en lugar de instanciar una conexión a la base de datos mediante el operador `new` en el código cada vez que se necesita
    //una conexión, se utiliza un método de fábrica que devuelve una interfaz:
    //SqlConnection myConnection = new SqlConnection(connectionString);
    //IDbConnection myConnection = myFactory.CreateConnection();
    //La ventaja es que, con solo modificar el método CreateConnection, se pueden realizar cambios globales en todo el proyecto,
    //intercambiando servidores de bases de datos o incluso proveedores de bases de datos sin tener que modificar el código en todos los
    //lugares donde se utiliza la conexión.

    public static ProductId Of(Guid value)
    {
        //Lanza excepcion si es null. Manera simplificada en vez de hacer un if y dentro del if hacer excepcion
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("ProductId cannot be empty.");
        }

        return new ProductId(value);
    }
}