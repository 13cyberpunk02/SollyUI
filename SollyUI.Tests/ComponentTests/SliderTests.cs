using System.Globalization;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class SliderTests : SollyTestContext
{
    [Fact]
    public void Renders_range_input()
    {
        var cut = RenderC<SSlider>(p => p.Add(x => x.Value, 50d));

        cut.Find("input[type=range]").Should().NotBeNull();
    }

    [Fact]
    public void Reflects_min_max_step()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Min, 10d)
            .Add(x => x.Max, 20d)
            .Add(x => x.Step, 0.5d)
            .Add(x => x.Value, 15d));

        var input = cut.Find("input[type=range]");
        input.GetAttribute("min").Should().Be("10");
        input.GetAttribute("max").Should().Be("20");
        input.GetAttribute("step").Should().Be("0.5");
    }

    [Fact]
    public void Value_uses_invariant_culture()
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            var cut = RenderC<SSlider>(p => p
                .Add(x => x.Min, 0d)
                .Add(x => x.Max, 100d)
                .Add(x => x.Step, 0.5d)
                .Add(x => x.Value, 22.5d));

            // must be "22.5", not "22,5" — a comma breaks the native range input
            cut.Find("input[type=range]").GetAttribute("value").Should().Be("22.5");
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void Percent_css_variable_uses_invariant_culture()
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            var cut = RenderC<SSlider>(p => p
                .Add(x => x.Min, 0d)
                .Add(x => x.Max, 3d)
                .Add(x => x.Value, 1d));

            // 33.333% — a comma here silently invalidates the CSS
            var style = cut.Find(".s-slider").GetAttribute("style");
            style.Should().Contain("--s-pct:");
            style.Should().NotContain(",");
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void ShowValue_renders_number()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Value, 42d)
            .Add(x => x.ShowValue, true));

        cut.Find(".s-slider-num").TextContent.Should().Contain("42");
    }

    [Fact]
    public void ShowValue_false_hides_number()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Value, 42d)
            .Add(x => x.ShowValue, false)
            .Add(x => x.Label, "Volume"));

        cut.FindAll(".s-slider-num").Should().BeEmpty();
    }

    [Fact]
    public void Format_function_is_used()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Value, 22d)
            .Add(x => x.Format, v => $"{v:0} °C"));

        cut.Find(".s-slider-num").TextContent.Should().Contain("22 °C");
    }

    [Fact]
    public void Ticks_are_rendered()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Min, 1d)
            .Add(x => x.Max, 5d)
            .Add(x => x.Ticks, [1, 2, 3, 4, 5]));

        cut.FindAll(".s-slider-tick").Should().HaveCount(5);
    }

    [Fact]
    public void Disabled_has_attribute()
    {
        var cut = RenderC<SSlider>(p => p
            .Add(x => x.Disabled, true));

        cut.Find("input[type=range]").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Literal_value_does_not_throw()
    {
        var ex = Record.Exception(() =>
            RenderC<SSlider>(p => p.Add(x => x.Value, 40d)));

        ex.Should().BeNull();
    }
}