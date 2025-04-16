using ErrorOr;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Application.User.DeleteProfilePicture;

public class DeleteProfilePictureHandler(
    IConfiguration configuration,
    ICurrentUser currentUser) : ICommandHandler<DeleteProfilePictureCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var root = configuration["Storage:Root"]!;
        var userId = currentUser.GetUserId();
        var directory = Path.Combine(root, userId!.Value.ToString());
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var existingAvatars = Directory.GetFiles(directory, "avatar*");
        foreach (var existingAvatar in existingAvatars)
        {
            File.Delete(existingAvatar);
        }
        
        return new Success();
    }
}