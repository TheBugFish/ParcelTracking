using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKS.ParcelLogistics.BusinessLogic;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using SKS.ParcelLogistics.DataAccess.Mock;
using SKS.ParcelLogistics.DataAccess.SQL;
using SKS.ParcelLogistics.ServiceAgents;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace SKS.ParcelLogistics.WebService
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;
        }


        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });

            // -- Configuration fails?
            Mapper.AssertConfigurationIsValid();

            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<ParcelModel>, ParcelValidator>();
            services.AddTransient<IValidator<TruckModel>, TruckValidator>();
            services.AddTransient<IValidator<WarehouseModel>, WarehouseValidator>();
            services.AddTransient<IValidator<RecipientModel>, RecipientValidator>();
            //services.AddTransient<DbContext, ParcelLogisticsContext>();


              services.AddDbContext<ParcelLogisticsContext>(options =>
                  options.UseSqlServer("" +
                  "Server=tcp:vaiaphraim.database.windows.net,1433;" +
                  "Initial Catalog=parcellogistics;" +
                  "Persist Security Info=False;" +
                  "User ID=boss;" +
                  "Password=Parcel123;" +
                  "MultipleActiveResultSets=False;" +
                  "Encrypt=True;" +
                  "TrustServerCertificate=False;" +
                  "Connection Timeout=30"
              )); 

        /*    services.AddDbContext<ParcelLogisticsContext>(options =>
                options.UseSqlServer("" +
                "Server=SEBASTIANLTW10\\SQLEXPRESS;" +
                "Initial Catalog=ParcelLogistics;" +
                "Integrated Security=SSPI;"
            )); */

            services.AddScoped<DbContext>(sp => sp.GetService<ParcelLogisticsContext>());

            //AddSingleton, geht nur, wenn alle abhängigkeiten singletons sind
            //"schrecklich" - nur eine instanz, datenbank würde gelockt werden... für business logic vielleicht... bleibt aber bis server / programm beendet z.b. IIS restart alle 24h, etc.

            //AddTransient: jedes mal neuer konstruktor
            //z.b. validator, aber NICHT: datenbank

            //AddScoped - sage DI, welche Klasse für Interface instanziert werden soll
            //AddScoped: pro HTTP request

            //Database still broken... using list-DB
            if (!this.HostingEnvironment.IsDevelopment())
            {
                services.AddSingleton<IHopRepository, MockHopRepository>(); // Singleton, weil mock - nur debug!
                services.AddSingleton<IParcelRepository, MockParcelRepository>(); // Singleton, weil mock - nur debug!
                services.AddSingleton<IWarehouseRepository, MockWarehouseRepository>(); // Singleton, weil mock - nur debug!
                services.AddSingleton<ITruckRepository, MockTruckRepository>(); // Singleton, weil mock - nur debug!
                services.AddScoped<IGeoEncodingAgent, MockGeoEncodingAgent>();
            }
            else
            {
                services.AddScoped<IHopRepository, SQLHopRepository>();
                services.AddScoped<IParcelRepository, SQLParcelRepository>();
                services.AddScoped<IWarehouseRepository, SQLWarehouseRepository>();
                services.AddScoped<ITruckRepository, SQLTruckRepository>();
                services.AddScoped<IGeoEncodingAgent, MockGeoEncodingAgent>(); //using MOCK to not harass google too much
                //services.AddScoped<IGeoEncodingAgent, GoogleGeoEncodingAgent>();
            }

            services.AddScoped<IBusinessLogic, BL>();
            services.AddScoped<ITrackingLogic, TrackingLogic>();
            services.AddScoped<IWarehouseLogic, WarehouseLogic>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Contract", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            this.HostingEnvironment = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contract");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ParcelLogistics}/{action=WarehouseGet}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DbContext>();
                dbContext.Database.Migrate();

            }
        }



        
    }
}