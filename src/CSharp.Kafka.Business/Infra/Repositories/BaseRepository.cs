using System.Data.SqlClient;
using CSharp.Kafka.Business.Infra.Configurations;

namespace CSharp.Kafka.Business.Infra.Repositories
{
    public class BaseRepository
    {
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(EnvironmentKeyVault.ConnectionString);
        }
    }
}
