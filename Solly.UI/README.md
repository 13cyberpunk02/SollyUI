# Solly.UI

A glassmorphism + neon component library for Blazor. Frosted-glass surfaces, a single-hue neon accent system, and a full set of form, overlay, layout, and data components — all driven by one design-token palette you can recolor at runtime.

Works in **Blazor Server** and **Blazor WebAssembly**. No Node build step — CSS and fonts ship inside the package as static web assets.

[![NuGet](https://img.shields.io/nuget/v/Solly.UI.svg)](https://www.nuget.org/packages/SollyUI)
[![License: MIT](https://img.shields.io/badge/License-MIT-informational.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com)

---

## Install

```bash
dotnet add package Solly.UI
```

## Setup

Register the services in `Program.cs`:

```csharp
using Solly.UI;

builder.Services.AddSolly.UI(options =>
{
    options.Theme = "dark";          // "dark" | "light"
    options.Palette = SPalette.Teal; // accent hue
});
```

Reference the stylesheet in your host page (`App.razor`, `_Host.cshtml`, or `index.html`):

```html
<link rel="stylesheet" href="_content/Solly.UI/solly.css" />
```

To avoid a flash of the wrong theme on first paint, apply the stored theme before Blazor boots — add this to the `<head>` of your host page:

```html
<script>
    (function () {
        var t = localStorage.getItem('solly-theme') || 'dark';
        document.documentElement.setAttribute('data-solly-theme', t);
    })();
</script>
```

Add the usings to `_Imports.razor`:

```razor
@using Solly.UI
@using Solly.UI.Components
@using Solly.UI.Core
```

## Quick start

```razor
<SButton Variant="SVariant.Primary" Icon="@SIcons.Check">Save</SButton>

<SCard Title="Revenue" Eyebrow="LAST 30 DAYS">
    <SProgress Value="72" Level="SAlertLevel.Success" ShowValue Label="Storage" />
</SCard>

<SAlert Level="SAlertLevel.Warning" Title="Heads up" Dismissible>
    You're using 92% of your quota.
</SAlert>
```

---

## Components

**Form inputs** — `SButton`, `SInput`, `STextArea`, `SNumberInput<T>`, `SCheckbox`, `SSwitch`, `SRadioGroup<T>`, `SSelect<T>` (searchable, multi-select, async), `SSlider`, `SDatePicker`.

**Form layout** — `SGForm`, `SFormSection`, `SFormField`, `SFormActions`, with a built-in validation summary.

**Overlays** — `SModal`, `SDrawer`, `SPopover`, `STooltip`, `SDropdownMenu` (with nested submenus), `SPopconfirm`, `SToast` (service-driven).

**Layout & shell** — `SShell`, `SSidebar`, `SHeader`, `SNavGroup`, `SNavLink`, `SCollapseButton`, `SThemeToggle`, `SAmbient`.

**Data** — `STable<T>` with sorting, pagination, search, row selection, cell templates, and server-side data via an items provider. `SPagination` is also available standalone.

**Display** — `SCard`, `SBadge`, `SChip`, `SAvatar` (+ `SAvatarGroup`), `SAlert`, `SProgress` (linear + circular), `STabs`, `SAccordion`, `SBreadcrumbs`, `SSkeleton`, `SEmpty`, `SIcon`.

---

## Theming

Everything derives from a single accent hue expressed as three CSS custom properties (`--s-h`, `--s-s`, `--s-l`). Change the hue and the entire neon system — glows, gradients, focus rings — recolors.

Pick a preset or a custom hue at runtime:

```csharp
@inject SollyThemeService Theme

<SButton OnClick="@(() => Theme.SetPaletteAsync(SPalette.Violet))">Violet</SButton>
<SButton OnClick="@(() => Theme.SetPaletteAsync(280, 90, 60))">Custom</SButton>
<SButton OnClick="@(() => Theme.ToggleAsync())">Toggle dark/light</SButton>
```

Built-in palettes: `Teal` (default), `Cyan`, `Violet`, `Magenta`, `Amber`, `Lime`, plus `Custom`. Theme and palette choices persist to `localStorage`.

---

## Requirements

- .NET 10.0
- Blazor Server or Blazor WebAssembly

---

## License

[MIT](LICENSE) © 2026 13cyberpunk02
