using AzgaraCRM.WebUI.Domain.Settings;
using AzgaraCRM.WebUI.ExceptionHandlers;
using AzgaraCRM.WebUI.Middlewares;
using AzgaraCRM.WebUI.Persistence.DataContexts;
using AzgaraCRM.WebUI.Persistence.Interceptors;
using AzgaraCRM.WebUI.Persistence.Repositories;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services;
using AzgaraCRM.WebUI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Telegram.WebUI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddWebApplicationBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.AddPresentation(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        return builder;
    }

    #region
    private static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDevTools();
        services.AddExposers();
        services.AddSwagger();
        services.AddMapper();
        services.AddSecurity(configuration);
        services.AddSignalR();
        services.AddFluentValidation();
        services.AddHelper();
        services.AddHttpContextAccessor();
        services.AddJsonConverter();
        services.AddServices();
        services.AddFluentValidation();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.WithOrigins("http://localhost:5173", "https://chat-app-jade-five.vercel.app") // ✅ Specify your Vue.js frontend origin
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    private static IServiceCollection AddDevTools(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection AddExposers(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<AlreadyExistsExceptionHandler>();
        services.AddExceptionHandler<ArgumentIsNotValidExceptionHandler>();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<InternalServerExceptionHandler>();

        services.AddControllers(options =>
            options.Conventions.Add(new RouteTokenTransformerConvention(new RouteTransformer())));

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on the textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() },
            });
        });

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    private static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new InvalidOperationException($"{nameof(JwtSettings)} is not configurated.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidateLifetime = jwtSettings.ValidateLifeTime,
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
            //options.Events = new JwtBearerEvents
            //{
            //    OnMessageReceived = context =>
            //    {
            //        var accessToken = context.Request.Query["access_token"];

            //        var path = context.HttpContext.Request.Path;
            //        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat-hub"))
            //        {
            //            context.Token = accessToken;
            //        }

            //        return Task.CompletedTask;
            //    }
            //};
        });

        return services;
    }

    private static IServiceCollection AddHelper(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDbConnection"));
            options.AddInterceptors(new AuditableInterceptor());
        });

        return services;
    }

    private static IServiceCollection AddJsonConverter(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFoodService, FoodService>();

        return services;
    }
    #endregion
}