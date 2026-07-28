using System.Globalization;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class ProgressTests : SollyTestContext
{
    [Fact]
    public void Renders_linear_bar()
    {
        var cut = RenderC<SProgress>(p => p.Add(x => x.Value, 50));
        cut.FindAll(".s-progress-bar").Should().HaveCount(1);
    }

    [Fact]
    public void Bar_width_matches_percent()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 30)
            .Add(x => x.Max, 100));
        cut.Find(".s-progress-bar").GetAttribute("style").Should().Contain("width:30%");
    }

    [Fact]
    public void Percent_respects_max()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 5)
            .Add(x => x.Max, 10));
        cut.Find(".s-progress-bar").GetAttribute("style").Should().Contain("width:50%");
    }

    [Fact]
    public void Clamps_above_max()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 150)
            .Add(x => x.Max, 100));
        cut.Find(".s-progress-bar").GetAttribute("style").Should().Contain("width:100%");
    }

    [Fact]
    public void Percent_uses_invariant_culture()
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");
            var cut = RenderC<SProgress>(p => p
                .Add(x => x.Value, 1)
                .Add(x => x.Max, 3));
            // 33.33% — comma would break the inline style
            cut.Find(".s-progress-bar").GetAttribute("style").Should().NotContain(",");
        }
        finally { CultureInfo.CurrentCulture = original; }
    }

    [Fact]
    public void ShowValue_renders_percentage()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 42)
            .Add(x => x.ShowValue, true));
        cut.Find(".s-progress-val").TextContent.Should().Contain("42%");
    }

    [Fact]
    public void ShowValue_false_hides_value()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 42)
            .Add(x => x.Label, "X"));
        cut.FindAll(".s-progress-val").Should().BeEmpty();
    }

    [Fact]
    public void Renders_label()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 10)
            .Add(x => x.Label, "Uploading"));
        cut.Find(".s-progress-label").TextContent.Should().Contain("Uploading");
    }

    [Fact]
    public void Format_overrides_display()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 3)
            .Add(x => x.ShowValue, true)
            .Add(x => x.Format, v => $"{v} GB"));
        cut.Find(".s-progress-val").TextContent.Should().Contain("3 GB");
    }

    [Fact]
    public void Indeterminate_applies_class()
    {
        var cut = RenderC<SProgress>(p => p.Add(x => x.Indeterminate, true));
        cut.Find(".s-progress").ClassList.Should().Contain("s-indet");
    }

    [Theory]
    [InlineData(SAlertLevel.Success, "s-progress-success")]
    [InlineData(SAlertLevel.Warning, "s-progress-warning")]
    [InlineData(SAlertLevel.Error, "s-progress-error")]
    public void Level_applies_class(SAlertLevel level, string expected)
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Value, 50)
            .Add(x => x.Level, level));
        cut.Find(".s-progress").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Circular_renders_ring()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Circular, true)
            .Add(x => x.Value, 50));
        cut.FindAll(".s-progress-ring").Should().HaveCount(1);
        cut.FindAll(".s-ring-fill").Should().HaveCount(1);
    }

    [Fact]
    public void Circular_show_value()
    {
        var cut = RenderC<SProgress>(p => p
            .Add(x => x.Circular, true)
            .Add(x => x.Value, 75)
            .Add(x => x.ShowValue, true));
        cut.Find(".s-ring-label").TextContent.Should().Contain("75%");
    }

    [Fact]
    public void Ring_dashoffset_uses_invariant_culture()
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");
            var cut = RenderC<SProgress>(p => p
                .Add(x => x.Circular, true)
                .Add(x => x.Value, 33));
            cut.Find(".s-ring-fill").GetAttribute("style").Should().NotContain(",");
        }
        finally { CultureInfo.CurrentCulture = original; }
    }

    [Fact]
    public void Has_progressbar_role()
    {
        var cut = RenderC<SProgress>(p => p.Add(x => x.Value, 40));
        cut.Find("[role=progressbar]").Should().NotBeNull();
    }
}