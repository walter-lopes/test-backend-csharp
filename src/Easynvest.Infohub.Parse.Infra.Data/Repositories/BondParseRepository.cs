using Dapper;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;
using Easynvest.Infohub.Parse.Infra.Data.Statements;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Infra.Data.Repositories
{
    public class BondParseRepository : Repository, IBondParseRepository
    {
        public BondParseRepository(IOptions<ConnectionStringOption> connectionString, ILogger<BondParseRepository> logger) : base(connectionString, logger)
        {
        }

        public async Task<IReadOnlyCollection<BondParse>> GetAll()
        {
            using (var conn = CreateOracleConnection())
            {
                var response = await conn.QueryAsync<BondParse>(BondParseStatement.GetAll);
                return response.ToList();
            }
        }

        public async Task<BondParse> GetBy(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var getBondParse = new
            {
                BondType = bondType != string.Empty ? bondType : "-",
                BondIndex = bondIndex != string.Empty ? bondIndex : "-",
                IsAntecipatedSell = isAntecipatedSell != string.Empty ? isAntecipatedSell : "-"
            };
            using (var conn = CreateOracleConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<BondParse>(BondParseStatement.GetBy, getBondParse);
            }
        }

        public async Task Create(BondParse createBondParse)
        {
            var createBond = new
            {
                BondType = createBondParse.BondType != string.Empty ? createBondParse.BondType : "-",
                BondIndex = createBondParse.BondIndex != string.Empty ? createBondParse.BondIndex : "-",
                IsAntecipatedSell = createBondParse.IsAntecipatedSell != string.Empty ? createBondParse.IsAntecipatedSell : "-",
                IdCustodyManagerBond = createBondParse.IdCustodyManagerBond
            };

            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(BondParseStatement.Create, createBond);
            }
        }
        public async Task Update(BondParse updateBondParse)
        {
            var updateBond = new
            {
                BondType = updateBondParse.BondType != string.Empty ? updateBondParse.BondType : "-",
                BondIndex = updateBondParse.BondIndex != string.Empty ? updateBondParse.BondIndex : "-",
                IsAntecipatedSell = updateBondParse.IsAntecipatedSell != string.Empty ? updateBondParse.IsAntecipatedSell : "-",
                IdCustodyManagerBond = updateBondParse.IdCustodyManagerBond
            };
            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(BondParseStatement.Update, updateBond);
            }
        }
        public async Task Delete(string bondIndex, string bondType, string IsAntecipatedSell)
        {
            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(BondParseStatement.Delete, new { bondIndex, bondType, IsAntecipatedSell });
            }
        }
    }
}
