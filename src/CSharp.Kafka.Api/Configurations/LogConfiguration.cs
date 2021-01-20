using Serilog;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using CSharp.Kafka.Business.Infra.Configurations;
using Microsoft.ApplicationInsights.Extensibility;
using CSharp.Kafka.Business.Shared.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace CSharp.Kafka.Api.Configurations
{
    public static class LogConfiguration
    {
        public static void ImplementLogConfigurationService(this IServiceCollection services)
        {
            var telemetryConfig = TelemetryConfiguration.CreateDefault();
            telemetryConfig.InstrumentationKey = EnvironmentKeyVault.InstrumentationKey;
            telemetryConfig.TelemetryInitializers.Add(new ApplicationInsightsInitializer());

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(telemetryConfig, TelemetryConverter.Traces, LogEventLevel.Information)
                .CreateLogger();

            var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions
            {
                EnableAdaptiveSampling = false,
                InstrumentationKey = EnvironmentKeyVault.InstrumentationKey
            };
            services.AddApplicationInsightsTelemetry(applicationInsightsServiceOptions);
        }
    }
}
