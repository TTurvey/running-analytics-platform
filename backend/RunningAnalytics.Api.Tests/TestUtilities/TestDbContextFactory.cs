using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Infrastructure.Data;

namespace RunningAnalytics.Api.Tests.TestUtilities;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static ApplicationDbContext CreateAndSeed(params object[] entities)
    {
        var context = CreateInMemory();

        foreach (var entity in entities)
            context.Add(entity);

        context.SaveChanges();
        return context;
    }
}
