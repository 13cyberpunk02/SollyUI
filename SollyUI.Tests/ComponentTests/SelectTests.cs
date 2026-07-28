using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class SelectTests : SollyTestContext
{
   private static readonly string[] Options = { "Alpha", "Beta", "Gamma" };

    private record City(string Name, string Country);

    private static readonly City[] Cities =
    {
        new("Berlin", "DE"),
        new("Paris", "FR"),
        new("Lisbon", "PT"),
    };

    [Fact]
    public void Shows_placeholder_when_empty()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Placeholder, "Pick one"));

        cut.Find(".s-select-value").TextContent.Should().Contain("Pick one");
    }

    [Fact]
    public void Shows_selected_value()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Value, "Beta"));

        cut.Find(".s-select-value").TextContent.Should().Contain("Beta");
    }

    [Fact]
    public void Closed_by_default()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options));

        cut.FindAll(".s-select-item").Should().BeEmpty();
    }

    [Fact]
    public void Opening_renders_all_items()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item").Should().HaveCount(3);
    }

    [Fact]
    public void Clicking_item_updates_value()
    {
        string? value = null;

        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-select-trigger").Click();
        cut.FindAll(".s-select-item")[1].Click();

        value.Should().Be("Beta");
    }

    [Fact]
    public void Clicking_item_closes_the_list()
    {
        string? value = null;

        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-select-trigger").Click();
        cut.FindAll(".s-select-item")[0].Click();

        cut.FindAll(".s-select-item").Should().BeEmpty();
    }

    [Fact]
    public void Empty_items_shows_empty_text()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Array.Empty<string>())
            .Add(x => x.EmptyText, "Nothing here"));

        cut.Find(".s-select-trigger").Click();

        cut.Find(".s-select-empty").TextContent.Should().Contain("Nothing here");
    }

    [Fact]
    public void Null_items_shows_empty_text()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.EmptyText, "No options"));

        cut.Find(".s-select-trigger").Click();

        cut.Find(".s-select-empty").TextContent.Should().Contain("No options");
    }

    [Fact]
    public void Uses_Display_function_in_trigger()
    {
        var cut = RenderC<SSelect<City>>(p => p
            .Add(x => x.Items, Cities)
            .Add(x => x.Display, c => c.Name)
            .Add(x => x.Value, Cities[0]));

        cut.Find(".s-select-value").TextContent.Should().Contain("Berlin");
    }

    [Fact]
    public void Uses_Display_function_in_list()
    {
        var cut = RenderC<SSelect<City>>(p => p
            .Add(x => x.Items, Cities)
            .Add(x => x.Display, c => $"{c.Name} ({c.Country})"));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item")[0].TextContent.Should().Contain("Berlin (DE)");
    }

    [Fact]
    public void Falls_back_to_ToString_without_Display()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Value, "Alpha"));

        cut.Find(".s-select-value").TextContent.Should().Contain("Alpha");
    }

    [Fact]
    public void Record_equality_highlights_selection()
    {
        // a different instance with the same values must still match
        var cut = RenderC<SSelect<City>>(p => p
            .Add(x => x.Items, Cities)
            .Add(x => x.Display, c => c.Name)
            .Add(x => x.Value, new City("Paris", "FR")));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item.s-selected").Should().HaveCount(1);
        cut.Find(".s-select-item.s-selected").TextContent.Should().Contain("Paris");
    }

    [Fact]
    public void Searchable_renders_search_box()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Searchable, true));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-search-input").Should().HaveCount(1);
    }

    [Fact]
    public void Not_searchable_has_no_search_box()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-search-input").Should().BeEmpty();
    }

    [Fact]
    public void Search_filters_items()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Searchable, true));

        cut.Find(".s-select-trigger").Click();
        cut.Find(".s-select-search-input").Input("bet");

        cut.FindAll(".s-select-item").Should().HaveCount(1);
        cut.Find(".s-select-item").TextContent.Should().Contain("Beta");
    }

    [Fact]
    public void Search_is_case_insensitive()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Searchable, true));

        cut.Find(".s-select-trigger").Click();
        cut.Find(".s-select-search-input").Input("ALPHA");

        cut.FindAll(".s-select-item").Should().HaveCount(1);
    }

    [Fact]
    public void Custom_Filter_is_used()
    {
        var cut = RenderC<SSelect<City>>(p => p
            .Add(x => x.Items, Cities)
            .Add(x => x.Display, c => c.Name)
            .Add(x => x.Searchable, true)
            .Add(x => x.Filter, (c, q) => c.Country.Contains(q, StringComparison.OrdinalIgnoreCase)));

        cut.Find(".s-select-trigger").Click();
        cut.Find(".s-select-search-input").Input("FR");

        cut.FindAll(".s-select-item").Should().HaveCount(1);
        cut.Find(".s-select-item").TextContent.Should().Contain("Paris");
    }

    [Fact]
    public void Multiple_mode_renders_chips()
    {
        var selected = new List<string> { "Alpha", "Beta" };

        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Multiple, true)
            .Add(x => x.Values, selected));

        cut.FindAll(".s-chip").Should().HaveCount(2);
    }

    [Fact]
    public void Multiple_mode_collapses_overflow_chips()
    {
        var selected = new List<string> { "Alpha", "Beta", "Gamma" };

        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Multiple, true)
            .Add(x => x.MaxChips, 2)
            .Add(x => x.Values, selected));

        cut.FindAll(".s-chip-more").Should().HaveCount(1);
        cut.Find(".s-chip-more").TextContent.Should().Contain("1");
    }

    [Fact]
    public void Multiple_mode_stays_open_after_pick()
    {
        IEnumerable<string> selected = [];

        var cut = RenderC<SSelect<string>>(p =>
        {
            var initialValue = selected.ToList();
            p
                .Add(x => x.Items, Options)
                .Add(x => x.Multiple, true)
                .Bind(x => x.Values, initialValue, v =>
                {
                    if (v is not null) selected = v;
                }, () => initialValue);
        });

        cut.Find(".s-select-trigger").Click();
        cut.FindAll(".s-select-item")[0].Click();

        cut.FindAll(".s-select-item").Should().NotBeEmpty();
    }

    [Fact]
    public void Multiple_mode_toggles_selection()
    {
        IEnumerable<string> selected = [];

        var cut = RenderC<SSelect<string>>(p =>
        {
            var initialValue = selected.ToList();
            p
                .Add(x => x.Items, Options)
                .Add(x => x.Multiple, true)
                .Bind(x => x.Values, initialValue, v =>
                {
                    if (v is not null) selected = v;
                }, () => initialValue);
        });

        cut.Find(".s-select-trigger").Click();
        cut.FindAll(".s-select-item")[1].Click();

        selected.Should().ContainSingle().Which.Should().Be("Beta");
    }

    [Fact]
    public void Clearable_shows_clear_when_selected()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Clearable, true)
            .Add(x => x.Value, "Alpha"));

        cut.FindAll(".s-select-clear").Should().HaveCount(1);
    }

    [Fact]
    public void Clearable_hides_clear_when_empty()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Clearable, true));

        cut.FindAll(".s-select-clear").Should().BeEmpty();
    }

    [Fact]
    public void Clear_resets_value()
    {
        string? value = "Alpha";

        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Clearable, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-select-clear").Click();

        value.Should().BeNull();
    }

    [Fact]
    public void Disabled_select_does_not_open()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Disabled, true));

        cut.Find(".s-select-trigger").Click();

        cut.FindAll(".s-select-item").Should().BeEmpty();
    }

    [Fact]
    public void Disabled_select_has_class()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Disabled, true));

        cut.Find(".s-select-trigger").ClassList.Should().Contain("s-disabled");
    }

    [Fact]
    public void Renders_label()
    {
        var cut = RenderC<SSelect<string>>(p => p
            .Add(x => x.Items, Options)
            .Add(x => x.Label, "Country"));

        cut.Find("label").TextContent.Should().Contain("Country");
    }
}