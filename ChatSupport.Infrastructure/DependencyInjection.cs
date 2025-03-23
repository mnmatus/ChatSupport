using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Infrastructure.Persistence;
using ChatSupport.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatSupport.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddScoped<IAppDBContext>(provider => provider.GetRequiredService<AppDBContext>());
            services.AddSingleton<IChatSessionService, ChatSessionService>();
            services.AddSingleton<IAgentQueueService, AgentQueueService>();
            services.AddTransient<ISystemDateTimeService, SystemDateTimeService.SystemDateTimeService>();

            services.AddHostedService<AgentChatCoordinatorService>();
            services.AddHostedService<ChatSessionMonitorService>();
            return services;
        }
    }
}
