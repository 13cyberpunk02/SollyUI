# SollyUI.Tests

Unit and component tests for SollyUI, built on **bUnit** + **xUnit**.

## Run

```bash
dotnet test
```

## Structure

- `SollyTestContext` — base class for all component tests. Registers the library services and runs bUnit's JSInterop in **Loose** mode, so components that call into `glassy.js` (portals, focus traps, theme/palette) render without each JS call being stubbed.
- `ComponentTests/` — one test file per component. Simple components and services use `.cs`; anything with `EditForm`, cascading parameters, overlays, or multi-child composition uses `.razor` (inline `@<...>` render fragments only compile in Razor files).

## Notes

- The project targets `Microsoft.NET.Sdk.Razor` so `.razor` test files compile. A plain `Microsoft.NET.Sdk` silently ignores them.
- `_Imports.razor` in the project root brings in the component namespaces, bUnit, FluentAssertions, and xUnit.
