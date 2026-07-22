using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Solly.UI.Components;
using Solly.UI.Core;
using Solly.UI.Icons;

namespace SollyUI.Tests.ComponentTests;

public class ButtonTests : SollyTestContext
{
   [Fact]
    public void Renders_child_content()
    {
        var cut = RenderC<SButton>(p => p
            .AddChildContent("Click me"));

        cut.Find("button").TextContent.Should().Contain("Click me");
    }

    [Fact]
    public void Applies_default_variant_class()
    {
        var cut = RenderC<SButton>(p => p
            .AddChildContent("x"));

        cut.Find("button").ClassList.Should().Contain("s-v-default");
    }

    [Theory]
    [InlineData(SVariant.Primary, "s-v-primary")]
    [InlineData(SVariant.Danger, "s-v-danger")]
    [InlineData(SVariant.Ghost, "s-v-ghost")]
    public void Applies_variant_class(SVariant variant, string expected)
    {
        var cut = RenderC<SButton>(p => p
            .Add(x => x.Variant, variant)
            .AddChildContent("x"));

        cut.Find("button").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Has_glass_class()
    {
        var cut = RenderC<SButton>(p => p.AddChildContent("x"));

        cut.Find("button").ClassList.Should().Contain("s-glass");
    }

    [Fact]
    public void Disabled_button_has_attribute()
    {
        var cut = RenderC<SButton>(p => p
            .Add(x => x.Disabled, true)
            .AddChildContent("x"));

        cut.Find("button").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Type_defaults_to_button()
    {
        var cut = RenderC<SButton>(p => p.AddChildContent("x"));

        cut.Find("button").GetAttribute("type").Should().Be("button");
    }

    [Fact]
    public void Type_can_be_submit()
    {
        var cut = RenderC<SButton>(p => p
            .Add(x => x.Type, "submit")
            .AddChildContent("x"));

        cut.Find("button").GetAttribute("type").Should().Be("submit");
    }

    [Fact]
    public void Invokes_OnClick()
    {
        var clicked = false;

        var cut = RenderC<SButton>(p => p
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true))
            .AddChildContent("x"));

        cut.Find("button").Click();

        clicked.Should().BeTrue();
    }

    [Fact]
    public void Disabled_button_does_not_invoke_OnClick()
    {
        var clicked = false;

        var cut = RenderC<SButton>(p => p
            .Add(x => x.Disabled, true)
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true))
            .AddChildContent("x"));

        cut.Find("button").Click();

        clicked.Should().BeFalse();
    }

    [Fact]
    public void Renders_icon_when_provided()
    {
        var cut = RenderC<SButton>(p => p
            .Add(x => x.Icon, SIcons.Check)
            .AddChildContent("Save"));

        cut.FindAll("svg").Should().NotBeEmpty();
    }

    [Fact]
    public void Renders_no_icon_by_default()
    {
        var cut = RenderC<SButton>(p => p.AddChildContent("x"));

        cut.FindAll("svg").Should().BeEmpty();
    }

    [Fact]
    public void Forwards_unmatched_attributes()
    {
        var cut = RenderC<SButton>(p => p
            .AddUnmatched("data-testid", "save-btn")
            .AddChildContent("x"));

        cut.Find("button").GetAttribute("data-testid").Should().Be("save-btn");
    }

    [Fact]
    public void Merges_custom_class()
    {
        var cut = RenderC<SButton>(p => p
            .Add(x => x.Class, "my-custom")
            .AddChildContent("x"));

        cut.Find("button").ClassList.Should().Contain("my-custom");
        cut.Find("button").ClassList.Should().Contain("s-btn");
    }
}