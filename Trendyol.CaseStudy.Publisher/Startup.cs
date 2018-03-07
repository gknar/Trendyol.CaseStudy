using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Trendyol.CaseStudy.Messaging;
using Trendyol.CaseStudy.Messaging.Configurations;

namespace Trendyol.CaseStudy.Publisher
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
            services.AddMvc()
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.Formatting = Formatting.Indented;
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";
                    });


            services.AddScoped<IPublisher, Messaging.Publisher>();
            services.AddSingleton(GetConfiguration());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Trendyol Mail Gateway API", Version = "v1"});
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trendyol Mail Gateway API v1.0"); });
        }

        private BusConfigurations GetConfiguration()
        {
            var configurations = new BusConfigurations();
            Configuration.GetSection("BusConfigurations").Bind(configurations);
            return configurations;
        }
    }
}