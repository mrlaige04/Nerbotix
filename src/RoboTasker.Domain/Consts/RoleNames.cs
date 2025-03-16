namespace RoboTasker.Domain.Consts;

public class RoleNames
{
    public const string Admin = "Admin";
    public const string User = "User";

    public static IList<string> GetAll()
    {
        return [Admin, User];
    }
}