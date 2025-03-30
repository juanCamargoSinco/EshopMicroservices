namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                //Se recibe la peticion mediante llamado post del api configurado con Carter
                //Esa peticion, el request, se convierte con Mapster a un command, una solicitud para crear el producto
                var command = request.Adapt<CreateProductCommand>();

                //El command se envia con MediatR a la clase CraeteProductHandler, se comunica con ella
                //sin embargo el no lo sabe, no interactua directamente con CreateProductHandler gracias a MediatR
                // lo que hace que no esten acoplados
                var result = await sender.Send(command);

                //CreateProductHandler maneja la logica de creacion y devuelve un resultado que con Mapster
                //convertimos en un CreateProductResponse y lo devolvemos como respuesta
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);

            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");

        }
    }
}
