using Bunit;
using FluentAssertions;
using Solly.UI.Components;
using Solly.UI.Icons;

namespace SollyUI.Tests.ComponentTests;

public class InputTests : SollyTestContext
{
   [Fact]
    public void Renders_label()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Label, "Name")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("label").TextContent.Should().Contain("Name");
    }

    [Fact]
    public void Required_renders_asterisk()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Label, "Name")
            .Add(x => x.Required, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-req").TextContent.Should().Contain("*");
    }

    [Fact]
    public void Two_way_binding_updates_value()
    {
        string? value = "initial";

        var cut = RenderC<SInput>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").Input("changed");

        value.Should().Be("changed");
    }

    [Fact]
    public void Reflects_incoming_value()
    {
        string? value = "hello";

        var cut = RenderC<SInput>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").GetAttribute("value").Should().Be("hello");
    }

    [Fact]
    public void Renders_placeholder()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Placeholder, "Jane Doe")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").GetAttribute("placeholder").Should().Be("Jane Doe");
    }

    [Fact]
    public void Renders_hint()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Hint, "Your full name")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-hint").TextContent.Should().Contain("Your full name");
    }

    [Fact]
    public void Type_defaults_to_text()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").GetAttribute("type").Should().Be("text");
    }

    [Fact]
    public void Type_can_be_password()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Type, "password")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").GetAttribute("type").Should().Be("password");
    }

    [Fact]
    public void Clearable_shows_clear_button_when_filled()
    {
        string? value = "text";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.FindAll(".s-input-clear").Should().NotBeEmpty();
    }

    [Fact]
    public void Clearable_hides_clear_button_when_empty()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.FindAll(".s-input-clear").Should().BeEmpty();
    }

    [Fact]
    public void Clear_button_empties_value()
    {
        string? value = "text";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-input-clear").Click();

        value.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Disabled_input_has_attribute()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Disabled, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Disabled_hides_clear_button()
    {
        string? value = "text";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Clearable, true)
            .Add(x => x.Disabled, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.FindAll(".s-input-clear").Should().BeEmpty();
    }

    [Fact]
    public void Renders_icon_when_provided()
    {
        string? value = "";

        var cut = RenderC<SInput>(p => p
            .Add(x => x.Icon, SIcons.Search)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.FindAll("svg").Should().NotBeEmpty();
    }
}