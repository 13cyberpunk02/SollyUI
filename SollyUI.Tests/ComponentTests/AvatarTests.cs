using Bunit;
using FluentAssertions;
using Solly.UI.Components;
using Solly.UI.Core;
using Solly.UI.Icons;

namespace SollyUI.Tests.ComponentTests;

public class AvatarTests : SollyTestContext
{
    [Fact]
    public void Renders_initials_from_name()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "Anna Meyer"));
        cut.Find(".s-avatar-initials").TextContent.Should().Be("AM");
    }

    [Fact]
    public void Single_name_gives_one_initial()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "Cher"));
        cut.Find(".s-avatar-initials").TextContent.Should().Be("C");
    }

    [Fact]
    public void Initials_are_uppercased()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "boris klein"));
        cut.Find(".s-avatar-initials").TextContent.Should().Be("BK");
    }

    [Fact]
    public void Renders_image_when_src_given()
    {
        var cut = RenderC<SAvatar>(p => p
            .Add(x => x.Src, "https://example.com/a.jpg")
            .Add(x => x.Name, "Anna"));
        cut.FindAll(".s-avatar-img").Should().HaveCount(1);
        cut.FindAll(".s-avatar-initials").Should().BeEmpty();
    }

    [Fact]
    public void Renders_icon_when_given_and_no_src()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Icon, SIcons.Cursor));
        cut.FindAll("svg").Should().NotBeEmpty();
    }

    [Fact]
    public void Default_size_is_medium()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "A B"));
        cut.Find(".s-avatar").ClassList.Should().Contain("s-avatar-md");
    }

    [Theory]
    [InlineData(SAvatarSize.XSmall, "s-avatar-xs")]
    [InlineData(SAvatarSize.Small, "s-avatar-sm")]
    [InlineData(SAvatarSize.Large, "s-avatar-lg")]
    [InlineData(SAvatarSize.XLarge, "s-avatar-xl")]
    public void Applies_size_class(SAvatarSize size, string expected)
    {
        var cut = RenderC<SAvatar>(p => p
            .Add(x => x.Name, "A B")
            .Add(x => x.Size, size));
        cut.Find(".s-avatar").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Round_by_default()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "A B"));
        cut.Find(".s-avatar").ClassList.Should().Contain("s-avatar-round");
    }

    [Fact]
    public void Square_applies_class()
    {
        var cut = RenderC<SAvatar>(p => p
            .Add(x => x.Name, "A B")
            .Add(x => x.Square, true));
        cut.Find(".s-avatar").ClassList.Should().Contain("s-avatar-square");
    }

    [Fact]
    public void No_status_by_default()
    {
        var cut = RenderC<SAvatar>(p => p.Add(x => x.Name, "A B"));
        cut.FindAll(".s-avatar-status").Should().BeEmpty();
    }

    [Theory]
    [InlineData(SAvatarStatus.Online, "s-status-online")]
    [InlineData(SAvatarStatus.Away, "s-status-away")]
    [InlineData(SAvatarStatus.Busy, "s-status-busy")]
    [InlineData(SAvatarStatus.Offline, "s-status-offline")]
    public void Status_applies_class(SAvatarStatus status, string expected)
    {
        var cut = RenderC<SAvatar>(p => p
            .Add(x => x.Name, "A B")
            .Add(x => x.Status, status));
        cut.Find(".s-avatar-status").ClassList.Should().Contain(expected);
    }

    [Fact]
    public void Same_name_gives_same_hue()
    {
        var a = RenderC<SAvatar>(p => p.Add(x => x.Name, "Anna Meyer"));
        var b = RenderC<SAvatar>(p => p.Add(x => x.Name, "Anna Meyer"));
        a.Find(".s-avatar").GetAttribute("style").Should().Be(
            b.Find(".s-avatar").GetAttribute("style"));
    }

    [Fact]
    public void Fixed_hue_is_used()
    {
        var cut = RenderC<SAvatar>(p => p
            .Add(x => x.Name, "Anna")
            .Add(x => x.Hue, 200));
        cut.Find(".s-avatar").GetAttribute("style").Should().Contain("200");
    }
}

public class AvatarGroupTests : SollyTestContext
{
    [Fact]
    public void Renders_children()
    {
        var cut = RenderC<SAvatarGroup>(p => p
            .AddChildContent<SAvatar>(a => a.Add(x => x.Name, "A B"))
            .AddChildContent<SAvatar>(a => a.Add(x => x.Name, "C D")));
        cut.FindAll(".s-avatar").Should().HaveCount(2);
    }

    [Fact]
    public void Overflow_renders_more_bubble()
    {
        var cut = RenderC<SAvatarGroup>(p => p
            .Add(x => x.Overflow, 3)
            .AddChildContent<SAvatar>(a => a.Add(x => x.Name, "A B")));
        cut.Find(".s-avatar-more").TextContent.Should().Contain("+3");
    }

    [Fact]
    public void No_overflow_no_bubble()
    {
        var cut = RenderC<SAvatarGroup>(p => p
            .AddChildContent<SAvatar>(a => a.Add(x => x.Name, "A B")));
        cut.FindAll(".s-avatar-more").Should().BeEmpty();
    }
}