using FluentAssertions;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class ToastServiceTests
{
     [Fact]
    public void Starts_empty()
    {
        var svc = new SToastService();

        svc.Items.Should().BeEmpty();
    }

    [Fact]
    public void Show_adds_message_and_raises_event()
    {
        var svc = new SToastService();
        var raised = 0;
        svc.Changed += () => raised++;

        svc.Success("Saved");

        svc.Items.Should().HaveCount(1);
        svc.Items[0].Level.Should().Be(SToastLevel.Success);
        raised.Should().Be(1);
    }

    [Theory]
    [InlineData(SToastLevel.Info)]
    [InlineData(SToastLevel.Success)]
    [InlineData(SToastLevel.Warning)]
    [InlineData(SToastLevel.Error)]
    public void Each_level_can_be_shown(SToastLevel level)
    {
        var svc = new SToastService();

        svc.Show(new SToastMessage { Text = "x", Level = level });

        svc.Items[0].Level.Should().Be(level);
    }

    [Fact]
    public void Title_is_preserved()
    {
        var svc = new SToastService();

        svc.Success("All records imported.", "Import complete");

        svc.Items[0].Title.Should().Be("Import complete");
        svc.Items[0].Text.Should().Be("All records imported.");
    }

    [Fact]
    public void Remove_by_id()
    {
        var svc = new SToastService();
        svc.Info("A");
        var id = svc.Items[0].Id;

        svc.Remove(id);

        svc.Items.Should().BeEmpty();
    }

    [Fact]
    public void Remove_unknown_id_is_a_noop()
    {
        var svc = new SToastService();
        svc.Info("A");
        var raised = 0;
        svc.Changed += () => raised++;

        svc.Remove(Guid.NewGuid());

        svc.Items.Should().HaveCount(1);
        raised.Should().Be(0);
    }

    [Fact]
    public void Clear_removes_all()
    {
        var svc = new SToastService();
        svc.Info("A");
        svc.Info("B");

        svc.Clear();

        svc.Items.Should().BeEmpty();
    }

    [Fact]
    public void Clear_on_empty_does_not_raise()
    {
        var svc = new SToastService();
        var raised = 0;
        svc.Changed += () => raised++;

        svc.Clear();

        raised.Should().Be(0);
    }

    [Fact]
    public void Error_gets_longer_duration_than_default()
    {
        var svc = new SToastService();
        svc.Info("info");
        svc.Error("boom");

        var info = svc.Items[0].DurationMs;
        var error = svc.Items[1].DurationMs;

        error.Should().BeGreaterThan(info);
    }

    [Fact]
    public void Ids_are_unique()
    {
        var svc = new SToastService();
        svc.Info("A");
        svc.Info("B");

        svc.Items[0].Id.Should().NotBe(svc.Items[1].Id);
    }

    [Fact]
    public void Order_is_preserved()
    {
        var svc = new SToastService();
        svc.Info("first");
        svc.Info("second");
        svc.Info("third");

        svc.Items.Select(x => x.Text).Should().Equal("first", "second", "third");
    }
}