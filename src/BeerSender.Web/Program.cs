using BeerSender.Domain;
using BeerSender.Domain.Boxes;
using BeerSender.Web.EventPublishing;
using JasperFx.Events.Daemon;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDomain();

builder.AddNpgsqlDataSource("marten-db");
builder.Services.AddMarten(opt =>
    {
        opt.DatabaseSchemaName = "beersender";
        opt.ApplyDomainConfig();
        opt.AddProjections();
    })
    .AddAsyncDaemon(DaemonMode.Solo)
    .UseNpgsqlDataSource();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.MapRazorPages();
    
app.MapHub<EventHub>("event-hub");

app.Run();