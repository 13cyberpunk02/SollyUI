using Bunit;
using FluentAssertions;
using Solly.UI.Components;

namespace SollyUI.Tests.ComponentTests;

public class NumberInputTests : SollyTestContext
{
    [Fact]
    public void Renders_text_input_not_number()
    {
        var cut = RenderC<SNumberInput<int>>(p => p.Add(x => x.Value, 0));
        cut.Find("input").GetAttribute("type").Should().Be("text");
    }

    [Fact]
    public void Inputmode_numeric_for_integers()
    {
        var cut = RenderC<SNumberInput<int>>(p => p.Add(x => x.Value, 0));
        cut.Find("input").GetAttribute("inputmode").Should().Be("numeric");
    }

    [Fact]
    public void Inputmode_decimal_when_decimals()
    {
        var cut = RenderC<SNumberInput<decimal>>(p => p
            .Add(x => x.Value, 0m)
            .Add(x => x.Decimals, 2));
        cut.Find("input").GetAttribute("inputmode").Should().Be("decimal");
    }

    [Fact]
    public void Shows_initial_value()
    {
        var cut = RenderC<SNumberInput<int>>(p => p.Add(x => x.Value, 42));
        cut.Find("input").GetAttribute("value").Should().Be("42");
    }

    [Fact]
    public void Renders_label()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.Label, "Quantity"));
        cut.Find("label").TextContent.Should().Contain("Quantity");
    }

    [Fact]
    public void Steppers_shown_by_default()
    {
        var cut = RenderC<SNumberInput<int>>(p => p.Add(x => x.Value, 0));
        cut.FindAll(".s-num-step").Should().HaveCount(2);
    }

    [Fact]
    public void Steppers_hidden_when_disabled_via_ShowSteppers()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.ShowSteppers, false));
        cut.FindAll(".s-num-step").Should().BeEmpty();
    }

    [Fact]
    public void Increment_stepper_raises_value()
    {
        var value = 5;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Step, 1m));

        cut.FindAll(".s-num-step")[0].Click();
        value.Should().Be(6);
    }

    [Fact]
    public void Decrement_stepper_lowers_value()
    {
        var value = 5;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Step, 1m));

        cut.FindAll(".s-num-step")[1].Click();
        value.Should().Be(4);
    }

    [Fact]
    public void Increment_respects_step()
    {
        var value = 0;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Step, 5m));

        cut.FindAll(".s-num-step")[0].Click();
        value.Should().Be(5);
    }

    [Fact]
    public void Increment_clamps_to_max()
    {
        var value = 99;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Max, 99m)
            .Add(x => x.Step, 1m));

        cut.FindAll(".s-num-step")[0].Click();
        value.Should().Be(99);
    }

    [Fact]
    public void Increment_disabled_at_max()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 99)
            .Add(x => x.Max, 99m));
        cut.FindAll(".s-num-step")[0].HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Decrement_disabled_at_min()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.Min, 0m));
        cut.FindAll(".s-num-step")[1].HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Typing_digits_updates_value()
    {
        var value = 0;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").Input("123");
        value.Should().Be(123);
    }

    [Fact]
    public void Typing_letters_is_ignored()
    {
        var value = 0;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value));

        cut.Find("input").Input("12a3");
        value.Should().Be(123);
    }

    [Fact]
    public void Negative_rejected_when_min_non_negative()
    {
        var value = 0;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Min, 0m));

        cut.Find("input").Input("-5");
        value.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Negative_allowed_when_min_negative()
    {
        var value = 0;
        var cut = RenderC<SNumberInput<int>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Min, -50m));

        cut.Find("input").Input("-5");
        value.Should().Be(-5);
    }

    [Fact]
    public void Prefix_renders()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.Prefix, "€"));
        cut.Find(".s-num-prefix").TextContent.Should().Contain("€");
    }

    [Fact]
    public void Suffix_renders()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.Suffix, "%"));
        cut.Find(".s-num-suffix").TextContent.Should().Contain("%");
    }

    [Fact]
    public void Disabled_input_has_attribute()
    {
        var cut = RenderC<SNumberInput<int>>(p => p
            .Add(x => x.Value, 0)
            .Add(x => x.Disabled, true));
        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Works_with_decimal_type()
    {
        var value = 0m;
        var cut = RenderC<SNumberInput<decimal>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Decimals, 2)
            .Add(x => x.Step, 0.5m));

        cut.FindAll(".s-num-step")[0].Click();
        value.Should().Be(0.5m);
    }

    [Fact]
    public void Works_with_nullable_type()
    {
        decimal? value = null;
        var cut = RenderC<SNumberInput<decimal?>>(p => p
            .Bind(x => x.Value, value, v => value = v, () => value)
            .Add(x => x.Decimals, 2));

        cut.Find("input").Input("19.99");
        value.Should().Be(19.99m);
    }
}