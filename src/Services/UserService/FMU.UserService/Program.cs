using FMU.EventBus.Abstractions;
using FMU.EventBus.Events;
using FMU.UserService.Application.Interfaces;
using FMU.UserService.EventHandlers;
using FMU.UserService.Infrastructure.Persistence.Context;
using FMU.UserService.Infrastructure.Persistence.Repositories;
using FMU.UserService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using FMU.EventBus.InMemory.Extensions;

var builder = WebApplication.CreateBuilder(args);

//ConfigureLogging(builder);

ConfigureServices(builder);

var app = builder.Build();

ConfigureEventBus(app);

ConfigureMiddleware(app);

MigrateDatabase(app);

app.Run();

// ========== HELPER METHODS ==========

//void ConfigureLogging(WebApplicationBuilder builder)
//{
//    Log.Logger = new LoggerConfiguration()
//        .ReadFrom.Configuration(builder.Configuration)
//        .Enrich.FromLogContext()
//        .WriteTo.Console()
//        .CreateLogger();

//    builder.Host.UseSerilog();
//}

void ConfigureServices(WebApplicationBuilder builder)
{
    var services = builder.Services;
    var configuration = builder.Configuration;

    // API Controllers
    services.AddControllers();

    // Swagger/OpenAPI
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FMU User Service API",
            Version = "v1",
            Description = "Football Manager Unity - User Service API",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "your.email@example.com"
            }
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        });
    });

    // Database
    services.AddDbContext<UserDbContext>(options =>
        options.UseNpgsql(
            configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName)));

    // Repositories
    services.AddScoped<IUserRepository, UserRepository>();

    // Services
    services.AddScoped<IUserService, UserService>();

    // AutoMapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Health Checks
    //services.AddHealthChecks()
    //    .AddDbContextCheck<UserDbContext>("database")
    //    .AddCheck<ApiHealthCheck>("api_health_check");

    // CORS
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    });

    // Register event handlers
    builder.Services.AddTransient<UserCreatedIntegrationEventHandler>();

    // Add EventBus
    builder.Services.AddInMemoryEventBus();

    // Authentication & Authorization
    //var jwtSettings = configuration.GetSection("JwtSettings");
    //var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

    //services.AddAuthentication(options =>
    //{
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //})
    //.AddJwtBearer(options =>
    //{
    //    options.RequireHttpsMetadata = false;
    //    options.SaveToken = true;
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(key),
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidIssuer = jwtSettings["Issuer"],
    //        ValidAudience = jwtSettings["Audience"],
    //        ClockSkew = TimeSpan.Zero
    //    };
    //});

    // Event Bus (RabbitMQ)
}

void ConfigureEventBus(WebApplication app)
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();

    // Subscribe to integration events
    eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();
}

void ConfigureMiddleware(WebApplication app)
{
    // Development-specific middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FMU User Service API v1"));
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Exception handling middleware for production
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Global exception handler middleware
    //app.UseMiddleware<ErrorHandlingMiddleware>();

    // Request/response logging
    //app.UseSerilogRequestLogging();

    // Default middleware
    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    // Health check endpoint
    //app.MapHealthChecks("/health");

    // Map controllers
    app.MapControllers();
}


void MigrateDatabase(WebApplication app)
{
    //using (var scope = app.Services.CreateScope())
    //{
    //    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    //    try
    //    {
    //        db.Database.Migrate();
    //        Log.Information("Database migrated successfully");
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex, "An error occurred while migrating the database");
    //    }
    //}
}

// Api Health Check class
public class ApiHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        if (isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("API is healthy"));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("API is unhealthy"));
    }
}