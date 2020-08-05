using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NoviSDP2.Controllers;
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


            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<CryptoController, CryptoController>();
            // this requires InMemory Nuget Package
            services.AddDbContext<DbTestContext>(options => options.UseInMemoryDatabase("TestString")
            .EnableSensitiveDataLogging()
            );


            //Addidentity registrers the services
            services.AddIdentity<Person, IdentityRole<int>>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                
              
            })
                    .AddEntityFrameworkStores<DbTestContext>()
                  
                   

                    .AddDefaultTokenProviders();

            services.AddIdentityCore<Student>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;


            })

                                .AddSignInManager<SignInManager<Student>>()
                                .AddRoles<IdentityRole<int>>()

                                .AddDefaultTokenProviders()
                                .AddEntityFrameworkStores<DbTestContext>();


            //services.AddAuthorization(config =>
            //{
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    var defaultAuthPolicy = defaultAuthBuilder
            //    .RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.DateOfBirth)
            //    .Build();

            //    config.DefaultPolicy = defaultAuthPolicy;
            //});

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Novi.Cookie";
                config.LoginPath = "/Home/Login";
                config.LogoutPath = "/Home/Logout";
            }
            );



            services.AddControllersWithViews();


        }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void  Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole<int>> roleManager)
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

            // this is to set the currrency to Euro sign
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
           
            RolesData.SeedRoles(roleManager).Wait();
        }

    }

    public static class IdentityExtensions
    {
        public static IdentityBuilder AddSecondIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped<UserManager<TUser>, AspNetUserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();
            services.TryAddScoped<RoleManager<TRole>, AspNetRoleManager<TRole>>();

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }
    }
}
