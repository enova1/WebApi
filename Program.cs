using Hangfire;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Hangfire.Dashboard;
using Hangfire.EntityFrameworkCore;
using WebApi.Services.Hangfire;
using Microsoft.OpenApi.Models;

namespace WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        // Create the builder and services
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Hangfire Microservice",
                Description = "This Api Microservice is the main Hangfire scheduler service.</br></br><a href='https://localhost:7098/hangfire'>Hangfire dashboard</a>",
                TermsOfService = new Uri("https://blazorui20230314133145.azurewebsites.net/privacy-policy"),
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "Support@B2Gnow.com",
                    Url = new Uri("https://blazorui20230314133145.azurewebsites.net")
                }
            });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                build => build
                    .WithOrigins(
                        "https://localhost:7007") // Allow only this origin can be changed to allow multiple origins with a list of strings
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });


        var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection")
                                 ?? throw new InvalidOperationException("Connection string 'HangfireConnection' not found.");


        builder.Services.AddDbContext<ApplicationDbContext>();
        //builder.Services.AddDbContext<EmployeeDbContext>();

        builder.Services.AddHangfire(configuration =>
            configuration.UseEFCoreStorage(dbContextOptionsBuilder =>
                    dbContextOptionsBuilder.UseSqlite(hangfireConnectionString),
                new EFCoreStorageOptions
                {
                    CountersAggregationInterval = new TimeSpan(0, 5, 0),
                    DistributedLockTimeout = new TimeSpan(0, 10, 0),
                    JobExpirationCheckInterval = new TimeSpan(0, 30, 0),
                    QueuePollInterval = new TimeSpan(0, 0, 15),
                    Schema = string.Empty,
                    SlidingInvisibilityTimeout = new TimeSpan(0, 5, 0),
                }).UseDatabaseCreator());

        builder.Services.AddHangfireServer(options => { options.WorkerCount = 1; });

        builder.Services.AddScoped<Continuations>();
        builder.Services.AddScoped<Delayed>();
        builder.Services.AddScoped<Recurring>();
        builder.Services.AddScoped<RunOnce>();

        // Add services to the container.
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseMiddleware<ModelStateValidationMiddleware>();

        // Enable CORS
        app.UseCors("AllowSpecificOrigin");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Enable Hangfire dashboard
        app.UseHangfireDashboard(
            "/hangfire",
            new DashboardOptions
            {
                AppPath = "https://localhost:7098/swagger/index.html",
                Authorization = Array.Empty<IDashboardAuthorizationFilter>(),
                DarkModeEnabled = true,
                DashboardTitle = "Hangfire BCT Dashboard",
                DisplayStorageConnectionString = true,
                DefaultRecordsPerPage = 10
            });

        // Run the application.
        app.Run();
    }
}