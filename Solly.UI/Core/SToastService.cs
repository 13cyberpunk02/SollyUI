namespace Solly.UI.Core;

public sealed class SToastService
{
    private readonly List<SToastMessage> _items = new();

    public IReadOnlyList<SToastMessage> Items => _items;

    public event Action? Changed;

    public void Show(SToastMessage m)
    {
        _items.Add(m);
        Changed?.Invoke();
    }

    public void Info(string text, string? title = null) =>
        Show(new SToastMessage { Text = text, Title = title, Level = SToastLevel.Info });

    public void Success(string text, string? title = null) =>
        Show(new SToastMessage { Text = text, Title = title, Level = SToastLevel.Success });

    public void Warning(string text, string? title = null) =>
        Show(new SToastMessage { Text = text, Title = title, Level = SToastLevel.Warning });

    public void Error(string text, string? title = null) =>
        Show(new SToastMessage { Text = text, Title = title, Level = SToastLevel.Error, DurationMs = 7000 });

    public void Remove(Guid id)
    {
        var n = _items.RemoveAll(x => x.Id == id);
        if (n > 0) Changed?.Invoke();
    }

    public void Clear()
    {
        if (_items.Count == 0) return;
        _items.Clear();
        Changed?.Invoke();
    }
}