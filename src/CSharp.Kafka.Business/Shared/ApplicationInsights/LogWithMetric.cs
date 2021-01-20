using System;
using Serilog;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using CSharp.Kafka.Business.Domain.Enums;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class LogWithMetric : ILogWithMetric
    {
        private Stopwatch _stopwatch;
        private string EventName { get; set; }
        private string EventFullName { get; set; }
        private List<CustomMetric> Metrics { get; set; }
        private readonly ITelemetryProxy _telemetryProxy;
        private readonly IApplicationInsightsMetrics _appInsightsMetrics;

        public LogWithMetric(IApplicationInsightsMetrics appInsightsMetrics, ITelemetryProxy telemetryProxy)
        {
            _appInsightsMetrics = appInsightsMetrics;
            _telemetryProxy = telemetryProxy;
        }

        public LogWithMetric(string eventName, IApplicationInsightsMetrics appInsightsMetrics)
        {
            _appInsightsMetrics = appInsightsMetrics;
            SetTitle(eventName);
            ConfigMetric(eventName);
        }

        public void LogWithEvent(string text, Metric metricEnum, Exception exception = null)
        {
            if (_telemetryProxy == null)
                return;

            switch (metricEnum)
            {
                case Metric.Init:
                case Metric.Success:
                    Log.Information($"{metricEnum} - {EventFullName} - {text}");
                    _telemetryProxy.TrackEvent($"{metricEnum} - {EventFullName}");
                    break;
                case Metric.Error:
                    if (exception != null)
                        Log.Error(exception, $"{metricEnum} - {EventFullName} - {text}");
                    else
                        Log.Error($"{metricEnum} - {EventFullName} - {text}");
                    _telemetryProxy.TrackEvent($"{metricEnum} - {EventFullName}");
                    break;
                default:
                    Log.Information(text);
                    _telemetryProxy.TrackEvent(EventFullName);
                    break;
            }

            _appInsightsMetrics.AddMetric(Metrics.FirstOrDefault(x => x.MetricType == metricEnum));
        }

        public void LogWithCustomMetric(string text, Metric metricEnum, string metricName = null, Exception exception = null)
        {
            metricName = !string.IsNullOrEmpty(metricName) ? RemoveAccents(metricName) : EventName;

            switch (metricEnum)
            {
                case Metric.Init:
                case Metric.Success:
                    Log.Information($"{metricEnum} - {EventFullName} - {text}");
                    break;
                case Metric.Error:
                    if (exception != null)
                        Log.Error(exception, $"{metricEnum} - {EventFullName} - {text}");
                    else
                        Log.Error($"{metricEnum} - {EventFullName} - {text}");
                    break;
                default:
                    Log.Information(text);
                    break;
            }

            _appInsightsMetrics.AddMetric(AddCustomMetric(metricName, metricEnum));
        }

        private CustomMetric AddCustomMetric(string metricName, Metric metricEnum)
        {
            CustomMetric customMetric = metricEnum switch
            {
                Metric.Init => new CustomMetric(Metric.Init, $"QtdInicio{metricName}"),
                Metric.Success => new CustomMetric(Metric.Success, $"QtdSucesso{metricName}"),
                Metric.Error => new CustomMetric(Metric.Error, $"QtdErro{metricName}"),
                _ => new CustomMetric(Metric.General, $"{metricName}"),
            };
            return customMetric;
        }

        public void SetTitle(string title)
        {
            EventFullName = title;
            EventName = RemoveAccents(title);
            ConfigMetric(title);
        }

        public void Start()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            LogWithEvent($"DataHora: {DateTime.Now}", Metric.Init);
        }

        public void Finish()
        {
            _stopwatch.Stop();
            LogWithEvent($"Tempo Processamento: {_stopwatch.Elapsed}", Metric.Success);
        }

        public void Error(Exception ex)
            => LogWithEvent($"Erro: {ex.Message}", Metric.Error, exception: ex);

        public void LogStart()
            => LogWithCustomMetric(EventFullName, Metric.Init);

        public void LogFinish(string additionalInfo = "")
            => LogWithCustomMetric($"{EventFullName} {additionalInfo}", Metric.Success);

        public void LogError(Exception e)
            => LogWithCustomMetric($"{EventFullName} - Erro: {e.Message}", Metric.Error, exception: e);

        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        private void ConfigMetric(string eventName)
        {
            Metrics = CustomMetric.MetricFactory($"QtdInicio{EventFullName}", $"QtdSucesso{EventName}", $"QtdErro{EventName}, {eventName}");
        }

        private string RemoveAccents(string text)
        {
            text = text.Replace(" ", "").Trim();
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
