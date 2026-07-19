using FluentValidation;
using MediatR;

namespace SistemaClinica.Application.Middlewares;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var resultados = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var erros = resultados
            .SelectMany(resultado => resultado.Errors)
            .Where(erro => erro is not null)
            .Select(erro => erro.ErrorMessage)
            .Distinct()
            .ToList();

        if (erros.Count == 0)
            return await next(cancellationToken);

        var response = Activator.CreateInstance<TResponse>();
        var errorsProperty = typeof(TResponse).GetProperty("Errors");

        if (errorsProperty?.GetValue(response) is List<string> responseErrors)
        {
            responseErrors.AddRange(erros);
            return response;
        }

        throw new ValidationException(erros.Select(erro => new FluentValidation.Results.ValidationFailure(string.Empty, erro)));
    }
}
