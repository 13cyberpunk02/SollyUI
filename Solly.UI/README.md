# Solly.UI

Glassmorphism component library for Blazor. Works on **Blazor Web App** (Server / WebAssembly / Auto) and standalone **Blazor WebAssembly**.

No Bootstrap. No CSS framework. One stylesheet, one JS module, zero runtime dependencies.

---

## Install

```bash
dotnet add package Solly.UI
```

## Setup

### 1. Register services

```csharp
using Solly.UI;

builder.Services.AddSollyUI();

// or with options
builder.Services.AddSollyUI(o => o.Theme = "auto");   // "dark" | "light" | "auto"
```

### 2. Enable interactivity

**This is the most common setup mistake.** Blazor Web App renders statically by default — components will look correct but **clicks will not work**.

`Program.cs`:

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ...

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

`App.razor`:

```razor
<Routes @rendermode="InteractiveServer" />
```

Setting the render mode globally on `<Routes>` is recommended — otherwise your layout (and anything in it, like `SThemeToggle`) stays static even when pages are interactive.

Standalone Blazor WebAssembly is interactive by default; no render mode needed.

### 3. Add the stylesheet

`App.razor` (or `index.html` for standalone WASM):

```html
<head>
    <!-- Prevents a theme flash on load. Must run before the stylesheet. -->
    <script>
        (function () {
            try {
                var t = localStorage.getItem('solly-theme') || 'dark';
                if (t === 'auto') t = matchMedia('(prefers-color-scheme: light)').matches ? 'light' : 'dark';
                document.documentElement.setAttribute('data-solly-theme', t);
            } catch (e) { }
        })();
    </script>

    <link rel="stylesheet" href="_content/Solly.UI/solly.css" />
    <link rel="stylesheet" href="app.css" />
    <HeadOutlet />
</head>
```

Order matters: `solly.css` defines the design tokens that everything else reads.

The inline script cannot be replaced with C# — Blazor boots after first paint, so without it users see a dark flash before the light theme applies.

### 4. Import namespaces

`_Imports.razor`:

```razor
@using Solly.UI.Components
@using Solly.UI.Core
@using Solly.UI.Icons
```

### 5. Give it something to blur

Glassmorphism is invisible on a flat background. Add a gradient or image to `app.css`:

```css
body {
    margin: 0;
    min-height: 100vh;
    font-family: var(--s-font);
    color: var(--s-text);
    background: linear-gradient(135deg, #1a1a2e 0%, #4a2b6b 50%, #16213e 100%) fixed;
}

[data-solly-theme="light"] body {
    background: linear-gradient(135deg, #e0c3fc 0%, #8ec5fc 100%) fixed;
}
```

If you started from the Blazor template, delete these rules from `app.css` — they fight the library's focus rings:

```css
.valid.modified:not([type=checkbox]) { outline: 1px solid #26b050; }
.invalid { outline: 1px solid #e50000; }
```

---

## Components

### SButton

```razor
<SButton>Default</SButton>
<SButton Variant="SVariant.Primary" Icon="@SIcons.Check">Save</SButton>
<SButton Variant="SVariant.Danger" Icon="@SIcons.X">Delete</SButton>
<SButton Variant="SVariant.Ghost">Cancel</SButton>
<SButton Disabled>Disabled</SButton>
<SButton Type="submit" Variant="SVariant.Primary">Submit</SButton>

@* Shows a spinner and blocks re-entry until OnClick completes *@
<SButton OnClick="SaveAsync">Async</SButton>
```

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | — | Button label |
| `Icon` | `string?` | — | SVG path from `SIcons` |
| `IconSize` | `int` | `18` | |
| `Variant` | `SVariant` | `Default` | `Default`, `Primary`, `Ghost`, `Danger` |
| `Disabled` | `bool` | `false` | |
| `Type` | `string` | `"button"` | Use `"submit"` inside `EditForm` |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Awaited; spinner shows while pending |

The ripple follows the pointer via CSS custom properties — no JS.

---

### SInput

Inherits `InputBase<string?>` — full `EditForm` and validation support.

```razor
<SInput @bind-Value="_name"
        Label="Name"
        Placeholder="Jane Doe"
        Icon="@SIcons.Search"
        Hint="Your full name"
        Required
        Clearable />

<SInput @bind-Value="_password" Label="Password" Type="password" />
```

| Parameter | Type | Default |
|---|---|---|
| `Label` | `string?` | — |
| `Placeholder` | `string?` | — |
| `Hint` | `string?` | — |
| `Icon` | `string?` | — |
| `Type` | `string` | `"text"` |
| `Disabled` | `bool` | `false` |
| `Required` | `bool` | `false` |
| `Clearable` | `bool` | `false` |

`Required` is visual only (renders `*`). Use data annotations for actual validation.

---

### STextArea

```razor
<STextArea @bind-Value="_bio"
           Label="Bio"
           Placeholder="Tell us about yourself…"
           MaxLength="200"
           Rows="3"
           Hint="Markdown supported" />

<STextArea @bind-Value="_notes" Label="Notes" AutoGrow Rows="2" />
```

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Rows` | `int` | `4` | |
| `MaxLength` | `int?` | — | Shows a live character counter |
| `AutoGrow` | `bool` | `false` | Grows with content; requires JS |

---

### SCheckbox

Does **not** inherit `InputBase` — works with or without `EditForm`.

```razor
<SCheckbox @bind-Value="_agree" Label="I agree to the terms" />
<SCheckbox @bind-Value="_news" Label="Send updates" Hint="Weekly, no spam" />
<SCheckbox Value="false" Indeterminate Label="Partially selected" />
<SCheckbox Value="true" Disabled Label="Locked on" />
```

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Value` / `ValueChanged` | `bool` | `false` | Supports `@bind-Value` |
| `Label` | `string?` | — | Or use `ChildContent` for rich labels |
| `Hint` | `string?` | — | |
| `Indeterminate` | `bool` | `false` | Visual only — does not affect `Value` |
| `Disabled` | `bool` | `false` | |

Inside an `EditForm`, `@bind-Value` wires up `ValueExpression` and the component calls `NotifyFieldChanged` so validation fires:

```csharp
[Range(typeof(bool), "true", "true", ErrorMessage = "You must agree")]
public bool Agree { get; set; }
```

---

### SSwitch

Same binding model as `SCheckbox`.

```razor
<SSwitch @bind-Value="_dark" Label="Dark mode" />
<SSwitch @bind-Value="_beta" Label="Beta features" Hint="May be unstable" />
<SSwitch Value="true" Disabled Label="Locked on" />
```

---

### SSelect&lt;TValue&gt;

Generic, searchable, multi-select capable, with async loading.

```razor
@* basic *@
<SSelect TValue="string" Items="_options" @bind-Value="_choice"
         Label="Option" Clearable />

@* custom display *@
<SSelect TValue="City" Items="_cities" @bind-Value="_city"
         Display="@(c => $"{c.Name} ({c.Country})")"
         Label="City" Searchable Clearable />

@* multiple — note @bind-Values, not @bind-Value *@
<SSelect TValue="City" Items="_cities" @bind-Values="_selectedCities"
         Display="@(c => c.Name)"
         Label="Cities" Multiple Searchable Clearable />

@* async source *@
<SSelect TValue="string" ItemsProvider="SearchAsync" @bind-Value="_remote"
         Label="Remote" Searchable />

@* custom item rendering *@
<SSelect TValue="City" Items="_cities" @bind-Value="_city" Display="@(c => c.Name)">
    <ItemTemplate Context="c">
        <span style="display:flex;flex-direction:column;">
            <strong>@c.Name</strong>
            <small style="opacity:.6;">@c.Country</small>
        </span>
    </ItemTemplate>
</SSelect>
```

```csharp
async Task<IEnumerable<string>> SearchAsync(string query, CancellationToken ct)
{
    var response = await Http.GetFromJsonAsync<string[]>($"/api/search?q={query}", ct);
    return response ?? Array.Empty<string>();
}
```

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Items` | `IEnumerable<TValue>?` | — | Static source |
| `ItemsProvider` | `Func<string, CancellationToken, Task<IEnumerable<TValue>>>?` | — | Async source; receives the query. Overrides `Items` |
| `Value` / `ValueChanged` | `TValue?` | — | Single mode. `@bind-Value` |
| `Values` / `ValuesChanged` | `IEnumerable<TValue>` | — | Multiple mode. `@bind-Values` |
| `Multiple` | `bool` | `false` | Renders chips in the trigger |
| `MaxChips` | `int` | `3` | Overflow collapses to `+N` |
| `Display` | `Func<TValue, string>?` | `ToString()` | Used in trigger, chips, and default items |
| `Filter` | `Func<TValue, string, bool>?` | contains, case-insensitive | Client-side only |
| `ItemTemplate` | `RenderFragment<TValue>?` | — | List rendering only; trigger still uses `Display` |
| `Comparer` | `IEqualityComparer<TValue>` | `Default` | |
| `Searchable` | `bool` | `false` | Adds a search box; autofocuses on open |
| `Clearable` | `bool` | `false` | |
| `DebounceMs` | `int` | `250` | Applies to `ItemsProvider` only |

Keyboard: `↓`/`↑` navigate, `Home`/`End` jump, `Enter` select, `Space` select (from trigger), `Esc`/`Tab` close.

**`ItemsProvider` does its own filtering** — the component does not re-filter the results. Return what you want shown.

**Use records or supply a `Comparer`.** With a plain `class`, the default `EqualityComparer` uses reference equality and the selected item won't highlight:

```csharp
record City(string Name, string Country);   // structural equality — works
```

**Pass materialized collections.** `Items="_x.Where(...)"` re-evaluates on every render.

**In `Multiple` mode, assign a new collection** in your handler rather than mutating the one you passed in.

---

### SDatePicker

Inherits `InputBase<DateTime?>`. Culture-aware — respects `CultureInfo.CurrentCulture` for month names, day names, and first day of week.

```razor
<SDatePicker @bind-Value="_date"
             Label="Date"
             Format="dd.MM.yyyy"
             Min="DateTime.Today.AddYears(-1)"
             Max="DateTime.Today.AddYears(1)" />
```

| Parameter | Type | Default |
|---|---|---|
| `Format` | `string` | `"dd.MM.yyyy"` |
| `Min` / `Max` | `DateTime?` | — |
| `Clearable` | `bool` | `true` |

---

### SPopover

The primitive behind `SSelect` and `SDatePicker`. Handles positioning, viewport flipping, outside-click and `Escape` dismissal.

```razor
<SPopover @ref="_pop">
    <Anchor>
        <SButton OnClick="() => _pop!.ToggleAsync()">Open</SButton>
    </Anchor>
    <ChildContent>
        <div style="padding:1rem;">Panel content</div>
    </ChildContent>
</SPopover>

@code {
    SPopover? _pop;
}
```

| Member | Notes |
|---|---|
| `OpenAsync()` / `CloseAsync()` / `ToggleAsync()` | |
| `IsOpen` | `bool` |
| `OpenChanged` | `EventCallback<bool>` |

---

### SIcon

```razor
<SIcon Name="@SIcons.Check" />
<SIcon Name="@SIcons.Calendar" Size="24" />
<SIcon Name="@SIcons.Spinner" Class="s-spin" />
```

Icons are compile-time constants — no runtime fetch, no bundle bloat.

Available: `ChevronDown`, `ChevronLeft`, `ChevronRight`, `Check`, `Minus`, `X`, `Calendar`, `Spinner`, `Loader`, `Search`, `Sun`, `Moon`.

Custom icons are just SVG path markup on a 24×24 viewBox:

```csharp
public static class MyIcons
{
    public const string Heart = "<path d='M19 14c1.5-1.5 3-3.5 3-5.5A5.5 5.5 0 0 0 12 5a5.5 5.5 0 0 0-10 3.5c0 2 1.5 4 3 5.5l7 7Z'/>";
}
```

---

### SThemeToggle

```razor
<SThemeToggle />
```

Reads the persisted theme on first render and applies it. Drop it in your layout.

---

## Theming

### Switching programmatically

```razor
@inject SollyThemeService Theme

<SButton OnClick="() => Theme.SetAsync("light")">Light</SButton>
<SButton OnClick="Theme.ToggleAsync">Toggle</SButton>

<p>Current: @Theme.Theme</p>
```

| Member | Notes |
|---|---|
| `Theme` | `string` — `"dark"`, `"light"`, or `"auto"` |
| `IsDark` | `bool` |
| `SetAsync(string)` | Persists to `localStorage` |
| `ToggleAsync()` | |
| `InitAsync()` | Reads stored theme. Call from `OnAfterRenderAsync(firstRender)` |
| `Changed` | `event Action?` |

The service is `Scoped` — on Blazor Server this keeps per-user state isolated. Subscribe to `Changed` if you need to react:

```csharp
protected override void OnInitialized() => Theme.Changed += OnChanged;
private void OnChanged() => InvokeAsync(StateHasChanged);
public void Dispose() => Theme.Changed -= OnChanged;
```

### Design tokens

Override any of these in your own stylesheet, loaded **after** `solly.css`:

```css
:root, [data-solly-theme="dark"] {
  --s-surface:      rgba(255, 255, 255, 0.08);
  --s-surface-hi:   rgba(255, 255, 255, 0.14);
  --s-border:       rgba(255, 255, 255, 0.18);
  --s-text:         rgba(255, 255, 255, 0.94);
  --s-text-dim:     rgba(255, 255, 255, 0.72);
  --s-accent:       #7c5cff;
  --s-danger:       #ff5c7c;
  --s-shadow:       0 8px 32px rgba(0, 0, 0, 0.35);
  --s-blur:         16px;
  --s-radius:       14px;
  --s-radius-sm:    9px;
  --s-dur:          180ms;
  --s-ease:         cubic-bezier(.2, .8, .2, 1);
  --s-font:         ui-sans-serif, system-ui, -apple-system, "Segoe UI", Roboto, sans-serif;
}

[data-solly-theme="light"] {
  --s-surface:    rgba(255, 255, 255, 0.5);
  --s-surface-hi: rgba(255, 255, 255, 0.75);
  --s-border:     rgba(255, 255, 255, 0.8);
  --s-text:       #16161f;
  --s-text-dim:   rgba(22, 22, 31, 0.62);
  --s-accent:     #6344e0;
  --s-danger:     #d92d54;
  --s-shadow:     0 8px 32px rgba(31, 38, 135, 0.14);
}
```

Note the separate `--s-accent` per theme: `#7c5cff` on a light background doesn't give enough contrast for white button text.

### Custom theme

```css
[data-solly-theme="ocean"] {
  --s-accent: #00b4d8;
  --s-surface: rgba(0, 119, 182, 0.12);
  /* ... */
}
```

```csharp
await Theme.SetAsync("ocean");
```

### Styling hooks

All classes are prefixed `s-`. Nothing is scoped, so you can override anything:

```css
.s-btn { border-radius: 999px; }
.s-select-trigger { min-width: 20rem; }
```

---

## Full example

```razor
@page "/"

<EditForm Model="_model" OnValidSubmit="SubmitAsync">
    <DataAnnotationsValidator />

    <div style="display:flex;flex-direction:column;gap:1.25rem;max-width:24rem;">
        <SInput @bind-Value="_model.Name" Label="Name" Required Clearable />

        <SSelect TValue="string" Items="_countries" @bind-Value="_model.Country"
                 Label="Country" Searchable Clearable />

        <SDatePicker @bind-Value="_model.Birthday" Label="Birthday"
                     Max="DateTime.Today" />

        <STextArea @bind-Value="_model.Bio" Label="Bio" MaxLength="200" />

        <SCheckbox @bind-Value="_model.Agree" Label="I agree to the terms" />

        <SButton Type="submit" Variant="SVariant.Primary" Icon="@SIcons.Check">
            Submit
        </SButton>
    </div>
</EditForm>

@code {
    class Model
    {
        [Required] public string? Name { get; set; }
        [Required] public string? Country { get; set; }
        public DateTime? Birthday { get; set; }
        [StringLength(200)] public string? Bio { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree")]
        public bool Agree { get; set; }
    }

    Model _model = new();
    string[] _countries = { "Germany", "France", "Netherlands", "Portugal", "Spain" };

    async Task SubmitAsync() { /* ... */ }
}
```

---

## Troubleshooting

| Symptom | Cause                                                                                                                        |
|---|------------------------------------------------------------------------------------------------------------------------------|
| Components render but clicks do nothing, console is empty | Static SSR. Add `@rendermode="InteractiveServer"` to `<Routes>`                                                              |
| Everything is unstyled / text is black on white | `solly.css` returned 404. Check `_content/Solly.UI/solly.css` in Network                                                     |
| Glass effect invisible | Flat background. `body` needs a gradient or image                                                                            |
| Green outline on inputs | Blazor template's `.valid.modified` in `app.css`. Delete it                                                                  |
| Popovers open but sit in the wrong place | `solly.js` failed to load. Check the Network tab                                                                             |
| Selected item not highlighted in `SSelect` | `TValue` is a `class` with reference equality. Use a `record` or pass a `Comparer`                                           |
| Theme flashes dark then light on load | The inline script in `<head>` is missing                                                                                     |
| `requires a value for the 'ValueExpression' parameter` | An `InputBase`-derived component (`SInput`, `STextArea`, `SDatePicker`) used with a literal `Value` instead of `@bind-Value` |

---

## Browser support

| Feature | Minimum |
|---|---|
| `backdrop-filter` | Chrome 76, Safari 9, Firefox 103 |
| `color-mix()` | Chrome 111, Safari 16.2, Firefox 113 |
| `:has()` | Chrome 105, Safari 15.4, Firefox 121 |
| `@property` | Chrome 85, Safari 16.4, Firefox 128 |

Blur is expensive. `--s-blur: 16px` on a hundred simultaneous elements will drop frames on low-end hardware — lower it or drop `.s-glass` for list items.

`prefers-reduced-motion` is respected automatically.

---

## Building from source

```bash
git clone https://github.com/13cyberpunk02/Solly.UI
cd Solly.UI
dotnet build
dotnet run --project Solly.Demo
```

Keep both a Server and a WebAssembly demo referencing the library — most interop and prerendering bugs only show up as a discrepancy between the two.

```bash
dotnet pack Solly.UI -c Release
dotnet nuget push Solly.UI/bin/Release/Solly.UI.0.1.0.nupkg -k $NUGET_KEY -s https://api.nuget.org/v3/index.json
```

---

## Design notes

**Why global CSS instead of scoped?** Scoped CSS binds selectors to the owning component's generated attribute. A shared class like `.s-field` declared in `SInput.razor.css` silently does nothing inside `SSelect`, and every nested component needs `::deep`. With a `s-` prefix, collisions aren't a real risk, and consumers can override anything without fighting specificity. This is what MudBlazor and Radzen do too.

**Why don't `SCheckbox` and `SSwitch` inherit `InputBase`?** `InputBase` throws without a `ValueExpression`, which only `@bind-Value` provides. Checkboxes and switches are constantly used outside forms — filters, toolbars, settings panels. They implement the binding contract manually and cascade `EditContext` only when present. `SInput`, `STextArea`, and `SDatePicker` do inherit `InputBase`, because string parsing and validation are exactly what it's good at.

---

## License

MIT