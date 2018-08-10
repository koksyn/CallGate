using System;
using CallGate.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CallGate.Services.Helper
{
    public interface IJwtHelper : ITransientDependency
    {
        void AddJwtAuthentication(IServiceCollection services);
        string GenerateToken(Guid userId, string username, DateTime notBeforeDate, DateTime expirationDate);
    }
}