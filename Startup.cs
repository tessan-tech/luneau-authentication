using System;
using LuneauAuthentication.Models;
using LuneauAuthentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;

namespace LuneauAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private AppSettings ConfigureSettings(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureAuthentication(services);
            ConfigureSettings(services);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication API", Version = "v1" });
            });
            services
                .AddSingleton(sp => GetMongoCollection<Organization>(sp, "organization"))
                .AddSingleton<JwtService>(); ;
        }

        public void ConfigureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, AdminAuthHandler>("AdminAuthentication", null)
                .AddScheme<AuthenticationSchemeOptions, OrganizationAuthHandler>("OrganizationAuthentication", null);
        }

        public IMongoCollection<TDocument> GetMongoCollection<TDocument>(IServiceProvider serviceProvider, string collectionName)
        {
            MongoInfos mongoInfos = serviceProvider.GetService<IOptions<AppSettings>>().Value.Mongo;
            return new MongoClient(mongoInfos.ConnectionString)
                .GetDatabase(mongoInfos.DatabaseName)
                .GetCollection<TDocument>(collectionName);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API V1");
            });

            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
