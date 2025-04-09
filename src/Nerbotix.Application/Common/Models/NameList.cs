namespace Nerbotix.Application.Common.Models;

public class NameList
{
    public IList<string> Names { get; set; } = [];
    
    public static NameList Empty => new NameList();
}