using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Reservation.Interfaces;
using Reservation.Repositories;
using Reservation.Services;

namespace Reservation
{
    public class Program4
    {
        public static void Program4Main(string[] args)
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

                        services.AddDbContext<ReservationDbContext>(opt =>
                        {
                            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                            var connectionString = config.GetConnectionString("DefaultConnection");
                            opt.UseNpgsql(connectionString, opts => opts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
                        });

                        services.AddScoped<IReservationRepository, ReservationRepository>();
                        services.AddScoped<IReservationService, ReservationService>();
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

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                            endpoints.MapHealthChecks("/manage/health");
                        });
                    });
                });
    }
}
