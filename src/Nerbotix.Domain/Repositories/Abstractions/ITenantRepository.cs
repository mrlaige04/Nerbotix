using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Repositories.Abstractions;

public interface ITenantRepository<TTenantEntity> : IBaseRepository<TTenantEntity> 
    where TTenantEntity : class, ITenantEntity<Guid>
{
    
}