namespace Ordering.Domain.Models;
//Primitive obsession
//Se refiere a el uso excesivo de tipos de datos primitivos (como int, string, float, etc.)
//en lugar de crear objetos que representen conceptos del dominio de tu aplicación.
// ¿Por qué es un problema?
//Falta de expresividad: No sabes lo que representa ese string.
//Duplicación de lógica: Validaciones y reglas se repiten por todos lados.
//Mayor posibilidad de errores: Es fácil pasar un string donde no deberías.

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        return product;
    }
}

//Strongly Typed ID Pattern
//Es un patrón donde cada tipo de entidad en tu dominio tiene su propio tipo específico de identificador,
//en lugar de usar Guid, int, string, etc. directamente
//Ofrece seguridad topografica y legibilidad al ser especifico en lo que hay que pasar generara error de compilcacion si envia lo que no es,
//lo que no pasaria con un string.