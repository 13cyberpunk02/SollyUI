# SollyUI.Demo

Blazor **Server** host for the SollyUI demo. A thin shell — all the actual pages live in [`SollyUI.Demo.Shared`](../SollyUI.Demo.Shared).

## Run

```bash
dotnet run --project SollyUI.Demo
```

Then open the printed `https://localhost:xxxx` URL.

## How it's wired

- References `SollyUI` (the library) and `SollyUI.Demo.Shared` (the pages).
- `Program.cs` registers the library and the shared assembly:

  ```csharp
  builder.Services.AddSollyUI(o => o.Theme = "dark");

  app.MapRazorComponents<App>()
  	.AddInteractiveServerRenderMode()
  	.AddAdditionalAssemblies(typeof(SollyUI.Demo.Shared.Components.Layout.MainLayout).Assembly);
  ```

- `Components/App.razor` links both stylesheets and renders `<Routes @rendermode="InteractiveServer" />`.
- `Components/Routes.razor` points the router at the shared assembly via `AdditionalAssemblies`.

Not published to NuGet — this is a demo host.
