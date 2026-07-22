using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class TextAreaTests : SollyTestContext
{
    [Fact]
    public void Two_way_binding_updates_value()
    {
        string? value = "";

        var cut = RenderC<STextArea>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("textarea").Input("hello world");

        value.Should().Be("hello world");
    }

    [Fact]
    public void Rows_defaults_to_four()
    {
        string? value = "";

        var cut = RenderC<STextArea>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("textarea").GetAttribute("rows").Should().Be("4");
    }

    [Fact]
    public void Rows_can_be_set()
    {
        string? value = "";

        var cut = RenderC<STextArea>(p => p
            .Add(x => x.Rows, 8)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("textarea").GetAttribute("rows").Should().Be("8");
    }

    [Fact]
    public void MaxLength_renders_counter()
    {
        string? value = "abc";

        var cut = RenderC<STextArea>(p => p
            .Add(x => x.MaxLength, 200)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-ta-count").TextContent.Should().Contain("3");
        cut.Find(".s-ta-count").TextContent.Should().Contain("200");
    }

    [Fact]
    public void No_MaxLength_no_counter()
    {
        string? value = "abc";

        var cut = RenderC<STextArea>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.FindAll(".s-ta-count").Should().BeEmpty();
    }

    [Fact]
    public void Disabled_has_attribute()
    {
        string? value = "";

        var cut = RenderC<STextArea>(p => p
            .Add(x => x.Disabled, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("textarea").HasAttribute("disabled").Should().BeTrue();
    }
}