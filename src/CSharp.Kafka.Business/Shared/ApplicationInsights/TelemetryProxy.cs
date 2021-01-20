using Microsoft.ApplicationInsights;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class TelemetryProxy : ITelemetryProxy
    {
        private readonly TelemetryClient _telemetryClient;

        public TelemetryProxy(TelemetryClient telemetryClient) => _telemetryClient = telemetryClient;

        public void TrackEvent(string eventName) => _telemetryClient.TrackEvent(eventName);
    }
}
