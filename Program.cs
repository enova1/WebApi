
using DataAccess;

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


            builder.Services.AddDbContext<AuthorizedUserDbContext>();
            builder.Services.AddDbContext<EmployeeDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            builder.Services.AddSingleton<IConfiguration>(configuration);

            // Add services to the container.
            var app = builder.Build();

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

            // Run the application.
            app.Run();
        }
    }
}
