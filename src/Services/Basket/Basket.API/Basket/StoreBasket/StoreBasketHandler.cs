
using Discount.Grpc;

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

    internal class StoreBasketCommandHandler(IBasketRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;

            //Validacion de descuento mediante consumo de microservicio de descuentos con gRPC 
            await DeterminarDescuento(cart, cancellationToken);
            //Almacenar en la BD y actualizar el cache
            //Marten actualiza en BD, Redis maneja en cache
            await repository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }

        private async Task DeterminarDescuento(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var cupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= cupon.Amount;
            }
        }
    }
}
