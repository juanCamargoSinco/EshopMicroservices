﻿namespace Catalog.API.Products.GetProductById
{
    //Es buena practica siempre definir Request y Response en el endpoint aunque no se use
    //public record GetProductByIdRequest();
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                //MediatR activara el correspondiente Handler
                var result = await sender.Send(new GetProductByIdQuery(id));

                var response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);

            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
        }
    }
}
