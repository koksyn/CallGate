using CallGate.DependencyInjection;
using CallGate.Services;
using RethinkDb.Driver.Net;

namespace CallGate.Data
{
    public interface IRethinkDbConnectionFactory : IScopedDependency
    {
        Connection CreateConnection();

        void CloseConnection();

        RethinkDbOptions GetOptions();
    }
}