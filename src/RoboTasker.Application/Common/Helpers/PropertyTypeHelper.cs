using System.Globalization;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Common.Helpers;

public static class PropertyTypeHelper
{
    public static string GetDefaultValue(RobotPropertyType type)
    {
        return type switch
        {
            RobotPropertyType.String => string.Empty,
            RobotPropertyType.Boolean => bool.FalseString,
            RobotPropertyType.Number => 0.ToString(CultureInfo.InvariantCulture),
            RobotPropertyType.DateTime => DateTime.MinValue.ToString(CultureInfo.InvariantCulture),
            _ => string.Empty
        };
    }
}