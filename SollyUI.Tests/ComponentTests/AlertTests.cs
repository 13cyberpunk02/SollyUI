using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Solly.UI.Components;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class AlertTests : SollyTestContext
{
    [Fact]
    public void Renders_child_content()
    {
        var cut = RenderC<SAlert>(p => p.AddChildContent("Something happened"));
        cut.Find(".s-alert").TextContent.Should().Contain("Something happened");
    }

    [Fact]
    public void Default_level_is_info()
    {
        var cut = RenderC<SAlert>(p => p.AddChildContent("x"));
        cut.Find(".s-alert").ClassList.Should().Contain("s-alert-info");
    }

    [Theory]
    [InlineData(SAlertLevel.Success, "s-alert-success")]
    [InlineData(SAlertLevel.Warning, "s-alert-warning")]
    [InlineData(SAlertLevel.Error, "s-alert-error")]
    [InlineData(SAlertLevel.Info, "s-alert-info")]
    public void Applies_level_class(SAlertLevel level, string expected)
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Level, level)
            .AddChildContent("x"));
        cut.Find(".s-alert").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Renders_title()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Title, "Heads up")
            .AddChildContent("body"));
        cut.Find(".s-alert-title").TextContent.Should().Contain("Heads up");
    }

    [Fact]
    public void No_title_no_title_element()
    {
        var cut = RenderC<SAlert>(p => p.AddChildContent("body"));
        cut.FindAll(".s-alert-title").Should().BeEmpty();
    }

    [Fact]
    public void Shows_icon_by_default()
    {
        var cut = RenderC<SAlert>(p => p.AddChildContent("x"));
        cut.FindAll(".s-alert-icon").Should().NotBeEmpty();
    }

    [Fact]
    public void ShowIcon_false_hides_icon()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.ShowIcon, false)
            .AddChildContent("x"));
        cut.FindAll(".s-alert-icon").Should().BeEmpty();
    }

    [Fact]
    public void Soft_applies_class()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Soft, true)
            .AddChildContent("x"));
        cut.Find(".s-alert").ClassList.Should().Contain("s-alert-soft");
    }

    [Fact]
    public void Not_dismissible_by_default()
    {
        var cut = RenderC<SAlert>(p => p.AddChildContent("x"));
        cut.FindAll(".s-alert-x").Should().BeEmpty();
    }

    [Fact]
    public void Dismissible_renders_close_button()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Dismissible, true)
            .AddChildContent("x"));
        cut.FindAll(".s-alert-x").Should().HaveCount(1);
    }

    [Fact]
    public void Dismiss_removes_alert_and_raises_callback()
    {
        var dismissed = false;
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Dismissible, true)
            .Add(x => x.OnDismiss, EventCallback.Factory.Create(this, () => dismissed = true))
            .AddChildContent("x"));

        cut.Find(".s-alert-x").Click();

        cut.FindAll(".s-alert").Should().BeEmpty();
        dismissed.Should().BeTrue();
    }

    [Fact]
    public void Error_uses_alert_role()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Level, SAlertLevel.Error)
            .AddChildContent("x"));
        cut.Find(".s-alert").GetAttribute("role").Should().Be("alert");
    }

    [Fact]
    public void Non_error_uses_status_role()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.Level, SAlertLevel.Info)
            .AddChildContent("x"));
        cut.Find(".s-alert").GetAttribute("role").Should().Be("status");
    }

    [Fact]
    public void Renders_action_content()
    {
        var cut = RenderC<SAlert>(p => p
            .Add(x => x.ActionContent, b => b.AddMarkupContent(0, "<button id='act'>Undo</button>"))
            .AddChildContent("x"));
        cut.FindAll("#act").Should().HaveCount(1);
    }
}
    
