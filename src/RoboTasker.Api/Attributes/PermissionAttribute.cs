using Microsoft.AspNetCore.Mvc;

namespace RoboTasker.Api.Attributes;

public class PermissionAttribute : TypeFilterAttribute
{
    public string Permission { get; set; }
    public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
    {
        Permission = permission;
        Arguments = [permission];
    }
}