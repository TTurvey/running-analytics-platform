using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Models;

namespace RunningAnalytics.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    //public DbSet<User> Users { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    modelBuilder.Entity<Objective>().HasData(
    //        new Objective { Id = 1, Name = "Visit London", Description = "Visit London" },
    //        new Objective { Id = 2, Name = "Visit Birmingham", Description = "Visit Birmingham" },
    //        new Objective { Id = 3, Name = "Visit Manchester", Description = "Visit Manchester" });

    //    modelBuilder.Entity<User>().HasData(
    //        new User { Id = 1, FirstName = "John", LastName = "Smith", Email = "johnsmith@email.com", Username = "User1", Password = "Abc123" },
    //        new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "janesmith@email.com", Username = "User2", Password = "Def456" });
    //}
}