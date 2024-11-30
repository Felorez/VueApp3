using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<MyData> MyData { get; set; }
    public object? UserId { get; internal set; }
    public object? UserName { get; internal set; }
    public object UserDescription { get; internal set; }

}