# SollyUI.Demo.Shared

Razor Class Library containing every demo page, layout, and demo-only styling. Both the Server (`SollyUI.Demo`) and WebAssembly (`SollyUI.Demo.Wasm`) hosts reference this project, so the demo lives in exactly one place and stays identical across render modes.

## What's here

- `Pages/` — one page per component (Button, Table, Drawer, Form, …).
- `MainLayout.razor` — the demo shell: sidebar, header, theme toggle, toast host.
- `wwwroot/demo.css` — styling for the demo chrome only (`.demo-*` classes). The components themselves are styled by `SollyUI`'s `solly.css`.

## Using it from a host

The host must tell its router about this assembly, otherwise the `@page` components here return 404.

**Router** (`Routes.razor` / `App.razor`):

```razor
<Router AppAssembly="typeof(Program).Assembly"
        AdditionalAssemblies="[typeof(SollyUI.Demo.Shared._Imports).Assembly]">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="@typeof(MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

**Blazor Web App host also needs it in `Program.cs`:**

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SollyUI.Demo.Shared.Components.Layout.MainLayout).Assembly);
```

**Stylesheets** in the host page:

```html
  <link rel="stylesheet" href="_content/Solly.UI/solly.css"/>
  <link rel="stylesheet" href="_content/SollyUI.Demo.Shared/app.css" />
```

This project is part of the demo and is **not** published to NuGet.
