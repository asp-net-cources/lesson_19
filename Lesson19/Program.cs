using Lesson19.JsonSettings.Converters;
using Lesson19.JsonSettings.Policies;
using System.Text.Json.Serialization;
using Lesson19.Data;
using Lesson19.Data.EF;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection")
                       ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = new UpperCaseNamingPolicy();
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new ProductJsonConverter());
    });

builder.Services.AddDbContext<IDataContext, EfDataContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .WriteTo.File(new CompactJsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMetricServer();
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    using (app.Logger.BeginScope("Start handling for random number {RandomNumber}", new Random().Next()))
        using (app.Logger.BeginScope(new Dictionary<string, int>() { { "UserId", 123} }))
    {
        await next.Invoke();
    }
});

app.Run();