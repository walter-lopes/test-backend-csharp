using Easynvest.Infohub.Parse.Domain.Entities;
using FluentValidation;

namespace Easynvest.Infohub.Parse.Domain.Validators
{
    internal class BondParseValidator : AbstractValidator<BondParse>
    {
        internal BondParseValidator()
        {
            RuleFor(i => i.BondType).NotNull().WithMessage("O campo Tipo IF não pode ser nulo.");
            RuleFor(i => i.BondType).NotEmpty().WithMessage("O campo Tipo IF não pode ser vazio.");
            RuleFor(i => i.BondIndex).NotNull().WithMessage("O campo Indexador não pode ser nulo.");
            RuleFor(i => i.IsAntecipatedSell).NotNull().WithMessage("O campo Condição Resgate não pode ser nulo.");
            RuleFor(i => i.IdCustodyManagerBond).NotNull().WithMessage("O campo Código Virtual não pode ser nulo.");
        }
    }
}