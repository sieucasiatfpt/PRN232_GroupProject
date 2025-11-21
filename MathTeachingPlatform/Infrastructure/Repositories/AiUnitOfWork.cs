using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AiUnitOfWork : UnitOfWorkBase, IAiUnitOfWork
    {
        private IRepository<AIConfig>? _aiConfigs;
        private IRepository<AICallLog>? _aiCallLogs;
        private IRepository<AIHistoryChat>? _aiHistoryChats;

        public AiUnitOfWork(AiDbContext context) : base(context) { }

        public IRepository<AIConfig> AIConfigs => _aiConfigs ??= new Repository<AIConfig>(_context);
        public IRepository<AICallLog> AICallLogs => _aiCallLogs ??= new Repository<AICallLog>(_context);
        public IRepository<AIHistoryChat> AIHistoryChats => _aiHistoryChats ??= new Repository<AIHistoryChat>(_context);
    }
}
