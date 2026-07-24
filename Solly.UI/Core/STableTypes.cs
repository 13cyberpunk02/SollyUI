namespace Solly.UI.Core;

public enum SSortDirection { None, Ascending, Descending }

public enum SAlign { Start, Center, End }

/// <summary>What the table asks its provider for.</summary>
public sealed class STableRequest
{
    public int Skip { get; init; }
    public int Take { get; init; }
    public string? SortColumn { get; init; }
    public SSortDirection SortDirection { get; init; } = SSortDirection.None;
    public string? Search { get; init; }
    public CancellationToken CancellationToken { get; init; }
}

/// <summary>What the provider returns.</summary>
public sealed class STableResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>Total row count across all pages, for the pager.</summary>
    public int TotalCount { get; init; }

    public static STableResult<T> Empty { get; } = new();
}

public static class GAlignExtensions
{
    public static string ToCssClass(this SAlign a) => a switch
    {
        SAlign.Center => "s-ta-center",
        SAlign.End    => "s-ta-end",
        _             => "s-ta-start"
    };
}