namespace Catalog.API.Products.GetProducts
{
    //Es buena practica siempre definir Query o Command y Result en el Handler
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    //IDocumentSession viene de Marten. 
    internal class GetProductsQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            //var products = await session.Query<Product>().ToListAsync(cancellationToken);

            var products = await session.Query<Product>()
                    .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
