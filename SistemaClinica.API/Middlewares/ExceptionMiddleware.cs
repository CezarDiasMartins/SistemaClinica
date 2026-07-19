using System.Net;
using System.Text.Json;
using SistemaClinica.Application.Response;

namespace SistemaClinica.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Erro não tratado na API.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new GenericNoDataResponse { Errors = ["Ocorreu um erro inesperado. Tente novamente mais tarde."] };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
