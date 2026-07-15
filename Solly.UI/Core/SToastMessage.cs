namespace Solly.UI.Core;

public sealed class SToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public string? Title { get; init; }
    public string Text { get; init; } = "";
    public SToastLevel Level { get; init; } = SToastLevel.Info;
    public int DurationMs { get; init; } = 4000;
    public bool Dismissible { get; init; } = true;
    public string? ActionText { get; init; }
    public Func<Task>? OnAction { get; init; }
}