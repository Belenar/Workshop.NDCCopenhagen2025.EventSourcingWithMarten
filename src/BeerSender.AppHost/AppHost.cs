using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("marten-db")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(config => 
        config.WithLifetime(ContainerLifetime.Persistent));

builder.AddProject<Projects.BeerSender_Web>("command-api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.BeerSender_QueryAPI>("query-api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
