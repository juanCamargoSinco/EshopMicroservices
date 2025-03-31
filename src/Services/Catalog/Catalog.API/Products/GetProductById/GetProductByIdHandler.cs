namespace Catalog.API.Products.GetProductById
{
    //Para operaciones de consulta se debe usar un Query object
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);

    //Clase encargada de manejar el objeto query, interactura con la BD y devolver el result object
    //Acorde a IQueryHandler Esta clase espera un GetProductByIdQuery como entrada y devolvera GetProductByIdResult
    internal class GetProductByIdQueryHandler (IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException(query.Id);
            }

            return new GetProductByIdResult(product);
        }
    }
}
