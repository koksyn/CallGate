using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CallGate.Services.Helper
{
    public class JwtHelper :  IJwtHelper
    {
        private readonly IOptions<ConfigurationManager> _configurationManager;

        public JwtHelper(IOptions<ConfigurationManager> configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager.Value.JwtSiginingKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                
                // for WebSockets we cannot send headers with JWT token
                // we need to fetch it from Query string
                // https://github.com/aspnet/Home/issues/2881
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (
                            context.Request.Query.ContainsKey("jwtToken") &&
                            !context.Request.Headers.ContainsKey("Authorization")
                        ){
                            var jwtToken = context.Request.Query["jwtToken"];
                            
                            if (!string.IsNullOrEmpty(jwtToken))
                            {
                                context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
                            }
                        }
                        
                        return Task.CompletedTask;
                    }
                };
            });
        }
        
        public string GenerateToken(Guid userId, string username, DateTime notBeforeDate, DateTime expirationDate)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(notBeforeDate).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expirationDate).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager.Value.JwtSiginingKey)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
