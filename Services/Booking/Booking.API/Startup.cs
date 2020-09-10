using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Booking.API.Data;
using Booking.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Booking.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers().AddNewtonsoftJson(opt =>
      {
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      })
      .Services
      .AddCustomMVC(Configuration)
      .AddCustomDbContext(Configuration)
      .AddSwagger(Configuration)
      .AddAutoMapperMethod(Configuration);

        services.AddScoped<IGenericRepository, GenericRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseSwagger()
       .UseSwaggerUI(c =>
       {
         c.SwaggerEndpoint("v1/swagger.json", "Booking.API V1");
       });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }

  public static class CustomExtensionMethods
  {

    public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                  builder => builder
                  .SetIsOriginAllowed((host) => true)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
      });

      return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<BookingContext>(options =>
      {
        options.UseSqlServer(configuration["ConnectionString"],
            sqlServerOptionsAction: sqlOptions =>
            {
              sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
              sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
      });

      return services;
    }

    public static IServiceCollection AddAutoMapperMethod(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddAutoMapper(typeof(Startup).Assembly);

      return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Hotel Management System - Booking HTTP API",
          Version = "v1",
          Description = "The Hotel Microservice",
          Contact = new OpenApiContact
          {
            Name = "M Haris",
            Email = string.Empty,
            Url = new Uri("https://github.com/haristauqir"),
          }
        });
      });

      return services;

    }
  }
}
