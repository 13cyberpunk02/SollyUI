using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Solly.UI.Core.Interop;

public sealed class SollyInterop(IJSRuntime js) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _module = new(() => js.InvokeAsync<IJSObjectReference>(
        "import", "./_content/Solly.UI/solly.js").AsTask());

    private Task<IJSObjectReference> Module => _module.Value;

    /// <summary>Positions a popover under an anchor, flipping up if needed.</summary>
    public async ValueTask AnchorAsync(ElementReference popover, ElementReference anchor)
    {
        var m = await Module;
        await m.InvokeVoidAsync("anchor", popover, anchor);
    }

    /// <summary>Registers outside-click/Escape handling. Returns a disposable handle.</summary>
    public async ValueTask<IJSObjectReference> RegisterDismissAsync<T>(
        ElementReference root, ElementReference panel, DotNetObjectReference<T> target) where T : class
    {
        var m = await Module;
        return await m.InvokeAsync<IJSObjectReference>("registerDismiss", root, panel, target);
    }

    public async ValueTask FocusAsync(ElementReference el)
    {
        var m = await Module;
        await m.InvokeVoidAsync("focusEl", el);
    }

    public async ValueTask ScrollIntoViewAsync(ElementReference container, int index)
    {
        var m = await Module;
        await m.InvokeVoidAsync("scrollItemIntoView", container, index);
    }

    public async ValueTask SetThemeAsync(string theme)
    {
        var m = await Module;
        await m.InvokeVoidAsync("setTheme", theme);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_module.IsValueCreated) return;
        try
        {
            var m = await _module.Value;
            await m.DisposeAsync();
        }
        catch (JSDisconnectedException) { }
        catch (ObjectDisposedException) { }
        catch (TaskCanceledException) { }
    }
    
    /// <summary>Reads the persisted theme ("dark" | "light" | "auto"), or null.</summary>
    public async ValueTask<string?> GetStoredThemeAsync()
    {
        try
        {
            var m = await Module;
            return await m.InvokeAsync<string?>("getStoredTheme");
        }
        catch (JSDisconnectedException) { return null; }
        catch (ObjectDisposedException) { return null; }
        catch (TaskCanceledException) { return null; }
    }

    /// <summary>Resolves the OS colour-scheme preference: "dark" | "light".</summary>
    public async ValueTask<string> GetSystemThemeAsync()
    {
        var m = await Module;
        return await m.InvokeAsync<string>("getSystemTheme");
    }
    
    public async ValueTask<IJSObjectReference?> AutoGrowAsync(ElementReference el)
    {
        try
        {
            var m = await Module;
            return await m.InvokeAsync<IJSObjectReference>("autoGrow", el);
        }
        catch (JSDisconnectedException) { return null; }
        catch (ObjectDisposedException) { return null; }
    }
    
    public async ValueTask<IJSObjectReference?> PortalAsync(ElementReference el)
    {
        try
        {
            var m = await Module;
            return await m.InvokeAsync<IJSObjectReference>("portal", el);
        }
        catch (JSDisconnectedException) { return null; }
        catch (ObjectDisposedException) { return null; }
    }
    
    public async ValueTask<IJSObjectReference?> TrapFocusAsync<T>(
        ElementReference el, DotNetObjectReference<T> target) where T : class
    {
        try
        {
            var m = await Module;
            return await m.InvokeAsync<IJSObjectReference>("trapFocus", el, target);
        }
        catch (JSDisconnectedException) { return null; }
        catch (ObjectDisposedException) { return null; }
    }

    public async ValueTask<IJSObjectReference?> LockScrollAsync()
    {
        try
        {
            var m = await Module;
            return await m.InvokeAsync<IJSObjectReference>("lockScroll");
        }
        catch (JSDisconnectedException) { return null; }
        catch (ObjectDisposedException) { return null; }
    }
    
    public async ValueTask AnchorTipAsync(ElementReference tip, ElementReference anchor, string placement)
    {
        try
        {
            var m = await Module;
            await m.InvokeVoidAsync("anchorTip", tip, anchor, placement);
        }
        catch (JSDisconnectedException) { }
        catch (ObjectDisposedException) { }
    }
}