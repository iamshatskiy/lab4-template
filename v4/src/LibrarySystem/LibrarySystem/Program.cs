using LibrarySystem.Interfaces;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace LibrarySystem
{
    public class Program2
    {
        public static void Main(string[] args)
        {
            // Запуск приложения.
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Добавление сервисов в контейнер.
                        services.AddControllers();
                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen();

                        services.AddCors();
                        services.AddHealthChecks();

                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway Docs", Version = "v1" });

                            // Установка пути для комментариев к Swagger JSON и UI.
                            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                            // c.IncludeXmlComments(xmlPath);
                        });

                        services.AddScoped<ILibrarySystemService, LibrarySystemService>();
                    })
                    .Configure((context, app) =>
                    {
                        // Конфигурация конвейера HTTP-запросов.

                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API V1");
                            });
                        }

                        app.UseAuthorization();

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
