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
using System.Reflection;

namespace AzureIoTWeb
{
    public class Program
    {
        public static string currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static IConfiguration AppSetting { get; set; }
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
           

            //AppSetting = new ConfigurationBuilder()
            //      .SetBasePath(currentPath)
            //      .AddJsonFile("appsettings.json")
            //      .AddEnvironmentVariables();

            builder.Configuration.SetBasePath(currentPath).AddJsonFile("appsettings.json").AddEnvironmentVariables();


            builder.Services.Configure<EventHubSettings>(options =>
            {
                options.ConnectionString = "Endpoint=sb://ihsuprodsgres026dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=rzHAwoGsTb6fVwWyXIbaZzIqnpKRvITSs6HzAehJQpI=;EntityPath=iothub-ehub-winsonioth-20611456-ec47ae0394";
                options.Name = "iothub-ehub-winsonioth-20611456-ec47ae0394";
            });
           //Configure MongoDB Settingsadd
            builder.Services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = builder.Configuration.GetSection("MongoDb:ConnectionString").Value;
                //options.ConnectionString = "mongodb://cc25cb67-0ee0-4-231-b9ee:7sR8MUHY5jamExXKl6l7rONr63CHyKd1DBJoykXAAFawH7TpK80o5wfJS0v31vyL3tOTZsICwCzmmZ0P1dT7mQ==@cc25cb67-0ee0-4-231-b9ee.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@cc25cb67-0ee0-4-231-b9ee@";
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

            //app.UseHttpsRedirection();
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