using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Categories.DeleteCategory;

public class DeleteCategoryHandler(
    ITenantRepository<RobotCategory> robotCategoryRepository) : ICommandHandler<DeleteCategoryCommand>
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
        
        await robotCategoryRepository.DeleteAsync(category, cancellationToken);
        
        return new Success();
    }
}