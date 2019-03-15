using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Easynvest.Infohub.Parse.Infra.IoC;
using Easynvest.Infohub.Parse.Api.Helpers;
using Easynvest.Infohub.Parse.Api.HealthChecks;

namespace Easynvest.Infohub.Parse.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddSingleton(Configuration)
                .AddOptions(Configuration)
                .AddAuthenticatedUser()
                .AddJwtAuthentication(Configuration)
                .AddAuthorizationPolicy()
                .AddMediatR()
                .AddHandlers()
                .AddRepositories()
                .ConfigureCorsService()
                .AddHealthCheck()
                .AddMvc(configuration => configuration.Filters.Add(typeof(HtmlDecoderFilter)))
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Easynvest.Infohub",
                    Version = "v1",
                    Description = "Api para cadastro de de-para do infohub",
                    Contact = new Contact { Url = "https://bitbucket.org/easynvest/easynvest.infohub/src" }
                });
                options.AddSecurityDefinition(
                    "bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Favor gerar um token jwt para efetuar a requisição",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            loggerFactory.AddLog4Net($"log4net.{env.EnvironmentName}.config");

            app.UseSwagger()
                .ConfigureCors(env)
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Easynvest.Infohub.Parse"))
                .UseAuthentication()
                .UseHealthAllEndpoints()
                .UseMvc();
        }
    }
}
