using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RestfulQr.Api.Core;
using RestfulQr.Api.Core.Auth;
using RestfulQr.Api.Core.Caching;
using RestfulQr.Api.Core.Providers;
using RestfulQr.Api.Services;
using RestfulQr.Api.Services.Impl;
using RestfulQr.Core.Rendering;
using RestfulQr.Domain;
using RestfulQr.Migrations;
using RestfulQr.Persistence;
using RestfulQr.Persistence.Local;
using RestfulQr.Persistence.S3;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace RestfulQr
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Migrations
            services.AddDbContext<RestfulQrDbContext>(options =>
            {
                var config = Configuration.GetConnectionString("QrCodeDb");

                options.UseNpgsql(Configuration.GetConnectionString("QrCodeDb"), x => x.MigrationsAssembly("RestfulQr.Migrations"));
            });

            // Auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => { });

            // Redis Cache
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetSection("Redis")["Configuration"];
                options.InstanceName = "Redis";
            });

            // API Services
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IImageService, ImageService>();

            // Per Request Providers
            services.AddScoped<QrCodeRenderOptions>();
            services.AddScoped<ApiKeyProvider>();
            services.AddScoped<ICacheProvider<ApiKey>, CacheProvider<ApiKey>>();
            services.AddScoped<ICacheProvider<byte[]>, CacheProvider<byte[]>>();

            // Persistence
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddScoped<IRestfulQrCodeRepository, RestfulQrCodeRepository>();

            // Image Persistence
            if (Configuration["StorageEngine"] == "s3")
            {
                // AWS
                services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
                services.AddScoped<IImagePersistor, S3ImagePersistor>();
            } 
            else if (Configuration["StorageEngine"] == "local")
            {
                // Create the directory
                Directory.CreateDirectory(Configuration.GetSection("LocalStorage")["Path"]);
                services.AddScoped<IImagePersistor, LocalImagePersistor>();
            } 
            else
            {
                throw new ArgumentException("Configuration must contain a valid value for \"StorageEngine\"");
            }
            
            // Rendering
            services.AddScoped<IQrCodeRenderer, Renderer>();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestfulQr", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestfulQrDbContext context)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestfulQr v1"));

                context.Database.Migrate();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseQrCodeOptionsExtraction();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
