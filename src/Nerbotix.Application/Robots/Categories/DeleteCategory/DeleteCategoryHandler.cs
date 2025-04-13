using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Robots.Categories.DeleteCategory;

public class DeleteCategoryHandler(
    ITenantRepository<RobotCategory> robotCategoryRepository,
    ITenantRepository<Robot> robotRepository) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await robotCategoryRepository.GetAsync(
            c => c.Id == request.Id,
            cancellationToken: cancellationToken);
        
        if (category == null)
        {
            return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }

        if (await robotRepository.ExistsAsync(c => c.CategoryId == request.Id, cancellationToken: cancellationToken))
        {
            return Error.Failure(CategoryErrors.CannotDeleteLinkedEntities, CategoryErrors.CannotDeleteRobotsDescription);
        }
        
        await robotCategoryRepository.DeleteAsync(category, cancellationToken);
        
        return new Success();
    }
}