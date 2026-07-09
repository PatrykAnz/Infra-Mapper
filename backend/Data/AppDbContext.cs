using Microsoft.EntityFrameworkCore;
using InfraMapper.Models;

namespace InfraMapper.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CloudTask> Tasks { get; set; }
}
