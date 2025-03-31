﻿namespace Catalog.API.Products.UpdateProduct
{
    //Para operaciones de escritura se debe usar un Command Object
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    //Separacion de commands y querys con interfaz ICommand
    : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id de producto es requerido");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido")
                                .Length(2, 150).WithMessage("El nombre debe tener entre 2 y 150 caracteres");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;  

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
