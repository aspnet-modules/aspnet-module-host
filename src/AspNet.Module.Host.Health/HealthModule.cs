using AspNet.Module.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNet.Module.Host.Health;

/// <summary>
///     Health Модуль
/// </summary>
public class HealthModule : IAspNetModule
{
    public void ConfigureApp(WebApplication app)
    {
        app.UseHealthChecksPrometheusExporter("/metrics");
        app.UseHealthChecks(HealthCheckPath.SelfPath, new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
        app.UseHealthChecks(HealthCheckPath.ReadyPath, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("services")
        });
    }

    public void Configure(AspNetModuleContext ctx) =>
        ctx.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
}