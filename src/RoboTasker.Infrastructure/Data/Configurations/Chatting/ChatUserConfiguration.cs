using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Chatting;

namespace RoboTasker.Infrastructure.Data.Configurations.Chatting;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.ToTable("chats_users");
        builder.HasKey(x => new { x.UserId, x.ChatId });
    }
}