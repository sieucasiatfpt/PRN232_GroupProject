using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAiUnitOfWork : IUnitOfWork
    {
        IRepository<AIConfig> AIConfigs { get; }
        IRepository<AICallLog> AICallLogs { get; }
        IRepository<AIHistoryChat> AIHistoryChats { get; }
    }
}
