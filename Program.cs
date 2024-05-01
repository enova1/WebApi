using Hangfire;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Hangfire.Dashboard;
using Hangfire.EntityFrameworkCore;
using Hangfire.SqlServer;
using Hangfire.SQLite;
using WebApi.Controllers.v1;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create the builder and services
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    build => build.WithOrigins("https://localhost:7007") // Allow only this origin can be changed to allow multiple origins with a list of strings
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            // IConfigurationRoot configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json")
            //     .Build();
            // builder.Services.AddSingleton<IConfiguration>(configuration);

            // Add Hangfire services
            // builder.Services.AddHangfire(globalConfiguration => globalConfiguration
            //     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            //     .UseSimpleAssemblyNameTypeSerializer()
            //     .UseRecommendedSerializerSettings()
            //     .UseEFCoreStorage(optionsBuilder => { optionsBuilder.UseSqlite(@"Data Source=C:\Users\kozlo\source\GitHub\DAL\DataAccess\bin\Debug\net8.0\WebApi.db"); })
            // );

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'HangfireConnection' not found.");


            builder.Services.AddDbContext<AuthorizedUserDbContext>();
            builder.Services.AddDbContext<EmployeeDbContext>();

            builder.Services.AddHangfire(configuration =>
                configuration.UseEFCoreStorage(dbContextOptionsBuilder =>
                            dbContextOptionsBuilder.UseSqlite(connectionString),
                        new EFCoreStorageOptions
                        {
                            CountersAggregationInterval = new TimeSpan(0, 5, 0),
                            DistributedLockTimeout = new TimeSpan(0, 10, 0),
                            JobExpirationCheckInterval = new TimeSpan(0, 30, 0),
                            QueuePollInterval = new TimeSpan(0, 0, 15),
                            Schema = string.Empty,
                            SlidingInvisibilityTimeout = new TimeSpan(0, 5, 0),
                        }).
                    UseDatabaseCreator());

            builder.Services.AddHangfireServer(options =>
            {
                options.WorkerCount = 1;
            });


            // Add services to the container.
            var app = builder.Build();

            // Initialize Hangfire schema
            // InitializeHangfireSchema(app.Services);

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ModelStateValidationMiddleware>();
            // Enable CORS
            app.UseCors("AllowSpecificOrigin");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Enable Hangfire dashboard (optional)
            // app.UseHangfireDashboard(
            //     string.Empty,
            //     new DashboardOptions
            //     {
            //         AppPath = null,
            //         Authorization = Array.Empty<IDashboardAuthorizationFilter>(),
            //     });



            // Run the application.
            app.Run();
        }

        // private static void InitializeHangfireSchema(IServiceProvider serviceProvider)
        // {
        //     // Ensure Hangfire tables are created in the database
        //     using (var scope = serviceProvider.CreateScope())
        //     {
        //         var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //         dbContext.Database.Migrate();
        //     }
        // }
    }
}
