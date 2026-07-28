namespace Solly.UI.Core;

public static class SChipLevelExtensions
{
    public static string ToCssClass(this SChipLevel l) => l switch
    {
        SChipLevel.Neutral => "s-chip-neutral",
        SChipLevel.Success => "s-chip-success",
        SChipLevel.Warning => "s-chip-warning",
        SChipLevel.Error   => "s-chip-error",
        SChipLevel.Info    => "s-chip-info",
        _                  => "s-chip-accent"
    };
}