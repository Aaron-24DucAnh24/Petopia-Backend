using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Implementations;
using PetAdoption.DataLayer.Extensions;
using PetAdoption.Business.Data;
using PetAdoption.Business.Validators;
using PetAdoption.Business.Utils;
using PetAdoption.Business.Contexts;
using PetAdoption.Business.Models;
using Microsoft.IdentityModel.Tokens;

namespace PetAdoption.Business.Extensions
{
  public static class ServiceExtension
  {
    public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddCacheService(configuration);
      services.AddModelValidators();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICookieService, CookieService>();
      services.AddScoped<IStorageService, StorageService>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<IUserService, UserService>();
    }

    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddApplicationDbContext(configuration, AppSettingKey.DB_CONNECTION_STRING);
      services.AddScoped<IUserContext, UserContext>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddHttpContextAccessor();
      services.AddDataLayerServices();
      services.AddSwaggerService();
      services.AddJwtAuthentication(configuration);
      services.AddCorsPolicies(configuration);
    }

    public static void AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
      var redisCacheSetting = configuration.GetSection(AppSettingKey.REDIS_CACHE).Get<RedisCacheSettingModel>();
      if (redisCacheSetting != null && !string.IsNullOrEmpty(redisCacheSetting.ConnectionString))
      {
        services.AddStackExchangeRedisCache(options =>
        {
          options.InstanceName = redisCacheSetting.InstanceName;
          options.Configuration = redisCacheSetting.ConnectionString;
        });
      }
      else
      {
        services.AddDistributedMemoryCache();
      }
      services.AddMemoryCache();
      services.AddSingleton<ICacheManager, CacheManager>();
    }

    public static void AddModelValidators(this IServiceCollection services)
    {
      services.AddScoped<IModelValidationService, ModelValidationService>();
      services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
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
          options.TokenValidationParameters = TokenUtil.CreateTokenValidationParameters(configuration);
          options.Events = new JwtBearerEvents()
          {
            OnMessageReceived = context =>
            {
              var endpoint = context.HttpContext.GetEndpoint();
              var authorized = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null;
              if (authorized)
              {
                var accessToken = TokenUtil.GetAccessTokenFromRequest(context.Request)
                  ?? throw new UnauthorizedAccessException();
                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                var isValid = authService.ValidateAccessToken(accessToken);

                if (isValid)
                {
                  context.Token = accessToken;
                }
                else
                {
                  throw new SecurityTokenValidationException();
                }
              }
              return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
              var claimsPrincipal = context.Principal
                ?? throw new SecurityTokenValidationException();
              var userContextInfo = TokenUtil.GetUserContextInfoFromClaims(claimsPrincipal.Claims)
                ?? throw new SecurityTokenValidationException();
              var userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
              userContext.Email = userContextInfo.Email;
              userContext.FirstName = userContextInfo.FirstName;
              userContext.Role = userContextInfo.Role;
              userContext.Id = userContextInfo.Id;
              userContext.LastName = userContextInfo.LastName;
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
        options.OperationFilter<SecureEndpointAuthRequirementFilter>();
      });
    }

    //--------------- Swagger configuration -----------------//
    internal class SecureEndpointAuthRequirementFilter : IOperationFilter
    {
      public void Apply(OpenApiOperation operation, OperationFilterContext context)
      {
        if (!context.ApiDescription
          .ActionDescriptor
          .EndpointMetadata
          .OfType<AuthorizeAttribute>()
          .Any())
        {
          return;
        }
        operation.Security = new List<OpenApiSecurityRequirement>
        {
          new OpenApiSecurityRequirement
          {
            [new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
              Name = "Bearer",
              In = ParameterLocation.Header,
            }] = new List<string>()
          }
        };
      }
    }
  }
}