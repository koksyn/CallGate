using System;
using CallGate.DependencyInjection;

namespace CallGate.Seeders
{
    public interface ISeeder : IComparable<ISeeder>, ITransientDependency
    {
        void Seed();
        int GetPriority();
    }
}