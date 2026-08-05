namespace Solly.UI.Core;

public class SCommand
{
    public string Title { get; set; } = "";
    public string? Group { get; set; }
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public string? Description { get; set; }

    /// <summary>Extra text matched by search but not displayed (aliases, keywords).</summary>
    public string? Keywords { get; set; }

    public Func<Task>? OnInvoke { get; set; }

    /// <summary>Navigation target; used if OnInvoke is null.</summary>
    public string? Href { get; set; }

    public bool Disabled { get; set; }
}