using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Calculator.API.Configurations;

public static class SwaggerConfiguration
{
    public static WebApplicationBuilder GetSwaggerConfiguration(this WebApplicationBuilder builder)
    {

        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TeamTitan Calculator API",
                Version = "v1",
                Description = "ASP.NET Core 6.0 Web API",
                Contact = new OpenApiContact
                {
                    Name = "Github",
                    Url = new Uri("https://github.com/DubieleckiBartosz"),
                }
            });
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer token",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[] { }}
            });
        });

        return builder;
    }
}