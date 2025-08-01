using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Templates;
using webApi.Application.Interfaces;
using webApi.Application.Mappings;
using webApi.Application.Services;
using webApi.Data;
using webApi.Domain.Interfaces;
using webApi.Infrastructure.Repositories;

namespace webApi.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Configura el logger inicial antes de levantar el host
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("🚀 Iniciando la API WEB...");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                                                   .ReadFrom.Configuration(context.Configuration)
                                                   .ReadFrom.Services(services)
                                                   .WriteTo.Console(new ExpressionTemplate(
                                                       "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                                                   .WriteTo.Debug());

                builder.Services.AddExceptionHandler<Middlewares.GlobalExceptionHandler>();
                builder.Services.AddProblemDetails(); // necesario para usar IProblemDetailsService

                builder.Services.AddControllers();

                builder.Services.AddDbContext<DatabaseContext>(options =>
                   options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

                builder.Services.AddScoped<IProductRepository, ProductRepository>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ProductMapper>();

                // Add services to the container.
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddSwaggerGen();
                builder.Services.AddCors();

                var app = builder.Build();

                app.Use(async (context, next) =>
                                {
                                    if (context.Request.Path == "/")
                                    {
                                        context.Response.Redirect("/swagger/index.html", permanent: true);
                                        return;
                                    }

                                    await next();
                                });

                app.UseCors(opt =>
                {
                    opt.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseExceptionHandler();
                app.MapControllers();


                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "❌ La aplicación falló al iniciar");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}