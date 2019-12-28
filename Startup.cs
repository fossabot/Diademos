using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Diademos.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http.Connections;

namespace Diademos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<News>();
            services.AddSignalR(e =>

            {

                e.MaximumReceiveMessageSize = 67108864;
                e.StreamBufferCapacity = 67108864;
                e.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");            
            });
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub(options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                    options.ApplicationMaxBufferSize = 67108864;
                    options.LongPolling.PollTimeout = new System.TimeSpan(0, 0, 10);
                });
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
