using ChatSupport.Application.Common.Interfaces;

namespace ChatSupport.Infrastructure.SystemDateTimeService
{
    public class SystemDateTimeService : ISystemDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
