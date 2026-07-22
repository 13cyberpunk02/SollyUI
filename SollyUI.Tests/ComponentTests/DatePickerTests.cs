using System.Globalization;
using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class DatePickerTests : SollyTestContext
{
      [Fact]
    public void Shows_placeholder_when_empty()
    {
        DateTime? value = null;

        var cut = RenderC<SDatePicker>(p => p
            .Add(x => x.Placeholder, "Pick a date")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-value").TextContent.Should().Contain("Pick a date");
    }

    [Fact]
    public void Formats_selected_date()
    {
        DateTime? value = new DateTime(2026, 7, 22);

        var cut = RenderC<SDatePicker>(p => p
            .Add(x => x.Format, "yyyy-MM-dd")
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-value").TextContent.Should().Contain("2026-07-22");
    }

    [Fact]
    public void Opening_renders_calendar_grid()
    {
        DateTime? value = new DateTime(2026, 7, 22);

        var cut = RenderC<SDatePicker>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();

        // 7 day-of-week headers + 42 day cells
        cut.FindAll(".s-cal-dow").Should().HaveCount(7);
        cut.FindAll(".s-cal-day").Should().HaveCount(42);
    }

    [Fact]
    public void Clicking_a_day_sets_the_value()
    {
        DateTime? value = new DateTime(2026, 7, 22);

        var cut = RenderC<SDatePicker>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();

        var target = cut.FindAll(".s-cal-day")
            .First(e => !e.HasAttribute("disabled") && e.TextContent.Trim() == "15");
        target.Click();

        value!.Value.Day.Should().Be(15);
    }

    [Fact]
    public void Min_disables_earlier_days()
    {
        DateTime? value = new DateTime(2026, 7, 15);

        var cut = RenderC<SDatePicker>(p => p
            .Add(x => x.Min, new DateTime(2026, 7, 10))
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();

        cut.FindAll(".s-cal-day[disabled]").Should().NotBeEmpty();
    }

    [Fact]
    public void Selected_day_has_class()
    {
        DateTime? value = new DateTime(2026, 7, 22);

        var cut = RenderC<SDatePicker>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();

        cut.FindAll(".s-cal-day.s-selected").Should().HaveCount(1);
    }

    [Fact]
    public void Next_month_advances_the_view()
    {
        DateTime? value = new DateTime(2026, 7, 22);

        var cut = RenderC<SDatePicker>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();
        var before = cut.Find(".s-cal-title").TextContent;

        cut.FindAll(".s-cal-nav")[1].Click();

        cut.Find(".s-cal-title").TextContent.Should().NotBe(before);
    }

    [Fact]
    public void Respects_culture_first_day_of_week()
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");   // Monday first
            DateTime? value = new DateTime(2026, 7, 22);

            var cut = RenderC<SDatePicker>(p => p
                .Bind(x => x.Value, value, v => value = v, () => value));

            cut.Find(".s-dp-trigger").Click();

            var first = cut.FindAll(".s-cal-dow")[0].TextContent.Trim();
            first.Should().StartWith("Mo");
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void Disabled_does_not_open()
    {
        DateTime? value = null;

        var cut = RenderC<SDatePicker>(p => p
            .Add(x => x.Disabled, true)
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find(".s-dp-trigger").Click();

        cut.FindAll(".s-cal-day").Should().BeEmpty();
    }
}