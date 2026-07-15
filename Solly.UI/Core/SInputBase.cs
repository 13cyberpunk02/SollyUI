using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Solly.UI.Core;

public abstract class SInputBase<TValue> : InputBase<TValue>
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool Disabled { get; set; }

    protected string? ValidationClass => EditContext?.FieldCssClass(FieldIdentifier);
}