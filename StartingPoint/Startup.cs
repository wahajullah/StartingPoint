using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using StartingPoint.Data;
using StartingPoint.Helpers;
using StartingPoint.Models;
using StartingPoint.Services;
using StartingPoint.Models.CommonViewModel;

namespace StartingPoint
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
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();


            services.AddScoped<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            DefaultIdentityOptions _DefaultIdentityOptions = null;

            var _IServiceScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
            var _CreateScope = _IServiceScopeFactory.CreateScope();
            var _ServiceProvider = _CreateScope.ServiceProvider;
            var _context = _ServiceProvider.GetRequiredService<ApplicationDbContext>();
            bool IsDBCanConnect = _context.Database.CanConnect();


            if (IsDBCanConnect && _context.DefaultIdentityOptions.Count() > 0)
                _DefaultIdentityOptions = _context.DefaultIdentityOptions.Where(x => x.Id == 1).SingleOrDefault();
            else
            {
                IConfigurationSection _IConfigurationSection = Configuration.GetSection("IdentityDefaultOptions");
                services.Configure<DefaultIdentityOptions>(_IConfigurationSection);
                _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
            }

            AddIdentityOptions.SetOptions(services, _DefaultIdentityOptions);

            // Get Super Admin Default options
            services.Configure<SuperAdminDefaultOptions>(Configuration.GetSection("SuperAdminDefaultOptions"));
            services.Configure<ApplicationInfo>(Configuration.GetSection("ApplicationInfo"));

            services.AddTransient<ICommon, Common>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRoles, Roles>();
            services.AddTransient<IFunctional, Functional>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
