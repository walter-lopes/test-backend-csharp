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
    public class IssuerParseRepository : Repository, IIssuerParseRepository
    {
        public IssuerParseRepository(IOptions<ConnectionStringOption> connectionString, ILogger<IssuerParseRepository> logger) : base(connectionString, logger)
        {

        }

        public async Task<IReadOnlyCollection<IssuerParse>> GetAll()
        {
            using (var conn = CreateOracleConnection())
            {
                var response = await conn.QueryAsync<IssuerParse>(IssuerParseStatement.GetAll);
                return response.ToList();
            }
        }

        public async Task<IssuerParse> GetBy(string issuerNameCetip)
        {
            var getIssuerParse = new
            {
                IssuerNameCetip = issuerNameCetip != string.Empty ? issuerNameCetip : "-"
            };
            using (var conn = CreateOracleConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<IssuerParse>(IssuerParseStatement.GetBy, getIssuerParse);
            }
        }

        public async Task Create(IssuerParse parse)
        {
            var createIssuer = new
            {
                IssuerNameCetip = parse.IssuerNameCetip != string.Empty ? parse.IssuerNameCetip : "-",
                IssuerNameCustodyManager = parse.IssuerNameCustodyManager != string.Empty ? parse.IssuerNameCustodyManager : "-",
            };

            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(IssuerParseStatement.Create, createIssuer);
            }
        }
        public async Task Update(IssuerParse parse)
        {
            var updateIssuer = new
            {
                IssuerNameCetip = parse.IssuerNameCetip != string.Empty ? parse.IssuerNameCetip : "-",
                IssuerNameCustodyManager = parse.IssuerNameCustodyManager != string.Empty ? parse.IssuerNameCustodyManager : "-",
            };

            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(IssuerParseStatement.Update, updateIssuer);
            }
        }
        public async Task Delete(IssuerParse parse)
        {
            using (var conn = CreateOracleConnection())
            {
                await conn.ExecuteAsync(IssuerParseStatement.Delete, new { parse.IssuerNameCetip });
            }
        }
    }
}
