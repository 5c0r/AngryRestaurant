using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AngryRESTaurant.WebAPI.Consumers;
using AngryRESTaurant.WebAPI.Contracts;
using AngryRESTaurant.WebAPI.Repository;
using AngryRESTaurant.WebAPI.Services;
using Marten;
using MassTransit;
using MediatR;
using Serilog;

namespace AngryRESTaurant.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Hosting { get; }

        public Startup(IConfiguration configuration, IHostEnvironment hosting)
        {
            Configuration = configuration;
            Hosting = hosting;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AngryRESTaurant.WebAPI", Version = "v1" });
            });

            // Marten for persistence , this should also included some Session injection also ,
            // which we sadly don't use them yet
            var postgresConnstring = Configuration.GetConnectionString("Marten");
            services.AddMarten(opts =>
            {
                opts.Connection(postgresConnstring);

                if (Hosting.IsDevelopment())
                {
                    opts.AutoCreateSchemaObjects = AutoCreate.All;
                }
            });

            // TODO: Next episode ? MassTransit
            services.AddMassTransit(mt =>
            {
                // if (Hosting.IsDevelopment())
                // {
                //     mt.UsingInMemory((context, cfg) =>
                //     {
                //         cfg.TransportConcurrencyLimit = 10;
                //
                //         cfg.ConfigureEndpoints(context);
                //     });
                // }

                // TODO: What else B-RabbitMQ ??
                mt.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost");

                    MessageDataDefaults.TimeToLive = TimeSpan.FromMinutes(5);
                    MessageDataDefaults.Threshold = 2000;
                    MessageDataDefaults.AlwaysWriteToRepository = false;
                    cfg.ConcurrentMessageLimit = 1;

                    cfg.ConfigureEndpoints(ctx);
                    // TODO: Marten as repository ? Not yet
                });

                mt.AddRequestClient<OrderCreateRequest>();

                mt.AddConsumer<Waiter>();
                mt.AddConsumer<Cook>();
                mt.AddConsumer<Server>();
            });

            services.AddMassTransitHostedService();

            // Should have done this, why I am so stupid...
            services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));
            services.AddMediatR(typeof(Startup));


            // Dependency Injection
            services.AddScoped<OrderingService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AngryRESTaurant.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSerilogRequestLogging(); // <-- Add this line
        }
    }
}
