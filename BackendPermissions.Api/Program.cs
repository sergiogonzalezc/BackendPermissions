using BackendPermissions.Application.Services;
using BackendPermissions.Infraestructura.Repository;
using BackendPermissions.Application.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Globalization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Configuration;
using BackendPermissions.Application.ConfiguracionApi;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BackendPermissions.Api.Model;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using System.Diagnostics;
using System.Reflection;
using BackendPermissions.Common;
using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;
using MediatR;
using Autofac.Core;
using Elasticsearch.Net;
using Nest;

namespace BackendPermissions.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string envName = builder.Environment.EnvironmentName;

            if (envName != "Development" && envName != "qa")
            {
                //builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
            }


            // Add services to the container.
            builder.Services.AddScoped<IPermissionsApplication, PermissionsApplication>();
            builder.Services.AddScoped<IPermissionsRepository, PermissionsEFRepository>();
            builder.Services.AddScoped(typeof(IPermissionsApplication), typeof(PermissionsApplication));
            builder.Services.AddScoped(typeof(IPermissionsRepository), typeof(PermissionsEFRepository));

            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ErrorHandlingFilterAttribute>();
            });

            //builder.Services.AddMediatR(typeof(Program).Assembly);
            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            }

            builder.Services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
            });

            builder.Services.AddOptions();

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(pool).DefaultIndex("permision-index");
            var client = new ElasticClient(settings);
            builder.Services.AddSingleton(client);

            //builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // define culture spanish CL
            var cultureInfo = new CultureInfo("es-CL");
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
                SupportedCultures = new List<CultureInfo>
                {
                    cultureInfo,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    cultureInfo,
                }
            });

            app.UseCors(option =>
            {
                option.WithOrigins("http://127.0.0.1:5173");
                option.WithOrigins("http://127.0.0.1:8081");
                option.WithOrigins("http://127.0.0.1:8082");
                option.WithOrigins("http://localhost:5173");
                option.WithOrigins("http://localhost:8081");
                option.WithOrigins("http://localhost:8082");

                //option.AllowAnyOrigin();
                option.AllowAnyMethod();
                option.AllowAnyHeader();
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else
            {
                //Solo habilitado en producción
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

                app.UseHsts();
            }

            app.UseExceptionHandler("/error");

            app.UseRouting();

            app.UseResponseCaching();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();
            app.UseCors();

            app.Run();


            // SGC - Asigna la version del Assembly al Log4Net
            Assembly thisApp = Assembly.GetExecutingAssembly();

            AssemblyName name = new AssemblyName(thisApp.FullName);

            // Identifica la versión del ensamblado
            GlobalDiagnosticsContext.Set("VersionApp", name.Version.ToString());

            // Identifica el nombre del ensamblado para poder identificar el ejecutable
            GlobalDiagnosticsContext.Set("AppName", name.Name);

            // Obtiene el process ID
            Process currentProcess = Process.GetCurrentProcess();

            GlobalDiagnosticsContext.Set("ProcessID", "PID " + currentProcess.Id.ToString());

            ServiceLog.Write(Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "===== INICIO API [BackendPermissions.Api] =====");
        }
    }
}