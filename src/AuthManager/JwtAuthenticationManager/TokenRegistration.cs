using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationManager;

public static class TokenRegistration
{
    public static IServiceCollection RegisterTokenBearer(this IServiceCollection services, string issuer, string audience, string key)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
                o.Events = new JwtBearerEvents()
                {
                    OnChallenge = async (context) =>
                    {
                        if (context.AuthenticateFailure != null)
                        {
                            var error = string.IsNullOrEmpty(context.ErrorDescription)
                                ? context.AuthenticateFailure?.Message
                                : context.ErrorDescription;
                            context.HandleResponse();
                            context.Response.ContentType = "application/json";
                            var statusCode = context?.AuthenticateFailure is SecurityTokenExpiredException
                                ? 403
                                : 401;
                            if (context != null)
                            {
                                context.Response.StatusCode = statusCode;
                                await context.Response.WriteAsJsonAsync(new
                                {
                                    ErrorMessage = statusCode == 403
                                        ? $"403 Expired token: {error}"
                                        : $"401 Not authorized: {error}"
                                });
                            }
                        }
                    }
                };
            });

        return services;
    }
}