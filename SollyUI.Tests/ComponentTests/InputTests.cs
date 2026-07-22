using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class InputTests : SollyTestContext
{
    [Fact]
    public void Renders_label()
    {
        string? value = "";
        var cut = Render((Action<ComponentParameterCollectionBuilder<SInput>>)(p => p
            .Add(x => x.Label, "Name")
            .Bind(x => x.Value, value, v => value = v!, () => value)));

        cut.Find("label").TextContent.Should().Contain("Name");
    }

    [Fact]
    public void Two_way_binding_updates_value()
    {
        var value = "initial";

        var cut = Render<SInput>(p => p
            .Bind(x => x.Value, value, v => value = v!, () => value));

        cut.Find("input").Input("changed");

        value.Should().Be("changed");
    }

    [Fact]
    public void Reflects_incoming_value()
    {
        string? value = "hello";
        var cut = Render((Action<ComponentParameterCollectionBuilder<SInput>>)(p => p
            .Bind(x => x.Value, "hello", _ => { }, () => "hello")));

        cut.Find("input").GetAttribute("value").Should().Be("hello");
    }

    [Fact]
    public void Clearable_shows_clear_button_when_filled()
    {
        var value = "text";   
        var cut = Render((Action<ComponentParameterCollectionBuilder<SInput>>)(p => p
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value)));
        cut.FindAll(".s-input-clear").Should().NotBeEmpty();
    }

    [Fact]
    public void Clear_button_empties_value()
    {
        var value = "text";
        var cut = Render<SInput>(p => p
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-input-clear").Click();

        value.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Disabled_input_has_attribute()
    {
        string? value = "";
        var cut = Render((Action<ComponentParameterCollectionBuilder<SInput>>)(p => p
            .Add(x => x.Disabled, true)
            .Bind(x => x.Value, "", _ => { }, () => "")));

        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }
}