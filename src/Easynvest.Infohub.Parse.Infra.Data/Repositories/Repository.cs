using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Easynvest.Infohub.Parse.Infra.Data.Repositories
{
    public abstract class Repository
    {
        protected Repository(IOptions<ConnectionStringOption> connectionString, ILogger logger)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            ConnectionStringOptions = connectionString ?? throw new System.ArgumentNullException(nameof(connectionString));
        }

        protected IDbConnection CreateOracleConnection(string connectionName = "OracleConnection")
        {
            return new OracleConnection(ConnectionStringOptions?.Value?[connectionName] ?? string.Empty);
        }

        protected ILogger Logger { get; }
        protected IOptions<ConnectionStringOption> ConnectionStringOptions { get; }
    }
}
