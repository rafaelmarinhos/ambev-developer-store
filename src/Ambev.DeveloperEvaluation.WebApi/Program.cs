using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.IoC.Extensions;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Information("Starting web application");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddDefaultLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.AddBasicHealthChecks();

builder.Services.AddDbContext<DefaultContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
    )
);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.RegisterDependencies();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseBasicHealthChecks();

app.MapControllers();

app.UseDefaultLogging();

app.Run();
public partial class Program { }