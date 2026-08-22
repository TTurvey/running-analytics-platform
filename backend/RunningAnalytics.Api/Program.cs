using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RunningAnalyticsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => "Running Analytics API");

app.Run();
