using General.Application;
using General.Application.Contracts;
using General.Infrastructure.Repositories;
using Shared.Implementations;

namespace General.API.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder GetDependencyInjection(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>(); 

        return builder;
    }

    public static WebApplicationBuilder GetShared(this WebApplicationBuilder builder)
    {
        var services = builder.Services; 
        services.RegisterFileService();
        services.GetAccessoriesDependencyInjection();
        services.RegisterValidatorPipeline();
        services.RegisterAutoMapper(typeof(AssemblyGeneralApplicationReference).Assembly);
        services.RegisterMediator(typeof(AssemblyGeneralApplicationReference)); 

        return builder;
    }
}