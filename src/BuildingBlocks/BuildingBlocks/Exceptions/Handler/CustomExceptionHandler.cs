using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public  class CustomExceptionHandler
        (ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {

        //Se bloquearan las excepcion y se devolvera la respuesta en base a la excepcion
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error message: {exceptionMessage}, Hora ocurrencia {time}",
                exception.Message, DateTime.UtcNow
                );

            //Pattern matching: permite comprobar si un valor cumple con un cierto patrón y, en caso afirmativo, realizar acciones específicas.
            //Se utiliza comúnmente con la palabra clave is y con el operador switch.
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                //Si la excepcion no es ninguno de estos tipos usara el siguiente codigo
                _ =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };

            //Se añaden propiedades custom al objeto ProblemDetails para dar informacion adicional en la excepcion
            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            //Se formatea el error como un Json
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
            return true;
        }
    }
}
