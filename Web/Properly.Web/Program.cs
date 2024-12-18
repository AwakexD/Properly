namespace Properly.Web
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using AutoMapper;
    using CloudinaryDotNet;
    using dotenv.net;
    using SendGrid;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Hosting;
    using Properly.Common;
    using Properly.Data;
    using Properly.Data.Common;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models.User;
    using Properly.Data.Repositories;
    using Properly.Data.Seeding;
    using Properly.Services.Data;
    using Properly.Services.Data.Admin;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Services.Messaging;
    using Properly.Web.Hubs;
    using Properly.Web.Infrastructure.ModelBinders;
    using Properly.Web.ViewModels;


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            // AntiForgery Header
            services.AddAntiforgery(options =>
            {
                options.HeaderName = GlobalConstants.AntiForgeryHeaderName;
            });

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new YearToDateTimeModelBinderProvider());
            });

            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
            
            // SignalR
            services.AddSignalR();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IListingOptionsService, ListingOptionsService>();
            services.AddTransient<IAdminListingOptionsService, AdminListingOptionsService>();
            services.AddTransient<IAdminListingService, AdminListingService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IUserService, UserService>();

            // Cloudinary
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;
            services.AddSingleton(cloudinary);

            // SendGrid
            DotEnv.Load(options: new DotEnvOptions(probeForEnv:true));
            services.AddSingleton<ISendGridClient>(provider =>
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                return new SendGridClient(apiKey);
            });
            services.AddSingleton<IEmailSender, SendGridEmailSender>();

            // Register AutoMapper
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            services.AddSingleton(typeof(IMapper), AutoMapperConfig.MapperInstance);
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enforce default culture
            var defaultCulture = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new[] { defaultCulture },
                SupportedUICultures = new[] { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UserStatusHub>("/userStatusHub"); 
            });
            app.MapRazorPages();
        }
    }
}