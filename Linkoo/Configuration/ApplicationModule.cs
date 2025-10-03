using Autofac;
using FluentValidation;
using ReportApp.Common.BaseEndpoint;
using ReportApp.Common.BaseEndPoint;
using ReportApp.Common.BaseHandler;
using ReportApp.Common.Helper;
using ReportApp.Common.Helper.PasswordServices;
using ReportApp.Common.views;
using ReportApp.Domain;
using ReportApp.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ReportApp.Features.Common;

namespace ReportApp.Configuration
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");

                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(connectionString)
                    .Options;

                return new Context(options);
            }).As<Context>().InstancePerLifetimeScope();

            //#region JWT Authentication Registration
            //// Register JWT authentication
            //builder.Register(context =>
            //{
            //    var config = context.Resolve<IConfiguration>();
            //    var jwtSettings = config.GetSection("JwtSettings");
            //    var secretKey = jwtSettings.GetValue<string>("SecretKey");
            //    if (string.IsNullOrEmpty(secretKey))
            //    {
            //        throw new InvalidOperationException("SecretKey is not configured properly in appsettings.json");
            //    }

            //    var key = Encoding.UTF8.GetBytes(secretKey);

            //    return new JwtBearerOptions
            //    {
            //        TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            //            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateAudience = true,
            //            ValidateIssuer = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidateLifetime = true,
            //        }
            //    };
            ////}).As<JwtBearerOptions>().SingleInstance();
            //#endregion



            builder.RegisterType<Mediator>()
             .As<IMediator>()
             .InstancePerLifetimeScope();


            #region Services Registration
            builder.RegisterType<UserInfo>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TokenHelper>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoProvider>().AsSelf().InstancePerLifetimeScope();
            #endregion


            #region MediatR Handlers Registration
            // Register MediatR request handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            #region FluentValidation Registration
            // Register FluentValidation validators
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            // Register specific endpoint parameters
            #region Endpoint Registration
            // Register specific endpoint parameters
            builder.RegisterGeneric(typeof(BaseEndpointParamter<>))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(BaseEndpointParamterWithoutTRquest))
                .AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Repository Registration
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            #endregion

            builder.RegisterGeneric(typeof(BaseRequestHandlerParamters<>))
                 .AsSelf()
                 .InstancePerLifetimeScope();

     

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterType < PasswordHelper>()
               .As<IPasswordHelper>()
               .SingleInstance();


            builder.RegisterType<NoOpValidator<Unit>>()
              .As<IValidator<Unit>>()
              .SingleInstance();

        }
    }

}