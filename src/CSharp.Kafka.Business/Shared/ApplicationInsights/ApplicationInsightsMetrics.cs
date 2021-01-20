using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class ApplicationInsightsMetrics : IApplicationInsightsMetrics
    {
        private readonly TelemetryClient telemetry;

        public ApplicationInsightsMetrics(TelemetryClient telemetry, string key)
        {
            this.telemetry = telemetry;
            this.telemetry.Context.InstrumentationKey = key;
        }

        public void AddMetric(CustomMetric metrica)
        {
            if (metrica != null)
            {
                telemetry.TrackMetric(new MetricTelemetry
                {
                    Name = metrica.MetricName,
                    Sum = metrica.MetricValue
                });
            }
        }
    }
}
