using Easynvest.Infohub.Parse.Domain.Entities;
using FluentValidation;

namespace Easynvest.Infohub.Parse.Domain.Validators
{
    internal class IssuerParseValidator : AbstractValidator<IssuerParse>
    {
        internal IssuerParseValidator()
        {
            RuleFor(i => i.IssuerNameCustodyManager).NotNull().WithMessage("O campo Mnemônico não pode ser nulo.");
            RuleFor(i => i.IssuerNameCetip).NotNull().WithMessage("O campo Emissor não pode ser nulo.");
        }
    }
}
