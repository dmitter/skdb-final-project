using FluentAssertions;

namespace OutsourceTracker.Domain.Tests;

public class TimeSpentTests
{
    [Theory]
    [InlineData("2020-01-01", "2020-01-10", "2020-01-02", "2020-01-08")]
    [InlineData("2020-01-01", "2020-01-10", "2020-01-05", "2020-01-20")]
    [InlineData("2020-01-01", "2020-01-10", "2019-12-01", "2020-01-20")]
    [InlineData("2020-01-01", "2020-01-10", "2019-12-01", "2020-01-05")]
    public void IntersectWith_returns_true_on_overlaps(string startTime1, string endTime1, string startTime2,
        string endTime2)
    {
        var timeSpent1 = CreateTimeSpent(DateTime.Parse(startTime1), DateTime.Parse(endTime1));
        var timeSpent2 = CreateTimeSpent(DateTime.Parse(startTime2), DateTime.Parse(endTime2));

        timeSpent1.IntersectWith(timeSpent2).Should().BeTrue();
    }

    [Theory]
    [InlineData("2020-01-01", "2020-01-10", "2020-01-10", "2020-01-20")]
    [InlineData("2020-01-10", "2020-01-20", "2020-01-01", "2020-01-10")]
    public void IntersectWith_returns_false_without_overlaps(string startTime1, string endTime1, string startTime2,
        string endTime2)
    {
        var timeSpent1 = CreateTimeSpent(DateTime.Parse(startTime1), DateTime.Parse(endTime1));
        var timeSpent2 = CreateTimeSpent(DateTime.Parse(startTime2), DateTime.Parse(endTime2));

        timeSpent1.IntersectWith(timeSpent2).Should().BeFalse();
    }

    private static TimeSpent CreateTimeSpent(DateTime startTime, DateTime endTime)
    {
        return new TimeSpent
        {
            Employee = new Employee { Name = "Test", Position = new Position { Name = "Test", HourlyRate = 0 } },
            Task = new Task { Name = "Test" },
            StartTime = startTime,
            EndTime = endTime
        };
    }
}