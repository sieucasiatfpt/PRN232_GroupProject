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
    public class ExamUnitOfWork : UnitOfWorkBase, IExamUnitOfWork
    {
        private IRepository<ExamAttempt>? _examAttempts;

        public ExamUnitOfWork(ExamDbContext context) : base(context) { }

        public IRepository<ExamAttempt> ExamAttempts => _examAttempts ??= new Repository<ExamAttempt>(_context);
    }
}
