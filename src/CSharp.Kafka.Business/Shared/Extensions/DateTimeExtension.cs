using System;
using System.Diagnostics.CodeAnalysis;

namespace CSharp.Kafka.Business.Shared.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DateTimeExtension
    {
        public static DateTime UtcBrazil(this DateTime date)
        {
            TimeZoneInfo brazilian = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, brazilian);
        }
    }
}
