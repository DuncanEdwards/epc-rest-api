using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Epc.API.Entities;
using Epc.API.Services;
using Epc.API.Security;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using Epc.API.Helpers;

namespace Epc.API
{
    /// <summary>
    /// Main entry point
    /// </summary>
    public class Startup
    {
        #region Private Methods

        /// <summary>
        /// Configures the database context to SQL Server EPC Database
        /// </summary>
        private void ConfigureDbContext(IServiceCollection services)
        {
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["connectionStrings:epcDBConnectionString"];
            services.AddDbContext<EpcContext>(o => o.UseSqlServer(connectionString));
        }

        #endregion

        #region Public Methods

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Entity framework Db Context
            ConfigureDbContext(services);

            //Enable Cross-Origin Requests
            services.AddCors();

            // register the repository
            services.AddScoped<IEpcRepository, EpcRepository>();

            //Required to use options
            services.AddOptions();

            //Token provider options
            services.Configure<TokenProviderOptions>(Configuration.GetSection("TokenProvider"));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            EpcContext epcContext,
            IOptions<TokenProviderOptions> tokenProviderOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                //TODO:Configure properly for PROD
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }

            //Create test data
            epcContext.EnsureSeedDataForContext();

            //Hook in Jwt Bearer authorization
            app.UseJwtBearerAuthentication(TokenHelper.GetJwtBearerOptions(tokenProviderOptions.Value));


            app.UseMvc();
        }

        #endregion
    }
}
