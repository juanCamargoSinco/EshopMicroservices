namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category)
        //Separacion de commands y querys con interfaz IQuery
        : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    internal class GetProductByCategoryQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().Where(x => x.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);
                        
            return new GetProductByCategoryResult(products);
        }
    }
}
