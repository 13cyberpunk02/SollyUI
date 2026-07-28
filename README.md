# SollyUI

A glassmorphism + neon component library for Blazor — frosted-glass surfaces, a single-hue neon accent system, and a full component set for Blazor Server and WebAssembly.

[![NuGet](https://img.shields.io/nuget/v/SollyUI.svg)](https://www.nuget.org/packages/SollyUI)
[![License: MIT](https://img.shields.io/badge/License-MIT-informational.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com)

## Install

```bash
dotnet add package SollyUI //soon not now
```

See the [library README](src/SollyUI/README.md) for setup, usage, and the full component list.

## Repository layout

| Project | Description |
|---|---|
| [`Solly.UI`](src/SollyUI) | The component library. This is what ships to NuGet. |
| [`SollyUI.Demo.Shared`](src/SollyUI.Demo.Shared) | Razor Class Library with the demo pages, shared by both hosts. |
| [`SollyUI.Demo`](src/SollyUI.Demo) | Blazor **Server** host for the demo. |
| [`SollyUI.Demo.Wasm`](src/SollyUI.Demo.Wasm) | Blazor **WebAssembly** host for the demo. |
| [`SollyUI.Tests`](tests/SollyUI.Tests) | bUnit + xUnit test suite. |

> Paths above assume a `src/` and `tests/` layout — adjust the links if your folders differ.

## Running the demo

Server:

```bash
dotnet run --project src/SollyUI.Demo
```

WebAssembly:

```bash
dotnet run --project src/SollyUI.Demo.Wasm
```

Both hosts render the same demo pages from `SollyUI.Demo.Shared`, proving the components work identically under Server and WASM.

## Tests

```bash
dotnet test
```

## License

[MIT](LICENSE) © 2026 13cyberpunk02
