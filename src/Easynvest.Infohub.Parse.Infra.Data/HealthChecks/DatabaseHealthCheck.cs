using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace Easynvest.Infohub.Parse.Infra.Data.HealthChecks
{
    public class DatabaseHealthCheck : HealthCheck
    {
        private readonly IHealthCheckRepository _repository;

        public DatabaseHealthCheck(IHealthCheckRepository repository) : base("oracle_connection_check")
        {
            _repository = repository;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                _repository.Ping();
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
            }
            catch (Exception ex)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }
}
