using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecondHand.model;
using System.Text.Json.Serialization;

namespace SecondHand
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
            services.AddResponseCompression();

            services.AddDbContext<Databases>(options =>
                options.UseSqlite(Configuration.GetConnectionString("MainDBContext")));
            
            services.AddDbContext<TokenDatabase>(options =>
                options.UseSqlite(Configuration.GetConnectionString("UserDBContext")));

            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.AllowTrailingCommas = false;
                option.JsonSerializerOptions.WriteIndented = true;
                option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
                app.UseExceptionHandler("/error");
            }

            app.UsePathBase("/api");
            app.UseResponseCompression();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
