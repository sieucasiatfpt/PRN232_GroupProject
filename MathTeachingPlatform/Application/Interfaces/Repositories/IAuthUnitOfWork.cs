using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAuthUnitOfWork : IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<Student> Students { get; }
        IRepository<Payment> Payments { get; }
    }
}
