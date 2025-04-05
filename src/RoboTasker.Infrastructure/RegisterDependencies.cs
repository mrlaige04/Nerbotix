using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoboTasker.Application.Algorithms;
using RoboTasker.Application.BackgroundJobs;
using RoboTasker.Application.Chatting;
using RoboTasker.Application.Common.Data;
using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Services;
using RoboTasker.Domain.Algorithms;
using RoboTasker.Domain.Repositories;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;
using RoboTasker.Infrastructure.Algorithms.Classical;
using RoboTasker.Infrastructure.Algorithms.Heuristic;
using RoboTasker.Infrastructure.Algorithms.MathBased;
using RoboTasker.Infrastructure.Authentication;
using RoboTasker.Infrastructure.Authentication.Providers;
using RoboTasker.Infrastructure.Authentication.Services;
using RoboTasker.Infrastructure.Chatting;
using RoboTasker.Infrastructure.Data;
using RoboTasker.Infrastructure.Data.Interceptors;
using RoboTasker.Infrastructure.Data.Prefill;
using RoboTasker.Infrastructure.Emailing;
using RoboTasker.Infrastructure.Hangfire;
using RoboTasker.Infrastructure.Hangfire.Jobs;

namespace RoboTasker.Infrastructure;

public static class RegisterDependencies
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTaskDistribution(configuration);
        services.AddAppAuthentication(configuration);
        services.AddDatabase(configuration);
        services.AddEmailing(configuration);
        services.AddBackgroundJobs(configuration);
        services.AddChatting(configuration);
    }

    private static void AddTaskDistribution(this IServiceCollection services, IConfiguration configuration)
    {
        // Classical algorithms
        services.AddKeyedScoped<ITaskDistributionAlgorithm, LoadBalancingTaskDistributionAlgorithm>(AlgorithmNames.LoadBalancing);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, LatestTaskFinishedTaskDistributionAlgorithm>(AlgorithmNames.LatestTaskFinished);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, RoundRobinTaskDistributionAlgorithm>(AlgorithmNames.RoundRobin);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, LeastConnectionsTaskDistributionAlgorithm>(AlgorithmNames.LeastConnections);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, RandomTaskDistributionAlgorithm>(AlgorithmNames.Random);
        
        // Math-based
        services.AddKeyedScoped<ITaskDistributionAlgorithm, LinearOptimizationTaskDistributionAlgorithm>(AlgorithmNames.LinearOptimization);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, FuzzyLogicTaskDistributionAlgorithm>(AlgorithmNames.FuzzyLogic);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, AssignmentProblemTaskDistributionAlgorithm>(AlgorithmNames.AssignmentProblem);
        
        // Heuristic
        services.AddKeyedScoped<ITaskDistributionAlgorithm, GeneticTaskDistributionAlgorithm>(AlgorithmNames.GeneticTask);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, AntColonyTaskDistributionAlgorithm>(AlgorithmNames.AntColony);
        services.AddKeyedScoped<ITaskDistributionAlgorithm, SimulatedAnnealingTaskDistributionAlgorithm>(AlgorithmNames.SimulatedAnnealing);
        
        // AI-based
        
    }

    private static void AddEmailing(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(EmailOptions.SectionName).Get<EmailOptions>();
        ArgumentNullException.ThrowIfNull(options);
        
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services
            .AddFluentEmail(options.From)
            .AddSmtpSender(new SmtpClient(options.Host)
            {
                Port = options.Port,
                Credentials = new NetworkCredential(options.From, options.Password),
                EnableSsl = true,
                UseDefaultCredentials = false
            });

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<TemplateService>();
    }

    private static void AddChatting(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddScoped<IChatService, ChatService>();
    }
    
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine(connectionString);
        services.AddScoped<ISaveChangesInterceptor, AssignTenantInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        
        services.AddDbContext<RoboTaskerDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions
                    .UseNetTopologySuite()
                    .MigrationsAssembly(typeof(RoboTaskerDbContext).Assembly)
            );
        });
        
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<RoboTaskerDbContext>());
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(ITenantRepository<>), typeof(TenantBaseRepository<>));
        
        services.AddScoped<ITenantSeeder, AppDbContextSeeder>();
    }
    
    private static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions);

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = jwtOptions.ToTokenValidationParameters();
            });
        
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Tokens.PasswordResetTokenProvider = "NumericEmail";
                options.ClaimsIdentity.EmailClaimType = JwtRegisteredClaimNames.Email;
                options.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.Name;
                options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;

                options.Tokens.EmailConfirmationTokenProvider = "24LengthCode";
            })
            .AddRoles<Role>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<CodeConfirmationTokenProvider<User>>("24LengthCode")
            .AddTokenProvider<NumericEmailTokenProvider<User>>("NumericEmail")
            .AddEntityFrameworkStores<RoboTaskerDbContext>();

        services.AddScoped<TokenService>();

        services.AddScoped<IUserEmailSender, UserEmailSender>();
    }

    private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HangfireDashboardOptions>(configuration.GetSection(HangfireDashboardOptions.SectionName));
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddHangfire((sp, cfg) =>
        {
            cfg.UseSimpleAssemblyNameTypeSerializer();
            cfg.UsePostgreSqlStorage(opt => opt.UseNpgsqlConnection(connectionString));
        });
        
        services.AddHangfireServer();

        services.AddScoped<TaskAssignJob>();
        services.AddScoped<IJobsService, JobsService>();
    }
}