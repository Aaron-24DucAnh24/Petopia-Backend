using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Services.Interfaces;
using PetAdoption.Business.Services.Implementations;
using PetAdoption.DataLayer.Extensions;
using PetAdoption.Business.Data;
using PetAdoption.Business.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PetAdoption.Business.Models;
using PetAdoption.Business.Contexts;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation;
using PetAdoption.Business.Validators;

namespace PetAdoption.Business.Extensions
{
  public static class ServiceExtension
  {
    public static void AddBusinessServices(this IServiceCollection services)
    {
      services.AddSingleton<ICacheService, CacheService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICookieService, CookieService>();
      services.AddScoped<IBlobService, BlobService>();

      services.AddModelValidators();
    }

    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddApplicationDbContext(configuration, "database");
      services.AddScoped<IUserContext, UserContext>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddHttpContextAccessor();
      services.AddDataLayerServices();
      services.AddSwaggerService();
      services.AddJwtAuthentication(configuration);
      services.AddCorsPolicies();
    }

    public static void AddModelValidators(this IServiceCollection services)
    {
      services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
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
              Endpoint? endpoint = context.HttpContext.GetEndpoint();
              bool authorized = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null;

              if (authorized)
              {
                string? accessToken = TokenUtil.GetAccessTokenFromRequest(context.Request) ?? throw new Exception(ExceptionMessage.UNAUTHORIZED);
                IAuthService authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                bool isValid = authService.ValidateAccessToken(accessToken);

                if (isValid)
                {
                  context.Token = accessToken;
                }
                else
                {
                  throw new Exception(ExceptionMessage.INVALID_ACCESS_TOKEN);
                }
              }

              return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
              ClaimsPrincipal? claimsPrincipal = context.Principal ?? throw new Exception(ExceptionMessage.INVALID_ACCESS_TOKEN);
              UserContextInfo userContextInfo = TokenUtil.GetUserContextInfo(claimsPrincipal) ?? throw new Exception(ExceptionMessage.INVALID_ACCESS_TOKEN);

              IUserContext userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
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

    public static void AddCorsPolicies(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("public", policy =>
        {
          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
      });
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