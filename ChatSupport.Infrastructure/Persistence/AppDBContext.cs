using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Domain.Entities;
using ChatSupport.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ChatSupport.Infrastructure.Persistence
{
    public class AppDBContext : DbContext, IAppDBContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Agent> Agent { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public void SeedAgents()
        {
            if (!Agent.Any())
            {
                Agent.AddRange(new List<Agent>
                {
                    //new Agent(Role.MidLevel),
                    //new Agent(Role.MidLevel),
                    new Agent(Role.Junior),
                    new Agent(Role.Junior),
                    new Agent(Role.MidLevel),
                    new Agent(Role.MidLevel),
                    new Agent(Role.Senior),
                    new Agent(Role.TeamLead),
                    new Agent(Role.OverflowJunior),
                    new Agent(Role.OverflowJunior),
                    new Agent(Role.OverflowJunior),
                    new Agent(Role.OverflowJunior),
                    new Agent(Role.OverflowJunior),
                    new Agent(Role.OverflowJunior),
                });

                //Agent.AddRange(new List<Agent>
                //{
                //    new Agent(Role.TeamLead),
                //    new Agent(Role.MidLevel),
                //    new Agent(Role.Junior),
                //    new Agent(Role.Senior),
                //    new Agent(Role.OverflowJunior)
                //});

                SaveChanges();
            }
        }
    }
}
