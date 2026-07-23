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

        var module = JSInterop.SetupModule("./_content/Solly.UI/solly.js");

        module.SetupVoid("anchor", _ => true);
        module.SetupVoid("anchorTip", _ => true);
        module.SetupVoid("focusEl", _ => true);
        module.SetupVoid("scrollItemIntoView", _ => true);
        module.SetupVoid("setTheme", _ => true);

        foreach (var fn in new[] { "registerDismiss", "portal", "trapFocus", "lockScroll", "autoGrow" })
        {
            var handle = module.SetupModule(fn);
            handle.SetupVoid("dispose", _ => true);
        }

        // calls returning values
        module.Setup<string?>("getStoredTheme", _ => true).SetResult(null);
        module.Setup<string>("getSystemTheme", _ => true).SetResult("dark");
    }

    /// <summary>
    /// Disambiguates Render&lt;T&gt;(Action&lt;...&gt;) from Render(RenderFragment).
    /// </summary>
    protected IRenderedComponent<T> RenderC<T>(
        Action<ComponentParameterCollectionBuilder<T>> configure) where T : IComponent
        => Render(configure);
}