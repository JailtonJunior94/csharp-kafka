using Microsoft.ApplicationInsights.Channel;
using CSharp.Kafka.Business.Infra.Configurations;
using Microsoft.ApplicationInsights.Extensibility;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class ApplicationInsightsInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.InstrumentationKey = EnvironmentKeyVault.InstrumentationKey;
            telemetry.Context.Cloud.RoleName = EnvironmentKeyVault.CloudRoleName;
        }
    }
}
