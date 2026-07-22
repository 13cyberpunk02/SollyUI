using FluentAssertions;
using Solly.UI.Core;

namespace SollyUI.Tests.ComponentTests;

public class ToastServiceTests
{
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
    public void Clear_removes_all()
    {
        var svc = new SToastService();
        svc.Info("A");
        svc.Info("B");

        svc.Clear();

        svc.Items.Should().BeEmpty();
    }

    [Fact]
    public void Error_gets_longer_duration()
    {
        var svc = new SToastService();
        svc.Error("boom");

        svc.Items[0].DurationMs.Should().BeGreaterThan(4000);
    }
}