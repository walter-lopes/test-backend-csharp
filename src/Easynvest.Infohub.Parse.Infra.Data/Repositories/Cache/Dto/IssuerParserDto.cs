using Easynvest.Infohub.Parse.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache.Dto
{
    public class IssuerParserDto
    {
        public string IssuerNameCustodyManager { get; set; }

        public string IssuerNameCetip { get; set; }

        public IssuerParserDto() { }

        public IssuerParserDto(string issuerNameCustodyManager, string issuerNameCetip)
        {
            this.IssuerNameCustodyManager = issuerNameCustodyManager;
            this.IssuerNameCetip = issuerNameCetip;
        }

        public IssuerParse ToDomain() => IssuerParse.Create(this.IssuerNameCustodyManager, this.IssuerNameCetip).Value;

        public static IReadOnlyCollection<IssuerParse> ToDomain(IList<IssuerParserDto> dtos)
        {
            IList<IssuerParse> issuers = new List<IssuerParse>();

            foreach (var dto in dtos) issuers.Add(dto.ToDomain());

            return issuers.ToList();
        }
    }
}
