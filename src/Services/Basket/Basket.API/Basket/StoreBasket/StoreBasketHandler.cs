
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    //Crear clase de validacion (command validator)
    // que se ejecuta por el ValidationBehavior en buildingblock
    // y que solo sirve para commandos (clase que implementa ICommand) 
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart no puede ser nulo");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("El UserName es requerido");
        }
    }

    internal class StoreBasketCommandHandler(IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            //La logica sera almacenar en la BD y actualizar el cache
            ShoppingCart cart = command.Cart;

            await repository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }
    }
}
