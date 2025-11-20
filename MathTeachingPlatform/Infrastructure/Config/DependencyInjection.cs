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
using Npgsql;

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

            string Normalize(string conn)
            {
                try
                {
                    var poolUriStr = cfg["Supabase:PrimaryDatabaseUri"];
                    if (string.IsNullOrWhiteSpace(poolUriStr)) return conn;
                    var nb = new NpgsqlConnectionStringBuilder(conn);
                    if (nb.Host.EndsWith(".supabase.co"))
                    {
                        var uri = new Uri(poolUriStr);
                        var userInfo = uri.UserInfo.Split(':');
                        nb.Host = uri.Host;
                        nb.Port = uri.Port;
                        nb.Username = userInfo[0];
                        if (userInfo.Length > 1) nb.Password = userInfo[1];
                        return nb.ConnectionString;
                    }
                    return conn;
                }
                catch { return conn; }
            }

            authConn = Normalize(authConn);
            contentConn = Normalize(contentConn);
            examConn = Normalize(examConn);
            aiConn = Normalize(aiConn);

            try
            {
                var ab = new NpgsqlConnectionStringBuilder(authConn);
                var cb = new NpgsqlConnectionStringBuilder(contentConn);
                var eb = new NpgsqlConnectionStringBuilder(examConn);
                var aib = new NpgsqlConnectionStringBuilder(aiConn);
                Console.WriteLine($"DB hosts -> auth:{ab.Host} content:{cb.Host} exam:{eb.Host} ai:{aib.Host}");
            }
            catch { }

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
                var baseUrl = cfg["Services:TeacherService:BaseUrl"] ?? "https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<ISubjectApiClient, SubjectApiClient>(client =>
            {
                var baseUrl = cfg["Services:SubjectService:BaseUrl"] ?? "https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IStudentApiClient, StudentApiClient>(client =>
            {
                var baseUrl = cfg["Services:StudentService:BaseUrl"] ?? "https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IClassApiClient, ClassApiClient>(client =>
            {
                var baseUrl = cfg["Services:ClassService:BaseUrl"] ?? "https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Register JWT service
            services.AddSingleton<IJwtService, JwtService>();

            // Register business services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IAiConfigService, AiConfigService>();
            services.AddScoped<IAiHistoryChatService, AiHistoryChatService>();

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
