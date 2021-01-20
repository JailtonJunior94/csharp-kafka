using System;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public interface ILogWithMetric
    {
        void SetTitle(string title);
        void Start();
        void Finish();
        void Error(Exception exception);
        void LogStart();
        void LogFinish(string additionalInfo = "");
        void LogError(Exception exception);
        void LogInfo(string message);
    }
}
