using CallGate.DependencyInjection;

namespace CallGate.Data
{
    public interface IRethinkDbManager : IScopedDependency
    {
        void EnsureDatabaseCreated();

        void Reconfigure(int shards, int replicas);
    }
}