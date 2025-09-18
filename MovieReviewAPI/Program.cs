using FluentValidation;
using MediatR;
using MovieReviewApi.Api.Middleware;
using MovieReviewApi.Application.Behaviors;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Validators.ActorValidator;
using MovieReviewApi.Infrastructure.Data;
using MovieReviewApi.Infrastructure.Extensions;
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

    builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

    builder.Services.AddValidatorsFromAssemblyContaining<CreateActorValidator>();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(IApplicationDbContext).Assembly));

    //access http context while using mediatr logging pipeline
    builder.Services.AddHttpContextAccessor();

    //register the IPipelineBehavior and it's implementation LoggingPipelineBehavior
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));




    //builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddControllers();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddInfrastructure(builder.Configuration);

    //builder.Services.AddControllers(options =>
    //{
    //    options.Filters.Add<ValidationFilter>();
    //});

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();


    app.UseAuthorization();

    app.MapControllers();

    app.Run();


public partial class Program { }