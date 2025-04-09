using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Data;

public class NerbotixDbContext(DbContextOptions<NerbotixDbContext> options) 
    : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Tenant> Tenants { get; set; } = null!;

    public DbSet<Robot> Robots { get; set; } = null!;
    public DbSet<RobotCategory> RobotCategories { get; set; } = null!;
    public DbSet<RobotCommunication> RobotCommunications { get; set; } = null!;
    public DbSet<RobotCustomProperty> RobotCustomProperties { get; set; } = null!;
    public DbSet<RobotProperty> RobotProperties { get; set; } = null!;
    public DbSet<RobotPropertyValue> RobotPropertyValues { get; set; } = null!;
    
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    
    public DbSet<RobotTask> Tasks { get; set; } = null!;
    public DbSet<RobotTaskData> TasksData { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        builder.Ignore<IdentityUserRole<Guid>>();
    }
}