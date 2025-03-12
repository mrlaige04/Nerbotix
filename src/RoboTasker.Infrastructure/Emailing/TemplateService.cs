namespace RoboTasker.Infrastructure.Emailing;

public class TemplateService
{
    public async Task<string> RenderTemplate(string templateName, Dictionary<string, string> placeholders)
    {
        var templateContent = await ReadTemplate(templateName);

        foreach (var placeholder in placeholders)
        {
            templateContent = templateContent.Replace($"[{placeholder.Key}]", placeholder.Value);
        }
        
        return templateContent;
    }

    private static async Task<string> ReadTemplate(string templateName)
    {
        var project = Path.GetDirectoryName(typeof(TemplateService).Assembly.Location)!;
        var path = Path.Combine(project, "Emailing", "Templates", $"{templateName}.html");
        
        using var streamReader = new StreamReader(path);
        return await streamReader.ReadToEndAsync();
    }
}