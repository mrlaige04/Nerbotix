using ErrorOr;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.User.GetProfilePicture;

public class GetProfilePictureHandler(
    IConfiguration configuration) : IQueryHandler<GetProfilePictureQuery, FileStream?>
{
    public async Task<ErrorOr<FileStream?>> Handle(GetProfilePictureQuery request, CancellationToken cancellationToken)
    {
        var root = configuration["Storage:Root"]!;
        var directory = Path.Combine(root, request.UserId.ToString());

        if (!Directory.Exists(directory))
        {
            return Stream.Null as FileStream ?? null;
        }
        
        var files = Directory.GetFiles(directory, "avatar.*").ToList();
        if (files.Count == 0)
        {
            return Stream.Null as FileStream ?? null;
        }
        
        var path = files.First();
        return File.OpenRead(path);
    }
}