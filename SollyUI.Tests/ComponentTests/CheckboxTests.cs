using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class CheckboxTests : SollyTestContext
{
     [Fact]
    public void Reflects_value_as_checked()
    {
        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Value, true));

        cut.Find("input[type=checkbox]").IsChecked().Should().BeTrue();
    }

    [Fact]
    public void Unchecked_by_default()
    {
        var cut = RenderC<SCheckbox>();

        cut.Find("input[type=checkbox]").IsChecked().Should().BeFalse();
    }

    [Fact]
    public void Toggling_invokes_ValueChanged()
    {
        var value = false;

        var cut = RenderC<SCheckbox>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input[type=checkbox]").Change(true);

        value.Should().BeTrue();
    }

    [Fact]
    public void Literal_value_does_not_throw()
    {
        var ex = Record.Exception(() =>
            RenderC<SCheckbox>(p => p
                .Add(x => x.Value, true)
                .Add(x => x.Label, "Static")));

        ex.Should().BeNull();
    }

    [Fact]
    public void Renders_label()
    {
        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Label, "I agree"));

        cut.Find(".s-cb-label").TextContent.Should().Contain("I agree");
    }

    [Fact]
    public void Renders_hint()
    {
        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Label, "Updates")
            .Add(x => x.Hint, "Weekly, no spam"));

        cut.Find(".s-hint").TextContent.Should().Contain("Weekly, no spam");
    }

    [Fact]
    public void Disabled_has_attribute()
    {
        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Disabled, true));

        cut.Find("input[type=checkbox]").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Indeterminate_applies_class()
    {
        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Indeterminate, true));

        cut.Find(".s-cb-box").ClassList.Should().Contain("s-indet");
    }

    [Fact]
    public void Indeterminate_does_not_change_value()
    {
        var value = false;

        var cut = RenderC<SCheckbox>(p => p
            .Add(x => x.Indeterminate, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        value.Should().BeFalse();
    }
}