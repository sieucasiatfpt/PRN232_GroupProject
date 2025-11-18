using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Models.Payment;
using Infrastructure.ApiClients;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            var authConn = cfg.GetConnectionString("AuthDatabase") ?? throw new System.Exception("AuthDatabase missing");
            var contentConn = cfg.GetConnectionString("ContentDatabase") ?? throw new System.Exception("ContentDatabase missing");
            var examConn = cfg.GetConnectionString("ExamDatabase") ?? throw new System.Exception("ExamDatabase missing");
            var aiConn = cfg.GetConnectionString("AiDatabase") ?? throw new System.Exception("AiDatabase missing");

            services.AddDbContext<AuthDbContext>(o =>
                o.UseNpgsql(authConn, npgsql => npgsql.EnableRetryOnFailure())
            );
            services.AddDbContext<ContentDbContext>(o =>
                o.UseNpgsql(contentConn, npgsql => npgsql.EnableRetryOnFailure())
            );
            services.AddDbContext<ExamDbContext>(o =>
                o.UseNpgsql(examConn, npgsql => npgsql.EnableRetryOnFailure())
            );
            services.AddDbContext<AiDbContext>(o =>
                o.UseNpgsql(aiConn, npgsql => npgsql.EnableRetryOnFailure())
            );

            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
            services.AddScoped<IContentUnitOfWork, ContentUnitOfWork>();
            services.AddScoped<IExamUnitOfWork, ExamUnitOfWork>();
            services.AddScoped<IAiUnitOfWork, AiUnitOfWork>();

            // Configure HTTP clients for microservice communication
            services.AddHttpClient<ITeacherApiClient, TeacherApiClient>(client =>
            {
                var baseUrl = cfg["Services:TeacherService:BaseUrl"] ?? "https://localhost:7075";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<ISubjectApiClient, SubjectApiClient>(client =>
            {
                var baseUrl = cfg["Services:SubjectService:BaseUrl"] ?? "https://localhost:7075";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IStudentApiClient, StudentApiClient>(client =>
            {
                var baseUrl = cfg["Services:StudentService:BaseUrl"] ?? "https://localhost:7075";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IClassApiClient, ClassApiClient>(client =>
            {
                var baseUrl = cfg["Services:ClassService:BaseUrl"] ?? "https://localhost:7075";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Register business services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IClassService, ClassService>();

            // Payment services
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            // MOMO API Payment
            services.Configure<MomoOptionModel>(cfg.GetSection("MomoAPI"));
            services.AddScoped<IMomoService, MomoService>();

            services.AddHttpClient();

            return services;
        }
    }
}
