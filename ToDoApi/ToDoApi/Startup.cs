using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IO;
using System.Security.Claims;
using ToDoApi.Authorization;
using ToDoApi.Services;
using ToDoCore.Interfaces;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => 
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApi", Version = "v1" });
            });

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddMvcCore()
                .AddApiExplorer();

            services.AddDbContext<ToDoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ToDoDbContext")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IToDoService, ToDoService>();

            services.AddOptions();
            services.Configure<ReminderConfig>(Configuration.GetSection("ReminderConfig"));
            services.AddHostedService<ReminderService>();

            services.AddCors(c =>
            {
                c.AddPolicy("ToDoCors", options => 
                    options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ToDoDbContext dataContext, IHostApplicationLifetime applicationLifetime)
        {
            dataContext.Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApi v1"); c.DocumentTitle = "To Do App"; });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ToDoCors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationLifetime.ApplicationStarted.Register(OnStartup);
            applicationLifetime.ApplicationStopped.Register(OnShutdown);
        }

        private void OnStartup()
        {
            Log.Debug("ToDoApi started!");
        }

        private void OnShutdown()
        {
            Log.Debug("ToDoApi stopped!");
        }
    }
}
