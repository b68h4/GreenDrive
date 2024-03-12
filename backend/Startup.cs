using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDrive.Components;
using GreenDrive.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using AspNetCoreRateLimit;

namespace GreenDrive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ConfigurationHelper.Initialize(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<DownloadStorage>();
            services.AddTransient<Cache>();
            services.AddSingleton<DriveApiService>();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddCors(options =>
            {

                options.AddDefaultPolicy(
                    builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            if (Configuration.GetValue<bool>("IpRateLimiting:Enabled"))
            {
                services.AddMemoryCache();
                services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
                services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
                services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseMiddleware<Firewall>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
            if (Configuration.GetValue<bool>("IpRateLimiting:Enabled"))
            {
                app.UseIpRateLimiting();
            }
        }
    }
}
