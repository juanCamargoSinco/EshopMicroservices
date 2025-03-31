namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    //A pesar de que se llama CreateProductComandHandler,
    //se ejecuta primero esta clase como un Pipeline Behavior antes de que se ejecute
    //el metodo Handle de CreateProductComandHandler
    //al acabar si continua con todo el codigo del metodo Handle
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {

        public CreateProductCommandValidator()
        {
            RuleFor(x  => x.Name).NotEmpty().WithMessage("El nombre es requerido");
            RuleFor(x => x.Category).NotEmpty().WithMessage("La categoria es requerida");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("La imagen es requerida");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
        }

    }


    internal class CreateProductComandHandler
        (IDocumentSession session)
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
