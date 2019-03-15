using Easynvest.Infohub.Parse.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Domain.Interfaces
{
    public interface IBondParseRepository
    {
        Task<IReadOnlyCollection<BondParse>> GetAll();

        Task<BondParse> GetBy(string bondType, string bondIndex, string isAntecipatedSell);

        Task Create(BondParse createBondParse);

        Task Update(BondParse updateBondParse);

        Task Delete(string bondIndex, string bondType, string IsAntecipatedSell);
    }
}
