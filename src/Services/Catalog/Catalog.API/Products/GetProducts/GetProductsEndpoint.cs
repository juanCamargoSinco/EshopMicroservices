//No hay usings debido a que estamos usando el archivo GlobalUsing lo que hace mas limpio el codigo

namespace Catalog.API.Products.GetProducts
{

    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //ISender viene del MediatR
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();

                var result = await sender.Send(query);

                //Para que mapster se pueda adaptar sin configuraciones adicionales los parametros deben llamarse iguales
                //Es decir que result, lo que se obtiene de sender.Send es un IEnumerable<Product> Products}
                //Y quiero adaptar a un IEnumerable<Product> Products
                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);

            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product")
            .WithDescription("Get Product");
        }
    }
}
