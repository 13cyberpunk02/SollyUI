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
        var cut = Render<SCheckbox>(p => p
            .Add(x => x.Value, true));

        cut.Find("input[type=checkbox]")
            .IsChecked().Should().BeTrue();
    }

    [Fact]
    public void Toggling_invokes_ValueChanged()
    {
        var value = false;
        var cut = Render<SCheckbox>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input[type=checkbox]").Change(true);

        value.Should().BeTrue();
    }

    [Fact]
    public void Works_without_EditForm()
    {
        var ex = Record.Exception(() =>
            Render<SCheckbox>(p => p
                .Add(x => x.Value, true)
                .Add(x => x.Label, "Static")));

        ex.Should().BeNull();
    }

    [Fact]
    public void Disabled_blocks_interaction()
    {
        var cut = Render<SCheckbox>(p => p
            .Add(x => x.Disabled, true)
            .Add(x => x.Value, false));

        cut.Find("input[type=checkbox]").HasAttribute("disabled").Should().BeTrue();
    }
}

public class SwitchTests : SollyTestContext
{
    [Fact]
    public void Toggling_invokes_ValueChanged()
    {
        var value = false;
        var cut = Render<SSwitch>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input[type=checkbox]").Change(true);

        value.Should().BeTrue();
    }

    [Fact]
    public void Works_without_EditForm()
    {
        var ex = Record.Exception(() =>
            Render<SSwitch>(p => p.Add(x => x.Value, true)));

        ex.Should().BeNull();
    }
}