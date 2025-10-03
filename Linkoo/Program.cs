
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReportApp.Common.Helper;
using ReportApp.Common.views;
using ReportApp.Configuration;
using ReportApp.Domain;
using ReportApp.Features.Common.Notification;
using ReportApp.Filters;
using System.Text;

namespace ReportApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Add Entity Framework Core with SQL Server
            builder.Services.AddDbContext<Context>(options => options.
            UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #region JwtSettings
            builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JWTSetting"));
            builder.Services.AddAuthentication(options =>
            {
                // Use JWT as the default authentication method
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JWTSetting").Get<JWTSetting>();

                // Set Token Validation Rules
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
            #endregion
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
           
            builder.Host.ConfigureContainer<ContainerBuilder>(container =>
            {
                container.RegisterModule(new ApplicationModule());
            });
            builder.Services.AddScoped<UserInfoProvider>();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<UserInfoFilter>(); 
            });
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UserNotificationEventHandler).Assembly));


            builder.Services.AddScoped<UserInfoFilter>();
            builder.Services.AddScoped<CustomizeAuthorizeAttribute>();
            builder.Services.AddMemoryCache();
            var app = builder.Build();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
