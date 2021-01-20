using System;
using System.ComponentModel;

namespace CSharp.Kafka.Business.Infra.Configurations
{
    public static class EnvironmentKeyVault
    {
        public static T GetValue<T>(string key)
        {
            var valor = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            return (T)ChangeType(typeof(T), valor);
        }

        private static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static string InstrumentationKey
        {
            get
            {
                var value = string.IsNullOrEmpty(GetValue<string>("InstrumentationKey")) ? GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY") : GetValue<string>("InstrumentationKey");
                return value;
            }
        }
        public static string ConnectionString { get { return GetValue<string>("ConnectionString"); } }
        public static string CloudRoleName { get { return GetValue<string>("CloudRoleName"); } }
    }
}
