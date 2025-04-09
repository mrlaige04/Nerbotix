using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Chatting;

namespace Nerbotix.Infrastructure.Data.Configurations.Chatting;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.ToTable("chats_users");
        builder.HasKey(x => new { x.UserId, x.ChatId });
    }
}