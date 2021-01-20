namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public interface ITelemetryProxy
    {
        void TrackEvent(string eventName);
    }
}
