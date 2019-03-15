using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dapper;
using Easynvest.Infohub.Parse.Infra.Data.Repositories;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;

namespace Easynvest.Infohub.Parse.Infra.Data.HealthChecks
{
    public class DatabaseHealthCheckRepository : Repository, IHealthCheckRepository
    {
        public DatabaseHealthCheckRepository(IOptions<ConnectionStringOption> connectionString, ILogger<DatabaseHealthCheckRepository> logger)
            : base(connectionString, logger)
        {
        }

        public void Ping()
        {
            using (var conn = CreateOracleConnection())
            {
                conn.QuerySingleAsync("SELECT 1 FROM API_TICKETMANAGER.INDEX_PARSE WHERE ROWNUM = 1");
            }
        }
    }
}
