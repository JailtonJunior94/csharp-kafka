namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public interface IApplicationInsightsMetrics
    {
        void AddMetric(CustomMetric metrica);
    }
}
