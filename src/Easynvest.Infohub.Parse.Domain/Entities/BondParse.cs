using Easynvest.Infohub.Parse.Domain.Validators;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using System.Linq;

namespace Easynvest.Infohub.Parse.Domain.Entities
{
    public class BondParse
    {
        protected BondParse()
        {
        }

        private BondParse(string bondType, string bondIndex, string isAntecipatedSell, int idCustodyManagerBond)
        {
            BondType = bondType?.Trim().ToUpper();
            BondIndex = bondIndex?.Trim().ToUpper();
            IsAntecipatedSell = isAntecipatedSell?.Trim().ToUpper();
            IdCustodyManagerBond = idCustodyManagerBond;
        }

        public string BondType { get; }
        public string BondIndex { get; }
        public string IsAntecipatedSell { get; }
        public int IdCustodyManagerBond { get; }

        public static Response<BondParse> Create(string bondType, string bondIndex, string isAntecipatedSell, int idCustodyManagerBond)
        {
            var bondIndexParse = new BondParse(bondType, bondIndex, isAntecipatedSell, idCustodyManagerBond);
            var validator = new BondParseValidator().Validate(bondIndexParse);

            if (validator.IsValid)
                return Response<BondParse>.Ok(bondIndexParse);

            return Response<BondParse>.Fail(validator.Errors.Select(x => x.ErrorMessage));
        }
    }
}
