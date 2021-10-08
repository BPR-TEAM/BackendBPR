using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using BackendBPR.Database;

namespace BackendBPR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddCors((options =>
                { options.AddPolicy("AllowAndrei", options=>options.WithOrigins("https://orange-bush-0a396ce03.azurestaticapps.net/",
                 "http://10.10.23.187", "http://10.10.23.187:3000").AllowAnyHeader().AllowAnyMethod());}));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrangeBushApi", Version = "v1" });
            });

            services.AddDbContext<OrangeBushContext>(options 
                => options.UseNpgsql("Server=orangebush.postgres.database.azure.com;Database=postgres;Port=5432;User Id=bpr@orangebush;Password=279280282MLAmla;Ssl Mode=Require;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 🦍
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrangeBushApi 🦍 v1"));

            
                app.UseDeveloperExceptionPage();
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAndrei");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
