using CSharp.Kafka.Business.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using CSharp.Kafka.Business.Application.Services;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Api.Configurations
{
    public static class DependenciesConfiguration
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            /* Services */
            services.AddScoped<ICustomerService, CustomerService>();

            /* Repositories */
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
