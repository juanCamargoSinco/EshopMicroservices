namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);


    internal class CreateProductComandHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //Logica de negocio para crear un producto
            // Crear un Product a partir del command object
            // Guardar en la DB
            // return CreateProductResult result

            // Crear un Product a partir del command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // Guardar en la DB
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult result
            return new CreateProductResult(product.Id);
            
        }
    }
}
