namespace Solly.UI.Core;

public static  class SAlertLevelExtensions
{
    public static string ToCssClass(this SAlertLevel l) => l switch
    {
        SAlertLevel.Success => "s-alert-success",
        SAlertLevel.Warning => "s-alert-warning",
        SAlertLevel.Error   => "s-alert-error",
        _                   => "s-alert-info"
    };

    public static string DefaultIcon(this SAlertLevel l) => l switch
    {
        SAlertLevel.Success => Icons.SIcons.CircleCheck,
        SAlertLevel.Warning => Icons.SIcons.TriangleAlert,
        SAlertLevel.Error   => Icons.SIcons.CircleAlert,
        _                   => Icons.SIcons.CircleInfo
    };
}