using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class SmokeTests : SollyTestContext
{
    [Fact]
    public void Button_renders()
    {
        var cut = RenderC<SButton>(p => p.AddChildContent("Click"));
        cut.Find("button").TextContent.Should().Contain("Click");
    }
}