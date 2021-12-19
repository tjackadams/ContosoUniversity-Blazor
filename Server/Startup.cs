using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using ContosoUniversity.Server.Infrastructure.Behaviours;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Shared;
using ContosoUniversity.Shared.Infrastructure;

namespace ContosoUniversity.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            services
                .AddCustomDbContext(Configuration)
                .AddAutoMapper(typeof(Startup), typeof(SharedAssemblyMarker))
                .AddCustomMediatR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }

    internal static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(SharedAssemblyMarker))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityModelBinderBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContext<SchoolContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionString"],
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30),
                                null);
                        });
                });

            return services;
        }
    }
}
