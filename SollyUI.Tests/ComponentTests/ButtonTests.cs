using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Solly.UI.Components;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class ButtonTests : SollyTestContext
{
    [Fact]
    public void Renders_child_content()
    {
        var cut = Render<SButton>(p => p
            .AddChildContent("Click me"));

        cut.Find("button").TextContent.Should().Contain("Click me");
    }

    [Fact]
    public void Applies_variant_class()
    {
        var cut = Render<SButton>(p => p
            .Add(x => x.Variant, SVariant.Primary));

        cut.Find("button").ClassList.Should().Contain("s-v-primary");
    }

    [Fact]
    public void Disabled_button_has_attribute()
    {
        var cut = Render<SButton>(p => p
            .Add(x => x.Disabled, true));

        cut.Find("button").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task Invokes_OnClick()
    {
        var clicked = false;
        var cut = Render<SButton>(p => p
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true)));

        await cut.Find("button").ClickAsync(new MouseEventArgs());

        clicked.Should().BeTrue();
    }

    [Fact]
    public async Task Disabled_button_does_not_invoke_OnClick()
    {
        var clicked = false;
        var cut = Render<SButton>(p => p
            .Add(x => x.Disabled, true)
            .Add(x => x.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true)));

        // disabled button: the handler guards on Disabled internally
        await cut.Find("button").ClickAsync(new MouseEventArgs());

        clicked.Should().BeFalse();
    }

    [Fact]
    public void Renders_icon_when_provided()
    {
        var cut = Render<SButton>(p => p
            .Add(x => x.Icon, Solly.UI.Icons.SIcons.Check));

        cut.FindAll("svg").Should().NotBeEmpty();
    }
}