using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuneauAuthentication.Models;
using LuneauAuthentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LuneauAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureAuthentication(services);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddSingleton(sp => GetMongoCollection<Organization>(sp, "organization"));
        }

        public void ConfigureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, AdminAuthHandler>("AdminAuthentication", null)
                .AddScheme<AuthenticationSchemeOptions, AdminAuthHandler>("OrganizationAuthentication", null);
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
