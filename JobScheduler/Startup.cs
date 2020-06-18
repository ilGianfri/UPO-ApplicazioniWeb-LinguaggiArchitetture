using JobScheduler.Areas.Identity;
using JobScheduler.BackgroundWorker;
using JobScheduler.Controllers;
using JobScheduler.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JobScheduler
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
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                    options.EnableSensitiveDataLogging(true);
                });

            services.AddDefaultIdentity<IdentityUser>(options => options.User.RequireUniqueEmail = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.TryAddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddHttpContextAccessor();
            //services.AddSingleton<HttpClient>();

            services.TryAddScoped<UserMethods>();
            services.TryAddScoped<NodesMethods>();
            services.TryAddScoped<JobsMethods>();
            services.TryAddScoped<SchedulesMethods>();
            services.TryAddScoped<GroupsMethods>();
            services.TryAddScoped<JobReportMethods>();

            services.TryAddScoped<JobsScheduler>();
            services.TryAddScoped<JobRunner>();

            services.TryAddScoped<DataSeed>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Scheduler Master API", Version = "v1", Description = "This is the API exposed by the Master" });
            });

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                    options.SaveToken = true;
                });

            services.AddMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            /** API Docs */
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Scheduler Master API V1");
            });
            /**                  */

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            using var scope = app.ApplicationServices.CreateScope();
            var seed = scope.ServiceProvider.GetService<DataSeed>();
            await seed.SeedAsync();

            //Background worker that runs jobs on the master or sends them to slaves
            scope.ServiceProvider.GetService<JobsScheduler>();
        }
    }
}
