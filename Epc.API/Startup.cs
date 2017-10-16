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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Epc.API.Middleware;

namespace Epc.API
{
    /// <summary>
    /// Main entry point
    /// </summary>
    public class Startup
    {

        #region Public Methods

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            //Enable Cross-Origin Requests
            services.AddCors();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Add Url Helper
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            // register the repository
            services.AddScoped<IEpcRepository, EpcRepository>();
            // register the emailer
            services.AddScoped<IEmailSender, MailgunEmailSender>();
            // Register the DB context
            services.AddDbContext<EpcContext>();


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
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            } else
            {
                //TODO:Configure properly for PROD, lets maybe not just turn everything off! 
                // Probably make it configurable in case React JS URL changes over time
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }


            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<User, Models.UserDto>().ForMember(
                       dest => dest.Type,
                       opt => opt.MapFrom(src => src.Type.Name));
            });

            //Create test data
            epcContext.EnsureSeedDataForContext();

            //Add custom response header so pagination is returned to browser
            app.UseAddCustomResponseHeaders();

            app.UseAuthentication();

            app.UseMvc();
        }

        #endregion
    }
}
