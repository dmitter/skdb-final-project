using Microsoft.EntityFrameworkCore;
using OutsourceTracker.Domain;
using Task = OutsourceTracker.Domain.Task;

namespace OutsourceTracker.Data;

public class OutsourceTrackerContext(string connectionString) : DbContext
{
    public DbSet<Employee> Employees { get; } = null!;

    public DbSet<Position> Positions { get; } = null!;

    public DbSet<Task> Tasks { get; } = null!;

    public DbSet<TimeSpent> TimeSpent { get; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL(connectionString);
    }
}