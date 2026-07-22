using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class PopoverTests : SollyTestContext
{
     [Fact]
    public void Closed_by_default()
    {
        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>")));

        cut.FindAll("#panel").Should().BeEmpty();
    }

    [Fact]
    public async Task OpenAsync_renders_the_panel()
    {
        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>")));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());

        cut.FindAll("#panel").Should().HaveCount(1);
        cut.Instance.IsOpen.Should().BeTrue();
    }

    [Fact]
    public async Task CloseAsync_removes_the_panel()
    {
        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>")));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());
        await cut.InvokeAsync(() => cut.Instance.CloseAsync());

        cut.FindAll("#panel").Should().BeEmpty();
        cut.Instance.IsOpen.Should().BeFalse();
    }

    [Fact]
    public async Task ToggleAsync_flips_state()
    {
        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>")));

        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());
        cut.Instance.IsOpen.Should().BeTrue();

        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());
        cut.Instance.IsOpen.Should().BeFalse();
    }

    [Fact]
    public async Task Raises_OpenChanged()
    {
        var states = new List<bool>();

        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>"))
            .Add(x => x.OpenChanged, EventCallback.Factory.Create<bool>(this, s => states.Add(s))));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());
        await cut.InvokeAsync(() => cut.Instance.CloseAsync());

        states.Should().Equal(true, false);
    }

    [Fact]
    public async Task OnDismissAsync_closes()
    {
        var cut = RenderC<SPopover>(p => p
            .Add(x => x.Anchor, b => b.AddMarkupContent(0, "<button>open</button>"))
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<div id='panel'>hi</div>")));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());
        await cut.InvokeAsync(() => cut.Instance.OnDismissAsync());

        cut.Instance.IsOpen.Should().BeFalse();
    }
}