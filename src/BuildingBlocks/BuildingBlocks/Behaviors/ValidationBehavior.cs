using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        //Se usa un IEnumerable para recolectar todos los handle metods para poder validar todo en un solo lugar
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        //request es la solicitud (request) de entrada del cliente
        //next es hacia adonde va a ir la peticion despues de pasar por aca (pipeline behavior o handle)
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //se crea un contexto a partir de la peticion recibida, la cual deberia ser un comando ICommand
            var context = new ValidationContext<TRequest>(request);

            //Ejecutamos todos los validadores ejecutados durante el request pasando como parametro el contexto,
            //en si el comando recibido
            var validationsResult = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var failures = validationsResult.Where(x => x.Errors.Any())
                .SelectMany(x => x.Errors).ToList();

            if (failures.Any()) {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
