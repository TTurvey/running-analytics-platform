using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Models;

namespace RunningAnalytics.Api.Data;

public class RunningAnalyticsDbContext : DbContext
{
    public RunningAnalyticsDbContext(
        DbContextOptions<RunningAnalyticsDbContext> options)
        : base(options)
    {
    }
}