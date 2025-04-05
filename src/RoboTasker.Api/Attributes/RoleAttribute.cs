using Microsoft.AspNetCore.Mvc;

namespace RoboTasker.Api.Attributes;

public class RoleAttribute : TypeFilterAttribute
{
    public string Role { get; set; }
    public RoleAttribute(string role) : base(typeof(RoleFilter))
    {
        Role = role;
        Arguments = [role];
    }
}