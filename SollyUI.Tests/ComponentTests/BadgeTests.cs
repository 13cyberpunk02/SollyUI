using Bunit;
using FluentAssertions;
using Solly.UI.Components;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class BadgeTests : SollyTestContext
{
    [Fact]
    public void Renders_child_content()
    {
        var cut = RenderC<SBadge>(p => p.AddChildContent("Live"));
        cut.Find(".s-badge").TextContent.Should().Contain("Live");
    }

    [Fact]
    public void Default_level_has_no_extra_class()
    {
        var cut = RenderC<SBadge>(p => p.AddChildContent("x"));
        var cls = cut.Find(".s-badge").ClassList;
        cls.Should().NotContain("s-badge-success");
        cls.Should().NotContain("s-badge-neutral");
    }

    [Theory]
    [InlineData(SBadgeLevel.Success, "s-badge-success")]
    [InlineData(SBadgeLevel.Warning, "s-badge-warning")]
    [InlineData(SBadgeLevel.Error, "s-badge-error")]
    [InlineData(SBadgeLevel.Info, "s-badge-info")]
    [InlineData(SBadgeLevel.Neutral, "s-badge-neutral")]
    public void Applies_level_class(SBadgeLevel level, string expected)
    {
        var cut = RenderC<SBadge>(p => p
            .Add(x => x.Level, level)
            .AddChildContent("x"));
        cut.Find(".s-badge").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void No_pulse_dot_by_default()
    {
        var cut = RenderC<SBadge>(p => p.AddChildContent("x"));
        cut.FindAll(".s-badge-dot").Should().BeEmpty();
    }

    [Fact]
    public void Pulse_renders_dot()
    {
        var cut = RenderC<SBadge>(p => p
            .Add(x => x.Pulse, true)
            .AddChildContent("x"));
        cut.FindAll(".s-badge-dot").Should().HaveCount(1);
    }

    [Fact]
    public void Mono_applies_class()
    {
        var cut = RenderC<SBadge>(p => p
            .Add(x => x.Mono, true)
            .AddChildContent("v1.0"));
        cut.Find(".s-badge").ClassList.Should().Contain("s-badge-mono");
    }
}