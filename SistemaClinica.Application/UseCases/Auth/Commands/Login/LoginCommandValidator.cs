using FluentValidation;

namespace SistemaClinica.Application.UseCases.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o e-mail.")
            .EmailAddress().WithMessage("Informe um e-mail válido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Informe a senha.");
    }
}
