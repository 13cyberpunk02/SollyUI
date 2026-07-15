using Microsoft.AspNetCore.Components;

namespace Solly.UI.Core;

public abstract class SComponentBase : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? Attributes { get; set; }

    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    /// <summary>Unique id, stable across re-renders.</summary>
    protected string Uid { get; } = $"s{Guid.NewGuid():N}"[..9];

    protected string Cls(params string?[] parts)
    {
        var all = parts.Concat([Class])
            .Where(p => !string.IsNullOrWhiteSpace(p));
        return string.Join(' ', all);
    }
}