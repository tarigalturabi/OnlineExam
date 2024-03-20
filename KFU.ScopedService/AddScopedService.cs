using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFU.Core.Authentication;
using KFU.Core.Interfaces.Exam;
using KFU.Core.Interfaces.Security;
using KFU.Data.Exams;
using KFU.Data.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Portal.Ui.Middlewares;


namespace KFU.ScopedService
{
    public class AddScopedService
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {            
            services.AddScoped<ISecurity, DSecurity>();
            services.AddScoped<IExam, ExamSercive>();
            ConfigureJwt(services, configuration);
        }

        public static void RegisterAppUsings(IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHanlingMiddleware>();
        }

        private static void ConfigureJwt(IServiceCollection services , IConfiguration configuration)
        {
            // Configure JWT authentication
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
        }
    }
}
