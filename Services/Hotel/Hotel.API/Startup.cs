using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hotel.API.Data;
using Hotel.API.Services;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Hotel.API.Extensions;
using Microsoft.Extensions.Logging.Debug;

namespace hotel.api
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
            services.AddControllers().AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .Services
            .AddCustomMVC(Configuration)
            .AddCustomDbContext(Configuration)
            .AddSwagger(Configuration)
            .AddAutoMapperMethod(Configuration);

            ConfigureAuthService(services);

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRespository, RoomRepository>();
            services.AddScoped<IImageService, ImageService>();
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
             // prevent from mapping "sub" claim to nameidentifier.
             JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

             var identityUrl = Configuration.GetValue<string>("IdentityUrl");

             services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

             }).AddJwtBearer(options =>
             {
                 options.Authority = identityUrl;
                 //options.MetadataAddress = "http://identityapi/.well-known/openid-configuration";
                 options.RequireHttpsMetadata = false;
                 options.Audience = "hotel";
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                            //** also log error with logger for inspection.
                            logger.LogError(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel.API V1");
                 c.OAuthClientId("orderingswaggerui");
                 c.OAuthAppName("Ordering Swagger UI");
             });

            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultControllerRoute();
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
    var loggerFactory = new LoggerFactory();
    loggerFactory.AddProvider(new DebugLoggerProvider());
        
      services.AddDbContext<HotelContext>(options =>
      {
        options.UseSqlServer(configuration["ConnectionString"],
            sqlServerOptionsAction: sqlOptions =>
            {
            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });

        options.UseLoggerFactory(loggerFactory);
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
                    Title = "Hotel Management System - Hotel HTTP API",
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
