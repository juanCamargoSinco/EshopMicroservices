
namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id de producto es requerido");
        }
    }
    internal class DeleteProductCommandHandler
        //Inyectar session para interactuar con la BD  de postgres mediante Marten
        (IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException(command.Id);
            }

            session.Delete<Product>(product);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);

        }
    }
}
