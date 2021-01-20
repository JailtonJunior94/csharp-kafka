using Microsoft.ApplicationInsights.DataContracts;

namespace CSharp.Kafka.Business.Shared.ApplicationInsights
{
    public class TelemetryContextWrapper
    {
        private readonly TelemetryContext telemetryContext;

        protected TelemetryContextWrapper() { }

        public TelemetryContextWrapper(TelemetryContext telemetryContext) => this.telemetryContext = telemetryContext;

        public virtual string InstrumentationKey
        {
            get => telemetryContext.InstrumentationKey;
            set => telemetryContext.InstrumentationKey = value;
        }
    }
}
