using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Epc.API.Entities;
using Epc.API.Services;
using Microsoft.Extensions.Options;
using System.Text;
using Epc.API.Helpers;
using Epc.API.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        public void ConfigureServices(
            IServiceCollection services)
        {
            //Entity framework Db Context
            //ConfigureDbContext(services);

            //Enable Cross-Origin Requests
            services.AddCors();

            // register the repository
            services.AddScoped<IEpcRepository, EpcRepository>();
            // register the emailer
            services.AddScoped<IEmailSender, MailgunEmailSender>();

            //Required to use options
            services.AddOptions();

            //Token provider settings
            services.Configure<TokenProviderSettings>(Configuration.GetSection("TokenProvider"));
            //Email settings
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            //Get token provider options for token validation config
            var serviceProvider = services.BuildServiceProvider();
            var tokenProviderOptions = serviceProvider.GetService<IOptions<TokenProviderSettings>>();

            //Hook in Jwt Bearer authorization service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenHelper.GetTokenValidationParameters(tokenProviderOptions.Value);
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            EpcContext epcContext)
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

            app.UseAuthentication();

            app.UseMvc();
        }

        #endregion
    }
}
