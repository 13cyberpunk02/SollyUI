namespace Solly.UI.Core;

public sealed class SollyOptions
{
    /// <summary>"dark" | "light". Applied to &lt;html data-solly-theme&gt;.</summary>
    public string Theme { get; set; } = "dark";
    
    /// <summary>Accent palette. Teal by default.</summary>
    public SPalette Palette { get; set; } = SPalette.Teal;

    /// <summary>Custom hue 0-360. Used when Palette is Custom.</summary>
    public int Hue { get; set; } = 173;
    public int Saturation { get; set; } = 100;
    public int Lightness { get; set; } = 47;
}