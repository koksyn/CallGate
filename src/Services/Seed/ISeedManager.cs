using CallGate.DependencyInjection;

namespace CallGate.Services.Seed
{
    public interface ISeedManager : ITransientDependency
    {
        void Seed();
    }
}