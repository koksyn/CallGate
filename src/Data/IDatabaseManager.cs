using CallGate.DependencyInjection;

namespace CallGate.Data
{
    public interface IDatabaseManager : IScopedDependency
    {
        void EnsureDatabaseCreated();
        void Commit();
    }
}