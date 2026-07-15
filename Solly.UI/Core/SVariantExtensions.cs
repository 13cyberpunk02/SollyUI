namespace Solly.UI.Core;

public static class SVariantExtensions
{
    public static string ToCssClass(this SVariant v) => v switch
    {
        SVariant.Primary => "s-v-primary",
        SVariant.Ghost   => "s-v-ghost",
        SVariant.Danger  => "s-v-danger",
        _                => "s-v-default"
    };
}