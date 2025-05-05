namespace Ordering.Application.Orders.Queries.GetOrdersByName;
public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {

        var orders = await dbContext.Orders
                //Incluye los order items amarrados a la orden
                .Include(o => o.OrderItems)
                //Cuando solo se leen entidades y no se van a modificar en el mismo contexto, usar asnotracking
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);

        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }
}