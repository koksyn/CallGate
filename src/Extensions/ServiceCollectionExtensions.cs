using Microsoft.Extensions.DependencyInjection;

namespace CallGate.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TService Resolve<TService>(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            return (TService) serviceProvider.GetService(typeof(TService));
        }
    }
}
