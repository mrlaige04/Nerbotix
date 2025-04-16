using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.User.GetProfilePicture;

public record GetProfilePictureQuery(Guid UserId) : IQuery<FileStream?>;