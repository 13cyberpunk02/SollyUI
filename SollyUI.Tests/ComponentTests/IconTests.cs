using System.Reflection;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;
using Solly.UI.Icons;

namespace SollyUI.Tests.ComponentTests;

public class IconTests : SollyTestContext
{
    [Fact]
    public void Renders_svg()
    {
        var cut = RenderC<SIcon>(p => p
            .Add(x => x.Name, SIcons.Check));

        cut.Find("svg").Should().NotBeNull();
    }

    [Fact]
    public void Default_size_is_18()
    {
        var cut = RenderC<SIcon>(p => p
            .Add(x => x.Name, SIcons.Check));

        var svg = cut.Find("svg");
        svg.GetAttribute("width").Should().Be("18");
        svg.GetAttribute("height").Should().Be("18");
    }

    [Fact]
    public void Size_can_be_set()
    {
        var cut = RenderC<SIcon>(p => p
            .Add(x => x.Name, SIcons.Check)
            .Add(x => x.Size, 32));

        cut.Find("svg").GetAttribute("width").Should().Be("32");
    }

    [Fact]
    public void Uses_currentColor()
    {
        var cut = RenderC<SIcon>(p => p
            .Add(x => x.Name, SIcons.Check));

        cut.Find("svg").GetAttribute("stroke").Should().Be("currentColor");
    }

    [Fact]
    public void Is_hidden_from_assistive_tech()
    {
        var cut = RenderC<SIcon>(p => p
            .Add(x => x.Name, SIcons.Check));

        cut.Find("svg").GetAttribute("aria-hidden").Should().Be("true");
    }

    [Fact]
    public void All_icon_constants_render_without_throwing()
    {
        var icons = typeof(SIcons)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToArray();

        icons.Should().NotBeEmpty();

        foreach (var path in icons)
        {
            var ex = Record.Exception(() =>
                RenderC<SIcon>(p => p.Add(x => x.Name, path)));

            ex.Should().BeNull();
        }
    }
}