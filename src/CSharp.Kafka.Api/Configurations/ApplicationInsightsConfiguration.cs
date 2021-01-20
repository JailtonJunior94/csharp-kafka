using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CSharp.Kafka.Business.Infra.Configurations;
using Microsoft.ApplicationInsights.Extensibility;
using CSharp.Kafka.Business.Shared.ApplicationInsights;

namespace CSharp.Kafka.Api.Configurations
{
    public static class ApplicationInsightsConfiguration
    {
        public static void RegisterApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITelemetryProxy, TelemetryProxy>();
            services.AddScoped<ILogWithMetric, LogWithMetric>();

            services.AddSingleton<ITelemetryInitializer>(j => new ApplicationInsightsInitializer());
            services.AddApplicationInsightsTelemetry(configuration);
            services.ImplementLogConfigurationService();

            var telemetryClient = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            var metrics = new ApplicationInsightsMetrics(telemetryClient, EnvironmentKeyVault.InstrumentationKey);

            services.AddScoped(j => metrics);
            services.AddScoped<IApplicationInsightsMetrics>(x => metrics);
        }
    }
}
