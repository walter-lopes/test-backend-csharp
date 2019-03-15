using Easynvest.Infohub.Parse.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Domain.Interfaces
{
    public interface IIssuerParseRepository
    {
        Task<IReadOnlyCollection<IssuerParse>> GetAll();
        Task<IssuerParse> GetBy(string issuerNameCetip);
        Task Create(IssuerParse parse);
        Task Update(IssuerParse parse);
        Task Delete(IssuerParse parse);
    }
}
