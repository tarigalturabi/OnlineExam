using System.Text;
using KFU.Core.Authentication;
using KFU.Core.Interfaces.Cache;
using KFU.Core.Interfaces.Exam;
using KFU.Core.Interfaces.Security;
using KFU.Data.Cache;
using KFU.Data.Exams;
using KFU.Data.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Portal.Ui.Middlewares;
using StackExchange.Redis;


namespace KFU.ScopedService
{
    public class AddScopedService
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {            
            services.AddScoped<ISecurity, DSecurity>();
            services.AddScoped<IExam, ExamSercive>();
            ConfigureJwt(services, configuration);

            // Register caching service
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, InMemoryCacheService>();

            // Register Redis cache
            
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

        private static void ConfigureRedis(IServiceCollection services , IConfiguration configurations)
        {
            var redisConnStr = configurations["RedisCache:ConnectionString"];
            var instanceName = configurations["RedisCache:InstanceName"];
            var configuration = ConfigurationOptions.Parse(redisConnStr);
            
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                configuration.ResolveDns = true;
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddSingleton<IDistributedCache>(provider =>
            {
                var connectionMultiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
                return new RedisCache(new RedisCacheOptions
                {
                    //Configuration  = configuration,
                    InstanceName = instanceName,
                });
            });

            // Register caching service
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}
