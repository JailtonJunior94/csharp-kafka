using Microsoft.ApplicationInsights;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class TelemetryClientWrapper
    {
        private readonly TelemetryClient telemetryClient;

        public TelemetryClientWrapper(TelemetryClient telemetryClient) =>
            this.telemetryClient = telemetryClient;

        public virtual TelemetryContextWrapper Context => new
            TelemetryContextWrapper(telemetryClient.Context);

        public virtual MetricWrapper GetMetric(string metricId) =>
            new MetricWrapper(telemetryClient.GetMetric(metricId));
    }
}
