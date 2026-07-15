using Microsoft.Extensions.Options;
using Solly.UI.Core.Interop;

namespace Solly.UI.Core;

public sealed class SollyThemeService(SollyInterop interop, IOptions<SollyOptions> options)
{
    private string _theme = options.Value.Theme;


    public async Task InitAsync()
    {
        var stored = await interop.GetStoredThemeAsync();
        if (!string.IsNullOrEmpty(stored)) _theme = stored;
        Changed?.Invoke();
    }
    
    public string Theme => _theme;
    public bool IsDark => _theme == "dark";

    public event Action? Changed;

    public async Task SetAsync(string theme)
    {
        if (_theme == theme) return;
        _theme = theme;
        await interop.SetThemeAsync(theme);
        Changed?.Invoke();
    }

    public Task ToggleAsync() => SetAsync(IsDark ? "light" : "dark");

    /// <summary>Applies the initial theme. Call from OnAfterRenderAsync(firstRender).</summary>
    public ValueTask ApplyAsync() => interop.SetThemeAsync(_theme);
}