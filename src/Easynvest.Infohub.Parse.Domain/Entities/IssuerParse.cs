using Easynvest.Infohub.Parse.Domain.Validators;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using System.Linq;

namespace Easynvest.Infohub.Parse.Domain.Entities
{
    public class IssuerParse
    {
        protected IssuerParse()
        {
        }

        private IssuerParse(string issuerNameCustodyManager, string issuerNameCetip)
        {
            IssuerNameCustodyManager = issuerNameCustodyManager?.Trim().ToUpper();
            IssuerNameCetip = issuerNameCetip?.Trim().ToUpper();
        }

        public string IssuerNameCustodyManager { get; }

        public string IssuerNameCetip { get; }

        public static Response<IssuerParse> Create(string issuerNameCustodyManager, string issuerNameCetip)
        {
            var issuerParse = new IssuerParse(issuerNameCustodyManager, issuerNameCetip);
            var validator = new IssuerParseValidator().Validate(issuerParse);

            if (validator.IsValid)
                return Response<IssuerParse>.Ok(issuerParse);

            return Response<IssuerParse>.Fail(validator.Errors.Select(x => x.ErrorMessage));
        }
    }
}
