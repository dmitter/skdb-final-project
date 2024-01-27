using Microsoft.EntityFrameworkCore;
using OutsourceTracker.Domain;
using Task = OutsourceTracker.Domain.Task;

namespace OutsourceTracker.Data;

public class OutsourceTrackerContext(string connectionString) : DbContext
{
    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Position> Positions { get; set; } = null!;

    public DbSet<Task> Tasks { get; set; } = null!;

    public DbSet<TimeSpent> TimeSpent { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL(connectionString);
    }
}