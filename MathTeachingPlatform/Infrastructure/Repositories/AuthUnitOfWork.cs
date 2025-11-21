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
    public class AuthUnitOfWork : UnitOfWorkBase, IAuthUnitOfWork
    {
        private IRepository<User>? _users;
        private IRepository<Teacher>? _teachers;
        private IRepository<Student>? _students;
        private IRepository<Payment>? _payments;

        public AuthUnitOfWork(AuthDbContext context) : base(context) { }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Teacher> Teachers => _teachers ??= new Repository<Teacher>(_context);
        public IRepository<Student> Students => _students ??= new Repository<Student>(_context);
        public IRepository<Payment> Payments => _payments ??= new Repository<Payment>(_context);
    }
}
