using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Rating
{
    public class Program3
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Добавление сервисов в контейнер.
                        services.AddHealthChecks();
                        services.AddControllers();
                        services.AddEndpointsApiExplorer();

                        services.AddDbContext<RatingDbContext>(opt =>
                        {
                            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                            var connectionString = config.GetConnectionString("DefaultConnection");
                            opt.UseNpgsql(connectionString, opts => opts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
                        });

                        services.AddScoped<IRatingRepository, RatingRepository>();
                        services.AddScoped<IRatingService, RatingService>();
                        services.AddCors();
                    })
                    .Configure((context, app) =>
                    {
                        // Конфигурация конвейера HTTP-запросов.
                        app.UseCors(builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("X-Total-Count")
                            .WithExposedHeaders(""));

                        app.UseHsts();

                        app.UseRouting();

                        app.MapControllers();
                        app.MapHealthChecks("/manage/health");
                    });
                });
    }
}
