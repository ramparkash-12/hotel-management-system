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
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API
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
            services.AddControllers()
            .Services
            .AddCustomMVC(Configuration)
            .AddCustomDbContext(Configuration)
            .AddAutoMapperMethod(Configuration)
            .AddSwagger(Configuration)
            .AddJwtBearerSettings(Configuration);

            services.AddSingleton<ISystemClock, SystemClock>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); 
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint("v1/swagger.json", "Identity.API V1");
             });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
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
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(configuration["ConnectionString"],
            sqlServerOptionsAction: sqlOptions =>
            {
            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
      });
      services.AddMvc(opt => 
        {
            var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        });

      IdentityBuilder builder = services.AddIdentityCore<AppUser>(options =>
            {
              // Password settings.
              options.Password.RequiredLength = 6;
              // Default Lockout settings.
              options.Lockout.AllowedForNewUsers = false;

              options.User.RequireUniqueEmail = true;
            });

        builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
        builder.AddEntityFrameworkStores<ApplicationDbContext>();
        builder.AddRoleValidator<RoleValidator<Role>>();    
        builder.AddRoleManager<RoleManager<Role>>();
        builder.AddSignInManager<SignInManager<AppUser>>();

        services.AddAuthorization(options =>{
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            options.AddPolicy("RequireAdminManagerRole", policy => policy.RequireRole("Admin", "Manager"));
        });
    
      return services;
    }

    public static IServiceCollection AddJwtBearerSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        return services;
    }

    public static IServiceCollection AddAutoMapperMethod(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Models.AppUser).Assembly);
        
        return services;
    }

    
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hotel Management System - User HTTP API",
                    Version = "v1",
                    Description = "The Identity Microservice",
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
