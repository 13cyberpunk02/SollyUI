namespace Solly.UI.Core;

public static class SPaletteExtensions
{
    public static (int H, int S, int L) ToHsl(this SPalette p) => p switch
    {
        SPalette.Cyan    => (190, 100, 50),
        SPalette.Violet  => (265, 100, 65),
        SPalette.Magenta => (320, 100, 62),
        SPalette.Amber   => (38, 100, 55),
        SPalette.Lime    => (95, 85, 55),
        _                => (173, 100, 47),
    };
}