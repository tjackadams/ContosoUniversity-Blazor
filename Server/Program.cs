using System;
using System.IO;
using AutoMapper;
using ContosoUniversity.Server;
using ContosoUniversity.Server.Infrastructure;
using ContosoUniversity.Server.Infrastructure.Behaviours;
using ContosoUniversity.Server.Infrastructure.Extensions;
using ContosoUniversity.Shared;
using ContosoUniversity.Shared.Infrastructure;
using DelegateDecompiler;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

var AppName = "ContosoUniversity.Server";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.WithProperty("ApplicationContext", AppName)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", AppName);
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews()
        .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>())
        .Services
        .AddRazorPages()
        .Services
        .AddDbContext<SchoolContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"],
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30),
                        null);
                });
        })
        .AddAutoMapper(typeof(Startup), typeof(SharedAssemblyMarker))
        .AddMediatR(typeof(Startup), typeof(SharedAssemblyMarker))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityModelBinderBehaviour<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");
        

    Log.Information("Applying migrations ({ApplicationContext})...", AppName);
    //app.MigrateDbContext<SchoolContext>((context, services) =>
    //{
    //    var env = services.GetRequiredService<IWebHostEnvironment>();
    //    var logger = services.GetRequiredService<ILogger<SchoolContextSeed>>();

    //    new SchoolContextSeed()
    //        .SeedAsync(context, env, logger)
    //        .Wait();
    //});

    Log.Information("Starting web host ({ApplicationContext})...", AppName);
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


static IHost BuildWebHost(IConfiguration configuration, string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.CaptureStartupErrors(false)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog();
        })
        .Build();
}

