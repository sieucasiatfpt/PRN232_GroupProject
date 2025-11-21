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
    public class ContentUnitOfWork : UnitOfWorkBase, IContentUnitOfWork
    {
        private IRepository<Class>? _classes;
        private IRepository<ClassStudent>? _classStudents;
        private IRepository<Subject>? _subjects;
        private IRepository<Syllabus>? _syllabi;
        private IRepository<ExamMatrix>? _examMatrices;
        private IRepository<ExamQuestion>? _examQuestions;
        private IRepository<Activity>? _activities;
        private IRepository<ExamAssignment>? _examAssignments;

        public ContentUnitOfWork(ContentDbContext context) : base(context) { }

        public IRepository<Class> Classes => _classes ??= new Repository<Class>(_context);
        public IRepository<ClassStudent> ClassStudents => _classStudents ??= new Repository<ClassStudent>(_context);
        public IRepository<Subject> Subjects => _subjects ??= new Repository<Subject>(_context);
        public IRepository<Syllabus> Syllabi => _syllabi ??= new Repository<Syllabus>(_context);
        public IRepository<ExamMatrix> ExamMatrices => _examMatrices ??= new Repository<ExamMatrix>(_context);
        public IRepository<ExamQuestion> ExamQuestions => _examQuestions ??= new Repository<ExamQuestion>(_context);
        public IRepository<Activity> Activities => _activities ??= new Repository<Activity>(_context);
        public IRepository<ExamAssignment> ExamAssignments => _examAssignments ??= new Repository<ExamAssignment>(_context);
    }
}
