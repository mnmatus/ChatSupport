using ChatSupport.Api.MiddleWares;
using ChatSupport.Application;
using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Infrastructure;
using ChatSupport.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddLogging((builder => builder.AddConsole()));
builder.Services.AddLogging();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//// Seed agents on application start
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
//    context.SeedAgents();
//}


using (var scope = app.Services.CreateScope())
{
    // Seed agents on application start
    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    context.SeedAgents();

    // Upsert agents to capacity
    var agents = context.Agent.ToList();
    var agentQueueService = scope.ServiceProvider.GetRequiredService<IAgentQueueService>();
    foreach (var agent in agents)
    {
        await agentQueueService.UpsertAgentToCapacity(agent);
    }
}

app.Run();
