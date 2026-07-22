using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class ThemeServiceTests : SollyTestContext
{
    private SollyThemeService Svc => Services.GetRequiredService<SollyThemeService>();

    [Fact]
    public void Defaults_to_dark()
    {
        Svc.Theme.Should().Be("dark");
        Svc.IsDark.Should().BeTrue();
    }

    [Fact]
    public async Task SetAsync_changes_theme_and_raises_event()
    {
        var raised = 0;
        Svc.Changed += () => raised++;

        await Svc.SetAsync("light");

        Svc.Theme.Should().Be("light");
        Svc.IsDark.Should().BeFalse();
        raised.Should().Be(1);
    }

    [Fact]
    public async Task SetAsync_to_same_theme_is_a_noop()
    {
        var raised = 0;
        Svc.Changed += () => raised++;

        await Svc.SetAsync("dark");

        raised.Should().Be(0);
    }

    [Fact]
    public async Task ToggleAsync_flips_between_dark_and_light()
    {
        await Svc.ToggleAsync();
        Svc.Theme.Should().Be("light");

        await Svc.ToggleAsync();
        Svc.Theme.Should().Be("dark");
    }

    [Fact]
    public async Task Custom_theme_name_is_accepted()
    {
        await Svc.SetAsync("ocean");

        Svc.Theme.Should().Be("ocean");
        Svc.IsDark.Should().BeFalse();
    }
}