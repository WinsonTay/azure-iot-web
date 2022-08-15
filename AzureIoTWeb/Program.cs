using AzureIoTWeb.Data;
using AzureIoTWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using AzureIoTWeb.Services;
using AzureIoTWeb.Controllers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace AzureIoTWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            builder.Services.AddAuthentication()
                .AddIdentityServerJwt();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000");
                                  });
            });
            //builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "clientapp/build";
            });

            builder.Services.Configure<EventHubSettings>(options =>
            {
                options.ConnectionString = builder.Configuration.GetSection("EventHub:ConnectionString").Value;
                options.Name = builder.Configuration.GetSection("EventHub:HubName").Value;
            });
           //Configure MongoDB Settings
            builder.Services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = builder.Configuration.GetSection("MongoDb:ConnectionString").Value;
                options.DatabaseName = builder.Configuration.GetSection("MongoDb:DatabaseName").Value;
            });
            builder.Services.AddSingleton<IDbContext, MongoDbClient>();

            builder.Services.AddSingleton<IIoTHub, IoTHubService>();
            builder.Services.AddHostedService<MyHostedService>();
       


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowOrigin");

            app.UseAuthentication();
            //app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            //app.MapRazorPages();

            //app.MapFallbackToFile("index.html");
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

            });
            app.Run();
       
        }
    }
}