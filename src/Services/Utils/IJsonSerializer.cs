using CallGate.DependencyInjection;

namespace CallGate.Services.Utils
{
    public interface IJsonSerializer : ITransientDependency
    {
        string Serialize(object obj);
    }
}
