using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using NoviSDP2.Repository;

namespace NoviSDP2
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

            services.AddControllersWithViews();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // this requires InMemory Nuget Package
            services.AddDbContext<DbTestContext>(options => options.UseInMemoryDatabase("TestString"));


            //Addidentity registrers the services
            services.AddIdentity<Employee, IdentityRole<int>>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<DbTestContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });


            var service = services.FirstOrDefault(s => s.ImplementationType == typeof(UserValidator<Employee>));
            if (service != null)
            {
                services.Remove(service);


            }
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

          
            app.UseRouting();


            // who is the visitor?
            app.UseAuthentication();

            // check if the visitor is allowed
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
   
            });


            //Here is initialize the DB with some mock data
            DbInitialize.Init(app);
        }
    }
}
