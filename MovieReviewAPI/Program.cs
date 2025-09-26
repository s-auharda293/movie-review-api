using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using MovieReviewApi.Api.Middleware;
using MovieReviewApi.Application.Behaviors;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Interfaces.Hangfire;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Application.Validators.ActorValidator;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Data;
using MovieReviewApi.Infrastructure.Extensions;
using MovieReviewApi.Infrastructure.Jobs;
using MovieReviewApi.Infrastructure.Mapping;
using MovieReviewApi.Infrastructure.Services;
using MovieReviewApi.Infrastructure.Services.Identity;
using Serilog;

    Log.Logger = new LoggerConfiguration()
      //.ReadFrom.Configuration(builder.Configuration)
      .MinimumLevel.Information()
      .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
      .WriteTo.File
      (
      path: "Logs/log.txt",
      rollingInterval: RollingInterval.Day,
      formatProvider: System.Globalization.CultureInfo.CurrentCulture
      )
      .CreateLogger();

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddHangfire(configuration => configuration
     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
     .UseSimpleAssemblyNameTypeSerializer()
     .UseRecommendedSerializerSettings()
     .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateActorValidator>();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(IApplicationDbContext).Assembly));

    //access http context while using mediatr logging pipeline
    builder.Services.AddHttpContextAccessor();

    //register the IPipelineBehavior and it's implementation LoggingPipelineBehavior
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

    builder.Services.AddScoped<ILogFileCleaner, LogFileCleaner>();
    builder.Services.AddScoped<DeleteLogsJob>();



//builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddControllers();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

// Adding Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieReviewApi", Version = "v1" });

    //configuring swagger to use security
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token in the following format: {your token here} do not add the word 'Bearer' before it."
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
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
    });
});

// Adding Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<MovieReviewDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // Adding Jwt from extension method
    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJwt(builder.Configuration);
    builder.Services.ConfigureCors();


builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<ValidationFilter>();
//});

builder.Services.AddAutoMapper(cfg =>
{
    // configure your profiles here
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.MapHangfireDashboard();

    HangfireJobScheduler.ScheduleJobs();

    using (var scope = app.Services.CreateScope())
{
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await IdentityRoleSeeder.SeedRolesAsync(roleManager);
        await IdentityRoleSeeder.SeedAdminUserAsync(userManager,mediator);
    }


app.Run();


public partial class Program { }