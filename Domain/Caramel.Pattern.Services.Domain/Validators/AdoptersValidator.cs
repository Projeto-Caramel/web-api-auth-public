using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class AdoptersValidator : AbstractValidator<Adopter>
    {
        public AdoptersValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O Nome é obrigatório.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}
