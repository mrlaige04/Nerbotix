using Microsoft.AspNetCore.Mvc;

namespace RoboTasker.Api.Attributes;

public class OnlySuperAdminAttribute() : TypeFilterAttribute(typeof(OnlySuperAdminFilter));