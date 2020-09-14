using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Booking.API.Data;
using Booking.API.Idempotency;
using Booking.API.IntegrationEvents.EventHandling;
using Booking.API.IntegrationEvents.Events;
using Booking.API.Services;
using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMQ;
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
using RabbitMQ.Client;

namespace Booking.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public ILifetimeScope AutofacContainer { get; private set; }

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
      
      services.AddScoped<IEventRequestManager, EventRequestManager>();

      services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
      {
        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

        var factory = new ConnectionFactory()
        {
          HostName = Configuration["EventBusConnection"],
          DispatchConsumersAsync = true
        };

        if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
        {
          factory.UserName = Configuration["EventBusUserName"];
        }

        if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
        {
          factory.Password = Configuration["EventBusPassword"];
        }

        var retryCount = 5;
        if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
        {
          retryCount = int.Parse(Configuration["EventBusRetryCount"]);
        }

        return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
      });

      RegisterEventBus(services);
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

      ConfigureEventBus(app);
    }

    private void ConfigureEventBus(IApplicationBuilder app)
    {
      var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

      eventBus.Subscribe<CustomerDetailsChangedIntegrationEvent, CustomerDetailsChangedIntegrationEventHandler>();
    }


    private void RegisterEventBus(IServiceCollection services)
        {
          var subscriptionClientName = Configuration["SubscriptionClientName"];
          
          services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
          {
              var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
              var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
              var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
              var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

              var retryCount = 5;
              if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
              {
                  retryCount = int.Parse(Configuration["EventBusRetryCount"]);
              }

              return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, iLifetimeScope, subscriptionClientName);
          });

          services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

          services.AddTransient<CustomerDetailsChangedIntegrationEventHandler>();
          services.AddTransient<CustomerDetailsChangedIntegrationEvent>();
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
