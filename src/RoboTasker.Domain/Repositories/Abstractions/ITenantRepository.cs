using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Repositories.Abstractions;

public interface ITenantRepository<TTenantEntity> : IBaseRepository<TTenantEntity> 
    where TTenantEntity : class, ITenantEntity<Guid>
{
    
}