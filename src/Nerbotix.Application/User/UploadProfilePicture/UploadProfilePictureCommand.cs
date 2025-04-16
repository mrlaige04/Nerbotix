using Microsoft.AspNetCore.Http;
using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.User.UploadProfilePicture;

public class UploadProfilePictureCommand : ITenantCommand
{
    public IFormFile File { get; set; } = null!;
}

