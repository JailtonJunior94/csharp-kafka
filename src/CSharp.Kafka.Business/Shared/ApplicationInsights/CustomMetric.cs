using CSharp.Kafka.Business.Domain.Enums;
using System.Collections.Generic;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class CustomMetric
    {
        public Metric MetricType { get; set; }
        public string MetricName { get; set; }
        public double MetricValue { get; set; }

        public CustomMetric(Metric metricType, string metricName, double metricValue = 1)
        {
            MetricType = metricType;
            MetricName = metricName;
            MetricValue = metricValue;
        }

        public static List<CustomMetric> MetricFactory(string metricNameInit, string metricNameSuccess, string metricNameError)
        {
            var MetricValueDefault = 1;
            if (string.IsNullOrEmpty(metricNameInit) || string.IsNullOrEmpty(metricNameSuccess) || string.IsNullOrEmpty(metricNameError))
                return new List<CustomMetric>();

            return new List<CustomMetric>()
            {
                new CustomMetric(Metric.Init, metricNameInit, MetricValueDefault),
                new CustomMetric(Metric.Success, metricNameSuccess, MetricValueDefault),
                new CustomMetric(Metric.Error, metricNameError, MetricValueDefault)
            };
        }
    }
}
