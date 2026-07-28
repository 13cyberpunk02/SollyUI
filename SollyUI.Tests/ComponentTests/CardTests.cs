using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Solly.UI.Components;
using Solly.UI.Core;
using Solly.UI.Icons;

namespace SollyUI.Tests.ComponentTests;

public class CardTests : SollyTestContext
{
    [Fact]
    public void Renders_child_content()
    {
        var cut = RenderC<SCard>(p => p.AddChildContent("body text"));
        cut.Find(".s-card").TextContent.Should().Contain("body text");
    }

    [Fact]
    public void Renders_title()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Title, "Revenue")
            .AddChildContent("x"));
        cut.Find(".s-card-title").TextContent.Should().Contain("Revenue");
    }

    [Fact]
    public void Renders_subtitle()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Title, "T")
            .Add(x => x.Subtitle, "last 30 days")
            .AddChildContent("x"));
        cut.Find(".s-card-sub").TextContent.Should().Contain("last 30 days");
    }

    [Fact]
    public void Renders_eyebrow()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Eyebrow, "DEPLOY")
            .AddChildContent("x"));
        cut.Find(".s-eyebrow").TextContent.Should().Contain("DEPLOY");
    }

    [Fact]
    public void Renders_icon()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Icon, SIcons.Star)
            .Add(x => x.Title, "T")
            .AddChildContent("x"));
        cut.FindAll(".s-card-icon").Should().HaveCount(1);
    }

    [Fact]
    public void Default_variant_class()
    {
        var cut = RenderC<SCard>(p => p.AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().Contain("s-card-default");
    }

    [Theory]
    [InlineData(SCardVariant.Neon, "s-card-neon")]
    [InlineData(SCardVariant.Flat, "s-card-flat")]
    [InlineData(SCardVariant.Outline, "s-card-outline")]
    public void Applies_variant_class(SCardVariant variant, string expected)
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Variant, variant)
            .AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Lens_class_by_default()
    {
        var cut = RenderC<SCard>(p => p.AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().Contain("s-card-lens");
    }

    [Fact]
    public void Lens_false_removes_class()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Lens, false)
            .AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().NotContain("s-card-lens");
    }

    [Fact]
    public void Renders_as_div_by_default()
    {
        var cut = RenderC<SCard>(p => p.AddChildContent("x"));
        cut.Find(".s-card").TagName.Should().Be("DIV");
    }

    [Fact]
    public void Href_renders_as_anchor()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Href, "/button")
            .AddChildContent("x"));
        var el = cut.Find(".s-card");
        el.TagName.Should().Be("A");
        el.GetAttribute("href").Should().Be("/button");
    }

    [Fact]
    public void Href_is_interactive()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Href, "/x")
            .AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().Contain("s-card-interactive");
    }

    [Fact]
    public void OnClick_makes_it_a_button_role()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.OnClick, EventCallback.Factory.Create(this, () => { }))
            .AddChildContent("x"));
        cut.Find(".s-card").GetAttribute("role").Should().Be("button");
    }

    [Fact]
    public void OnClick_fires()
    {
        var clicked = false;
        var cut = RenderC<SCard>(p => p
            .Add(x => x.OnClick, EventCallback.Factory.Create(this, () => clicked = true))
            .AddChildContent("x"));

        cut.Find(".s-card").Click();
        clicked.Should().BeTrue();
    }

    [Fact]
    public void Renders_footer_content()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.FooterContent, b => b.AddMarkupContent(0, "<span id='f'>foot</span>"))
            .AddChildContent("x"));
        cut.FindAll("#f").Should().HaveCount(1);
    }

    [Fact]
    public void Renders_actions_content()
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Title, "T")
            .Add(x => x.ActionsContent, b => b.AddMarkupContent(0, "<span id='a'>act</span>"))
            .AddChildContent("x"));
        cut.FindAll("#a").Should().HaveCount(1);
    }

    [Theory]
    [InlineData(SCardPadding.Compact, "s-card-p-sm")]
    [InlineData(SCardPadding.Normal, "s-card-p-md")]
    [InlineData(SCardPadding.Roomy, "s-card-p-lg")]
    [InlineData(SCardPadding.None, "s-card-p-none")]
    public void Applies_padding_class(SCardPadding pad, string expected)
    {
        var cut = RenderC<SCard>(p => p
            .Add(x => x.Padding, pad)
            .AddChildContent("x"));
        cut.Find(".s-card").ClassList.Should().Contain(expected);
    }
}