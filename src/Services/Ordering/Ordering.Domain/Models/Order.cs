namespace Ordering.Domain.Models;
//Aggregate root
//Rich domain model: Encapsula data y comportamiento, aplicando metodos que incoporan reglas de negocio y logica de dominio.
//Ejemplo: logica para añadir o remover items. Al encapsular esta logica aqui se asegura que se cumplan las reglas de negocio
//Al ser el aggregate root es responsable de gestionar su propio estado y el estado de sus elementos asociados
public class Order : Aggregate<OrderId>
{
    //Añadir o quitar items pertenece a la orden, porque son agregados del aggregate root
    //y el aggregate root es la unica manera de interactuar con los agregados
    //-----------------
    //La palabra clave readonly en C# indica que el campo solo puede asignarse una vez, ya sea en su declaración (como en tu ejemplo) o dentro del constructor de la clase.
    //No significa que el contenido del objeto sea inmutable, sino que la referencia a ese objeto no puede cambiar una vez asignada.
    //Usar readonly ayuda a proteger la integridad del campo, asegurando que la referencia no cambie después de la construcción del objeto. Esto es útil cuando quieres que
    //una colección se use internamente sin preocuparte de que otro método accidentalmente la reemplace por otra.
    //Asegura la inmutabilidad de la referencia, pero no de la colección. 

    //Significa que la referencia a la lista solo puede asignarse una vez, ya sea en la misma declaración o dentro
    //del constructor de la clase. Una vez que el constructor ha terminado, no podrás volver a asignar _orderItems a otro objeto;
    //sin embargo,
    //el contenido de la lista (añadir, quitar o modificar elementos) sigue siendo totalmente mutable
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    //StrongerTypedId
    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalPrice //=> OrderItems.Sum(x => x.Price * x.Quantity); Alternativa que la hace propiedad de solo lectura 
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }
    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    //Logica para añadir o remover items.

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
        }
    }

}
