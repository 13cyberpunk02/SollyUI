namespace Solly.UI.Core;

public enum SCardVariant { Default, Neon, Flat, Outline }
public enum SCardPadding { None, Compact, Normal, Roomy }

public static class GCardEnumExtensions
{
    public static string ToCssClass(this SCardVariant v) => v switch
    {
        SCardVariant.Neon    => "s-card-neon",
        SCardVariant.Flat    => "s-card-flat",
        SCardVariant.Outline => "s-card-outline",
        _                    => "s-card-default"
    };

    public static string ToCssClass(this SCardPadding p) => p switch
    {
        SCardPadding.None    => "s-card-p-none",
        SCardPadding.Compact => "s-card-p-sm",
        SCardPadding.Roomy   => "s-card-p-lg",
        _                    => "s-card-p-md"
    };
}