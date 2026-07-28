using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Solly.UI;

namespace SollyUI.Tests;

/// <summary>
/// bUnit's JSInterop is strict by default — any unconfigured call throws.
/// SollyInterop imports "./_content/Solly.UI/solly.js" and calls functions on it.
/// We register the module and stub every function the components use.
/// </summary>
public abstract class SollyTestContext : BunitContext
{
    protected SollyTestContext()
    {
        Services.AddSollyUI();
        JSInterop.Mode = JSRuntimeMode.Loose;   // ← незастабленное возвращает default, не кидает

        // остальное можно оставить или убрать — в Loose оно не обязательно,
        // но пусть будет для явности:
        var m = JSInterop.SetupModule("./_content/Solly.UI/solly.js");
        m.SetupVoid("anchor", _ => true);
        m.SetupVoid("anchorTip", _ => true);
        m.SetupVoid("focusEl", _ => true);
        m.SetupVoid("scrollItemIntoView", _ => true);
        m.SetupVoid("setTheme", _ => true).SetVoidResult();
        m.SetupVoid("setPalette", _ => true).SetVoidResult();
        m.Setup<string?>("getStoredTheme", _ => true).SetResult(null);
        m.Setup<int[]?>("getStoredPalette", _ => true).SetResult(null);
    }

    /// <summary>
    /// Disambiguates Render&lt;T&gt;(Action&lt;...&gt;) from Render(RenderFragment).
    /// </summary>
    protected IRenderedComponent<T> RenderC<T>(
        Action<ComponentParameterCollectionBuilder<T>> configure) where T : IComponent
        => Render(configure);
}
