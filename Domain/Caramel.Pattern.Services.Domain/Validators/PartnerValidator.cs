using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class PartnerValidator : AbstractValidator<Partner>
    {
        public PartnerValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O Nome é obrigatório.");
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Regra de usuário inválida.");
            RuleFor(x => x.MaxCapacity)
                .NotEmpty().GreaterThan(0).WithMessage("A capacidade máxima deve ser maior que zero.");
        }
    }
}
