using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Services
{
    public class DirectoryAccessHealth : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            if (Directory.Exists("c:\\MyDir"))
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Main dir missing"));

        }
    }
}
