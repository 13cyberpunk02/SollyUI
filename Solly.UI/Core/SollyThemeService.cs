using Microsoft.Extensions.Options;
using Solly.UI.Core.Interop;

namespace Solly.UI.Core;

public sealed class SollyThemeService(SollyInterop interop, IOptions<SollyOptions> options)
{
    private string _theme = options.Value.Theme;
    private (int H, int S, int L) _hsl = options.Value.Palette == SPalette.Custom ?
            (options.Value.Hue, options.Value.Saturation, options.Value.Lightness)
            : options.Value.Palette.ToHsl();

    public (int H, int S, int L) Palette => _hsl;


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
    
    public async Task SetPaletteAsync(SPalette p)
    {
        _hsl = p.ToHsl();
        await interop.SetPaletteAsync(_hsl.H, _hsl.S, _hsl.L);
        Changed?.Invoke();
    }

    public async Task SetPaletteAsync(int h, int s = 100, int l = 47)
    {
        _hsl = (h, s, l);
        await interop.SetPaletteAsync(h, s, l);
        Changed?.Invoke();
    }

    public Task ToggleAsync() => SetAsync(IsDark ? "light" : "dark");

    /// <summary>Applies the initial theme. Call from OnAfterRenderAsync(firstRender).</summary>
    public ValueTask ApplyAsync() => interop.SetThemeAsync(_theme);
}