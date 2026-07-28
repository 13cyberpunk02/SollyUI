# SollyUI.Demo.Wasm

Blazor **WebAssembly** host for the SollyUI demo. A thin shell — all the actual pages live in [`SollyUI.Demo.Shared`](../SollyUI.Demo.Shared). Running this alongside the Server host proves the components behave identically in the browser with no server round-trips.

## Run

```bash
dotnet run --project SollyUI.Demo.Wasm
```

Then open the printed `https://localhost:xxxx` URL.

## How it's wired

- References `SollyUI` (the library) and `SollyUI.Demo.Shared` (the pages).
- `Program.cs` registers the library:

  ```csharp
  builder.Services.AddSollyUI(o => o.Theme = "dark");
  ```

- `App.razor` (or the root component) points the router at the shared assembly:

  ```razor
    <Router AppAssembly="@typeof(Program).Assembly"
            AdditionalAssemblies="[typeof(Shared.Components.Pages.Home).Assembly]">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>    
    </Router>
  ```

- `wwwroot/index.html` links both stylesheets and includes the pre-boot theme script:

  ```html
    <link rel="stylesheet" href="_content/Solly.UI/solly.css"/>
    <link rel="stylesheet" href="_content/SollyUI.Demo.Shared/app.css"/>
  ```

Not published to NuGet — this is a demo host.
