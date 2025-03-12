namespace RoboTasker.Application.Common.Models;

public class IdList
{
    public IList<Guid> Ids { get; set; } = [];
    
    public static IdList Empty => new IdList();
}