namespace Basket.API.Basket.GetBasket
{

    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);

    //No olvidar añadir QUERY o COMMAND al nombre del handler segun su funcion. 
    internal class GetBasketQueryHandler(IBasketRepository repository)
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(query.UserName, cancellationToken);

            return new GetBasketResult(basket);
        }
    }
}
