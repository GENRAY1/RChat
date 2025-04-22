using System.Text;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RChat.Application.Abstractions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.Search.Chat;
using RChat.Application.Services.Search.GlobalSearch;
using RChat.Application.Services.Search.Message;
using RChat.Application.Services.Search.User;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users.Repository;
using RChat.Infrastructure.Background.TaskQueue;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.Repositories;
using RChat.Infrastructure.Exceptions;
using RChat.Infrastructure.Services.Authentication;
using RChat.Infrastructure.Services.Search;
using RChat.Infrastructure.Services.Search.Common;
using RChat.Infrastructure.Services.Search.GlobalSearch;

namespace RChat.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerService();
        services.AddBackgroundServices();
        services.AddStorageServices(configuration);
        services.AddSearchServices(configuration);
        services.AddAuthenticationService(configuration);
    }

    private static void AddStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") 
                                  ?? throw new ConfigurationException("DefaultConnection");
        
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres() 
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations()) 
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
    }

    private static void AddSearchServices(this IServiceCollection services, IConfiguration configuration)
    {
        ElasticsearchOptions? elasticConfig =
            configuration.GetSection("ElasticSearch").Get<ElasticsearchOptions>();
        
        if(elasticConfig is null)
            throw new ConfigurationException("Elastic search configuration is missing or invalid");

        var settings = new ElasticsearchClientSettings(new Uri(elasticConfig.Url))
            .Authentication(new BasicAuthentication(elasticConfig.Username, elasticConfig.Password));
        
        services.AddSingleton<ElasticsearchClient>(new ElasticsearchClient(settings));
        services.AddScoped<IUserSearchService, UserSearchService>();
        services.AddScoped<IChatSearchService, ChatSearchService>();
        services.AddScoped<IMessageSearchService, MessageSearchService>();
        services.AddScoped<IGlobalSearchService, GlobalSearchService>();
    }

    private static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<TaskQueueBackgroundService>();
    }
    
    private static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenTrends.Api", Version = "v0.1" });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Description = "Please insert JWT token into field"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
    
    private static void AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsConfiguration = configuration.GetSection("Jwt");
        services.Configure<JwtOptions>(jwtOptionsConfiguration);
        
        var jwtOptions = jwtOptionsConfiguration.Get<JwtOptions>() ??
                         throw new ConfigurationException(jwtOptionsConfiguration.Path);
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordEncryptorService, PasswordEncryptorService>();
        
        services.AddScoped<IAuthContext, AuthContext>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        
                        return Task.CompletedTask;
                    }
                };
            });
    }
}