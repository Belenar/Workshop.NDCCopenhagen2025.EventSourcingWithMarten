using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Commands;
using BeerSender.Domain.JsonConfiguration;
using JasperFx.Events.Projections;
using Marten;
using Marten.Events.Projections;
using Marten.Services.Json.Transformations;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.Domain;

public static class DomainExtensions
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        services.AddScoped<CommandRouter>();
    }
    
    public static void ApplyDomainConfig(this StoreOptions options)
    {
        
    }
    
    public static void AddProjections(this StoreOptions options)
    {
        
    }
}