using ErrorOr;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Application.User.UploadProfilePicture;

public class UploadProfilePictureHandler(
    IConfiguration configuration,
    ICurrentUser currentUser) : ICommandHandler<UploadProfilePictureCommand>
{
    public async Task<ErrorOr<Success>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
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
        
        var ext = Path.GetExtension(request.File.FileName);
        await using var fs = File.Create(Path.Combine(directory, $"avatar{ext}"));
        await request.File.CopyToAsync(fs, cancellationToken);

        return new Success();
    }
}