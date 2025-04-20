namespace Ordering.Domain.Models;
//Representa articulos individuales dentro del pedido. Contiene informacion del producto
//Modelo anemico : No tiene ( o en algunos casos tiene muy poca ) logica de negocio.
// Actua mas como una estructura de data con gets y sets
//La logica de negocio relacionada al modelo se implementa fuera de la entidad
//En este caso no tiene logica por que la logica le pertenece a su agreggate root, es decir el order
public class OrderItem : Entity<OrderItemId>
{
    internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
    //StrongTypedIds evitan mezclar ids al no usar datos primitivos
    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
}