namespace RoboTasker.Infrastructure.Storage;

public class StorageOptions
{
    public const string SectionName = "Storage";
    
    public string Root { get; set; } = null!;
}