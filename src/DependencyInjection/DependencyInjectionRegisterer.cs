using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace CallGate.DependencyInjection
{
    public class DependencyInjectionRegisterer
    {
        public static void RegisterAssemblies(IServiceCollection services)
        {
            var transients = GetTypesByImplementedInterface(typeof(ITransientDependency));
            var singletons = GetTypesByImplementedInterface(typeof(ISingletonDependency));
            var scopeds = GetTypesByImplementedInterface(typeof(IScopedDependency));

            RegisterTypesByImplementedInterface(transients, ServiceLifetime.Transient, services);
            RegisterTypesByImplementedInterface(singletons, ServiceLifetime.Singleton, services);
            RegisterTypesByImplementedInterface(scopeds, ServiceLifetime.Scoped, services);
            
            RegisterFilters(services);
        }

        private static IEnumerable<(Type, Type)> GetTypesByImplementedInterface(Type implementedInterface)
        {
            var interfaces = GetImplementations(implementedInterface).Where(type => type.IsInterface);

            foreach (var @interface in interfaces)
            {
                var classes = GetImplementations(@interface).Where(type => type.IsClass && !type.IsAbstract);

                foreach (var @class in classes)
                {
                    yield return (@interface, @class);
                }
            }
        }

        private static IEnumerable<Type> GetImplementations(Type implementedInterface)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => implementedInterface.IsAssignableFrom(type) && implementedInterface != type);
        }

        private static void RegisterTypesByImplementedInterface(IEnumerable<(Type, Type)> implementations, ServiceLifetime lifetime, IServiceCollection services)
        {
            foreach (var implementation in implementations)
            {
                services.Add(new ServiceDescriptor(implementation.Item1, implementation.Item2, lifetime));
            }
        }

        private static void RegisterFilters(IServiceCollection services)
        {
            var filters = GetImplementations(typeof(IActionFilterDependency));

            foreach (var filter in filters)
            {
                services.AddScoped(filter);
            }
        }
    }
}
