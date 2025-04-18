using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Robots.Communications;
using Newtonsoft.Json;

namespace Nerbotix.Infrastructure.Data.Configurations.Robots;

public class HttpCommunicationConfiguration : IEntityTypeConfiguration<HttpCommunication>
{
    public void Configure(EntityTypeBuilder<HttpCommunication> builder)
    {
        builder.Property(c => c.Headers)
            .HasConversion(
                h => JsonConvert.SerializeObject(h),
                h => JsonConvert.DeserializeObject<Dictionary<string, string>>(h) ?? new Dictionary<string, string>()
            );
    }
}