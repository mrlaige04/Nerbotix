using Microsoft.AspNetCore.Mvc;

namespace Nerbotix.Api.Attributes;

public class OnlySuperAdminAttribute() : TypeFilterAttribute(typeof(OnlySuperAdminFilter));