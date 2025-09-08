using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Commands;
using BeerSender.Domain.JsonConfiguration;
using BeerSender.Domain.Projections;
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

        services.AddTransient<ICommandHandler<CreateBox>, CreateBoxHandler>();
        services.AddTransient<ICommandHandler<AddShippingLabel>, AddLabelHandler>();
        services.AddTransient<ICommandHandler<AddBeerBottle>, AddBeerBottleHandler>();
        services.AddTransient<ICommandHandler<CloseBox>, CloseBoxHandler>();
        services.AddTransient<ICommandHandler<SendBox>, SendBoxHandler>();
    }
    
    public static void ApplyDomainConfig(this StoreOptions options)
    {
        options.Schema.For<OpenBox>()
            .Identity(ob => ob.BoxId)
            .UseNumericRevisions(true);
    }
    
    public static void AddProjections(this StoreOptions options)
    {
        options.Projections.Add<OpenBoxProjection>(ProjectionLifecycle.Async);
    }
}