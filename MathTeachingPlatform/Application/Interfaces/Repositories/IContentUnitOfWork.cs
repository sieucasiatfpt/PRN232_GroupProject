using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IContentUnitOfWork : IUnitOfWork
    {
        IRepository<Class> Classes { get; }
        IRepository<ClassStudent> ClassStudents { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Syllabus> Syllabi { get; }
        IRepository<ExamMatrix> ExamMatrices { get; }
        IRepository<ExamQuestion> ExamQuestions { get; }
        IRepository<Activity> Activities { get; }
        IRepository<ExamAssignment> ExamAssignments { get; }
    }
}
