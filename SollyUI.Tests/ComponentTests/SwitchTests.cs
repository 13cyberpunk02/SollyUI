using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class SwitchTests :SollyTestContext
{
    [Fact]
    public void Reflects_value_as_checked()
    {
        var cut = RenderC<SSwitch>(p => p
            .Add(x => x.Value, true));

        cut.Find("input[type=checkbox]").IsChecked().Should().BeTrue();
    }

    [Fact]
    public void Toggling_invokes_ValueChanged()
    {
        var value = false;

        var cut = RenderC<SSwitch>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input[type=checkbox]").Change(true);

        value.Should().BeTrue();
    }

    [Fact]
    public void Literal_value_does_not_throw()
    {
        var ex = Record.Exception(() =>
            RenderC<SSwitch>(p => p.Add(x => x.Value, true)));

        ex.Should().BeNull();
    }

    [Fact]
    public void Renders_label()
    {
        var cut = RenderC<SSwitch>(p => p
            .Add(x => x.Label, "Dark mode"));

        cut.Find(".s-sw-label").TextContent.Should().Contain("Dark mode");
    }

    [Fact]
    public void Disabled_has_attribute()
    {
        var cut = RenderC<SSwitch>(p => p
            .Add(x => x.Disabled, true));

        cut.Find("input[type=checkbox]").HasAttribute("disabled").Should().BeTrue();
    }
}