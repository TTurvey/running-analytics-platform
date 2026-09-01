using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunningAnalytics.Api;
using RunningAnalytics.Infrastructure.Data;

namespace RunningAnalytics.Api.Tests.TestUtilities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        // This runs AFTER Program.cs has registered PostgreSQL
        WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove PostgreSQL provider
                var optionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                );
                if (optionsDescriptor != null)
                    services.Remove(optionsDescriptor);

                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ApplicationDbContext)
                );
                if (contextDescriptor != null)
                    services.Remove(contextDescriptor);

                // Add InMemory provider
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase($"ApiTests_{Guid.NewGuid()}")
                );

                // Build provider and ensure DB is created
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        });
    }
}

