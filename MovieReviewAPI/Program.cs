using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using MovieReviewApi.Api.Filters;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Services;
using MovieReviewApi.Infrastructure.Extensions;
using MovieReviewApi.Infrastructure.Repositories;
using Serilog;
using MovieReviewApi.Application.Validators.ActorValidator;

try
{
    Log.Logger = new LoggerConfiguration()
      //.ReadFrom.Configuration(builder.Configuration)
      .MinimumLevel.Information()
      .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
      .WriteTo.File
      (
      path: "Logs/log.txt",
      rollingInterval: RollingInterval.Day
      )
      .CreateLogger();

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    // Add services to the container.

    builder.Services.AddValidatorsFromAssemblyContaining<CreateActorValidator>();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MovieReviewApi.Application.MediatRAssemblyMarker).Assembly));

    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<RequestResponseLoggingFilter>();
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddScoped<IActorRepository, ActorRepository>();
    builder.Services.AddScoped<IActorService, ActorService>();

    builder.Services.AddScoped<IMovieRepository, MovieRepository>();
    builder.Services.AddScoped<IMovieService, MovieService>();

    builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
    builder.Services.AddScoped<IReviewService, ReviewService>();

    //builder.Services.AddControllers(options =>
    //{
    //    options.Filters.Add<ValidationFilter>();
    //});

    var app = builder.Build();

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

}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
}
finally {
    Log.CloseAndFlush();
}