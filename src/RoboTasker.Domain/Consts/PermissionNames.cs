namespace RoboTasker.Domain.Consts;

public class PermissionNames
{
    public const string CategoriesGroup = "Categories";
    public const string CategoriesRead = "Categories.Read";
    public const string CategoriesCreate = "Categories.Create";
    public const string CategoriesUpdate = "Categories.Update";
    public const string CategoriesDelete = "Categories.Delete";
    
    public const string CapabilitiesGroup = "Capabilities";
    public const string CapabilitiesRead = "Capabilities.Read";
    public const string CapabilitiesCreate = "Capabilities.Create";
    public const string CapabilitiesUpdate = "Capabilities.Update";
    public const string CapabilitiesDelete = "Capabilities.Delete";
    
    public const string RobotsGroup = "Robots";
    public const string RobotsRead = "Robots.Read";
    public const string RobotsCreate = "Robots.Create";
    public const string RobotsUpdate = "Robots.Update";
    public const string RobotsDelete = "Robots.Delete";
    
    public const string TasksGroup = "Tasks";
    public const string TasksRead = "Tasks.Read";
    public const string TasksCreate = "Tasks.Create";
    public const string TasksUpdate = "Tasks.Update";
    public const string TasksDelete = "Tasks.Delete";
    
    public const string UsersGroup = "Users";
    public const string UsersRead = "Users.Read";
    public const string UsersCreate = "Users.Create";
    public const string UsersUpdate = "Users.Update";
    public const string UsersDelete = "Users.Delete";
    
    public const string RolesGroup = "Roles";
    public const string RolesRead = "Roles.Read";
    public const string RolesCreate = "Roles.Create";
    public const string RolesUpdate = "Roles.Update";
    public const string RolesDelete = "Roles.Delete";
    
    public const string PermissionsGroup = "Permissions";
    public const string PermissionsRead = "Permissions.Read";
    public const string PermissionsCreate = "Permissions.Create";
    public const string PermissionsUpdate = "Permissions.Update";
    public const string PermissionsDelete = "Permissions.Delete";
    
    public const string TenantSettingsGroup = "TenantSettings";
    public const string TenantSettingsRead = "TenantSettings.Read";
    public const string TenantSettingsCreate = "TenantSettings.Create";
    public const string TenantSettingsUpdate = "TenantSettings.Update";
    public const string TenantSettingsDelete = "TenantSettings.Delete";
    
    public const string ChatGroup = "Chat";
    public const string ChatRead = "Chat.Read";
    public const string ChatCreate = "Chat.Create";
    public const string ChatUpdate = "Chat.Update";
    public const string ChatDelete = "Chat.Delete";

    public static Dictionary<string, string[]> GetAll()
    {
        return new Dictionary<string, string[]>
        {
            { CategoriesGroup, [CategoriesRead, CategoriesCreate, CategoriesUpdate, CategoriesDelete] },
            { CapabilitiesGroup, [CapabilitiesRead, CapabilitiesCreate, CapabilitiesUpdate, CapabilitiesDelete] },
            { RobotsGroup, [RobotsRead, RobotsCreate, RobotsUpdate, RobotsDelete] },
            { TasksGroup, [TasksRead, TasksCreate, TasksUpdate, TasksDelete] },
            { UsersGroup, [UsersRead, UsersCreate, UsersUpdate, UsersDelete] },
            { RolesGroup, [RolesRead, RolesCreate, RolesUpdate, RolesDelete] },
            { PermissionsGroup, [PermissionsRead, PermissionsCreate, PermissionsUpdate, PermissionsDelete] },
            { TenantSettingsGroup, [TenantSettingsRead, TenantSettingsCreate, TenantSettingsUpdate, TenantSettingsDelete] },
            { ChatGroup, [ChatRead, ChatCreate, ChatUpdate, ChatDelete] }
        };
    }
}