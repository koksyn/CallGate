using CallGate.DependencyInjection;

namespace CallGate.Services.Authentication
{
    public interface IHashService : ITransientDependency
    {
        string GenerateHash(string data);
    }
}