using ChatSupport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatSupport.Application.Common.Interfaces
{
    public interface IAppDBContext
    {
        DbSet<Agent> Agent { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
