using Microsoft.ApplicationInsights;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class MetricWrapper
    {
        private readonly Metric metric;

        protected MetricWrapper() { }

        public MetricWrapper(Metric metric) => this.metric = metric;

        public virtual void TrackValue(double metricValue) => metric.TrackValue(metricValue);
    }
}
