using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Implementations;
using Petopia.DataLayer.Extensions;
using Petopia.Business.Utils;
using Petopia.Business.Contexts;
using Petopia.Business.Models.Setting;
using Petopia.Business.Filters;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Braintree;
using Petopia.Business.Models.Enums;
using Petopia.Business.Validators;
using AutoMapper;
using Petopia.Business.Data;
using System.Security.Claims;
using Petopia.Business.Models.User;

namespace Petopia.Business.Extensions
{
  public static class ServiceExtension
  {
    public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddModelValidators();
      services.AddCacheService(configuration);
      services.AddEmailService(configuration);
      services.AddElasticsearchService(configuration);
      services.AddPaymentService(configuration);
      services.AddAutoMapper(configuration);
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICookieService, CookieService>();
      services.AddScoped<IUserService, UserService>();
      services.AddTransient<IHttpService, Implementations.HttpService>();
      services.AddScoped<IPetService, PetService>();
      services.AddScoped<IAzureService, AzureService>();
    }

    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddApplicationDbContext(configuration, AppSettingKey.DB_CONNECTION_STRING);
      services.AddScoped<IUserContext, UserContext>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddHttpContextAccessor();
      services.AddDataLayerServices();
      services.AddSwaggerService();
      services.AddHttpClient();
      services.AddJwtAuthentication(configuration);
      services.AddCorsPolicies(configuration);
    }

    public static void AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
      RedisCacheSettingModel? redisCacheSettings = configuration.GetSection(AppSettingKey.REDIS_CACHE).Get<RedisCacheSettingModel>();
      if (redisCacheSettings != null && !string.IsNullOrEmpty(redisCacheSettings.ConnectionString))
      {
        services.AddStackExchangeRedisCache(options =>
        {
          options.InstanceName = redisCacheSettings.InstanceName;
          options.Configuration = redisCacheSettings.ConnectionString;
        });
      }
      services.AddMemoryCache();
      services.AddSingleton<ICacheManager, CacheManager>();
    }

    public static void AddModelValidators(this IServiceCollection services)
    {
      services.AddFluentValidationAutoValidation();
      services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    }

    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
      EmailSettingModel? emailSettings = configuration.GetSection(AppSettingKey.EMAIL).Get<EmailSettingModel>();
      if (emailSettings != null)
      {
        services.AddSingleton(emailSettings);
        services.AddScoped<IEmailService, EmailService>();
      }
    }

    public static void AddElasticsearchService(this IServiceCollection services, IConfiguration configuration)
    {
      ElasticsearchSettingModel? elasticSearchSettings = configuration.GetSection(AppSettingKey.ELASTICSEARCH).Get<ElasticsearchSettingModel>();
      if (elasticSearchSettings != null)
      {
        var settings = new ElasticsearchClientSettings(new Uri(elasticSearchSettings.Url))
          .Authentication(new BasicAuthentication(
            elasticSearchSettings.Username,
            elasticSearchSettings.Password
          ));
        services.AddSingleton(settings);
        services.AddScoped<IElasticsearchService, ElasticsearchService>();
      }
    }

    public static void AddPaymentService(this IServiceCollection services, IConfiguration configuration)
    {
      BraintreeSettingModel? braintreeSettings = configuration.GetSection(AppSettingKey.BRAINTREE).Get<BraintreeSettingModel>();
      if (braintreeSettings != null)
      {
        BraintreeGateway gateway = new()
        {
          Environment = braintreeSettings.IsProduction ? Braintree.Environment.PRODUCTION : Braintree.Environment.SANDBOX,
          MerchantId = braintreeSettings.MerchantId,
          PublicKey = braintreeSettings.PublicKey,
          PrivateKey = braintreeSettings.PrivateKey
        };
        services.AddSingleton<IBraintreeGateway>(gateway);
        services.AddScoped<IPaymentService, PaymentService>();
      }
    }

    public static void AddAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
      MapperConfiguration mapperConfig = new(mc =>
      {
        mc.AddProfile(new MappingProfiles());
      });
      IMapper mapper = mapperConfig.CreateMapper();
      services.AddSingleton(mapper);
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
      services
        .AddAuthentication(opt =>
        {
          opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.SaveToken = true;
          options.TokenValidationParameters = TokenUtils.CreateTokenValidationParameters(TokenType.AccessToken, configuration);
          options.Events = new JwtBearerEvents()
          {
            OnMessageReceived = context =>
            {
              Endpoint? endpoint = context.HttpContext.GetEndpoint();
              bool authorized = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null;
              if (authorized)
              {
                string accessToken = TokenUtils.GetAccessTokenFromRequest(context.Request)
                  ?? throw new UnauthorizedAccessException();
                context.HttpContext.RequestServices
                  .GetRequiredService<IAuthService>()
                  .ValidateAccessToken(accessToken);
                context.Token = accessToken;
              }
              return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
              ClaimsPrincipal claimsPrincipal = context.Principal
                ?? throw new SecurityTokenValidationException();
              UserContextModel userContextInfo = TokenUtils.GetUserContextInfoFromClaims(claimsPrincipal.Claims)
                ?? throw new SecurityTokenValidationException();
              IUserContext userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
              userContext.SetUserContext(userContextInfo);
              return Task.CompletedTask;
            }
          };
        });
    }

    public static void AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddCors(options => options.AddDefaultPolicy(policy =>
      {
        string? originConfig = configuration.GetSection(AppSettingKey.CORS_ORIGIN).Get<string>();
        if (string.IsNullOrEmpty(originConfig) || originConfig.Equals("*"))
        {
          policy
            .SetIsOriginAllowed((_) => true)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        }
        else
        {
          string[] origins = originConfig
            .Split(";", StringSplitOptions.RemoveEmptyEntries)
            .ToArray();
          policy
            .WithOrigins(origins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        }
      }));
    }

    public static void AddSwaggerService(this IServiceCollection services)
    {
      services.AddSwaggerGen(options =>
      {
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });
        options.OperationFilter<RequiredAuthenticationFilter>();
        options.OperationFilter<AccessRoleFilter>();
      });
    }
  }
}