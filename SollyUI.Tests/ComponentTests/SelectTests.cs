using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class SelectTests : SollyTestContext
{
    private static readonly string[] Options = { "Alpha", "Beta", "Gamma" };

    [Fact]
    public void Shows_placeholder_when_empty()
    {
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Placeholder, "Pick one"));

        cut.Find(".s-select-value").TextContent.Should().Contain("Pick one");
    }

    [Fact]
    public void Shows_selected_value()
    {
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Value, "Beta"));

        cut.Find(".s-select-value").TextContent.Should().Contain("Beta");
    }

    [Fact]
    public void Opening_renders_all_items()
    {
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item").Should().HaveCount(3);
    }

    [Fact]
    public void Clicking_item_updates_value_and_closes()
    {
        string? value = null;
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-select-trigger").Click();
        cut.FindAll(".s-select-item")[1].Click();

        value.Should().Be("Beta");
        cut.FindAll(".s-select-item").Should().BeEmpty();  // closed
    }

    [Fact]
    public void Empty_items_shows_empty_text()
    {
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Array.Empty<string>())
            .Add(x => x.EmptyText, "Nothing here"));

        cut.Find(".s-select-trigger").Click();

        cut.Find(".s-select-empty").TextContent.Should().Contain("Nothing here");
    }

    [Fact]
    public void Uses_Display_function()
    {
        var cities = new[] { new City("Berlin", "DE"), new City("Paris", "FR") };

        var cut = Render<SSelect<City>>(p => p
            .Add(x => x.Items, cities)
            .Add(x => x.Display, c => c.Name)
            .Add(x => x.Value, cities[0]));

        cut.Find(".s-select-value").TextContent.Should().Contain("Berlin");
    }

    [Fact]
    public void Record_equality_highlights_selection()
    {
        var cities = new[] { new City("Berlin", "DE"), new City("Paris", "FR") };

        var cut = Render<SSelect<City>>(p => p
            .Add(x => x.Items, cities)
            .Add(x => x.Display, c => c.Name)
            .Add(x => x.Value, new City("Paris", "FR")));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item.s-selected").Should().HaveCount(1);
    }

    [Fact]
    public void Multiple_mode_renders_chips()
    {
        var selected = new List<string> { "Alpha", "Beta" };

        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Multiple, true)
            .Add(x => x.Values, selected));

        cut.FindAll(".s-chip").Should().HaveCount(2);
    }

    [Fact]
    public void Disabled_select_does_not_open()
    {
        var cut = Render<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Disabled, true));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item").Should().BeEmpty();
    }

    private record City(string Name, string Country);
}